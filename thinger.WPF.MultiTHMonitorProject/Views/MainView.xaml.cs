using Prism.Events;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using thinger.WPF.MultiTHMonitorModels;
using thinger.WPF.MultiTHMonitorModels.SQL;
using thinger.WPF.MultiTHMonitorProject.Command;
using thinger.WPF.MultiTHMonitorProject.Events;

namespace thinger.WPF.MultiTHMonitorProject.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        #region 定义变量字段
        private DispatcherTimer timer = new DispatcherTimer();
        //private readonly IEventAggregator eventAggregator;
        #endregion


        public MainView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            #region  //直接给最小、最大、关闭按钮绑定事件

            //btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            //btnMax.Click += (s, e) =>
            //{
            //    if (this.WindowState == WindowState.Maximized)
            //    {
            //        this.WindowState = WindowState.Normal;
            //    }
            //    else
            //    {
            //        this.WindowState = WindowState.Maximized;
            //    }

            //};
            btnClose.Click += (s, e) =>
            {
                this.Close();
            };
            MoveColorZone.MouseMove += (s, e) =>
            {
                //判断鼠标正在拖动的过程中
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();//允许移动窗口
            };
            //添加一个双击放大事件
            //MoveColorZone.MouseDoubleClick += (s, e) =>
            //{
            //    if (this.WindowState == WindowState.Normal)
            //        this.WindowState = WindowState.Maximized;
            //    else
            //        this.WindowState = WindowState.Normal;
            //};
            #endregion
            //SysAdmin sysAdmin = CommonMethods.CurrentAdmin;
            //this.txtAdminName.Text = sysAdmin.LoginName;

            //获取到参数设置视图页面中发布的订阅消息
            eventAggregator.GetEvent<DeviceMessageEvent>().Subscribe(DevSubMessage);
            #region 启用定时器刷新时间
            //系统时间
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += StoreTimer_Elapsed;
            timer.Start();
            #endregion
        }

        private void DevSubMessage(object obj)
        {
            Device device = (Device)obj;
            if (device != null)
            {
                if (device.IsConnected)
                {
                    SolidColorBrush solidColorBrush = new SolidColorBrush();
                    solidColorBrush.Color = Color.FromRgb(1, 192, 200);
                    this.ConnectStatusLed.Background = solidColorBrush;

                    DropShadowEffect dropShadowEffect = new DropShadowEffect();
                    dropShadowEffect.Color = Color.FromRgb(1, 192, 200);
                    dropShadowEffect.ShadowDepth = 0;
                    dropShadowEffect.BlurRadius = 10;
                    this.ConnectStatusLed.Effect = dropShadowEffect;
                }
                else
                {
                    SolidColorBrush solidColorBrush = new SolidColorBrush();
                    solidColorBrush.Color = Colors.Red;
                    this.ConnectStatusLed.Background = solidColorBrush;

                    DropShadowEffect dropShadowEffect = new DropShadowEffect();
                    dropShadowEffect.Color = Colors.Red;
                    dropShadowEffect.ShadowDepth = 0;
                    dropShadowEffect.BlurRadius = 10;
                    this.ConnectStatusLed.Effect = dropShadowEffect;
                }
            }
        }

        private string[] weeks = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
        private string week
        {
            get { return weeks[Convert.ToInt32(DateTime.Now.DayOfWeek)]; }
        }
        private void StoreTimer_Elapsed(object sender, EventArgs e)
        {
            //更新时间及通信状态
            this.SysTime.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("HH:mm:ss") + " " + week;

            //CommonDataMethods.StoreTimer();

        }
    }
}
