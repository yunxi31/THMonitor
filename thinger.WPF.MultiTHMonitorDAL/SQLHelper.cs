using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thinger.WPF.MultiTHMonitorDAL
{
    public class SQLHelper
    {
        //读取配置文件获得连接字符串
        private static string connString = ConfigurationManager.ConnectionStrings["connString"].ToString();

        /// <summary>
        /// 检查是否为数据库瞬态/临时性异常（如死锁、超时、网络丢包等），这类异常适合重试
        /// </summary>
        private static bool IsTransient(SqlException ex)
        {
            foreach (SqlError error in ex.Errors)
            {
                switch (error.Number)
                {
                    case 1205:  // 事务死锁牺牲品 (Deadlock)
                    case -2:    // 连接或查询超时 (Timeout)
                    case 40613: // 数据库临时不可用 (Database transiently unavailable)
                    case 40501: // 引擎忙 (Database engine busy)
                    case 40197: // 服务发生故障正在切换 (Error processing request)
                    case 10053: // 传输层错误/连接断开 (Transport-level error)
                    case 10054: // 传输层错误/连接被强行关闭
                    case 10060: // 网络连接超时 (Connection timeout)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 深度克隆 SqlParameter，防止在重试时出现 "SqlParameter 已包含在另一个 Collection 中" 的异常
        /// </summary>
        private static SqlParameter[] CloneParameters(SqlParameter[] paramArray)
        {
            if (paramArray == null) return null;
            SqlParameter[] cloned = new SqlParameter[paramArray.Length];
            for (int i = 0; i < paramArray.Length; i++)
            {
                cloned[i] = new SqlParameter
                {
                    ParameterName = paramArray[i].ParameterName,
                    Value = paramArray[i].Value ?? DBNull.Value,
                    SqlDbType = paramArray[i].SqlDbType,
                    Size = paramArray[i].Size,
                    Direction = paramArray[i].Direction,
                    Precision = paramArray[i].Precision,
                    Scale = paramArray[i].Scale,
                    SourceColumn = paramArray[i].SourceColumn,
                    SourceVersion = paramArray[i].SourceVersion,
                    IsNullable = paramArray[i].IsNullable
                };
            }
            return cloned;
        }

        #region 同步方法 (Synchronous Methods)

        /// <summary>
        /// 通用的高频请求重试执行封装
        /// </summary>
        private static T ExecuteWithRetry<T>(string cmdText, SqlParameter[] paramArray, Func<SqlCommand, T> action)
        {
            const int maxRetries = 3;
            const int delayMs = 150;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                    {
                        if (paramArray != null)
                        {
                            cmd.Parameters.AddRange(CloneParameters(paramArray));
                        }
                        conn.Open();
                        return action(cmd);
                    }
                }
                catch (SqlException ex) when (attempt < maxRetries && IsTransient(ex))
                {
                    System.Diagnostics.Trace.WriteLine($"SQL transient error {ex.Number} on attempt {attempt}. Retrying in {delayMs * attempt}ms...");
                    System.Threading.Thread.Sleep(delayMs * attempt); // 递增避让延迟
                }
                catch (SqlException ex)
                {
                    throw new Exception($"执行 SQL 发生数据库异常，SQL: {cmdText}", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception($"执行 SQL 发生未知异常，SQL: {cmdText}", ex);
                }
            }
            throw new Exception("数据库操作在重试后依然失败。");
        }

        /// <summary>
        /// 执行insert、update、delete类型的SQL语句
        /// </summary>
        public static int ExecuteNonQuery(string cmdText, SqlParameter[] paramArray = null)
        {
            return ExecuteWithRetry(cmdText, paramArray, (cmd) => cmd.ExecuteNonQuery());
        }

        /// <summary>
        /// 返回单一结果的查询
        /// </summary>
        public static object ExecuteScalar(string cmdText, SqlParameter[] paramArray = null)
        {
            return ExecuteWithRetry(cmdText, paramArray, (cmd) => cmd.ExecuteScalar());
        }

        /// <summary>
        /// 执行返回一个只读结果集的查询
        /// </summary>
        public static SqlDataReader ExecuteReader(string cmdText, SqlParameter[] paramArray = null)
        {
            const int maxRetries = 3;
            const int delayMs = 150;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                SqlConnection conn = null;
                SqlCommand cmd = null;
                try
                {
                    conn = new SqlConnection(connString);
                    cmd = new SqlCommand(cmdText, conn);
                    if (paramArray != null)
                    {
                        cmd.Parameters.AddRange(CloneParameters(paramArray));
                    }
                    conn.Open();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (SqlException ex) when (attempt < maxRetries && IsTransient(ex))
                {
                    cmd?.Dispose();
                    conn?.Dispose();
                    System.Diagnostics.Trace.WriteLine($"SQL Reader transient error {ex.Number} on attempt {attempt}. Retrying in {delayMs * attempt}ms...");
                    System.Threading.Thread.Sleep(delayMs * attempt);
                }
                catch (SqlException ex)
                {
                    cmd?.Dispose();
                    conn?.Dispose();
                    throw new Exception($"执行 SqlDataReader 发生数据库异常，SQL: {cmdText}", ex);
                }
                catch (Exception ex)
                {
                    cmd?.Dispose();
                    conn?.Dispose();
                    throw new Exception($"执行 SqlDataReader 发生未知异常，SQL: {cmdText}", ex);
                }
            }
            throw new Exception("SqlDataReader 重试后依然失败。");
        }

        /// <summary>
        /// 返回包含一张数据表的数据集的查询
        /// </summary>
        public static DataSet GetDataSet(string sql, string tableName = null)
        {
            return GetDataSet(sql, null, tableName);
        }

        /// <summary>
        /// 返回包含一张数据表的数据集的查询
        /// </summary>
        public static DataSet GetDataSet(string sql, SqlParameter[] paramArray = null, string tableName = null)
        {
            return ExecuteWithRetry(sql, paramArray, (cmd) =>
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    if (tableName == null)
                        da.Fill(ds);
                    else
                        da.Fill(ds, tableName);
                    return ds;
                }
            });
        }

        /// <summary>
        /// 执行查询，返回一个或多个表的DataSet
        /// </summary>
        public static DataSet GetDataSet(Dictionary<string, string> dicTableAndSql)
        {
            const int maxRetries = 3;
            const int delayMs = 150;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            conn.Open();
                            foreach (string tbName in dicTableAndSql.Keys)
                            {
                                cmd.CommandText = dicTableAndSql[tbName];
                                da.Fill(ds, tbName);
                            }
                            return ds;
                        }
                    }
                }
                catch (SqlException ex) when (attempt < maxRetries && IsTransient(ex))
                {
                    System.Diagnostics.Trace.WriteLine($"SQL GetDataSet transient error {ex.Number} on attempt {attempt}. Retrying in {delayMs * attempt}ms...");
                    System.Threading.Thread.Sleep(delayMs * attempt);
                }
                catch (SqlException ex)
                {
                    throw new Exception("执行 GetDataSet(Dictionary) 发生数据库异常", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("执行 GetDataSet(Dictionary) 发生未知异常", ex);
                }
            }
            throw new Exception("GetDataSet(Dictionary) 重试后依然失败。");
        }

        /// <summary>
        /// 基于事务提交
        /// </summary>
        public static bool ExecuteNonQueryByTran(string sql, List<SqlParameter[]> paramArrayList)
        {
            const int maxRetries = 3;
            const int delayMs = 150;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                SqlConnection conn = null;
                SqlTransaction transaction = null;
                try
                {
                    conn = new SqlConnection(connString);
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand(sql, conn, transaction))
                    {
                        foreach (SqlParameter[] param in paramArrayList)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(CloneParameters(param));
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (SqlException ex) when (attempt < maxRetries && IsTransient(ex))
                {
                    try { transaction?.Rollback(); } catch { }
                    transaction?.Dispose();
                    conn?.Dispose();

                    System.Diagnostics.Trace.WriteLine($"SQL Transaction transient error {ex.Number} on attempt {attempt}. Retrying in {delayMs * attempt}ms...");
                    System.Threading.Thread.Sleep(delayMs * attempt);
                }
                catch (SqlException ex)
                {
                    try { transaction?.Rollback(); } catch { }
                    transaction?.Dispose();
                    conn?.Dispose();
                    throw new Exception("ExecuteNonQueryByTran 发生数据库事务异常", ex);
                }
                catch (Exception ex)
                {
                    try { transaction?.Rollback(); } catch { }
                    transaction?.Dispose();
                    conn?.Dispose();
                    throw new Exception("ExecuteNonQueryByTran 发生未知事务异常", ex);
                }
                finally
                {
                    transaction?.Dispose();
                    conn?.Dispose();
                }
            }
            throw new Exception("ExecuteNonQueryByTran 重试后依然失败。");
        }

        #endregion

        #region 异步方法 (Asynchronous Methods)

        /// <summary>
        /// 通用的高频请求异步重试执行封装
        /// </summary>
        private static async Task<T> ExecuteWithRetryAsync<T>(string cmdText, SqlParameter[] paramArray, Func<SqlCommand, Task<T>> action)
        {
            const int maxRetries = 3;
            const int delayMs = 150;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                    {
                        if (paramArray != null)
                        {
                            cmd.Parameters.AddRange(CloneParameters(paramArray));
                        }
                        await conn.OpenAsync();
                        return await action(cmd);
                    }
                }
                catch (SqlException ex) when (attempt < maxRetries && IsTransient(ex))
                {
                    System.Diagnostics.Trace.WriteLine($"SQL async transient error {ex.Number} on attempt {attempt}. Retrying in {delayMs * attempt}ms...");
                    await Task.Delay(delayMs * attempt); // 非阻塞式异步延迟
                }
                catch (SqlException ex)
                {
                    throw new Exception($"执行 SQL 发生数据库异常(Async)，SQL: {cmdText}", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception($"执行 SQL 发生未知异常(Async)，SQL: {cmdText}", ex);
                }
            }
            throw new Exception("数据库操作在重试后依然失败(Async)。");
        }

        /// <summary>
        /// 异步执行insert、update、delete类型的SQL语句
        /// </summary>
        public static async Task<int> ExecuteNonQueryAsync(string cmdText, SqlParameter[] paramArray = null)
        {
            return await ExecuteWithRetryAsync(cmdText, paramArray, async (cmd) => await cmd.ExecuteNonQueryAsync());
        }

        /// <summary>
        /// 异步返回单一结果的查询
        /// </summary>
        public static async Task<object> ExecuteScalarAsync(string cmdText, SqlParameter[] paramArray = null)
        {
            return await ExecuteWithRetryAsync(cmdText, paramArray, async (cmd) => await cmd.ExecuteScalarAsync());
        }

        /// <summary>
        /// 异步执行返回一个只读结果集的查询
        /// </summary>
        public static async Task<SqlDataReader> ExecuteReaderAsync(string cmdText, SqlParameter[] paramArray = null)
        {
            const int maxRetries = 3;
            const int delayMs = 150;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                SqlConnection conn = null;
                SqlCommand cmd = null;
                try
                {
                    conn = new SqlConnection(connString);
                    cmd = new SqlCommand(cmdText, conn);
                    if (paramArray != null)
                    {
                        cmd.Parameters.AddRange(CloneParameters(paramArray));
                    }
                    await conn.OpenAsync();
                    return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                }
                catch (SqlException ex) when (attempt < maxRetries && IsTransient(ex))
                {
                    cmd?.Dispose();
                    conn?.Dispose();
                    System.Diagnostics.Trace.WriteLine($"SQL Reader async transient error {ex.Number} on attempt {attempt}. Retrying in {delayMs * attempt}ms...");
                    await Task.Delay(delayMs * attempt);
                }
                catch (SqlException ex)
                {
                    cmd?.Dispose();
                    conn?.Dispose();
                    throw new Exception($"执行 SqlDataReaderAsync 发生数据库异常，SQL: {cmdText}", ex);
                }
                catch (Exception ex)
                {
                    cmd?.Dispose();
                    conn?.Dispose();
                    throw new Exception($"执行 SqlDataReaderAsync 发生未知异常，SQL: {cmdText}", ex);
                }
            }
            throw new Exception("SqlDataReaderAsync 重试后依然失败。");
        }

        /// <summary>
        /// 异步返回包含一张数据表的数据集的查询
        /// </summary>
        public static async Task<DataSet> GetDataSetAsync(string sql, string tableName = null)
        {
            return await GetDataSetAsync(sql, null, tableName);
        }

        /// <summary>
        /// 异步返回包含一张数据表的数据集的查询
        /// </summary>
        public static async Task<DataSet> GetDataSetAsync(string sql, SqlParameter[] paramArray = null, string tableName = null)
        {
            return await ExecuteWithRetryAsync(sql, paramArray, async (cmd) =>
            {
                return await Task.Run(() =>
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        if (tableName == null)
                            da.Fill(ds);
                        else
                            da.Fill(ds, tableName);
                        return ds;
                    }
                });
            });
        }

        /// <summary>
        /// 异步执行查询，返回一个或多个表的DataSet
        /// </summary>
        public static async Task<DataSet> GetDataSetAsync(Dictionary<string, string> dicTableAndSql)
        {
            const int maxRetries = 3;
            const int delayMs = 150;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    return await Task.Run(async () =>
                    {
                        using (SqlConnection conn = new SqlConnection(connString))
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = conn;
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataSet ds = new DataSet();
                                await conn.OpenAsync();
                                foreach (string tbName in dicTableAndSql.Keys)
                                {
                                    cmd.CommandText = dicTableAndSql[tbName];
                                    da.Fill(ds, tbName);
                                }
                                return ds;
                            }
                        }
                    });
                }
                catch (SqlException ex) when (attempt < maxRetries && IsTransient(ex))
                {
                    System.Diagnostics.Trace.WriteLine($"SQL GetDataSet async transient error {ex.Number} on attempt {attempt}. Retrying in {delayMs * attempt}ms...");
                    await Task.Delay(delayMs * attempt);
                }
                catch (SqlException ex)
                {
                    throw new Exception("执行 GetDataSetAsync(Dictionary) 发生数据库异常", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("执行 GetDataSetAsync(Dictionary) 发生未知异常", ex);
                }
            }
            throw new Exception("GetDataSetAsync(Dictionary) 重试后依然失败。");
        }

        /// <summary>
        /// 异步基于事务提交
        /// </summary>
        public static async Task<bool> ExecuteNonQueryByTranAsync(string sql, List<SqlParameter[]> paramArrayList)
        {
            const int maxRetries = 3;
            const int delayMs = 150;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                SqlConnection conn = null;
                SqlTransaction transaction = null;
                try
                {
                    conn = new SqlConnection(connString);
                    await conn.OpenAsync();
                    transaction = conn.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand(sql, conn, transaction))
                    {
                        foreach (SqlParameter[] param in paramArrayList)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(CloneParameters(param));
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (SqlException ex) when (attempt < maxRetries && IsTransient(ex))
                {
                    try { transaction?.Rollback(); } catch { }
                    transaction?.Dispose();
                    conn?.Dispose();

                    System.Diagnostics.Trace.WriteLine($"SQL Transaction async transient error {ex.Number} on attempt {attempt}. Retrying in {delayMs * attempt}ms...");
                    await Task.Delay(delayMs * attempt);
                }
                catch (SqlException ex)
                {
                    try { transaction?.Rollback(); } catch { }
                    transaction?.Dispose();
                    conn?.Dispose();
                    throw new Exception("ExecuteNonQueryByTranAsync 发生数据库事务异常", ex);
                }
                catch (Exception ex)
                {
                    try { transaction?.Rollback(); } catch { }
                    transaction?.Dispose();
                    conn?.Dispose();
                    throw new Exception("ExecuteNonQueryByTranAsync 发生未知事务异常", ex);
                }
                finally
                {
                    transaction?.Dispose();
                    conn?.Dispose();
                }
            }
            throw new Exception("ExecuteNonQueryByTranAsync 重试后依然失败。");
        }

        #endregion
    }
}
