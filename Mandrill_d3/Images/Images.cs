using D3jsLib.Utilities;

namespace D3jsLib
{
    public class ImageStyle : ChartStyle
    {
        public string Tooltip { get; set; }
    }

    public class ImageModel : ChartModel
    {
        public string ImagePath { get; set; }
        public string Tooltip { get; set; }
    }

    public class Image : Chart
    {
        private string _imagePath;
        public string ImagePath
        {
            get
            {
                return this._imagePath;
            }
            set
            {
                this._imagePath = ChartsUtilities.CreateResourcePath(value);
            }
        }
        public ImageStyle ImageStyle { get; set; }

        public Image(string imagePath, ImageStyle imageStyle)
        {
            this.ImagePath = imagePath;
            this.ImageStyle = imageStyle;
        }

        public override void CreateChartModel(int counter)
        {
            ImageModel model = new ImageModel();
            model.Width = this.ImageStyle.Width;
            model.Height = this.ImageStyle.Height;
            model.ImagePath = this.ImagePath;
            model.Tooltip = this.ImageStyle.Tooltip;
            model.DivId = "div" + counter.ToString();

            // set grid address
            model.GridRow = this.ImageStyle.GridRow;
            model.GridColumn = this.ImageStyle.GridColumn;

            // always round up for the grid size so chart is smaller then container
            model.SizeX = (int)System.Math.Ceiling(this.ImageStyle.Width / 100d);
            model.SizeY = (int)System.Math.Ceiling(this.ImageStyle.Height / 100d);

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivImageTemp" + counter.ToString();
            ImageModel model = this.ChartModel as ImageModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.Images.DivImage.html", templateName);
            return colString;
        }
    }
}

