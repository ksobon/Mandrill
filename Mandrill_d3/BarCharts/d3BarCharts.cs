using System.Collections.Generic;
using System.Windows.Media;
using D3jsLib.Utilities;
using System.Web.Script.Serialization;

namespace D3jsLib.d3BarCharts
{
    public class DivContent
    {
        public object SourceObject { get; set; }
        public int RowNumber { get; set; }

        public DivContent(Chart chart)
        {
            this.RowNumber = chart.RowNumber;
            this.SourceObject = chart;
        }
    }

    public class BarChartModel : ChartModel
    {
        public string BarFill { get; set; }
        public string BarHover { get; set; }
        public string YAxisLabel { get; set; }
        public bool Domain { get; set; }
        public string DomainA { get; set; }
        public string DomainB { get; set; }
        public string TickMarksX { get; set; }
        public string Data { get; set; }
    }

    public class BarDataPoint
    {
        public string name { get; set; }
        public double value { get; set; }
    }

    public class BarData
    {
        public List<BarDataPoint> Data { get; set; }
        public Domain Domain { get; set; }
    }

    public class BarStyle
    {
        public Color BarColor { get; set; }
        public Color BarHoverColor { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string YAxisLabel { get; set; }
        public int TickMarksX { get; set; }
    }

    public class d3BarChart : Chart
    {
        public string UniqueName { get; set; }
        public string CssStyleString { get; set; }
        public string ScriptString { get; set; }
        public List<string> ImportsList { get; set; }

        public BarData BarChartData;
        public BarStyle BarChartStyle;

        public d3BarChart(BarData data, BarStyle style)
        {
            this.BarChartData = data;
            this.BarChartStyle = style;
        }

        public override void CreateChartModel()
        {
            BarChartModel model = new BarChartModel();
            model.DivId = "bar" + this.UniqueName;
            model.ColMdValue = this.ColMdValue;
            model.Width = this.BarChartStyle.Width.ToString();
            model.Height = this.BarChartStyle.Height.ToString();
            model.YAxisLabel = this.BarChartStyle.YAxisLabel;
            model.DivName = this.UniqueName;
            model.TickMarksX = this.BarChartStyle.TickMarksX.ToString();
            model.BarFill = ChartsUtilities.ColorToHexString(this.BarChartStyle.BarColor);
            model.BarHover = ChartsUtilities.ColorToHexString(this.BarChartStyle.BarHoverColor);

            if (this.BarChartData.Domain == null)
            {
                model.Domain = false;
            }
            else
            {
                model.Domain = true;
                model.DomainA = this.BarChartData.Domain.A.ToString();
                model.DomainB = this.BarChartData.Domain.B.ToString();
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(this.BarChartData.Data);
            model.Data = jsData;

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempBar" + counter.ToString();
            BarChartModel model = this.ChartModel as BarChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.BarCharts.BarChartScript.html", templateName);
            return colString;
        }

        public override Dictionary<string, int> AssignUniqueName(Dictionary<string, int> nameChecklist)
        {
            string uniqueName;
            // tag it with UniqueName
            if (nameChecklist.ContainsKey("barchart"))
            {
                int lastUsedId = nameChecklist["barchart"];
                uniqueName = "barchart" + (lastUsedId + 1).ToString();
                nameChecklist["barchart"] = lastUsedId + 1;
            }
            else
            {
                uniqueName = "barchart1";
                nameChecklist["barchart"] = 1;
            }
            this.UniqueName = uniqueName;

            return nameChecklist;
        }
    }
}
