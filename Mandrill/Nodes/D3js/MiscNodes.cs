using Autodesk.DesignScript.Runtime;
using D3jsLib;

namespace Charts
{
    /// <summary>
    ///     Nodes shared across all chart types.
    /// </summary>
    public class MiscNodes
    {
        private MiscNodes()
        { }

        /// <summary>
        ///     Custom Area Chart domain.
        /// </summary>
        /// <param name="A">Domain start.</param>
        /// <param name="B">Domain end.</param>
        /// <returns name="Domain">Area Chart Domain.</returns>
        /// <search>area, chart, domain</search>
        public static Domain Domain(double A = 0.0, double B = 1.0)
        {
            Domain d = new Domain(A, B);
            return d;
        }

        /// <summary>
        ///     Returns null value.
        /// </summary>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static object GetNull()
        {
            return null;
        }
    }
}
