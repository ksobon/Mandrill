using D3jsLib.GroupedBarChart;
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
        /// <param name="BarHoverColor"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="YAxisLabel"></param>
        /// <param name="Colors"></param>
        /// <returns name="Style">Bar Chart Style object.</returns>
        /// <search>grouped, bar, chart, style</search>
        public static GroupedBarChartStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,255,0,0)")] DSCore.Color BarHoverColor,
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] List<DSCore.Color> Colors,
            int Width = 1000,
            int Height = 500,
            string YAxisLabel = "Label"
            )
        {
            GroupedBarChartStyle style = new GroupedBarChartStyle();
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
            List<GroupedBarChartDataPoint> dataPoints = new List<GroupedBarChartDataPoint>();
            foreach (List<object> subList in Values)
            {
                GroupedBarChartDataPoint dataPoint = new GroupedBarChartDataPoint();
                dataPoint.Name = subList[0].ToString();
                Dictionary<string, double> values = new Dictionary<string, double>();
                for (int i = 1; i < subList.Count(); i++)
                {
                    values.Add(Headers[i], Convert.ToDouble(subList[i]));
                }
                dataPoint.Values = values;
                dataPoints.Add(dataPoint);
            }
            GroupedBarChartData data = new GroupedBarChartData();
            data.Data = dataPoints;
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

            List<GroupedBarChartDataPoint> dataPoints = new List<GroupedBarChartDataPoint>();
            var csv = new List<string[]>();
            var lines = File.ReadAllLines(_filePath);

            string[] headersArray = lines[0].Split(',');
            for (int i = 1; i < lines.Count(); i++)
            {
                GroupedBarChartDataPoint dataPoint = new GroupedBarChartDataPoint();
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

            GroupedBarChartData data = new GroupedBarChartData();
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
        public static D3jsLib.GroupedBarChart.GroupedBarChart Chart(GroupedBarChartData Data, GroupedBarChartStyle Style)
        {
            D3jsLib.GroupedBarChart.GroupedBarChart chart = new D3jsLib.GroupedBarChart.GroupedBarChart(Data, Style);
            return chart;
        }
    }
}
