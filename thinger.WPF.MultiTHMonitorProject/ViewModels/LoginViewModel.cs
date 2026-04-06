using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using thinger.WPF.MultiTHMonitorBLL;
using thinger.WPF.MultiTHMonitorModels.SQL;
using thinger.WPF.MultiTHMonitorProject.Command;
using thinger.WPF.MultiTHMonitorProject.Views;

namespace thinger.WPF.MultiTHMonitorProject.ViewModels
{
    //IDialogAware:实现弹窗会话服务
    public class LoginViewModel : BindableBase,IDialogAware
    {
		public LoginViewModel()
		{
			LoginCommand = new DelegateCommand<object>(ExeLogin);
			LogoutCommand = new DelegateCommand<string>(ExeCloseLogin);

		}

        

        //通过命令方法退出登录窗体，实现关闭的操作
        private void ExeCloseLogin(string obj)
        {
            Environment.Exit(0);
        }

        public DelegateCommand<object> LoginCommand { get; private set; }
		public DelegateCommand<string> LogoutCommand { get; private set; }
		/// <summary>
		/// 登录姓名
		/// </summary>
		private string loginName;

		public string LoginName
		{
			get { return loginName; }
			set { loginName = value; RaisePropertyChanged(); }
		}
		/// <summary>
		/// 登录密码
		/// </summary>
		private string loginPwd;

		public string LoginPwd
		{
			get { return loginPwd; }
			set { loginPwd = value; RaisePropertyChanged(); }
		}
		/// <summary>
		/// 错误提示消息
		/// </summary>
		private string loginTip;

        public string LoginTip
		{
			get { return loginTip; }
			set { loginTip = value; RaisePropertyChanged(); }
		}
		public event Action<IDialogResult> RequestClose;
		public string Title { get; set; }

        private void ExeLogin(object obj)
        {
			Window loginWindow = obj as Window;
			if (string.IsNullOrWhiteSpace(LoginName) || string.IsNullOrWhiteSpace(LoginPwd))
			{
				LoginTip = "**用户名或密码不能为空!!!";
				return;
			}
			else 
			{
                //封装对象
                SysAdmin objAdmin = new SysAdmin()
                {
                    LoginName = LoginName,
                    LoginPwd = LoginPwd
                };
                //用户查询
                objAdmin = new SysAdminManage().AdminLogin(objAdmin);
                if (objAdmin == null)
                {
					LoginTip = "**用户名或密码错误,请重新输入!!!";
					LoginName = "";
					LoginPwd = "";
					return;
                }
                else
                {

                    CommonMethods.CurrentAdmin = objAdmin;
                    RequestClose?.Invoke(new DialogResult(ButtonResult.OK));//通知登录成功
                    
                }
            }
        }
		
		public bool CanCloseDialog()
        {
			return true;
        }

        public void OnDialogClosed()
        {
			RequestClose?.Invoke(new DialogResult(ButtonResult.No));
		}

        public void OnDialogOpened(IDialogParameters parameters)
        {
           
        }
    }
}
