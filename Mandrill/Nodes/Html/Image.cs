using Autodesk.DesignScript.Runtime;
using D3jsLib;
using System.IO;

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
        /// <param name="FilePath">Path to the image.</param>
        /// <param name="Style">Style</param>
        /// <returns name="Image">Image object</returns>
        /// <search>image</search>
        public static D3jsLib.Image Create(object FilePath, ImageStyle Style)
        {
            // get full path to file as string
            // if File.FromPath is used it returns FileInfo class
            string _filePath = "";
            try
            {
                _filePath = (string)FilePath;
            }
            catch
            {
                _filePath = ((FileInfo)FilePath).FullName;
            }

            D3jsLib.Image image = new D3jsLib.Image(_filePath, Style);
            return image;
        }

        /// <summary>
        ///     Image Style object 
        /// </summary>
        /// <param name="Address">Grid Coordinates.</param>
        /// <param name="Width">Width of the image.</param>
        /// <param name="Height">Height of the image.</param>
        /// <param name="Tooltip">Tooltip message that will appear when you hover over image.</param>
        /// <returns name="Style">Style object.</returns>
        /// <search>image style, style</search>
        public static ImageStyle Style(
            [DefaultArgument("Charts.MiscNodes.GetNull()")] GridAddress Address,
            int Width = 150,
            int Height = 150,
            string Tooltip = "")
        {
            ImageStyle style = new ImageStyle();
            style.Width = Width;
            style.Height = Height;
            style.Tooltip = Tooltip;
            style.SizeX = (int)System.Math.Ceiling(Width / 100d);
            style.SizeY = (int)System.Math.Ceiling(Height / 100d);

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
