using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Media;

namespace D3jsLib.d3LineCharts
{
    public class LineChartStyle
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string YAxisLabel { get; set; }
        public Color LineColor { get; set; }
        public int TickMarksX { get; set; }
    }

    public class LineChartDataPoint
    {
        public string name { get; set; }
        public double value { get; set; }
    }

    public class LineChartData
    {
        public List<LineChartDataPoint> Data { get; set; }
        public Domain Domain { get; set; }
    }

    public class LineChartModel : ChartModel
    {
        public string Data { get; set; }
        public string YAxisLabel { get; set; }
        public string LineColor { get; set; }
        public string TickMarksX { get; set; }
        public bool Domain { get; set; }
        public string DomainA { get; set; }
        public string DomainB { get; set; }

    }

    public class d3LineChart : Chart
    {
        public LineChartData LineChartData;
        public LineChartStyle LineChartStyle;
        public string UniqueName { get; set; }
        public string ScriptString { get; set; }
        public List<string> ImportsList { get; set; }

        public d3LineChart(LineChartData data, LineChartStyle style)
        {
            this.LineChartData = data;
            this.LineChartStyle = style;
        }

        public override void CreateChartModel()
        {
            LineChartModel model = new LineChartModel();
            model.DivId = "line" + this.UniqueName;
            model.ColMdValue = this.ColMdValue;
            model.Width = this.LineChartStyle.Width.ToString();
            model.Height = this.LineChartStyle.Height.ToString();
            model.YAxisLabel = this.LineChartStyle.YAxisLabel;
            model.LineColor = ChartsUtilities.ColorToHexString(this.LineChartStyle.LineColor);
            model.TickMarksX = this.LineChartStyle.TickMarksX.ToString();

            if (this.LineChartData.Domain == null)
            {
                model.Domain = false;
            }
            else
            {
                model.Domain = true;
                model.DomainA = this.LineChartData.Domain.A.ToString();
                model.DomainB = this.LineChartData.Domain.B.ToString();
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(this.LineChartData.Data);
            model.Data = jsData;

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempLine" + counter.ToString();
            LineChartModel model = this.ChartModel as LineChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.LineCharts.LineChartScript.html", templateName);
            return colString;
        }

        public override Dictionary<string, int> AssignUniqueName(Dictionary<string, int> nameChecklist)
        {
            string uniqueName;

            // tag it with UniqueName
            if (nameChecklist.ContainsKey("linechart"))
            {
                int lastUsedId = nameChecklist["linechart"];
                uniqueName = "linechart" + (lastUsedId + 1).ToString();
                nameChecklist["linechart"] = lastUsedId + 1;
            }
            else
            {
                uniqueName = "linechart1";
                nameChecklist["linechart"] = 1;
            }
            this.UniqueName = uniqueName;

            return nameChecklist;
        }
    }
}
