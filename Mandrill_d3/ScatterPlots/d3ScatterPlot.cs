using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Script.Serialization;

namespace D3jsLib.d3ScatterPlots
{
    public class ScatterPlotStyle : ChartStyle
    {
        public string YAxisLabel { get; set; }
        public string XAxisLabel { get; set; }
        public Color DotColor { get; set; }
    }

    public class ScatterPlotDataPoint
    {
        public string name { get; set; }
        public double valueX { get; set; }
        public double valueY { get; set; }
        public double size { get; set; }
        public string color { get; set; }
    }

    public class ScatterPlotData
    {
        public List<ScatterPlotDataPoint> Data { get; set; }
        public Domain DomainX { get; set; }
        public Domain DomainY { get; set; }
    }

    public class ScatterPlotModel : ChartModel
    {
        public string Data { get; set; }
        public string YAxisLabel { get; set; }
        public string XAxisLabel { get; set; }
        public string DotColor { get; set; }
        public bool DomainX { get; set; }
        public bool DomainY { get; set; }
        public string DomainXA { get; set; }
        public string DomainXB { get; set; }
        public string DomainYA { get; set; }
        public string DomainYB { get; set; }
    }

    public class d3ScatterPlot : Chart
    {
        public ScatterPlotData Data;
        public ScatterPlotStyle Style;

        public d3ScatterPlot(ScatterPlotData data, ScatterPlotStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override void CreateChartModel(int counter)
        {
            ScatterPlotModel model = new ScatterPlotModel();
            model.Width = this.Style.Width;
            model.Height = this.Style.Height;
            model.YAxisLabel = this.Style.YAxisLabel;
            model.XAxisLabel = this.Style.XAxisLabel;
            model.DotColor = ChartsUtilities.ColorToHexString(this.Style.DotColor);
            model.DivId = "div" + counter.ToString();
            model.Margins = this.Style.Margins;

            // set grid address
            model.GridRow = this.Style.GridRow;
            model.GridColumn = this.Style.GridColumn;

            // always round up for the grid size so chart is smaller then container
            model.SizeX = (int)System.Math.Ceiling(this.Style.Width / 100d);
            model.SizeY = (int)System.Math.Ceiling(this.Style.Height / 100d);

            if (this.Data.DomainX == null)
            {
                model.DomainX = false;
            }
            else
            {
                model.DomainX = true;
                model.DomainXA = this.Data.DomainX.A.ToString();
                model.DomainXB = this.Data.DomainX.B.ToString();
            }

            if (this.Data.DomainY == null)
            {
                model.DomainY = false;
            }
            else
            {
                model.DomainY = true;
                model.DomainYA = this.Data.DomainY.A.ToString();
                model.DomainYB = this.Data.DomainY.B.ToString();
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(this.Data.Data);
            model.Data = jsData;

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempScatter" + counter.ToString();
            ScatterPlotModel model = this.ChartModel as ScatterPlotModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.ScatterPlots.ScatterPlotScript.html", templateName);
            return colString;
        }
    }
}
