namespace D3jsLib
{
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
    ///     Shared class defining Domain for all Charts.
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
        public virtual string ColMdValue { get; set; }
        public virtual int RowNumber { get; set; }
        public virtual ChartModel ChartModel { get; set; }

        public abstract void CreateChartModel(int counter);
        public abstract string EvaluateModelTemplate(int counter);
        public abstract string EvaluateDivTemplate(int counter);
    }

    /// <summary>
    ///     Base class for all chart models.
    /// </summary>
    public abstract class ChartModel
    {
        public virtual string ColMdValue { get; set; }
        public virtual string Width { get; set; }
        public virtual string Height { get; set; }
        public virtual string DivName { get; set; }
        public virtual string DivId { get; set; }
        public virtual string GridRow { get; set; }
        public virtual string GridColumn { get; set; }
        public virtual string SizeX { get; set; }
        public virtual string SizeY { get; set; }
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
    }
}
