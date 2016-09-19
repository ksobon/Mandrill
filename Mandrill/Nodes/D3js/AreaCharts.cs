using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using D3jsLib.d3AreaCharts;
using sColor = System.Windows.Media.Color;
using System;
using D3jsLib;

namespace Charts
{
    /// <summary>
    ///     Area Chart
    /// </summary>
    public class AreaChart
    {
        internal AreaChart()
        {
        }

        /// <summary>
        ///     Area Chart Style object.
        /// </summary>
        /// <param name="Width">Width of the entire chart in pixels.</param>
        /// <param name="Height">Height of the entire chart in pixels.</param>
        /// <param name="YAxisLabel">Text used to label Y Axis.</param>
        /// <param name="AreaColor">Color for Area Chart fill.</param>
        /// <param name="TickMarksX">Approximate number of tick marks on X Axis.</param>
        /// <returns name="Style">Area Chart Style.</returns>
        /// <search>area, chart, style</search>
        public static AreaChartStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,50,130,190)")] DSCore.Color AreaColor,
            int Width = 1000,
            int Height = 500,
            string YAxisLabel = "Label",
            int TickMarksX = 10)
        {
            AreaChartStyle style = new AreaChartStyle();
            style.Width = Width;
            style.Height = Height;
            style.YAxisLabel = YAxisLabel;
            style.AreaColor = sColor.FromArgb(AreaColor.Alpha, AreaColor.Red, AreaColor.Green, AreaColor.Blue);
            style.TickMarksX = TickMarksX;

            return style;
        }

        /// <summary>
        ///     Area Chart Data object.
        /// </summary>
        /// <param name="Names">Name property for each value.</param>
        /// <param name="Values">Numerical value to plot.</param>
        /// <param name="Domain">Optional domain for data set.</param>
        /// <returns name="Data">Area Chart Data.</returns>
        /// <search>area, chart, data</search>
        public static AreaChartData Data(
            List<string> Names, 
            List<double> Values, 
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] Domain Domain)
        {
            List<AreaChartDataPoint> dataPoints = Names.Zip(Values, (x, y) => new AreaChartDataPoint { name = x, value = y }).ToList();
            AreaChartData areaData = new AreaChartData();
            areaData.Data = dataPoints;
            areaData.Domain = Domain;
            return areaData;
        }

        /// <summary>
        ///     Area Chart Data from CSV File.
        /// </summary>
        /// <param name="FilePath">File Path to where CSV is stored.</param>
        /// <param name="Domain">Custom domain.</param>
        /// <returns name="Data">Area Chart Data.</returns>
        /// <search>area, chart, data</search>
        public static AreaChartData DataFromCSV(
            string FilePath, 
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] Domain Domain)
        { 
            List<AreaChartDataPoint> dataPoints = new List<AreaChartDataPoint>();
            var csv = new List<string[]>();
            var lines = System.IO.File.ReadAllLines(FilePath);
            for (int i = 0; i < lines.Count(); i++)
            {
                string line = lines[i];
                if (i > 0)
                {
                    string lineName = line.Split(',')[0];
                    double lineValue = Convert.ToDouble(line.Split(',')[1]);
                    dataPoints.Add(new AreaChartDataPoint { name = lineName, value = lineValue });
                }
            }
            AreaChartData areaData = new AreaChartData();
            areaData.Data = dataPoints;
            areaData.Domain = Domain;
            return areaData;
        }

        /// <summary>
        ///     New Area Chart object
        /// </summary>
        /// <param name="Data">Area Chart Data.</param>
        /// <param name="Style">Area Chart Style.</param>
        /// <returns name="Chart">Area Chart.</returns>
        /// <search>area, chart</search>
        public static d3AreaChart Chart(AreaChartData Data, AreaChartStyle Style)
        {
            d3AreaChart chart = new d3AreaChart(Data, Style);
            return chart;
        }
    }
}
