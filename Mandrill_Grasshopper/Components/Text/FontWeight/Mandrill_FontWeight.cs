using Grasshopper.Kernel;
using System;
using Mandrill_Resources.Properties;

namespace Mandrill_Grasshopper.Components.PDF
{
    public class Mandrill_FontWeight : Mandrill_BaseDropdown
    {
        public Mandrill_FontWeight() : 
            base("FontWeight", "Weight", 
                "List of availabel font weights.", 
                Resources.CategoryName, Resources.SubCategory_Text)
        {
            m_userItems = Mandrill_Grasshopper.Utilities.Utilities.EnumGetItems<D3jsLib.Text.FontWeights>();
        }

        public override void CreateAttributes()
        {
            this.m_attributes = new Mandrill_FontWeightAttributes(this);
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }

        public override System.Guid ComponentGuid
        {
            get { return new Guid("{a43ce3d4-927c-45a6-abbd-38870243e365}"); }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return Resources.Mandrill_UI_tn_FontWeight_tn_FontWeight; }
        }
    }
}