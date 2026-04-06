using MiniExcelLibs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using thinger.DataConvertLib;
using thinger.WPF.MultiTHMonitorModels;
using thinger.WPF.MultiTHMonitorModels.Config;

namespace thinger.WPF.MultiTHMonitorProject.ViewModels
{
    public class VariableConfigViewModel :BindableBase,IDialogAware
    {
        public VariableConfigViewModel()
        {
            AddVarCommand = new DelegateCommand(ExeAddVar);
            DeleteVarCommand = new DelegateCommand<string>(ExeDelVar);
            SelectRowCommand = new DelegateCommand<object>(ExeSelectVar);
            ModifyVarCommand = new DelegateCommand<string>(ExeModifyVar);
            LogoutCommand = new DelegateCommand<object>(ExeCloseVar);
            this.VarsList = new ObservableCollection<Variable>(GetAllVars());
            GetGroups();
        }

       


        #region 成员属性
        private List<Group> totalGroups = new List<Group>();
        private List<Variable> totalVars = new List<Variable>();
        #endregion
        #region 视图属性
        public ObservableCollection<string> GroupNames { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> DataTypes { get; set; } = new ObservableCollection<string>(Enum.GetNames(typeof(DataType)));
        private ObservableCollection<Variable> varsList;

        public ObservableCollection<Variable> VarsList
        {
            get { return varsList; }
            set { varsList = value; RaisePropertyChanged(); }
        }
        private string selectGroupName;

        public string SelectGroupName
        {
            get { return selectGroupName; }
            set { selectGroupName = value; RaisePropertyChanged(); }
        }

        private string selectDataType;

        public string SelectDataType
        {
            get { return selectDataType; }
            set { selectDataType = value; }
        }

        private string varName;

        public string VarName
        {
            get { return varName; }
            set { varName = value; RaisePropertyChanged(); }
        }
        private ushort start;

        public ushort Start
        {
            get { return start; }
            set { start = value; }
        }
        private int offsetOrLength;

        public int OffsetOrLength
        {
            get { return offsetOrLength; }
            set { offsetOrLength = value; }
        }
        private float scale;

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        private float offset;

        public float Offset
        {
            get { return offset; }
            set { offset = value; }
        }
        private bool posAlarm;

        public bool PosAlarm
        {
            get { return posAlarm; }
            set { posAlarm = value; }
        }
        private bool negAlarm;

        public bool NegAlarm
        {
            get { return negAlarm; }
            set { negAlarm = value; }
        }

        private string remark;

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        #endregion
        
        #region 命令属性

        public DelegateCommand AddVarCommand { get; set; }
        public DelegateCommand<string> DeleteVarCommand { get; set; }
        public DelegateCommand<string> ModifyVarCommand { get; set; }
        public DelegateCommand<object> SelectRowCommand { get; set; }
        public DelegateCommand<object> LogoutCommand { get; private set; }
        #endregion

        #region 方法
        private void GetGroups()
        {
            totalGroups=GetAllGroups();
            if (totalGroups != null)
            {
                foreach (var item in totalGroups)
                {
                    GroupNames.Add(item.GroupName);
                }
            }
            
        }
        /// <summary>
        /// 查询组
        /// </summary>
        /// <returns></returns>
        private List<Group> GetAllGroups()
        {
            var groupPath = Environment.CurrentDirectory + "\\Config\\Group.xlsx";
            if (!File.Exists(groupPath))
            {
                return null;
            }
            totalGroups = MiniExcel.Query<Group>(groupPath).ToList();
            return totalGroups;
        }
        /// <summary>
        /// 选择变量行
        /// </summary>
        /// <param name="obj"></param>
        private void ExeSelectVar(object obj)
        {
            Variable variable = obj as Variable;
            if (variable != null)
            {
                this.VarName=variable.VarName;
                this.OffsetOrLength=variable.OffsetOrLength;
                this.PosAlarm=variable.PosAlarm;
                this.NegAlarm=variable.NegAlarm;
                this.Scale=variable.Scale;
                this.Offset=variable.Offset;
                this.Start = variable.Start;
                this.Remark = variable.Remark;
            }
        }
        /// <summary>
        /// 修改变量信息
        /// </summary>
        /// <param name="obj"></param>
        private void ExeModifyVar(string obj)
        {
            Variable variable = totalVars.Find(c => c.VarName == obj);
            variable.GroupName = this.SelectGroupName;
            variable.VarName = obj;
            variable.Start = this.Start;
            variable.OffsetOrLength=this.OffsetOrLength;
            variable.DataType = this.SelectDataType;
            variable.PosAlarm=this.PosAlarm;
            variable.NegAlarm=this.NegAlarm;
            variable.Scale=this.Scale;
            variable.Offset=this.Offset;
            variable.Remark = this.Remark;
            //重新写入数据
            var stream = File.Create(Environment.CurrentDirectory + "\\Config\\Variable.xlsx");
            MiniExcel.SaveAs(stream, totalVars);
            stream.Close();//不要忘记关闭流
            VarsList = new ObservableCollection<Variable>(GetAllVars());
            Clear();
        }
        /// <summary>
        /// 删除变量信息
        /// </summary>
        /// <param name="obj"></param>
        private void ExeDelVar(string obj)
        {
            totalVars.RemoveAll(c => c.VarName == obj);
            var stream = File.Create(Environment.CurrentDirectory + "\\Config\\Variable.xlsx");
            MiniExcel.SaveAs(stream, totalVars);
            stream.Close();//不要忘记关闭流
            VarsList = new ObservableCollection<Variable>(GetAllVars());
        }
        /// <summary>
        /// 添加变量信息
        /// </summary>
        private void ExeAddVar()
        {
            if (IsVarNameExits(VarName))
            {
                MessageBox.Show("变量名称已经存在，请重新添加！！！");
                Clear();
                return;
            }
            totalVars.Add(new Variable()
            {
                GroupName = this.selectGroupName,
                VarName=this.VarName,
                Start = this.Start,
                OffsetOrLength = this.OffsetOrLength,
                DataType = this.SelectDataType,
                PosAlarm = this.PosAlarm,
                NegAlarm = this.NegAlarm,
                Scale = this.Scale,
                Offset = this.Offset,
                Remark = this.Remark
            });
            try
            {
                //此处用这种方式写入文件，防止提示文件已存在
                var stream = File.Create(Environment.CurrentDirectory + "\\Config\\Variable.xlsx");
                MiniExcel.SaveAs(stream, totalVars);
                stream.Close();//不要忘记关闭流
                VarsList = new ObservableCollection<Variable>(GetAllVars());
                Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show("添加变量信息失败，失败原因：" + ex.Message);
            }
        }
        /// <summary>
        /// 获取所有变量
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private IEnumerable<Variable> GetAllVars()
        {
            var varPath = Environment.CurrentDirectory + "\\Config\\Variable.xlsx";
            if (!File.Exists(varPath))
            {
                return new List<Variable>();
            }
            totalVars = MiniExcel.Query<Variable>(varPath).ToList();
            return totalVars;
        }

        /// <summary>
        /// 判断变量是否重复
        /// </summary>
        /// <param name="varName">变量名称</param>
        /// <returns></returns>
        private bool IsVarNameExits(string varName)
        {
            return totalVars.FindAll(c => c.VarName == varName).ToList().Count > 0;
        }

        /// <summary>
        /// 清空输入框数据
        /// </summary>
        private void Clear()
        {
            this.VarName = "";
            this.Start = 0;
            this.OffsetOrLength = 0;
            this.PosAlarm = false;
            this.NegAlarm = false;
            this.Offset=0;
            this.Scale = 0;
            this.Remark = "";
        }
        private void ExeCloseVar(object obj)
        {
            OnDialogClosed();
        }
        #endregion

        #region 弹窗会话接口实现
        public string Title { get; set; }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            DialogParameters keys = new DialogParameters();
            keys.Add("VariableViewValue", "关闭弹窗！！");
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, keys));
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("ShowVariableParam");
        }
        #endregion
    }
}
