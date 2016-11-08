using D3jsLib.Legend;
using System.Collections.Generic;
using sColor = System.Drawing.Color;
using D3jsLib.Utilities;
using Autodesk.DesignScript.Runtime;
using D3jsLib;

namespace Legends
{
    /// <summary>
    ///     Grouped Bar Chart nodes.
    /// </summary>
    public class Legend
    {
        internal Legend()
        {
        }

        /// <summary>
        ///     Legend Style
        /// </summary>
        /// <param name="Colors">List of colors for each rectangle.</param>
        /// <param name="Address">Grid Coordinates.</param>
        /// <param name="Width">Width in pixels.</param>
        /// <param name="Height">Height in pixels.</param>
        /// <param name="Title">Title below legend.</param>
        /// <param name="RectangleSize">Rectangle size in pixels.</param>
        /// <returns name="Style">Style</returns>
        public static LegendStyle Style(
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] List<DSCore.Color> Colors,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            int Width = 200,
            int Height = 400,
            string Title = "Title",
            int RectangleSize = 20
            )
        {
            LegendStyle style = new LegendStyle();
            style.Width = Width;
            style.Height = Height;
            style.Title = Title;
            style.RectangleSize = RectangleSize;

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
        ///     Data
        /// </summary>
        /// <param name="Names">List of strings.</param>
        /// <returns name="Data">Data</returns>
        public static LegendData Data(
            List<string> Names)
        {
            LegendData data = new LegendData();
            data.Data = Names;

            return data;
        }

        /// <summary>
        ///     Legend
        /// </summary>
        /// <param name="Data">Legend Data</param>
        /// <param name="Style">Legend Style</param>
        /// <returns name="legend">Legend object.</returns>
        public static D3jsLib.Legend.Legend Create(LegendData Data, LegendStyle Style)
        {
            D3jsLib.Legend.Legend chart = new D3jsLib.Legend.Legend(Data, Style);
            return chart;
        }
    }
}

