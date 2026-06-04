using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.WPF.MultiTHMonitorHelper;
using thinger.WPF.MultiTHMonitorModels.Config;
using thinger.WPF.MultiTHMonitorModels;
using System.IO;
using System.Windows.Threading;
using thinger.WPF.MultiTHMonitorBLL;
using thinger.WPF.MultiTHMonitorModels.SQL;
using System.Threading;
using thinger.DataConvertLib;

namespace thinger.WPF.MultiTHMonitorProject.Command
{
    internal class CommonDataMethods
    {
        private DispatcherTimer _timer = new DispatcherTimer();
        private static Device device = new Device();
        //大小端
        private DataFormat dataFormat = DataFormat.ABCD;

        //取消线程源
        private CancellationTokenSource cts;

        //#region 方法
        public  void OpenTimer()
        {
            //_timer.Tick += new EventHandler(LoadDevice);
            //_timer.Tick += new EventHandler(StoreTimer_Elapsed);
            //_timer.Interval = new TimeSpan(0, 0, 1, 0);
            //_timer.Start();
        }

        

        //private static void LoadDevice(object sender, EventArgs e)
        //{
        //    device = DeviceInfoLoad();
        //}
        //public static Device DeviceInfoLoad()
        //{
        //    string devicePath = Environment.CurrentDirectory + "\\Config\\Device.ini";
        //    //通信组参数路径
        //    string groupPath = Environment.CurrentDirectory + "\\Config\\Group.xlsx";

        //    //变量路径
        //    string variablePath = Environment.CurrentDirectory + "\\Config\\Variable.xlsx";

        //    if (!File.Exists(devicePath))
        //    {
        //        CommonMethods.AddLog(1, "设备文件不存在");
        //        return null;
        //    }
        //    List<Group> groupList = LoadGroup(groupPath, variablePath);
        //    if (groupList != null && groupList.Count > 0)
        //    {
        //        try
        //        {
        //            return new Device()
        //            {
        //                IPAddress = IniConfigHelper.ReadIniData("设备参数", "IP地址", "127.0.0.1", devicePath),
        //                Port = Convert.ToInt32(IniConfigHelper.ReadIniData("设备参数", "端口号", "502", devicePath)),
        //                CurrentRecipe = IniConfigHelper.ReadIniData("设备参数", "当前配方", "", devicePath),
        //                GroupList = groupList
        //            };
        //        }
        //        catch (Exception ex)
        //        {
        //            CommonMethods.AddLog(1, "设备信息加载失败：" + ex.Message);
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// 加载通信组信息
        ///// </summary>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //private static List<Group> LoadGroup(string grouppath, string variablepath)
        //{
        //    if (!File.Exists(grouppath))
        //    {
        //        CommonMethods.AddLog(1, "通信组文件不存在");
        //        return null;
        //    }

        //    if (!File.Exists(variablepath))
        //    {
        //        CommonMethods.AddLog(1, "通信变量文件不存在");
        //        return null;
        //    }
        //    List<Group> GpList = null;

        //    try
        //    {
        //        GpList = MiniExcel.Query<Group>(grouppath).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonMethods.AddLog(1, "通信组加载失败：" + ex.Message);
        //        return null;
        //    }
        //    List<Variable> VarList = null;
        //    try
        //    {
        //        VarList = MiniExcel.Query<Variable>(variablepath).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonMethods.AddLog(1, "通信变量加载失败：" + ex.Message);
        //        return null;
        //    }
        //    if (GpList != null && VarList != null)
        //    {
        //        foreach (var group in GpList)
        //        {
        //            group.VarList = VarList.FindAll(c => c.GroupName == group.GroupName).ToList();
        //        }
        //        return GpList;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        ///// <summary>
        ///// 更新数据存储
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        ////private void StoreTimer_Elapsed(object sender, EventArgs e)
        ////{
        ////    //定时存储
        ////    if (CommonMethods.Device.IsConnected)
        ////    {
        ////        if (CommonMethods.Device["1#站点温度"] != null)
        ////        {
        ////            new ActualDataManage().AddActualData(new ActualData()
        ////            {
        ////                InsertTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        ////                Station1Temp = CommonMethods.Device["1#站点温度"]?.ToString(),
        ////                Station1Humidity = CommonMethods.Device["1#站点湿度"]?.ToString(),
        ////                Station2Temp = CommonMethods.Device["2#站点温度"]?.ToString(),
        ////                Station2Humidity = CommonMethods.Device["2#站点湿度"]?.ToString(),
        ////                Station3Temp = CommonMethods.Device["3#站点温度"]?.ToString(),
        ////                Station3Humidity = CommonMethods.Device["3#站点湿度"]?.ToString(),
        ////                Station4Temp = CommonMethods.Device["4#站点温度"]?.ToString(),
        ////                Station4Humidity = CommonMethods.Device["4#站点湿度"]?.ToString(),
        ////                Station5Temp = CommonMethods.Device["5#站点温度"]?.ToString(),
        ////                Station5Humidity = CommonMethods.Device["5#站点湿度"]?.ToString(),
        ////                Station6Temp = CommonMethods.Device["6#站点温度"]?.ToString(),
        ////                Station6Humidity = CommonMethods.Device["6#站点湿度"]?.ToString(),
        ////            });
        ////        }
        ////    }
        ////}
        //public static void StoreTimer()
        //{
        //    //定时存储
        //    if (CommonMethods.Device.IsConnected)
        //    {
        //        if (CommonMethods.Device["1#站点温度"] != null)
        //        {
        //            new ActualDataManage().AddActualData(new ActualData()
        //            {
        //                InsertTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        //                Station1Temp = CommonMethods.Device["1#站点温度"]?.ToString(),
        //                Station1Humidity = CommonMethods.Device["1#站点湿度"]?.ToString(),
        //                Station2Temp = CommonMethods.Device["2#站点温度"]?.ToString(),
        //                Station2Humidity = CommonMethods.Device["2#站点湿度"]?.ToString(),
        //                Station3Temp = CommonMethods.Device["3#站点温度"]?.ToString(),
        //                Station3Humidity = CommonMethods.Device["3#站点湿度"]?.ToString(),
        //                Station4Temp = CommonMethods.Device["4#站点温度"]?.ToString(),
        //                Station4Humidity = CommonMethods.Device["4#站点湿度"]?.ToString(),
        //                Station5Temp = CommonMethods.Device["5#站点温度"]?.ToString(),
        //                Station5Humidity = CommonMethods.Device["5#站点湿度"]?.ToString(),
        //                Station6Temp = CommonMethods.Device["6#站点温度"]?.ToString(),
        //                Station6Humidity = CommonMethods.Device["6#站点湿度"]?.ToString(),
        //            });
        //        }
        //    }
        //}
        //private void PLCCommunication(Device device)
        //{
        //    while (!cts.IsCancellationRequested)
        //    {
        //        if (device.IsConnected)
        //        {
        //            foreach (var gp in device.GroupList)
        //            {
        //                byte[] data = null;
        //                int reqLength = 0;
        //                if (gp.StoreArea == "输入线圈" || gp.StoreArea == "输出线圈")
        //                {
        //                    switch (gp.StoreArea)
        //                    {
        //                        case "输入线圈":
        //                            data = CommonMethods.Modbus.ReadInputCoils(gp.Start, gp.Length);
        //                            reqLength = ShortLib.GetByteLengthFromBoolLength(gp.Length);
        //                            break;
        //                        case "输出线圈":
        //                            data = CommonMethods.Modbus.ReadOutputCoils(gp.Start, gp.Length);
        //                            reqLength = ShortLib.GetByteLengthFromBoolLength(gp.Length);
        //                            break;
        //                        default:
        //                            break;
        //                    }

        //                    if (data != null && data.Length == reqLength)
        //                    {
        //                        foreach (var variable in gp.VarList)
        //                        {
        //                            int start = variable.Start - gp.Start;

        //                            DataType dataType = (DataType)Enum.Parse(typeof(DataType), variable.DataType, true);

        //                            switch (dataType)
        //                            {
        //                                case DataType.Bool:
        //                                    variable.VarValue = BitLib.GetBitFromByteArray(data, start, variable.OffsetOrLength);
        //                                    break;
        //                                default:
        //                                    break;
        //                            }

        //                            device.UpdateVariable(variable);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        device.IsConnected = false;
        //                    }
        //                }
        //                else
        //                {
        //                    switch (gp.StoreArea)
        //                    {
        //                        case "输入寄存器":
        //                            data = CommonMethods.Modbus.ReadInputRegisters(gp.Start, gp.Length);
        //                            reqLength = gp.Length * 2;
        //                            break;
        //                        case "输出寄存器":
        //                            data = CommonMethods.Modbus.ReadOutputRegisters(gp.Start, gp.Length);
        //                            reqLength = gp.Length * 2;
        //                            break;
        //                        default:
        //                            break;
        //                    }
        //                    if (data != null && data.Length == reqLength)
        //                    {
        //                        foreach (var variable in gp.VarList)
        //                        {
        //                            int start = variable.Start - gp.Start;

        //                            start *= 2;

        //                            DataType dataType = (DataType)Enum.Parse(typeof(DataType), variable.DataType, true);

        //                            switch (dataType)
        //                            {
        //                                case DataType.Bool:
        //                                    variable.VarValue = BitLib.GetBitFrom2BytesArray(data, start, variable.OffsetOrLength, dataFormat == DataFormat.BADC || dataFormat == DataFormat.DCBA);
        //                                    break;
        //                                case DataType.Byte:
        //                                    variable.VarValue = ByteLib.GetByteFromByteArray(data, start);
        //                                    break;
        //                                case DataType.Short:
        //                                    variable.VarValue = ShortLib.GetShortFromByteArray(data, start);
        //                                    break;
        //                                case DataType.UShort:
        //                                    variable.VarValue = UShortLib.GetUShortFromByteArray(data, start);
        //                                    break;
        //                                case DataType.Int:
        //                                    variable.VarValue = IntLib.GetIntFromByteArray(data, start, dataFormat);
        //                                    break;
        //                                case DataType.UInt:
        //                                    variable.VarValue = UIntLib.GetUIntFromByteArray(data, start, dataFormat);
        //                                    break;
        //                                case DataType.Float:
        //                                    variable.VarValue = FloatLib.GetFloatFromByteArray(data, start, dataFormat);
        //                                    break;
        //                                case DataType.Double:
        //                                    variable.VarValue = DoubleLib.GetDoubleFromByteArray(data, start, dataFormat);
        //                                    break;
        //                                case DataType.Long:
        //                                    variable.VarValue = LongLib.GetLongFromByteArray(data, start, dataFormat);
        //                                    break;
        //                                case DataType.ULong:
        //                                    variable.VarValue = ULongLib.GetULongFromByteArray(data, start, dataFormat);
        //                                    break;
        //                                case DataType.String:
        //                                    variable.VarValue = StringLib.GetStringFromByteArrayByEncoding(data, start, variable.OffsetOrLength * 2, Encoding.ASCII);
        //                                    break;
        //                                case DataType.ByteArray:
        //                                    variable.VarValue = ByteArrayLib.GetByteArrayFromByteArray(data, start, variable.OffsetOrLength * 2);
        //                                    break;
        //                                case DataType.HexString:
        //                                    variable.VarValue = StringLib.GetHexStringFromByteArray(data, start, variable.OffsetOrLength * 2);
        //                                    break;
        //                                default:
        //                                    break;
        //                            }

        //                            variable.VarValue = MigrationLib.GetMigrationValue(variable.VarValue, variable.Scale.ToString(), variable.Offset.ToString()).Content;

        //                            device.UpdateVariable(variable);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        device.IsConnected = false;
        //                    }
        //                }
        //            }

        //            Thread.Sleep(500);
        //        }
        //        else
        //        {
        //            if (device.ReConnectSign)
        //            {
        //                CommonMethods.Modbus.DisConnect();
        //                //重连
        //                Thread.Sleep(device.ReConnectTime);
        //            }

        //            CommonMethods.Modbus = new ModbusTCP();

        //            device.IsConnected = CommonMethods.Modbus.Connect(CommonMethods.Device.IPAddress, CommonMethods.Device.Port);

        //            if (device.ReConnectSign == false)
        //            {
        //                device.ReConnectSign = true;
        //            }
        //        }
        //    }
        //}
        //#endregion

    }
}
