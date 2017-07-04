using D3jsLib.Utilities;

namespace D3jsLib.LineChart
{
    public class LineChartStyle : ChartStyle
    {
        public string YAxisLabel { get; set; }
        public string LineColor { get; set; }
        public int TickMarksX { get; set; }
    }

    public class LineChartData : ChartData
    {
    }

    public class LineChart : Chart
    {
        public LineChart(LineChartData data, LineChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempLine" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.LineCharts.LineChartScript.html", templateName);
            return colString;
        }
    }
}
