using System.Collections.Generic;
using D3jsLib;

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
        /// <returns name="pdfStyle">PDF Style settings.</returns>
        /// <search>pdf, style, settings, pdf style</search>
        public static D3jsLib.PdfStyle PDFStyle(
            string pdfSize, 
            string pdfOrientation, 
            string pdfHorizontalFit,
            string pdfVerticalFit,
            int compression=10, 
            int marginTop=0, 
            int marginRight=0, 
            int marginBottom=0, 
            int marginLeft=0)
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
        /// 
        /// </summary>
        /// <param name="report"></param>
        /// <param name="pdfStyle"></param>
        /// <param name="filePath"></param>
        public static void SaveAsPDF(D3jsLib.Report report, D3jsLib.PdfStyle pdfStyle, string filePath)
        {
            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
            
            // set converter options
            SelectPdf.HtmlToPdfOptions options = converter.Options;
            options.PdfPageOrientation = pdfStyle.Orientation;
            options.PdfPageSize = pdfStyle.Size;
            options.JpegCompressionLevel = pdfStyle.Compression;
            options.JavaScriptEnabled = true;
            options.EmbedFonts = true;
            options.KeepImagesTogether = true;
            options.KeepTextsTogether = true;
            options.AutoFitHeight = pdfStyle.VerticalFit;
            options.AutoFitWidth = pdfStyle.HorizontalFit;
            options.MarginTop = pdfStyle.MarginTop;
            options.MarginRight = pdfStyle.MarginRight;
            options.MarginBottom = pdfStyle.MarginBottom;
            options.MarginLeft = pdfStyle.MarginLeft;
            
            // convert html to document object
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(report.HtmlString);
            doc.Save(filePath);
            doc.Close();
        }
    }
}

