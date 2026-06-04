using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinger.WPF.MultiTHMonitorProject.Command;

namespace thinger.WPF.MultiTHMonitorProject.ViewModels
{
    public class ModifyParamSetViewModel : BindableBase, IDialogAware
    {
        public ModifyParamSetViewModel()
        {
            ConfirmCommand = new DelegateCommand(Confirm);
            CancelCommand = new DelegateCommand(Cancel);
        }
        #region 命令属性
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand ConfirmCommand { get; set; }
        #endregion

        #region 视图属性
        private string siteName;

        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; RaisePropertyChanged(); }
        }

        private string siteValue;

        public string SiteValue
        {
            get { return siteValue; }
            set { siteValue = value; RaisePropertyChanged(); }
        }

        private string newSiteValue="0.0";

        public string NewSiteValue
        {
            get { return newSiteValue; }
            set { newSiteValue = value; RaisePropertyChanged(); }
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
            keys.Add("ParamViewValue1", SiteName);
            keys.Add("ParamViewValue2", NewSiteValue);
            var result = CommonMethods.CommonWrite(SiteName, NewSiteValue);
            if (result)
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, keys));
            }
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("ShowVariableParam");
            SiteName = parameters.GetValue<string>("ShowSiteName");
            SiteValue= parameters.GetValue<string>("ShowSiteValue");
        }
        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        private void Confirm()
        {
            OnDialogClosed();
        }

       

      
        #endregion
    }
}
