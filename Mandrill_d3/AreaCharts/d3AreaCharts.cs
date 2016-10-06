using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Media;

namespace D3jsLib.d3AreaCharts
{
    public class AreaChartStyle : ChartStyle
    {
        public string YAxisLabel { get; set; }
        public Color AreaColor { get; set; }
        public int TickMarksX { get; set; }
    }

    public class AreaChartDataPoint
    {
        public string name { get; set; }
        public double value { get; set; }
    }

    public class AreaChartData
    {
        public List<AreaChartDataPoint> Data { get; set; }
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
        public AreaChartData AreaChartData;
        public AreaChartStyle AreaChartStyle;
        public string UniqueName { get; set; }
        public string ScriptString { get; set; }
        public List<string> ImportsList { get; set; }

        public d3AreaChart(AreaChartData data, AreaChartStyle style)
        {
            this.AreaChartData = data;
            this.AreaChartStyle = style;
        }

        public override void CreateChartModel(int counter)
        {
            AreaChartModel model = new AreaChartModel();
            model.Width = this.AreaChartStyle.Width.ToString();
            model.Height = this.AreaChartStyle.Height.ToString();
            model.YAxisLabel = this.AreaChartStyle.YAxisLabel;
            model.AreaColor = ChartsUtilities.ColorToHexString(this.AreaChartStyle.AreaColor);
            model.TickMarksX = this.AreaChartStyle.TickMarksX.ToString();
            model.DivId = "div" + counter.ToString();

            // set grid address
            model.GridRow = this.AreaChartStyle.GridRow.ToString();
            model.GridColumn = this.AreaChartStyle.GridColumn.ToString();

            // always round up for the grid size so chart is smaller then container
            model.SizeX = System.Math.Ceiling(this.AreaChartStyle.Width / 100d).ToString();
            model.SizeY = System.Math.Ceiling(this.AreaChartStyle.Height / 100d).ToString();

            if (this.AreaChartData.Domain == null)
            {
                model.Domain = false;
            }
            else
            {
                model.Domain = true;
                model.DomainA = this.AreaChartData.Domain.A.ToString();
                model.DomainB = this.AreaChartData.Domain.B.ToString();
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(this.AreaChartData.Data);
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

