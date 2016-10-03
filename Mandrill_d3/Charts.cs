using System;
using System.Collections.Generic;
using System.Text;
using D3jsLib.Utilities;
using System.IO;
using System.Reflection;

namespace D3jsLib
{
    public static class Charts
    {
        private static string CreateResourcePath(string relativePath)
        {
            // charts is a list of all charts
            // each chart has an md value and row id assigned
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase); // this is bin folder
            string localAssemblyFolder = new Uri(assemblyFolder).LocalPath;
            string mandrillPath = localAssemblyFolder.Remove(localAssemblyFolder.Length - 3);

            string cssFileName = Path.Combine(mandrillPath, relativePath);
            Uri uri = new Uri(cssFileName);
            string absoluteResourcePath = Uri.UnescapeDataString(uri.AbsoluteUri); // must remove %20 space encoding

            return absoluteResourcePath;
        }

        public static string CompileHtmlString(List<object> charts)
        {
            StringBuilder b = new StringBuilder();
            b.AppendLine("<!DOCTYPE html>");
            b.AppendLine("<head>");
            b.AppendLine("<meta content=\"utf-8\">");

            // handle resource file imports
            string demoCssPath = CreateResourcePath(@"extra\gridster\demo.css");
            string gridsterCssPath = CreateResourcePath(@"extra\gridster\jquery.gridster.min.css");
            string d3Path = CreateResourcePath(@"extra\d3\d3.v3.min.js");
            string jqueryPath = CreateResourcePath(@"extra\gridster\jquery.min.js");
            string jqueryGridsterPath = CreateResourcePath(@"extra\gridster\jquery.gridster.min.js");
            

            b.AppendLine("<link rel=\"stylesheet\" href=\"" + demoCssPath + "\">");
            b.AppendLine("<link rel=\"stylesheet\" href=\"" + gridsterCssPath + "\">");
            b.AppendLine("<script type=\"text/javascript\" src=\"" + d3Path + "\"></script>");
            b.AppendLine("<script type=\"text/javascript\" src=\"" + jqueryPath + "\"></script>");
            b.AppendLine("<script type=\"text/javascript\" src=\"" + jqueryGridsterPath + "\" type=\"text/javascript\" charset=\"utf-8\"></script>");

            // handle CSS style
            //b.AppendLine("<style>");
            //b.AppendLine(ChartsUtilities.StreamEmbeddedResource("Mandrill_d3.Gridster.main.css"));
            //b.AppendLine("</style>");
            b.AppendLine("</head>");
            b.AppendLine("<body>");

            // add gridster definition
            b.AppendLine(ChartsUtilities.StreamEmbeddedResource("Mandrill_d3.Gridster.gridster.html"));
            b.AppendLine("<div class=\"gridster\">");
            b.AppendLine("<ul style = \"height: 1000px; width: 1000px; position: absolute;\">");

            int counter = 0;
            foreach (object o in charts)
            {
                Chart chart = o as Chart;

                // create chart model
                chart.CreateChartModel(counter);

                // get chart div for gridster
                string divString = chart.EvaluateDivTemplate(counter);
                b.AppendLine(divString);
                counter += 1;
            }

            b.AppendLine("</ul>");
            b.AppendLine("</div>");

            counter = 0;
            foreach (object o in charts)
            {
                Chart chart = o as Chart;

                // get chart js code
                string chartCode = chart.EvaluateModelTemplate(counter);
                b.AppendLine(chartCode);
                counter += 1;
            }

            b.AppendLine("</body>");
            b.AppendLine("</html>");

            return b.ToString();
        }
    }
}
