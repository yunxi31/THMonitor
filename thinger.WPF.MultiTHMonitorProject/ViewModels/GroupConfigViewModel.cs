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
using thinger.WPF.MultiTHMonitorModels;

namespace thinger.WPF.MultiTHMonitorProject.ViewModels
{
    public class GroupConfigViewModel : BindableBase, IDialogAware
    {
        public GroupConfigViewModel()
        {
            AddGroupCommand = new DelegateCommand(ExeAddGroup);
            DeleteGroupCommand = new DelegateCommand<string>(ExeDelGroup);
            SelectRowCommand = new DelegateCommand<object>(ExeSelectGroup);
            ModifyGroupCommand = new DelegateCommand<string>(ExeModifyGroup);
            LogoutCommand = new DelegateCommand<object>(ExeCloseGroup);
            this.GroupsList =new ObservableCollection<Group>(GetAllGroups());
        }

       

        #region 普通变量属性

        //private string groupPath = ""; 
        private List<Group> totalGroups = new List<Group>();
        #endregion

        #region 视图属性

        private ObservableCollection<Group> groupList;

        public ObservableCollection<Group> GroupsList
        {
            get { return groupList; }
            set { groupList = value; RaisePropertyChanged(); }
        }

        private string groupName;

        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; RaisePropertyChanged(); }
        }

        private ushort start;

        public ushort Start
        {
            get { return start; }
            set { start = value; RaisePropertyChanged(); }
        }
        private ushort length;

        public ushort Length
        {
            get { return length; }
            set { length = value; RaisePropertyChanged(); }
        }


        public ObservableCollection<string> StoreAreaList { get; set; } = new ObservableCollection<string>
        {
            "输入线圈","输出线圈","输入寄存器","输出寄存器"
        };

        private string selectStoreArea;

        public string SelectStoreArea
        {
            get { return selectStoreArea; }
            set { selectStoreArea = value; RaisePropertyChanged(); }
        }
        private string remark;

        public string Remark
        {
            get { return remark; }
            set { remark = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 命令属性

        public DelegateCommand AddGroupCommand { get; set; }
        public DelegateCommand<string> DeleteGroupCommand { get; set; }
        public DelegateCommand<string> ModifyGroupCommand { get; set; }
        public DelegateCommand<object> SelectRowCommand { get; set; }
        public DelegateCommand<object> LogoutCommand { get; private set; }
        #endregion

        #region 弹窗服务接口实现
        public string Title { get; set; }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            DialogParameters keys = new DialogParameters();
            keys.Add("GroupViewValue", "关闭弹窗测试！！");
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, keys));
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("ShowGroupParam");
        }
        #endregion

        #region 方法
        private void ExeAddGroup()
        {
            if (IsVarNameExits(GroupName))
            {
                MessageBox.Show("通信组名称已经存在，请重新添加！！！");
                Clear();
                return;
            }
            totalGroups.Add(new Group()
            {
                GroupName =this.GroupName,
                Start =this.Start,
                StoreArea = this.SelectStoreArea,
                Length = this.Length,
                Remark = this.Remark
            });
            try
            {
                //此处用这种方式写入文件，防止提示文件已存在
                var stream = File.Create(Environment.CurrentDirectory + "\\Config\\Group.xlsx");
                MiniExcel.SaveAs(stream, totalGroups);
                stream.Close();//不要忘记关闭流
                GroupsList = new ObservableCollection<Group>(GetAllGroups());
                Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show("添加通信组失败，失败原因："+ex.Message);
            }
            
        }
        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ExeDelGroup(string obj)
        {
            totalGroups.RemoveAll(c => c.GroupName == obj);
            var stream = File.Create(Environment.CurrentDirectory + "\\Config\\Group.xlsx");
            MiniExcel.SaveAs(stream, totalGroups);
            stream.Close();//不要忘记关闭流
            GroupsList = new ObservableCollection<Group>(GetAllGroups());
        }
        /// <summary>
        /// 修改组
        /// </summary>
        /// <param name="obj">组名称参数</param>
        private void ExeModifyGroup(string obj)
        {
            Group group = totalGroups.Find(c => c.GroupName == obj);
            group.GroupName = obj;
            group.Start = this.Start;
            group.Length = this.Length;
            group.Remark = this.Remark;
            group.StoreArea = this.SelectStoreArea;
            //重新写入数据
            var stream = File.Create(Environment.CurrentDirectory + "\\Config\\Group.xlsx");
            MiniExcel.SaveAs(stream, totalGroups);
            stream.Close();//不要忘记关闭流
            GroupsList = new ObservableCollection<Group>(GetAllGroups());
            Clear();
        }
        /// <summary>
        /// 查询通信组
        /// </summary>
        private List<Group> GetAllGroups()
        {
            var groupPath = Environment.CurrentDirectory + "\\Config\\Group.xlsx";
            if (!File.Exists(groupPath))
            {
                return new List<Group>();
            }
            totalGroups = MiniExcel.Query<Group>(groupPath).ToList();
            return totalGroups;
        }
        /// <summary>
        /// 点击表格行，选择当前组
        /// </summary>
        private void ExeSelectGroup(object obj)
        {
            Group group = obj as Group;
            if (group != null)
            {
                this.GroupName = group.GroupName;
                this.Start = group.Start;
                this.Length = group.Length;
                this.Remark = group.Remark;
            }
        }
        
        /// <summary>
        /// 判断通信组是否重复
        /// </summary>
        /// <param name="groupName">通信组名</param>
        /// <returns></returns>
        private bool IsVarNameExits(string groupName)
        {
            return totalGroups.FindAll(c => c.GroupName == groupName).ToList().Count > 0; 
        }
        
        /// <summary>
        /// 清空输入框数据
        /// </summary>
        private void Clear()
        {
            this.GroupName = "";
            this.Start = 0;
            this.Length= 0;
            this.Remark = "";
        }
        private void ExeCloseGroup(object obj)
        {
            OnDialogClosed();
        }
        #endregion


    }
}
