using System;
using System.Drawing;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib;
using D3jsLib.ScatterPlotMatrix;
using System.Collections.Generic;
using System.Linq;
using D3jsLib.Utilities;
using System.Web.Script.Serialization;

namespace Mandrill_Grasshopper.Components.ScatterPlotMatrix
{
    public class Mandrill_ScatterPlotMatrixStyle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_ScatterPlotMatrixStyle class.
        /// </summary>
        public Mandrill_ScatterPlotMatrixStyle()
          : base("ScatterPlotMatrixStyle", "Style",
              "Scatter Plot Matrix Style.",
              Resources.CategoryName, Resources.SubCategory_Multivariate)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("Colors", "C", Resources.Style_ColorsDesc, GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("Address", "A", Resources.Style_AddressDesc, GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("Width", "W", Resources.Style_WidthDesc, GH_ParamAccess.item, 1000);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Style", "S", Resources.Chart_StyleDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Color> colors = new List<Color>();
            GridAddress address = new GridAddress(1, 1);
            int width = 1000;

            DA.GetData<GridAddress>(1, ref address);
            DA.GetData<int>(2, ref width);

            // create style
            ScatterPlotMatrixStyle style = new ScatterPlotMatrixStyle();

            if (DA.GetDataList<Color>(0, colors))
            {
                List<string> hexColors = colors.Select(x => ChartsUtilities.ColorToHexString(Color.FromArgb(x.A, x.R, x.G, x.B))).ToList();
                style.Colors = new JavaScriptSerializer().Serialize(hexColors);
            }
            else
            {
                style.Colors = null;
            }

            style.GridRow = address.X;
            style.GridColumn = address.Y;
            style.Width = width;
            style.SizeX = (int)Math.Ceiling(width / 100d);
            style.SizeY = (int)Math.Ceiling(width / 100d);

            DA.SetData(0, style);
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
                return Resources.Charts_ScatterPlotMatrix_Style;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{21bcd9e8-7ccd-47a7-9b14-6c48e34dafb6}"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }
    }
}