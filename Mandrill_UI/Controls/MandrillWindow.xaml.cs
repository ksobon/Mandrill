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

            // set webbroser options
            EO.WebEngine.BrowserOptions options = new EO.WebEngine.BrowserOptions();
            //options.AllowJavaScript = false;
            options.EnableWebSecurity = false;
            //options.DefaultEncoding = System.Text.Encoding.UTF8;

            this.browser.WebView.SetOptions(options);

            // attach window to dynamo
            this.Owner = MandrillWindowNodeModel.dv;

            // add closing event
            this.Closing += MandrillWindowNodeModel.OnWindowClosing;
        }
    }
}
