using D3jsLib.Utilities;
using System.Drawing;

namespace D3jsLib
{
    public class TextStyle : ChartStyle
    {
        public double FontSize { get; set; }
        public Color FontColor { get; set; }
        public string FontWeight { get; set; }
        public string FontStyle { get; set; }
        public string FontTransform { get; set; }
    }

    public class TextNoteModel : ChartModel
    {
        public string Text { get; set; }
        public string FontSize { get; set; }
        public string FontColor { get; set; }
        public string FontWeight { get; set; }
        public string FontStyle { get; set; }
        public string FontTransform { get; set; }
    }

    public class TextNote : Chart
    {
        public string Text { get; set; }
        public TextStyle TextStyle { get; set; }

        public TextNote(string text, TextStyle textStyle)
        {
            this.Text = text;
            this.TextStyle = textStyle;
        }

        public override void CreateChartModel(int counter)
        {
            TextNoteModel model = new TextNoteModel();
            model.Text = this.Text;
            model.FontSize = this.TextStyle.FontSize.ToString();
            model.FontColor = ChartsUtilities.ColorToHexString(this.TextStyle.FontColor);
            model.FontWeight = this.TextStyle.FontWeight;
            model.FontStyle = this.TextStyle.FontStyle;
            model.FontTransform = this.TextStyle.FontTransform;
            model.DivId = "div" + counter.ToString();

            // set grid address
            model.GridRow = this.TextStyle.GridRow;
            model.GridColumn = this.TextStyle.GridColumn;

            // always round up for the grid size so chart is smaller then container
            model.SizeX = (int)System.Math.Ceiling(this.TextStyle.Width / 100d);
            model.SizeY = (int)System.Math.Ceiling(this.TextStyle.Height / 100d);

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTextTemp" + counter.ToString();
            TextNoteModel model = this.ChartModel as TextNoteModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.TextNotes.DivTextNote.html", templateName);
            return colString;
        }
    }
}
