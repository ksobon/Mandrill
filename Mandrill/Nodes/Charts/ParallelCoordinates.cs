using D3jsLib.ParallelCoordinates;
using System.Collections.Generic;
using sColor = System.Drawing.Color;
using System.IO;
using System;
using Autodesk.DesignScript.Runtime;
using D3jsLib;
using D3jsLib.Utilities;

namespace Charts
{
    /// <summary>
    ///     Grouped Bar Chart nodes.
    /// </summary>
    public class ParallelCoordinates
    {
        internal ParallelCoordinates()
        {
        }

        /// <summary>
        ///     Parallel Coordinates Style object.
        /// </summary>
        /// <param name="LineColor">Color of the selected Lines/Values.</param>
        /// <param name="Address">Grid Coordinates.</param>
        /// <param name="Margins">Margins in pixels.</param>
        /// <param name="Width">Width of the Chart in pixels.</param>
        /// <param name="Height">Height of the Chart in pixels.</param>
        /// <returns name="Style">Parallel Coordinates Style.</returns>
        /// <search>parallel, coordinates, style</search>
        public static ParallelCoordinatesStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,50,130,190)")] DSCore.Color LineColor,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            [DefaultArgument("Charts.MiscNodes.Margins()")] Margins Margins,
            int Width = 1000,
            int Height = 500)
        {
            ParallelCoordinatesStyle style = new ParallelCoordinatesStyle();
            style.Width = Width;
            style.Height = Height;
            style.LineColor = ChartsUtilities.ColorToHexString(sColor.FromArgb(LineColor.Alpha, LineColor.Red, LineColor.Green, LineColor.Blue));
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
        ///     Parallel Coordinates Data.
        /// </summary>
        /// <param name="Headers">Names of each Axis.</param>
        /// <param name="Values">Values for each data point.</param>
        /// <returns name="Data">Parallel Coordinates Data.</returns>
        /// <search>parallel, coordinates, data</search>
        public static ParallelCoordinatesData Data(
            List<string> Headers,
            List<List<object>> Values)
        {
            ParallelCoordinatesData data = new ParallelCoordinatesData();
            data.Data = ChartsUtilities.DataToJsonString(ChartsUtilities.Data2FromList(Headers, Values));

            return data;
        }

        /// <summary>
        ///     Parallel Coordinates Data.
        /// </summary>
        /// <param name="FilePath">File Path for CSV file.</param>
        /// <returns name="Data">Parallel Coordinates Data.</returns>
        /// <search>parallel, coordinates, data, csv</search>
        public static ParallelCoordinatesData DataFromCSV(
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

            ParallelCoordinatesData data = new ParallelCoordinatesData();
            data.Data = ChartsUtilities.DataToJsonString(ChartsUtilities.Data2FromCsv(_filePath));

            return data;
        }

        /// <summary>
        ///     Parallel Coordinates Chart.
        /// </summary>
        /// <param name="Data">Parallel Coordinates Data object.</param>
        /// <param name="Style">Parallel Coordinates Style object.</param>
        /// <returns name="Chart">Parallel Coordinates Chart.</returns>
        public static ParallelCoordinatesChart Chart(ParallelCoordinatesData Data, ParallelCoordinatesStyle Style)
        {
            ParallelCoordinatesChart chart = new ParallelCoordinatesChart(Data, Style);
            return chart;
        }
    }
}