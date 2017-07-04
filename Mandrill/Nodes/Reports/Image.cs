//using System.IO;
//using System.Windows.Media.Imaging;

//namespace Report
//{
//    /// <summary>
//    ///     Image based report.
//    /// </summary>
//    public class Image
//    {
//        internal Image()
//        {
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="report"></param>
//        /// <param name="filePath"></param>
//        public static void SaveAsPNG(D3jsLib.Report report, string filePath)
//        {
//            string html = report.HtmlString;

//            //string html = "<html><body><p>This is a shitty html code</p>"
//            //+ "<p>This is another html line</p></body>";

//            System.Drawing.Image image = TheArtOfDev.HtmlRenderer.WinForms.HtmlRender.RenderToImageGdiPlus(html, new System.Drawing.Size(1800, 600));
//            image.Save(filePath);
//            //BitmapFrame image = TheArtOfDev.HtmlRenderer.WPF.HtmlRender.RenderToImage(html, new System.Windows.Size(1800, 600));

//            //using (FileStream stream = new FileStream(filePath, FileMode.Create))
//            //{
//            //    BitmapEncoder encoder = new PngBitmapEncoder();
//            //    encoder.Frames.Add(BitmapFrame.Create(image));
//            //    encoder.Save(stream);
//            //} 
//        }
//    }
//}
