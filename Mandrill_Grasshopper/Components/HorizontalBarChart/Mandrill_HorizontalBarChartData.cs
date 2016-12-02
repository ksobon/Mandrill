using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.HorizontalBarChart;
using D3jsLib.Utilities;
using D3jsLib;
using System.Web.Script.Serialization;

namespace Mandrill_Grasshopper.Components.HorizontalBarChart
{
    public class Mandrill_HorizontalBarChartData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_HorizontalBarChartData class.
        /// </summary>
        public Mandrill_HorizontalBarChartData()
          : base("HorizontalBarChartData", "Data",
              "Horizontal Bar Chart Data",
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
            pManager.AddIntegerParameter("Colors", "C", Resources.Data_ColorsDesc, GH_ParamAccess.list);
            pManager.AddGenericParameter("Domain", "D", Resources.Data_Domain, GH_ParamAccess.item);
            pManager[3].Optional = true;
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
            List<int> colors = new List<int>();
            Domain domain = null;

            if (!DA.GetDataList<string>(0, names)) return;
            if (!DA.GetDataList<double>(1, values)) return;
            DA.GetDataList<int>(2, colors);
            DA.GetData<Domain>(3, ref domain);

            List<DataPoint1> dataPoints;
            if (colors.Count > 0)
            {
                dataPoints = names.ZipThree(values, colors, (x, y, z) => new DataPoint1 { name = x, value = y, color = z }).ToList();
            }
            else
            {
                dataPoints = names.Zip(values, (x, y) => new DataPoint1 { name = x, value = y }).ToList();
            }
            HorizontalBarChartData data = new HorizontalBarChartData();
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
                return Resources.Charts_HorizontalBarChart_Data;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{753bebb8-7223-45e1-951e-ae47e3913ba4}"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quarternary;
            }
        }
    }
}