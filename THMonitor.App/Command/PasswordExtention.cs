using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace thinger.WPF.MultiTHMonitorProject.Command
{
    /// <summary>
    /// 这个专门用于处理密码框不支持数据绑定的方式
    /// 通过给他添加一个扩展类，实现附加属性的方式进行数据绑定操作
    /// </summary>
    public class PasswordExtention
    {
        //添加附加属性方式的快捷键是propa+tab
        public static string GetPassword (DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //OnPasswordPropertyChanged:属性变更事件
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordExtention), new PropertyMetadata(string.Empty,OnPasswordPropertyChanged));

        /// <summary>
        /// 密码属性变更事件
        /// </summary>
        /// <param name="dependencyObject">依赖对象</param>
        /// <param name="eventArgs">变更的事件参数</param>
        private static void OnPasswordPropertyChanged(DependencyObject dependencyObject,DependencyPropertyChangedEventArgs eventArgs)
        {
            //依赖对象转换为密码框
            var password = dependencyObject as PasswordBox;
            //获取参数中的新值并转换为字符串
            string pwd=eventArgs.NewValue as string;

            //如果新值不为空，并且密码框中的密码不等于新知，那么就把pwd赋值给密码框依赖对象中密码属性的值。
            if (pwd!=null&&password.Password!=pwd)
            {
                password.Password = pwd;
            }
        }
    }
    /// <summary>
    /// 密码的行为
    /// </summary>
    public class PasswordBehavior:Behavior<PasswordBox>
    {
        /// <summary>
        /// 重写OnAttached此方法
        /// 此方法是在行为附加到关联对象后调用
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            //关联对象事件绑定，触发密码变化的操作。
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
        }

        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            //通过密码扩展类获取到密码的值
            string password = PasswordExtention.GetPassword(passwordBox);
            //判断密码框对象和密码是否为空，如果不为空，将其设置给密码扩展对象
            if (passwordBox!=null && passwordBox.Password!=password)
            {
                PasswordExtention.SetPassword(passwordBox, passwordBox.Password);
            }
        }

        /// <summary>
        /// 重写OnDetaching此方法
        /// 当行为与关联对象分离时调用。
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
        }
    }
}
