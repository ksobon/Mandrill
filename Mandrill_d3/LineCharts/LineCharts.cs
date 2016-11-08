using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Script.Serialization;

namespace D3jsLib.LineChart
{
    public class LineChartStyle : ChartStyle
    {
        public string YAxisLabel { get; set; }
        public Color LineColor { get; set; }
        public int TickMarksX { get; set; }
    }

    public class LineChartData
    {
        public List<DataPoint1> Data { get; set; }
        public Domain Domain { get; set; }
    }

    public class LineChartModel : ChartModel
    {
        public string Data { get; set; }
        public string YAxisLabel { get; set; }
        public string LineColor { get; set; }
        public string TickMarksX { get; set; }
        public bool Domain { get; set; }
        public string DomainA { get; set; }
        public string DomainB { get; set; }
    }

    public class d3LineChart : Chart
    {
        public LineChartData Data;
        public LineChartStyle Style;

        public d3LineChart(LineChartData data, LineChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override void CreateChartModel(int counter)
        {
            LineChartModel model = new LineChartModel();
            model.Width = this.Style.Width;
            model.Height = this.Style.Height;
            model.YAxisLabel = this.Style.YAxisLabel;
            model.LineColor = ChartsUtilities.ColorToHexString(this.Style.LineColor);
            model.TickMarksX = this.Style.TickMarksX.ToString();
            model.DivId = "div" + counter.ToString();
            model.Margins = this.Style.Margins;

            // set grid address
            model.GridRow = this.Style.GridRow;
            model.GridColumn = this.Style.GridColumn;

            // always round up for the grid size so chart is smaller then container
            model.SizeX = (int)System.Math.Ceiling(this.Style.Width / 100d);
            model.SizeY = (int)System.Math.Ceiling(this.Style.Height / 100d);

            if (this.Data.Domain == null)
            {
                model.Domain = false;
            }
            else
            {
                model.Domain = true;
                model.DomainA = this.Data.Domain.A.ToString();
                model.DomainB = this.Data.Domain.B.ToString();
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(this.Data.Data);
            model.Data = jsData;

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempLine" + counter.ToString();
            LineChartModel model = this.ChartModel as LineChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.LineCharts.LineChartScript.html", templateName);
            return colString;
        }
    }
}
