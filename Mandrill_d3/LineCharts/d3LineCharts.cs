using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Media;

namespace D3jsLib.d3LineCharts
{
    public class LineChartStyle : ChartStyle
    {
        public string YAxisLabel { get; set; }
        public Color LineColor { get; set; }
        public int TickMarksX { get; set; }
    }

    public class LineChartDataPoint
    {
        public string name { get; set; }
        public double value { get; set; }
    }

    public class LineChartData
    {
        public List<LineChartDataPoint> Data { get; set; }
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
            model.Width = this.Style.Width.ToString();
            model.Height = this.Style.Height.ToString();
            model.YAxisLabel = this.Style.YAxisLabel;
            model.LineColor = ChartsUtilities.ColorToHexString(this.Style.LineColor);
            model.TickMarksX = this.Style.TickMarksX.ToString();
            model.DivId = "div" + counter.ToString();

            // set grid address
            model.GridRow = this.Style.GridRow.ToString();
            model.GridColumn = this.Style.GridColumn.ToString();

            // always round up for the grid size so chart is smaller then container
            model.SizeX = System.Math.Ceiling(this.Style.Width / 100d).ToString();
            model.SizeY = System.Math.Ceiling(this.Style.Height / 100d).ToString();

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

        public override string EvaluateDivTemplate(int counter)
        {
            string templateName = "divTempLine" + counter.ToString();
            LineChartModel model = this.ChartModel as LineChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.Gridster.divTemplate.html", templateName);
            return colString;
        }
    }
}
