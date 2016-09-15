using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Media;

namespace D3jsLib.StackedBarChart
{
    public class StackedBarChartStyle
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<string> Colors { get; set; }
        public string YAxisLabel { get; set; }
        public Color BarHoverColor { get; set; }
    }

    public class StackedBarChartDataPoint
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

    public class StackedBarChartData
    {
        public List<StackedBarChartDataPoint> Data { get; set; }
        public Domain Domain { get; set; }

        public string ToJsonString()
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (StackedBarChartDataPoint dp in this.Data)
            {
                list.Add(dp.ToDictionary());
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(list);

            return jsData;
        }
    }

    public class StackedBarChartModel : ChartModel
    {
        public string Data { get; set; }
        public string YAxisLabel { get; set; }
        public bool Colors { get; set; }
        public string DomainColors { get; set; }
        public bool Domain { get; set; }
        public string DomainA { get; set; }
        public string DomainB { get; set; }
        public string BarHover { get; set; }
    }

    public class StackedBarChart : Chart
    {
        public StackedBarChartData StackedBarChartData;
        public StackedBarChartStyle StackedBarChartStyle;
        public string UniqueName { get; set; }
        public string ScriptString { get; set; }
        public List<string> ImportsList { get; set; }

        public StackedBarChart(StackedBarChartData data, StackedBarChartStyle style)
        {
            this.StackedBarChartData = data;
            this.StackedBarChartStyle = style;
        }

        public override void CreateChartModel()
        {
            StackedBarChartModel model = new StackedBarChartModel();
            model.DivId = "stackedBar" + this.UniqueName;
            model.ColMdValue = this.ColMdValue;
            model.Width = this.StackedBarChartStyle.Width.ToString();
            model.Height = this.StackedBarChartStyle.Height.ToString();
            model.YAxisLabel = this.StackedBarChartStyle.YAxisLabel;
            model.BarHover = ChartsUtilities.ColorToHexString(this.StackedBarChartStyle.BarHoverColor);

            if (this.StackedBarChartStyle.Colors != null)
            {
                string domainColors = new JavaScriptSerializer().Serialize(this.StackedBarChartStyle.Colors);
                model.DomainColors = domainColors;
                model.Colors = true;
            }
            else
            {
                model.Colors = false;
            }


            if (this.StackedBarChartData.Domain == null)
            {
                model.Domain = false;
            }
            else
            {
                model.Domain = true;
                model.DomainA = this.StackedBarChartData.Domain.A.ToString();
                model.DomainB = this.StackedBarChartData.Domain.B.ToString();
            }

            model.Data = this.StackedBarChartData.ToJsonString();

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempStackedBar" + counter.ToString();
            StackedBarChartModel model = this.ChartModel as StackedBarChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.StackedBarChart.d3StackedBarChart.html", templateName);
            return colString;
        }

        public override Dictionary<string, int> AssignUniqueName(Dictionary<string, int> nameChecklist)
        {
            string uniqueName;

            // tag it with UniqueName
            if (nameChecklist.ContainsKey("stackedBarChart"))
            {
                int lastUsedId = nameChecklist["stackedBarChart"];
                uniqueName = "stackedBarChart" + (lastUsedId + 1).ToString();
                nameChecklist["stackedBarChart"] = lastUsedId + 1;
            }
            else
            {
                uniqueName = "stackedBarChart";
                nameChecklist["stackedBarChart"] = 1;
            }
            this.UniqueName = uniqueName;

            return nameChecklist;
        }
    }
}
