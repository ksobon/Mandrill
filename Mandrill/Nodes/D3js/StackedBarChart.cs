using D3jsLib.StackedBarChart;
using System.Collections.Generic;
using sColor = System.Windows.Media.Color;
using D3jsLib.Utilities;
using Autodesk.DesignScript.Runtime;
using D3jsLib;
using System.IO;
using System.Linq;
using System;

namespace Charts
{
    /// <summary>
    ///     Stacked Bar Chart nodes.
    /// </summary>
    public class StackedBarChart
    {
        internal StackedBarChart()
        {
        }

        /// <summary>
        ///     Stacked Bar Chart Style.
        /// </summary>
        /// <param name="BarHoverColor"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="YAxisLabel"></param>
        /// <param name="Colors"></param>
        /// <returns name="Style">Stacked Bar Chart Style</returns>
        /// <search>stacked, bar, chart, style</search>
        public static StackedBarChartStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,255,0,0)")] DSCore.Color BarHoverColor,
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] List<DSCore.Color> Colors,
            int Width = 1000,
            int Height = 500,
            string YAxisLabel = "Label"
            )
        {
            StackedBarChartStyle style = new StackedBarChartStyle();
            style.Width = Width;
            style.Height = Height;
            style.YAxisLabel = YAxisLabel;
            style.BarHoverColor = sColor.FromArgb(BarHoverColor.Alpha, BarHoverColor.Red, BarHoverColor.Green, BarHoverColor.Blue);

            if (Colors != null)
            {
                List<string> hexColors = new List<string>();
                foreach (DSCore.Color color in Colors)
                {
                    string col = ChartsUtilities.ColorToHexString(sColor.FromArgb(color.Alpha, color.Red, color.Green, color.Blue));
                    hexColors.Add(col);
                }
                style.Colors = hexColors;
            }
            else
            {
                style.Colors = null;
            }

            return style;
        }

        /// <summary>
        ///     Staked Bar Chart Data object.
        /// </summary>
        /// <param name="Headers">Names of all values that will be grouped. First value is always "Name".</param>
        /// <param name="Values">Nested List of values where first item in a sub-list is Group Name. Following items 
        /// must match number of value names defined in Headers input. </param>
        /// <param name="Domain">Y Domain for the Chart.</param>
        /// <returns name="Data">Stacked Bar Chart Data</returns>
        /// <search>data, stacked, bar, chart</search>
        public static StackedBarChartData Data(
            List<string> Headers,
            List<List<object>> Values,
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] Domain Domain)
        {
            List<StackedBarChartDataPoint> dataPoints = new List<StackedBarChartDataPoint>();
            foreach (List<object> subList in Values)
            {
                StackedBarChartDataPoint dataPoint = new StackedBarChartDataPoint();
                dataPoint.Name = subList[0].ToString();
                Dictionary<string, double> values = new Dictionary<string, double>();
                for (int i = 1; i < subList.Count(); i++)
                {
                    values.Add(Headers[i], Convert.ToDouble(subList[i]));
                }
                dataPoint.Values = values;
                dataPoints.Add(dataPoint);
            }
            StackedBarChartData data = new StackedBarChartData();
            data.Data = dataPoints;
            data.Domain = Domain;

            return data;
        }

        /// <summary>
        ///     Stacked Bar Chart Data object.
        /// </summary>
        /// <param name="FilePath">Path to a CSV file.</param>
        /// <param name="Domain">Y Domain of a chart.</param>
        /// <returns name="Data">Data Class for use with Grouped Bar Chart.</returns>
        /// <search>data, stacked bar chart, chart, bar, grouped</search>
        public static StackedBarChartData DataFromCSV(
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

            List<StackedBarChartDataPoint> dataPoints = new List<StackedBarChartDataPoint>();
            var csv = new List<string[]>();
            var lines = File.ReadAllLines(_filePath);

            string[] headersArray = lines[0].Split(',');
            for (int i = 1; i < lines.Count(); i++)
            {
                StackedBarChartDataPoint dataPoint = new StackedBarChartDataPoint();
                dataPoint.Name = lines[i].Split(',')[0];

                string[] lineArray = lines[i].Split(',');
                Dictionary<string, double> values = new Dictionary<string, double>();
                for (int j = 1; j < lineArray.Count(); j++)
                {
                    values.Add(headersArray[j], Convert.ToDouble(lineArray[j]));
                }

                dataPoint.Values = values;
                dataPoints.Add(dataPoint);
            }

            StackedBarChartData data = new StackedBarChartData();
            data.Data = dataPoints;
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
        public static D3jsLib.StackedBarChart.StackedBarChart Chart(StackedBarChartData Data, StackedBarChartStyle Style)
        {
            D3jsLib.StackedBarChart.StackedBarChart chart = new D3jsLib.StackedBarChart.StackedBarChart(Data, Style);
            return chart;
        }
    }
}

