using Grasshopper.Kernel;
using System;
using Mandrill_Resources.Properties;
using SelectPdf;

namespace Mandrill_Grasshopper.Components.PDF
{
    public class Mandrill_PdfSize : Mandrill_BaseDropdown
    {
        public Mandrill_PdfSize() :
            base("PdfSize", "Size",
                "List of available page sizes for printing PDFs.",
                Resources.CategoryName, Resources.SubCategoryPDF)
        {
            m_userItems = Mandrill_Grasshopper.Utilities.Utilities.EnumGetItems<PdfPageSize>();
        }

        public override void CreateAttributes()
        {
            this.m_attributes = new Mandrill_PdfSizeAttributes(this);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        public override System.Guid ComponentGuid
        {
            get { return new Guid("{615e8789-2c25-442c-8348-02b3824d7364}"); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return Resources.Mandrill_UI_pdf_SizeUI_pdf_SizeUI; }
        }
    }
}