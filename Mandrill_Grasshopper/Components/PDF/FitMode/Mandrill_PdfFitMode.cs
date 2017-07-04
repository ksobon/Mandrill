using Grasshopper.Kernel;
using System;
using Mandrill_Resources.Properties;
using SelectPdf;

namespace Mandrill_Grasshopper.Components.PDF
{
    public class Mandrill_PdfFitMode : Mandrill_BaseDropdown
    {
        public Mandrill_PdfFitMode() :
            base("PdfFitMode", "FitMode",
                "List of available Fit Modes for printing PDFs.",
                Resources.CategoryName, Resources.SubCategoryPDF)
        {
            m_userItems = Mandrill_Grasshopper.Utilities.Utilities.EnumGetItems<HtmlToPdfPageFitMode>();
        }

        public override void CreateAttributes()
        {
            this.m_attributes = new Mandrill_PdfFitModeAttributes(this);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        public override System.Guid ComponentGuid
        {
            get { return new Guid("{c27e0d12-5547-4cbe-bbac-44633656f0ca}"); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return Resources.Mandrill_UI_pdf_FitModeUI_pdf_FitModeUI; }
        }
    }
}