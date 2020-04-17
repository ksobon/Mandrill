using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.NormalizedStackedBarChart;

namespace Mandrill_Grasshopper.Components.NormalizedStackedBarChart
{
    public class Mandrill_NormalizedStackedBarChart : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the NormalizedStackedBarChart class.
        /// </summary>
        public Mandrill_NormalizedStackedBarChart()
          : base("NormalizedStackedBarChart", "Chart",
              "Normalized Stacked Bar Chart",
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
            NormalizedStackedBarChartData data = null;
            NormalizedStackedBarChartStyle style = null;

            if (!DA.GetData(0, ref data)) return;
            if (!DA.GetData(1, ref style)) return;

            var chart = new D3jsLib.NormalizedStackedBarChart.NormalizedStackedBarChart(data, style);

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
                return Resources.Charts_NormalizedStackedBarChart_Chart;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{7cee74a7-d641-47d7-b996-7a017e0bd79b}"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quinary; }
        }
    }
}