using Autodesk.DesignScript.Runtime;
using System.Collections.Generic;

namespace MandrillTypes
{
    /// <summary>
    ///     Utility methods for UI nodes
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        ///     Static method for report generation.
        /// </summary>
        /// <param name="chartObjects"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static D3jsLib.Report CreateGridsterReport(List<object> chartObjects)
        {
            string finalHtmlString = D3jsLib.Charts.CompileHtmlString(chartObjects, true);
            return new D3jsLib.Report(finalHtmlString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static bool BuildBool(bool value)
        {
            return value;
        }
    }
}
