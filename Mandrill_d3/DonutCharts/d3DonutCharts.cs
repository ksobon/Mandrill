using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Media;

namespace D3jsLib.DonutChart
{
    public class DonutChartStyle
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Color HoverColor { get; set; }
        public List<string> Colors { get; set; }
        public bool Labels { get; set; }
        public bool Legend { get; set; }
    }

    public class DonutChartDataPoint
    {
        public string name { get; set; }
        public double val { get; set; }
    }

    public class DonutChartData
    {
        public List<DonutChartDataPoint> Data { get; set; }
    }

    public class DonutChartModel : ChartModel
    {
        public string HoverColor { get; set; }
        public string Data { get; set; }
        public bool Colors { get; set; }
        public string DomainColors { get; set; }
        public bool Labels { get; set; }
        public bool Legend { get; set; }
    }

    public class DonutChart : Chart
    {
        public DonutChartData DonutChartData;
        public DonutChartStyle DonutChartStyle;
        public string UniqueName { get; set; }

        public DonutChart(DonutChartData data, DonutChartStyle style)
        {
            this.DonutChartData = data;
            this.DonutChartStyle = style;
        }

        public override void CreateChartModel()
        {
            DonutChartModel model = new DonutChartModel();
            model.DivId = "donut" + this.UniqueName;
            model.ColMdValue = this.ColMdValue;
            model.Width = this.DonutChartStyle.Width.ToString();
            model.Height = this.DonutChartStyle.Height.ToString();
            model.HoverColor = ChartsUtilities.ColorToHexString(this.DonutChartStyle.HoverColor);
            model.Labels = this.DonutChartStyle.Labels;
            model.Legend = this.DonutChartStyle.Legend;

            if (this.DonutChartStyle.Colors != null)
            {
                string domainColors = new JavaScriptSerializer().Serialize(this.DonutChartStyle.Colors);
                model.DomainColors = domainColors;
                model.Colors = true;
            }
            else
            {
                model.Colors = false;
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(this.DonutChartData.Data);
            model.Data = jsData;

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempDonut" + counter.ToString();
            DonutChartModel model = this.ChartModel as DonutChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.DonutCharts.DonutChartScript.html", templateName);
            return colString;
        }

        public override Dictionary<string, int> AssignUniqueName(Dictionary<string, int> nameChecklist)
        {
            string uniqueName;

            // tag chart with unique name
            if (nameChecklist.ContainsKey("donutChart"))
            {
                int lastUsedId = nameChecklist["donutChart"];
                uniqueName = "donutChart" + (lastUsedId + 1).ToString();
                nameChecklist["donutChart"] = lastUsedId + 1;
            }
            else
            {
                uniqueName = "donutChart";
                nameChecklist["donutChart"] = 1;
            }
            this.UniqueName = uniqueName;

            return nameChecklist;
        }
    }
}