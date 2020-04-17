using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.StackedBarChart;
using System.Collections.Generic;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using D3jsLib;
using D3jsLib.Utilities;

namespace Mandrill_Grasshopper.Components.StackedBarChart
{
    public class Mandrill_StackedBarChartData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_StackedBarChartDataFromCSV class.
        /// </summary>
        public Mandrill_StackedBarChartData()
          : base("StackedBarChartData", "Data",
              "Stacked Bar Chart Data",
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
            var headers = new List<string>();
            GH_Structure<GH_String> values;
            D3jsLib.Domain domain = null;

            if (!DA.GetDataList(0, headers)) return;
            if (!DA.GetDataTree(1, out values)) return;
            DA.GetData(2, ref domain);

            var data = new StackedBarChartData();
            data.Data = ChartsUtilities.DataToJsonString(Utilities.Utilities.Data2FromTree(headers, values));
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
                return Resources.Charts_StackedBarChart_Data;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{33ad38f7-9bcc-4b22-8061-39a99f00c341}"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }
    }
}