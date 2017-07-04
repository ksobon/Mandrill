using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.BarChart;
using System.Web.Script.Serialization;

namespace Mandrill_Grasshopper.Components.BarChart
{
    public class Mandrill_BarChartDataFromCSV : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_BarChartDataFromCSV class.
        /// </summary>
        public Mandrill_BarChartDataFromCSV()
          : base("BarChartDataFromCSV", "Data",
              "Bar Chart Data",
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
            D3jsLib.Domain domain = null;

            if (!DA.GetData<string>(0, ref filePath)) return;
            DA.GetData<D3jsLib.Domain>(1, ref domain);

            BarData data = new BarData();
            data.Data = new JavaScriptSerializer().Serialize(D3jsLib.Utilities.ChartsUtilities.Data1FromCsv(filePath));
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
                return Resources.Charts_BarChart_DataFromCSV;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{d45c2fbb-ff5e-4eaf-b2dd-12e4068e53b7}"); }
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