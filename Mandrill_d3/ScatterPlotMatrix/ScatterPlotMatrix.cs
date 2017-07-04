using D3jsLib.Utilities;

namespace D3jsLib.ScatterPlotMatrix
{
    public class ScatterPlotMatrixStyle : ChartStyle
    {
        public string Colors { get; set; }
    }

    public class ScatterPlotMatrixData : ChartData
    {
    }

    public class ScatterPlotMatrix : Chart
    {
        public ScatterPlotMatrix(ScatterPlotMatrixData data, ScatterPlotMatrixStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempScatterPlotMatrix" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.ScatterPlotMatrix.ScatterPlotMatrix.html", templateName);
            return colString;
        }
    }
}