using MiniExcelLibs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using thinger.DataConvertLib;
using thinger.WPF.MultiTHMonitorBLL;
using thinger.WPF.MultiTHMonitorHelper;
using thinger.WPF.MultiTHMonitorModels;
using thinger.WPF.MultiTHMonitorModels.Config;
using thinger.WPF.MultiTHMonitorModels.SQL;
using thinger.WPF.MultiTHMonitorProject.Command;

namespace thinger.WPF.MultiTHMonitorProject.ViewModels
{
    public class MainViewModel : BindableBase, IConfigureService
    {
        public MainViewModel() { }
        //通过构造函数注入服务依赖
        public MainViewModel(IRegionManager regionManager)
        {
            OpenViewCommand = new DelegateCommand<string>(OpenView);
            this._regionManager = regionManager;
            LoadInfo();
            _timer.Tick+= StoreTimer_Elapsed;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
            
        }
        
        #region 成员变量
        private string _title = "Prism Application";
        private readonly IRegionManager _regionManager;
        private DispatcherTimer _timer = new DispatcherTimer();
        //private CommonDataMethods dataMethods = new CommonDataMethods();
        public List<OperateLog> AddLog=new List<OperateLog>();
        //设备参数路径
        private string devicePath = Environment.CurrentDirectory + "\\Config\\Device.ini";

        //通信组参数路径
        private string groupPath = Environment.CurrentDirectory + "\\Config\\Group.xlsx";

        //变量路径
        private string variablePath = Environment.CurrentDirectory + "\\Config\\Variable.xlsx";

        //大小端
        private DataFormat dataFormat = DataFormat.ABCD;
        //取消线程源
        private CancellationTokenSource cts;
        #endregion

        #region 命令属性
        public DelegateCommand<string> OpenViewCommand { get; private set; }
        //public DelegateCommand ConfirmSetCommand { get; set; }
        #endregion

        #region 视图属性
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string iPAddress;

        public string IPAddress
        {
            get { return iPAddress; }
            set { iPAddress = value; RaisePropertyChanged(); }
        }
        private int port;
        public int Port
        {
            get { return port; }
            set { port = value; RaisePropertyChanged(); }
        }
        private string CurrentTime
        {
            get { return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 定时存储
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StoreTimer_Elapsed(object sender, EventArgs e)
        {
            //定时存储
            if (CommonMethods.Device.IsConnected)
            {
                if (CommonMethods.Device["模块1温度"] != null)
                {
                    new ActualDataManage().AddActualData(new ActualData()
                    {
                        InsertTime = CurrentTime,
                        Station1Temp = CommonMethods.Device.CurrentValue["模块1温度"]?.ToString(),
                        Station1Humidity = CommonMethods.Device.CurrentValue["模块1湿度"]?.ToString(),
                        Station2Temp = CommonMethods.Device.CurrentValue["模块2温度"]?.ToString(),
                        Station2Humidity = CommonMethods.Device.CurrentValue["模块2湿度"]?.ToString(),
                        Station3Temp = CommonMethods.Device.CurrentValue["模块3温度"]?.ToString(),
                        Station3Humidity = CommonMethods.Device.CurrentValue["模块3湿度"]?.ToString(),
                        Station4Temp = CommonMethods.Device.CurrentValue["模块4温度"]?.ToString(),
                        Station4Humidity = CommonMethods.Device.CurrentValue["模块4湿度"]?.ToString(),
                        Station5Temp = CommonMethods.Device.CurrentValue["模块5温度"]?.ToString(),
                        Station5Humidity = CommonMethods.Device.CurrentValue["模块5湿度"]?.ToString(),
                        Station6Temp = CommonMethods.Device.CurrentValue["模块6温度"]?.ToString(),
                        Station6Humidity = CommonMethods.Device.CurrentValue["模块6湿度"]?.ToString(),
                    });
                }
            }
        }

         private void OpenView(string obj)
        {
            //通过区域去设置需要显示的内容
            _regionManager.Regions["ContentRegion"].RequestNavigate(obj);
        }

        /// <summary>
        /// 在主窗体中加载配置信息
        /// </summary>
        private void LoadInfo()
        {
            CommonMethods.Device = LoadDevice(devicePath);
            if (CommonMethods.Device != null)
            {
                AddLog.Add(new OperateLog {LogIcon= "AlertCircleCheckOutline",IconColor= "#a0e254", OperateTime =CurrentTime,OperateInfo="配置信息加载成功"});
                cts = new CancellationTokenSource();
                //CommonMethods.Device.AlarmTrigEvent += Device_AlarmTrigEvent;

                Task.Run(() =>
                {
                    PLCCommunication(CommonMethods.Device);
                }, cts.Token);
            }
        }

        /// <summary>
        /// 加载配置信息的方法
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private Device LoadDevice(string path)
        {
            if (!File.Exists(path))
            {
                AddLog.Add(new OperateLog { LogIcon = "AlertCircle",IconColor="Red", OperateTime = CurrentTime, OperateInfo = "设备文件不存在" });
                return null;
            }

            List<Group> GroupList = LoadGroup(groupPath, variablePath);

            if (GroupList != null && GroupList.Count > 0)
            {
                try
                {
                    return new Device()
                    {
                        IPAddress = IniConfigHelper.ReadIniData("设备参数", "IP地址", "127.0.0.1", path),
                        Port = Convert.ToInt32(IniConfigHelper.ReadIniData("设备参数", "端口号", "502", path)),
                        CurrentRecipe = IniConfigHelper.ReadIniData("设备参数", "当前配方", "", path),
                        GroupList = GroupList
                    };
                }
                catch (Exception ex)
                {
                    AddLog.Add(new OperateLog {LogIcon= "ArchiveArrowDownOutline",IconColor="Red",OperateTime = CurrentTime, OperateInfo = "设备信息加载失败:"+ex.Message });
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 加载通信组信息
        /// </summary>
        /// <param name="grouppath"></param>
        /// <param name="variablepath"></param>
        /// <returns></returns>
        private List<Group> LoadGroup(string grouppath, string variablepath)
        {
            if (!File.Exists(grouppath))
            {
                AddLog.Add(new OperateLog {LogIcon= "ArrowDownBoldCircleOutline",IconColor="Yellow", OperateTime = CurrentTime, OperateInfo = "通信组文件不存在" });
                return null;
            }

            if (!File.Exists(variablepath))
            {
                AddLog.Add(new OperateLog { LogIcon = "ArrowDownBoldCircleOutline", IconColor = "Yellow",OperateTime = CurrentTime, OperateInfo = "通信变量文件不存在" });
                return null;
            }

            List<Group> GpList = null;

            try
            {
                GpList = MiniExcel.Query<Group>(grouppath).ToList();
            }
            catch (Exception ex)
            {
                AddLog.Add(new OperateLog { LogIcon = "ArrowDownBoldCircleOutline", IconColor = "Yellow", OperateTime = CurrentTime, OperateInfo = "通信组加载失败:" + ex.Message });
                return null;
            }
            List<Variable> VarList = null;
            try
            {
                VarList = MiniExcel.Query<Variable>(variablepath).ToList();
            }
            catch (Exception ex)
            {
                AddLog.Add(new OperateLog { LogIcon = "ArrowDownBoldCircleOutline", IconColor = "Yellow",OperateTime = CurrentTime, OperateInfo = "通信变量加载失败：" + ex.Message });
                return null;
            }


            if (GpList != null && VarList != null)
            {
                foreach (var group in GpList)
                {
                    group.VarList = VarList.FindAll(c => c.GroupName == group.GroupName).ToList();
                }
                return GpList;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// PLC通信连接方法
        /// </summary>
        /// <param name="device"></param>
        private void PLCCommunication(Device device)
        {
            while (!cts.IsCancellationRequested)
            {
                if (device.IsConnected)
                {
                    foreach (var gp in device.GroupList)
                    {
                        byte[] data = null;
                        int reqLength = 0;
                        if (gp.StoreArea == "输入线圈" || gp.StoreArea == "输出线圈")
                        {
                            switch (gp.StoreArea)
                            {
                                case "输入线圈":
                                    data = CommonMethods.Modbus.ReadInputCoils(gp.Start, gp.Length);
                                    reqLength = ShortLib.GetByteLengthFromBoolLength(gp.Length);
                                    break;
                                case "输出线圈":
                                    data = CommonMethods.Modbus.ReadOutputCoils(gp.Start, gp.Length);
                                    reqLength = ShortLib.GetByteLengthFromBoolLength(gp.Length);
                                    break;
                                default:
                                    break;
                            }

                            if (data != null && data.Length == reqLength)
                            {
                                foreach (var variable in gp.VarList)
                                {
                                    int start = variable.Start - gp.Start;

                                    DataType dataType = (DataType)Enum.Parse(typeof(DataType), variable.DataType, true);

                                    switch (dataType)
                                    {
                                        case DataType.Bool:
                                            variable.VarValue = BitLib.GetBitFromByteArray(data, start, variable.OffsetOrLength);
                                            break;
                                        default:
                                            break;
                                    }

                                    device.UpdateVariable(variable);
                                }
                            }
                            else
                            {
                                device.IsConnected = false;
                            }
                        }
                        else
                        {
                            switch (gp.StoreArea)
                            {
                                case "输入寄存器":
                                    data = CommonMethods.Modbus.ReadInputRegisters(gp.Start, gp.Length);
                                    reqLength = gp.Length * 2;
                                    break;
                                case "输出寄存器":
                                    data = CommonMethods.Modbus.ReadOutputRegisters(gp.Start, gp.Length);
                                    reqLength = gp.Length * 2;
                                    break;
                                default:
                                    break;
                            }
                            if (data != null && data.Length == reqLength)
                            {
                                foreach (var variable in gp.VarList)
                                {
                                    int start = variable.Start - gp.Start;

                                    start *= 2;

                                    DataType dataType = (DataType)Enum.Parse(typeof(DataType), variable.DataType, true);

                                    switch (dataType)
                                    {
                                        case DataType.Bool:
                                            variable.VarValue = BitLib.GetBitFrom2BytesArray(data, start, variable.OffsetOrLength, dataFormat == DataFormat.BADC || dataFormat == DataFormat.DCBA);
                                            break;
                                        case DataType.Byte:
                                            variable.VarValue = ByteLib.GetByteFromByteArray(data, start);
                                            break;
                                        case DataType.Short:
                                            variable.VarValue = ShortLib.GetShortFromByteArray(data, start);
                                            break;
                                        case DataType.UShort:
                                            variable.VarValue = UShortLib.GetUShortFromByteArray(data, start);
                                            break;
                                        case DataType.Int:
                                            variable.VarValue = IntLib.GetIntFromByteArray(data, start, dataFormat);
                                            break;
                                        case DataType.UInt:
                                            variable.VarValue = UIntLib.GetUIntFromByteArray(data, start, dataFormat);
                                            break;
                                        case DataType.Float:
                                            variable.VarValue = FloatLib.GetFloatFromByteArray(data, start, dataFormat);
                                            break;
                                        case DataType.Double:
                                            variable.VarValue = DoubleLib.GetDoubleFromByteArray(data, start, dataFormat);
                                            break;
                                        case DataType.Long:
                                            variable.VarValue = LongLib.GetLongFromByteArray(data, start, dataFormat);
                                            break;
                                        case DataType.ULong:
                                            variable.VarValue = ULongLib.GetULongFromByteArray(data, start, dataFormat);
                                            break;
                                        case DataType.String:
                                            variable.VarValue = StringLib.GetStringFromByteArrayByEncoding(data, start, variable.OffsetOrLength * 2, Encoding.ASCII);
                                            break;
                                        case DataType.ByteArray:
                                            variable.VarValue = ByteArrayLib.GetByteArrayFromByteArray(data, start, variable.OffsetOrLength * 2);
                                            break;
                                        case DataType.HexString:
                                            variable.VarValue = StringLib.GetHexStringFromByteArray(data, start, variable.OffsetOrLength * 2);
                                            break;
                                        default:
                                            break;
                                    }

                                    variable.VarValue = MigrationLib.GetMigrationValue(variable.VarValue, variable.Scale.ToString(), variable.Offset.ToString()).Content;

                                    device.UpdateVariable(variable);
                                }
                            }
                            else
                            {
                                device.IsConnected = false;
                            }
                        }
                    }

                    Thread.Sleep(500);
                }
                else
                {
                    if (device.ReConnectSign)
                    {
                        CommonMethods.Modbus.DisConnect();
                        //重连
                        Thread.Sleep(device.ReConnectTime);
                    }

                    CommonMethods.Modbus = new ModbusTCP();

                    device.IsConnected = CommonMethods.Modbus.Connect(CommonMethods.Device.IPAddress, CommonMethods.Device.Port);

                    if (device.ReConnectSign == false)
                    {
                        device.ReConnectSign = true;
                    }
                }
            }
        }


        private void Device_AlarmTrigEvent(bool arg1, Variable arg2)
        {
            throw new NotImplementedException();
        }
        
        //配置显示主窗体中登录的用户名和默认显示的界面
        public void Configure()
        {
            UserName = CommonMethods.CurrentAdmin.LoginName;
            NavigationParameters keys = new NavigationParameters();
            keys.Add("AddLogList",AddLog);
            _regionManager.Regions["ContentRegion"].RequestNavigate("MonitorView",keys);
        }

        #endregion
    }
}
