using D3jsLib.Utilities;

namespace D3jsLib.ParallelCoordinates
{
    public class ParallelCoordinatesStyle : ChartStyle
    {
        public string LineColor { get; set; }
    }

    public class ParallelCoordinatesData : ChartData
    {
    }

    public class ParallelCoordinatesChart : Chart
    {
        public ParallelCoordinatesChart(ParallelCoordinatesData data, ParallelCoordinatesStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempParallelCoordinates" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.ParallelCoordinatesChart.parallelCoordinatesChart.html", templateName);
            return colString;
        }
    }
}