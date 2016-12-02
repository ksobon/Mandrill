using System;
using System.Drawing;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib;
using D3jsLib.d3ScatterPlots;
using D3jsLib.Utilities;

namespace Mandrill_Grasshopper.Components.ScatterPlot
{
    public class Mandrill_ScatterPlotStyle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_ScatterPlotStyle class.
        /// </summary>
        public Mandrill_ScatterPlotStyle()
          : base("ScatterPlotStyle", "Style",
              "Scatter Plot Style",
              Resources.CategoryName, Resources.SubCategory_ScatterPlot)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("DotColor", "DC", Resources.Style_HoverColorDesc, GH_ParamAccess.item, Color.FromArgb(50, 130, 190));
            pManager.AddGenericParameter("Address", "A", Resources.Style_AddressDesc, GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Margins", "M", Resources.Style_MarginsDesc, GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Width", "W", Resources.Style_WidthDesc, GH_ParamAccess.item, 1000);
            pManager.AddIntegerParameter("Height", "H", Resources.Style_HeightDesc, GH_ParamAccess.item, 500);
            pManager.AddTextParameter("YAxisLabel", "YL", Resources.Style_YAxisLabelDesc, GH_ParamAccess.item, "LabelY");
            pManager.AddTextParameter("XAxisLabel", "XL", Resources.Style_YAxisLabelDesc, GH_ParamAccess.item, "LabelX");
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
            Color dotColor = Color.FromArgb(50, 130, 190);
            GridAddress address = new GridAddress(1, 1);
            Margins margins = new Margins();
            int width = 1000;
            int height = 500;
            string yAxisLabel = "LabelY";
            string xAxisLabel = "LabelX";

            DA.GetData<Color>(0, ref dotColor);
            DA.GetData<GridAddress>(1, ref address);
            DA.GetData<Margins>(2, ref margins);
            DA.GetData<int>(3, ref width);
            DA.GetData<int>(4, ref height);
            DA.GetData<string>(5, ref yAxisLabel);
            DA.GetData<string>(6, ref xAxisLabel);

            // create style
            ScatterPlotStyle style = new ScatterPlotStyle();
            style.DotColor = ChartsUtilities.ColorToHexString(dotColor);
            style.GridRow = address.X;
            style.GridColumn = address.Y;
            style.Margins = margins;
            style.Width = width;
            style.Height = height;
            style.YAxisLabel = yAxisLabel;
            style.XAxisLabel = xAxisLabel;
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
                return Resources.Charts_ScatterPlot_Style;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{14baf4d2-1552-44c3-bfb3-0edb19414097}"); }
        }
    }
}