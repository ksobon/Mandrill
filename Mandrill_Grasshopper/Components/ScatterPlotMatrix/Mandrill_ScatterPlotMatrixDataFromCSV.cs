using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.ScatterPlotMatrix;
using D3jsLib.Utilities;

namespace Mandrill_Grasshopper.Components.ScatterPlotMatrix
{
    public class Mandrill_ScatterPlotMatrixDataFromCSV : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_ScatterPlotMatrixDataFromCSV class.
        /// </summary>
        public Mandrill_ScatterPlotMatrixDataFromCSV()
          : base("ScatterPlotMatrixDataFromCSV", "Data",
              "Scatter Plot Matrix Data.",
              Resources.CategoryName, Resources.SubCategory_Multivariate)
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

            ScatterPlotMatrixData data = new ScatterPlotMatrixData();
            data.Data = ChartsUtilities.DataToJsonString(ChartsUtilities.Data2FromCSV(filePath));

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
                return Resources.Charts_ScatterPlotMatrix_DataFromCSV;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{49922a7b-b5d5-4e5e-922a-fe0dca67b4fc}"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }
    }
}