using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;

namespace Mandrill_Grasshopper.Components.Misc
{
    public class Mandrill_Margins : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_Margins class.
        /// </summary>
        public Mandrill_Margins()
          : base("Margins", "Margins",
              "Margins around the chart.",
              Resources.CategoryName, Resources.SubCategoryMisc)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Top", "T", Resources.Misc_TopMarginDesc, GH_ParamAccess.item, 20);
            pManager.AddIntegerParameter("Bottom", "B", Resources.Misc_BottomMarginDesc, GH_ParamAccess.item, 40);
            pManager.AddIntegerParameter("Right", "R", Resources.Misc_RightMarginDesc, GH_ParamAccess.item, 20);
            pManager.AddIntegerParameter("Left", "L", Resources.Misc_LeftMarginDesc, GH_ParamAccess.item, 40);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Margins", "M", Resources.Style_MarginsDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var top = 20;
            var bottom = 40;
            var right = 20;
            var left = 40;

            DA.GetData(0, ref top);
            DA.GetData(1, ref bottom);
            DA.GetData(2, ref right);
            DA.GetData(3, ref left);

            var margins = new D3jsLib.Margins(top, bottom, left, right);

            DA.SetData(0, margins);
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
                return Resources.Charts_MiscNodes_Margins;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{3992066a-b8ae-41d7-b48a-9c9bb46e2100}"); }
        }
    }
}