using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.BarChart;
using System.Web.Script.Serialization;

namespace Mandrill_Grasshopper.Components.BarChart
{
    public class Mandrill_BarChartData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_BarChartData class.
        /// </summary>
        public Mandrill_BarChartData()
          : base("BarChartData", "Data",
              "Bar Chart Data",
              Resources.CategoryName, Resources.SubCategoryBarChart)
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
            var names = new List<string>();
            var values = new List<double>();
            D3jsLib.Domain domain = null;

            if (!DA.GetDataList(0, names)) return;
            if (!DA.GetDataList(1, values)) return;
            DA.GetData(2, ref domain);

            var dataPoints = names.Zip(values, (x, y) => new D3jsLib.DataPoint1 { name = x, value = y }).ToList();
            var data = new BarData();
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
                return Resources.Charts_BarChart_Data;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{6727baee-6e35-4e2e-8f50-f08accc48dc6}"); }
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