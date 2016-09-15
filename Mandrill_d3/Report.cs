using SelectPdf;

namespace D3jsLib
{
    public class Report
    {
        public string HtmlString { get; set; }

        public Report(string _htmlString)
        {
            this.HtmlString = _htmlString;
        }
    }

    public class PdfStyle
    {
        public PdfPageSize Size;
        public PdfPageOrientation Orientation;
        public int Compression;
        public int MarginTop;
        public int MarginRight;
        public int MarginBottom;
        public int MarginLeft;
        public HtmlToPdfPageFitMode VerticalFit;
        public HtmlToPdfPageFitMode HorizontalFit;

        public PdfStyle()
        {
        }
    }
}
