using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.AreaCharts;
using System.Web.Script.Serialization;

namespace Mandrill_Grasshopper.Components.AreaChart
{
    public class Mandrill_AreaChartData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_AreaChartData class.
        /// </summary>
        public Mandrill_AreaChartData()
          : base("AreaChartData", "Data",
              "Area Chart Data",
              Resources.CategoryName, Resources.SubCategoryAreaChart)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Names", "N", Resources.Data_NamesDesc, GH_ParamAccess.list);
            pManager.AddNumberParameter("Values", "V", Resources.Data_ValuesDesc, GH_ParamAccess.list);
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
            List<string> names = new List<string>();
            List<double> values = new List<double>();
            D3jsLib.Domain domain = null;

            if (!DA.GetDataList<string>(0, names)) return;
            if (!DA.GetDataList<double>(1, values)) return;
            DA.GetData<D3jsLib.Domain>(2, ref domain);

            List<D3jsLib.DataPoint1> dataPoints = names.Zip(values, (x, y) => new D3jsLib.DataPoint1 { name = x, value = y }).ToList();
            AreaChartData data = new AreaChartData();
            data.Data = new JavaScriptSerializer().Serialize(dataPoints);
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
                return Resources.Charts_AreaChart_Data;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{de9ccc06-11d5-4bf1-a738-aac37b59859e}"); }
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