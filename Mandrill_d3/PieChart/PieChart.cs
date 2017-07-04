using D3jsLib.Utilities;

namespace D3jsLib.PieChart
{
    public class PieChartStyle : ChartStyle
    {
        public string HoverColor { get; set; }
        public string Colors { get; set; }
        public bool Labels { get; set; }
    }

    public class PieChartData : ChartData
    {
    }

    public class PieChart : Chart
    {
        public PieChart(PieChartData data, PieChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempPie" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.PieChart.PieChartScript.html", templateName);
            return colString;
        }
    }
}
