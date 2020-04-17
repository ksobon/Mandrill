using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using Grasshopper.Kernel.Parameters;
// ReSharper disable InconsistentNaming

namespace Mandrill_Grasshopper.Components.Report
{
    public class Mandrill_CreateReport : GH_Component, IGH_VariableParameterComponent
    {
        public string InnerHtml = "";

        /// <summary>
        /// Initializes a new instance of the Mandrill_CreateReport class.
        /// </summary>
        public Mandrill_CreateReport()
          : base("Create Report", "Report",
              "Creates Report object that can be viewed in a Window or printer to PDF.",
              Resources.CategoryName, Resources.SubCategoryReport)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Chart0", "A", "Chart 0", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Report", "R", Resources.Report_ReportDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var charts = new List<object>();
            for (var i = 0; i < Params.Input.Count; i++)
            {
                D3jsLib.Chart currentChart = null;
                DA.GetData(i, ref currentChart);
                if (currentChart != null)
                {
                    charts.Add(currentChart);
                }
                else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, Resources.Warning_MissingInput);
                }
            }

            var finalHtmlString = D3jsLib.Charts.CompileHtmlString(charts, false);
            InnerHtml = finalHtmlString;
            var report = new D3jsLib.Report(finalHtmlString);
            DA.SetData(0, report);
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
                return Resources.Mandrill_ChromeWindow_MandrillReportNode_MandrillReportNode;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{b5367f1c-28bb-468c-99da-a725b70ea88e}"); }
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Output) return false;
            return index != 0;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Output) return false;
            if (Params.Input.Count <= 1) return false;
            return index != 0;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            var param = new Param_GenericObject
            {
                Name = $"Chart{index}",
                NickName = GH_ComponentParamServer.InventUniqueNickname("BCDEFGHIJKLMNOPQRSTUVWXYZ", Params.Input),
                Description = $"Chart {index}",
                Access = GH_ParamAccess.item
            };
            Params.RegisterInputParam(param, index);

            return param;
        }

        bool IGH_VariableParameterComponent.DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }

        void IGH_VariableParameterComponent.VariableParameterMaintenance()
        {
        }
    }
}