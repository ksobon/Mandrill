using System;
using System.Drawing;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using D3jsLib;
using D3jsLib.BarChart;
using D3jsLib.Utilities;

namespace Mandrill.Grasshopper.Components.Charts.BarChart
{
    public class Style : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Style()
          : base("BarChartStyle", "Style",
              "Create a Bar Chart style object.",
              Resources.CategoryName, Resources.SubCategoryBarChart)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("BarColor", "BC", Resources.Style_BarColorDesc, GH_ParamAccess.item, Color.FromArgb(50, 130, 190));
            pManager.AddColourParameter("BarHoverColor", "HC", Resources.Style_HoverColorDesc, GH_ParamAccess.item, Color.FromArgb(255, 0, 0));
            pManager.AddGenericParameter("Address", "A", Resources.Style_AddressDesc, GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("Margins", "M", Resources.Style_MarginsDesc, GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("Width", "W", Resources.Style_WidthDesc, GH_ParamAccess.item, 1000);
            pManager.AddIntegerParameter("Height", "H", Resources.Style_HeightDesc, GH_ParamAccess.item, 500);
            pManager.AddTextParameter("YAxisLabel", "L", Resources.Style_YAxisLabelDesc, GH_ParamAccess.item, "Label");
            pManager.AddIntegerParameter("TickMarksX", "T", Resources.Style_TickMarksXDesc, GH_ParamAccess.item, 2);
            pManager.AddBooleanParameter("XTextRotation", "R", Resources.Style_XTextRotationDesc, GH_ParamAccess.item, false);
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
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var barColor = Color.FromArgb(50, 130, 190);
            var hoverColor = Color.FromArgb(255, 0, 0);
            var address = new GridAddress(1,1);
            var width = 1000;
            var height = 500;
            var yAxisLabel = "Label";
            var tickMarks = 2;
            var xRotation = false;
            var margins = new Margins();

            DA.GetData(0, ref barColor);
            DA.GetData(1, ref hoverColor);
            DA.GetData(2, ref address);
            DA.GetData(3, ref margins);
            DA.GetData(4, ref width);
            DA.GetData(5, ref height);
            DA.GetData(6, ref yAxisLabel);
            DA.GetData(7, ref tickMarks);
            DA.GetData(8, ref xRotation);

            // create style
            var style = new BarStyle();
            style.BarColor = ChartsUtilities.ColorToHexString(barColor);
            style.BarHoverColor = ChartsUtilities.ColorToHexString(hoverColor);
            style.GridRow = address.X;
            style.GridColumn = address.Y;
            style.Width = width;
            style.Height = height;
            style.YAxisLabel = yAxisLabel;
            style.TickMarksX = tickMarks;
            style.xTextRotation = xRotation;
            style.Margins = margins;
            style.SizeX = (int)Math.Ceiling(width / 100d);
            style.SizeY = (int)Math.Ceiling(height / 100d);

            DA.SetData(0, style);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resources.Charts_BarChart_Style;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{191d1721-0bcb-4f5a-8ee3-edfb406a64e3}"); }
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }
    }
}
