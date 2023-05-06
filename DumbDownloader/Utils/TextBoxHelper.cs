using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DumbDownloader.Utils
{
    // https://youtu.be/An7kwDYt3OQ : 까불이 코더 Attached Property 만들기 강의
    // Binding 시 한글 바로 출력하기를 Attached Property 로 구현하기
    public class TextBoxHelper
    {
        // 
        public static readonly DependencyProperty UseOnPropertyChangedProperty =
            DependencyProperty.RegisterAttached("UseOnPropertyChanged", 
                typeof(bool), // 이 Property 의 타입
                typeof(TextBoxHelper), new PropertyMetadata(false, OnUseOnPropertyChanged));


        #region Publics
        // propa 누르고 tab-tab 을 누르면 자동으로 Attached property 골격 코드가 만들어 진다.
        public static bool GetUseOnPropertyChanged(DependencyObject obj)
        {
            return (bool)obj.GetValue(UseOnPropertyChangedProperty);
        }

        public static void SetUseOnPropertyChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(UseOnPropertyChangedProperty, value);
        }
        #endregion


        #region Privates
        private static void OnUseOnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox == null)
                return;

            if ((bool)e.NewValue)
            {// 새로운 값이 들어오면 TextBox_TextChanged() 를 delegate 에 등록
                textBox.TextChanged += TextBox_TextChanged;
            }
            else
            {// 새로운 값이 없으면 delegate 삭제
                textBox.TextChanged -= TextBox_TextChanged;
            }
        }

        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            BindingExpression bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty); // TextBox.Text 에 적용된 바인딩 얻기
            bindingExpression?.UpdateSource();
        }
        #endregion
    }
}
