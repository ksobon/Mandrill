using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Media;

namespace D3jsLib.DonutChart
{
    public class DonutChartStyle : ChartStyle
    {
        public Color HoverColor { get; set; }
        public List<string> Colors { get; set; }
        public bool Labels { get; set; }
        public bool Legend { get; set; }
    }

    public class DonutChartDataPoint
    {
        public string name { get; set; }
        public double val { get; set; }
    }

    public class DonutChartData
    {
        public List<DonutChartDataPoint> Data { get; set; }
    }

    public class DonutChartModel : ChartModel
    {
        public string HoverColor { get; set; }
        public string Data { get; set; }
        public bool Colors { get; set; }
        public string DomainColors { get; set; }
        public bool Labels { get; set; }
        public bool Legend { get; set; }
    }

    public class DonutChart : Chart
    {
        public DonutChartData DonutChartData;
        public DonutChartStyle DonutChartStyle;
        public string UniqueName { get; set; }

        public DonutChart(DonutChartData data, DonutChartStyle style)
        {
            this.DonutChartData = data;
            this.DonutChartStyle = style;
        }

        public override void CreateChartModel(int counter)
        {
            DonutChartModel model = new DonutChartModel();
            model.Width = this.DonutChartStyle.Width.ToString();
            model.Height = this.DonutChartStyle.Height.ToString();
            model.HoverColor = ChartsUtilities.ColorToHexString(this.DonutChartStyle.HoverColor);
            model.Labels = this.DonutChartStyle.Labels;
            model.Legend = this.DonutChartStyle.Legend;
            model.DivId = "div" + counter.ToString();

            // set grid address
            model.GridRow = this.DonutChartStyle.GridRow.ToString();
            model.GridColumn = this.DonutChartStyle.GridColumn.ToString();

            // always round up for the grid size so chart is smaller then container
            model.SizeX = System.Math.Ceiling(this.DonutChartStyle.Width / 100d).ToString();
            model.SizeY = System.Math.Ceiling(this.DonutChartStyle.Height / 100d).ToString();

            if (this.DonutChartStyle.Colors != null)
            {
                string domainColors = new JavaScriptSerializer().Serialize(this.DonutChartStyle.Colors);
                model.DomainColors = domainColors;
                model.Colors = true;
            }
            else
            {
                model.Colors = false;
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(this.DonutChartData.Data);
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

        public override string EvaluateDivTemplate(int counter)
        {
            string templateName = "divTempDonut" + counter.ToString();
            DonutChartModel model = this.ChartModel as DonutChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.Gridster.divTemplate.html", templateName);
            return colString;
        }
    }
}