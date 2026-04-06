using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using thinger.DataConvertLib;
using thinger.WPF.MultiTHMonitorHelper;
using thinger.WPF.MultiTHMonitorModels;
using thinger.WPF.MultiTHMonitorProject.Command;
using thinger.WPF.MultiTHMonitorProject.Events;

namespace thinger.WPF.MultiTHMonitorProject.ViewModels
{
    public class ParamSetViewModel:BindableBase
    {
        //public ParamSetViewModel(){}
        //此处利用构造函数注入一个弹窗服务依赖
        public ParamSetViewModel(IDialogService dialogService,IEventAggregator eventAggregator)
        {
            ConfirmSetCommand = new DelegateCommand(ExeConfirmSet);
            CancelSetCommand = new DelegateCommand(ExeCancelSet);
            OpenGroupViewCommand = new DelegateCommand<string>(ExeOpenGroupView);
            OpenVariableViewCommand = new DelegateCommand<string>(ExeOpenVariableView);
            OpenModifyParamSetViewCommand = new DelegateCommand<object>(ExeOpenModifyParamSetView);
            dataMethods.OpenTimer();
            GetLimitParam();
            this._dialogService=dialogService;
            this._eventAggregator=eventAggregator;
        }

        #region 变量
        //private string devPath=string.Empty;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private CommonDataMethods dataMethods = new CommonDataMethods();
        #endregion
        
        #region 命令属性
        public DelegateCommand ConfirmSetCommand { get; set; }
        public DelegateCommand CancelSetCommand { get; set; }
        public DelegateCommand<string> OpenGroupViewCommand { get; set; }
        public DelegateCommand<string> OpenVariableViewCommand { get; set; }
        public DelegateCommand<object> OpenModifyParamSetViewCommand { get; set; }
        #endregion

        #region 视图模型属性
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
        private ObservableCollection<Group> groupList;

        public ObservableCollection<Group> GroupsList
        {
            get { return groupList; }
            set { groupList = value; RaisePropertyChanged(); }
        }
        #region 1#站点
        private string stateHTemp01= "0.0℃";

        public string StateHTemp01
        {
            get { return stateHTemp01; }
            set { stateHTemp01 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmTemp01;

        public bool IsAlarmTemp01
        {
            get { return isAlarmTemp01; }
            set
            { isAlarmTemp01 = value;RaisePropertyChanged(); }
        }
        private bool isAlarmHum01;

        public bool IsAlarmHum01
        {
            get { return isAlarmHum01; }
            set
            { isAlarmHum01 = value; RaisePropertyChanged(); }
        }
        private string ledAlarmHTemp01= "#98df44";

        public string LedAlarmHTemp01
        {
            get { return ledAlarmHTemp01; }
            set 
            {
                if (isAlarmTemp01)
                {
                    ledAlarmHTemp01 = "Red";
                }
                else
                {
                    ledAlarmHTemp01 = "#98df44";
                }
                
                RaisePropertyChanged(); 
            }
        }


        private string stateLTemp01= "0.0℃";

        public string StateLTemp01
        {
            get { return stateLTemp01; }
            set { stateLTemp01 = value; RaisePropertyChanged(); }
        }
      
        private string ledAlarmLTemp01 = "#98df44";

        public string LedAlarmLTemp01
        {
            get { return ledAlarmLTemp01; }
            set
            {
                if (isAlarmTemp01)
                {
                    ledAlarmLTemp01 = "Red";
                }
                else
                {
                    ledAlarmLTemp01 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        private string stateHHum01="0%";

        public string StateHHum01
        {
            get { return stateHHum01; }
            set { stateHHum01 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmHHum01 = "#98df44";

        public string LedAlarmHHum01
        {
            get { return ledAlarmHHum01; }
            set
            {
                if (isAlarmHum01)
                {
                    ledAlarmHHum01 = "Red";
                }
                else
                {
                    ledAlarmHHum01 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        private string stateLHum01="0%";

        public string StateLHum01
        {
            get { return stateLHum01; }
            set { stateLHum01 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmLHum01 = "#98df44";

        public string LedAlarmLHum01
        {
            get { return ledAlarmLHum01; }
            set
            {
                if (isAlarmHum01)
                {
                    ledAlarmLHum01 = "Red";
                }
                else
                {
                    ledAlarmLHum01 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        #endregion

        #region 2#站点
        private string stateHTemp02= "0.0℃";

        public string StateHTemp02
        {
            get { return stateHTemp02; }
            set { stateHTemp02 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmTemp02;

        public bool IsAlarmTemp02
        {
            get { return isAlarmTemp02; }
            set
            { isAlarmTemp02 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmHum02;

        public bool IsAlarmHum02
        {
            get { return isAlarmHum02; }
            set
            { isAlarmHum02 = value; RaisePropertyChanged(); }
        }
        private string ledAlarmHTemp02 = "#98df44";

        public string LedAlarmHTemp02
        {
            get { return ledAlarmHTemp02; }
            set
            {
                if (isAlarmTemp02)
                {
                    ledAlarmHTemp02 = "Red";
                }
                else
                {
                    ledAlarmHTemp02 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }


        private string stateLTemp02 = "0.0℃";

        public string StateLTemp02
        {
            get { return stateLTemp02; }
            set { stateLTemp02 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmLTemp02 = "#98df44";

        public string LedAlarmLTemp02
        {
            get { return ledAlarmLTemp02; }
            set
            {
                if (isAlarmTemp02)
                {
                    ledAlarmLTemp02 = "Red";
                }
                else
                {
                    ledAlarmLTemp02 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        private string stateHHum02 = "0%";

        public string StateHHum02
        {
            get { return stateHHum02; }
            set { stateHHum02 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmHHum02 = "#98df44";

        public string LedAlarmHHum02
        {
            get { return ledAlarmHHum02; }
            set
            {
                if (isAlarmHum02)
                {
                    ledAlarmHHum02 = "Red";
                }
                else
                {
                    ledAlarmHHum02 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        private string stateLHum02="0%";

        public string StateLHum02
        {
            get { return stateLHum02; }
            set { stateLHum02 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmLHum02 = "#98df44";

        public string LedAlarmLHum02
        {
            get { return ledAlarmLHum02; }
            set
            {
                if (isAlarmHum02)
                {
                    ledAlarmLHum02 = "Red";
                }
                else
                {
                    ledAlarmLHum02 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        #endregion

        #region 3#站点
        private string stateHTemp03 = "0.0℃";

        public string StateHTemp03
        {
            get { return stateHTemp03; }
            set { stateHTemp03 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmTemp03;

        public bool IsAlarmTemp03
        {
            get { return isAlarmTemp03; }
            set
            { isAlarmTemp03 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmHum03;

        public bool IsAlarmHum03
        {
            get { return isAlarmHum03; }
            set
            { isAlarmHum03 = value; RaisePropertyChanged(); }
        }
        private string ledAlarmHTemp03 = "#98df44";

        public string LedAlarmHTemp03
        {
            get { return ledAlarmHTemp03; }
            set
            {
                if (isAlarmTemp03)
                {
                    ledAlarmHTemp03 = "Red";
                }
                else
                {
                    ledAlarmHTemp03 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }


        private string stateLTemp03 = "0.0℃";

        public string StateLTemp03
        {
            get { return stateLTemp03; }
            set { stateLTemp03 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmLTemp03 = "#98df44";

        public string LedAlarmLTemp03
        {
            get { return ledAlarmLTemp03; }
            set
            {
                if (isAlarmTemp03)
                {
                    ledAlarmLTemp03 = "Red";
                }
                else
                {
                    ledAlarmLTemp03 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        private string stateHHum03 = "0%";

        public string StateHHum03
        {
            get { return stateHHum03; }
            set { stateHHum03 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmHHum03 = "#98df44";

        public string LedAlarmHHum03
        {
            get { return ledAlarmHHum03; }
            set
            {
                if (isAlarmHum03)
                {
                    ledAlarmHHum03 = "Red";
                }
                else
                {
                    ledAlarmHHum03 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        private string stateLHum03 = "0%";

        public string StateLHum03
        {
            get { return stateLHum03; }
            set { stateLHum03 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmLHum03 = "#98df44";

        public string LedAlarmLHum03
        {
            get { return ledAlarmLHum03; }
            set
            {
                if (isAlarmHum03)
                {
                    ledAlarmLHum03 = "Red";
                }
                else
                {
                    ledAlarmLHum03 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        #endregion

        #region 4#站点
        private string stateHTemp04 = "0.0℃";

        public string StateHTemp04
        {
            get { return stateHTemp04; }
            set { stateHTemp04 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmTemp04;

        public bool IsAlarmTemp04
        {
            get { return isAlarmTemp04; }
            set
            { isAlarmTemp04 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmHum04;

        public bool IsAlarmHum04
        {
            get { return isAlarmHum04; }
            set
            { isAlarmHum04 = value; RaisePropertyChanged(); }
        }
        private string ledAlarmHTemp04 = "#98df44";

        public string LedAlarmHTemp04
        {
            get { return ledAlarmHTemp04; }
            set
            {
                if (isAlarmTemp04)
                {
                    ledAlarmHTemp04 = "Red";
                }
                else
                {
                    ledAlarmHTemp04 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }


        private string stateLTemp04 = "0.0℃";

        public string StateLTemp04
        {
            get { return stateLTemp04; }
            set { stateLTemp04 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmLTemp04 = "#98df44";

        public string LedAlarmLTemp04
        {
            get { return ledAlarmLTemp04; }
            set
            {
                if (isAlarmTemp04)
                {
                    ledAlarmLTemp04 = "Red";
                }
                else
                {
                    ledAlarmLTemp04 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        private string stateHHum04 = "0%";

        public string StateHHum04
        {
            get { return stateHHum04; }
            set { stateHHum04 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmHHum04 = "#98df44";

        public string LedAlarmHHum04
        {
            get { return ledAlarmHHum04; }
            set
            {
                if (isAlarmHum04)
                {
                    ledAlarmHHum04 = "Red";
                }
                else
                {
                    ledAlarmHHum04 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        private string stateLHum04 = "0%";

        public string StateLHum04
        {
            get { return stateLHum04; }
            set { stateLHum04 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmLHum04 = "#98df44";

        public string LedAlarmLHum04
        {
            get { return ledAlarmLHum04; }
            set
            {
                if (isAlarmHum04)
                {
                    ledAlarmLHum04 = "Red";
                }
                else
                {
                    ledAlarmLHum04 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        #endregion

        #region 5#站点
        private string stateHTemp05 = "0.0℃";

        public string StateHTemp05
        {
            get { return stateHTemp05; }
            set { stateHTemp05 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmTemp05;

        public bool IsAlarmTemp05
        {
            get { return isAlarmTemp05; }
            set
            { isAlarmTemp05 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmHum05;

        public bool IsAlarmHum05
        {
            get { return isAlarmHum05; }
            set
            { isAlarmHum05 = value; RaisePropertyChanged(); }
        }
        private string ledAlarmHTemp05 = "#98df44";

        public string LedAlarmHTemp05
        {
            get { return ledAlarmHTemp05; }
            set
            {
                if (isAlarmTemp05)
                {
                    ledAlarmHTemp05 = "Red";
                }
                else
                {
                    ledAlarmHTemp05 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }


        private string stateLTemp05 = "0.0℃";

        public string StateLTemp05
        {
            get { return stateLTemp05; }
            set { stateLTemp05 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmLTemp05 = "#98df44";

        public string LedAlarmLTemp05
        {
            get { return ledAlarmLTemp05; }
            set
            {
                if (isAlarmTemp05)
                {
                    ledAlarmLTemp05 = "Red";
                }
                else
                {
                    ledAlarmLTemp05 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        private string stateHHum05 = "0%";

        public string StateHHum05
        {
            get { return stateHHum05; }
            set { stateHHum05 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmHHum05 = "#98df44";

        public string LedAlarmHHum05
        {
            get { return ledAlarmHHum05; }
            set
            {
                if (isAlarmHum05)
                {
                    ledAlarmHHum05 = "Red";
                }
                else
                {
                    ledAlarmHHum05 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        private string stateLHum05 = "0%";
        public string StateLHum05
        {
            get { return stateLHum05; }
            set { stateLHum05 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmLHum05 = "#98df44";

        public string LedAlarmLHum05
        {
            get { return ledAlarmLHum05; }
            set
            {
                if (isAlarmHum05)
                {
                    ledAlarmLHum05 = "Red";
                }
                else
                {
                    ledAlarmLHum05 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        #endregion

        #region 6#站点
        private string stateHTemp06 = "0.0℃";

        public string StateHTemp06
        {
            get { return stateHTemp06; }
            set { stateHTemp06 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmTemp06;

        public bool IsAlarmTemp06
        {
            get { return isAlarmTemp06; }
            set
            { isAlarmTemp06 = value; RaisePropertyChanged(); }
        }
        private bool isAlarmHum06;

        public bool IsAlarmHum06
        {
            get { return isAlarmHum06; }
            set
            { isAlarmHum06 = value; RaisePropertyChanged(); }
        }
        private string ledAlarmHTemp06 = "#98df44";

        public string LedAlarmHTemp06
        {
            get { return ledAlarmHTemp06; }
            set
            {
                if (isAlarmTemp06)
                {
                    ledAlarmHTemp06 = "Red";
                }
                else
                {
                    ledAlarmHTemp06 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }


        private string stateLTemp06= "0.0℃";

        public string StateLTemp06
        {
            get { return stateLTemp06; }
            set { stateLTemp06 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmLTemp06 = "#98df44";

        public string LedAlarmLTemp06
        {
            get { return ledAlarmLTemp06; }
            set
            {
                if (isAlarmTemp06)
                {
                    ledAlarmLTemp06 = "Red";
                }
                else
                {
                    ledAlarmLTemp06 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        private string stateHHum06="0%";

        public string StateHHum06
        {
            get { return stateHHum06; }
            set { stateHHum06 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmHHum06 = "#98df44";

        public string LedAlarmHHum06
        {
            get { return ledAlarmHHum06; }
            set
            {
                if (isAlarmHum06)
                {
                    ledAlarmHHum06 = "Red";
                }
                else
                {
                    ledAlarmHHum06 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        private string stateLHum06="0%";
        public string StateLHum06
        {
            get { return stateLHum06; }
            set { stateLHum06 = value; RaisePropertyChanged(); }
        }

        private string ledAlarmLHum06 = "#98df44";

        public string LedAlarmLHum06
        {
            get { return ledAlarmLHum06; }
            set
            {
                if (isAlarmHum06)
                {
                    ledAlarmLHum06 = "Red";
                }
                else
                {
                    ledAlarmLHum06 = "#98df44";
                }

                RaisePropertyChanged();
            }
        }
        #endregion
        #endregion

        #region 方法
        private void ExeConfirmSet()
        {
            string devPath = Environment.CurrentDirectory + "\\Config\\Device.ini";
            if (!File.Exists(devPath))
            {
                var stream = File.Create(Environment.CurrentDirectory + "\\Config\\Device.ini");
                bool result = IniConfigHelper.WriteIniData("设备参数", "IP地址", IPAddress, devPath);
                result &= IniConfigHelper.WriteIniData("设备参数", "d端口号", Port.ToString(), devPath);
                if (result)
                {

                    Device device = new Device
                    {
                        IPAddress = IPAddress,
                        Port = Port,
                        IsConnected = true
                    };
                    CommonMethods.Device = device;
                    MessageBox.Show("通信成功!!!");
                    //向DeviceMessageEvent发送一个消息
                    _eventAggregator.GetEvent<DeviceMessageEvent>().Publish(device);
                }
                else
                {
                    MessageBox.Show("通信设置失败");
                }
                stream.Close();
            }
            else
            {
                bool result = IniConfigHelper.WriteIniData("设备参数", "IP地址", IPAddress, devPath);
                result &= IniConfigHelper.WriteIniData("设备参数", "d端口号", Port.ToString(), devPath);
                if (result)
                {
                    Device device = new Device 
                    {
                        IPAddress = IPAddress,
                        Port = Port,
                        IsConnected = true
                    }; 
                    CommonMethods.Device=device;
                    _eventAggregator.GetEvent<DeviceMessageEvent>().Publish(device);
                    MessageBox.Show("通信成功!!!");
                }
                else
                {
                    MessageBox.Show("通信设置失败");
                }
            }

        }

        /// <summary>
        /// 取消配置的方法
        /// </summary>
        private void ExeCancelSet()
        {
            
            Device device = new Device
            {
                IPAddress = "",
                Port = 0,
                IsConnected = false
            };
            CommonMethods.Device = device;
            _eventAggregator.GetEvent<DeviceMessageEvent>().Publish(device);

            IPAddress = "";
            Port = 0;
        }

        //打开通信组配置窗口方法
        private void ExeOpenGroupView(string obj)
        {
            DialogParameters keys = new DialogParameters();
            keys.Add("ShowParam", "参数修改窗口");
            _dialogService.ShowDialog(obj, keys, callback =>
            {
                if (callback.Result == ButtonResult.OK)
                {
                    string result = callback.Parameters.GetValue<string>("GroupViewValue");
                }
            });
        }

        private void ExeOpenVariableView(string obj)
        {
            DialogParameters keys = new DialogParameters();
            keys.Add("ShowVariableParam", "变量配置窗口");
            _dialogService.ShowDialog(obj, keys, callback =>
            {
                if (callback.Result == ButtonResult.OK)
                {
                    string result = callback.Parameters.GetValue<string>("VariableViewValue");
                }
            });
        }
        
        private void ExeOpenModifyParamSetView(object obj)
        {
            var objArray = (object[])obj;
            DialogParameters keys = new DialogParameters();
            keys.Add("ShowVariableParam", "变量配置窗口");
            keys.Add("ShowSiteName", objArray[1].ToString());
            keys.Add("ShowSiteValue", objArray[2].ToString());
            _dialogService.ShowDialog(objArray[0].ToString(), keys, callback =>
            {
                if (callback.Result == ButtonResult.OK)
                {
                    string result1 = callback.Parameters.GetValue<string>("ParamViewValue1");
                    string result2 = callback.Parameters.GetValue<string>("ParamViewValue2");
                    GetLimitParam();
                }
            });
        }

        private void GetLimitParam()
        {
            if (CommonMethods.Device.IsConnected)
            {
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块1温度高限"))
                {
                    StateHTemp01 = $"{CommonMethods.Device["模块1温度高限"]}℃";
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块1温度低限"))
                {
                    StateLTemp01 =$"{CommonMethods.Device["模块1温度低限"]}℃";
                }
               
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块1温度报警启用"))
                {
                    IsAlarmTemp01 = Convert.ToBoolean(CommonMethods.Device["模块1温度报警启用"]);
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块1湿度高限"))
                {
                    StateHHum01 = $"{CommonMethods.Device["模块1湿度高限"]}%";
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块1湿度低限"))
                {
                    StateLHum01 = $"{CommonMethods.Device["模块1湿度低限"]}%";
                }

                if (CommonMethods.Device.CurrentValue.ContainsKey("模块1湿度报警启用"))
                {
                    IsAlarmHum01 = Convert.ToBoolean(CommonMethods.Device["模块1湿度报警启用"]);
                }


                if (CommonMethods.Device.CurrentValue.ContainsKey("模块2温度高限"))
                {
                    StateHTemp02 = $"{CommonMethods.Device["模块2温度高限"]}℃";
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块2温度低限"))
                {
                    StateLTemp02 = $"{CommonMethods.Device["模块2温度低限"]}℃";
                }

                if (CommonMethods.Device.CurrentValue.ContainsKey("模块2温度报警启用"))
                {
                    IsAlarmTemp02 = Convert.ToBoolean(CommonMethods.Device["模块2温度报警启用"]);
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块2湿度高限"))
                {
                    StateHHum02 = $"{CommonMethods.Device["模块2湿度高限"]}%";
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块2湿度低限"))
                {
                    StateLHum02 = $"{CommonMethods.Device["模块2湿度低限"]}%";
                }

                if (CommonMethods.Device.CurrentValue.ContainsKey("模块2湿度报警启用"))
                {
                    IsAlarmHum02 = Convert.ToBoolean(CommonMethods.Device["模块2湿度报警启用"]);
                }


                if (CommonMethods.Device.CurrentValue.ContainsKey("模块3温度高限"))
                {
                    StateHTemp03 = $"{CommonMethods.Device["模块3温度高限"]}℃";
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块3温度低限"))
                {
                    StateLTemp03 = $"{CommonMethods.Device["模块3温度低限"]}℃";
                }

                if (CommonMethods.Device.CurrentValue.ContainsKey("模块3温度报警启用"))
                {
                    IsAlarmTemp03 = Convert.ToBoolean(CommonMethods.Device["模块3温度报警启用"]);
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块3湿度高限"))
                {
                    StateHHum03 = $"{CommonMethods.Device["模块3湿度高限"]}%";
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块3湿度低限"))
                {
                    StateLHum03 = $"{CommonMethods.Device["模块3湿度低限"]}%";
                }

                if (CommonMethods.Device.CurrentValue.ContainsKey("模块3湿度报警启用"))
                {
                    IsAlarmHum03 = Convert.ToBoolean(CommonMethods.Device["模块3湿度报警启用"]);
                }



                if (CommonMethods.Device.CurrentValue.ContainsKey("模块4温度高限"))
                {
                    StateHTemp04= $"{CommonMethods.Device["模块4温度高限"]}℃";
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块4温度低限"))
                {
                    StateLTemp04 = $"{CommonMethods.Device["模块4温度低限"]}℃";
                }

                if (CommonMethods.Device.CurrentValue.ContainsKey("模块4温度报警启用"))
                {
                    IsAlarmTemp04 = Convert.ToBoolean(CommonMethods.Device["模块4温度报警启用"]);
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块3湿度高限"))
                {
                    StateHHum04 = $"{CommonMethods.Device["模块4湿度高限"]}%";
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块4湿度低限"))
                {
                    StateLHum04 = $"{CommonMethods.Device["模块4湿度低限"]}%";
                }

                if (CommonMethods.Device.CurrentValue.ContainsKey("模块4湿度报警启用"))
                {
                    IsAlarmHum04 = Convert.ToBoolean(CommonMethods.Device["模块4湿度报警启用"]);
                }


                if (CommonMethods.Device.CurrentValue.ContainsKey("模块5温度高限"))
                {
                    StateHTemp05 = $"{CommonMethods.Device["模块5温度高限"]}℃";
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块5温度低限"))
                {
                    StateLTemp05 = $"{CommonMethods.Device["模块5温度低限"]}℃";
                }

                if (CommonMethods.Device.CurrentValue.ContainsKey("模块5温度报警启用"))
                {
                    IsAlarmTemp05 = Convert.ToBoolean(CommonMethods.Device["模块5温度报警启用"]);
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块5湿度高限"))
                {
                    StateHHum05 = $"{CommonMethods.Device["模块5湿度高限"]}%";
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块5湿度低限"))
                {
                    StateLHum05 = $"{CommonMethods.Device["模块5湿度低限"]}%";
                }

                if (CommonMethods.Device.CurrentValue.ContainsKey("模块5湿度报警启用"))
                {
                    IsAlarmHum05 = Convert.ToBoolean(CommonMethods.Device["模块5湿度报警启用"]);
                }


                if (CommonMethods.Device.CurrentValue.ContainsKey("模块6温度高限"))
                {
                    StateHTemp06 = $"{CommonMethods.Device["模块6温度高限"]}℃";
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块6温度低限"))
                {
                    StateLTemp06 = $"{CommonMethods.Device["模块6温度低限"]}℃";
                }

                if (CommonMethods.Device.CurrentValue.ContainsKey("模块6温度报警启用"))
                {
                    IsAlarmTemp06 = Convert.ToBoolean(CommonMethods.Device["模块6温度报警启用"]);
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块5湿度高限"))
                {
                    StateHHum06 = $"{CommonMethods.Device["模块6湿度高限"]}%";
                }
                if (CommonMethods.Device.CurrentValue.ContainsKey("模块6湿度低限"))
                {
                    StateLHum06 = $"{CommonMethods.Device["模块6湿度低限"]}%";
                }

                if (CommonMethods.Device.CurrentValue.ContainsKey("模块6湿度报警启用"))
                {
                    IsAlarmHum06 = Convert.ToBoolean(CommonMethods.Device["模块6湿度报警启用"]);
                }
            }
        }
        #endregion
    }
}
