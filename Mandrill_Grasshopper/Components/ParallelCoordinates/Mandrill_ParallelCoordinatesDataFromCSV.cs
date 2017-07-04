using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib.ParallelCoordinates;
using D3jsLib.Utilities;

namespace Mandrill_Grasshopper.Components.ParallelCoordinates
{
    public class Mandrill_ParallelCoordinatesDataFromCSV : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_ParallelCoordinatesDataFromCSV class.
        /// </summary>
        public Mandrill_ParallelCoordinatesDataFromCSV()
          : base("ParallelCoordinatesDataFromCSV", "Data",
              "Parallel Coordinates Data.",
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

            ParallelCoordinatesData data = new ParallelCoordinatesData();
            data.Data = ChartsUtilities.DataToJsonString(ChartsUtilities.Data2FromCsv(filePath));

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
                return Resources.Charts_ParallelCoordinates_DataFromCSV;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{623686c0-17e8-4779-8a8a-8f0aa5dd10ab}"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }
    }
}