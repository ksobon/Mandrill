using D3jsLib.Utilities;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Media;

namespace D3jsLib.ParallelCoordinates
{
    public class ParallelCoordinatesStyle
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Color LineColor { get; set; }
    }

    public class ParallelCoordinatesDataPoint
    {
        public string Name { get; set; }
        public Dictionary<string, double> Values { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> output = new Dictionary<string, object>();
            output.Add("Name", this.Name);
            foreach (var value in this.Values)
            {
                output.Add(value.Key, value.Value);
            }

            return output;
        }
    }

    public class ParallelCoordinatesData
    {
        public List<ParallelCoordinatesDataPoint> Data { get; set; }

        public string ToJsonString()
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (ParallelCoordinatesDataPoint dp in this.Data)
            {
                list.Add(dp.ToDictionary());
            }

            // serialize C# Array into JS Array
            var serializer = new JavaScriptSerializer();
            string jsData = serializer.Serialize(list);

            return jsData;
        }
    }

    public class ParallelCoordinatesModel : ChartModel
    {
        public string Data { get; set; }
        public string LineColor { get; set; }
    }

    public class ParallelCoordinatesChart : Chart
    {
        public ParallelCoordinatesData ParallelCoordinatestData;
        public ParallelCoordinatesStyle ParallelCoordinatesStyle;
        public string UniqueName { get; set; }

        public ParallelCoordinatesChart(ParallelCoordinatesData data, ParallelCoordinatesStyle style)
        {
            this.ParallelCoordinatestData = data;
            this.ParallelCoordinatesStyle = style;
        }

        public override void CreateChartModel()
        {
            ParallelCoordinatesModel model = new ParallelCoordinatesModel();
            model.DivId = "parallelCoordinates" + this.UniqueName;
            model.ColMdValue = this.ColMdValue;
            model.Width = this.ParallelCoordinatesStyle.Width.ToString();
            model.Height = this.ParallelCoordinatesStyle.Height.ToString();
            model.LineColor = ChartsUtilities.ColorToHexString(this.ParallelCoordinatesStyle.LineColor);
            model.Data = this.ParallelCoordinatestData.ToJsonString();

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTempParallelCoordinates" + counter.ToString();
            ParallelCoordinatesModel model = this.ChartModel as ParallelCoordinatesModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.ParallelCoordinatesChart.parallelCoordinatesChart.html", templateName);
            return colString;
        }

        public override Dictionary<string, int> AssignUniqueName(Dictionary<string, int> nameChecklist)
        {
            string uniqueName;

            // tag it with UniqueName
            if (nameChecklist.ContainsKey("parallelCoordinatesChart"))
            {
                int lastUsedId = nameChecklist["parallelCoordinatesChart"];
                uniqueName = "parallelCoordinatesChart" + (lastUsedId + 1).ToString();
                nameChecklist["parallelCoordinatesChart"] = lastUsedId + 1;
            }
            else
            {
                uniqueName = "parallelCoordinatesChart";
                nameChecklist["parallelCoordinatesChart"] = 1;
            }
            this.UniqueName = uniqueName;

            return nameChecklist;
        }
    }
}