using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DumbDownloader.Utils
{
    // https://youtu.be/An7kwDYt3OQ : 까불이 코더 Attached Property 영상 참조

    /** 사용법
     * <PasswordBox 
     *  utils:PasswordBoxHelper.BoundPassword="{Binding InputPassword, Mode=TwoWay}" />
     *  여기서 InputPassword 는 ViewModel 에 정의된 사용자 property
     */
    public class PasswordBoxHelper
    {
        // Using a DependencyProperty as the backing store for BoundPassword.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword", 
                typeof(string), // 이 Attached Property 의 타입
                typeof(PasswordBoxHelper), 
                new PropertyMetadata("<default>", OnBoundPasswordChanged)); // 변경되면 불릴 callback

        public static string GetBoundPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject obj, string value)
        {
            obj.SetValue(BoundPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            if (passwordBox == null)
                return;

            passwordBox.PasswordChanged -= PasswordChanged;
            string newString = (string)e.NewValue ?? string.Empty; // ?? 은 lhs가 null 이면 오른쪽 실행.
            if (newString != passwordBox.Password)
            {
                passwordBox.Password = newString;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            SetBoundPassword(passwordBox, passwordBox.Password);
        }
    }
}
