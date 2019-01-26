namespace Mandrill.ChromeWindow
{
    /// <summary>
    ///     Interaction logic for MandrillWindow.xaml
    /// </summary>
    public partial class MandrillWindow
    {
        /// <summary>
        ///     Mandrill Window class.
        /// </summary>
        public MandrillWindow()
        {
            InitializeComponent();

            // make a license request
            Authentication.License.RequestLicense();

            // set WebBrowser options
            //options.AllowJavaScript = false;
            //options.DefaultEncoding = System.Text.Encoding.UTF8;
            EO.WebEngine.BrowserOptions options = new EO.WebEngine.BrowserOptions();
            options.EnableWebSecurity = false;
            browser.WebView.SetOptions(options);

            // attach window to dynamo
            Owner = MandrillWindowNodeModel.Dv;

            // add closing event
            Closing += MandrillWindowNodeModel.OnWindowClosing;
        }
    }
}
