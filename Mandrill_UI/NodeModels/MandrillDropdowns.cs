using System;
using System.Collections.Generic;
using CoreNodeModels;
using Dynamo.Utilities;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Linq;
using Newtonsoft.Json;
#pragma warning disable 1591

namespace Mandrill_UI
{
    /// <summary>
    /// Custom Enumeration Class for dropdown
    /// </summary>
    public abstract class CustomGenericEnumerationDropDown : DSDropDownBase
    {
        /// <summary>
        /// Generic Enumeration Dropdown
        /// </summary>
        /// <param name="name">Node Name</param>
        /// <param name="enumerationType">Type of Enumeration to Display</param>
        protected CustomGenericEnumerationDropDown(string name, Type enumerationType) : base(name)
        {
            EnumerationType = enumerationType;
            PopulateDropDownItems();
        }

        [JsonConstructor]
        protected CustomGenericEnumerationDropDown(string name, Type enumerationType, IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts)
            : base(name, inPorts, outPorts)
        {
            EnumerationType = enumerationType;
            PopulateDropDownItems();
        }

        /// <summary>
        /// Type of Enumeration
        /// </summary>
        private Type EnumerationType
        {
            get;
        }

        protected override SelectionState PopulateItemsCore(string currentSelection)
        {
            PopulateDropDownItems();
            return SelectionState.Done;
        }

        /// <summary>
        /// Populate Items in Dropdown menu
        /// </summary>
        public void PopulateDropDownItems()
        {
            if (EnumerationType != null)
            {
                // Clear the dropdown list
                Items.Clear();

                // Get all enumeration names and add them to the dropdown menu
                foreach (string name in Enum.GetNames(EnumerationType))
                {
                    Items.Add(new DynamoDropDownItem(name, Enum.Parse(EnumerationType, name)));
                }

                Items = Items.OrderBy(x => x.Name).ToObservableCollection();
            }
        }

        /// <summary>
        /// Assign the selected Enumeration value to the output
        /// </summary>
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            // If the dropdown is still empty try to populate it again          
            if (Items.Count == 0 || Items.Count == -1)
            {
                if (EnumerationType != null && Enum.GetNames(EnumerationType).Length > 0)
                {
                    PopulateItems();
                }
            }

            // get the selected items name
            var stringNode = AstFactory.BuildStringNode(Items[SelectedIndex].Name);

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
    public class PdfFitModeUi : CustomGenericEnumerationDropDown
    {
        private const string OutputName = "pdfFitMode";
        public PdfFitModeUi() : base(OutputName, typeof(SelectPdf.HtmlToPdfPageFitMode)) { }

        [JsonConstructor]
        public PdfFitModeUi(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) 
            : base(OutputName, typeof(SelectPdf.HtmlToPdfPageFitMode), inPorts, outPorts) { }
    }

    [NodeName("PDF Orientation")]
    [NodeCategory("Archi-lab_Mandrill.Report.Pdf")]
    [NodeDescription("Retrieves all available PDF Page Orientations.")]
    [IsDesignScriptCompatible]
    public class PdfOrientationUi : CustomGenericEnumerationDropDown
    {
        private const string OutputName = "pdfOrientation";
        public PdfOrientationUi() : base(OutputName, typeof(SelectPdf.PdfPageOrientation)) { }

        [JsonConstructor]
        public PdfOrientationUi(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) 
            : base(OutputName, typeof(SelectPdf.PdfPageOrientation), inPorts, outPorts) { }
    }

    [NodeName("PDF Size")]
    [NodeCategory("Archi-lab_Mandrill.Report.Pdf")]
    [NodeDescription("Retrieves all available PDF Page Sizes.")]
    [IsDesignScriptCompatible]
    public class PdfSizeUi : CustomGenericEnumerationDropDown
    {
        private const string OutputName = "pdfSize";
        public PdfSizeUi() : base(OutputName, typeof(SelectPdf.PdfPageSize)) { }

        [JsonConstructor]
        public PdfSizeUi(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) 
            : base(OutputName, typeof(SelectPdf.PdfPageSize), inPorts, outPorts) { }
    }

    [NodeName("Font Weight")]
    [NodeCategory("Archi-lab_Mandrill.Text.Text")]
    [NodeDescription("Select Font Weight type for a Text Note.")]
    [IsDesignScriptCompatible]
    public class TnFontWeight : CustomGenericEnumerationDropDown
    {
        private const string OutputName = "fontWeight";
        public TnFontWeight() : base(OutputName, typeof(D3jsLib.Text.FontWeights)) { }

        [JsonConstructor]
        public TnFontWeight(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) 
            : base(OutputName, typeof(D3jsLib.Text.FontWeights), inPorts, outPorts) { }
    }

    [NodeName("Font Style")]
    [NodeCategory("Archi-lab_Mandrill.Text.Text")]
    [NodeDescription("Select Font Style for a Text Note.")]
    [IsDesignScriptCompatible]
    public class TnFontStyle : CustomGenericEnumerationDropDown
    {
        private const string OutputName = "fontStyle";
        public TnFontStyle() : base(OutputName, typeof(D3jsLib.Text.FontStyle)) { }

        [JsonConstructor]
        public TnFontStyle(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) 
            : base(OutputName, typeof(D3jsLib.Text.FontStyle), inPorts, outPorts) { }
    }

    [NodeName("Font Transform")]
    [NodeCategory("Archi-lab_Mandrill.Text.Text")]
    [NodeDescription("Select transformation type for a Text Note.")]
    [IsDesignScriptCompatible]
    public class TnFontTransform : CustomGenericEnumerationDropDown
    {
        private const string OutputName = "fontTransform";
        public TnFontTransform() : base(OutputName, typeof(D3jsLib.Text.FontTransform)) { }

        [JsonConstructor]
        public TnFontTransform(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) 
            : base(OutputName, typeof(D3jsLib.Text.FontTransform), inPorts, outPorts) { }
    }
}