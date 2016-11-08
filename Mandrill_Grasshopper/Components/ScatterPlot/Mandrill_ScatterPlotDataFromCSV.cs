using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.d3ScatterPlots;
using D3jsLib;
using System.Collections.Generic;
using System.Linq;

namespace Mandrill_Grasshopper.Components.ScatterPlot
{
    public class Mandrill_ScatterPlotDataFromCSV : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_ScatterPlotDataFromCSV class.
        /// </summary>
        public Mandrill_ScatterPlotDataFromCSV()
          : base("ScatterPlotDataFromCSV", "Data",
              "Scatter Plot Data",
              Resources.CategoryName, Resources.SubCategory_ScatterPlot)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("filePath", "P", Resources.Data_FilePathDesc, GH_ParamAccess.item);
            pManager.AddGenericParameter("DomainX", "DX", Resources.Data_Domain, GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("DomainY", "DY", Resources.Data_Domain, GH_ParamAccess.item);
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
            string filePath = null;
            Domain domainX = null;
            Domain domainY = null;

            if (!DA.GetData<string>(0, ref filePath)) return;
            DA.GetData<Domain>(1, ref domainX);
            DA.GetData<Domain>(2, ref domainY);

            List<ScatterPlotDataPoint> dataPoints = new List<ScatterPlotDataPoint>();
            var csv = new List<string[]>();
            var lines = System.IO.File.ReadAllLines(filePath);
            for (int i = 1; i < lines.Count(); i++)
            {
                string line = lines[i];
                if (i > 0)
                {
                    string lineName = line.Split(',')[0];
                    double lineValueX = Convert.ToDouble(line.Split(',')[1]);
                    double lineValueY = Convert.ToDouble(line.Split(',')[2]);
                    double lineSize = Convert.ToDouble(line.Split(',')[3]);
                    string lineColor = line.Split(',')[4];

                    dataPoints.Add(new ScatterPlotDataPoint { name = lineName, valueX = lineValueX, valueY = lineValueY, size = lineSize, color = lineColor });
                }
            }
            ScatterPlotData data = new ScatterPlotData();
            data.Data = dataPoints;
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
                return Resources.Charts_ScatterPlot_DataFromCSV;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{0e7e5de6-69b9-4722-b021-7465c05c573b}"); }
        }
    }
}