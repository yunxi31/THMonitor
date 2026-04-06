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
    public class ActualDataService
    {
        /// <summary>
        /// 实时数据存储
        /// </summary>
        /// <param name="varNameList"></param>
        /// <param name="varValueList"></param>
        /// <returns></returns>
        public int AddActualData(ActualData actualData)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Insert into ActualData(InsertTime,Station1Temp,Station1Humidity,");

            stringBuilder.Append("Station2Temp,Station2Humidity,Station3Temp,Station3Humidity,");

            stringBuilder.Append("Station4Temp,Station4Humidity,Station5Temp,Station5Humidity,");

            stringBuilder.Append("Station6Temp,Station6Humidity) values(@InsertTime,@Station1Temp,@Station1Humidity,");

            stringBuilder.Append("@Station2Temp,@Station2Humidity,@Station3Temp,@Station3Humidity,");

            stringBuilder.Append("@Station4Temp,@Station4Humidity,@Station5Temp,@Station5Humidity,");

            stringBuilder.Append("@Station6Temp,@Station6Humidity)");

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@InsertTime",actualData.InsertTime),
                new SqlParameter("@Station1Temp",actualData.Station1Temp),
                new SqlParameter("@Station1Humidity",actualData.Station1Humidity),
                new SqlParameter("@Station2Temp",actualData.Station2Temp),
                new SqlParameter("@Station2Humidity",actualData.Station2Humidity),
                new SqlParameter("@Station3Temp",actualData.Station3Temp),
                new SqlParameter("@Station3Humidity",actualData.Station3Humidity),
                new SqlParameter("@Station4Temp",actualData.Station4Temp),
                new SqlParameter("@Station4Humidity",actualData.Station4Humidity),
                new SqlParameter("@Station5Temp",actualData.Station5Temp),
                new SqlParameter("@Station5Humidity",actualData.Station5Humidity),
                new SqlParameter("@Station6Temp",actualData.Station6Temp),
                new SqlParameter("@Station6Humidity",actualData.Station6Humidity),
            };

            return SQLHelper.ExecuteNonQuery(stringBuilder.ToString(), param);
        }


        /// <summary>
        /// 根据时间间隔和参数名称查询数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public DataTable QueryActualDataByTime(string start, string end, List<string> columns)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Select InsertTime,");

            stringBuilder.Append(string.Join(",", columns));

            stringBuilder.Append(" from ActualData where InsertTime between @Start and @End");

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Start",start),
                new SqlParameter("@End",end)
            };

            try
            {
                DataSet dataSet = SQLHelper.GetDataSet(stringBuilder.ToString(), param);

                if (dataSet.Tables.Count > 0)
                {
                    return dataSet.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ActualData> QueryActualDataByTime(string start, string end)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Select *");

            stringBuilder.Append(" from ActualData where InsertTime between @Start and @End");

            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Start",start),
                new SqlParameter("@End",end)
            };

            SqlDataReader reader = SQLHelper.ExecuteReader(stringBuilder.ToString(), param.ToArray());

            List<ActualData> actualDatas = new List<ActualData>();

            while (reader.Read())
            {
                actualDatas.Add(new ActualData()
                {
                    InsertTime = reader["InsertTime"].ToString(),
                    Station1Temp = reader["Station1Temp"].ToString(),
                    Station1Humidity = reader["Station1Humidity"].ToString(),
                    Station2Temp = reader["Station2Temp"].ToString(),
                    Station2Humidity = reader["Station2Humidity"].ToString(),
                     Station3Temp = reader["Station3Temp"].ToString(),
                    Station3Humidity = reader["Station3Humidity"].ToString(),
                    Station4Temp = reader["Station3Temp"].ToString(),
                    Station4Humidity = reader["Station3Humidity"].ToString(),
                    Station5Temp = reader["Station5Temp"].ToString(),
                    Station5Humidity = reader["Station5Humidity"].ToString(),
                    Station6Temp = reader["Station6Temp"].ToString(),
                    Station6Humidity = reader["Station6Humidity"].ToString(),
                });
            }

            return actualDatas;
        }

    }
}
