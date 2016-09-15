using DSCore;
using sColor = System.Windows.Media.Color;
using D3jsLib;
using Autodesk.DesignScript.Runtime;

namespace Text
{
    /// <summary>
    ///     Text Box Class.
    /// </summary>
    public class Text
    {

        internal Text()
        {
        }

        /// <summary>
        ///     Text note object.
        /// </summary>
        /// <param name="Text">Input string.</param>
        /// <param name="Style">Input Text Style.</param>
        /// <returns name="Text">Text note object.</returns>
        /// <search>text, note</search>
        public static TextNote Create(string Text, TextStyle Style)
        {
            TextNote tn = new D3jsLib.TextNote(Text, Style);
            return tn;
        }

        /// <summary>
        ///     Text style object.
        /// </summary>
        /// <param name="FontSize">Font size as double.</param>
        /// <param name="FontWeight">Font Weight.</param>
        /// <param name="FontStyle">Font Style.</param>
        /// <param name="FontAlign">Font Align Style.</param>
        /// <param name="FontTransform">Font Transform.</param>
        /// <param name="FontColor">Font color.</param>
        /// <returns name="Style">Text Style.</returns>
        /// <search>text, style</search>
        public static TextStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,0,0,0)")] Color FontColor,
            double FontSize = 20.0,
            string FontWeight = "normal",
            string FontStyle = "normal", 
            string FontAlign = "center",
            string FontTransform = "none")
        {
            TextStyle style = new TextStyle();
            style.FontSize = FontSize;
            style.FontColor = sColor.FromArgb(FontColor.Alpha, FontColor.Red, FontColor.Green, FontColor.Blue);
            style.FontWeight = FontWeight;
            style.FontStyle = FontStyle;
            style.FontAlign = FontAlign;
            style.FontTransform = FontTransform;

            return style;
        }
    }
}
