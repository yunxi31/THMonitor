using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Regions;
using System.Diagnostics;
using thinger.WPF.MultiTHMonitorProject.Command;
using thinger.WPF.MultiTHMonitorModels.SQL;
using thinger.WPF.MultiTHMonitorBLL;
using Prism.Commands;
using Microsoft.Win32;
using Prism.Services.Dialogs;
using MiniExcelLibs;
using System.IO;
using System.Windows.Media.Imaging;
using LiveChartsCore.SkiaSharpView.WPF;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace thinger.WPF.MultiTHMonitorProject.ViewModels
{
    public class HistoryViewModel:BindableBase
    {
        public HistoryViewModel()
        {
            QueryByTimeCommand = new DelegateCommand(ExeQueryByTime);
            QueryByFastCommand = new DelegateCommand(ExeQueryByFast);
            SavePicCommand = new DelegateCommand<CartesianChart>(ExeSavePicture);
            SaveExcelCommand = new DelegateCommand<CartesianChart>(ExeSaveExcel);
            ShowCommand = new DelegateCommand(GetLines);
        }

        #region 命令属性
        public DelegateCommand QueryByTimeCommand { get; set; }
        public DelegateCommand QueryByFastCommand { get; set; }
        public DelegateCommand<CartesianChart> SavePicCommand { get; set; }
        public DelegateCommand<CartesianChart> SaveExcelCommand { get; set; }
        public DelegateCommand ShowCommand { get; private set; }
        #endregion

        #region 成员变量
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
        private static List<string> HistoryTimes=new List<string>();
        #endregion

        #region 属性
        private List<ActualData> actualDatas;

        public List<ActualData> ActualDatas
        {
            get { return actualDatas; }
            set { actualDatas = value; RaisePropertyChanged(); }
        }
        //private List<string> times;

        //public List<string> Times
        //{
        //    get { return times; }
        //    set { times = value; }
        //}
        private DateTime startNowDate = DateTime.Now;

        public DateTime StartNowDate
        {
            get { return startNowDate; }
            set
            {
                startNowDate = value;
                RaisePropertyChanged();
            }
        }
        private DateTime startNowTime = DateTime.Now;

        public DateTime StartNowTime
        {
            get { return startNowTime; }
            set { startNowTime = value; RaisePropertyChanged(); }
        }
        private DateTime endNowDate = DateTime.Now;

        public DateTime EndNowDate
        {
            get { return endNowDate; }
            set { endNowDate = value; RaisePropertyChanged(); }
        }
        private DateTime endNowTime = DateTime.Now;

        public DateTime EndNowTime
        {
            get { return endNowTime; }
            set { endNowTime = value; RaisePropertyChanged(); }
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
            Color = new SKColor(0,0, 0),
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
        {
            Values = Values_Hum01,
            Fill = null,
            GeometrySize = 8,
            GeometryStroke = new SolidColorPaint(new SKColor(24, 180, 187, 255)),
            LineSmoothness = 1,

            Stroke = new SolidColorPaint(new SKColor(24, 180, 187, 255)) { StrokeThickness = 3 },
            Name = "1#站点湿度"

        };
        private static LineSeries<double> lineSeriesHum01 = new LineSeries<double>
        {
            Values = Values_Temp01,
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

        #region 方法
        private void ExeQueryByTime()
        {
            string start = StartNowDate.ToString("yyyy-MM-dd") + StartNowTime.ToString(" hh:mm:ss");
            string end = EndNowDate.ToString("yyyy-MM-dd") + EndNowTime.ToString(" hh:mm:ss");
            GetActualDatas(start,end);
            GetXAxis();
            GetData();
        }
        private void ExeQueryByFast()
        {
            string start = DateTime.Now.AddHours(-2.0f).ToString();
            string end = DateTime.Now.ToString();
            GetActualDatas(start, end);
            GetXAxis();
            GetData();
        }
        private void ExeSavePicture(CartesianChart chart)
        {
            
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "图片文件(*.jpg)|*.jpg|所有文件|*.*";
            saveFileDialog.FileName = "趋势保存图片" + DateTime.Now.ToString("yyyyMMddHHmmss");

            saveFileDialog.Title = "历史趋势保存图片";
            saveFileDialog.DefaultExt = "jpg";
            saveFileDialog.AddExtension = true;
            //Nullable<bool> result = saveFileDialog.ShowDialog();
            FileStream fs = new FileStream($"{saveFileDialog.FileName}", FileMode.Create);
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)chart.ActualWidth + 10, (int)chart.ActualHeight + 10, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(chart);

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            //保存到路径中
            encoder.Save(fs);
            //释放资源
            fs.Close();
            fs.Dispose();
            MessageBox.Show("保存成功");
            
            Process.Start(new ProcessStartInfo { UseShellExecute = true, FileName = saveFileDialog.FileName });
            
        }
        private void ExeSaveExcel(CartesianChart obj)
        {
            string start = StartNowDate.ToString("yyyy-MM-dd") + StartNowTime.ToString(" hh:mm:ss");
            string end = EndNowDate.ToString("yyyy-MM-dd") + EndNowTime.ToString(" hh:mm:ss");
            List<ActualData> actualDatas=  GetActualDatas(start, end);

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "CSV文件(*.csv)|*.csv|所有文件|*.*";
            saveFileDialog.FileName = "趋势保存图片" + DateTime.Now.ToString("yyyyMMddHHmmss");

            saveFileDialog.Title = "历史趋势保存CSV";
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.AddExtension = true;

            MiniExcel.SaveAs(saveFileDialog.FileName, actualDatas, excelType: ExcelType.CSV);
            Process.Start(new ProcessStartInfo { UseShellExecute = true, FileName = saveFileDialog.FileName });
          
        }
        public List<ActualData> GetActualDatas(string startTime,string endTime)
        {
            ActualDatas = new ActualDataManage().QueryActualDataByTime(startTime,endTime);
            return ActualDatas;
        }
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
            if (ActualDatas!=null)
            {
                foreach (var item in ActualDatas)
                {
                    HistoryTimes.Add(item.InsertTime);
                }
            }
           
            return HistoryTimes;
        }
        public void GetData()
        {
            if (ActualDatas!=null)
            {
                foreach (var item in ActualDatas)
                {
                    Values_Temp01.Add(Convert.ToDouble(item.Station1Temp));
                    Values_Hum01.Add(Convert.ToDouble(item.Station1Humidity));

                    Values_Temp02.Add(Convert.ToDouble(item.Station2Temp));
                    Values_Hum02.Add(Convert.ToDouble(item.Station2Humidity));

                    Values_Temp03.Add(Convert.ToDouble(item.Station3Temp));
                    Values_Hum03.Add(Convert.ToDouble(item.Station3Humidity));

                    Values_Temp04.Add(Convert.ToDouble(item.Station4Temp));
                    Values_Hum04.Add(Convert.ToDouble(item.Station4Humidity));

                    Values_Temp05.Add(Convert.ToDouble(item.Station5Temp));
                    Values_Hum05.Add(Convert.ToDouble(item.Station5Humidity));

                    Values_Temp06.Add(Convert.ToDouble(item.Station6Temp));
                    Values_Hum06.Add(Convert.ToDouble(item.Station6Humidity));
                }
            }
            
        }
      
      
       
        #endregion

    }
}
