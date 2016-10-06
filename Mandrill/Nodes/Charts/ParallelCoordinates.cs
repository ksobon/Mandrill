using D3jsLib.ParallelCoordinates;
using System.Collections.Generic;
using sColor = System.Windows.Media.Color;
using System.IO;
using System.Linq;
using System;
using Autodesk.DesignScript.Runtime;
using D3jsLib;

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
        /// <param name="Width">Width of the Chart in pixels.</param>
        /// <param name="Height">Height of the Chart in pixels.</param>
        /// <returns name="Style">Parallel Coordinates Style.</returns>
        /// <search>parallel, coordinates, style</search>
        public static ParallelCoordinatesStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,50,130,190)")] DSCore.Color LineColor,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            int Width = 1000,
            int Height = 500)
        {
            ParallelCoordinatesStyle style = new ParallelCoordinatesStyle();
            style.Width = Width;
            style.Height = Height;
            style.LineColor = sColor.FromArgb(LineColor.Alpha, LineColor.Red, LineColor.Green, LineColor.Blue);

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
            List<ParallelCoordinatesDataPoint> dataPoints = new List<ParallelCoordinatesDataPoint>();
            foreach (List<object> subList in Values)
            {
                ParallelCoordinatesDataPoint dataPoint = new ParallelCoordinatesDataPoint();
                dataPoint.Name = subList[0].ToString();
                Dictionary<string, double> values = new Dictionary<string, double>();
                for (int i = 1; i < subList.Count(); i++)
                {
                    values.Add(Headers[i], Convert.ToDouble(subList[i]));
                }
                dataPoint.Values = values;
                dataPoints.Add(dataPoint);
            }
            ParallelCoordinatesData data = new ParallelCoordinatesData();
            data.Data = dataPoints;

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

            List<ParallelCoordinatesDataPoint> dataPoints = new List<ParallelCoordinatesDataPoint>();
            var csv = new List<string[]>();
            var lines = File.ReadAllLines(_filePath);

            string[] headersArray = lines[0].Split(',');
            for (int i = 1; i < lines.Count(); i++)
            {
                ParallelCoordinatesDataPoint dataPoint = new ParallelCoordinatesDataPoint();
                dataPoint.Name = lines[i].Split(',')[0];

                string[] lineArray = lines[i].Split(',');
                Dictionary<string, double> values = new Dictionary<string, double>();
                for (int j = 1; j < lineArray.Count(); j++)
                {
                    if (lineArray[j] != "")
                    {
                        values.Add(headersArray[j], Convert.ToDouble(lineArray[j]));
                    }
                    else
                    {
                        values.Add(headersArray[j], 0);
                    }
                }

                dataPoint.Values = values;
                dataPoints.Add(dataPoint);
            }

            ParallelCoordinatesData data = new ParallelCoordinatesData();
            data.Data = dataPoints;

            return data;
        }

        /// <summary>
        ///     Parallel Coordinates Chart.
        /// </summary>
        /// <param name="Data">Parallel Coordinates Data object.</param>
        /// <param name="Style">Parallel Coordinates Style object.</param>
        /// <returns name="Chart">Parallel Coordinates Chart.</returns>
        public static D3jsLib.ParallelCoordinates.ParallelCoordinatesChart Chart(ParallelCoordinatesData Data, ParallelCoordinatesStyle Style)
        {
            D3jsLib.ParallelCoordinates.ParallelCoordinatesChart chart = new D3jsLib.ParallelCoordinates.ParallelCoordinatesChart(Data, Style);
            return chart;
        }
    }
}