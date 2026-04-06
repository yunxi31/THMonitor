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
    public class SysAdminService
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="sysAdmin"></param>
        /// <returns></returns>
        public SysAdmin AdminLogin(SysAdmin sysAdmin)
        {
            string sql = "Select LoginId,ParamSet,Recipe,HistoryLog,HistoryTrend,UserManage " +

                 "from SysAdmin where LoginName=@LoginName and LoginPwd=@LoginPwd";

            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@LoginName",sysAdmin.LoginName),
                new SqlParameter("@LoginPwd",sysAdmin.LoginPwd),
            };

            DataSet dataSet = SQLHelper.GetDataSet(sql, parameter);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count == 1)
            {
                sysAdmin.LoginId = Convert.ToInt32(dataSet.Tables[0].Rows[0]["LoginId"]);
                sysAdmin.ParamSet = Convert.ToBoolean(dataSet.Tables[0].Rows[0]["ParamSet"]);
                sysAdmin.Recipe = Convert.ToBoolean(dataSet.Tables[0].Rows[0]["Recipe"]);
                sysAdmin.HistoryLog = Convert.ToBoolean(dataSet.Tables[0].Rows[0]["HistoryLog"]);
                sysAdmin.HistoryTrend = Convert.ToBoolean(dataSet.Tables[0].Rows[0]["HistoryTrend"]);
                sysAdmin.UserManage = Convert.ToBoolean(dataSet.Tables[0].Rows[0]["UserManage"]);

                return sysAdmin;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="sysAdmin"></param>
        /// <returns></returns>
        public int AddSysAdmin(SysAdmin sysAdmin)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Insert into SysAdmin(LoginName,LoginPwd,ParamSet,Recipe,HistoryLog,HistoryTrend,UserManage)");

            stringBuilder.Append(" values(@LoginName,@LoginPwd,@ParamSet,@Recipe,@HistoryLog,@HistoryTrend,@UserManage)");

            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@LoginName",sysAdmin.LoginName),
                new SqlParameter("@LoginPwd",sysAdmin.LoginPwd),
                new SqlParameter("@ParamSet",sysAdmin.ParamSet),
                new SqlParameter("@Recipe",sysAdmin.Recipe),
                new SqlParameter("@HistoryLog",sysAdmin.HistoryLog),
                new SqlParameter("@HistoryTrend",sysAdmin.HistoryTrend),
                new SqlParameter("@UserManage",sysAdmin.UserManage),
            };

            return SQLHelper.ExecuteNonQuery(stringBuilder.ToString(), parameter);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public int DeleteSysAdmin(int loginId)
        {
            string sql = "Delete from SysAdmin where LoginId=@LoginId";

            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@LoginId",loginId),
            };

            return SQLHelper.ExecuteNonQuery(sql, parameter);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="sysAdmin"></param>
        /// <returns></returns>
        public int ModifySysAdmin(SysAdmin sysAdmin)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Update SysAdmin set LoginName=@LoginName,LoginPwd=@LoginPwd,");

            stringBuilder.Append("ParamSet=@ParamSet,Recipe=@Recipe,");

            stringBuilder.Append("HistoryLog=@HistoryLog,HistoryTrend=@HistoryTrend,");

            stringBuilder.Append("UserManage=@UserManage where LoginId=@LoginId");

            SqlParameter[] parameter = new SqlParameter[]
            {
                new SqlParameter("@LoginId",sysAdmin.LoginId),
                new SqlParameter("@LoginName",sysAdmin.LoginName),
                new SqlParameter("@LoginPwd",sysAdmin.LoginPwd),
                new SqlParameter("@ParamSet",sysAdmin.ParamSet),
                new SqlParameter("@Recipe",sysAdmin.Recipe),
                new SqlParameter("@HistoryLog",sysAdmin.HistoryLog),
                new SqlParameter("@HistoryTrend",sysAdmin.HistoryTrend),
                new SqlParameter("@UserManage",sysAdmin.UserManage),
            };

            return SQLHelper.ExecuteNonQuery(stringBuilder.ToString(), parameter);
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>
        public List<SysAdmin> QuerySysAdmins()
        {
            string sql = "Select LoginId,LoginName,LoginPwd,ParamSet,Recipe,HistoryLog,HistoryTrend,UserManage from SysAdmin";

            SqlDataReader reader = SQLHelper.ExecuteReader(sql);

            List<SysAdmin> sysAdmins = new List<SysAdmin>();

            while (reader.Read())
            {
                sysAdmins.Add(new SysAdmin()
                {
                    LoginId = Convert.ToInt32(reader["LoginId"]),
                    LoginName = reader["LoginName"].ToString(),
                    LoginPwd = reader["LoginPwd"].ToString(),
                    ParamSet = Convert.ToBoolean(reader["ParamSet"]),
                    Recipe = Convert.ToBoolean(reader["Recipe"]),
                    HistoryLog = Convert.ToBoolean(reader["HistoryLog"]),
                    HistoryTrend = Convert.ToBoolean(reader["HistoryTrend"]),
                    UserManage = Convert.ToBoolean(reader["UserManage"])
                });
            }

            return sysAdmins;
        }
    }
}
