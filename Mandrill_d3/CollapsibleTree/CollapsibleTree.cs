using D3jsLib;
using D3jsLib.Utilities;

namespace Mandrill_d3.CollapsibleTree
{
    public class CollapsibleTreeStyle : ChartStyle
    {
    }

    public class CollapsibleTreeData : ChartData
    {
    }

    public sealed class CollapsibleTree : Chart
    {
        public CollapsibleTree(ChartData data, ChartStyle style)
        {
            Data = data;
            Style = style;
        }

        public override string EvaluateModelTemplate(int counter)
        {
            return ChartsUtilities.EvaluateTemplate(this, "Mandrill_d3.CollapsibleTree.CollapsibleTree.html",
                $"colDivTempArea{counter}");
        }
    }
}
