using System;
using System.Drawing;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib;
using D3jsLib.HorizontalBarChart;
using System.Collections.Generic;
using System.Linq;
using D3jsLib.Utilities;
using System.Web.Script.Serialization;

namespace Mandrill_Grasshopper.Components.HorizontalBarChart
{
    public class Mandrill_HorizontalBarChartStyle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_HorizontalBarChartStyle class.
        /// </summary>
        public Mandrill_HorizontalBarChartStyle()
          : base("HorizontalBarChartStyle", "Style",
              "Horizontal Bar Chart Style.",
              Resources.CategoryName, Resources.SubCategoryBarChart)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("BarColor", "BC", Resources.Style_BarColorDesc, GH_ParamAccess.list, new List<Color>() { Color.FromArgb(50, 130, 190) });
            pManager.AddColourParameter("BarHoverColor", "HC", Resources.Style_HoverColorDesc, GH_ParamAccess.item, Color.FromArgb(255, 0, 0));
            pManager.AddGenericParameter("Address", "A", Resources.Style_AddressDesc, GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("Margins", "M", Resources.Style_MarginsDesc, GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("Width", "W", Resources.Style_WidthDesc, GH_ParamAccess.item, 1000);
            pManager.AddIntegerParameter("Height", "H", Resources.Style_HeightDesc, GH_ParamAccess.item, 500);
            pManager.AddTextParameter("XAxisLabel", "L", Resources.Style_YAxisLabelDesc, GH_ParamAccess.item, "Label");
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
            var barColor = new List<Color>() { };
            var hoverColor = Color.FromArgb(255, 0, 0);
            var address = new GridAddress(1, 1);
            var width = 1000;
            var height = 500;
            var xAxisLabel = "Label";
            var margins = new Margins();

            DA.GetDataList(0, barColor);
            DA.GetData(1, ref hoverColor);
            DA.GetData(2, ref address);
            DA.GetData(3, ref margins);
            DA.GetData(4, ref width);
            DA.GetData(5, ref height);
            DA.GetData(6, ref xAxisLabel);

            // create style
            var style = new HorizontalBarChartStyle();
            var hexColors = barColor.Select(x => ChartsUtilities.ColorToHexString(Color.FromArgb(x.A, x.R, x.G, x.B))).ToList();
            style.BarColor = new JavaScriptSerializer().Serialize(hexColors);
            style.BarHoverColor = ChartsUtilities.ColorToHexString(hoverColor);
            style.GridRow = address.X;
            style.GridColumn = address.Y;
            style.Width = width;
            style.Height = height;
            style.YAxisLabel = xAxisLabel;
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
                return Resources.Charts_HorizontalBarChart_Style;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{00dee3e0-bf2a-41c2-9f5d-5997cc7a065f}"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quarternary;
            }
        }
    }
}