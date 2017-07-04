using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.GroupedBarChart;
using System.Collections.Generic;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using D3jsLib;
using D3jsLib.Utilities;

namespace Mandrill_Grasshopper.Components.GroupedBarChart
{
    public class Mandrill_GroupedBarChartData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_GroupedBarChartData class.
        /// </summary>
        public Mandrill_GroupedBarChartData()
          : base("GroupedBarChartData", "Data",
              "Grouped Bar Chart Data",
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
            pManager.AddGenericParameter("Domain", "D", Resources.Data_Domain, GH_ParamAccess.item);
            pManager[2].Optional = true;
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
            D3jsLib.Domain domain = null;

            if (!DA.GetDataList<string>(0, headers)) return;
            if (!DA.GetDataTree(1, out values)) return;
            DA.GetData<D3jsLib.Domain>(2, ref domain);

            GroupedBarChartData data = new GroupedBarChartData();
            data.Data = ChartsUtilities.DataToJsonString(Mandrill_Grasshopper.Utilities.Utilities.Data2FromTree(headers, values));
            data.Domain = domain;

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
                return Resources.Charts_GroupedBarChart_Data;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{eb1d54fe-4e64-4e82-9a7c-b778de146dec}"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }
    }
}