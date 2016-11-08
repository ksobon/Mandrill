using DSCore;
using sColor = System.Drawing.Color;
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
        ///     Create Text Note
        /// </summary>
        /// <param name="Text">String to be displayed.</param>
        /// <param name="Style">Style object.</param>
        /// <returns name="TextNote">Text node object</returns>
        public static TextNote Create(string Text, TextStyle Style)
        {
            TextNote tn = new D3jsLib.TextNote(Text, Style);
            return tn;
        }

        /// <summary>
        ///     Create Text Style
        /// </summary>
        /// <param name="FontColor">Color</param>
        /// <param name="Address">Grid Coordinates</param>
        /// <param name="Width">Width of Grid container.</param>
        /// <param name="Height">Height of Grid Container.</param>
        /// <param name="FontSize">Size</param>
        /// <param name="FontWeight">Weight</param>
        /// <param name="FontStyle">Style</param>
        /// <param name="FontTransform">Transform</param>
        /// <returns name="Style">Style for the Text Note object</returns>
        public static TextStyle Style(
            [DefaultArgument("DSCore.Color.ByARGB(1,0,0,0)")] Color FontColor,
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            int Width = 200,
            int Height = 100,
            double FontSize = 20.0,
            string FontWeight = "normal",
            string FontStyle = "normal",
            string FontTransform = "none")
        {
            TextStyle style = new TextStyle();
            style.FontSize = FontSize;
            style.FontColor = sColor.FromArgb(FontColor.Alpha, FontColor.Red, FontColor.Green, FontColor.Blue);
            style.FontWeight = FontWeight;
            style.FontStyle = FontStyle;
            style.FontTransform = FontTransform;
            style.Width = Width;
            style.Height = Height;

            if (Address != null)
            {
                style.GridRow = Address.X;
                style.GridColumn = Address.Y;
            }
            else
            {
                style.GridRow = 1;
                style.GridColumn = 1;
            }

            return style;
        }
    }
}
