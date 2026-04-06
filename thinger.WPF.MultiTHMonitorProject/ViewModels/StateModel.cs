using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Prism.Mvvm;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;

namespace thinger.WPF.MultiTHMonitorProject.ViewModels
{
    public class StateModel:BindableBase
    {
        public string Title { get; set; }
       
        public string TempVarName { get; set; }

        public string Temp { get; set; }

        public string HumidityVarName { get; set; }

        public bool ModuleError { get; set; }

        public string Hum { get; set; }
       
        public string StateVarName { get; set; }


        /// <summary>
        /// 仪表盘属性
        /// </summary>
        //public IEnumerable<ISeries> Series { get; set; }
        
        //private IEnumerable<ISeries> series;

        //public IEnumerable<ISeries> Series { get; set; }

        private IEnumerable<ISeries> series;

        public IEnumerable<ISeries> Series
        {
            get { return series; }
            set { series = value; RaisePropertyChanged(); }
        }


    }
}
