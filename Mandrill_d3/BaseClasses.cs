using D3jsLib.Utilities;
using System.Collections.Generic;

namespace D3jsLib
{
    public class Margins
    {
        private int _top;
        private int _bottom;
        private int _left;
        private int _right;

        public int Top
        {
            get { return this._top; }
            set { this._top = value; }
        }
        public int Bottom
        {
            get { return this._bottom; }
            set { this._bottom = value; }
        }
        public int Left
        {
            get { return this._left; }
            set { this._left = value; }
        }
        public int Right
        {
            get { return this._right; }
            set { this._right = value; }
        }

        public Margins()
        {
            this._top = 20;
            this._bottom = 40;
            this._left = 40;
            this._right = 20;
        }

        public Margins(int top, int bottom, int left, int right)
        {
            this._top = top;
            this._bottom = bottom;
            this._left = left;
            this._right = right;
        }
    }

    /// <summary>
    ///     Shared class GridAddress
    /// </summary>
    public class GridAddress
    {
        private int _x;
        private int _y;

        public int X
        {
            get { return this._x; }
            set { this._x = value; }
        }
        public int Y
        {
            get { return this._y; }
            set { this._y = value; }
        }

        public GridAddress()
        {
            this._x = 1;
            this._y = 1;
        }

        public GridAddress(int x, int y)
        {
            this._x = x;
            this._y = y;
        }
    }

    /// <summary>
    ///     Shared class Domain
    /// </summary>
    public class Domain
    {
        private double _a;
        private double _b;

        public double A
        {
            get { return this._a; }
            set { this._a = value; }
        }
        public double B
        {
            get { return this._b; }
            set { this._b = value; }
        }

        public Domain()
        {
            this._a = 0;
            this._b = 1;
        }

        public Domain(double a, double b)
        {
            this._a = a;
            this._b = b;
        }
    }

    /// <summary>
    ///     Base class for all Charts, Text Boxes etc.
    /// </summary>
    public abstract class Chart
    {
        public string DivId { get; set; }
        public virtual ChartModel ChartModel { get; set; }
        public virtual ChartStyle Style { get; set; }
        public virtual ChartData Data { get; set; }

        //public abstract void CreateChartModel(int counter);
        public abstract string EvaluateModelTemplate(int counter);

        public string EvaluateDivTemplate(int counter)
        {
            string templateName = "chart" + counter.ToString();
            //var model = this.ChartModel;
            string colString = ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.Gridster.divTemplate.html", templateName);
            return colString;
        }
    }

    /// <summary>
    ///     Base class for all chart models.
    /// </summary>
    public abstract class ChartModel
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public virtual string DivId { get; set; }
        public virtual int GridRow { get; set; }
        public virtual int GridColumn { get; set; }
        public virtual int SizeX { get; set; }
        public virtual int SizeY { get; set; }
        public virtual Margins Margins { get; set; }
    }

    /// <summary>
    ///     Base class for all chart styles.
    /// </summary>
    public abstract class ChartStyle
    {
        public virtual int GridRow { get; set; }
        public virtual int GridColumn { get; set; }
        public virtual int SizeX { get; set; }
        public virtual int SizeY { get; set; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public virtual Margins Margins { get; set; }
    }

    public abstract class ChartData
    {
        public virtual string Data { get; set; }
        public virtual Domain Domain { get; set; }
    }

    public class DataPoint1
    {
        public string name { get; set; }
        public double value { get; set; }
        public int color { get; set; }
    }

    public class DataPoint2
    {
        public string Name { get; set; }
        public Dictionary<string, double> Values { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> output = new Dictionary<string, object>();
            output.Add("Name", this.Name);
            foreach (var value in this.Values)
            {
                output.Add(value.Key, value.Value);
            }

            return output;
        }
    }
}
