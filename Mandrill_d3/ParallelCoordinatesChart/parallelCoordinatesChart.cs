using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Media;

namespace D3jsLib.ParallelCoordinates
{
    public class ParallelCoordinatesStyle : ChartStyle
    {
        public Color LineColor { get; set; }
    }

    public class ParallelCoordinatesDataPoint
    {
        public string Name { get; set; }
        public Dictionary<string, double> Values { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> output = new Dictionary<string, object>();
            output.Add("Name", this.Name);
            foreach (var value in this.Values)
            {
                output.Add(value.Key, value.Value);
            }

            return output;
        }
    }

    public class ParallelCoordinatesData
    {
        public List<ParallelCoordinatesDataPoint> Data { get; set; }

        public string ToJsonString()
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (ParallelCoordinatesDataPoint dp in this.Data)
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
        public string UniqueName { get; set; }

        public ParallelCoordinatesChart(ParallelCoordinatesData data, ParallelCoordinatesStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override void CreateChartModel(int counter)
        {
            ParallelCoordinatesModel model = new ParallelCoordinatesModel();
            model.Width = this.Style.Width.ToString();
            model.Height = this.Style.Height.ToString();
            model.LineColor = ChartsUtilities.ColorToHexString(this.Style.LineColor);
            model.Data = this.Data.ToJsonString();
            model.DivId = "div" + counter.ToString();

            // set grid address
            model.GridRow = this.Style.GridRow.ToString();
            model.GridColumn = this.Style.GridColumn.ToString();

            // always round up for the grid size so chart is smaller then container
            model.SizeX = System.Math.Ceiling(this.Style.Width / 100d).ToString();
            model.SizeY = System.Math.Ceiling(this.Style.Height / 100d).ToString();

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempParallelCoordinates" + counter.ToString();
            ParallelCoordinatesModel model = this.ChartModel as ParallelCoordinatesModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.ParallelCoordinatesChart.parallelCoordinatesChart.html", templateName);
            return colString;
        }

        public override string EvaluateDivTemplate(int counter)
        {
            string templateName = "divTempParallelCoordinates" + counter.ToString();
            ParallelCoordinatesModel model = this.ChartModel as ParallelCoordinatesModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.Gridster.divTemplate.html", templateName);
            return colString;
        }
    }
}