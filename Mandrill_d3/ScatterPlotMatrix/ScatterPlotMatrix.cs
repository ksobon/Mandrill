using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace D3jsLib.ScatterPlotMatrix
{
    public class ScatterPlotMatrixStyle : ChartStyle
    {
        public List<string> Colors { get; set; }
    }

    public class ScatterPlotMatrixDataPoint
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

    public class ScatterPlotMatrixData
    {
        public List<ScatterPlotMatrixDataPoint> Data { get; set; }

        public string ToJsonString()
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (ScatterPlotMatrixDataPoint dp in this.Data)
            {
                list.Add(dp.ToDictionary());
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(list);

            return jsData;
        }
    }

    public class ScatterPlotMatrixModel : ChartModel
    {
        public string Data { get; set; }
        public bool Colors { get; set; }
        public string DomainColors { get; set; }
        public string FrameSize { get; set; }
    }

    public class ScatterPlotMatrix : Chart
    {
        public ScatterPlotMatrixData Data;
        public ScatterPlotMatrixStyle Style;

        public ScatterPlotMatrix(ScatterPlotMatrixData data, ScatterPlotMatrixStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override void CreateChartModel(int counter)
        {
            ScatterPlotMatrixModel model = new ScatterPlotMatrixModel();
            model.Width = this.Style.Width.ToString();
            model.Height = this.Style.Width.ToString();
            model.Data = this.Data.ToJsonString();
            model.DivId = "div" + counter.ToString();

            // Frame width is equal to chart width / number of frames - 10 for padding
            int count = this.Data.Data[0].Values.Count;
            model.FrameSize = ((this.Style.Width / count) - 10).ToString();

            if (this.Style.Colors != null)
            {
                string domainColors = new JavaScriptSerializer().Serialize(this.Style.Colors);
                model.DomainColors = domainColors;
                model.Colors = true;
            }
            else
            {
                model.Colors = false;
            }

            // set grid address
            model.GridRow = this.Style.GridRow.ToString();
            model.GridColumn = this.Style.GridColumn.ToString();

            // always round up for the grid size so chart is smaller then container
            model.SizeX = System.Math.Ceiling(this.Style.Width / 100d).ToString();
            model.SizeY = System.Math.Ceiling(this.Style.Width / 100d).ToString();

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempScatterPlotMatrix" + counter.ToString();
            ScatterPlotMatrixModel model = this.ChartModel as ScatterPlotMatrixModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.ScatterPlotMatrix.ScatterPlotMatrix.html", templateName);
            return colString;
        }
    }
}