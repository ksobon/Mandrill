using D3jsLib.Utilities;

namespace D3jsLib
{
    public class ImageStyle : ChartStyle
    {
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

        public Image(string imagePath, ImageStyle imageStyle)
        {
            this.Style = imageStyle;
            this.ImagePath = imagePath;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivImageTemp" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.Images.DivImage.html", templateName);
            return colString;
        }
    }
}

