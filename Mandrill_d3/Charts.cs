using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3jsLib.d3BarCharts;
using D3jsLib.Utilities;
using System.IO;
using System.Reflection;

namespace D3jsLib
{
    public static class Charts
    {
        public static List<object> ProcessCharts(List<object> containers)
        {
            // incoming is a list of containers
            // first tag each object within same container with a unique RowId
            List<object> allCharts = new List<object>();
            int rowNumber = 1;
            foreach (object o in containers)
            {
                RowContainer rc = (RowContainer)o;
                if (rc != null)
                {
                    foreach (object o2 in rc.Contents)
                    {
                        Chart chart = o2 as Chart;
                        chart.RowNumber = rowNumber;
                        allCharts.Add(chart);
                    }
                }
                rowNumber++;
            }

            Dictionary<string, int> nameChecklist = new Dictionary<string, int>();
            List<object> output = new List<object>();

            foreach(object o in allCharts)
            {
                try
                {
                    var tn = o as TextNote;
                    var img = o as Image;

                    if (tn != null)
                    {
                        // process text note
                        output.Add(tn);
                    }
                    else if (img != null)
                    {
                        // process image
                        output.Add(img);
                    }
                    else
                    {
                        // process all other charts
                        Chart genChart = o as Chart;
                        nameChecklist = genChart.AssignUniqueName(nameChecklist);
                        output.Add(genChart);
                    }
                }
                catch { }
            }
            return output;
        }

        public static string CompileHtmlString(List<object> charts)
        {
            // charts is a list of all charts
            // each chart has an md value and row id assigned
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase); // this is bin folder
            string localAssemblyFolder = new Uri(assemblyFolder).LocalPath;
            string mandrillPath = localAssemblyFolder.Remove(localAssemblyFolder.Length - 3);

            string cssFileName = Path.Combine(mandrillPath, @"extra\bootstrap\css\bootstrap.min.css");
            Uri uri = new Uri(cssFileName);
            string absolutCssFilePath = Uri.UnescapeDataString(uri.AbsoluteUri); // must remove %20 space encoding

            string d3jsFileName = Path.Combine(mandrillPath, @"extra\d3\d3.v3.min.js");
            Uri uri1 = new Uri(d3jsFileName);
            string absoluted3FilePath = Uri.UnescapeDataString(uri1.AbsoluteUri).ToString();

            StringBuilder b = new StringBuilder();
            b.AppendLine("<!DOCTYPE html>");
            b.AppendLine("<head>");
            b.AppendLine("<meta content=\"utf-8\">");
            b.AppendLine("<link rel=\"stylesheet\" href=\"" + absolutCssFilePath + "\">");
            b.AppendLine("<script type=\"text/javascript\" src=\"" + absoluted3FilePath + "\"></script>");
            b.AppendLine("<style>");


            // append css style that is common for all bar charts
            // this gets appended only once
            foreach (object o in charts)
            {
                try
                {
                    var tn = o as TextNote;
                    var img = o as Image;
                    if (tn != null || img != null)
                    {
                        continue;
                    }
                    else
                    {
                        // all charts have the same css style so appending it once works
                        b.AppendLine(ChartsUtilities.StreamEmbeddedResource("Mandrill_d3.BarCharts.BarChart.css"));
                        break;
                    }
                }
                catch (Exception) { }
            }
            b.AppendLine("</style>");
            b.AppendLine("</head>");



            // create div layout (row and columns)
            // this gets appended for each bar chart
            List<DivContent> contents = new List<DivContent>();
            foreach (object o in charts)
            {
                Chart chart = o as Chart;
                DivContent divContent = new DivContent(chart);
                contents.Add(divContent);
            }
            var groupedContents = contents
                 .GroupBy(u => u.RowNumber)
                 .Select(grp => grp.ToList())
                 .ToList();

            // create divs
            string s1 = "<div class=\"row\">";
            string s2 = "</div>";
            foreach (List<DivContent> _list in groupedContents)
            {
                StringBuilder b2 = new StringBuilder();
                b2.AppendLine(s1);

                int counter = 0;
                foreach (DivContent chart in _list)
                {
                    // get chart from source
                    Chart ch = chart.SourceObject as Chart;

                    // create chart model for razor template
                    ch.CreateChartModel();

                    // evaluate chart model
                    string colString = ch.EvaluateModelTemplate(counter);

                    b2.AppendLine(colString);
                    counter++;
                }
                b2.AppendLine(s2);
                b.AppendLine(b2.ToString());
                b2.Clear();
            }

            return b.ToString();
        }
    }
}
