using System;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using System.Windows;
using Mandrill_Grasshopper.Components.Report;

namespace Mandrill_Grasshopper.Components.PDF
{
    public class Mandrill_PDF : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_PDF class.
        /// </summary>
        public Mandrill_PDF()
          : base("PrintToPdf", "Pdf",
              "PDF object.",
              Resources.CategoryName, Resources.SubCategoryPDF)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("filePath", "P", Resources.Data_FilePathDesc, GH_ParamAccess.item);
            pManager.AddGenericParameter("Style", "S", Resources.PDF_StyleDesc, GH_ParamAccess.item);
            pManager.AddBooleanParameter("Print", "P", Resources.Report_ShowWindowDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            D3jsLib.PdfStyle style = null;
            string filePath = null;
            bool print = false;

            if (!DA.GetData<string>(0, ref filePath)) return;
            if (!DA.GetData<D3jsLib.PdfStyle>(1, ref style)) return;
            if (!DA.GetData<bool>(2, ref print)) return;

            if (print)
            {
                bool printed = false;
                Report.MandrillWindow win = null;
                Mandrill_LaunchWindow winComponent = null;

                foreach (IGH_DocumentObject obj in Grasshopper.Instances.ActiveCanvas.Document.Objects)
                {
                    if (obj.GetType() == typeof(Mandrill_LaunchWindow))
                    {
                        winComponent = (Mandrill_LaunchWindow)obj;
                        if (winComponent != null)
                        {
                            win = winComponent.GetWindow();
                            if (win != null)
                            {
                                win.Print(filePath, style);
                                printed = true;
                                break;
                            }
                        }
                    }
                }

                if (!printed)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "There is either no Mandrill Window Launch component on canvas or one doesn't have an open Window.");
                }
            }
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
                return Resources.Mandrill_Print_MandrillPrintNodeModel;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{fc79eff1-c14a-4462-9d05-fcc7e7c3c296}"); }
        }
    }
}