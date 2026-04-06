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
    public class SysLogManage
    {
        private SysLogService sysLogService = new SysLogService();

        public int AddSysLog(SysLog sysLog)
        {
            return sysLogService.AddSysLog(sysLog);
        }

        //public DataTable QuerySysLogByCondition(string start, string end, string alarmType)
        //{
        //    return sysLogService.QuerySysLogByCondition(start, end, alarmType);
        //}
        public List<SysLog> QuerySysLogByCondition(string start, string end, string alarmType)
        {
            return sysLogService.QuerySysLogByCondition(start, end, alarmType);
        }
    }
}
