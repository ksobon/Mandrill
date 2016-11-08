using System;
using System.Collections.Generic;
using CoreNodeModels;
using Dynamo.Utilities;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Linq;

namespace Mandrill_UI
{
    /// <summary>
    ///     Custom Enumeration Class for dropdown
    /// </summary>
    public abstract class CustomGenericEnumerationDropDown : DSDropDownBase
    {
        /// <summary>
        ///     Generate Dropdown
        /// </summary>
        /// <param name="name"></param>
        /// <param name="enumerationType"></param>
        public CustomGenericEnumerationDropDown(string name, Type enumerationType) : base(name)
        {
            this.EnumerationType = enumerationType;
            PopulateItems();
        }

        /// <summary>
        ///     Type variable
        /// </summary>
        public Type EnumerationType;

        /// <summary>
        ///     Populate Dropdown
        /// </summary>
        /// <param name="currentSelection"></param>
        /// <returns></returns>
        protected override SelectionState PopulateItemsCore(string currentSelection)
        {
            if (this.EnumerationType != null)
            {
                // Clear the dropdown list
                Items.Clear();

                // Get all enumeration names and add them to the dropdown menu
                foreach (string name in Enum.GetNames(EnumerationType))
                {
                    Items.Add(new CoreNodeModels.DynamoDropDownItem(name, Enum.Parse(EnumerationType, name)));
                }

                Items = Items.OrderBy(x => x.Name).ToObservableCollection();
                SelectedIndex = 0;
                return SelectionState.Done;
            }
            return SelectionState.Restore;
        }

        /// <summary>
        ///     Build Output.
        /// </summary>
        /// <param name="inputAstNodes"></param>
        /// <returns></returns>
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            // If there the dropdown is still empty try to populate it again
            if (Items.Count == 0 || Items.Count == -1)
            {
                PopulateItems();
            }

            // get the selected items name
            var stringNode = AstFactory.BuildStringNode((string)Items[SelectedIndex].Name);

            // assign the selected name to an actual enumeration value
            var assign = AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), stringNode);

            // return the enumeration value
            return new List<AssociativeNode> { assign };
        }
    }

    [NodeName("PDF Fit Mode")]
    [NodeCategory("Archi-lab_Mandrill.Report.Pdf")]
    [NodeDescription("Retrieves all available PDF Page fit modes.")]
    [IsDesignScriptCompatible]
    public class pdf_FitModeUI : CustomGenericEnumerationDropDown
    {
        public pdf_FitModeUI() : base("pdfFitMode", typeof(SelectPdf.HtmlToPdfPageFitMode)) { }
    }

    [NodeName("PDF Orientation")]
    [NodeCategory("Archi-lab_Mandrill.Report.Pdf")]
    [NodeDescription("Retrieves all available PDF Page Orientations.")]
    [IsDesignScriptCompatible]
    public class pdf_OrientationUI : CustomGenericEnumerationDropDown
    {
        public pdf_OrientationUI() : base("pdfOrientation", typeof(SelectPdf.PdfPageOrientation)) { }
    }

    [NodeName("PDF Size")]
    [NodeCategory("Archi-lab_Mandrill.Report.Pdf")]
    [NodeDescription("Retrieves all available PDF Page Sizes.")]
    [IsDesignScriptCompatible]
    public class pdf_SizeUI : CustomGenericEnumerationDropDown
    {
        public pdf_SizeUI() : base("pdfSize", typeof(SelectPdf.PdfPageSize)) { }
    }

    [NodeName("Font Weight")]
    [NodeCategory("Archi-lab_Mandrill.Text.Text")]
    [NodeDescription("Select Font Weight type for a Text Note.")]
    [IsDesignScriptCompatible]
    public class tn_FontWeight : CustomGenericEnumerationDropDown
    {
        public tn_FontWeight() : base("fontWeight", typeof(D3jsLib.Text.FontWeights)) { }
    }

    [NodeName("Font Style")]
    [NodeCategory("Archi-lab_Mandrill.Text.Text")]
    [NodeDescription("Select Font Style for a Text Note.")]
    [IsDesignScriptCompatible]
    public class tn_FontStyle : CustomGenericEnumerationDropDown
    {
        public tn_FontStyle() : base("fontStyle", typeof(D3jsLib.Text.FontStyle)) { }
    }

    [NodeName("Font Transform")]
    [NodeCategory("Archi-lab_Mandrill.Text.Text")]
    [NodeDescription("Select transformation type for a Text Note.")]
    [IsDesignScriptCompatible]
    public class tn_FontTransform : CustomGenericEnumerationDropDown
    {
        public tn_FontTransform() : base("fontTransform", typeof(D3jsLib.Text.FontTransform)) { }
    }
}