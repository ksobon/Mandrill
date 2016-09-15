using System.Collections.Generic;

namespace D3jsLib
{
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

        public abstract void CreateChartModel();
        public abstract string EvaluateModelTemplate(int counter);
        public abstract Dictionary<string, int> AssignUniqueName(Dictionary<string, int> nameChecklist);
    }

    public abstract class ChartModel
    {
        public virtual string ColMdValue { get; set; }
        public virtual string Width { get; set; }
        public virtual string Height { get; set; }
        public virtual string DivName { get; set; }
        public virtual string DivId { get; set; }
    } 
}
