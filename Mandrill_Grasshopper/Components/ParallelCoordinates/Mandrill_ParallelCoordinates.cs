using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.ParallelCoordinates;

namespace Mandrill_Grasshopper.Components.ParallelCoordinates
{
    public class Mandrill_ParallelCoordinates : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_ParallelCoordinates class.
        /// </summary>
        public Mandrill_ParallelCoordinates()
          : base("ParallelCoordinates", "Chart",
              "Parallel Coordinates Chart.",
              Resources.CategoryName, Resources.SubCategory_Multivariate)
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
            ParallelCoordinatesData data = null;
            ParallelCoordinatesStyle style = null;

            if (!DA.GetData<ParallelCoordinatesData>(0, ref data)) return;
            if (!DA.GetData<ParallelCoordinatesStyle>(1, ref style)) return;

            ParallelCoordinatesChart chart = new ParallelCoordinatesChart(data, style);

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
                return Resources.Charts_ParallelCoordinates_Chart;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{c99db6ab-79e7-4202-8000-88e30105d242}"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }
    }
}