using D3jsLib.Utilities;

namespace D3jsLib.AreaCharts
{
    public class AreaChartStyle : ChartStyle
    {
        public string YAxisLabel { get; set; }
        public string AreaColor { get; set; }
        public int TickMarksX { get; set; }
    }

    public class AreaChartData : ChartData
    {
    }

    public class AreaChart : Chart
    {
        public AreaChart(AreaChartData data, AreaChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempArea" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.AreaCharts.AreaChartScript.html", templateName);
            return colString;
        }
    }
}

