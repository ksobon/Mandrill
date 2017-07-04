using D3jsLib.Utilities;

namespace D3jsLib.StackedBarChart
{
    public class StackedBarChartStyle : ChartStyle
    {
        public string Colors { get; set; }
        public string YAxisLabel { get; set; }
        public string BarHoverColor { get; set; }
    }

    public class StackedBarChartData : ChartData
    {
    }

    public class StackedBarChart : Chart
    {
        public StackedBarChart(StackedBarChartData data, StackedBarChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempStackedBar" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.StackedBarChart.d3StackedBarChart.html", templateName);
            return colString;
        }
    }
}
