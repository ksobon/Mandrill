using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.StackedBarChart;
using D3jsLib;
using D3jsLib.Utilities;

namespace Mandrill_Grasshopper.Components.StackedBarChart
{
    public class Mandrill_StackedBarChartDataFromCSV : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_StackedBarChartDataFromCSV class.
        /// </summary>
        public Mandrill_StackedBarChartDataFromCSV()
          : base("StackedBarChartDataFromCSV", "Data",
              "Stacked Bar Chart Data",
              Resources.CategoryName, Resources.SubCategoryBarChart)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("filePath", "P", Resources.Data_FilePathDesc, GH_ParamAccess.item);
            pManager.AddGenericParameter("Domain", "D", Resources.Data_Domain, GH_ParamAccess.item);
            pManager[1].Optional = true;
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
            string filePath = null;
            Domain domain = null;

            if (!DA.GetData<string>(0, ref filePath)) return;
            DA.GetData<Domain>(1, ref domain);

            StackedBarChartData data = new StackedBarChartData();
            data.Data = ChartsUtilities.DataToJsonString(ChartsUtilities.Data2FromCSV(filePath));
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
                return Resources.Charts_StackedBarChart_DataFromCSV;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{114bdb26-0077-411b-b7e8-011593226566}"); }
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