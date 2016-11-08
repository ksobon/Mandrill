using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Script.Serialization;

namespace D3jsLib.DonutChart
{
    public class DonutChartStyle : ChartStyle
    {
        public Color HoverColor { get; set; }
        public List<string> Colors { get; set; }
        public bool Labels { get; set; }
        public string TotalLabel { get; set; }
    }

    public class DonutChartData
    {
        public List<DataPoint1> Data { get; set; }
    }

    public class DonutChartModel : ChartModel
    {
        public string HoverColor { get; set; }
        public string Data { get; set; }
        public bool Colors { get; set; }
        public string DomainColors { get; set; }
        public bool Labels { get; set; }
        public string TotalLabel { get; set; }
    }

    public class DonutChart : Chart
    {
        public DonutChartData Data;
        public DonutChartStyle Style;

        public DonutChart(DonutChartData data, DonutChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override void CreateChartModel(int counter)
        {
            DonutChartModel model = new DonutChartModel();
            model.Width = this.Style.Width;
            model.HoverColor = ChartsUtilities.ColorToHexString(this.Style.HoverColor);
            model.Labels = this.Style.Labels;
            model.DivId = "div" + counter.ToString();
            model.TotalLabel = this.Style.TotalLabel;
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
            string templateName = "colDivTempDonut" + counter.ToString();
            DonutChartModel model = this.ChartModel as DonutChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.DonutCharts.DonutChartScript.html", templateName);
            return colString;
        }
    }
}