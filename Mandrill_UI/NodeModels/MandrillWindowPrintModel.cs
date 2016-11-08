using Autodesk.DesignScript.Runtime;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;
using System;
using System.Linq;
using System.Windows;

namespace Mandrill.Print
{
    /// <summary>
    ///     Hydra Share node implementation.
    /// </summary>
    [NodeName("Print to PDF")]
    [NodeCategory("Archi-lab_Mandrill.Report.Pdf")]
    [NodeDescription("Print Mandrill window to PDF.")]
    [IsDesignScriptCompatible]
    public class MandrillPrintNodeModel : NodeModel
    {
        private string message;

        /// <summary>
        ///     Request save action.
        /// </summary>
        public Action RequestPrint;

        /// <summary>
        ///     A message that will appear on the button
        /// </summary>
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
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
        public MandrillPrintNodeModel()
        {
            InPortData.Add(new PortData("FilePath", "A complete FilePath string including file extension."));
            InPortData.Add(new PortData("Style", "PDF Style that defines pdf size, orientation etc."));

            RegisterAllPorts();
            ArgumentLacing = LacingStrategy.Disabled;
            MessageCommand = new DelegateCommand(ShowMessage, CanShowMessage);
            Message = "   Print" + Environment.NewLine + "Window";
        }

        private static bool CanShowMessage(object obj)
        {
            return true;
        }

        private void ShowMessage(object obj)
        {
            this.RequestPrint();
        }

        /// <summary>
        ///     View customizer for CustomNodeModel Node Model.
        /// </summary>
        public class CustomNodeModelNodeViewCustomization : INodeViewCustomization<MandrillPrintNodeModel>
        {
            /// <summary>
            ///     Customization for Node View
            /// </summary>
            /// <param name="model">The NodeModel representing the node's core logic.</param>
            /// <param name="nodeView">The NodeView representing the node in the graph.</param>
            public void CustomizeView(MandrillPrintNodeModel model, NodeView nodeView)
            {
                var hydraControl = new MandrillPrintControl();
                nodeView.inputGrid.Width = 100;
                nodeView.inputGrid.Children.Add(hydraControl);
                hydraControl.DataContext = model;

                model.RequestPrint += () => PrintMandrillWindow(model, nodeView);
            }

            /// <summary>
            ///     Method that finds Mandrill Window and calls its Print() method.
            /// </summary>
            /// <param name="model"></param>
            /// <param name="nodeView"></param>
            public void PrintMandrillWindow(NodeModel model, NodeView nodeView)
            {
                string filePath;
                D3jsLib.PdfStyle style;

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

                    // process style input
                    var styleNode = model.InPorts[1].Connectors[0].Start.Owner;
                    var styleIndex = model.InPorts[1].Connectors[0].Start.Index;
                    var styleId = styleNode.GetAstIdentifierForOutputIndex(styleIndex).Name;
                    var styleMirror = nodeView.ViewModel.DynamoViewModel.Model.EngineController.GetMirror(styleId);
                    style = styleMirror.GetData().Data as D3jsLib.PdfStyle;
                }

                Mandrill.ChromeWindow.MandrillWindow win = null;

                // get Mandrill window and invoke Print Method.
                foreach (System.Windows.Window w in Application.Current.Windows)
                {
                    // Compare Type
                    if (w.GetType() == typeof(Mandrill.ChromeWindow.MandrillWindow))
                    {
                        win = (Mandrill.ChromeWindow.MandrillWindow)w;
                        break;
                    }
                }

                // call print method
                if (win != null)
                {
                    win.Print(filePath, style);
                }
                else
                {
                    MessageBox.Show("No open Mandrill Window found. Please Launch a new Mandrill Window to Print.");
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