using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using D3jsLib.d3ScatterPlots;
using sColor = System.Drawing.Color;
using D3jsLib.Utilities;
using System;
using D3jsLib;
using System.Web.Script.Serialization;
using System.IO;

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
        /// <param name="Address">Grid Coordinates.</param>
        /// <param name="Margins">Margins in pixels.</param>
        /// <param name="Width">Width in pixels.</param>
        /// <param name="Height">Height in pixels.</param>
        /// <param name="YAxisLabel">Label displayed for Y Axis.</param>
        /// <param name="XAxisLabel">Label displayed for X Axis.</param>
        /// <param name="DotColor">Color of Scatter Plot dot.</param>
        /// <returns name="Style">Scatter Plot Style.</returns>
        /// <search>style, scatter plot</search>
        public static ScatterPlotStyle Style(
            [DefaultArgument("Charts.MiscNodes.GetColorList()")] List<DSCore.Color> DotColor,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            [DefaultArgument("Charts.MiscNodes.Margins()")] Margins Margins,
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

            List<string> hexColors = DotColor.Select(x => ChartsUtilities.ColorToHexString(sColor.FromArgb(x.Alpha, x.Red, x.Green, x.Blue))).ToList();
            style.DotColor = new JavaScriptSerializer().Serialize(hexColors);

            style.Margins = Margins;
            style.SizeX = (int)Math.Ceiling(Width / 100d);
            style.SizeY = (int)Math.Ceiling(Height / 100d);

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
        ///     Scatter Plot Data.
        /// </summary>
        /// <param name="Names">Names for Data Points.</param>
        /// <param name="ValueX">Value along X Axis.</param>
        /// <param name="ValueY">Value along Y Axis.</param>
        /// <param name="Size">Size of displayed dot.</param>
        /// <param name="Colors">Color of each dot.</param>
        /// <param name="DomainX">Boundary domain for values along X Axis.</param>
        /// <param name="DomainY">Boundary domain for values along Y Axis.</param>
        /// <returns name="Data">Scatter Plot Data.</returns>
        /// <search>scatter plot, data</search>
        public static ScatterPlotData Data(
            List<string> Names, 
            List<double> ValueX, 
            List<double> ValueY, 
            List<double> Size, 
            List<int> Colors,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] Domain DomainX, 
            [DefaultArgument("Charts.MiscNodes.GetNull()")] Domain DomainY)
        {
            List<ScatterPlotDataPoint> dataPoints = Names
                .ZipFive(ValueX, ValueY, Size, Colors, (x, y, z, v, w) => new ScatterPlotDataPoint { name = x, valueX = y, valueY = z, size = v, color = w })
                .ToList();

            ScatterPlotData spData = new ScatterPlotData();
            spData.Data = new JavaScriptSerializer().Serialize(dataPoints);
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
            object FilePath,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] Domain DomainX,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] Domain DomainY)
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

            List<ScatterPlotDataPoint> dataPoints = new List<ScatterPlotDataPoint>();
            var csv = new List<string[]>();
            var lines = System.IO.File.ReadAllLines(_filePath);
            for (int i = 1; i < lines.Count(); i++)
            {
                string line = lines[i];
                if (i > 0)
                {
                    string lineName = line.Split(',')[0];
                    double lineValueX = Convert.ToDouble(line.Split(',')[1]);
                    double lineValueY = Convert.ToDouble(line.Split(',')[2]);
                    double lineSize = Convert.ToDouble(line.Split(',')[3]);
                    int lineColor = Convert.ToInt32(line.Split(',')[4]);

                    dataPoints.Add(new ScatterPlotDataPoint { name = lineName, valueX = lineValueX, valueY = lineValueY, size = lineSize, color = lineColor});
                }
            }
            ScatterPlotData scatterData = new ScatterPlotData();
            scatterData.Data = new JavaScriptSerializer().Serialize(dataPoints);
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
