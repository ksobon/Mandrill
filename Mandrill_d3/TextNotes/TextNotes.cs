using D3jsLib.Utilities;

namespace D3jsLib
{
    public class TextStyle : ChartStyle
    {
        public double FontSize { get; set; }
        public string FontColor { get; set; }
        public string FontWeight { get; set; }
        public string FontStyle { get; set; }
        public string FontTransform { get; set; }
    }

    public class TextNote : Chart
    {
        public string Text { get; set; }

        public TextNote(string text, TextStyle textStyle)
        {
            this.Text = text;
            this.Style = textStyle;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTextTemp" + counter.ToString();
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.TextNotes.DivTextNote.html", templateName);
            return colString;
        }
    }
}
