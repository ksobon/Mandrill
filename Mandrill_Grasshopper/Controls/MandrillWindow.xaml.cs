using HtmlAgilityPack;
using System;

namespace Mandrill_Grasshopper.Components.Report
{
    public partial class MandrillWindow : System.Windows.Window
    {
        public MandrillWindow()
        {
            InitializeComponent();

            // make a license request
            Mandrill.Authentication.License.RequestLicense();

            // set webbroser options
            EO.WebEngine.BrowserOptions options = new EO.WebEngine.BrowserOptions();
            //options.AllowJavaScript = false;
            options.EnableWebSecurity = false;
            //options.DefaultEncoding = System.Text.Encoding.UTF8;

            this.browser.WebView.SetOptions(options);


        }

        /// <summary>
        ///     Print method.
        /// </summary>
        public void Print(string filePath, D3jsLib.PdfStyle style)
        {
            EO.WebBrowser.WebView view = this.browser.WebView;
            if (view != null)
            {
                string htmlCode = view.GetHtml();

                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlCode);

                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='gridster-box']");
                if (nodes != null)
                {
                    foreach (HtmlNode n in nodes)
                    {
                        n.InnerHtml = "";
                    }

                    // attempt to move *dep file
                    D3jsLib.Utilities.ChartsUtilities.MoveDepFile();

                    // create converter
                    SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

                    // set converter options
                    SelectPdf.HtmlToPdfOptions options = converter.Options;
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
                    string finalFilePath = filePath;

                    Uri uri = new Uri(filePath);
                    string absoluteFilePath = Uri.UnescapeDataString(uri.AbsoluteUri);

                    if (Uri.IsWellFormedUriString(absoluteFilePath, UriKind.RelativeOrAbsolute))
                    {
                        Uri newUri = new Uri(absoluteFilePath);
                        finalFilePath = newUri.LocalPath;
                    }

                    // convert html to document object and save
                    SelectPdf.PdfDocument pdfDoc = converter.ConvertHtmlString(htmlDoc.DocumentNode.InnerHtml);
                    pdfDoc.Save(finalFilePath);
                    pdfDoc.Close();
                }
                else
                {
                    
                }
            }
        }
    }
}
