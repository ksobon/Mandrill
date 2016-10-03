using D3jsLib;
using System;

namespace Report
{
    /// <summary>
    ///     Report Class
    /// </summary>
    public class Pdf
    {
        internal Pdf()
        {
        }

        /// <summary>
        ///     PDF Settings that control document page size, orientation etc
        /// </summary>
        /// <param name="pdfSize">Size of the generated PDF Document. Example: Letter, 11x17 etc.</param>
        /// <param name="pdfOrientation">Orientation for the generated PDF document. Example: Landscape.</param>
        /// <param name="pdfHorizontalFit">Defines if Report will be resized (width) to automatically fit onto specified page size.</param>
        /// <param name="pdfVerticalFit">Defines if Report will be resized (height) to automatically fit onto specified page size.</param>
        /// <param name="compression">Value between 0-100 where 0 is best image quality while 100 ensures highest compression and lowest image quality.</param>
        /// <param name="marginTop">Margin value in points where 1 point is 1/72 inches.</param>
        /// <param name="marginRight">Margin value in points where 1 point is 1/72 inches.</param>
        /// <param name="marginBottom">Margin value in points where 1 point is 1/72 inches.</param>
        /// <param name="marginLeft">Margin value in points where 1 point is 1/72 inches.</param>
        /// <returns name="Style">PDF Style settings.</returns>
        /// <search>pdf, style, settings, pdf style</search>
        public static D3jsLib.PdfStyle Style(
            string pdfSize = "Letter11x17", 
            string pdfOrientation = "Landscape", 
            string pdfHorizontalFit = "AutoFit",
            string pdfVerticalFit = "AutoFit",
            int compression = 1, 
            int marginTop = 0, 
            int marginRight = 0, 
            int marginBottom = 0, 
            int marginLeft = 0)
        {
            PdfStyle style = new PdfStyle();
            style.Size = (SelectPdf.PdfPageSize)System.Enum.Parse(typeof(SelectPdf.PdfPageSize), pdfSize);
            style.Orientation = (SelectPdf.PdfPageOrientation)System.Enum.Parse(typeof(SelectPdf.PdfPageOrientation), pdfOrientation);
            style.VerticalFit = (SelectPdf.HtmlToPdfPageFitMode)System.Enum.Parse(typeof(SelectPdf.HtmlToPdfPageFitMode), pdfVerticalFit);
            style.HorizontalFit = (SelectPdf.HtmlToPdfPageFitMode)System.Enum.Parse(typeof(SelectPdf.HtmlToPdfPageFitMode), pdfHorizontalFit);
            style.Compression = compression;
            style.MarginTop = marginTop;
            style.MarginRight = marginRight;
            style.MarginBottom = marginBottom;
            style.MarginLeft = marginLeft;

            return style;
        }

        /// <summary>
        ///     Save as PDF
        /// </summary>
        /// <param name="report"></param>
        /// <param name="style">Style</param>
        /// <param name="filePath">File path that PDF will be saved at.</param>
        /// <returns name="Void">Void Return</returns>
        public static void SaveAs(D3jsLib.Report report, D3jsLib.PdfStyle style, string filePath)
        {
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
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(report.HtmlString);
            doc.Save(finalFilePath);
            doc.Close();
        }
    }
}

