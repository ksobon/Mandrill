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
            model.Width = this.ImageStyle.Width.ToString();
            model.Height = this.ImageStyle.Height.ToString();
            model.ImagePath = this.ImagePath;
            model.Tooltip = this.ImageStyle.Tooltip;
            model.DivId = "div" + counter.ToString();

            // set grid address
            model.GridRow = this.ImageStyle.GridRow.ToString();
            model.GridColumn = this.ImageStyle.GridColumn.ToString();

            // always round up for the grid size so chart is smaller then container
            model.SizeX = System.Math.Ceiling(this.ImageStyle.Width / 100d).ToString();
            model.SizeY = System.Math.Ceiling(this.ImageStyle.Height / 100d).ToString();

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivImageTemp" + counter.ToString();
            ImageModel model = this.ChartModel as ImageModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.Images.DivImage.html", templateName);
            return colString;
        }

        public override string EvaluateDivTemplate(int counter)
        {
            string templateName = "divTempImage" + counter.ToString();
            ImageModel model = this.ChartModel as ImageModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.Gridster.divTemplate.html", templateName);
            return colString;
        }
    }
}

