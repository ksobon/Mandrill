using System.Collections.Generic;
using System.Linq;
using D3jsLib.HorizontalBarChart;
using Autodesk.DesignScript.Runtime;
using D3jsLib;
using sColor = System.Drawing.Color;
using System.IO;

namespace Charts
{
    /// <summary>
    ///     Horizontal Bar Chart.
    /// </summary>
    public class HorizontalBarChart
    {
        internal HorizontalBarChart()
        {
        }

        /// <summary>
        ///     Horizontal Bar Chart Style
        /// </summary>
        /// <param name="BarColor">Fill color for bars.</param>
        /// <param name="BarHoverColor">Fill color when hovered over.</param>
        /// <param name="Address">Grid Coordinates.</param>
        /// <param name="Margins">Margins in pixels.</param>
        /// <param name="Width">Width of the entire chart in pixels.</param>
        /// <param name="Height">Height of the entire chart in pixels.</param>
        /// <param name="XAxisLabel">Text displayed in bottom-right corner of chart.</param>
        /// <returns name="Style">Style</returns>
        public static HorizontalBarChartStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,50,130,190)")] DSCore.Color BarColor,
            [DefaultArgument("DSCore.Color.ByARGB(1,255,0,0)")] DSCore.Color BarHoverColor,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            [DefaultArgument("Charts.MiscNodes.Margins(20,40,20,40)")] Margins Margins,
            int Width = 1000,
            int Height = 500,
            string XAxisLabel = "Label")
        {
            HorizontalBarChartStyle style = new HorizontalBarChartStyle();
            style.BarColor = sColor.FromArgb(BarColor.Alpha, BarColor.Red, BarColor.Green, BarColor.Blue);
            style.BarHoverColor = sColor.FromArgb(BarHoverColor.Alpha, BarHoverColor.Red, BarHoverColor.Green, BarHoverColor.Blue);
            style.Width = Width;
            style.Height = Height;
            style.YAxisLabel = XAxisLabel;
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
        ///     Horizontal Bar Chart Data.
        /// </summary>
        /// <param name="Names"></param>
        /// <param name="Values"></param>
        /// <param name="Domain"></param>
        /// <returns name="Data">Data</returns>
        public static HorizontalBarChartData Data(
            List<string> Names,
            List<double> Values,
            [DefaultArgument("Charts.MiscNodes.GetNull()")]Domain Domain)
        {
            List<DataPoint1> dataPoints = Names.Zip(Values, (x, y) => new DataPoint1 { name = x, value = y }).ToList();
            HorizontalBarChartData barData = new HorizontalBarChartData();
            barData.Data = dataPoints;
            barData.Domain = Domain;
            return barData;
        }

        /// <summary>
        ///     Horizontal Bar Chart Data.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="Domain"></param>
        /// <returns name="Data">Data</returns>
        public static HorizontalBarChartData DataFromCSV(
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

            HorizontalBarChartData barData = new HorizontalBarChartData();
            barData.Data = D3jsLib.Utilities.ChartsUtilities.Data1FromCSV(_filePath);
            barData.Domain = Domain;
            return barData;
        }

        /// <summary>
        ///     Horizontal Bar Chart
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Style"></param>
        /// <returns name="Chart">Chart</returns>
        public static D3jsLib.HorizontalBarChart.HorizontalBarChart Chart(HorizontalBarChartData Data, HorizontalBarChartStyle Style)
        {
            D3jsLib.HorizontalBarChart.HorizontalBarChart chart = new D3jsLib.HorizontalBarChart.HorizontalBarChart(Data, Style);
            return chart;
        }
    }
}
