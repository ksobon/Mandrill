using System.Collections.Generic;
using System.Linq;
using D3jsLib.HorizontalBarChart;
using Autodesk.DesignScript.Runtime;
using D3jsLib;
using sColor = System.Drawing.Color;
using System.IO;
using D3jsLib.Utilities;
using System.Web.Script.Serialization;

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
            [DefaultArgument("Charts.MiscNodes.GetColorList()")] List<DSCore.Color> BarColor,
            [DefaultArgument("DSCore.Color.ByARGB(1,255,0,0)")] DSCore.Color BarHoverColor,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            [DefaultArgument("Charts.MiscNodes.Margins(20,40,20,40)")] Margins Margins,
            int Width = 1000,
            int Height = 500,
            string XAxisLabel = "Label")
        {
            HorizontalBarChartStyle style = new HorizontalBarChartStyle();

            List<string> hexColors = BarColor.Select(x => ChartsUtilities.ColorToHexString(sColor.FromArgb(x.Alpha, x.Red, x.Green, x.Blue))).ToList();
            style.BarColor = new JavaScriptSerializer().Serialize(hexColors);

            style.BarHoverColor = ChartsUtilities.ColorToHexString(sColor.FromArgb(BarHoverColor.Alpha, BarHoverColor.Red, BarHoverColor.Green, BarHoverColor.Blue));
            style.Width = Width;
            style.Height = Height;
            style.YAxisLabel = XAxisLabel;
            style.Margins = Margins;
            style.SizeX = (int)System.Math.Ceiling(Width / 100d);
            style.SizeY = (int)System.Math.Ceiling(Height / 100d);

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
        /// <param name="Colors"></param>
        /// <param name="Domain"></param>
        /// <returns name="Data">Data</returns>
        public static HorizontalBarChartData Data(
            List<string> Names,
            List<double> Values,
            [DefaultArgument("Charts.MiscNodes.GetNull()")]List<int> Colors,
            [DefaultArgument("Charts.MiscNodes.GetNull()")]Domain Domain)
        {
            List<DataPoint1> dataPoints = new List<DataPoint1>();
            if (Colors != null)
            {
                dataPoints = Names.ZipThree(Values, Colors, (x, y, z) => new DataPoint1 { name = x, value = y, color = z }).ToList();
            }
            else
            {
                dataPoints = Names.Zip(Values, (x, y) => new DataPoint1 { name = x, value = y}).ToList();

            }
            HorizontalBarChartData barData = new HorizontalBarChartData();
            barData.Data = new JavaScriptSerializer().Serialize(dataPoints);
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
            barData.Data = new JavaScriptSerializer().Serialize(ChartsUtilities.Data1FromCSV(_filePath));
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
