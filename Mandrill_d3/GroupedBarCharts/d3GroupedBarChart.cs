using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Media;

namespace D3jsLib.GroupedBarChart
{
    public class GroupedBarChartStyle : ChartStyle
    {
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
        public GroupedBarChartData Data;
        public GroupedBarChartStyle Style;
        public string UniqueName { get; set; }

        public GroupedBarChart(GroupedBarChartData data, GroupedBarChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override void CreateChartModel(int counter)
        {
            GroupedBarChartModel model = new GroupedBarChartModel();
            model.Width = this.Style.Width.ToString();
            model.Height = this.Style.Height.ToString();
            model.YAxisLabel = this.Style.YAxisLabel;
            model.BarHover = ChartsUtilities.ColorToHexString(this.Style.BarHoverColor);
            model.DivId = "div" + counter.ToString();

            // set grid address
            model.GridRow = this.Style.GridRow.ToString();
            model.GridColumn = this.Style.GridColumn.ToString();

            // always round up for the grid size so chart is smaller then container
            model.SizeX = System.Math.Ceiling(this.Style.Width / 100d).ToString();
            model.SizeY = System.Math.Ceiling(this.Style.Height / 100d).ToString();
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


            if (this.Data.Domain == null)
            {
                model.Domain = false;
            }
            else
            {
                model.Domain = true;
                model.DomainA = this.Data.Domain.A.ToString();
                model.DomainB = this.Data.Domain.B.ToString();
            }

            model.Data = this.Data.ToJsonString();

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempGroupedBar" + counter.ToString();
            GroupedBarChartModel model = this.ChartModel as GroupedBarChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.GroupedBarCharts.GroupedBarChartScript.html", templateName);
            return colString;
        }

        public override string EvaluateDivTemplate(int counter)
        {
            string templateName = "divTempGroupedBar" + counter.ToString();
            GroupedBarChartModel model = this.ChartModel as GroupedBarChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.Gridster.divTemplate.html", templateName);
            return colString;
        }
    }
}
