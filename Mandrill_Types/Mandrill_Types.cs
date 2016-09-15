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
        ///     Handle creation of a RowContainer from multiple Chart inputs.
        /// </summary>
        /// <param name="chartObjects"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static D3jsLib.RowContainer CreateRowContainer(List<object> chartObjects)
        {
            D3jsLib.RowContainer rc = new D3jsLib.RowContainer(chartObjects);
            rc.AssignColMdValue();
            return rc;
        }

        /// <summary>
        ///     Handles creation of a Report from multiple Container inputs.
        /// </summary>
        /// <param name="containers"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static D3jsLib.Report CreateReport(List<object> containers)
        {
            List<object> processedCharts = D3jsLib.Charts.ProcessCharts(containers);
            string finalHtmlString = D3jsLib.Charts.CompileHtmlString(processedCharts);
            return new D3jsLib.Report(finalHtmlString);
        }
    }
}
