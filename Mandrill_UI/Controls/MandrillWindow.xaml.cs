using System;
using System.IO;
using System.Reflection;

namespace Mandrill.ChromeWindow
{
    /// <summary>
    /// Interaction logic for MandrillWindow.xaml
    /// </summary>
    public partial class MandrillWindow
    {
        /// <summary>
        /// Mandrill Window class.
        /// </summary>
        public MandrillWindow()
        {
            InitializeComponent();

            // make a license request
            Authentication.License.RequestLicense();

            // set child process location (preferred location somewhere where user has access and won't be stopped by anti-virus)
            EO.Base.Runtime.InitWorkerProcessExecutable(Path.Combine(AssemblyDirectory, "eowp.exe"));

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

        private static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
