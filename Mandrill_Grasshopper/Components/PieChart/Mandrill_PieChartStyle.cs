using System;
using System.Drawing;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib;
using D3jsLib.PieChart;
using System.Collections.Generic;
using System.Linq;
using D3jsLib.Utilities;
using System.Web.Script.Serialization;

namespace Mandrill_Grasshopper.Components.PieChart
{
    public class Mandrill_PieChartStyle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mandrill_PieChartStyle class.
        /// </summary>
        public Mandrill_PieChartStyle()
          : base("PieChartStyle", "Style",
              "Pie Chart Style",
              Resources.CategoryName, Resources.SubCategoryDonutChartDesc)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("HoverColor", "HC", Resources.Style_HoverColorDesc, GH_ParamAccess.item, Color.FromArgb(255, 0, 0));
            pManager.AddColourParameter("Colors", "C", Resources.Style_ColorsDesc, GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Address", "A", Resources.Style_AddressDesc, GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("Margins", "M", Resources.Style_MarginsDesc, GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("Width", "W", Resources.Style_WidthDesc, GH_ParamAccess.item, 400);
            pManager.AddBooleanParameter("Labels", "L", Resources.Style_LabelsDesc, GH_ParamAccess.item, true);
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
            Color hoverColor = Color.FromArgb(255, 0, 0);
            List<Color> colors = new List<Color>();
            GridAddress address = new GridAddress(1, 1);
            Margins margins = new Margins();
            int width = 400;
            bool labels = true;

            DA.GetData<Color>(0, ref hoverColor);
            DA.GetData<GridAddress>(2, ref address);
            DA.GetData<Margins>(3, ref margins);
            DA.GetData<int>(4, ref width);
            DA.GetData<bool>(5, ref labels);

            // create style
            PieChartStyle style = new PieChartStyle();

            if (DA.GetDataList<Color>(1, colors))
            {
                List<string> hexColors = colors.Select(x => ChartsUtilities.ColorToHexString(Color.FromArgb(x.A, x.R, x.G, x.B))).ToList();
                style.Colors = new JavaScriptSerializer().Serialize(hexColors);
            }
            else
            {
                style.Colors = null;
            }

            style.HoverColor = ChartsUtilities.ColorToHexString(hoverColor);
            style.GridRow = address.X;
            style.GridColumn = address.Y;
            style.Width = width;
            style.Labels = labels;
            style.Margins = margins;
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
                return Resources.Charts_PieChart_Style;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{e49db6a5-e678-4db3-a4cf-badbf17f3b85}"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }
    }
}