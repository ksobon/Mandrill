using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Script.Serialization;

namespace D3jsLib.ParallelCoordinates
{
    public class ParallelCoordinatesStyle : ChartStyle
    {
        public Color LineColor { get; set; }
    }

    public class ParallelCoordinatesData
    {
        public List<DataPoint2> Data { get; set; }

        public string ToJsonString()
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataPoint2 dp in this.Data)
            {
                list.Add(dp.ToDictionary());
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(list);

            return jsData;
        }
    }

    public class ParallelCoordinatesModel : ChartModel
    {
        public string Data { get; set; }
        public string LineColor { get; set; }
    }

    public class ParallelCoordinatesChart : Chart
    {
        public ParallelCoordinatesData Data;
        public ParallelCoordinatesStyle Style;

        public ParallelCoordinatesChart(ParallelCoordinatesData data, ParallelCoordinatesStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override void CreateChartModel(int counter)
        {
            ParallelCoordinatesModel model = new ParallelCoordinatesModel();
            model.Width = this.Style.Width;
            model.Height = this.Style.Height;
            model.LineColor = ChartsUtilities.ColorToHexString(this.Style.LineColor);
            model.Data = this.Data.ToJsonString();
            model.DivId = "div" + counter.ToString();
            model.Margins = this.Style.Margins;

            // set grid address
            model.GridRow = this.Style.GridRow;
            model.GridColumn = this.Style.GridColumn;

            // always round up for the grid size so chart is smaller then container
            model.SizeX = (int)System.Math.Ceiling(this.Style.Width / 100d);
            model.SizeY = (int)System.Math.Ceiling(this.Style.Height / 100d);

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempParallelCoordinates" + counter.ToString();
            ParallelCoordinatesModel model = this.ChartModel as ParallelCoordinatesModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.ParallelCoordinatesChart.parallelCoordinatesChart.html", templateName);
            return colString;
        }
    }
}