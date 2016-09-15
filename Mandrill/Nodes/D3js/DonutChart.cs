using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using D3jsLib.DonutChart;
using sColor = System.Windows.Media.Color;
using System;
using D3jsLib;
using D3jsLib.Utilities;
using System.IO;

namespace Charts
{
    /// <summary>
    ///     Donut Chart
    /// </summary>
    public class DonutChart
    {
        internal DonutChart()
        {
        }

        /// <summary>
        ///     Donut Chart Style object.
        /// </summary>
        /// <param name="HoverColor">Hover over color.</param>
        /// <param name="Colors">List of optional colors for chart values.</param>
        /// <param name="Width">Width of chart in pixels.</param>
        /// <param name="Height">Height of chart in pixels.</param>
        /// <param name="Labels">Boolean value that controls if Labels are displayed.</param>
        /// <param name="Legend">Boolean value that controls if Legend is displayed.</param>
        /// <returns name="Style">Donut Chart Object.</returns>
        /// <search>donut, chart, style</search>
        public static DonutChartStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,255,0,0)")] DSCore.Color HoverColor,
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] List<DSCore.Color> Colors,
            int Width = 1000,
            int Height = 500,
            bool Labels = true,
            bool Legend = false)
        {
            DonutChartStyle style = new DonutChartStyle();
            style.Width = Width;
            style.Height = Height;
            style.HoverColor = sColor.FromArgb(HoverColor.Alpha, HoverColor.Red, HoverColor.Green, HoverColor.Blue);
            style.Labels = Labels;
            style.Legend = Legend;

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
        ///     Donut Chart Data object.
        /// </summary>
        /// <param name="Names"></param>
        /// <param name="Values"></param>
        /// <returns name="Data"></returns>
        /// <search></search>
        public static DonutChartData Data(
            List<string> Names,
            List<double> Values)
        {
            List<DonutChartDataPoint> dataPoints = Names.Zip(Values, (x, y) => new DonutChartDataPoint { name = x, value = y }).ToList();
            DonutChartData data = new DonutChartData();
            data.Data = dataPoints;

            return data;
        }

        /// <summary>
        ///     Donut Chart Data from CSV.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns name="Data"></returns>
        /// <search></search>
        public static DonutChartData DataFromCSV(object FilePath)
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

            List<DonutChartDataPoint> dataPoints = new List<DonutChartDataPoint>();
            var csv = new List<string[]>();
            var lines = System.IO.File.ReadAllLines(_filePath);
            for (int i = 0; i < lines.Count(); i++)
            {
                string line = lines[i];
                if (i > 0)
                {
                    string lineName = line.Split(',')[0];
                    double lineValue = Convert.ToDouble(line.Split(',')[1]);
                    dataPoints.Add(new DonutChartDataPoint { name = lineName, value = lineValue });
                }
            }
            DonutChartData data = new DonutChartData();
            data.Data = dataPoints;

            return data;
        }

        /// <summary>
        ///     Donut Chart object.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Style"></param>
        /// <returns name="Chart"></returns>
        /// <search></search>
        public static D3jsLib.DonutChart.DonutChart Chart(DonutChartData Data, DonutChartStyle Style)
        {
            D3jsLib.DonutChart.DonutChart chart = new D3jsLib.DonutChart.DonutChart(Data, Style);
            return chart;
        }
    }
}