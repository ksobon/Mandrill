using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.NormalizedStackedBarChart;
using D3jsLib.Utilities;

namespace Mandrill_Grasshopper.Components.NormalizedStackedBarChart
{
    public class Mandrill_NormalizedStackedBarChartDataFromCSV : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the NormalizedStackedBarChartDataFromCSV class.
        /// </summary>
        public Mandrill_NormalizedStackedBarChartDataFromCSV()
          : base("NormalizedStackedBarChartDataFromCSV", "Data",
              "Normalized Stacked Bar Chart Data",
              Resources.CategoryName, Resources.SubCategoryBarChart)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("filePath", "P", Resources.Data_FilePathDesc, GH_ParamAccess.item);
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

            if (!DA.GetData<string>(0, ref filePath)) return;

            NormalizedStackedBarChartData data = new NormalizedStackedBarChartData();
            data.Data = ChartsUtilities.DataToJsonString(ChartsUtilities.Data2FromCSV(filePath));

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
                return Resources.Charts_NormalizedStackedBarChart_DataFromCSV;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{5de887a3-ec88-4cb6-bfb9-76a6b5d5ba22}"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quinary; }
        }
    }
}