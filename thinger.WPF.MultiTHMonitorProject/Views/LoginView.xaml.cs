using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using thinger.WPF.MultiTHMonitorProject.ViewModels;

namespace thinger.WPF.MultiTHMonitorProject.Views
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : UserControl
    {
        //鼠标是否按下
        bool _isMouseDown = false;
        //鼠标按下的位置
        Point _mousePosition;
        //鼠标按下控件的Margin
        //Thickness _mouseThickness;
        public LoginView()
        {
            InitializeComponent();
            //ColorZone.MouseMove += (s, e) =>
            //{
            //    if (e.LeftButton == MouseButtonState.Pressed)
            //    {
                    
            //    }
            //        //this.DragMove();

            //};
            //关闭登录窗体
            //btnClose.Click += (s, e) =>
            //{
            //    this.Close();
            //};
            ////移动登录窗体
            //LoginBox.MouseMove += (s, e) =>
            //{
            //    if (e.LeftButton == MouseButtonState.Pressed)
            //    {
            //        this.DragMove();
            //    }
            //};

            //Login.MouseMove += delegate (object sender_d, MouseEventArgs e_d)
            //{
            //    if (e_d.LeftButton == MouseButtonState.Pressed)
            //    {
            //        if (e_d.MouseDevice.Target is Control)
            //            return;
            //        this.DragMove();
            //    }

            //};

        }
        //protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonDown(e);

        //    // 获取鼠标相对标题栏位置  
        //    Point position = e.GetPosition(this.bTop);

        //    // 如果鼠标位置在标题栏内，允许拖动  
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        if (position.X >= 0 && position.X < bTop.ActualWidth && position.Y >= 0 && position.Y < bTop.ActualHeight)
        //        {
        //            this.DragMove();
        //        }
        //    }
        //}

        //private void DragMove()
        //{
        //    if (Login.WindowState == WindowState.Normal)
        //    {
        //        SendMessage(hs.Handle, WM_SYSCOMMAND, (IntPtr)0xf012, IntPtr.Zero);
        //        SendMessage(hs.Handle, WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
        //    }
        //}

        #region 软键盘功能
        private void StartKeyBoard()
        {
            //打开软键盘
            try
            {
                if (!System.IO.File.Exists(Environment.SystemDirectory + "\\osk.exe"))
                {
                    MessageBox.Show("软件盘可执行文件不存在！");
                    return;
                }

                softKey = System.Diagnostics.Process.Start(Environment.SystemDirectory + "\\osk.exe");
                // 上面的语句在打开软键盘后，系统还没用立刻把软键盘的窗口创建出来了。所以下面的代码用循环来查询窗口是否创建，只有创建了窗口
                // FindWindow才能找到窗口句柄，才可以移动窗口的位置和设置窗口的大小。这里是关键。
                IntPtr intptr = IntPtr.Zero;
                while (IntPtr.Zero == intptr)
                {
                    System.Threading.Thread.Sleep(100);
                    intptr = FindWindow(null, "屏幕键盘");
                }


                // 获取屏幕尺寸
                int iActulaWidth = (int)SystemParameters.WorkArea.Size.Width;
                int iActulaHeight = (int)SystemParameters.WorkArea.Size.Height;


                // 设置软键盘的显示位置，底部居中
                int posX = (iActulaWidth - 1000) / 2;
                int posY = (iActulaHeight - 300);


                //设定键盘显示位置
                MoveWindow(intptr, posX, posY, 1000, 300, true);


                //设置软键盘到前端显示
                SetForegroundWindow(intptr);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void PasswordBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StartKeyBoard();
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StartKeyBoard();
        }
        // 申明要使用的dll和api
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "MoveWindow")]
        public static extern bool MoveWindow(System.IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);


        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);


        private System.Diagnostics.Process softKey;

        Point anchorPoint;
        Point currentPoint;
        bool isInDrag = false;
        private TranslateTransform transform = new TranslateTransform();
        private void root_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isInDrag)
            {
                var element = sender as FrameworkElement;
                currentPoint = e.GetPosition(null);

                var transform = new TranslateTransform
                {
                    X = (currentPoint.X - anchorPoint.X),
                    Y = (currentPoint.Y - anchorPoint.Y)
                };
                this.RenderTransform = transform;
                anchorPoint = currentPoint;
            }
        }

        private void root_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isInDrag)
            {
                var element = sender as FrameworkElement;
                currentPoint = e.GetPosition(null);

                var transform = new TranslateTransform
                {
                    X = (currentPoint.X - anchorPoint.X),
                    Y = (currentPoint.Y - anchorPoint.Y)
                };
                this.RenderTransform = transform;
                anchorPoint = currentPoint;
            }
        }

        private void root_MouseMove(object sender, MouseEventArgs e)
        {
            if (isInDrag)
            {
                var element = sender as FrameworkElement;
                currentPoint = e.GetPosition(null);

                transform.X += currentPoint.X - anchorPoint.X;
                transform.Y += (currentPoint.Y - anchorPoint.Y);
                this.RenderTransform = transform;
                anchorPoint = currentPoint;
            }
        }






        #endregion
        // private bool IsMouseDown = false;
        // Point CPos;
        // private void B2_OnMouseDown(object sender, MouseButtonEventArgs e)
        // {
        //     UIElement ui = (UIElement)sender;
        //     IsMouseDown = true;
        //     ui.CaptureMouse();
        // }
        //private void ui2_MouseMove(object sender, MouseEventArgs e)
        // {
        //     Grid b = (Grid)sender;
        //     if (!IsMouseDown)
        //         return;
        //     UIElement parent = (UIElement)b.Parent;
        //     e.Handled = true;
        // }
        // private void B2_OnMouseUp(object sender, MouseButtonEventArgs e)
        // {
        //     IsMouseDown = false;
        //     UIElement ui = (UIElement)sender;
        //     ui.ReleaseMouseCapture();
        //     e.Handled = true;
        // }
       
    }

}
