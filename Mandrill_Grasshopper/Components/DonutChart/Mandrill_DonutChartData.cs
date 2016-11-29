using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.DonutChart;
using System.Web.Script.Serialization;

namespace Mandrill_Grasshopper.Components.DonutChart
{
    public class Mandrill_DonutChartData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_DonutChartData class.
        /// </summary>
        public Mandrill_DonutChartData()
          : base("DonutChartData", "Data",
              "Donut Chart Data",
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
            List<string> names = new List<string>();
            List<double> values = new List<double>();

            if (!DA.GetDataList<string>(0, names)) return;
            if (!DA.GetDataList<double>(1, values)) return;

            List<D3jsLib.DataPoint1> dataPoints = names.Zip(values, (x, y) => new D3jsLib.DataPoint1 { name = x, value = y }).ToList();
            DonutChartData data = new DonutChartData();
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
                return Resources.Charts_DonutChart_Data;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{5126a81a-b90e-4b0c-bd8f-412363691804}"); }
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