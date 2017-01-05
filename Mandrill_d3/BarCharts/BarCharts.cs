using D3jsLib.Utilities;

namespace D3jsLib.BarChart
{
    public class BarData : ChartData
    {
    }

    public class BarStyle : ChartStyle
    {
        public string BarColor { get; set; }
        public string BarHoverColor { get; set; }
        public string YAxisLabel { get; set; }
        public bool xTextRotation { get; set; }
        public int TickMarksX { get; set; }
        public bool Labels { get; set; }
    }

    public class BarChart : Chart
    {
        public BarChart(BarData data, BarStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempBar" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.BarCharts.BarChartScript.html", templateName);
            return colString;
        }
    }
}
