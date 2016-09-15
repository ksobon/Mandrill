using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using Dynamo.Wpf;
using System;

namespace CoreNodeModels
{
    [NodeName("Create Row Container")]
    [NodeDescription("Some Description")]
    [NodeCategory("Archi-lab_Mandrill.Report.Window")]
    [IsDesignScriptCompatible]
    public class MandrillRowContainerNode : VariableInputNode
    {
        public MandrillRowContainerNode()
        {
            InPortData.Add(new PortData("Chart0", "InTooltip0"));
            OutPortData.Add(new PortData("Container", "OutToolTip0"));

            RegisterAllPorts();

            ArgumentLacing = LacingStrategy.Disabled;
        }

        protected override string GetInputName(int index)
        {
            return "Chart" + index;
        }

        protected override string GetInputTooltip(int index)
        {
            return "InToolTip" + index;
        }

        protected override void RemoveInput()
        {
            if (InPortData.Count > 1)
                base.RemoveInput();
        }

        public override bool IsConvertible
        {
            get { return true; }
        }

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

            AssociativeNode functionCall =
                AstFactory.BuildFunctionCall(
                    new Func<List<object>, D3jsLib.RowContainer>(MandrillTypes.Utilities.CreateRowContainer),
                    new List<AssociativeNode>() { listNode });

            return new[]
            {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall),
            };
        }
    }

    public class MandrillRowContainerViewCustomization : VariableInputNodeViewCustomization, INodeViewCustomization<MandrillRowContainerNode>
    {
        public void CustomizeView(MandrillRowContainerNode model, Dynamo.Controls.NodeView nodeView)
        {
            base.CustomizeView(model, nodeView);
        }
    }


    [NodeName("Create Report")]
    [NodeDescription("Some Description")]
    [NodeCategory("Archi-lab_Mandrill.Report.Window")]
    [IsDesignScriptCompatible]
    public class MandrillReportNode : VariableInputNode
    {
        public MandrillReportNode()
        {
            InPortData.Add(new PortData("Container0", "InTooltip0"));
            OutPortData.Add(new PortData("Report", "OutToolTip0"));

            RegisterAllPorts();

            ArgumentLacing = LacingStrategy.Disabled;
        }

        protected override string GetInputName(int index)
        {
            return "Container" + index;
        }

        protected override string GetInputTooltip(int index)
        {
            return "InToolTip" + index;
        }

        protected override void RemoveInput()
        {
            if (InPortData.Count > 1)
                base.RemoveInput();
        }

        public override bool IsConvertible
        {
            get { return true; }
        }

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

            AssociativeNode functionCall =
                AstFactory.BuildFunctionCall(
                    new Func<List<object>, D3jsLib.Report>(MandrillTypes.Utilities.CreateReport),
                    new List<AssociativeNode>() { listNode });

            return new[]
            {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall),
            };
        }
    }

    public class MandrillReportViewCustomization : VariableInputNodeViewCustomization, INodeViewCustomization<MandrillReportNode>
    {
        public void CustomizeView(MandrillReportNode model, Dynamo.Controls.NodeView nodeView)
        {
            base.CustomizeView(model, nodeView);
        }
    }
}
