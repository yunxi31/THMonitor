using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace thinger.WPF.MultiTHMonitorProject.ViewModels
{
    public class PromptViewModel : BindableBase, IDialogAware
    {
        public PromptViewModel()
        {
            ConfirmCommand = new DelegateCommand(Confirm);
        }

        #region 命令属性
        public DelegateCommand ConfirmCommand { get; set; }
        #endregion

        #region 视图属性
        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; RaisePropertyChanged(); }
        }

        private string title = "提示";
        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 弹窗会话接口实现
        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Message"))
            {
                Message = parameters.GetValue<string>("Message");
            }
            if (parameters.ContainsKey("Title"))
            {
                Title = parameters.GetValue<string>("Title");
            }
        }

        private void Confirm()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }
        #endregion
    }
}
