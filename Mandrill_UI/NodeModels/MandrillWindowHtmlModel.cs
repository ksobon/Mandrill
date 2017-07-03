using Autodesk.DesignScript.Runtime;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;
using System;
using System.Linq;
using System.Windows;
using HtmlAgilityPack;

namespace Mandrill.Html
{
    /// <summary>
    /// Exports Mandrill Report to Html.
    /// </summary>
    [NodeName("Save to Html")]
    [NodeCategory("Archi-lab_Mandrill.Report.Html")]
    [NodeDescription("Save Mandrill window as Html file.")]
    [IsDesignScriptCompatible]
    [InPortNames("FilePath", "Report")]
    [InPortDescriptions("A complete FilePath string including file extension.", "Mandrill Report containing all Charts.")]
    [InPortTypes("String", "Report")]
    public class MandrillHtmlNodeModel : NodeModel
    {
        /// <summary>
        ///     Request save action.
        /// </summary>
        public Action RequestSave;

        private string _message;
        /// <summary>
        ///     A message that will appear on the button
        /// </summary>
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged("NodeMessage");
            }
        }

        /// <summary>
        ///     Delegate Command.
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand MessageCommand { get; set; }

        /// <summary>
        ///     The constructor for a NodeModel is used to create
        ///     the input and output ports and specify the argument
        ///     lacing.
        /// </summary>
        public MandrillHtmlNodeModel()
        {
            //InPortData.Add(new PortData(, ));
            //InPortData.Add(new PortData(, ));

            RegisterAllPorts();
            ArgumentLacing = LacingStrategy.Disabled;
            MessageCommand = new DelegateCommand(ShowMessage, CanShowMessage);
            Message = " Save" + Environment.NewLine + " Html";
        }

        private static bool CanShowMessage(object obj)
        {
            return true;
        }

        private void ShowMessage(object obj)
        {
            this.RequestSave();
        }

        /// <summary>
        ///     View customizer for CustomNodeModel Node Model.
        /// </summary>
        public class CustomNodeModelNodeViewCustomization : INodeViewCustomization<MandrillHtmlNodeModel>
        {
            /// <summary>
            ///     Customization for Node View
            /// </summary>
            /// <param name="model">The NodeModel representing the node's core logic.</param>
            /// <param name="nodeView">The NodeView representing the node in the graph.</param>
            public void CustomizeView(MandrillHtmlNodeModel model, NodeView nodeView)
            {
                var buttonControl = new Print.MandrillPrintControl();
                nodeView.inputGrid.Width = 100;
                nodeView.inputGrid.Children.Add(buttonControl);
                buttonControl.DataContext = model;

                model.RequestSave += () => SaveMandrillWindow(model, nodeView);
            }

            /// <summary>
            ///     Method that finds Mandrill Window and calls its Print() method.
            /// </summary>
            /// <param name="model"></param>
            /// <param name="nodeView"></param>
            public void SaveMandrillWindow(NodeModel model, NodeView nodeView)
            {
                string filePath;
                D3jsLib.Report report;

                // collect inputs
                // prevent running if any input ports are empty
                if (model.InPorts.Any(x => x.Connectors.Count == 0))
                {
                    return;
                }
                else
                {
                    var graph = nodeView.ViewModel.DynamoViewModel.Model.CurrentWorkspace;

                    // process filePath input
                    var filePathNode = model.InPorts[0].Connectors[0].Start.Owner;
                    var filePathIndex = model.InPorts[0].Connectors[0].Start.Index;
                    var filePathId = filePathNode.GetAstIdentifierForOutputIndex(filePathIndex).Name;
                    var filePathMirror = nodeView.ViewModel.DynamoViewModel.Model.EngineController.GetMirror(filePathId);
                    filePath = filePathMirror.GetData().Data as string;

                    // process report input
                    var reportNode = model.InPorts[1].Connectors[0].Start.Owner;
                    var reportIndex = model.InPorts[1].Connectors[0].Start.Index;
                    var reportId = reportNode.GetAstIdentifierForOutputIndex(reportIndex).Name;
                    var reportMirror = nodeView.ViewModel.DynamoViewModel.Model.EngineController.GetMirror(reportId);
                    report = reportMirror.GetData().Data as D3jsLib.Report;
                }

                // print PDF
                this.SaveHtml(report, filePath);
            }

            private void SaveHtml(D3jsLib.Report report, string filePath)
            {
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(report.HtmlString);

                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='gridster-box']");
                foreach (HtmlNode n in nodes)
                {
                    n.InnerHtml = "";
                }

                // created unescaped file path removes %20 from path etc.
                string finalFilePath = filePath;

                Uri uri = new Uri(filePath);
                string absoluteFilePath = Uri.UnescapeDataString(uri.AbsoluteUri);

                if (Uri.IsWellFormedUriString(absoluteFilePath, UriKind.RelativeOrAbsolute))
                {
                    Uri newUri = new Uri(absoluteFilePath);
                    finalFilePath = newUri.LocalPath;
                }

                try
                {
                    // save html
                    System.IO.File.WriteAllText(filePath, htmlDoc.DocumentNode.InnerHtml);
                }
                catch
                {
                    MessageBox.Show("Printing failed. Is file open in another application?");
                }
            }

            /// <summary>
            ///     Dispose of model.
            /// </summary>
            public void Dispose()
            {
            }
        }
    }
}