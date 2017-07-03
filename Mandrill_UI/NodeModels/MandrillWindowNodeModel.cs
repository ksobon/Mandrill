using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Dynamo.Controls;
using Dynamo.Engine;
using Dynamo.Graph.Nodes;
using Dynamo.Models;
using Dynamo.Scheduler;
using Dynamo.ViewModels;
using Dynamo.Wpf;
using Mandrill.ChromeWindow;
using ProtoCore.AST.AssociativeAST;

namespace Mandrill.UI.NodeModels
{
    /// <summary>
    /// Custom Mandrill Chrome Window node implementation
    /// </summary>
    [NodeName("Report Window")]
    [NodeCategory("Archi-lab_Mandrill.Report.Window")]
    [NodeDescription("Use this node to launch a new window that charts will be displayed in.")]
    [IsDesignScriptCompatible]
    [InPortNames("Report", "Launch")]
    [InPortTypes("string", "bool")]
    [InPortDescriptions("Html report to render.", "Boolean toggle to launch window.")]
    [OutPortNames("Launched")]
    [OutPortDescriptions("Launched Window.")]
    [OutPortTypes("Boolean")]
    public class MandrillWindowNodeModel : NodeModel
    {
        /// <summary>
        /// Window closed variable
        /// </summary>
        public bool IsWindowClosed = true;

        /// <summary>
        /// 
        /// </summary>
        public bool LaunchInput;

        /// <summary>
        /// 
        /// </summary>
        public MandrillWindow MWindow;

        /// <summary>
        /// Window event
        /// </summary>
        public Action RequestNewWindow;

        /// <summary>
        /// Window closing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnWindowClosing(object sender, CancelEventArgs e)
        {
            var win = (MandrillWindow)sender;
            var model = win.DataContext as MandrillWindowNodeModel;
            if (model != null) model.IsWindowClosed = true;
            if (model != null) model.MWindow = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            var win = (MandrillWindow) sender;
            var model = win.DataContext as MandrillWindowNodeModel;
            if (model != null) model.MWindow = win;
            if (model != null) model.IsWindowClosed = false;
        }

        /// <summary>
        /// Html string change event
        /// </summary>
        public event Action RequestChangeHtmlString;

        /// <summary>
        /// On request event handler
        /// </summary>
        protected virtual void OnRequestChangeHtmlString()
        {
            RequestChangeHtmlString?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnRequestNewWindow()
        {
            RequestNewWindow?.Invoke();
        }

        private string _myHtml;

        /// <summary>
        /// Html string - databinding
        /// </summary>
        public string MyHtml
        {
            get { return _myHtml; }
            set
            {
                if (_myHtml == value) return;
                _myHtml = value;
                RaisePropertyChanged("MyHtml");
            }
        }

        /// <summary>
        /// Defines Mandrill Node model
        /// </summary>
        public MandrillWindowNodeModel()
        {
            RegisterAllPorts();
            ArgumentLacing = LacingStrategy.Disabled;

            PropertyChanged += HtmlString_PropertyChanged;
            foreach (var port in InPorts)
            {
                port.Connectors.CollectionChanged += Connectors_ReportChanged;
            }
        }

        private void Connectors_ReportChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnRequestChangeHtmlString();
        }

        private void HtmlString_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "CachedValue")
                return;

            if (InPorts.Any(x => x.Connectors.Count == 0))
                return;

            OnRequestChangeHtmlString();
        }

        /// <summary>
        /// Retrieves input string from connected input node.
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        public string GetInputString(EngineController engine)
        {
            var htmlString = "";

            // If there is an input supplied
            if (HasConnectedInput(0))
            {
                // retrieve input string from input
                var node = InPorts[0].Connectors[0].Start.Owner;
                var nodeIndex = InPorts[0].Connectors[0].Start.Index;
                var nodeName = node.GetAstIdentifierForOutputIndex(nodeIndex).Name;
                var mirrorData = engine.GetMirror(nodeName);
                var report = mirrorData.GetData().Data as D3jsLib.Report;
                if (report != null) htmlString = report.HtmlString;
            }
            else
            {
                htmlString = string.Empty;
            }
            return htmlString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        public bool GetBooleanInput(EngineController engine)
        {
            bool output;

            // If there is an input supplied
            if (HasConnectedInput(1))
            {
                // retrieve input string from input
                var node = InPorts[1].Connectors[0].Start.Owner;
                var nodeIndex = InPorts[1].Connectors[0].Start.Index;
                var nodeName = node.GetAstIdentifierForOutputIndex(nodeIndex).Name;
                var mirrorData = engine.GetMirror(nodeName);
                output = (bool)mirrorData.GetData().Data;
            }
            else
            {
                output = false;
            }
            return output;
        }

        /// <summary>
        /// 
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

            AssociativeNode boolNode = AstFactory.BuildBooleanNode(IsWindowClosed);

            return new[]
            {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), boolNode),
            };
        }
    }

    /// <summary>
    /// Mandrill node customization implementation.
    /// </summary>
    public class MandrillWindowNodeViewCustomization : INodeViewCustomization<MandrillWindowNodeModel>
    {
        private DynamoModel _dynamoModel;
        private DynamoViewModel _dynamoViewModel;
        private MandrillWindowNodeModel _mandrillNode;
        private const string DefaultHtml =
                        @"<html>
                    <head>
                    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
                    </head>
                      <div></div>
                    <body>
                    </body>
                    </html>";

        /// <summary>
        /// View customization.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="nodeView"></param>
        public void CustomizeView(MandrillWindowNodeModel model, NodeView nodeView)
        {
            _dynamoModel = nodeView.ViewModel.DynamoViewModel.Model;
            _dynamoViewModel = nodeView.ViewModel.DynamoViewModel;
            _mandrillNode = model;

            // attach input update and new window events to Dynamo control
            _mandrillNode.RequestChangeHtmlString += UpdateHtmlString;
            model.RequestNewWindow += CreateNewWindow;
        }

        private void CreateNewWindow()
        {
            if (_mandrillNode.LaunchInput)
            {
                if (!_mandrillNode.IsWindowClosed) return;

                var mandrillWindow = new MandrillWindow();
                if (string.IsNullOrEmpty(_mandrillNode.MyHtml))
                {
                    _mandrillNode.MyHtml = DefaultHtml;
                }

                mandrillWindow.DataContext = _mandrillNode;
                mandrillWindow.Show();
                _mandrillNode.IsWindowClosed = false;
            }
            else
            {
                if (!_mandrillNode.IsWindowClosed)
                {
                    _mandrillNode.MWindow.Close();
                }
            }
        }

        private void UpdateHtmlString()
        {
            var s = _dynamoViewModel.Model.Scheduler;
            var t = new DelegateBasedAsyncTask(s, () =>
            {
                _mandrillNode.LaunchInput = _mandrillNode.GetBooleanInput(_dynamoModel.EngineController);
                _mandrillNode.RequestNewWindow();
                _mandrillNode.MyHtml = _mandrillNode.GetInputString(_dynamoModel.EngineController);
            });

            s.ScheduleForExecution(t);
        }

        /// <summary>
        /// Dispose of model.
        /// </summary>
        public void Dispose() { }
    }

}