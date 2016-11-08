using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;

namespace Mandrill_Grasshopper.Components.Misc
{
    public class Mandrill_Domain : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Domain class.
        /// </summary>
        public Mandrill_Domain()
          : base("Domain", "Domain",
              "Range for Y-Axis.",
             Resources.CategoryName , Resources.SubCategoryMisc)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Start", "A", Resources.Misc_DomainStartDesc, GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("End", "B", Resources.Misc_DomainEndDesc, GH_ParamAccess.item, 1.0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Domain", "D", Resources.Data_Domain, GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double a = 0.0;
            double b = 1.0;

            DA.GetData<double>(0, ref a);
            DA.GetData<double>(1, ref b);

            D3jsLib.Domain domain = new D3jsLib.Domain(a, b);

            DA.SetData(0, domain);
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
                return Resources.Charts_MiscNodes_Domain;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{1f4a7d2f-6447-4e8b-8c64-30b66cb6e89f}"); }
        }
    }
}