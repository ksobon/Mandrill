using D3jsLib.Utilities;

namespace D3jsLib.Legend
{
    public class LegendStyle : ChartStyle
    {
        public string Title { get; set; }
        public string Colors { get; set; }
        public int RectangleSize { get; set; }
    }

    public class LegendData : ChartData
    {
    }

    public class Legend : Chart
    {
        public Legend(LegendData data, LegendStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempLegend" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.Legend.Legend.html", templateName);
            return colString;
        }
    }
}
