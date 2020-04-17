using System;
using System.Drawing;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib;
using D3jsLib.ParallelCoordinates;
using D3jsLib.Utilities;

namespace Mandrill_Grasshopper.Components.ParallelCoordinates
{
    public class Mandrill_ParallelCoordinatesStyle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_ParallelCoordinatesStyle class.
        /// </summary>
        public Mandrill_ParallelCoordinatesStyle()
          : base("ParallelCoordinatesStyle", "Style",
              "Parallel Coordinates Style.",
              Resources.CategoryName, Resources.SubCategory_Multivariate)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("LineColor", "LC", Resources.Style_LineColorDesc, GH_ParamAccess.item, Color.FromArgb(50, 130, 190));
            pManager.AddGenericParameter("Address", "A", Resources.Style_AddressDesc, GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Margins", "M", Resources.Style_MarginsDesc, GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Width", "W", Resources.Style_WidthDesc, GH_ParamAccess.item, 1000);
            pManager.AddIntegerParameter("Height", "H", Resources.Style_HeightDesc, GH_ParamAccess.item, 500);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Style", "S", Resources.Chart_StyleDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var lineColor = Color.FromArgb(50, 130, 190);
            var address = new GridAddress(1, 1);
            var margins = new Margins();
            var width = 1000;
            var height = 500;

            DA.GetData(0, ref lineColor);
            DA.GetData(1, ref address);
            DA.GetData(2, ref margins);
            DA.GetData(3, ref width);
            DA.GetData(4, ref height);

            // create style
            var style = new ParallelCoordinatesStyle();
            style.LineColor = ChartsUtilities.ColorToHexString(lineColor);
            style.GridRow = address.X;
            style.GridColumn = address.Y;
            style.Width = width;
            style.Height = height;
            style.Margins = margins;
            style.SizeX = (int)Math.Ceiling(width / 100d);
            style.SizeY = (int)Math.Ceiling(height / 100d);

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
                return Resources.Charts_ParallelCoordinates_Style;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{9c2144fb-a9bd-4c68-b16f-dec1c9d57617}"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }
    }
}