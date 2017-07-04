using D3jsLib.Utilities;

namespace D3jsLib.d3ScatterPlots
{
    public class ScatterPlotStyle : ChartStyle
    {
        public string YAxisLabel { get; set; }
        public string XAxisLabel { get; set; }
        public string DotColor { get; set; }
    }

    public class ScatterPlotDataPoint
    {
        public string name { get; set; }
        public double valueX { get; set; }
        public double valueY { get; set; }
        public double size { get; set; }
        public int color { get; set; }
    }

    public class ScatterPlotData : ChartData
    {
        public Domain DomainX { get; set; }
        public Domain DomainY { get; set; }
    }

    public class d3ScatterPlot : Chart
    {
        public d3ScatterPlot(ScatterPlotData data, ScatterPlotStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempScatter" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.ScatterPlots.ScatterPlotScript.html", templateName);
            return colString;
        }
    }
}
