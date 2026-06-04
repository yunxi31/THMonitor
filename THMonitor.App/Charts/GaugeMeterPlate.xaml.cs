using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace thinger.WPF.MultiTHMonitorProject.Charts
{
    /// <summary>
    /// GaugeMeterPlate.xaml 的交互逻辑
    /// </summary>
    public partial class GaugeMeterPlate : UserControl
    {
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(GaugeMeterPlate),
                new PropertyMetadata(default(int), new PropertyChangedCallback(OnValuePropertyChanged)));


        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(GaugeMeterPlate),
                new PropertyMetadata(double.NaN, new PropertyChangedCallback(OnPropertyChanged)));


        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(GaugeMeterPlate),
                new PropertyMetadata(double.NaN, new PropertyChangedCallback(OnPropertyChanged)));


        public Brush PlateBackground
        {
            get { return (Brush)GetValue(PlateBackgroundProperty); }
            set { SetValue(PlateBackgroundProperty, value); }
        }
        public static readonly DependencyProperty PlateBackgroundProperty =
            DependencyProperty.Register("PlateBackground", typeof(Brush), typeof(GaugeMeterPlate), null);


        public Brush PlateBorderBrush
        {
            get { return (Brush)GetValue(PlateBorderBrushProperty); }
            set { SetValue(PlateBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty PlateBorderBrushProperty =
            DependencyProperty.Register("PlateBorderBrush", typeof(Brush), typeof(GaugeMeterPlate), null);


        public Thickness PlateBorderThickness
        {
            get { return (Thickness)GetValue(PlateBorderThicknessProperty); }
            set { SetValue(PlateBorderThicknessProperty, value); }
        }
        public static readonly DependencyProperty PlateBorderThicknessProperty =
            DependencyProperty.Register("PlateBorderThickness", typeof(Thickness), typeof(GaugeMeterPlate), null);


        public static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as GaugeMeterPlate).DrawScale();
        }

        public static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as GaugeMeterPlate).DrawAngle();
        }

        public GaugeMeterPlate()
        {
            InitializeComponent();

            Loaded += MeterPlate_Loaded;
        }

        private void MeterPlate_Loaded(object sender, RoutedEventArgs e)
        {
            this.DrawScale();
        }

        /// <summary>
        /// 画表盘的刻度
        /// </summary>
        private void DrawScale()
        {
            this.canvasPlate.Children.Clear();

            for (double i = 0; i <= this.Maximum - this.Minimum; i++)
            {
                //添加刻度线
                Line lineScale = new Line();

                if (i % 10 == 0)
                {
                    //注意Math.Cos和Math.Sin的参数是弧度，记得将角度转为弧度制
                    lineScale.X1 = 200 - 170 * Math.Cos(i * (270 / (this.Maximum - this.Minimum)) * Math.PI / 180);
                    lineScale.Y1 = 200 - 170 * Math.Sin(i * (270 / (this.Maximum - this.Minimum)) * Math.PI / 180);
                    lineScale.Stroke = new SolidColorBrush(Colors.White);
                    lineScale.StrokeThickness = 3;

                    //添加刻度值
                    TextBlock txtScale = new TextBlock();
                    txtScale.Text = (i + this.Minimum).ToString();
                    txtScale.Width = 34;
                    txtScale.TextAlignment = TextAlignment.Center;
                    txtScale.Foreground = new SolidColorBrush(Colors.White);
                    txtScale.RenderTransform = new RotateTransform() { Angle = 45, CenterX = 17, CenterY = 8 };
                    txtScale.FontSize = 18;

                    Canvas.SetLeft(txtScale, 200 - 155 * Math.Cos(i * (270 / (this.Maximum - this.Minimum)) * Math.PI / 180) - 17);
                    Canvas.SetTop(txtScale, 200 - 155 * Math.Sin(i * (270 / (this.Maximum - this.Minimum)) * Math.PI / 180) - 10);

                    this.canvasPlate.Children.Add(txtScale);
                }
                else
                {
                    lineScale.X1 = 200 - 180 * Math.Cos(i * (270 / (this.Maximum - this.Minimum)) * Math.PI / 180);
                    lineScale.Y1 = 200 - 180 * Math.Sin(i * (270 / (this.Maximum - this.Minimum)) * Math.PI / 180);
                    lineScale.Stroke = new SolidColorBrush(Colors.White);
                    lineScale.StrokeThickness = 1;
                }

                lineScale.X2 = 200 - 190 * Math.Cos(i * (270 / (this.Maximum - this.Minimum)) * Math.PI / 180);
                lineScale.Y2 = 200 - 190 * Math.Sin(i * (270 / (this.Maximum - this.Minimum)) * Math.PI / 180);

                this.canvasPlate.Children.Add(lineScale);
            }
        }

        private void DrawAngle()
        {
            double step = 270.0 / (this.Maximum - this.Minimum);
            DoubleAnimation da = new DoubleAnimation(this.Value * step - 45, new Duration(TimeSpan.FromMilliseconds(200)));
            this.rtPointer.BeginAnimation(RotateTransform.AngleProperty, da);
        }

    }
}
