using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib;

namespace Mandrill_Grasshopper.Components.Text
{
    public class Mandrill_Text : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_Text class.
        /// </summary>
        public Mandrill_Text()
          : base("Text", "Text",
              "Creates Text object.",
              Resources.CategoryName, Resources.SubCategory_Text)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "T", Resources.Text_TextValueDesc, GH_ParamAccess.item);
            pManager.AddGenericParameter("Style", "S", Resources.Text_StyleDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Text", "T", Resources.Text_TextDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string text = "";
            TextStyle style = null;

            if (!DA.GetData<string>(0, ref text)) return;
            if (!DA.GetData<TextStyle>(1, ref style)) return;

            TextNote tn = new TextNote(text, style);

            DA.SetData(0, tn);
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
                return Resources.Text_Text_Create;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{08d1f5d1-8fca-4894-8850-8cca78669d11}"); }
        }
    }
}