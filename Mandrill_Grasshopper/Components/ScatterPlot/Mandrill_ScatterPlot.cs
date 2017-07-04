using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.d3ScatterPlots;

namespace Mandrill_Grasshopper.Components.ScatterPlot
{
    public class Mandrill_ScatterPlot : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_ScatterPlot class.
        /// </summary>
        public Mandrill_ScatterPlot()
          : base("ScatterPlot", "Chart",
              "Scatter Plot Chart.",
              Resources.CategoryName, Resources.SubCategory_ScatterPlot)
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
            ScatterPlotData data = null;
            ScatterPlotStyle style = null;

            if (!DA.GetData<ScatterPlotData>(0, ref data)) return;
            if (!DA.GetData<ScatterPlotStyle>(1, ref style)) return;

            d3ScatterPlot chart = new d3ScatterPlot(data, style);

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
                return Resources.Charts_ScatterPlot_Chart;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{37d4c7dc-2d74-4cff-b250-1e2d8beb3124}"); }
        }
    }
}