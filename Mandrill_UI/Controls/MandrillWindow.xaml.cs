namespace Mandrill.ChromeWindow
{
    /// <summary>
    /// Interaction logic for MandrillWindow.xaml
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

            // set webbroser options
            //options.AllowJavaScript = false;
            //options.DefaultEncoding = System.Text.Encoding.UTF8;
            // (Konrad) These options are critical for the app to work. 
            // We can load d3.js file and other resource only if security is disabled.
            var options = new EO.WebEngine.BrowserOptions
            {
                EnableWebSecurity = false
            };
            browser.WebView.SetOptions(options);

            // attach window to dynamo
            Owner = MandrillWindowNodeModel.Dv;

            // add closing event
            Closing += MandrillWindowNodeModel.OnWindowClosing;
        }
    }
}
