using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Script.Serialization;

namespace D3jsLib.PieChart
{
    public class PieChartStyle : ChartStyle
    {
        public Color HoverColor { get; set; }
        public List<string> Colors { get; set; }
        public bool Labels { get; set; }
    }

    public class PieChartData
    {
        public List<DataPoint1> Data { get; set; }
    }

    public class PieChartModel : ChartModel
    {
        public string HoverColor { get; set; }
        public string Data { get; set; }
        public bool Colors { get; set; }
        public string DomainColors { get; set; }
        public bool Labels { get; set; }
    }

    public class PieChart : Chart
    {
        public PieChartData Data;
        public PieChartStyle Style;

        public PieChart(PieChartData data, PieChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override void CreateChartModel(int counter)
        {
            PieChartModel model = new PieChartModel();
            model.Width = this.Style.Width;
            model.HoverColor = ChartsUtilities.ColorToHexString(this.Style.HoverColor);
            model.Labels = this.Style.Labels;
            model.DivId = "div" + counter.ToString();
            model.Margins = this.Style.Margins;

            // set grid address
            model.GridRow = this.Style.GridRow;
            model.GridColumn = this.Style.GridColumn;

            // always round up for the grid size so chart is smaller then container
            model.SizeX = (int)System.Math.Ceiling(this.Style.Width / 100d);
            model.SizeY = (int)System.Math.Ceiling(this.Style.Width / 100d);

            if (this.Style.Colors != null)
            {
                string domainColors = new JavaScriptSerializer().Serialize(this.Style.Colors);
                model.DomainColors = domainColors;
                model.Colors = true;
            }
            else
            {
                model.Colors = false;
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(this.Data.Data);
            model.Data = jsData;

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempPie" + counter.ToString();
            PieChartModel model = this.ChartModel as PieChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.PieChart.PieChartScript.html", templateName);
            return colString;
        }
    }
}
