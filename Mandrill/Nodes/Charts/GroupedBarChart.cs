using D3jsLib.GroupedBarChart;
using System.Collections.Generic;
using sColor = System.Drawing.Color;
using D3jsLib.Utilities;
using Autodesk.DesignScript.Runtime;
using D3jsLib;
using System.IO;
using System.Linq;
using System;
using System.Web.Script.Serialization;

namespace Charts
{
    /// <summary>
    ///     Grouped Bar Chart nodes.
    /// </summary>
    public class GroupedBarChart
    {
        internal GroupedBarChart()
        {
        }

        /// <summary>
        ///     Grouped Bar Chart Style.
        /// </summary>
        /// <param name="BarHoverColor">Hover over color.</param>
        /// <param name="Address">Grid Coordinates</param>
        /// <param name="Margins">Margins in pixels.</param>
        /// <param name="Width">Width in pixels.</param>
        /// <param name="Height">Height in pixels.</param>
        /// <param name="YAxisLabel">Label for Y-Axis.</param>
        /// <param name="Colors">Optional list of colors for each group.</param>
        /// <returns name="Style">Bar Chart Style object.</returns>
        /// <search>grouped, bar, chart, style</search>
        public static GroupedBarChartStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,255,0,0)")] DSCore.Color BarHoverColor,
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] List<DSCore.Color> Colors,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            [DefaultArgument("Charts.MiscNodes.Margins()")] Margins Margins,
            int Width = 1000,
            int Height = 500,
            string YAxisLabel = "Label"
            )
        {
            GroupedBarChartStyle style = new GroupedBarChartStyle();
            style.Width = Width;
            style.Height = Height;
            style.YAxisLabel = YAxisLabel;
            style.BarHoverColor = ChartsUtilities.ColorToHexString(sColor.FromArgb(BarHoverColor.Alpha, BarHoverColor.Red, BarHoverColor.Green, BarHoverColor.Blue));
            style.Margins = Margins;
            style.SizeX = (int)Math.Ceiling(Width / 100d);
            style.SizeY = (int)Math.Ceiling(Height / 100d);

            if (Colors != null)
            {
                List<string> hexColors = Colors.Select(x => ChartsUtilities.ColorToHexString(sColor.FromArgb(x.Alpha, x.Red, x.Green, x.Blue))).ToList();
                style.Colors = new JavaScriptSerializer().Serialize(hexColors);
            }
            else
            {
                style.Colors = null;
            }

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
        ///     Grouped Bar Chart Data.
        /// </summary>
        /// <param name="Headers"></param>
        /// <param name="Values"></param>
        /// <param name="Domain"></param>
        /// <returns name="Data">Grouped Bar Chart Data object</returns>
        public static GroupedBarChartData Data(
            List<string> Headers,
            List<List<object>> Values,
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] Domain Domain)
        {
            GroupedBarChartData data = new GroupedBarChartData();
            data.Data = ChartsUtilities.DataToJsonString(ChartsUtilities.Data2FromList(Headers, Values));
            data.Domain = Domain;

            return data;
        }

        /// <summary>
        ///     Grouped Bar Chart Data object.
        /// </summary>
        /// <param name="FilePath">Path to a CSV file.</param>
        /// <param name="Domain">Y Domain of a chart.</param>
        /// <returns name="Data">Data Class for use with Grouped Bar Chart.</returns>
        /// <search>data, grouped bar chart, chart, bar, grouped</search>
        public static GroupedBarChartData DataFromCSV(
            object FilePath,
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] Domain Domain)
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

            GroupedBarChartData data = new GroupedBarChartData();
            data.Data = ChartsUtilities.DataToJsonString(ChartsUtilities.Data2FromCSV(_filePath));
            data.Domain = Domain;

            return data;
        }

        /// <summary>
        ///     New Grouped Bar Chart object.
        /// </summary>
        /// <param name="Data">Bar Chart Data.</param>
        /// <param name="Style">Bar Chart Style.</param>
        /// <returns name="Chart">Generated Bar Chart.</returns>
        /// <search>bar, chart, grouped</search>
        public static D3jsLib.GroupedBarChart.GroupedBarChart Chart(GroupedBarChartData Data, GroupedBarChartStyle Style)
        {
            D3jsLib.GroupedBarChart.GroupedBarChart chart = new D3jsLib.GroupedBarChart.GroupedBarChart(Data, Style);
            return chart;
        }
    }
}
