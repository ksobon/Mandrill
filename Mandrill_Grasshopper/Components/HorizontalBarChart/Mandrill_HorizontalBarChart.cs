using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.HorizontalBarChart;

namespace Mandrill_Grasshopper.Components.HorizontalBarChart
{
    public class Mandrill_HorizontalBarChart : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_HorizontalBarChart class.
        /// </summary>
        public Mandrill_HorizontalBarChart()
          : base("HorizontalBarChart", "Chart",
              "Horizontal Bar Chart",
              Resources.CategoryName, Resources.SubCategoryBarChart)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "D", Resources.Chart_DataDesc, GH_ParamAccess.item);
            pManager.AddGenericParameter("Style", "S", Resources.Chart_StyleDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Chart", "C", Resources.Chart_ChartDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HorizontalBarChartData data = null;
            HorizontalBarChartStyle style = null;

            if (!DA.GetData<HorizontalBarChartData>(0, ref data)) return;
            if (!DA.GetData<HorizontalBarChartStyle>(1, ref style)) return;

            D3jsLib.HorizontalBarChart.HorizontalBarChart chart = new D3jsLib.HorizontalBarChart.HorizontalBarChart(data, style);

            DA.SetData(0, chart);
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
                return Resources.Charts_HorizontalBarChart_Chart;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{fc8ad8ca-2b1c-4d66-8ccc-b644e41433fa}"); }
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