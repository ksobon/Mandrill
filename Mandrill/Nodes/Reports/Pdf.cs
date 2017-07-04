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
    }
}

