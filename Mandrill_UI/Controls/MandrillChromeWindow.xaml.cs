
namespace Mandrill.ChromeWindow
{
    /// <summary>
    /// Interaction logic for MandrillChromeWindow.xaml
    /// </summary>
    public partial class MandrillChromeWindow : System.Windows.Window
    {
        /// <summary>
        /// 
        /// </summary>
        public MandrillChromeWindow()
        {
            InitializeComponent();

            // set webbroser options
            EO.WebEngine.BrowserOptions options = new EO.WebEngine.BrowserOptions();
            options.AllowJavaScript = true;
            options.EnableWebSecurity = false;
            options.DefaultEncoding = System.Text.Encoding.UTF8;

            this.browser.WebView.SetOptions(options);

            // attach window to dynamo
            this.Owner = MandrillChromeWindowNodeModel.dv;

            // add closing event
            this.Closing += MandrillChromeWindowNodeModel.OnWindowClosing;
        }
    }
}
