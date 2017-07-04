using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using D3jsLib.AreaCharts;
using sColor = System.Drawing.Color;
using System;
using D3jsLib;
using D3jsLib.Utilities;
using System.Web.Script.Serialization;
using System.IO;

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
        /// <param name="Address">Grid Coordinates.</param>
        /// <param name="Margins">Marings in pixels.</param>
        /// <param name="Width">Width of the entire chart in pixels.</param>
        /// <param name="Height">Height of the entire chart in pixels.</param>
        /// <param name="YAxisLabel">Text used to label Y Axis.</param>
        /// <param name="AreaColor">Color for Area Chart fill.</param>
        /// <param name="TickMarksX">Approximate number of tick marks on X Axis.</param>
        /// <param name="LineValue">If values is set it will show a horizontal line across the chart.</param>
        /// <param name="LineLabel">Text used to describe horizontal line across the chart.</param>
        /// <returns name="Style">Area Chart Style.</returns>
        /// <search>area, chart, style</search>
        public static AreaChartStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,50,130,190)")] DSCore.Color AreaColor,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            [DefaultArgument("Charts.MiscNodes.Margins(20,40,20,40)")] Margins Margins,
            int Width = 1000,
            int Height = 500,
            string YAxisLabel = "Label",
            int TickMarksX = 10,
            double LineValue = 0.0,
            string LineLabel = "")
        {
            AreaChartStyle style = new AreaChartStyle();
            style.Width = Width;
            style.Height = Height;
            style.YAxisLabel = YAxisLabel;
            style.AreaColor = ChartsUtilities.ColorToHexString(sColor.FromArgb(AreaColor.Alpha, AreaColor.Red, AreaColor.Green, AreaColor.Blue));
            style.TickMarksX = TickMarksX;
            style.Margins = Margins;
            style.SizeX = (int)Math.Ceiling(Width / 100d);
            style.SizeY = (int)Math.Ceiling(Height / 100d);
            style.LineValue = LineValue;
            style.LineLabel = LineLabel;

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
            List<DataPoint1> dataPoints = Names.Zip(Values, (x, y) => new DataPoint1 { name = x, value = y }).ToList();
            AreaChartData areaData = new AreaChartData();
            areaData.Data = new JavaScriptSerializer().Serialize(dataPoints);
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

            AreaChartData areaData = new AreaChartData();
            areaData.Data = new JavaScriptSerializer().Serialize(ChartsUtilities.Data1FromCsv(_filePath));
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
        public static D3jsLib.AreaCharts.AreaChart Chart(AreaChartData Data, AreaChartStyle Style)
        {
            D3jsLib.AreaCharts.AreaChart chart = new D3jsLib.AreaCharts.AreaChart(Data, Style);
            return chart;
        }
    }
}
