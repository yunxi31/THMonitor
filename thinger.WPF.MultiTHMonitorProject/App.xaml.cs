using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Windows;
using thinger.WPF.MultiTHMonitorBLL;
using thinger.WPF.MultiTHMonitorModels.SQL;
using thinger.WPF.MultiTHMonitorProject.Command;
using thinger.WPF.MultiTHMonitorProject.ViewModels;
using thinger.WPF.MultiTHMonitorProject.Views;

namespace thinger.WPF.MultiTHMonitorProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private SysLogManage sysLogManager = new SysLogManage();
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        //使用依赖注入，注入对应的视图模块
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<MonitorView, MonitorViewModel>();
            containerRegistry.RegisterForNavigation<ParamSetView, ParamSetViewModel>();
            containerRegistry.RegisterForNavigation<RecipeView, RecipeViewModel>();
            containerRegistry.RegisterForNavigation<AlarmView, AlarmViewModel>();
            containerRegistry.RegisterForNavigation<HistoryView, HistoryViewModel>();
            containerRegistry.RegisterForNavigation<UserManageView, UserManageViewModel>();
            containerRegistry.RegisterDialog<GroupConifgView, GroupConfigViewModel>();
            containerRegistry.RegisterDialog<VariableConfigView, VariableConfigViewModel>();
            containerRegistry.RegisterDialog<ModifyParamSetView, ModifyParamSetViewModel>();
        }
        /// <summary>
        /// 重写此方法，把MainView当主窗体显示，而LoginView当弹窗显示
        /// </summary>
        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            dialog.ShowDialog("LoginView", callback =>
            {
                if (callback.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }
            });
            //通过自定义的配置服务接口，获取需要显示的内容和用户名
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if (service!=null)
            {
                service.Configure();
            }
            base.OnInitialized();
        }
        private void AddLog()
        {
            SysLog sysLog = new SysLog()
            {
                InsertTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Operator = CommonMethods.CurrentAdmin.LoginName,
                Note =$"{CommonMethods.CurrentAdmin.LoginName} {DateTime.Now.ToString("yyyy - MM - dd HH:mm: ss")}登录系统"
            };
            sysLogManager.AddSysLog(sysLog);
        }
    }
}
