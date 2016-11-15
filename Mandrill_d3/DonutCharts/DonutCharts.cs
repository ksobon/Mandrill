using D3jsLib.Utilities;

namespace D3jsLib.DonutChart
{
    public class DonutChartStyle : ChartStyle
    {
        public string HoverColor { get; set; }
        public string Colors { get; set; }
        public bool Labels { get; set; }
        public string TotalLabel { get; set; }
    }

    public class DonutChartData : ChartData
    {
    }

    public class DonutChart : Chart
    {
        public DonutChart(DonutChartData data, DonutChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempDonut" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.DonutCharts.DonutChartScript.html", templateName);
            return colString;
        }
    }
}