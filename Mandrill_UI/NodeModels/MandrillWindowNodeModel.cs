using Autodesk.DesignScript.Runtime;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Dynamo.Models;
using Dynamo.ViewModels;
using Dynamo.Scheduler;
using Dynamo.Engine;
using System.Linq;
using Newtonsoft.Json;

namespace Mandrill.ChromeWindow
{
    /// <summary>
    /// Custom Mandrill Chrome Window node implementation
    /// </summary>
    [NodeName("Report Window")]
    [NodeCategory("Archi-lab_Mandrill.Report.Window")]
    [NodeDescription("Use this node to launch a new window that charts will be displayed in.")]
    [IsDesignScriptCompatible]
    public class MandrillWindowNodeModel : NodeModel
    {
        /// <summary>
        /// Window closed variable
        /// </summary>
        public bool IsWindowClosed = true;

        /// <summary>
        /// Dynamo view variable
        /// </summary>
        [JsonIgnore]
        public static DynamoView Dv;

        /// <summary>
        /// Window event
        /// </summary>
        [JsonIgnore]
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

        private string _message;

        /// <summary>
        /// Button message - databinding
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
        /// Delegate command for setting button message.
        /// </summary>
        [JsonIgnore]
        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand MessageCommand { get; set; }

        /// <summary>
        /// Defines Mandrill Node model
        /// </summary>
        public MandrillWindowNodeModel()
        {
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("Report", "Html report to render.")));
            RegisterAllPorts();
            ArgumentLacing = LacingStrategy.Disabled;

            PropertyChanged += HtmlString_PropertyChanged;
            foreach (var port in InPorts)
            {
                port.Connectors.CollectionChanged += Connectors_CollectionChanged;
            }

            MessageCommand = new DelegateCommand(ShowMessage, CanShowMessage);
            Message = " Launch" + Environment.NewLine + "Window";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inPorts"></param>
        /// <param name="outPorts"></param>
        [JsonConstructor]
        protected MandrillWindowNodeModel(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts,
            outPorts)
        {
            PropertyChanged += HtmlString_PropertyChanged;
            foreach (var port in InPorts)
            {
                port.Connectors.CollectionChanged += Connectors_CollectionChanged;
            }

            MessageCommand = new DelegateCommand(ShowMessage, CanShowMessage);
            Message = " Launch" + Environment.NewLine + "Window";
        }

        private void Connectors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
            var htmlString = string.Empty;

            // retrieve input string from input
            var node = InPorts[0].Connectors[0].Start.Owner;
            var nodeIndex = InPorts[0].Connectors[0].Start.Index;
            var nodeName = node.GetAstIdentifierForOutputIndex(nodeIndex).Name;
            var mirrorData = engine.GetMirror(nodeName);
            var report = mirrorData.GetData().Data as D3jsLib.Report;
            if (report != null) htmlString = report.HtmlString;
            return htmlString;
        }

        private static bool CanShowMessage(object obj)
        {
            return true;
        }

        private void ShowMessage(object obj)
        {
            RequestNewWindow();
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

            // load button control into node, set data context
            var helloDynamoControl = new Window.LaunchWindowButtonControl();
            nodeView.inputGrid.Width = 100;
            nodeView.inputGrid.Children.Add(helloDynamoControl);
            helloDynamoControl.DataContext = model;

            // attach mandrill window to Dynamo control
            MandrillWindowNodeModel.Dv = FindUpVisualTree<DynamoView>(nodeView);

            // attach input update and new window events to Dynamo control
            _mandrillNode.RequestChangeHtmlString += UpdateHtmlString;
            model.RequestNewWindow += CreateNewWindow;
        }

        private void CreateNewWindow()
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

        private void UpdateHtmlString()
        {
            var s = _dynamoViewModel.Model.Scheduler;
            var t = new DelegateBasedAsyncTask(s, () =>
            {
                _mandrillNode.MyHtml = _mandrillNode.GetInputString(_dynamoModel.EngineController);
            });

            s.ScheduleForExecution(t);
        }

        // Thanks to Hans Hubers for this bit.
        private static T FindUpVisualTree<T>(DependencyObject initial) where T : DependencyObject
        {
            var current = initial;
            while (current != null && current.GetType() != typeof(T))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as T;
        }

        /// <summary>
        /// Dispose of model.
        /// </summary>
        public void Dispose() { }
    }
}