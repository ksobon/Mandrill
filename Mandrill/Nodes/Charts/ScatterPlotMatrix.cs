using D3jsLib.ScatterPlotMatrix;
using System.Collections.Generic;
using sColor = System.Drawing.Color;
using D3jsLib.Utilities;
using Autodesk.DesignScript.Runtime;
using D3jsLib;
using System.IO;
using System.Linq;
using System;

namespace Charts
{
    /// <summary>
    ///     Scatter Plot Matrix Nodes.
    /// </summary>
    public class ScatterPlotMatrix
    {
        internal ScatterPlotMatrix()
        {
        }

        /// <summary>
        ///     Style
        /// </summary>
        /// <param name="Colors">List of Colors.</param>
        /// <param name="Address">Grid Coordinates.</param>
        /// <param name="Width">Width in pixels.</param>
        /// <returns name="Style">Style</returns>
        public static ScatterPlotMatrixStyle Style(
            [DefaultArgumentAttribute("Charts.MiscNodes.GetNull()")] List<DSCore.Color> Colors,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            int Width = 1000
            )
        {
            ScatterPlotMatrixStyle style = new ScatterPlotMatrixStyle();
            style.Width = Width;

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
        ///     Data.
        /// </summary>
        /// <param name="Headers">Header values.</param>
        /// <param name="Values">Data values.</param>
        /// <returns name="Data">Data</returns>
        public static ScatterPlotMatrixData Data(
            List<string> Headers,
            List<List<object>> Values)
        {
            List<DataPoint2> dataPoints = new List<DataPoint2>();
            foreach (List<object> subList in Values)
            {
                DataPoint2 dataPoint = new DataPoint2();
                dataPoint.Name = subList[0].ToString();
                Dictionary<string, double> values = new Dictionary<string, double>();
                for (int i = 1; i < subList.Count(); i++)
                {
                    values.Add(Headers[i], Convert.ToDouble(subList[i]));
                }
                dataPoint.Values = values;
                dataPoints.Add(dataPoint);
            }
            ScatterPlotMatrixData data = new ScatterPlotMatrixData();
            data.Data = dataPoints;

            return data;
        }


        /// <summary>
        ///     Data from CSV
        /// </summary>
        /// <param name="FilePath">File path to CSV file.</param>
        /// <returns name="Data">Data</returns>
        public static ScatterPlotMatrixData DataFromCSV(
            object FilePath)
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

            ScatterPlotMatrixData data = new ScatterPlotMatrixData();
            data.Data = ChartsUtilities.Data2FromCSV(_filePath);

            return data;
        }

        /// <summary>
        ///     Scatter Plot Matrix Chart
        /// </summary>
        /// <param name="Data">Data</param>
        /// <param name="Style">Style</param>
        /// <returns name="Chart">Chart.</returns>
        public static D3jsLib.ScatterPlotMatrix.ScatterPlotMatrix Chart(ScatterPlotMatrixData Data, ScatterPlotMatrixStyle Style)
        {
            D3jsLib.ScatterPlotMatrix.ScatterPlotMatrix chart = new D3jsLib.ScatterPlotMatrix.ScatterPlotMatrix(Data, Style);
            return chart;
        }
    }
}
