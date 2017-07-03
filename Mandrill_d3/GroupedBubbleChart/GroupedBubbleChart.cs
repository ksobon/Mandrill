using D3jsLib.Utilities;

namespace D3jsLib.GroupedBubbleChart
{
    public class GroupedBubbleChartStyle : ChartStyle
    {
        public string Colors { get; set; }
    }

    public class GroupedBubbleChartData : ChartData
    {
    }

    public class GroupedBubbleChart : Chart
    {
        public GroupedBubbleChart(GroupedBubbleChartData data, GroupedBubbleChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempGroupedBubble" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.GroupedBubbleChart.GroupedBubbleChartScript.html", templateName);
            return colString;
        }
    }
}
