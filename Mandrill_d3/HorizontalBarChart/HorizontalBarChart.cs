using System.Collections.Generic;
using System.Drawing;
using System.Web.Script.Serialization;
using D3jsLib.Utilities;

namespace D3jsLib.HorizontalBarChart
{
    public class HorizontalBarChartStyle : ChartStyle
    {
        public Color BarColor { get; set; }
        public Color BarHoverColor { get; set; }
        public string YAxisLabel { get; set; }
    }

    public class HorizontalBarChartData
    {
        public List<DataPoint1> Data { get; set; }
        public Domain Domain { get; set; }
    }

    public class HorizontalBarChartModel : ChartModel
    {
        public string BarFill { get; set; }
        public string BarHover { get; set; }
        public string YAxisLabel { get; set; }
        public bool Domain { get; set; }
        public string DomainA { get; set; }
        public string DomainB { get; set; }
        public string Data { get; set; }
    }

    public class HorizontalBarChart : Chart
    {
        public HorizontalBarChartData Data;
        public HorizontalBarChartStyle Style;

        public HorizontalBarChart(HorizontalBarChartData data, HorizontalBarChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override void CreateChartModel(int counter)
        {
            HorizontalBarChartModel model = new HorizontalBarChartModel();
            model.Width = this.Style.Width;
            model.Height = this.Style.Height;
            model.YAxisLabel = this.Style.YAxisLabel;
            model.BarFill = ChartsUtilities.ColorToHexString(this.Style.BarColor);
            model.BarHover = ChartsUtilities.ColorToHexString(this.Style.BarHoverColor);
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
            string templateName = "colDivTempHorizontalBar" + counter.ToString();
            HorizontalBarChartModel model = this.ChartModel as HorizontalBarChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.HorizontalBarChart.HorizontalBarChart.html", templateName);
            return colString;
        }
    }
}
