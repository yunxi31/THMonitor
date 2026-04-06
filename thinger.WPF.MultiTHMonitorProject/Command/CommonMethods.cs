using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.DataConvertLib;
using thinger.WPF.MultiTHMonitorHelper;
using thinger.WPF.MultiTHMonitorModels;
using thinger.WPF.MultiTHMonitorModels.Config;
using thinger.WPF.MultiTHMonitorModels.SQL;

namespace thinger.WPF.MultiTHMonitorProject.Command
{ 
    public class CommonMethods
    {
        //public static Action<int, string> AddLog;
        

        public static SysAdmin CurrentAdmin { get; set; }

        public static Device Device { get; set; }

        public static ModbusTCP Modbus { get; set; }

        private static Variable FindVariable(string varName)
        {
            foreach (var item in Device.GroupList)
            {
                var res = item.VarList.Find(c => c.VarName == varName);
                if (res != null)
                {
                    return res;
                }
                else
                {
                    continue;
                }
            }
            return null;
        }

        /// <summary>
        /// 通过写入的方法
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varValue"></param>
        /// <returns></returns>
        public static bool CommonWrite(string varName, string varValue)
        {
            var variable = FindVariable(varName);

            //第一步：找到对应的变量对象
            if (variable != null)
            {
                try
                {
                    //第二步：线性转换

                    DataType dataType = (DataType)Enum.Parse(typeof(DataType), variable.DataType);

                    var result = MigrationLib.SetMigrationValue(varValue, dataType, variable.Scale.ToString(), variable.Offset.ToString());

                    if (result.IsSuccess)
                    {
                        //第三步：写入PLC

                        switch (dataType)
                        {
                            case DataType.Bool:
                                return Modbus.PreSetSingleCoil(variable.Start, Convert.ToBoolean(result.Content));
                            case DataType.Short:
                                return Modbus.PreSetSingleRegister(variable.Start, Convert.ToInt16(result.Content));
                            case DataType.UShort:
                                return Modbus.PreSetSingleRegister(variable.Start, Convert.ToUInt16(result.Content));
                            case DataType.Int:
                                return Modbus.PreSetMultiRegisters(variable.Start, ByteArrayLib.GetByteArrayFromInt(Convert.ToInt32(result.Content)));
                            case DataType.UInt:
                                return Modbus.PreSetMultiRegisters(variable.Start, ByteArrayLib.GetByteArrayFromUInt(Convert.ToUInt32(result.Content)));
                            case DataType.Float:
                                return Modbus.PreSetMultiRegisters(variable.Start, ByteArrayLib.GetByteArrayFromFloat(Convert.ToSingle(result.Content)));
                            case DataType.Double:
                                return Modbus.PreSetMultiRegisters(variable.Start, ByteArrayLib.GetByteArrayFromDouble(Convert.ToDouble(result.Content)));
                            case DataType.Long:
                                return Modbus.PreSetMultiRegisters(variable.Start, ByteArrayLib.GetByteArrayFromLong(Convert.ToInt64(result.Content)));
                            case DataType.ULong:
                                return Modbus.PreSetMultiRegisters(variable.Start, ByteArrayLib.GetByteArrayFromULong(Convert.ToUInt64(result.Content)));
                            case DataType.String:
                                return Modbus.PreSetMultiRegisters(variable.Start, Encoding.ASCII.GetBytes(result.Content));
                            case DataType.ByteArray:
                                return Modbus.PreSetMultiRegisters(variable.Start, ByteArrayLib.GetByteArrayFromHexString(result.Content));
                            default:
                                break;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

      
    }
}
