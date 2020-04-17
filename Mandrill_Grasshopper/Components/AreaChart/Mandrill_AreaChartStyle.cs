using System;
using System.Drawing;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib;
using D3jsLib.Utilities;

namespace Mandrill_Grasshopper.Components.AreaChart
{
    public class Mandrill_AreaChartStyle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_AreaChartStyle class.
        /// </summary>
        public Mandrill_AreaChartStyle()
          : base("AreaChartStyle", "Style",
              "Create a Area Chart style object.",
              Resources.CategoryName, Resources.SubCategoryAreaChart)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("AreaColor", "AC", Resources.Style_AreaColorDesc, GH_ParamAccess.item, Color.FromArgb(50, 130, 190));
            pManager.AddGenericParameter("Address", "A", Resources.Style_AddressDesc, GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Margins", "M", Resources.Style_MarginsDesc, GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Width", "W", Resources.Style_WidthDesc, GH_ParamAccess.item, 1000);
            pManager.AddIntegerParameter("Height", "H", Resources.Style_HeightDesc, GH_ParamAccess.item, 500);
            pManager.AddTextParameter("YAxisLabel", "L", Resources.Style_YAxisLabelDesc, GH_ParamAccess.item, "Label");
            pManager.AddIntegerParameter("TickMarksX", "T", Resources.Style_TickMarksXDesc, GH_ParamAccess.item, 2);
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
            var areaColor = Color.FromArgb(50, 130, 190);
            var address = new GridAddress(1, 1);
            var width = 1000;
            var height = 500;
            var yAxisLabel = "Label";
            var tickMarks = 2;
            var margins = new Margins();

            DA.GetData(0, ref areaColor);
            DA.GetData(1, ref address);
            DA.GetData(2, ref margins);
            DA.GetData(3, ref width);
            DA.GetData(4, ref height);
            DA.GetData(5, ref yAxisLabel);
            DA.GetData(6, ref tickMarks);

            // create style
            var style = new D3jsLib.AreaCharts.AreaChartStyle();
            style.AreaColor = ChartsUtilities.ColorToHexString(areaColor);
            style.GridRow = address.X;
            style.GridColumn = address.Y;
            style.Width = width;
            style.Height = height;
            style.YAxisLabel = yAxisLabel;
            style.TickMarksX = tickMarks;
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
                return Resources.Charts_AreaChart_Style;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{b28118d8-c82e-4d99-9bf6-cd887244df4a}"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }
    }
}