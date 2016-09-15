using D3jsLib;

namespace Image
{
    /// <summary>
    ///     Image Class.
    /// </summary>
    public class Image
    {

        internal Image()
        {
        }

        /// <summary>
        ///     Image class.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="Style"></param>
        /// <returns name="Image"></returns>
        /// <search></search>
        public static D3jsLib.Image Create(string FilePath, ImageStyle Style)
        {
            D3jsLib.Image image = new D3jsLib.Image(FilePath, Style);
            return image;
        }

        /// <summary>
        ///     Image Style object 
        /// </summary>
        /// <param name="Width">Width of the image.</param>
        /// <param name="Height">Height of the image.</param>
        /// <param name="Tooltip">Tooltip message that will appear when you hover over image.</param>
        /// <returns name="Style"></returns>
        /// <search></search>
        public static ImageStyle Style(
            double Width = 150,
            double Height = 150,
            string Tooltip = "")
        {
            ImageStyle style = new ImageStyle();
            style.Width = Width;
            style.Height = Height;
            style.Tooltip = Tooltip;

            return style;
        }
    }
}
