using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Media;
using System;

namespace D3jsLib.d3AreaCharts
{
    public class AreaChartStyle
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string YAxisLabel { get; set; }
        public Color AreaColor { get; set; }
        public int TickMarksX { get; set; }
    }

    public class AreaChartDataPoint
    {
        public string name { get; set; }
        public double value { get; set; }
    }

    public class AreaChartData
    {
        public List<AreaChartDataPoint> Data { get; set; }
        public Domain Domain { get; set; }
    }

    public class AreaChartModel : ChartModel
    {
        public string YAxisLabel { get; set; }
        public string AreaColor { get; set; }
        public bool Domain { get; set; }
        public string DomainA { get; set; }
        public string DomainB { get; set; }
        public string TickMarksX { get; set; }
        public string Data { get; set; }
    }

    public class d3AreaChart : Chart
    {
        public AreaChartData AreaChartData;
        public AreaChartStyle AreaChartStyle;
        public string UniqueName { get; set; }
        public string ScriptString { get; set; }
        public List<string> ImportsList { get; set; }

        public d3AreaChart(AreaChartData data, AreaChartStyle style)
        {
            this.AreaChartData = data;
            this.AreaChartStyle = style;
        }

        public override void CreateChartModel()
        {
            AreaChartModel model = new AreaChartModel();
            model.DivId = "area" + this.UniqueName;
            model.ColMdValue = this.ColMdValue;
            model.Width = this.AreaChartStyle.Width.ToString();
            model.Height = this.AreaChartStyle.Height.ToString();
            model.YAxisLabel = this.AreaChartStyle.YAxisLabel;
            model.AreaColor = ChartsUtilities.ColorToHexString(this.AreaChartStyle.AreaColor);
            model.TickMarksX = this.AreaChartStyle.TickMarksX.ToString();

            if (this.AreaChartData.Domain == null)
            {
                model.Domain = false;
            }
            else
            {
                model.Domain = true;
                model.DomainA = this.AreaChartData.Domain.A.ToString();
                model.DomainB = this.AreaChartData.Domain.B.ToString();
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(this.AreaChartData.Data);
            model.Data = jsData;

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempArea" + counter.ToString();
            AreaChartModel model = this.ChartModel as AreaChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.AreaCharts.AreaChartScript.html", templateName);
            return colString;
        }

        public override Dictionary<string, int> AssignUniqueName(Dictionary<string, int> nameChecklist)
        {
            string uniqueName;

            // tag chart with unique name
            if (nameChecklist.ContainsKey("areaChart"))
            {
                int lastUsedId = nameChecklist["areaChart"];
                uniqueName = "areaChart" + (lastUsedId + 1).ToString();
                nameChecklist["areaChart"] = lastUsedId + 1;
            }
            else
            {
                uniqueName = "areaChart1";
                nameChecklist["areaChart"] = 1;
            }
            this.UniqueName = uniqueName;

            return nameChecklist;
        }
    }
}

