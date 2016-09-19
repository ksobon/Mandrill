using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using D3jsLib.d3BarCharts;
using sColor = System.Windows.Media.Color;
using D3jsLib;
using System;
using System.IO;

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
        /// <param name="Width">Width of the entire chart in pixels.</param>
        /// <param name="Height">Height of the entire chart in pixels.</param>
        /// <param name="YAxisLabel">Text displayed in top-left corner of chart.</param>
        /// <param name="TickMarksX">Approximate number of tick mark values for X Axis.</param>
        /// <returns name="Style">Bar Chart Style.</returns>
        /// <search>bar, chart, style</search>
        public static BarStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,50,130,190)")] DSCore.Color BarColor,
            [DefaultArgument("DSCore.Color.ByARGB(1,255,0,0)")] DSCore.Color BarHoverColor,
            int Width = 1000,
            int Height = 500,
            string YAxisLabel = "Label",
            int TickMarksX = 10)
        {
            BarStyle style = new BarStyle();
            style.BarColor = sColor.FromArgb(BarColor.Alpha, BarColor.Red, BarColor.Green, BarColor.Blue);
            style.BarHoverColor = sColor.FromArgb(BarHoverColor.Alpha, BarHoverColor.Red, BarHoverColor.Green, BarHoverColor.Blue);
            style.Width = Width;
            style.Height = Height;
            style.YAxisLabel = YAxisLabel;
            style.TickMarksX = TickMarksX;

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
            List<BarDataPoint> dataPoints = Names.Zip(Values, (x, y) => new BarDataPoint { name = x, value = y }).ToList();
            BarData barData = new BarData();
            barData.Data = dataPoints;
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

            List<BarDataPoint> dataPoints = new List<BarDataPoint>();
            var csv = new List<string[]>();
            var lines = File.ReadAllLines(_filePath);
            for (int i = 0; i < lines.Count(); i++)
            {
                string line = lines[i];
                if (i > 0)
                {
                    string dataName = line.Split(',')[0];
                    double dataValue = Convert.ToDouble(line.Split(',')[1]);
                    dataPoints.Add(new BarDataPoint { name = dataName, value = dataValue });
                }
            }
            BarData barData = new BarData();
            barData.Data = dataPoints;
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
        public static d3BarChart Chart(BarData Data, BarStyle Style)
        {
            d3BarChart chart = new d3BarChart(Data, Style);
            return chart;
        }
    }
}
