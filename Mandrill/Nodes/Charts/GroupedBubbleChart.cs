using D3jsLib.GroupedBubbleChart;
using System.Collections.Generic;
using sColor = System.Drawing.Color;
using D3jsLib.Utilities;
using Autodesk.DesignScript.Runtime;
using D3jsLib;
using System.IO;
using System.Linq;
using System;
using System.Web.Script.Serialization;

namespace Charts
{
    /// <summary>
    ///     Grouped Bubble Chart nodes.
    /// </summary>
    public class GroupedBubbleChart
    {
        internal GroupedBubbleChart()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Colors"></param>
        /// <param name="Address"></param>
        /// <param name="Margins"></param>
        /// <param name="Width"></param>
        /// <returns></returns>
        public static GroupedBubbleChartStyle Style(
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] List<DSCore.Color> Colors,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            [DefaultArgument("Charts.MiscNodes.Margins()")] Margins Margins,
            int Width = 1000
            )
        {
            GroupedBubbleChartStyle style = new GroupedBubbleChartStyle();
            style.Width = Width;
            style.Margins = Margins;
            style.SizeX = (int)Math.Ceiling(Width / 100d);
            style.SizeY = (int)Math.Ceiling(Width / 100d);

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
        /// 
        /// </summary>
        /// <param name="Names"></param>
        /// <param name="Values"></param>
        /// <param name="Colors"></param>
        /// <returns></returns>
        public static GroupedBubbleChartData Data(
            List<string> Names,
            List<double> Values,
            List<int> Colors)
        {
            GroupedBubbleChartData data = new GroupedBubbleChartData();
            List<DataPoint1> dataPoints = Names.ZipThree(Values, Colors, (x, y, z) => new DataPoint1 { name = x, value = y, color = z }).ToList();
            data.Data = new JavaScriptSerializer().Serialize(dataPoints);

            return data;
        }

        private static void Data3FromList(List<string> names, List<double> values, List<int> colors)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Style"></param>
        /// <returns></returns>
        public static D3jsLib.GroupedBubbleChart.GroupedBubbleChart Chart(GroupedBubbleChartData Data, GroupedBubbleChartStyle Style)
        {
            D3jsLib.GroupedBubbleChart.GroupedBubbleChart chart = new D3jsLib.GroupedBubbleChart.GroupedBubbleChart(Data, Style);
            return chart;
        }
    }
}