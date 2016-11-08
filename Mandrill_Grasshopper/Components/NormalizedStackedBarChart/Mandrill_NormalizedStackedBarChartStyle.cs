using System;
using System.Drawing;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib;
using D3jsLib.NormalizedStackedBarChart;
using System.Collections.Generic;
using System.Linq;
using D3jsLib.Utilities;

namespace Mandrill_Grasshopper.Components.NormalizedStackedBarChart
{
    public class Mandrill_NormalizedStackedBarChartStyle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_NormalizedStackedBarChartStyle class.
        /// </summary>
        public Mandrill_NormalizedStackedBarChartStyle()
          : base("NormalizedStackedBarChartStyle", "Style",
              "Normalized Stacked Bar Chart Style",
              Resources.CategoryName, Resources.SubCategoryBarChart)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("BarHoverColor", "HC", Resources.Style_HoverColorDesc, GH_ParamAccess.item, Color.FromArgb(255, 0, 0));
            pManager.AddColourParameter("Colors", "C", Resources.Style_ColorsDesc, GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Address", "A", Resources.Style_AddressDesc, GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("Margins", "M", Resources.Style_MarginsDesc, GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("Width", "W", Resources.Style_WidthDesc, GH_ParamAccess.item, 1000);
            pManager.AddIntegerParameter("Height", "H", Resources.Style_HeightDesc, GH_ParamAccess.item, 500);
            pManager.AddTextParameter("YAxisLabel", "L", Resources.Style_YAxisLabelDesc, GH_ParamAccess.item, "Label");
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
            Color hoverColor = Color.FromArgb(255, 0, 0);
            List<Color> colors = new List<Color>();
            GridAddress address = new GridAddress(1, 1);
            int width = 1000;
            int height = 500;
            string yAxisLabel = "Label";
            Margins margins = new Margins();

            DA.GetData<Color>(0, ref hoverColor);
            DA.GetData<GridAddress>(2, ref address);
            DA.GetData<Margins>(3, ref margins);
            DA.GetData<int>(4, ref width);
            DA.GetData<int>(5, ref height);
            DA.GetData<string>(6, ref yAxisLabel);

            // create style
            NormalizedStackedBarChartStyle style = new NormalizedStackedBarChartStyle();

            if (DA.GetDataList<Color>(1, colors))
            {
                List<string> hexColors = colors.Select(x => ChartsUtilities.ColorToHexString(Color.FromArgb(x.A, x.R, x.G, x.B))).ToList();
                style.Colors = hexColors;
            }
            else
            {
                style.Colors = null;
            }

            style.BarHoverColor = hoverColor;
            style.GridRow = address.X;
            style.GridColumn = address.Y;
            style.Width = width;
            style.Height = height;
            style.YAxisLabel = yAxisLabel;
            style.Margins = margins;

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
                return Resources.Charts_NormalizedStackedBarChart_Style;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{9e7072ca-286e-4b01-a4c1-431a9cbde450}"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quinary;
            }
        }
    }
}