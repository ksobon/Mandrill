using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Script.Serialization;

namespace D3jsLib.d3AreaCharts
{
    public class AreaChartStyle : ChartStyle
    {
        public string YAxisLabel { get; set; }
        public Color AreaColor { get; set; }
        public int TickMarksX { get; set; }
    }

    public class AreaChartData
    {
        public List<DataPoint1> Data { get; set; }
        public Domain Domain { get; set; }
    }

    public class AreaChartModel : ChartModel
    {
        public string YAxisLabel { get; set; }
        public string AreaColor { get; set; }
        public bool Domain { get; set; }
        public string DomainA { get; set; }
        public string DomainB { get; set; }
        public string TickMarksX { get; set; }
        public string Data { get; set; }
    }

    public class d3AreaChart : Chart
    {
        public AreaChartData Data;
        public AreaChartStyle Style;

        public d3AreaChart(AreaChartData data, AreaChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override void CreateChartModel(int counter)
        {
            AreaChartModel model = new AreaChartModel();
            model.Width = this.Style.Width;
            model.Height = this.Style.Height;
            model.YAxisLabel = this.Style.YAxisLabel;
            model.AreaColor = ChartsUtilities.ColorToHexString(this.Style.AreaColor);
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
            string templateName = "colDivTempArea" + counter.ToString();
            AreaChartModel model = this.ChartModel as AreaChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.AreaCharts.AreaChartScript.html", templateName);
            return colString;
        }
    }
}

