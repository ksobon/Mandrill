using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using Dynamo.Wpf;
using System;

namespace CoreNodeModels
{
    /// <summary>
    ///     Row Container
    /// </summary>
    [NodeName("Create Row Container")]
    [NodeDescription("Some Description")]
    [NodeCategory("Archi-lab_Mandrill.Report.Window")]
    [IsDesignScriptCompatible]
    [InPortTypes("Chart")]
    [InPortNames("Chart0")]
    [InPortDescriptions("Chart0")]
    [OutPortTypes("RowContainer")]
    [OutPortNames("Container")]
    [OutPortDescriptions("RowContainer0")]
    public class MandrillRowContainerNode : VariableInputNode
    {
        /// <summary>
        ///     Row Container Node
        /// </summary>
        public MandrillRowContainerNode()
        {
            RegisterAllPorts();
            ArgumentLacing = LacingStrategy.Disabled;
        }

        /// <summary>
        ///     Get Input name method.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override string GetInputName(int index)
        {
            return "Chart" + index;
        }

        /// <summary>
        ///     Get Input tooltip method.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override string GetInputTooltip(int index)
        {
            return "Chart" + index;
        }

        /// <summary>
        ///     Remove Input.
        /// </summary>
        protected override void RemoveInput()
        {
            if (InPortData.Count > 1)
                base.RemoveInput();
        }

        /// <summary>
        ///     Base methods.
        /// </summary>
        public override bool IsConvertible
        {
            get { return true; }
        }

        /// <summary>
        ///     AST Implementation
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

    /// <summary>
    ///     View Customization
    /// </summary>
    public class MandrillRowContainerViewCustomization : VariableInputNodeViewCustomization, INodeViewCustomization<MandrillRowContainerNode>
    {
        /// <summary>
        ///     Customize View
        /// </summary>
        /// <param name="model"></param>
        /// <param name="nodeView"></param>
        public void CustomizeView(MandrillRowContainerNode model, Dynamo.Controls.NodeView nodeView)
        {
            base.CustomizeView(model, nodeView);
        }
    }

    /// <summary>
    ///     Report
    /// </summary>
    [NodeName("Create Report")]
    [NodeDescription("Some Description")]
    [NodeCategory("Archi-lab_Mandrill.Report.Window")]
    [IsDesignScriptCompatible]
    [InPortTypes("RowContainer")]
    [InPortNames("RowContainer0")]
    [InPortDescriptions("RowContainer0")]
    [OutPortTypes("Report")]
    [OutPortNames("Report")]
    [OutPortDescriptions("Report0")]
    public class MandrillReportNode : VariableInputNode
    {
        /// <summary>
        ///     Report Node
        /// </summary>
        public MandrillReportNode()
        {
            RegisterAllPorts();
            ArgumentLacing = LacingStrategy.Disabled;
        }

        /// <summary>
        ///     Get input name
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override string GetInputName(int index)
        {
            return "Container" + index;
        }

        /// <summary>
        ///     Get input tooltip
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override string GetInputTooltip(int index)
        {
            return "InToolTip" + index;
        }

        /// <summary>
        ///     Remove input
        /// </summary>
        protected override void RemoveInput()
        {
            if (InPortData.Count > 1)
                base.RemoveInput();
        }

        /// <summary>
        ///     Base implementation
        /// </summary>
        public override bool IsConvertible
        {
            get { return true; }
        }

        /// <summary>
        ///     AST Implementation
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

    /// <summary>
    ///     View Customization
    /// </summary>
    public class MandrillReportViewCustomization : VariableInputNodeViewCustomization, INodeViewCustomization<MandrillReportNode>
    {
        /// <summary>
        ///     Customize view
        /// </summary>
        /// <param name="model"></param>
        /// <param name="nodeView"></param>
        public void CustomizeView(MandrillReportNode model, Dynamo.Controls.NodeView nodeView)
        {
            base.CustomizeView(model, nodeView);
        }
    }
}
