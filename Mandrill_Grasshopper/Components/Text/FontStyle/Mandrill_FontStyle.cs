using Grasshopper.Kernel;
using System;
using Mandrill_Resources.Properties;

namespace Mandrill_Grasshopper.Components.PDF
{
    public class Mandrill_FontStyle : Mandrill_BaseDropdown
    {
        public Mandrill_FontStyle() :
            base("FontStyle", "Style",
                "List of availabel font styles.",
                Resources.CategoryName, Resources.SubCategory_Text)
        {
            m_userItems = Mandrill_Grasshopper.Utilities.Utilities.EnumGetItems<D3jsLib.Text.FontStyle>();
        }

        public override void CreateAttributes()
        {
            this.m_attributes = new Mandrill_FontStyleAttributes(this);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        public override System.Guid ComponentGuid
        {
            get { return new Guid("{588272aa-8d8f-499b-bc3d-05d1ad542fd9}"); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return Resources.Mandrill_UI_tn_FontStyle_tn_FontStyle; }
        }
    }
}