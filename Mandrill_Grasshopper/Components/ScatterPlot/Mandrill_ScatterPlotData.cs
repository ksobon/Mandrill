using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.d3ScatterPlots;
using System.Collections.Generic;
using D3jsLib;
using D3jsLib.Utilities;
using System.Linq;
using System.Web.Script.Serialization;

namespace Mandrill_Grasshopper.Components.ScatterPlot
{
    public class Mandrill_ScatterPlotData : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_ScatterPlotData class.
        /// </summary>
        public Mandrill_ScatterPlotData()
          : base("ScatterPlotData", "Data",
              "Scatter Plot Data",
              Resources.CategoryName, Resources.SubCategory_ScatterPlot)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Names", "N", Resources.Data_NamesDesc, GH_ParamAccess.list);
            pManager.AddNumberParameter("ValueX", "VX", Resources.Data_ValuesDesc, GH_ParamAccess.list);
            pManager.AddNumberParameter("ValueY", "VX", Resources.Data_ValuesDesc, GH_ParamAccess.list);
            pManager.AddNumberParameter("Size", "S", Resources.Data_ValuesDesc, GH_ParamAccess.list);
            pManager.AddGenericParameter("DomainX", "DX", Resources.Data_Domain, GH_ParamAccess.item);
            pManager[4].Optional = true;
            pManager.AddGenericParameter("DomainY", "DY", Resources.Data_Domain, GH_ParamAccess.item);
            pManager[5].Optional = true;
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
            List<double> valuesX = new List<double>();
            List<double> valuesY = new List<double>();
            List<double> sizes = new List<double>();
            Domain domainX = null;
            Domain domainY = null;

            if (!DA.GetDataList<string>(0, names)) return;
            if (!DA.GetDataList<double>(1, valuesX)) return;
            if (!DA.GetDataList<double>(2, valuesY)) return;
            if (!DA.GetDataList<double>(3, sizes)) return;
            DA.GetData<Domain>(4, ref domainX);
            DA.GetData<Domain>(5, ref domainY);

            List<ScatterPlotDataPoint> dataPoints = names
                .ZipFour(valuesX, valuesY, sizes, (x, y, z, v) => new ScatterPlotDataPoint { name = x, valueX = y, valueY = z, size = v })
                .ToList();

            ScatterPlotData data = new ScatterPlotData();
            data.Data = new JavaScriptSerializer().Serialize(dataPoints);
            data.DomainX = domainX;
            data.DomainY = domainY;

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
                return Resources.Charts_ScatterPlot_Data;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{9b754a19-e131-4dfa-8d37-c45f838a2a69}"); }
        }
    }
}