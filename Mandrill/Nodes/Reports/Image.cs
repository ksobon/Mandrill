using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report
{
    /// <summary>
    ///     Image based report.
    /// </summary>
    public class Image
    {
        internal Image()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="report"></param>
        /// <param name="filePath"></param>
        public static void SaveAsPNG(D3jsLib.Report report, string filePath)
        {
            SelectPdf.HtmlToImage converter = new SelectPdf.HtmlToImage();

            // set converter options
            System.Drawing.Image image = converter.ConvertHtmlString(report.HtmlString);
        }
    }
}
