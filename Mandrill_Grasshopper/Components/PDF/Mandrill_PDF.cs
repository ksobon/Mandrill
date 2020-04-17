using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using HtmlAgilityPack;
using System.Windows;

namespace Mandrill_Grasshopper.Components.PDF
{
    public class Mandrill_PDF : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_PDF class.
        /// </summary>
        public Mandrill_PDF()
          : base("PrintToPdf", "Pdf",
              "PDF object.",
              Resources.CategoryName, Resources.SubCategoryPDF)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FilePath", "P", Resources.Data_FilePathDesc, GH_ParamAccess.item);
            pManager.AddGenericParameter("Report", "R", Resources.Report_ReportDesc, GH_ParamAccess.item);
            pManager.AddGenericParameter("Style", "S", Resources.PDF_StyleDesc, GH_ParamAccess.item);
            pManager.AddBooleanParameter("Print", "P", Resources.Report_ShowWindowDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            D3jsLib.PdfStyle style = null;
            string filePath = null;
            D3jsLib.Report report = null;
            var print = false;

            if (!DA.GetData(0, ref filePath)) return;
            if (!DA.GetData(1, ref report)) return;
            if (!DA.GetData(2, ref style)) return;
            if (!DA.GetData(3, ref print)) return;

            if (print)
            {
                PrintPDF(report, style, filePath);
            }
        }

        private void PrintPDF(D3jsLib.Report report, D3jsLib.PdfStyle style, string filePath)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(report.HtmlString);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='gridster-box']");
            foreach (var n in nodes)
            {
                n.InnerHtml = "";
            }

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

            try
            {
                // convert html to document object and save
                var pdfDoc = converter.ConvertHtmlString(htmlDoc.DocumentNode.InnerHtml);
                pdfDoc.Save(filePath);
                pdfDoc.Close();
            }
            catch
            {
                MessageBox.Show("Printing failed. Is file open in another application?");
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Resources.Mandrill_Print_MandrillPrintNodeModel;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{fc79eff1-c14a-4462-9d05-fcc7e7c3c296}"); }
        }
    }
}