using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace D3jsLib.Legend
{
    public class LegendStyle : ChartStyle
    {
        public string Title { get; set; }
        public List<string> Colors { get; set; }
        public int RectangleSize { get; set; }
    }

    public class LegendData
    {
        public List<string> Data { get; set; }
    }

    public class LegendModel : ChartModel
    {
        public string Data { get; set; }
        public string Title { get; set; }
        public bool Colors { get; set; }
        public string DomainColors { get; set; }
        public int RectangleSize { get; set; }
    }

    public class Legend : Chart
    {
        public LegendData Data;
        public LegendStyle Style;

        public Legend(LegendData data, LegendStyle style)
        {
            this.Data = data;
            this.Style = style;
        }

        public override void CreateChartModel(int counter)
        {
            LegendModel model = new LegendModel();
            model.Width = this.Style.Width.ToString();
            model.Height = this.Style.Height.ToString();
            model.DivId = "div" + counter.ToString();
            model.Title = this.Style.Title;
            model.RectangleSize = this.Style.RectangleSize;

            // set grid address
            model.GridRow = this.Style.GridRow.ToString();
            model.GridColumn = this.Style.GridColumn.ToString();

            // always round up for the grid size so chart is smaller then container
            model.SizeX = System.Math.Ceiling(this.Style.Width / 100d).ToString();
            model.SizeY = System.Math.Ceiling(this.Style.Height / 100d).ToString();

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
            string templateName = "colDivTempLegend" + counter.ToString();
            LegendModel model = this.ChartModel as LegendModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.Legend.Legend.html", templateName);
            return colString;
        }
    }
}
