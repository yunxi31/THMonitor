using System;
using System.Collections.Concurrent;
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
        private static readonly BlockingCollection<SysLog> _logQueue = new BlockingCollection<SysLog>();
        private SysLogService sysLogService = new SysLogService();

        static SysLogManage()
        {
            Task.Run(() => ProcessLogQueue());
        }

        private static void ProcessLogQueue()
        {
            var service = new SysLogService();
            foreach (var log in _logQueue.GetConsumingEnumerable())
            {
                try
                {
                    service.AddSysLog(log);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to write log to DB asynchronously: {ex.Message}");
                }
            }
        }

        public int AddSysLog(SysLog sysLog)
        {
            if (sysLog != null)
            {
                _logQueue.Add(sysLog);
            }
            return 1;
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
