using D3jsLib.Utilities;

namespace D3jsLib.HorizontalBarChart
{
    public class HorizontalBarChartStyle : ChartStyle
    {
        public string BarColor { get; set; }
        public string BarHoverColor { get; set; }
        public string YAxisLabel { get; set; }
    }

    public class HorizontalBarChartData : ChartData
    {
    }

    public class HorizontalBarChart : Chart
    {
        public HorizontalBarChart(HorizontalBarChartData data, HorizontalBarChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempHorizontalBar" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.HorizontalBarChart.HorizontalBarChart.html", templateName);
            return colString;
        }
    }
}
