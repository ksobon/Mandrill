using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib;

namespace Mandrill_Grasshopper.Components.Image
{
    public class Mandrill_ImageStyle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_ImageStyle class.
        /// </summary>
        public Mandrill_ImageStyle()
          : base("ImageStyle", "Style",
              "Image Style object.",
              Resources.CategoryName, Resources.SubCategoryImage)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Address", "A", Resources.Style_AddressDesc, GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddIntegerParameter("Width", "W", Resources.Style_WidthDesc, GH_ParamAccess.item, 150);
            pManager.AddIntegerParameter("Height", "H", Resources.Style_HeightDesc, GH_ParamAccess.item, 150);
            pManager.AddTextParameter("Tooltip", "T", Resources.Style_ImageTooltipDesc, GH_ParamAccess.item, "");
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Style", "S", Resources.Image_StyleDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var address = new GridAddress(1, 1);
            var width = 150;
            var height = 150;
            var tooltip = "";

            DA.GetData(0, ref address);
            DA.GetData(1, ref width);
            DA.GetData(2, ref height);
            DA.GetData(3, ref tooltip);

            var style = new ImageStyle();
            style.Width = width;
            style.Height = height;
            style.Tooltip = tooltip;
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
                return Resources.Image_Image_Style;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{1e411a69-9bec-4863-8532-883778be24d8}"); }
        }
    }
}