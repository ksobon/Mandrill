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

        //public override void CreateChartModel(int counter)
        //{
        //    ScatterPlotMatrixModel model = new ScatterPlotMatrixModel();
        //    model.Width = this.Style.Width;
        //    model.Height = this.Style.Width;
        //    model.Data = this.Data.ToJsonString();
        //    model.DivId = "div" + counter.ToString();

        //    // Frame width is equal to chart width / number of frames - 10 for padding
        //    int count = this.Data.Data[0].Values.Count;
        //    model.FrameSize = ((this.Style.Width / count) - 10).ToString();

        //    if (this.Style.Colors != null)
        //    {
        //        string domainColors = new JavaScriptSerializer().Serialize(this.Style.Colors);
        //        model.DomainColors = domainColors;
        //        model.Colors = true;
        //    }
        //    else
        //    {
        //        model.Colors = false;
        //    }

        //    // set grid address
        //    model.GridRow = this.Style.GridRow;
        //    model.GridColumn = this.Style.GridColumn;

        //    // always round up for the grid size so chart is smaller then container
        //    model.SizeX = (int)System.Math.Ceiling(this.Style.Width / 100d);
        //    model.SizeY = (int)System.Math.Ceiling(this.Style.Width / 100d);

        //    this.ChartModel = model;
        //}

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempScatterPlotMatrix" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.ScatterPlotMatrix.ScatterPlotMatrix.html", templateName);
            return colString;
        }
    }
}