using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using D3jsLib.LineChart;
using sColor = System.Drawing.Color;
using D3jsLib;
using System.IO;
using System;

namespace Charts
{
    /// <summary>
    ///     Line Chart nodes
    /// </summary>
    public class LineChart
    {
        internal LineChart()
        {
        }

        /// <summary>
        ///     Line Chart Style object.
        /// </summary>
        /// <param name="Address">Grid Coordinates.</param>
        /// <param name="Margins">Margins in pixels.</param>
        /// <param name="Width">Width of the entire chart in pixels.</param>
        /// <param name="Height">Height of the entire chart in pixels.</param>
        /// <param name="YAxisLabel">Text used to label Y Axis.</param>
        /// <param name="LineColor">Color for Line Chart Line.</param>
        /// <param name="TickMarksX">Approximate number of tick mark values for X Axis.</param>
        /// <returns name="Style">Line Chart Style.</returns>
        /// <search>line, style</search>
        public static LineChartStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,50,130,190)")] DSCore.Color LineColor,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            [DefaultArgument("Charts.MiscNodes.Margins()")] Margins Margins,
            int Width = 1000,
            int Height = 500,
            string YAxisLabel = "Label",
            int TickMarksX = 10)
        {
            LineChartStyle style = new LineChartStyle();
            style.Width = Width;
            style.Height = Height;
            style.YAxisLabel = YAxisLabel;
            style.LineColor = sColor.FromArgb(LineColor.Alpha, LineColor.Red, LineColor.Green, LineColor.Blue);
            style.TickMarksX = TickMarksX;
            style.Margins = Margins;

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
        ///     Line Chart Data object.
        /// </summary>
        /// <param name="Names">Name property for each value.</param>
        /// <param name="Values">Numerical value.</param>
        /// <param name="Domain">Optional domain for data set.</param>
        /// <returns name="Data">Line Chart Data.</returns>
        /// <search>data</search>
        public static LineChartData Data(
            List<string> Names, 
            List<double> Values, 
            [DefaultArgument("Charts.MiscNodes.GetNull()")] Domain Domain)
        {
            List<DataPoint1> dataPoints = Names.Zip(Values, (x, y) => new DataPoint1 { name = x, value = y }).ToList();
            LineChartData lineData = new LineChartData();
            lineData.Data = dataPoints;
            lineData.Domain = Domain;
            return lineData;
        }

        /// <summary>
        ///     Line Chart Data object.
        /// </summary>
        /// <param name="FilePath">CSV File Path.</param>
        /// <param name="Domain">Optional Domain for data set.</param>
        /// <returns name="Data">Line Chart Data</returns>
        /// <search>line, chart, data</search>
        public static LineChartData DataFromCSV(
            string FilePath,
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] Domain Domain)
        {
            List<DataPoint1> dataPoints = new List<DataPoint1>();
            var csv = new List<string[]>();
            var lines = File.ReadAllLines(FilePath);
            for (int i = 0; i < lines.Count(); i++)
            {
                string line = lines[i];
                if (i > 0)
                {
                    string dataName = line.Split(',')[0];
                    double dataValue = Convert.ToDouble(line.Split(',')[1]);
                    dataPoints.Add(new DataPoint1 { name = dataName, value = dataValue });
                }
            }
            LineChartData lineData = new LineChartData();
            lineData.Data = dataPoints;
            lineData.Domain = Domain;
            return lineData;
        }

        /// <summary>
        ///     New Line Chart object.
        /// </summary>
        /// <param name="Data">Line Chart Data.</param>
        /// <param name="Style">Line Chart Style.</param>
        /// <returns name="Chart">Line Chart.</returns>
        /// <search>line, chart</search>
        public static d3LineChart Chart(LineChartData Data, LineChartStyle Style)
        {
            d3LineChart chart = new d3LineChart(Data, Style);
            return chart;
        }
    }
}