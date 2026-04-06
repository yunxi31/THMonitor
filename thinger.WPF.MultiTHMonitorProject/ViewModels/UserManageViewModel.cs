using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.WPF.MultiTHMonitorBLL;
using thinger.WPF.MultiTHMonitorModels.SQL;

namespace thinger.WPF.MultiTHMonitorProject.ViewModels
{
    public class UserManageViewModel:BindableBase
    {
        
        private SysAdminManage sysAdminManage = new SysAdminManage();

        #region 命令属性

        public DelegateCommand SelectAllCommand{ get; set; }
        public DelegateCommand AddUserCommand{ get; set; }
        public DelegateCommand<object> DelUserCommand{ get; set; }
        public DelegateCommand<object> SelectRowCommand { get; set; }
        public DelegateCommand ModifyUserCommand { get;set; }
        #endregion

        public UserManageViewModel()
        {
            SelectAllCommand = new DelegateCommand(ExeSelectAll);
            AddUserCommand = new DelegateCommand(ExeAddUser);
            DelUserCommand = new DelegateCommand<object>(ExeDelUser);
            SelectRowCommand = new DelegateCommand<object>(ExeSelectRowUser);
            ModifyUserCommand = new DelegateCommand(ExeModifyUser);
            this.QueryUser();
        }


        #region 复选框属性
        private bool paramSetCheckedVal;

        public bool ParamSetCheckedVal
        {
            get { return paramSetCheckedVal; }
            set { paramSetCheckedVal = value; RaisePropertyChanged(); }
        }

        private bool recipeCheckedVal;

        public bool RecipeCheckedVal
        {
            get { return recipeCheckedVal; }
            set { recipeCheckedVal = value; RaisePropertyChanged(); }
        }

        private bool historyLogCheckedVal;

        public bool HistoryLogCheckedVal
        {
            get { return historyLogCheckedVal; }
            set { historyLogCheckedVal = value; RaisePropertyChanged(); }
        }
        private bool historyTrendCheckedVal;

        public bool HistoryTrendCheckedVal
        {
            get { return historyTrendCheckedVal; }
            set { historyTrendCheckedVal = value; RaisePropertyChanged(); }
        }
        private bool userManageCheckedVal;

        public bool UserManageCheckedVal
        {
            get { return userManageCheckedVal; }
            set { userManageCheckedVal = value; RaisePropertyChanged(); }
          
        }

        #endregion


        #region 信息输入属性
        private int loginId;

        public int LoginId
        {
            get { return loginId; }
            set { loginId = value; RaisePropertyChanged(); }
        }

        private string loginName;

        public string LoginName
        {
            get { return loginName; }
            set { loginName = value; RaisePropertyChanged(); }
        }

        private string loginPwd;

        public string LoginPwd
        {
            get { return loginPwd; }
            set { loginPwd = value; RaisePropertyChanged(); }
        }

        private string confirmLoginPwd;

        public string ConfirmLoginPwd
        {
            get { return confirmLoginPwd; }
            set { confirmLoginPwd = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 集合列表属性
        private ObservableCollection<SysAdmin> sysAdmins;

        public ObservableCollection<SysAdmin> SysAdmins
        {
            get { return sysAdmins; }
            set { sysAdmins = value; RaisePropertyChanged(); }
        }

        #endregion
        
        #region 方法
        /// <summary>
        /// 查询用户数据
        /// </summary>
        private void QueryUser()
        {
            SysAdmins = new ObservableCollection<SysAdmin>(sysAdminManage.QuerySysAdmins());
            
        }

        /// <summary>
        /// 全选和不全选操作
        /// </summary>
        private void ExeSelectAll()
        {
            if (!ParamSetCheckedVal || !UserManageCheckedVal ||!HistoryLogCheckedVal || !HistoryTrendCheckedVal ||!RecipeCheckedVal)
            {
                this.UserManageCheckedVal = true;
                this.HistoryLogCheckedVal = true;
                this.ParamSetCheckedVal = true;
                this.HistoryTrendCheckedVal=true;
                this.RecipeCheckedVal = true;
            }
            else
            {
                this.UserManageCheckedVal = false;
                this.HistoryLogCheckedVal = false;
                this.ParamSetCheckedVal = false;
                this.HistoryTrendCheckedVal = false;
                this.RecipeCheckedVal = false;
            }
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        private void ExeAddUser()
        {
            //此处最好对值做一个非空判断和提醒。
            if (this.LoginPwd==this.ConfirmLoginPwd)
            {
                SysAdmin sysAdmin = new SysAdmin()
                {
                    LoginPwd = this.LoginPwd,
                    LoginName = this.LoginName,
                    ParamSet = this.ParamSetCheckedVal,
                    HistoryLog = this.HistoryLogCheckedVal,
                    HistoryTrend=this.HistoryTrendCheckedVal,
                    Recipe = this.RecipeCheckedVal,
                    UserManage = this.UserManageCheckedVal
                };
               var result= sysAdminManage.AddSysAdmin(sysAdmin);
                if (result>0)
                {
                   QueryUser();
                }
                Clear();
            }
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        private void ExeDelUser(object obj)
        {
            var result = sysAdminManage.DeleteSysAdmin(Convert.ToInt32(obj));
            if (result > 0)
            {
                QueryUser();
            }
        }
        /// <summary>
        /// 选中一行，将其给文本输入框等赋值。
        /// </summary>
        /// <param name="obj"></param>
        private void ExeSelectRowUser(object obj)
        {
           SysAdmin sysAdmin=obj as SysAdmin;
            if (sysAdmin != null)
            {
                LoginName = sysAdmin.LoginName;
                LoginPwd = sysAdmin.LoginPwd;
                ConfirmLoginPwd = sysAdmin.LoginPwd;
                ParamSetCheckedVal = sysAdmin.ParamSet;
                UserManageCheckedVal = sysAdmin.UserManage;
                HistoryLogCheckedVal = sysAdmin.HistoryLog;
                HistoryTrendCheckedVal = sysAdmin.HistoryTrend;
                RecipeCheckedVal=sysAdmin.Recipe;
                LoginId = sysAdmin.LoginId;
            }
        }
        /// <summary>
        /// 修改当前选中行的值
        /// </summary>
        private void ExeModifyUser()
        {
            if (this.LoginPwd == this.ConfirmLoginPwd)
            {
                SysAdmin sysAdmin = new SysAdmin()
                {
                    LoginId = this.LoginId,
                    LoginPwd = this.LoginPwd,
                    LoginName = this.LoginName,
                    ParamSet = this.ParamSetCheckedVal,
                    HistoryLog = this.HistoryLogCheckedVal,
                    HistoryTrend = this.HistoryTrendCheckedVal,
                    Recipe = this.RecipeCheckedVal,
                    UserManage = this.UserManageCheckedVal
                };
                var result = sysAdminManage.ModifySysAdmin(sysAdmin);
                if (result > 0)
                {
                    QueryUser();
                }
                Clear();
            }
            
        }
        private void Clear()
        {
            LoginName = "";
            LoginPwd = "";
            ConfirmLoginPwd = "";
            ParamSetCheckedVal = false;
            HistoryLogCheckedVal = false;
            RecipeCheckedVal = false;
            UserManageCheckedVal = false;
        }
        #endregion

    }
}
