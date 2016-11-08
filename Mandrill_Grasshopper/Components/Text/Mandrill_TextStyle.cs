using System;
using System.Drawing;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib;

namespace Mandrill_Grasshopper.Components.Text
{
    public class Mandrill_TextStyle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_TextStyle class.
        /// </summary>
        public Mandrill_TextStyle()
          : base("TextStyle", "Style",
              "Text style object.",
              Resources.CategoryName, Resources.SubCategory_Text)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("FontColor", "C", Resources.Style_FontColorDesc, GH_ParamAccess.item, Color.FromArgb(0, 0, 0));
            pManager.AddGenericParameter("Address", "A", Resources.Style_AddressDesc, GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("Width", "W", Resources.Style_WidthDesc, GH_ParamAccess.item, 200);
            pManager.AddIntegerParameter("Height", "H", Resources.Style_HeightDesc, GH_ParamAccess.item, 100);
            pManager.AddNumberParameter("FontSize", "S", Resources.Style_FontSizeDesc, GH_ParamAccess.item, 20.0);
            pManager.AddTextParameter("FontWeight", "FW", Resources.Style_FontWeightDesc, GH_ParamAccess.item, "normal");
            pManager.AddTextParameter("FontStyle", "FS", Resources.Style_FontStyleDesc, GH_ParamAccess.item, "normal");
            pManager.AddTextParameter("FontTransform", "FT", Resources.Style_FontTransformDesc, GH_ParamAccess.item, "none");
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Style", "S", Resources.Text_StyleDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Color fontColor = Color.FromArgb(0, 0, 0);
            GridAddress address = new GridAddress(1,1);
            int width = 200;
            int height = 100;
            double fontSize = 20.0;
            string fontWeight = "normal";
            string fontStyle = "normal";
            string fontTransform = "none";

            DA.GetData<Color>(0, ref fontColor);
            DA.GetData<GridAddress>(1, ref address);
            DA.GetData<int>(2, ref width);
            DA.GetData<int>(3, ref height);
            DA.GetData<double>(4, ref fontSize);
            DA.GetData<string>(5, ref fontWeight);
            DA.GetData<string>(6, ref fontStyle);
            DA.GetData<string>(7, ref fontTransform);

            TextStyle style = new TextStyle();
            style.FontSize = fontSize;
            style.FontColor = fontColor;
            style.FontWeight = fontWeight;
            style.FontStyle = fontStyle;
            style.FontTransform = fontTransform;
            style.Width = width;
            style.Height = height;
            style.GridRow = address.X;
            style.GridColumn = address.Y;

            DA.SetData(0, style);
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
                return Resources.Text_Text_Style;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{28d44f23-593b-4067-a828-f6c241e86527}"); }
        }
    }
}