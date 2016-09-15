using D3jsLib.Utilities;
using System.Windows.Media;
using System;
using System.Collections.Generic;

namespace D3jsLib
{
    public class TextStyle
    {
        public double FontSize { get; set; }
        public Color FontColor { get; set; }
        public string FontWeight { get; set; }
        public string FontStyle { get; set; }
        public string FontAlign { get; set; }
        public string FontTransform { get; set; }
    }

    public class TextNoteModel : ChartModel
    {
        public string Text { get; set; }
        public string FontSize { get; set; }
        public string FontColor { get; set; }
        public string FontWeight { get; set; }
        public string FontStyle { get; set; }
        public string FontAlign { get; set; }
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

        public override void CreateChartModel()
        {
            TextNoteModel model = new TextNoteModel();
            model.Text = this.Text;
            model.ColMdValue = this.ColMdValue;
            model.FontSize = this.TextStyle.FontSize.ToString();
            model.FontColor = ChartsUtilities.ColorToHexString(this.TextStyle.FontColor);
            model.FontWeight = this.TextStyle.FontWeight;
            model.FontStyle = this.TextStyle.FontStyle;
            model.FontAlign = this.TextStyle.FontAlign;
            model.FontTransform = this.TextStyle.FontTransform;

            this.ChartModel = model;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            string templateName = "colDivTextTemp" + counter.ToString();
            TextNoteModel model = this.ChartModel as TextNoteModel;
            string colString = ChartsUtilities.EvaluateTemplate(model, "Mandrill_d3.TextNotes.DivTextNote.html", templateName);
            return colString;
        }

        public override Dictionary<string, int> AssignUniqueName(Dictionary<string, int> nameChecklist)
        {
            // text note doesn't need a unique name so this should never be called
            throw new NotImplementedException();
        }
    }
}
