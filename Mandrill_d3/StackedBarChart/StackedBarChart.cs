using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Script.Serialization;

namespace D3jsLib.StackedBarChart
{
    public class StackedBarChartStyle : ChartStyle
    {
        public List<string> Colors { get; set; }
        public string YAxisLabel { get; set; }
        public Color BarHoverColor { get; set; }
    }

    public class StackedBarChartData
    {
        public List<DataPoint2> Data { get; set; }
        public Domain Domain { get; set; }

        public string ToJsonString()
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataPoint2 dp in this.Data)
            {
                list.Add(dp.ToDictionary());
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(list);

            return jsData;
        }
    }

    public class StackedBarChartModel : ChartModel
    {
        public string Data { get; set; }
        public string YAxisLabel { get; set; }
        public bool Colors { get; set; }
        public string DomainColors { get; set; }
        public bool Domain { get; set; }
        public string DomainA { get; set; }
        public string DomainB { get; set; }
        public string BarHover { get; set; }
    }

    public class StackedBarChart : Chart
    {
        public StackedBarChartData Data;
        public StackedBarChartStyle Style;

        public StackedBarChart(StackedBarChartData data, StackedBarChartStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override void CreateChartModel(int counter)
        {
            StackedBarChartModel model = new StackedBarChartModel();
            model.Width = this.Style.Width;
            model.Height = this.Style.Height;
            model.YAxisLabel = this.Style.YAxisLabel;
            model.BarHover = ChartsUtilities.ColorToHexString(this.Style.BarHoverColor);
            model.DivId = "div" + counter.ToString();
            model.Margins = this.Style.Margins;

            // set grid address
            model.GridRow = this.Style.GridRow;
            model.GridColumn = this.Style.GridColumn;

            // always round up for the grid size so chart is smaller then container
            model.SizeX = (int)System.Math.Ceiling(this.Style.Width / 100d);
            model.SizeY = (int)System.Math.Ceiling(this.Style.Height / 100d);

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

            model.Data = this.Data.ToJsonString();

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempStackedBar" + counter.ToString();
            StackedBarChartModel model = this.ChartModel as StackedBarChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.StackedBarChart.d3StackedBarChart.html", templateName);
            return colString;
        }
    }
}
