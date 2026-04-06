using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.WPF.MultiTHMonitorModels.SQL;

namespace thinger.WPF.MultiTHMonitorDAL
{
    public class SysLogService
    {
        /// <summary>
        /// 添加一条系统日志
        /// </summary>
        /// <param name="sysLog"></param>
        /// <returns></returns>
        public int AddSysLog(SysLog sysLog)
        {
            string sql = "Insert into SysLog(InsertTime,Note,Operator,VarName,AlarmType) " +

                    "values(@InsertTime,@Note,@Operator,@VarName,@AlarmType)";

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@InsertTime",sysLog.InsertTime),
                new SqlParameter("@Note",sysLog.Note),
                new SqlParameter("@Operator",sysLog.Operator),
                new SqlParameter("@VarName",sysLog.VarName),
                new SqlParameter("@AlarmType",sysLog.AlarmType),
            };

            return SQLHelper.ExecuteNonQuery(sql, param);
        }

        /// <summary>
        /// 根据时间和日志类型进行查询
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="alarmType"></param>
        /// <returns></returns>
        //public DataTable QuerySysLogByCondition(string start, string end, string alarmType)
        //{
        //    string sql = "Select InsertTime,Note,Operator,VarName,AlarmType "

        //        + "from SysLog where InsertTime between @Start and @End";

        //    List<SqlParameter> param = new List<SqlParameter>()
        //    {
        //        new SqlParameter("@Start",start),
        //        new SqlParameter("@End",end),
        //    };

        //    if (alarmType!=null)
        //    {
        //        sql += " and AlarmType=@AlarmType";
        //        param.Add(new SqlParameter("@AlarmType", alarmType));
        //    }

        //    DataSet dataSet = SQLHelper.GetDataSet(sql, param.ToArray());

        //    if (dataSet != null && dataSet.Tables.Count > 0)
        //    {
        //        return dataSet.Tables[0];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="alarmType"></param>
        /// <returns></returns>
        public List<SysLog> QuerySysLogByCondition(string start, string end, string alarmType)
        {
           
            string sql = "Select InsertTime,Note,Operator,VarName,AlarmType "

                + "from SysLog where InsertTime between @Start and @End";

            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@Start",start),
                new SqlParameter("@End",end),
            };

            if (alarmType.Length>0)
            {
                sql += " and AlarmType=@AlarmType";
                param.Add(new SqlParameter("@AlarmType", alarmType));
            }

            SqlDataReader reader = SQLHelper.ExecuteReader(sql,param.ToArray());

            List<SysLog> sysLogs = new List<SysLog>();

            while (reader.Read())
            {
                sysLogs.Add(new SysLog()
                {
                    InsertTime = reader["InsertTime"].ToString(),
                    Note = reader["Note"].ToString(),
                    Operator = reader["Operator"].ToString(),
                    VarName = reader["VarName"].ToString(),
                    AlarmType = reader["AlarmType"].ToString()
                });
            }

            return sysLogs;
            
        }
    }
}
