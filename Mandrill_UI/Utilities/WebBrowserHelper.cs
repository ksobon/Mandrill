using EO.WebBrowser.Wpf;
using System.Windows;

namespace Mandrill.ChromeWindow
{
    /// <summary>
    ///     Attached property class that allows for property binding on WebBrowser control.
    /// </summary>
    class WebBrowserHelper
    {
        public static readonly DependencyProperty BodyProperty =
            DependencyProperty.RegisterAttached("Body", typeof(string), typeof(WebBrowserHelper), new PropertyMetadata(OnBodyChanged));

        public static string GetBody(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(BodyProperty);
        }

        public static void SetBody(DependencyObject dependencyObject, string body)
        {
            dependencyObject.SetValue(BodyProperty, body);
        }

        private static void OnBodyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var webBrowser = (WebControl)d;
            string value;
            if ((string)e.NewValue == string.Empty || (string)e.NewValue == null)
            {
                value = @"nbsp;";
            }
            else
            {
                value = (string)e.NewValue;
            }
            webBrowser.WebView.LoadHtml(value);
        }
    }
}