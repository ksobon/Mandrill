using Autodesk.DesignScript.Runtime;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using D3jsLib;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Mandrill.Html
{
    /// <summary>
    /// Exports Mandrill Report to Html.
    /// </summary>
    [NodeName("Save to Html")]
    [NodeCategory("Archi-lab_Mandrill.Report.Html")]
    [NodeDescription("Save Mandrill window as Html file.")]
    [IsDesignScriptCompatible]
    public class MandrillHtmlNodeModel : NodeModel
    {
        private string _message;

        /// <summary>
        /// Request save action.
        /// </summary>
        public Action RequestSave;

        /// <summary>
        /// A message that will appear on the button
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
        /// Delegate Command.
        /// </summary>
        [JsonIgnore]
        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand MessageCommand { get; set; }

        /// <summary>
        /// The constructor for a NodeModel is used to create
        /// the input and output ports and specify the argument
        /// lacing.
        /// </summary>
        public MandrillHtmlNodeModel()
        {
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("FilePath", "A complete FilePath string including file extension.")));
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("Report", "Mandrill Report containing all Charts.")));
            RegisterAllPorts();
            ArgumentLacing = LacingStrategy.Disabled;
            MessageCommand = new DelegateCommand(ShowMessage, CanShowMessage);
            Message = " Save" + Environment.NewLine + " Html";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inPorts"></param>
        /// <param name="outPorts"></param>
        [JsonConstructor]
        protected MandrillHtmlNodeModel(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts,
            outPorts)
        {
            MessageCommand = new DelegateCommand(ShowMessage, CanShowMessage);
            Message = " Save" + Environment.NewLine + " Html";
        }

        private static bool CanShowMessage(object obj)
        {
            return true;
        }

        private void ShowMessage(object obj)
        {
            RequestSave();
        }

        /// <summary>
        ///     View customizer for CustomNodeModel Node Model.
        /// </summary>
        public class CustomNodeModelNodeViewCustomization : INodeViewCustomization<MandrillHtmlNodeModel>
        {
            /// <summary>
            /// Customization for Node View
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
            /// Method that finds Mandrill Window and calls its Print() method.
            /// </summary>
            /// <param name="model"></param>
            /// <param name="nodeView"></param>
            public void SaveMandrillWindow(NodeModel model, NodeView nodeView)
            {
                // collect inputs
                // prevent running if any input ports are empty
                if (model.InPorts.Any(x => x.Connectors.Count == 0)) return;

                // process filePath input
                var filePathNode = model.InPorts[0].Connectors[0].Start.Owner;
                var filePathIndex = model.InPorts[0].Connectors[0].Start.Index;
                var filePathId = filePathNode.GetAstIdentifierForOutputIndex(filePathIndex).Name;
                var filePathMirror = nodeView.ViewModel.DynamoViewModel.Model.EngineController.GetMirror(filePathId);
                var filePath = filePathMirror.GetData().Data as string;

                // process report input
                var reportNode = model.InPorts[1].Connectors[0].Start.Owner;
                var reportIndex = model.InPorts[1].Connectors[0].Start.Index;
                var reportId = reportNode.GetAstIdentifierForOutputIndex(reportIndex).Name;
                var reportMirror = nodeView.ViewModel.DynamoViewModel.Model.EngineController.GetMirror(reportId);
                var report = reportMirror.GetData().Data as Report;

                // print PDF
                SaveHtml(report, filePath);
            }

            private static void SaveHtml(Report report, string filePath)
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(report.HtmlString);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='gridster-box']");
                foreach (var n in nodes)
                {
                    n.InnerHtml = "";
                }

                // created unescaped file path removes %20 from path etc.
                var finalFilePath = filePath;

                var uri = new Uri(filePath);
                var absoluteFilePath = Uri.UnescapeDataString(uri.AbsoluteUri);

                if (Uri.IsWellFormedUriString(absoluteFilePath, UriKind.RelativeOrAbsolute))
                {
                    var newUri = new Uri(absoluteFilePath);
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
            /// Dispose of model.
            /// </summary>
            public void Dispose()
            {
            }
        }
    }
}