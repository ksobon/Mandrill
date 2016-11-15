using D3jsLib.Utilities;

namespace D3jsLib.GroupedBarChart
{
    public class GroupedBarChartStyle : ChartStyle
    {
        public string Colors { get; set; }
        public string YAxisLabel { get; set; }
        public string BarHoverColor { get; set; }
    }

    public class GroupedBarChartData : ChartData
    {
    }

    public class GroupedBarChart : Chart
    {
        public GroupedBarChart(GroupedBarChartData data, GroupedBarChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempGroupedBar" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.GroupedBarCharts.GroupedBarChartScript.html", templateName);
            return colString;
        }
    }
}
