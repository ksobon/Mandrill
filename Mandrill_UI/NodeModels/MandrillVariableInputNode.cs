using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using Dynamo.Wpf;
using System;
using Newtonsoft.Json;

namespace Mandrill.ChromeWindow
{
    /// <summary>
    /// Report
    /// </summary>
    [NodeName("Create Report")]
    [NodeDescription("Creates Mandrill Report object that can be viewed in a Window or Printed to PDF.")]
    [NodeCategory("Archi-lab_Mandrill.Report.Window")]
    [IsDesignScriptCompatible]
    public class MandrillReportNode : VariableInputNode
    {
        /// <summary>
        /// Report Node
        /// </summary>
        public MandrillReportNode()
        {
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("Chart0", "MandrillChart0.")));
            OutPorts.Add(new PortModel(PortType.Output, this, new PortData("Report", "OutToolTip0.")));
            RegisterAllPorts();
            ArgumentLacing = LacingStrategy.Disabled;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inPorts"></param>
        /// <param name="outPorts"></param>
        [JsonConstructor]
        protected MandrillReportNode(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts) { }

        /// <summary>
        /// Get input name
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override string GetInputName(int index)
        {
            return "Chart" + index;
        }

        /// <summary>
        /// Get input tooltip
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override string GetInputTooltip(int index)
        {
            return "MandrillChart" + index;
        }

        /// <summary>
        /// Remove input
        /// </summary>
        protected override void RemoveInput()
        {
            if (InPorts.Count > 1) base.RemoveInput();
        }

        /// <summary>
        /// Base implementation
        /// </summary>
        public override bool IsConvertible
        {
            get { return true; }
        }

        /// <summary>
        /// AST Implementation
        /// </summary>
        /// <param name="inputAstNodes"></param>
        /// <returns></returns>
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (IsPartiallyApplied)
            {
                return new[]
                {
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode())
                };
            }

            AssociativeNode listNode = AstFactory.BuildExprList(inputAstNodes);

            var functionCall =
                AstFactory.BuildFunctionCall(
                    new Func<List<object>, D3jsLib.Report>(MandrillTypes.Utilities.CreateGridsterReport),
                    new List<AssociativeNode> { listNode });

            return new[]
            {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall),
            };
        }
    }

    /// <summary>
    /// View Customization
    /// </summary>
    public class MandrillReportViewCustomization : VariableInputNodeViewCustomization, INodeViewCustomization<MandrillReportNode>
    {
        /// <summary>
        /// Customize view
        /// </summary>
        /// <param name="model"></param>
        /// <param name="nodeView"></param>
        public void CustomizeView(MandrillReportNode model, Dynamo.Controls.NodeView nodeView)
        {
            base.CustomizeView(model, nodeView);
        }
    }
}
