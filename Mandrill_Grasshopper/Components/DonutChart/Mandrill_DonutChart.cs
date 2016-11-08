using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.DonutChart;

namespace Mandrill_Grasshopper.Components.DonutChart
{
    public class Mandrill_DonutChart : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_DonutChart class.
        /// </summary>
        public Mandrill_DonutChart()
          : base("DonutChart", "Chart",
              "Donut Chart object",
              Resources.CategoryName, Resources.SubCategoryDonutChartDesc)
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
            DonutChartData data = null;
            DonutChartStyle style = null;

            if (!DA.GetData<DonutChartData>(0, ref data)) return;
            if (!DA.GetData<DonutChartStyle>(1, ref style)) return;

            D3jsLib.DonutChart.DonutChart chart = new D3jsLib.DonutChart.DonutChart(data, style);

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
                return Resources.Charts_DonutChart_Chart;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{abecc7bf-e9ea-42d2-b7a7-23ca3070b149}"); }
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