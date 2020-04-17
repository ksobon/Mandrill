using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.AreaCharts;
using System.Web.Script.Serialization;

namespace Mandrill_Grasshopper.Components.AreaChart
{
    public class Mandrill_AreaChartDataFromCSV : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_AreaChartDataFromCSV class.
        /// </summary>
        public Mandrill_AreaChartDataFromCSV()
          : base("AreaChartDataFromCSV", "Data",
              "Area Chart Data",
              Resources.CategoryName, Resources.SubCategoryAreaChart)
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

            if (!DA.GetData(0, ref filePath)) return;
            DA.GetData(1, ref domain);

            var data = new AreaChartData();
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
                return Resources.Charts_AreaChart_DataFromCSV;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{d2140c14-78c0-444d-8617-6cb16e5715cf}"); }
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