using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using D3jsLib.DonutChart;
using sColor = System.Drawing.Color;
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
        /// <param name="Address">Grid Coordinates.</param>
        /// <param name="Margins">Margins in pixels.</param>
        /// <param name="Width">Width of chart in pixels.</param>
        /// <param name="Labels">Boolean value that controls if Labels are displayed.</param>
        /// <param name="TotalLabel">Text appearing at center of the Donut chart.</param>
        /// <returns name="Style">Donut Chart Object.</returns>
        /// <search>donut, chart, style</search>
        public static DonutChartStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,255,0,0)")] DSCore.Color HoverColor,
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] List<DSCore.Color> Colors,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            [DefaultArgument("Charts.MiscNodes.Margins()")] Margins Margins,
            int Width = 400,
            bool Labels = true,
            string TotalLabel = "TOTAL")
        {
            DonutChartStyle style = new DonutChartStyle();
            style.Width = Width;
            style.HoverColor = sColor.FromArgb(HoverColor.Alpha, HoverColor.Red, HoverColor.Green, HoverColor.Blue);
            style.Labels = Labels;
            style.TotalLabel = TotalLabel;
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
        ///     Donut Chart Data object.
        /// </summary>
        /// <param name="Names">Names of each data points.</param>
        /// <param name="Values">Values of each data point.</param>
        /// <returns name="Data">Donut Chart Data</returns>
        /// <search>data, donut, chart, donut chart data</search>
        public static DonutChartData Data(
            List<string> Names,
            List<double> Values)
        {
            List<DataPoint1> dataPoints = Names.Zip(Values, (x, y) => new DataPoint1 { name = x, value = y }).ToList();
            DonutChartData data = new DonutChartData();
            data.Data = dataPoints;

            return data;
        }

        /// <summary>
        ///     Donut Chart Data from CSV.
        /// </summary>
        /// <param name="FilePath">File Path to CSV file.</param>
        /// <returns name="Data">Donut Chart Data</returns>
        /// <search>csv, donut, chart, data</search>
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

            List<DataPoint1> dataPoints = new List<DataPoint1>();
            var csv = new List<string[]>();
            var lines = System.IO.File.ReadAllLines(_filePath);
            for (int i = 0; i < lines.Count(); i++)
            {
                string line = lines[i];
                if (i > 0)
                {
                    string lineName = line.Split(',')[0];
                    double lineValue = Convert.ToDouble(line.Split(',')[1]);
                    dataPoints.Add(new DataPoint1 { name = lineName, value = lineValue });
                }
            }
            DonutChartData data = new DonutChartData();
            data.Data = dataPoints;

            return data;
        }

        /// <summary>
        ///     Donut Chart object.
        /// </summary>
        /// <param name="Data">Donut Chart Data object.</param>
        /// <param name="Style">Donut Chart Style object.</param>
        /// <returns name="Chart">Donut Chart</returns>
        /// <search>donut chart, chart, donut</search>
        public static D3jsLib.DonutChart.DonutChart Chart(DonutChartData Data, DonutChartStyle Style)
        {
            D3jsLib.DonutChart.DonutChart chart = new D3jsLib.DonutChart.DonutChart(Data, Style);
            return chart;
        }
    }
}