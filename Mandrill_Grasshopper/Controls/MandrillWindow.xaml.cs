using HtmlAgilityPack;
using System;
using System.IO;
using System.Reflection;

namespace Mandrill_Grasshopper.Components.Report
{
    public partial class MandrillWindow
    {
        public MandrillWindow()
        {
            InitializeComponent();

            // make a license request
            Mandrill.Authentication.License.RequestLicense();

            try
            {
                // set child process location (preferred location somewhere where user has access and won't be stopped by anti-virus)
                EO.Base.Runtime.InitWorkerProcessExecutable(Path.Combine(AssemblyDirectory, "eowp.exe"));
            }
            catch
            {
                // ignore
            }

            // set WebBrowser options
            var options = new EO.WebEngine.BrowserOptions
            {
                EnableWebSecurity = false
            };

            browser.WebView.SetOptions(options);
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

        /// <summary>
        ///     Print method.
        /// </summary>
        public void Print(string filePath, D3jsLib.PdfStyle style)
        {
            var view = browser.WebView;
            if (view != null)
            {
                var htmlCode = view.GetHtml();

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlCode);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='gridster-box']");
                if (nodes != null)
                {
                    foreach (var n in nodes)
                    {
                        n.InnerHtml = "";
                    }

                    // attempt to move *dep file
                    D3jsLib.Utilities.ChartsUtilities.MoveDepFile();

                    // create converter
                    var converter = new SelectPdf.HtmlToPdf();

                    // set converter options
                    var options = converter.Options;
                    options.PdfPageOrientation = style.Orientation;
                    options.PdfPageSize = style.Size;
                    options.JpegCompressionLevel = style.Compression;
                    options.JavaScriptEnabled = true;
                    options.EmbedFonts = true;
                    options.KeepImagesTogether = true;
                    options.KeepTextsTogether = true;
                    options.AutoFitHeight = style.VerticalFit;
                    options.AutoFitWidth = style.HorizontalFit;
                    options.MarginTop = style.MarginTop;
                    options.MarginRight = style.MarginRight;
                    options.MarginBottom = style.MarginBottom;
                    options.MarginLeft = style.MarginLeft;

                    // created unescaped file path removes %20 from path etc.
                    var finalFilePath = filePath;

                    var uri = new Uri(filePath);
                    var absoluteFilePath = Uri.UnescapeDataString(uri.AbsoluteUri);

                    if (Uri.IsWellFormedUriString(absoluteFilePath, UriKind.RelativeOrAbsolute))
                    {
                        var newUri = new Uri(absoluteFilePath);
                        finalFilePath = newUri.LocalPath;
                    }

                    // convert html to document object and save
                    var pdfDoc = converter.ConvertHtmlString(htmlDoc.DocumentNode.InnerHtml);
                    pdfDoc.Save(finalFilePath);
                    pdfDoc.Close();
                }
            }
        }
    }
}
