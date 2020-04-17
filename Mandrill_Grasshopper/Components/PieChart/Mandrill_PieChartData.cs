using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.PieChart;
using System.Web.Script.Serialization;

namespace Mandrill_Grasshopper.Components.PieChart
{
    public class Mandrill_PieChartData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_PieChartData class.
        /// </summary>
        public Mandrill_PieChartData()
          : base("PieChartData", "Data",
              "Pie Chart Data",
              Resources.CategoryName, Resources.SubCategoryDonutChartDesc)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Names", "N", Resources.Data_NamesDesc, GH_ParamAccess.list);
            pManager.AddNumberParameter("Values", "V", Resources.Data_ValuesDesc, GH_ParamAccess.list);
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

            if (!DA.GetDataList(0, names)) return;
            if (!DA.GetDataList(1, values)) return;

            var dataPoints = names.Zip(values, (x, y) => new D3jsLib.DataPoint1 { name = x, value = y }).ToList();
            var data = new PieChartData();
            data.Data = new JavaScriptSerializer().Serialize(dataPoints);

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
                return Resources.Charts_PieChart_Data;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{ec883d87-b1ab-4d49-b424-0420f7098cdd}"); }
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