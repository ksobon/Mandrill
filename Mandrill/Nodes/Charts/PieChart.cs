using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using D3jsLib.PieChart;
using sColor = System.Drawing.Color;
using D3jsLib;
using D3jsLib.Utilities;
using System.IO;
using System.Web.Script.Serialization;

namespace Charts
{
    /// <summary>
    ///     Donut Chart
    /// </summary>
    public class PieChart
    {
        internal PieChart()
        {
        }

        /// <summary>
        ///     Pie Chart Style
        /// </summary>
        /// <param name="HoverColor">Hover over color.</param>
        /// <param name="Colors">List of optional colors for chart values.</param>
        /// <param name="Address">Grid Coordinates.</param>
        /// <param name="Margins">Margins in pixels.</param>
        /// <param name="Width">Width of chart in pixels.</param>
        /// <param name="Labels">Boolean value that controls if Labels are displayed.</param>
        /// <returns name="Style">Pie Chart Style object.</returns>
        public static PieChartStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,255,0,0)")] DSCore.Color HoverColor,
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] List<DSCore.Color> Colors,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            [DefaultArgument("Charts.MiscNodes.Margins()")] Margins Margins,
            int Width = 400,
            bool Labels = true)
        {
            PieChartStyle style = new PieChartStyle();
            style.Width = Width;
            style.HoverColor = ChartsUtilities.ColorToHexString(sColor.FromArgb(HoverColor.Alpha, HoverColor.Red, HoverColor.Green, HoverColor.Blue));
            style.Labels = Labels;
            style.Margins = Margins;
            style.SizeX = (int)System.Math.Ceiling(Width / 100d);
            style.SizeY = (int)System.Math.Ceiling(Width / 100d);

            if (Colors != null)
            {
                List<string> hexColors = Colors.Select(x => ChartsUtilities.ColorToHexString(sColor.FromArgb(x.Alpha, x.Red, x.Green, x.Blue))).ToList();
                style.Colors = new JavaScriptSerializer().Serialize(hexColors);
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
        ///     Pie Chart Data.
        /// </summary>
        /// <param name="Names">Names of each data points.</param>
        /// <param name="Values">Values of each data point.</param>
        /// <returns name="Data">Pie Chart data object.</returns>
        public static PieChartData Data(
            List<string> Names,
            List<double> Values)
        {
            List<DataPoint1> dataPoints = Names.Zip(Values, (x, y) => new DataPoint1 { name = x, value = y }).ToList();
            PieChartData data = new PieChartData();
            data.Data = new JavaScriptSerializer().Serialize(dataPoints);

            return data;
        }

        /// <summary>
        ///     Pie Chart Data
        /// </summary>
        /// <param name="FilePath">File Path to CSV file.</param>
        /// <returns name="Data">Pie Chart Data object.</returns>
        public static PieChartData DataFromCSV(object FilePath)
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

            PieChartData data = new PieChartData();
            data.Data = new JavaScriptSerializer().Serialize(ChartsUtilities.Data1FromCSV(_filePath));

            return data;
        }

        /// <summary>
        ///     Pie chart
        /// </summary>
        /// <param name="Data">Pie Chart data object.</param>
        /// <param name="Style">Pie Chart Style object.</param>
        /// <returns name="Chart">Pie Chart object.</returns>
        public static D3jsLib.PieChart.PieChart Chart(PieChartData Data, PieChartStyle Style)
        {
            D3jsLib.PieChart.PieChart chart = new D3jsLib.PieChart.PieChart(Data, Style);
            return chart;
        }
    }
}