using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.DonutChart;
using System.Web.Script.Serialization;

namespace Mandrill_Grasshopper.Components.DonutChart
{
    public class Mandrill_DonutChartDataFromCSV : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_DonutChartDataFromCSV class.
        /// </summary>
        public Mandrill_DonutChartDataFromCSV()
          : base("DonutChartDataFromCSV", "Data",
              "Donut Chart Data",
              Resources.CategoryName, Resources.SubCategoryDonutChartDesc)
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

            DonutChartData data = new DonutChartData();
            data.Data = new JavaScriptSerializer().Serialize(D3jsLib.Utilities.ChartsUtilities.Data1FromCsv(filePath));

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
                return Resources.Charts_DonutChart_DataFromCSV;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{10ff54da-4f65-42a5-a630-153280399a11}"); }
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