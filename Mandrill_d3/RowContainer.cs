using System;
using System.Collections.Generic;

namespace D3jsLib
{
    public class RowContainer
    {
        public List<object> Contents;

        public RowContainer(List<object> contents)
        {
            this.Contents = contents;
        }

        public static RowContainer byList(List<object> args)
        {
            return new RowContainer(args);
        }

        public RowContainer AssignColMdValue()
        {
            // define md-x value
            string mdValue = "";
            if (12 % this.Contents.Count != 0)
            {
                // process divs with remainder
                double value = 12 / this.Contents.Count;
                mdValue = Math.Floor(value).ToString();
                // ignore the remainder at the moment
            }
            else
            {
                //process evenly spaced divs
                mdValue = (12 / this.Contents.Count).ToString();
            }

            foreach (object o in this.Contents)
            {
                // all objects inherit colMdValue from Chart base class
                // its safe here to cast to Chart
                Chart chart = o as Chart;
                chart.ColMdValue = mdValue;
            }
            return this;
        }
    }
}
