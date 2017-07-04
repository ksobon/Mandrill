using Grasshopper.Kernel;
using System;
using Mandrill_Resources.Properties;

namespace Mandrill_Grasshopper.Components.PDF
{
    public class Mandrill_FontTransform : Mandrill_BaseDropdown
    {
        public Mandrill_FontTransform() :
            base("FontTransform", "Transform",
                "List of availabel font transforms.",
                Resources.CategoryName, Resources.SubCategory_Text)
        {
            m_userItems = Mandrill_Grasshopper.Utilities.Utilities.EnumGetItems<D3jsLib.Text.FontTransform>();
        }

        public override void CreateAttributes()
        {
            this.m_attributes = new Mandrill_FontTransformAttributes(this);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        public override System.Guid ComponentGuid
        {
            get { return new Guid("{10fd1286-57a9-4f21-9e2f-b46e53295c2b}"); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return Resources.Mandrill_UI_tn_FontTransform_tn_FontTransform; }
        }
    }
}