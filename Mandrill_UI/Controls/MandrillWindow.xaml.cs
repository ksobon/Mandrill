
namespace Mandrill.ChromeWindow
{
    /// <summary>
    ///     Interaction logic for MandrillWindow.xaml
    /// </summary>
    public partial class MandrillWindow : System.Windows.Window
    {
        /// <summary>
        ///     Mandrill Window class.
        /// </summary>
        public MandrillWindow()
        {
            InitializeComponent();

            // make a license request
            Mandrill.Authentication.License.RequestLicense();

            // set webbroser options
            //options.AllowJavaScript = false;
            //options.DefaultEncoding = System.Text.Encoding.UTF8;
            EO.WebEngine.BrowserOptions options = new EO.WebEngine.BrowserOptions();
            options.EnableWebSecurity = false;
            this.browser.WebView.SetOptions(options);

            // attach window to dynamo
            this.Owner = MandrillWindowNodeModel.dv;

            // add closing event
            this.Closing += MandrillWindowNodeModel.OnWindowClosing;
        }
    }
}
