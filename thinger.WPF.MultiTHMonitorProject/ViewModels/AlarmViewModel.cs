using Microsoft.Win32;
using MiniExcelLibs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using thinger.DataConvertLib;
using thinger.WPF.MultiTHMonitorBLL;
using thinger.WPF.MultiTHMonitorModels.SQL;

namespace thinger.WPF.MultiTHMonitorProject.ViewModels
{
    public class AlarmViewModel:BindableBase
    {
        public AlarmViewModel()
        {
            QueryAlarmCommand = new DelegateCommand(ExeQueryAlarm);
            ExportCommand = new DelegateCommand<object>(ExeExport);
        }

       
        #region 成员变量
        public ObservableCollection<string> AlarmTypes { get; set; } = new ObservableCollection<string>
        {
           "全部","触发","消除"
        };
        #endregion

        #region 命令属性
        public DelegateCommand QueryAlarmCommand { get; set; }
        public DelegateCommand<object> ExportCommand { get; set; }
        #endregion
        #region 属性
        private DataTable operateResults;

        public DataTable OperateResults
        {
            get { return operateResults; }
            set { operateResults = value; RaisePropertyChanged(); }
        }
        private List<SysLog> sysLogs;

        public List<SysLog> SysLogs
        {
            get { return sysLogs; }
            set { sysLogs = value; RaisePropertyChanged(); }
        }

        private string selectAlarmType;

        public string SelectAlarmType
        {
            get { return selectAlarmType; }
            set { selectAlarmType = value; RaisePropertyChanged(); }
        }
        private DateTime startNowDate=DateTime.Now;

        public DateTime StartNowDate
        {
            get { return startNowDate; }
            set 
            {
                startNowDate=value; 
                RaisePropertyChanged(); 
            }
        }
        private DateTime startNowTime= DateTime.Now;

        public DateTime StartNowTime
        {
            get { return startNowTime; }
            set { startNowTime = value; RaisePropertyChanged(); }
        }
        private DateTime endNowDate = DateTime.Now;

        public DateTime EndNowDate
        {
            get { return endNowDate; }
            set { endNowDate = value; RaisePropertyChanged(); }
        }
        private DateTime endNowTime= DateTime.Now;

        public DateTime EndNowTime
        {
            get { return endNowTime; }
            set { endNowTime = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 方法
        private void ExeExport(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Xlsx文件(*.xlsx)|*.xlsx|所有文件|*.*";
            saveFileDialog.Title = "导出日志报表";
            saveFileDialog.FileName = "日志报表" + DateTime.Now.ToString("yyyyMMddHHmmss");
            saveFileDialog.DefaultExt = "xlsx";
            saveFileDialog.AddExtension = true;

            MiniExcel.SaveAs(saveFileDialog.FileName, obj,excelType:ExcelType.XLSX);
            Process.Start(new ProcessStartInfo {UseShellExecute=true,FileName=saveFileDialog.FileName });
        }
        private void ExeQueryAlarm()
        {
            string start = StartNowDate.ToString("yyyy-MM-dd")+StartNowTime.ToString(" hh:mm:ss");
            string end = EndNowDate.ToString("yyyy-MM-dd") + EndNowTime.ToString(" hh:mm:ss");

            string alarmType = SelectAlarmType == "全部" ? "" : SelectAlarmType;

            List<SysLog> operateResult= new SysLogManage().QuerySysLogByCondition(start, end, alarmType);
            SysLogs = operateResult;
        }
      

        /// <summary>    
        /// 实体转换辅助类 DataTable转换到List  
        /// 使用方式
        ///把DataTable转换为IList<UserInfo>  
        ///IList<UserInfo> users = ModelConvertHelper<UserInfo>.ConvertToModel(dt);
        /// </summary>    
        public List<SysLog> ConvertToModel(DataTable dt)
        {
            // 定义集合    
            List<SysLog> ts = new List<SysLog>();

            // 获得此模型的类型   
            Type type = typeof(SysLog);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                SysLog t = new SysLog();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列   
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
        #endregion
    }
}
