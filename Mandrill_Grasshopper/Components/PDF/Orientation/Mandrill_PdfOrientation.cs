using Grasshopper.Kernel;
using System;
using Mandrill_Resources.Properties;
using SelectPdf;

namespace Mandrill_Grasshopper.Components.PDF
{
    public class Mandrill_PdfOrientation : Mandrill_BaseDropdown
    {
        public Mandrill_PdfOrientation() :
            base("PdfOrientation", "Orientation",
                "List of available Orientation modes for printing PDFs.",
                Resources.CategoryName, Resources.SubCategoryPDF)
        {
            m_userItems = Mandrill_Grasshopper.Utilities.Utilities.EnumGetItems<PdfPageOrientation>();
        }

        public override void CreateAttributes()
        {
            this.m_attributes = new Mandrill_PdfOrientationAttributes(this);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        public override System.Guid ComponentGuid
        {
            get { return new Guid("{021e1b77-24e2-416e-9ee7-38ca91c01c60}"); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return Resources.Mandrill_UI_pdf_OrientationUI_pdf_OrientationUI; }
        }
    }
}