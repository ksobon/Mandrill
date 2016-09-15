using D3jsLib.Utilities;
using System;
using System.Collections.Generic;

namespace D3jsLib
{
    public class ImageStyle
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public string Tooltip { get; set; }
    }

    public class ImageModel : ChartModel
    {
        public string ImagePath { get; set; }
        public string Tooltip { get; set; }
    }

    public class Image : Chart
    {
        public string ImagePath { get; set; }
        public ImageStyle ImageStyle { get; set; }

        public Image(string imagePath, ImageStyle imageStyle)
        {
            this.ImagePath = imagePath;
            this.ImageStyle = imageStyle;
        }

        public override void CreateChartModel()
        {
            ImageModel model = new ImageModel();
            model.Width = this.ImageStyle.Width.ToString();
            model.Height = this.ImageStyle.Height.ToString();
            model.ImagePath = this.ImagePath;
            model.Tooltip = this.ImageStyle.Tooltip;
            model.ColMdValue = this.ColMdValue;

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivImageTemp" + counter.ToString();
            ImageModel model = this.ChartModel as ImageModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.Images.DivImage.html", templateName);
            return colString;
        }

        public override Dictionary<string, int> AssignUniqueName(Dictionary<string, int> nameChecklist)
        {
            // image note doesn't need a unique name so this should never be called
            throw new NotImplementedException();
        }
    }
}

