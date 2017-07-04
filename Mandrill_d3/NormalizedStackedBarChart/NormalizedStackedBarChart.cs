using D3jsLib.Utilities;

namespace D3jsLib.NormalizedStackedBarChart
{
    public class NormalizedStackedBarChartStyle : ChartStyle
    {
        public string Colors { get; set; }
        public string YAxisLabel { get; set; }
        public string BarHoverColor { get; set; }
    }

    public class NormalizedStackedBarChartData : ChartData
    {
    }

    public class NormalizedStackedBarChart : Chart
    {
        public NormalizedStackedBarChart(NormalizedStackedBarChartData data, NormalizedStackedBarChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempNormalizedStackedBar" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.NormalizedStackedBarChart.NormalizedStackedBarChart.html", templateName);
            return colString;
        }
    }
}
