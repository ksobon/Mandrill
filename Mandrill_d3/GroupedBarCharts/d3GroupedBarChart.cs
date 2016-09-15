using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Media;

namespace D3jsLib.GroupedBarChart
{
    public class GroupedBarChartStyle
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<string> Colors { get; set; }
        public string YAxisLabel { get; set; }
        public Color BarHoverColor { get; set; }
    }

    public class GroupedBarChartDataPoint
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

    public class GroupedBarChartData
    {
        public List<GroupedBarChartDataPoint> Data { get; set; }
        public Domain Domain { get; set; }

        public string ToJsonString()
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (GroupedBarChartDataPoint dp in this.Data)
            {
                list.Add(dp.ToDictionary());
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(list);

            return jsData;
        }
    }

    public class GroupedBarChartModel : ChartModel
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

    public class GroupedBarChart : Chart
    {
        public GroupedBarChartData GroupedBarChartData;
        public GroupedBarChartStyle GroupedBarChartStyle;
        public string UniqueName { get; set; }

        public GroupedBarChart(GroupedBarChartData data, GroupedBarChartStyle style)
        {
            this.GroupedBarChartData = data;
            this.GroupedBarChartStyle = style;
        }

        public override void CreateChartModel()
        {
            GroupedBarChartModel model = new GroupedBarChartModel();
            model.DivId = "groupedBar" + this.UniqueName;
            model.ColMdValue = this.ColMdValue;
            model.Width = this.GroupedBarChartStyle.Width.ToString();
            model.Height = this.GroupedBarChartStyle.Height.ToString();
            model.YAxisLabel = this.GroupedBarChartStyle.YAxisLabel;
            model.BarHover = ChartsUtilities.ColorToHexString(this.GroupedBarChartStyle.BarHoverColor);

            if (this.GroupedBarChartStyle.Colors != null)
            {
                string domainColors = new JavaScriptSerializer().Serialize(this.GroupedBarChartStyle.Colors);
                model.DomainColors = domainColors;
                model.Colors = true;
            }
            else
            {
                model.Colors = false;
            }


            if (this.GroupedBarChartData.Domain == null)
            {
                model.Domain = false;
            }
            else
            {
                model.Domain = true;
                model.DomainA = this.GroupedBarChartData.Domain.A.ToString();
                model.DomainB = this.GroupedBarChartData.Domain.B.ToString();
            }

            model.Data = this.GroupedBarChartData.ToJsonString();

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempGroupedBar" + counter.ToString();
            GroupedBarChartModel model = this.ChartModel as GroupedBarChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.GroupedBarCharts.GroupedBarChartScript.html", templateName);
            return colString;
        }

        public override Dictionary<string, int> AssignUniqueName(Dictionary<string, int> nameChecklist)
        {
            string uniqueName;

            // tag it with UniqueName
            if (nameChecklist.ContainsKey("groupedBarChart"))
            {
                int lastUsedId = nameChecklist["groupedBarChart"];
                uniqueName = "groupedBarChart" + (lastUsedId + 1).ToString();
                nameChecklist["groupedBarChart"] = lastUsedId + 1;
            }
            else
            {
                uniqueName = "groupedBarChart";
                nameChecklist["groupedBarChart"] = 1;
            }
            this.UniqueName = uniqueName;

            return nameChecklist;
        }
    }
}
