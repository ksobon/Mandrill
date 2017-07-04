using System;
using System.Collections.Generic;
using System.Reflection;
using RazorEngine.Templating;
using RazorEngine;
using System.IO;
using System.Drawing;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using System.Web.Script.Serialization;
using RazorEngine.Compilation;
using RazorEngine.Compilation.ReferenceResolver;
using RazorEngine.Configuration;

namespace D3jsLib.Utilities
{
    public static class ChartsUtilities
    {
        /// <summary>
        /// Serialize list of data point 2's into a data string.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DataToJsonString(List<DataPoint2> data)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (var dp in data)
            {
                list.Add(dp.ToDictionary());
            }

            // serialize C# Array into JS Array
            var jsData = new JavaScriptSerializer().Serialize(list);

            return jsData;
        }

        /// <summary>
        /// Create List of DataPoint2 from List of values and headers.
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static List<DataPoint2> Data2FromList(List<string> headers, List<List<object>> values)
        {
            var dataPoints = new List<DataPoint2>();
            foreach (var subList in values)
            {
                var dataPoint = new DataPoint2 {Name = subList[0].ToString()};
                var dict = new Dictionary<string, double>();
                for (var i = 1; i < subList.Count; i++)
                {
                    dict.Add(headers[i], Convert.ToDouble(subList[i]));
                }
                dataPoint.Values = dict;
                dataPoints.Add(dataPoint);
            }
            return dataPoints;
        }

        /// <summary>
        /// Create a list of data point 2s from a CSV path.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<DataPoint2> Data2FromCsv(string filePath)
        {
            var dataPoints = new List<DataPoint2>();
            using (var csv = new CsvReader(new StreamReader(filePath), true))
            {
                var fieldCount = csv.FieldCount;
                var headers = csv.GetFieldHeaders();

                while (csv.ReadNextRecord())
                {
                    var dataPoint = new DataPoint2 {Name = csv[0]};

                    var dict = new Dictionary<string, double>();
                    for (var i = 1; i < fieldCount; i++)
                    {
                        try
                        {
                            dict.Add(headers[i], Convert.ToDouble(csv[i]));
                        }
                        catch
                        {
                            dict.Add(headers[i], 0);
                        }
                    }
                    dataPoint.Values = dict;
                    dataPoints.Add(dataPoint);
                }
            }
            return dataPoints;
        }

        /// <summary>
        /// Create list of DataPoint1 objects.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<DataPoint1> Data1FromCsv(string filePath)
        {
            var dataPoints = new List<DataPoint1>();
            using (var csv = new CsvReader(new StreamReader(filePath), true))
            {
                var fieldCount = csv.FieldCount;
                csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    switch (fieldCount)
                    {
                        case 2:
                            dataPoints.Add(new DataPoint1 { name = csv[0], value = Convert.ToDouble(csv[1]) });
                            break;
                        case 3:
                            dataPoints.Add(new DataPoint1 { name = csv[0], value = Convert.ToDouble(csv[1]), color = Convert.ToInt32(csv[2]) });
                            break;
                        default:
                            dataPoints.Add(new DataPoint1 { name = csv[0], value = Convert.ToDouble(csv[1]) });
                            break;
                    }
                }
            }
            return dataPoints;
        }

        /// <summary>
        /// Method for converting local path to HTML compatible file:/// path.
        /// </summary>
        /// <param name="filePath"></param>
        public static string CreateResourcePath(string filePath)
        {
            var uri = new Uri(filePath);
            var resourcePath = Uri.UnescapeDataString(uri.AbsoluteUri); // must remove %20 space encoding

            return resourcePath;
        }

        /// <summary>
        /// Since *dep file cannot be stored in bin folder it needs to be copied into it.
        /// </summary>
        public static void MoveDepFile()
        {
            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            if (assemblyFolder == null) return;

            var localAssemblyFolder = new Uri(assemblyFolder).LocalPath;
            var mandrillPath = localAssemblyFolder.Remove(localAssemblyFolder.Length - 3);
            var sourcePath = Path.Combine(mandrillPath, @"extra\Select.Html.dep");
            var targetPath = Path.Combine(mandrillPath, @"bin\Select.Html.dep");
            if (File.Exists(sourcePath) && !File.Exists(targetPath))
            {
                File.Move(sourcePath, targetPath);
            }
        }

        /// <summary>
        /// Zips five lists together.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="second"></param>
        /// <param name="third"></param>
        /// <param name="fourth"></param>
        /// <param name="fifth"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> ZipFive<T1, T2, T3, T4, T5, TResult>(
            this IEnumerable<T1> source,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            IEnumerable<T4> fourth,
            IEnumerable<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> func)
        {
            using (var e1 = source.GetEnumerator())
            using (var e2 = second.GetEnumerator())
            using (var e3 = third.GetEnumerator())
            using (var e4 = fourth.GetEnumerator())
            using (var e5 = fifth.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e5.MoveNext())
                    yield return func(e1.Current, e2.Current, e3.Current, e4.Current, e5.Current);
            }
        }

        /// <summary>
        /// Zips four lists together.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="second"></param>
        /// <param name="third"></param>
        /// <param name="fourth"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> ZipFour<T1, T2, T3, T4, TResult>(
            this IEnumerable<T1> source,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            IEnumerable<T4> fourth,
            Func<T1, T2, T3, T4, TResult> func)
        {
            using (var e1 = source.GetEnumerator())
            using (var e2 = second.GetEnumerator())
            using (var e3 = third.GetEnumerator())
            using (var e4 = fourth.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext())
                    yield return func(e1.Current, e2.Current, e3.Current, e4.Current);
            }
        }

        /// <summary>
        /// Zip three lists together.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="second"></param>
        /// <param name="third"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> ZipThree<T1, T2, T3, TResult>(
            this IEnumerable<T1> source,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            Func<T1, T2, T3, TResult> func)
        {
            using (var e1 = source.GetEnumerator())
            using (var e2 = second.GetEnumerator())
            using (var e3 = third.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
                    yield return func(e1.Current, e2.Current, e3.Current);
            }
        }

        /// <summary>
        /// Enumerates embedded reasource so that it can be read line by line.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<string> EnumerateEmbeddedResource(string fileName)
        {
            var result = new List<string>();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = fileName;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return result;

                using (var reader = new StreamReader(stream))
                {
                    string s;
                    while ((s = reader.ReadLine()) != null)
                    {
                        result.Add(s);
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Color to Hex string method.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string ColorToHexString(Color col)
        {
            return "#" + col.R.ToString("X2") + col.G.ToString("X2") + col.B.ToString("X2");
        }

        /// <summary>
        /// Return string stream of an embedded resource.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string StreamEmbeddedResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = fileName;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return string.Empty;

                using (var reader = new StreamReader(stream))
                {
                    var result = reader.ReadToEnd();
                    return result;
                }
            }
        }

        /// <summary>
        /// Generic Class for evaluating Razor Model templates.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="cssResourceName"></param>
        /// <param name="tempKey"></param>
        /// <returns></returns>
        public static string EvaluateTemplate<T>(T model, string cssResourceName, string tempKey)
        {
            var config = new TemplateServiceConfiguration
            {
                Debug = true,
                ReferenceResolver = new SystemRuntimeResolver()
            };
            Engine.Razor = RazorEngineService.Create(config);
            var template = StreamEmbeddedResource(cssResourceName);
            var result = Engine.Razor.RunCompile(template, tempKey, typeof(T), model);
            return result;
        }

        /// <summary>
        /// On Windows 10 it might be missing certain Assemblies and causes issues while compiling RazorEngine template. 
        /// It can be manually added using this method: https://github.com/Antaris/RazorEngine/issues/416
        /// </summary>
        private class SystemRuntimeResolver : IReferenceResolver
        {
            private static readonly IReferenceResolver Resolver = new UseCurrentAssembliesReferenceResolver();
            private static readonly Assembly SystemRuntime = Assembly.Load("System.Runtime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

            public IEnumerable<CompilerReference> GetReferences(TypeContext context, IEnumerable<CompilerReference> includeAssemblies = null)
            {
                var newReference = new[] { CompilerReference.From(SystemRuntime), };

                var assemblies = includeAssemblies?.Concat(newReference) ?? newReference;

                return Resolver.GetReferences(context, assemblies);
            }
        }
    }
}
