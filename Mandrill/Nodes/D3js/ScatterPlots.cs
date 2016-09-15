using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using D3jsLib.d3ScatterPlots;
using sColor = System.Windows.Media.Color;
using D3jsLib.Utilities;
using System;
using D3jsLib;

namespace Charts
{
    /// <summary>
    ///     Scatter Plot nodes.
    /// </summary>
    public class ScatterPlot
    {
        internal ScatterPlot()
        {
        }

        /// <summary>
        ///     Scatter Plot Style.
        /// </summary>
        /// <param name="Width">Width in pixels.</param>
        /// <param name="Height">Height in pixels.</param>
        /// <param name="YAxisLabel">Label displayed for Y Axis.</param>
        /// <param name="XAxisLabel">Label displayed for X Axis.</param>
        /// <param name="DotColor">Color of Scatter Plot dot.</param>
        /// <returns name="Style">Scatter Plot Style.</returns>
        /// <search>style, scatter plot</search>
        public static ScatterPlotStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,100,100,100)")] DSCore.Color DotColor,
            int Width = 1000,
            int Height = 500,
            string YAxisLabel = "Label",
            string XAxisLabel = "Label")
        {
            ScatterPlotStyle style = new ScatterPlotStyle();
            style.Width = Width;
            style.Height = Height;
            style.YAxisLabel = YAxisLabel;
            style.XAxisLabel = XAxisLabel;
            style.DotColor = sColor.FromArgb(DotColor.Alpha, DotColor.Red, DotColor.Green, DotColor.Blue);

            return style;
        }

        /// <summary>
        ///     Scatter Plot Data.
        /// </summary>
        /// <param name="Name">Names for Data Points.</param>
        /// <param name="ValueX">Value along X Axis.</param>
        /// <param name="ValueY">Value along Y Axis.</param>
        /// <param name="Size">Size of displayed dot.</param>
        /// <param name="DomainX">Boundary domain for values along X Axis.</param>
        /// <param name="DomainY">Boundary domain for values along Y Axis.</param>
        /// <returns name="Data">Scatter Plot Data.</returns>
        /// <search>scatter plot, data</search>
        public static ScatterPlotData Data(
            List<string> Name, 
            List<double> ValueX, 
            List<double> ValueY, 
            List<double> Size, 
            [DefaultArgument("Charts.MiscNodes.GetNull()")] Domain DomainX, 
            [DefaultArgument("Charts.MiscNodes.GetNull()")] Domain DomainY)
        {
            List<ScatterPlotDataPoint> dataPoints = Name
                .ZipFour(ValueX, ValueY, Size, (x, y, z, v) => new ScatterPlotDataPoint { name = x, valueX = y, valueY = z, size = v })
                .ToList();

            ScatterPlotData spData = new ScatterPlotData();
            spData.Data = dataPoints;
            spData.DomainX = DomainX;
            spData.DomainY = DomainY;
            return spData;
        }

        /// <summary>
        ///     Scatter Plot Data from CSV File.
        /// </summary>
        /// <param name="FilePath">File Path to where CSV is stored.</param>
        /// <param name="DomainX">Boundary domain for values along X Axis.</param>
        /// <param name="DomainY">Boundary domain for values along Y Axis.</param>
        /// <returns name="Data">Scatter Plot Data.</returns>
        /// <search>scatter plot, data</search>
        public static ScatterPlotData DataFromCSV(
            string FilePath,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] Domain DomainX,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] Domain DomainY)
        {
            List<ScatterPlotDataPoint> dataPoints = new List<ScatterPlotDataPoint>();
            var csv = new List<string[]>();
            var lines = System.IO.File.ReadAllLines(FilePath);
            for (int i = 0; i < lines.Count(); i++)
            {
                string line = lines[i];
                if (i > 0)
                {
                    string lineName = line.Split(',')[0];
                    double lineValueX = Convert.ToDouble(line.Split(',')[1]);
                    double lineValueY = Convert.ToDouble(line.Split(',')[2]);
                    double lineSize = Convert.ToDouble(line.Split(',')[3]);
                    string lineColor = line.Split(',')[4];

                    dataPoints.Add(new ScatterPlotDataPoint { name = lineName, valueX = lineValueX, valueY = lineValueY, size = lineSize, color = lineColor});
                }
            }
            ScatterPlotData scatterData = new ScatterPlotData();
            scatterData.Data = dataPoints;
            scatterData.DomainX = DomainX;
            scatterData.DomainY = DomainY;
            return scatterData;
        }

        /// <summary>
        ///     Scatter Plot Chart.
        /// </summary>
        /// <param name="Data">Scatter Plot Data.</param>
        /// <param name="Style">Scatter Plot Style.</param>
        /// <returns name="Chart">Scatter Plot Chart.</returns>
        /// <search>scatter plot, chart</search>
        public static d3ScatterPlot Chart(ScatterPlotData Data, ScatterPlotStyle Style)
        {
            d3ScatterPlot chart = new d3ScatterPlot(Data, Style);
            return chart;
        }
    }
}
