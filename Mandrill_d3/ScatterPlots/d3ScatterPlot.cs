using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Media;

namespace D3jsLib.d3ScatterPlots
{
    public class ScatterPlotStyle
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string YAxisLabel { get; set; }
        public string XAxisLabel { get; set; }
        public Color DotColor { get; set; }
    }

    public class ScatterPlotDataPoint
    {
        public string name { get; set; }
        public double valueX { get; set; }
        public double valueY { get; set; }
        public double size { get; set; }
        public string color { get; set; }
    }

    public class ScatterPlotData
    {
        public List<ScatterPlotDataPoint> Data { get; set; }
        public Domain DomainX { get; set; }
        public Domain DomainY { get; set; }
    }

    public class ScatterPlotModel : ChartModel
    {
        public string Data { get; set; }
        public string YAxisLabel { get; set; }
        public string XAxisLabel { get; set; }
        public string DotColor { get; set; }
        public bool DomainX { get; set; }
        public bool DomainY { get; set; }
        public string DomainXA { get; set; }
        public string DomainXB { get; set; }
        public string DomainYA { get; set; }
        public string DomainYB { get; set; }
    }

    public class d3ScatterPlot : Chart
    {
        public ScatterPlotData ScatterPlotData;
        public ScatterPlotStyle ScatterPlotStyle;
        public string UniqueName { get; set; }
        public string ScriptString { get; set; }
        public List<string> ImportsList { get; set; }

        public d3ScatterPlot(ScatterPlotData data, ScatterPlotStyle style)
        {
            this.ScatterPlotData = data;
            this.ScatterPlotStyle = style;
        }

        public override void CreateChartModel()
        {
            ScatterPlotModel model = new ScatterPlotModel();
            model.DivId = "scatterPlot" + this.UniqueName;
            model.ColMdValue = this.ColMdValue;
            model.Width = this.ScatterPlotStyle.Width.ToString();
            model.Height = this.ScatterPlotStyle.Height.ToString();
            model.YAxisLabel = this.ScatterPlotStyle.YAxisLabel;
            model.XAxisLabel = this.ScatterPlotStyle.XAxisLabel;
            model.DotColor = ChartsUtilities.ColorToHexString(this.ScatterPlotStyle.DotColor);

            if (this.ScatterPlotData.DomainX == null)
            {
                model.DomainX = false;
            }
            else
            {
                model.DomainX = true;
                model.DomainXA = this.ScatterPlotData.DomainX.A.ToString();
                model.DomainXB = this.ScatterPlotData.DomainX.B.ToString();
            }

            if (this.ScatterPlotData.DomainY == null)
            {
                model.DomainY = false;
            }
            else
            {
                model.DomainY = true;
                model.DomainYA = this.ScatterPlotData.DomainY.A.ToString();
                model.DomainYB = this.ScatterPlotData.DomainY.B.ToString();
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(this.ScatterPlotData.Data);
            model.Data = jsData;

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempScatter" + counter.ToString();
            ScatterPlotModel model = this.ChartModel as ScatterPlotModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.ScatterPlots.ScatterPlotScript.html", templateName);
            return colString;
        }

        public override Dictionary<string, int> AssignUniqueName(Dictionary<string, int> nameChecklist)
        {
            string uniqueName;

            // tag scatter plot with unique name
            if (nameChecklist.ContainsKey("scatterPlot"))
            {
                int lastUsedId = nameChecklist["scatterPlot"];
                uniqueName = "scatterPlot" + (lastUsedId + 1).ToString();
                nameChecklist["scatterPlot"] = lastUsedId + 1;
            }
            else
            {
                uniqueName = "scatterPlot1";
                nameChecklist["scatterPlot"] = 1;
            }
            this.UniqueName = uniqueName;

            return nameChecklist;
        }
    }
}
