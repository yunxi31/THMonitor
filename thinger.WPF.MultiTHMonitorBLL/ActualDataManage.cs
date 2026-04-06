using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.WPF.MultiTHMonitorDAL;
using thinger.WPF.MultiTHMonitorModels.SQL;

namespace thinger.WPF.MultiTHMonitorBLL
{
    public class ActualDataManage
    {
        private ActualDataService actualDataService = new ActualDataService();

        public int AddActualData(ActualData actualData)
        {
            return actualDataService.AddActualData(actualData);
        }

        public DataTable QueryActualDataByTime(string start, string end, List<string> columns)
        {
            return actualDataService.QueryActualDataByTime(start, end, columns);
        }

        public List<ActualData> QueryActualDataByTime(string start, string end)
        {
            return actualDataService.QueryActualDataByTime(start, end);
        }
    }
}
