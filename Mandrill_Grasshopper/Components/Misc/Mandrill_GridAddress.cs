using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;

namespace Mandrill.Grasshopper.Components.Misc
{
    public class Mandrill_GridAddress : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_GridAddress class.
        /// </summary>
        public Mandrill_GridAddress()
          : base("Address", "Address",
              "Location of the chart in a grid.",
              Resources.CategoryName, Resources.SubCategoryMisc)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Column", "C", Resources.Misc_AddressColumnDesc, GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("Row", "R", Resources.Misc_AddressRowDesc, GH_ParamAccess.item, 1);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Address", "A", Resources.Style_AddressDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int X = 1;
            int Y = 1;

            DA.GetData<int>(0, ref X);
            DA.GetData<int>(1, ref Y);

            D3jsLib.GridAddress address = new D3jsLib.GridAddress(X, Y);

            DA.SetData(0, address);
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
                return Resources.Charts_MiscNodes_Address;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{4ec60665-e722-435b-a0aa-e734523243c8}"); }
        }
    }
}