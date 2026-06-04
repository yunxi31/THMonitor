using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace thinger.WPF.MultiTHMonitorProject.Command
{
    public class CheckBoxStyle: DependencyObject
    {
        public static double GetIconSize(DependencyObject obj)
        {
            return (double)obj.GetValue(IconSizeProperty);
        }

        public static void SetIconSize(DependencyObject obj, double value)
        {
            obj.SetValue(IconSizeProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.RegisterAttached("IconSize", typeof(double), typeof(CheckBoxStyle), new PropertyMetadata(30d));
    }

}
