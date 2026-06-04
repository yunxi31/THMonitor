using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thinger.WPF.MultiTHMonitorProject.Charts
{
    [ObservableObject]
    public partial class DashBoardModel
    {
        public IEnumerable<ISeries> Series { get; set; }
       = new GaugeBuilder()
       .WithLabelsSize(50)
       .WithInnerRadius(75)
       .WithBackgroundInnerRadius(75)
       .WithBackground(new SolidColorPaint(new SKColor(100, 181, 246, 90)))
       .WithLabelsPosition(PolarLabelsPosition.ChartCenter)
       .AddValue(30, "gauge value", SKColors.YellowGreen, SKColors.Red) // defines the value and the color 
       .BuildSeries();
    }
}
