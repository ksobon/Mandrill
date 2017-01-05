using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using sColor = System.Drawing.Color;
using Autodesk.DesignScript.Runtime;
using D3jsLib.BarChart;
using D3jsLib;
using D3jsLib.Utilities;
using System.Web.Script.Serialization;

namespace Charts
{
    /// <summary>
    ///     Bar Charts Class.
    /// </summary>
    public class BarChart
    {
        internal BarChart()
        {
        }

        /// <summary>
        ///     Bar Chart Style object.
        /// </summary>
        /// <param name="BarColor">Fill color for bars.</param>
        /// <param name="BarHoverColor">Fill color when hovered over.</param>
        /// <param name="Address">Grid Coordinates.</param>
        /// <param name="Margins">Margins in pixels.</param>
        /// <param name="Width">Width of the entire chart in pixels.</param>
        /// <param name="Height">Height of the entire chart in pixels.</param>
        /// <param name="YAxisLabel">Text displayed in top-left corner of chart.</param>
        /// <param name="TickMarksX">Approximate number of tick mark values for X Axis.</param>
        /// <param name="xTextRotation">Indicates if labels along x-axis are rotated.</param>
        /// <param name="Labels">Indicates if labels with actual bar values appear above each bar.</param>
        /// <returns name="Style">Bar Chart Style.</returns>
        /// <search>bar, chart, style</search>
        public static BarStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,50,130,190)")] DSCore.Color BarColor,
            [DefaultArgument("DSCore.Color.ByARGB(1,255,0,0)")] DSCore.Color BarHoverColor,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            [DefaultArgument("Charts.MiscNodes.Margins(20,40,20,40)")] Margins Margins,
            int Width = 1000,
            int Height = 500,
            string YAxisLabel = "Label",
            int TickMarksX = 10,
            bool xTextRotation = false,
            bool Labels = false)
        {
            BarStyle style = new BarStyle();
            style.BarColor = ChartsUtilities.ColorToHexString(sColor.FromArgb(BarColor.Alpha, BarColor.Red, BarColor.Green, BarColor.Blue));
            style.BarHoverColor = ChartsUtilities.ColorToHexString(sColor.FromArgb(BarHoverColor.Alpha, BarHoverColor.Red, BarHoverColor.Green, BarHoverColor.Blue));
            style.Width = Width;
            style.Height = Height;
            style.YAxisLabel = YAxisLabel;
            style.TickMarksX = TickMarksX;
            style.xTextRotation = xTextRotation;
            style.Margins = Margins;
            style.SizeX = (int)Math.Ceiling(Width / 100d);
            style.SizeY = (int)Math.Ceiling(Height / 100d);
            style.Labels = Labels;

            if (Address != null)
            {
                style.GridRow = Address.X;
                style.GridColumn = Address.Y;
            }
            else
            {
                style.GridRow = 1;
                style.GridColumn = 1;
            }

            return style;
        }

        /// <summary>
        ///     Bar Chart Data object.
        /// </summary>
        /// <param name="Names">Name property for each value.</param>
        /// <param name="Values">Numerical value to plot.</param>
        /// <param name="Domain">Optional domain for data set.</param>
        /// <returns name="Data">Bar Chart Data</returns>
        /// <search>bar, chart, data</search>
        public static BarData Data(
            List<string> Names, 
            List<double> Values, 
            [DefaultArgument("Charts.MiscNodes.GetNull()")]Domain Domain)
        {
            List<DataPoint1> dataPoints = Names.Zip(Values, (x, y) => new DataPoint1 { name = x, value = y }).ToList();
            BarData barData = new BarData();
            barData.Data = new JavaScriptSerializer().Serialize(dataPoints);
            barData.Domain = Domain;
            return barData;
        }

        /// <summary>
        ///     Bar Chart Data object.
        /// </summary>
        /// <param name="FilePath">CSV File Path.</param>
        /// <param name="Domain">Optional Domain for data set.</param>
        /// <returns name="Data">Bar Chart Data</returns>
        /// <search>bar, chart, data</search>
        public static BarData DataFromCSV(
            object FilePath,
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")]Domain Domain)
        {
            // get full path to file as string
            // if File.FromPath is used it returns FileInfo class
            string _filePath = "";
            try
            {
                _filePath = (string)FilePath;
            }
            catch
            {
                _filePath = ((FileInfo)FilePath).FullName;
            }

            BarData barData = new BarData();
            barData.Data = new JavaScriptSerializer().Serialize(ChartsUtilities.Data1FromCSV(_filePath));
            barData.Domain = Domain;
            return barData;
        }

        /// <summary>
        ///     New Bar Chart object.
        /// </summary>
        /// <param name="Data">Bar Chart Data.</param>
        /// <param name="Style">Bar Chart Style.</param>
        /// <returns name="Chart">Generated Bar Chart.</returns>
        /// <search>bar, chart</search>
        public static D3jsLib.BarChart.BarChart Chart(BarData Data, BarStyle Style)
        {
            D3jsLib.BarChart.BarChart chart = new D3jsLib.BarChart.BarChart(Data, Style);
            return chart;
        }
    }
}
