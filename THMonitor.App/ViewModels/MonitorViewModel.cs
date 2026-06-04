using Collapsenav.Net.Tool;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using thinger.DataConvertLib;
using thinger.WPF.MultiTHMonitorBLL;
using thinger.WPF.MultiTHMonitorModels.SQL;
using thinger.WPF.MultiTHMonitorProject.Command;

namespace thinger.WPF.MultiTHMonitorProject.ViewModels
{
    public class MonitorViewModel : BindableBase, INavigationAware
    {
        public MonitorViewModel()
        {
            //GetTimes();
            //GetXAxis();
            ShowCommand = new DelegateCommand(GetLines);
            _timer.Tick += UpdateTimer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
            
        }

        public DelegateCommand ShowCommand { get; private set; }
        #region 成员变量
        private DispatcherTimer _timer = new DispatcherTimer();
        //private static ObservableCollection<string> Times { get; set; } = new ObservableCollection<string>();
        private static List<string> Times = new List<string>();
        private static List<double> Values_Temp01 = new List<double>();
        private static List<double> Values_Hum01 = new List<double>();
        private static List<double> Values_Temp02 = new List<double>();
        private static List<double> Values_Hum02 = new List<double>();
        private static List<double> Values_Temp03 = new List<double>();
        private static List<double> Values_Hum03 = new List<double>();
        private static List<double> Values_Temp04 = new List<double>();
        private static List<double> Values_Hum04 = new List<double>();
        private static List<double> Values_Temp05 = new List<double>();
        private static List<double> Values_Hum05 = new List<double>();
        private static List<double> Values_Temp06 = new List<double>();
        private static List<double> Values_Hum06 = new List<double>();
        #endregion

        #region 视图属性
        private List<OperateLog> logList;

        public List<OperateLog> LogList
        {
            get { return logList; }
            set { logList = value; RaisePropertyChanged(); }
        }

        #region 仪表盘变量
        private float stateTemp01;

        public float StateTemp01
        {
            get { return stateTemp01; }
            set { stateTemp01 = value; RaisePropertyChanged(); }
        }
        private float stateHum01;

        public float StateHum01
        {
            get { return stateHum01; }
            set { stateHum01 = value; RaisePropertyChanged(); }
        }

        private float stateTemp02;

        public float StateTemp02
        {
            get { return stateTemp02; }
            set { stateTemp02 = value; RaisePropertyChanged(); }
        }
        private float stateHum02;

        public float StateHum02
        {
            get { return stateHum02; }
            set { stateHum02 = value; RaisePropertyChanged(); }
        }
        private float stateTemp03;

        public float StateTemp03
        {
            get { return stateTemp03; }
            set { stateTemp03 = value; RaisePropertyChanged(); }
        }
        private float stateHum03;

        public float StateHum03
        {
            get { return stateHum03; }
            set { stateHum03 = value; RaisePropertyChanged(); }
        }
        private float stateTemp04;

        public float StateTemp04
        {
            get { return stateTemp04; }
            set { stateTemp04 = value; RaisePropertyChanged(); }
        }
        private float stateHum04;

        public float StateHum04
        {
            get { return stateHum04; }
            set { stateHum04 = value; RaisePropertyChanged(); }
        }
        private float stateTemp05;

        public float StateTemp05
        {
            get { return stateTemp05; }
            set { stateTemp05 = value; RaisePropertyChanged(); }
        }
        private float stateHum05;

        public float StateHum05
        {
            get { return stateHum05; }
            set { stateHum05 = value; RaisePropertyChanged(); }
        }
        private float stateTemp06;

        public float StateTemp06
        {
            get { return stateTemp06; }
            set { stateTemp06 = value; RaisePropertyChanged(); }
        }
        private float stateHum06;

        public float StateHum06
        {
            get { return stateHum06; }
            set { stateHum06 = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 柱状图图表变量
        //private List<double> stateTemp01Arr;

        //public List<double> StateTemp01Arr
        //{
        //    get { return stateTemp01Arr; }
        //    set { stateTemp01Arr = value; RaisePropertyChanged(); }
        //}

        #endregion
        //public List<StateModel> StateModels { get; set; } = new List<StateModel>
        //{
        //     new StateModel(){Title="1#站点",TempVarName="模块1温度",HumidityVarName="模块1湿度",StateVarName="模块1异常",Temp="温度:0.0℃",Hum="湿度:0.0%",ModuleError=false,Series=new GaugeBuilder()
        //    .WithLabelsSize(0)
        //    .WithInnerRadius(100)
        //    .WithBackgroundInnerRadius(100)
        //    .WithBackground(new SolidColorPaint(new SKColor(207, 216, 217, 255)))
        //    .AddValue(10, "Temp", new SKColor(251, 138, 105, 255), new SKColor(0, 183, 193, 255))
        //    .AddValue(20, "Hum", new SKColor(0, 183, 193, 255)) // defines the value and the color
        //    .BuildSeries()},
        //   new StateModel(){Title="2#站点",TempVarName="模块2温度",HumidityVarName="模块2湿度",StateVarName="模块2异常",Temp="温度:0.0℃",Hum="湿度:0.0%",ModuleError=false,Series=new GaugeBuilder()
        //    .WithLabelsSize(0)
        //    .WithInnerRadius(100)
        //    .WithBackgroundInnerRadius(100)
        //    .WithBackground(new SolidColorPaint(new SKColor(207, 216, 217, 255)))
        //    .AddValue(10, "Temp", new SKColor(251, 138, 105, 255), new SKColor(0, 183, 193, 255))
        //    .AddValue(20, "Hum", new SKColor(0, 183, 193, 255)) // defines the value and the color
        //    .BuildSeries()},
        //   new StateModel(){Title="3#站点",TempVarName="模块3温度",HumidityVarName="模块3湿度",StateVarName="模块3异常",Temp="温度:0.0℃",Hum="湿度:0.0%",ModuleError=false,Series=new GaugeBuilder()
        //    .WithLabelsSize(0)
        //    .WithInnerRadius(100)
        //    .WithBackgroundInnerRadius(100)
        //    .WithBackground(new SolidColorPaint(new SKColor(207, 216, 217, 255)))
        //    .AddValue(10, "Temp", new SKColor(251, 138, 105, 255), new SKColor(0, 183, 193, 255))
        //    .AddValue(20, "Hum", new SKColor(0, 183, 193, 255)) // defines the value and the color
        //    .BuildSeries()},
        //   new StateModel(){Title="4#站点",TempVarName="模块4温度",HumidityVarName="模块4湿度",StateVarName="模块4异常",Temp="温度:0.0℃",Hum="湿度:0.0%",ModuleError=false,Series=new GaugeBuilder()
        //    .WithLabelsSize(0)
        //    .WithInnerRadius(100)
        //    .WithBackgroundInnerRadius(100)
        //    .WithBackground(new SolidColorPaint(new SKColor(207, 216, 217, 255)))
        //    .AddValue(10, "Temp", new SKColor(251, 138, 105, 255), new SKColor(0, 183, 193, 255))
        //    .AddValue(20, "Hum", new SKColor(0, 183, 193, 255)) // defines the value and the color
        //    .BuildSeries()},
        //   new StateModel(){Title="5#站点",TempVarName="模块5温度",HumidityVarName="模块5湿度",StateVarName="模块5异常",Temp="温度:0.0℃",Hum="湿度:0.0%",ModuleError=false,Series=new GaugeBuilder()
        //    .WithLabelsSize(0)
        //    .WithInnerRadius(100)
        //    .WithBackgroundInnerRadius(100)
        //    .WithBackground(new SolidColorPaint(new SKColor(207, 216, 217, 255)))
        //    .AddValue(10, "Temp", new SKColor(251, 138, 105, 255), new SKColor(0, 183, 193, 255))
        //    .AddValue(20, "Hum", new SKColor(0, 183, 193, 255)) // defines the value and the color
        //    .BuildSeries()},
        //   new StateModel(){Title="6#站点",TempVarName="模块6温度",HumidityVarName="模块6湿度",StateVarName="模块6异常",Temp="温度:0.0℃",Hum="湿度:0.0%",ModuleError=false,Series=new GaugeBuilder()
        //    .WithLabelsSize(0)
        //    .WithInnerRadius(100)
        //    .WithBackgroundInnerRadius(100)
        //    .WithBackground(new SolidColorPaint(new SKColor(207, 216, 217, 255)))
        //    .AddValue(10, "Temp", new SKColor(251, 138, 105, 255), new SKColor(0, 183, 193, 255))
        //    .AddValue(20, "Hum", new SKColor(0, 183, 193, 255)) // defines the value and the color
        //    .BuildSeries()}
        //};
        
        #region 站点温湿度异常属性
        //private string temp_1= "温度:0.0℃";

        //public string Temp_1
        //{
        //    get { return temp_1; }
        //    set { temp_1= value; RaisePropertyChanged(); }
        //}
        //private string hum_1 = "湿度:0.0%";

        //public string Hum_1
        //{
        //    get { return hum_1; }
        //    set { hum_1 = value; RaisePropertyChanged(); }
        //}
        //private bool moduleError_1;

        //public bool ModuleError_1
        //{
        //    get { return moduleError_1; }
        //    set { moduleError_1 = value; RaisePropertyChanged(); }
        //}
        //private string temp_2 = "温度:0.0℃";

        //public string Temp_2
        //{
        //    get { return temp_2; }
        //    set { temp_2 = value; RaisePropertyChanged(); }
        //}
        //private string hum_2 = "湿度:0.0%";

        //public string Hum_2
        //{
        //    get { return hum_2; }
        //    set { hum_2 = value; RaisePropertyChanged(); }
        //}
        //private bool moduleError_2;

        //public bool ModuleError_2
        //{
        //    get { return moduleError_2; }
        //    set { moduleError_2 = value; RaisePropertyChanged(); }
        //}
        //private string temp_3 = "温度:0.0℃";

        //public string Temp_3
        //{
        //    get { return temp_3; }
        //    set { temp_3 = value; }
        //}
        //private string hum_3 = "湿度:0.0%";

        //public string Hum_3
        //{
        //    get { return hum_3; }
        //    set { hum_3 = value; RaisePropertyChanged(); }
        //}
        //private bool moduleError_3;

        //public bool ModuleError_3
        //{
        //    get { return moduleError_3; }
        //    set { moduleError_3 = value; RaisePropertyChanged(); }
        //}
        //private string temp_4 = "温度:0.0℃";

        //public string Temp_4
        //{
        //    get { return temp_4; }
        //    set { temp_4 = value; }
        //}
        //private string hum_4 = "湿度:0.0%";

        //public string Hum_4
        //{
        //    get { return hum_4; }
        //    set { hum_4 = value; RaisePropertyChanged(); }
        //}
        //private bool moduleError_4;

        //public bool ModuleError_4
        //{
        //    get { return moduleError_4; }
        //    set { moduleError_4 = value; RaisePropertyChanged(); }
        //}
        //private string temp_5 = "温度:0.0℃";

        //public string Temp_5
        //{
        //    get { return temp_5; }
        //    set { temp_5 = value; RaisePropertyChanged(); }
        //}
        //private string hum_5 = "湿度:0.0%";

        //public string Hum_5
        //{
        //    get { return hum_5; }
        //    set { hum_5 = value; RaisePropertyChanged(); }
        //}
        //private bool moduleError_5;

        //public bool ModuleError_5
        //{
        //    get { return moduleError_5; }
        //    set { moduleError_5 = value; RaisePropertyChanged(); }
        //}
        //private string temp_6 = "温度:0.0℃";

        //public string Temp_6
        //{
        //    get { return temp_6; }
        //    set { temp_6 = value; RaisePropertyChanged(); }
        //}
        //private string hum_6 = "湿度:0.0%";

        //public string Hum_6
        //{
        //    get { return hum_6; }
        //    set { hum_6 = value; RaisePropertyChanged(); }
        //}
        //private bool moduleError_6;

        //public bool ModuleError_6
        //{
        //    get { return moduleError_6; }
        //    set { moduleError_1 = value; RaisePropertyChanged(); }
        //}
        #endregion

        #endregion


        #region 复选框属性值
        private bool siteTemp01Show;

        public bool SiteTemp01Show
        {
            get { return siteTemp01Show; }
            set { siteTemp01Show = value; RaisePropertyChanged(); }
        }

        private bool siteHum01Show;

        public bool SiteHum01Show
        {
            get { return siteHum01Show; }
            set { siteHum01Show = value; RaisePropertyChanged(); }
        }

        private bool siteTemp02Show;

        public bool SiteTemp02Show
        {
            get { return siteTemp02Show; }
            set { siteTemp02Show = value; RaisePropertyChanged(); }
        }

        private bool siteHum02Show;

        public bool SiteHum02Show
        {
            get { return siteHum02Show; }
            set { siteHum02Show = value; RaisePropertyChanged(); }
        }

        private bool siteTemp03Show;

        public bool SiteTemp03Show
        {
            get { return siteTemp03Show; }
            set { siteTemp03Show = value; RaisePropertyChanged(); }
        }

        private bool siteHum03Show;

        public bool SiteHum03Show
        {
            get { return siteHum03Show; }
            set { siteHum03Show = value; RaisePropertyChanged(); }
        }

        private bool siteTemp04Show;

        public bool SiteTemp04Show
        {
            get { return siteTemp04Show; }
            set { siteTemp04Show = value; RaisePropertyChanged(); }
        }

        private bool siteHum04Show;

        public bool SiteHum04Show
        {
            get { return siteHum04Show; }
            set { siteHum04Show = value; RaisePropertyChanged(); }
        }

        private bool siteTemp05Show;

        public bool SiteTemp05Show
        {
            get { return siteTemp05Show; }
            set { siteTemp05Show = value; RaisePropertyChanged(); }
        }

        private bool siteHum05Show;

        public bool SiteHum05Show
        {
            get { return siteHum05Show; }
            set { siteHum05Show = value; RaisePropertyChanged(); }
        }

        private bool siteTemp06Show;

        public bool SiteTemp06Show
        {
            get { return siteTemp06Show; }
            set { siteTemp06Show = value; RaisePropertyChanged(); }
        }

        private bool siteHum06Show;

        public bool SiteHum06Show
        {
            get { return siteHum06Show; }
            set { siteHum06Show = value; RaisePropertyChanged(); }
        }
        #endregion

        #region 折线图表
        private List<ISeries> lineSeries;

        public List<ISeries> LineSeries
        {
            get { return lineSeries; }
            set { lineSeries = value; RaisePropertyChanged(); }
        }
        public SolidColorPaint TooltipTextPaint { get; set; } = new SolidColorPaint
        {
            Color = new SKColor(0, 0, 0),
            SKTypeface = SKTypeface.FromFamilyName("微软雅黑")
        };
        public SolidColorPaint LegendTextPaint { get; set; } =
       new SolidColorPaint
       {
           Color = new SKColor(250, 250, 250),
           SKTypeface = SKTypeface.FromFamilyName("微软雅黑")
       };
        #region 折线图数据
        private static LineSeries<double> lineSeriesTemp01 = new LineSeries<double>
        {     Values = Values_Hum01,
            Fill = null,
            GeometrySize = 8,
            GeometryStroke = new SolidColorPaint(new SKColor(24, 180, 187, 255)),
            LineSmoothness = 1,

            Stroke = new SolidColorPaint(new SKColor(24, 180, 187, 255)) { StrokeThickness = 3 },
            Name = "1#站点湿度"

        };
        private static LineSeries<double> lineSeriesHum01 = new LineSeries<double>
        {
            Values =Values_Temp01,
            Fill = null,
            GeometrySize = 8,
            GeometryStroke = new SolidColorPaint(new SKColor(251, 151, 118, 255)),
            LineSmoothness = 1,
            Stroke = new SolidColorPaint(new SKColor(251, 151, 118, 255)) { StrokeThickness = 3 },
            Name = "1#站点温度"
        };
        private static LineSeries<double> lineSeriesTemp02 = new LineSeries<double>
        {
            Values = Values_Hum02,
            Fill = null,
            GeometrySize = 4,
            LineSmoothness = 1,
            Stroke = new SolidColorPaint(new SKColor(150, 119, 206, 255)) { StrokeThickness = 3 },
            Name = "2#站点湿度"
        };
        private static LineSeries<double> lineSeriesHum02 = new LineSeries<double>
        {
            Values = Values_Temp02,
            Fill = null,
            GeometrySize = 4,
            LineSmoothness = 1,
            Stroke = new SolidColorPaint(new SKColor(150, 119, 206, 255)) { StrokeThickness = 3 },
            Name = "2#站点温度"
        };
        private static LineSeries<double> lineSeriesTemp03 = new LineSeries<double>
        {
            Values = Values_Hum02,
            Fill = null,
            GeometrySize = 4,
            LineSmoothness = 1,
            Stroke = new SolidColorPaint(new SKColor(239, 138, 124, 255)) { StrokeThickness = 3 },
            Name = "3#站点湿度"
        };
        private static LineSeries<double> lineSeriesHum03 = new LineSeries<double>
        {
            Values = Values_Temp03,
            Fill = null,
            GeometrySize = 4,
            LineSmoothness = 1,
            Stroke = new SolidColorPaint(new SKColor(255, 226, 2, 255)) { StrokeThickness = 3 },
            Name = "3#站点温度"
        };
        private static LineSeries<double> lineSeriesTemp04 = new LineSeries<double>
        {
            Values = Values_Hum04,
            Fill = null,
            GeometrySize = 4,
            LineSmoothness = 1,
            Stroke = new SolidColorPaint(new SKColor(225, 61, 75, 255)) { StrokeThickness = 3 },
            Name = "4#站点湿度"
        };
        private static LineSeries<double> lineSeriesHum04 = new LineSeries<double>
        {
            Values = Values_Temp04,
            Fill = null,
            GeometrySize = 4,
            LineSmoothness = 1,
            Stroke = new SolidColorPaint(new SKColor(0, 163, 138, 255)) { StrokeThickness = 3 },
            Name = "4#站点温度"
        };
        private static LineSeries<double> lineSeriesTemp05 = new LineSeries<double>
        {
            Values = Values_Hum05,
            Fill = null,
            GeometrySize = 4,
            LineSmoothness = 1,
            Stroke = new SolidColorPaint(new SKColor(24, 85, 181, 255)) { StrokeThickness = 3 },
            Name = "5#站点湿度"
        };
        private static LineSeries<double> lineSeriesHum05 = new LineSeries<double>
        {
            Values = Values_Temp05,
            Fill = null,
            GeometrySize = 4,
            LineSmoothness = 1,
            Stroke = new SolidColorPaint(new SKColor(63, 76, 150, 255)) { StrokeThickness = 3 },
            Name = "5#站点温度"
        };
        private static LineSeries<double> lineSeriesTemp06 = new LineSeries<double>
        {
            Values = Values_Hum06,
            Fill = null,
            GeometrySize = 4,
            LineSmoothness = 1,
            Stroke = new SolidColorPaint(new SKColor(80, 158, 40, 255)) { StrokeThickness = 3 },
            Name = "6#站点湿度"
        };
        private static LineSeries<double> lineSeriesHum06 = new LineSeries<double>
        {
            Values = Values_Temp06,
            Fill = null,
            GeometrySize = 4,
            LineSmoothness = 1,
            Stroke = new SolidColorPaint(new SKColor(188, 30, 131, 255)) { StrokeThickness = 3 },
            Name = "6#站点温度"
        };
        #endregion
     
        private static readonly SKColor s_gray = new(158, 199, 255);

        private Axis[] xAxes;

        public Axis[] XAxes
        {
            get { return xAxes; }
            set { xAxes = value; RaisePropertyChanged(); }
        } 
            
        public Axis[] YAxes { get; set; } =
        {
            new Axis
            {
               LabelsPaint = new SolidColorPaint(s_gray)
            }

        };

        #endregion

        #region 方法
        public void GetXAxis()
        {
            XAxes = new Axis[]
            {
                new Axis
                {
                    LabelsPaint = new SolidColorPaint(s_gray),
                    Labels=GetTimes()
                }
            };
        }
        public void GetLines()
        {
            LineSeries = new List<ISeries>();
            if (SiteTemp01Show)
            {
                LineSeries.Add(lineSeriesTemp01);
            }
            else
            {
                LineSeries.Remove(lineSeriesTemp01);
            }
            if (SiteHum01Show)
            {
                LineSeries.Add(lineSeriesHum01);
            }
            else
            {
                LineSeries.Remove(lineSeriesHum01);
            }
            if (SiteTemp02Show)
            {
                LineSeries.Add(lineSeriesTemp02);
            }
            else
            {
                LineSeries.Remove(lineSeriesTemp02);
            }
            if (SiteHum02Show)
            {
                LineSeries.Add(lineSeriesHum02);
            }
            else
            {
                LineSeries.Remove(lineSeriesHum02);
            }
            if (SiteTemp03Show)
            {
                LineSeries.Add(lineSeriesTemp03);
            }
            else
            {
                LineSeries.Remove(lineSeriesTemp03);
            }
            if (SiteHum03Show)
            {
                LineSeries.Add(lineSeriesHum03);
            }
            else
            {
                LineSeries.Remove(lineSeriesHum03);
            }
            if (SiteTemp04Show)
            {
                LineSeries.Add(lineSeriesTemp04);
            }
            else
            {
                LineSeries.Remove(lineSeriesTemp04);
            }
            if (SiteHum04Show)
            {
                LineSeries.Add(lineSeriesHum04);
            }
            else
            {
                LineSeries.Remove(lineSeriesHum04);
            }
            if (SiteTemp05Show)
            {
                LineSeries.Add(lineSeriesTemp05);
            }
            else
            {
                LineSeries.Remove(lineSeriesTemp05);
            }
            if (SiteHum05Show)
            {
                LineSeries.Add(lineSeriesHum05);
            }
            else
            {
                LineSeries.Remove(lineSeriesHum05);
            }
            if (SiteTemp06Show)
            {
                LineSeries.Add(lineSeriesTemp06);
            }
            else
            {
                LineSeries.Remove(lineSeriesTemp06);
            }
            if (SiteHum06Show)
            {
                LineSeries.Add(lineSeriesHum06);
            }
            else
            {
                LineSeries.Remove(lineSeriesHum06);
            }
        }
        public List<string> GetTimes()
        {
            Times.Add(DateTime.Now.ToString("HH:mm:ss"));
            if (Times.Count > 6)
            {
                Times.RemoveAt(0);
            }

            return Times;
        }
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (CommonMethods.Device.IsConnected)
            {
                UpdateTHMPieChartState01();
                UpdateTHMPieChartState02();
                UpdateTHMPieChartState03();
                UpdateTHMPieChartState04();
                UpdateTHMPieChartState05();
                UpdateTHMPieChartState06();
                GetXAxis();
            }
        }

        #region 【1】数据绑定
        private void UpdateTHMPieChartState01()
        {
            if (CommonMethods.Device.CurrentValue.ContainsKey("模块1温度"))
            {
               StateTemp01 = Convert.ToSingle(CommonMethods.Device["模块1温度"]);

                Values_Temp01.Add(Convert.ToDouble(StateTemp01));
                if (Values_Temp01.Count > 6)
                {
                    Values_Temp01.RemoveAt(0);
                }

            }
            if (CommonMethods.Device.CurrentValue.ContainsKey("模块1湿度"))
            {
                StateHum01 = Convert.ToSingle(CommonMethods.Device["模块1湿度"]);
                Values_Hum01.Add(Convert.ToDouble(StateHum01));
                if (Values_Hum01.Count > 6)
                {
                    Values_Hum01.RemoveAt(0);
                }
            }
        }
        private void UpdateTHMPieChartState02()
        {
            if (CommonMethods.Device.CurrentValue.ContainsKey("模块2温度"))
            {
                StateTemp02 = Convert.ToSingle(CommonMethods.Device["模块2温度"]);
                Values_Temp02.Add(Convert.ToDouble(StateTemp02));
                if (Values_Temp02.Count > 6)
                {
                    Values_Temp02.RemoveAt(0);
                }
            }
            if (CommonMethods.Device.CurrentValue.ContainsKey("模块2湿度"))
            {
                StateHum02 = Convert.ToSingle(CommonMethods.Device["模块2湿度"]);
                Values_Hum02.Add(Convert.ToDouble(StateHum02));
                if (Values_Hum02.Count > 6)
                {
                    Values_Hum02.RemoveAt(0);
                }
            }
        }
        private void UpdateTHMPieChartState03()
        {
            if (CommonMethods.Device.CurrentValue.ContainsKey("模块3温度"))
            {
                StateTemp03 = Convert.ToSingle(CommonMethods.Device["模块3温度"]);
                Values_Temp03.Add(Convert.ToDouble(StateTemp03));
                if (Values_Temp03.Count > 6)
                {
                    Values_Temp03.RemoveAt(0);
                }
            }
            if (CommonMethods.Device.CurrentValue.ContainsKey("模块3湿度"))
            {
                StateHum03 = Convert.ToSingle(CommonMethods.Device["模块3湿度"]);
                Values_Hum03.Add(Convert.ToDouble(StateHum03));
                if (Values_Hum03.Count > 6)
                {
                    Values_Hum03.RemoveAt(0);
                }
            }
        }
        private void UpdateTHMPieChartState04()
        {
            if (CommonMethods.Device.CurrentValue.ContainsKey("模块4温度"))
            {
                StateTemp04 = Convert.ToSingle(CommonMethods.Device["模块4温度"]);
                Values_Temp04.Add(Convert.ToDouble(StateTemp04));
                if (Values_Temp04.Count > 6)
                {
                    Values_Temp04.RemoveAt(0);
                }
            }
            if (CommonMethods.Device.CurrentValue.ContainsKey("模块4湿度"))
            {
                StateHum04= Convert.ToSingle(CommonMethods.Device["模块4湿度"]);
                Values_Hum04.Add(Convert.ToDouble(StateHum04));
                if (Values_Hum04.Count > 6)
                {
                    Values_Hum04.RemoveAt(0);
                }
            }
        }
        private void UpdateTHMPieChartState05()
        {
            if (CommonMethods.Device.CurrentValue.ContainsKey("模块5温度"))
            {
                StateTemp05 = Convert.ToSingle(CommonMethods.Device["模块5温度"]);
                Values_Temp05.Add(Convert.ToDouble(StateTemp05));
                if (Values_Temp05.Count > 6)
                {
                    Values_Temp05.RemoveAt(0);
                }
            }
            if (CommonMethods.Device.CurrentValue.ContainsKey("模块5湿度"))
            {
                StateHum05 = Convert.ToSingle(CommonMethods.Device["模块5湿度"]);
                Values_Hum05.Add(Convert.ToDouble(StateHum05));
                if (Values_Hum05.Count > 6)
                {
                    Values_Hum05.RemoveAt(0);
                }
            }
        }
        private void UpdateTHMPieChartState06()
        {
            if (CommonMethods.Device.CurrentValue.ContainsKey("模块6温度"))
            {
                StateTemp06 = Convert.ToSingle(CommonMethods.Device["模块6温度"]);
                Values_Temp06.Add(Convert.ToDouble(StateTemp06));
                if (Values_Temp06.Count > 6)
                {
                    Values_Temp06.RemoveAt(0);
                }
            }
            if (CommonMethods.Device.CurrentValue.ContainsKey("模块6湿度"))
            {
                StateHum06 = Convert.ToSingle(CommonMethods.Device["模块6湿度"]);
                Values_Hum06.Add(Convert.ToDouble(StateHum06));
                if (Values_Hum06.Count > 6)
                {
                    Values_Hum06.RemoveAt(0);
                }
            }
        }
        #endregion

        #region 导航获取参数
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("AddLogList"))
            {
                LogList = navigationContext.Parameters.GetValue<List<OperateLog>>("AddLogList");
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        #endregion


        #endregion
    }
}
