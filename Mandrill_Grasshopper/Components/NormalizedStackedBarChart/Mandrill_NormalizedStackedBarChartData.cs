using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.NormalizedStackedBarChart;
using System.Collections.Generic;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using D3jsLib;
using D3jsLib.Utilities;

namespace Mandrill_Grasshopper.Components.NormalizedStackedBarChart
{
    public class Mandrill_NormalizedStackedBarChartData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the NormalizedStackedBarChartData class.
        /// </summary>
        public Mandrill_NormalizedStackedBarChartData()
          : base("NormalizedStackedBarChartData", "Data",
              "Normalized Stacked Bar Chart Data",
              Resources.CategoryName, Resources.SubCategoryBarChart)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Headers", "H", Resources.Data_HeadersDesc, GH_ParamAccess.list);
            pManager.AddTextParameter("Values", "V", Resources.Data_ValuesTreeDesc, GH_ParamAccess.tree);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "D", Resources.Chart_DataDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> headers = new List<string>();
            GH_Structure<GH_String> values;

            if (!DA.GetDataList<string>(0, headers)) return;
            if (!DA.GetDataTree(1, out values)) return;

            NormalizedStackedBarChartData data = new NormalizedStackedBarChartData();
            data.Data = ChartsUtilities.DataToJsonString(Utilities.Utilities.Data2FromTree(headers, values));

            DA.SetData(0, data);
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
                return Resources.Charts_NormalizedStackedBarChart_Data;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{cb6ed428-e31d-446b-a93b-27f3453a2362}"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quinary; }
        }
    }
}