using D3jsLib.NormalizedStackedBarChart;
using System.Collections.Generic;
using sColor = System.Drawing.Color;
using D3jsLib.Utilities;
using Autodesk.DesignScript.Runtime;
using D3jsLib;
using System.IO;
using System.Linq;
using System;

namespace Charts
{
    /// <summary>
    ///     Normalized Stacked Bar Chart nodes.
    /// </summary>
    public class NormalizedStackedBarChart
    {
        internal NormalizedStackedBarChart()
        {
        }

        /// <summary>
        ///    Normalized Stacked Bar Chart Style.
        /// </summary>
        /// <param name="BarHoverColor"></param>
        /// <param name="Address">Grid Coordinates.</param>
        /// <param name="Margins">Margins in pixels.</param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="YAxisLabel"></param>
        /// <param name="Colors"></param>
        /// <returns name="Style">Normalized Stacked Bar Chart Style</returns>
        /// <search>normalized, stacked, bar, chart, style</search>
        public static NormalizedStackedBarChartStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,255,0,0)")] DSCore.Color BarHoverColor,
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] List<DSCore.Color> Colors,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            [DefaultArgument("Charts.MiscNodes.Margins()")] Margins Margins,
            int Width = 1000,
            int Height = 500,
            string YAxisLabel = "Label"
            )
        {
            NormalizedStackedBarChartStyle style = new NormalizedStackedBarChartStyle();
            style.Width = Width;
            style.Height = Height;
            style.YAxisLabel = YAxisLabel;
            style.BarHoverColor = sColor.FromArgb(BarHoverColor.Alpha, BarHoverColor.Red, BarHoverColor.Green, BarHoverColor.Blue);
            style.Margins = Margins;

            if (Colors != null)
            {
                List<string> hexColors = Colors.Select(x => ChartsUtilities.ColorToHexString(sColor.FromArgb(x.Alpha, x.Red, x.Green, x.Blue))).ToList();
                style.Colors = hexColors;
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
        ///     Normalized Stacked Bar Chart Data object.
        /// </summary>
        /// <param name="Headers">Names of all values that will be grouped. First value is always "Name".</param>
        /// <param name="Values">Nested List of values where first item in a sub-list is Group Name. Following items 
        /// must match number of value names defined in Headers input. </param>
        /// <returns name="Data">Normalized Stacked Bar Chart Data</returns>
        /// <search>normalized, data, stacked, bar, chart</search>
        public static NormalizedStackedBarChartData Data(
            List<string> Headers,
            List<List<object>> Values)
        {
            List<DataPoint2> dataPoints = new List<DataPoint2>();
            foreach (List<object> subList in Values)
            {
                DataPoint2 dataPoint = new DataPoint2();
                dataPoint.Name = subList[0].ToString();
                Dictionary<string, double> values = new Dictionary<string, double>();
                for (int i = 1; i < subList.Count(); i++)
                {
                    values.Add(Headers[i], Convert.ToDouble(subList[i]));
                }
                dataPoint.Values = values;
                dataPoints.Add(dataPoint);
            }
            NormalizedStackedBarChartData data = new NormalizedStackedBarChartData();
            data.Data = dataPoints;

            return data;
        }

        /// <summary>
        ///     Normalized Stacked Bar Chart Data object.
        /// </summary>
        /// <param name="FilePath">Path to a CSV file.</param>
        /// <returns name="Data">Data Class for use with Stacked Bar Chart.</returns>
        /// <search>normalized, data, stacked bar chart, chart, bar, grouped</search>
        public static NormalizedStackedBarChartData DataFromCSV(
            object FilePath)
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

            NormalizedStackedBarChartData data = new NormalizedStackedBarChartData();
            data.Data = ChartsUtilities.Data2FromCSV(_filePath);

            return data;
        }

        /// <summary>
        ///     New Normalized Stacked Bar Chart object.
        /// </summary>
        /// <param name="Data">Normalized Stacked Bar Chart Data.</param>
        /// <param name="Style">Normalized Stacked Bar Chart Style.</param>
        /// <returns name="Chart">Generated Bar Chart.</returns>
        /// <search>normalized, bar, chart, grouped</search>
        public static D3jsLib.NormalizedStackedBarChart.NormalizedStackedBarChart Chart(NormalizedStackedBarChartData Data, NormalizedStackedBarChartStyle Style)
        {
            D3jsLib.NormalizedStackedBarChart.NormalizedStackedBarChart chart = new D3jsLib.NormalizedStackedBarChart.NormalizedStackedBarChart(Data, Style);
            return chart;
        }
    }
}
