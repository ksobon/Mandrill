using System;
using System.Collections.Generic;
using System.Reflection;
using RazorEngine.Templating;
using RazorEngine;

namespace D3jsLib.Utilities
{
    public static class ChartsUtilities
    {
        /// <summary>
        ///     Zips four lists together.
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
        ///     Enumerates embedded reasource so that it can be read line by line.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<string> EnumerateEmbeddedResource(string fileName)
        {
            List<string> result = new List<string>();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = fileName;

            using (System.IO.Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                string s = String.Empty;
                while ((s = reader.ReadLine()) != null)
                {
                    result.Add(s);
                }
            }
            return result;
        }

        /// <summary>
        ///     Color to Hex string method.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string ColorToHexString(System.Windows.Media.Color col)
        {
            return "#" + col.R.ToString("X2") + col.G.ToString("X2") + col.B.ToString("X2");
        }

        /// <summary>
        ///     Return string stream of an embedded resource.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string StreamEmbeddedResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = fileName;

            using (System.IO.Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }

        /// <summary>
        ///     Generic Class for evaluating Razor Model templates.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="cssResourceName"></param>
        /// <param name="tempKey"></param>
        /// <returns></returns>
        public static string EvaluateTemplate<T>(T model, string cssResourceName, string tempKey)
        {
            string template = StreamEmbeddedResource(cssResourceName);
            string result = Engine.Razor.RunCompile(template, tempKey, typeof(T), model);
            return result;
        }
    }
}
