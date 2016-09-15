using Autodesk.DesignScript.Runtime;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Dynamo.Models;
using Dynamo.ViewModels;
using Dynamo.Scheduler;
using Dynamo.Engine;
using System.Linq;

namespace Mandrill.Window
{
    /// <summary>
    ///     Custom Mandrill Window node implementation
    /// </summary>
    [NodeName("Report Window")]
    [NodeCategory("Archi-lab_Mandrill.Report.Window")]
    [NodeDescription("Use this node to launch a new window that charts will be displayed in.")]
    [IsDesignScriptCompatible]
    public class MandrillWindowNodeModel : NodeModel, INotifyPropertyChanged
    {
        public bool isWindowClosed = true;
        public static DynamoView dv;
        public Action RequestNewWindow;

        public static void OnWindowClosing(object sender, CancelEventArgs e)
        {
            MandrillWindow win = (MandrillWindow)sender;
            MandrillWindowNodeModel model = win.DataContext as MandrillWindowNodeModel;
            model.isWindowClosed = true;
        }

        public event Action RequestChangeHtmlString;
        protected virtual void OnRequestChangeHtmlString()
        {
            if (RequestChangeHtmlString != null)
                RequestChangeHtmlString();
        }

        private string _myHtml;
        public string MyHtml
        {
            get { return _myHtml; }
            set
            {
                if (_myHtml != value)
                {
                    _myHtml = value;
                    RaisePropertyChanged("MyHtml");
                }
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                RaisePropertyChanged("NodeMessage");
            }
        }

        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand MessageCommand { get; set; }

        /// <summary>
        ///     Defines Mandrill Node model
        /// </summary>
        public MandrillWindowNodeModel()
        {
            InPortData.Add(new PortData("Report", "Html String to render."));
            RegisterAllPorts();
            ArgumentLacing = LacingStrategy.Disabled;

            this.PropertyChanged += HtmlString_PropertyChanged;
            foreach (var port in InPorts)
            {
                port.Connectors.CollectionChanged += Connectors_CollectionChanged;
            }

            MessageCommand = new DelegateCommand(ShowMessage, CanShowMessage);
            Message = " Launch" + Environment.NewLine + "Window";
        }

        void Connectors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnRequestChangeHtmlString();
        }

        void HtmlString_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "CachedValue")
                return;

            if (InPorts.Any(x => x.Connectors.Count == 0))
                return;

            OnRequestChangeHtmlString();
        }

        /// <summary>
        ///     Retrieves input string from connected input node.
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        public string GetInputString(EngineController engine)
        {
            string htmlString;

            // If there is an input supplied
            if (HasConnectedInput(0))
            {
                // retrieve input string from input
                var node = InPorts[0].Connectors[0].Start.Owner;
                var nodeIndex = InPorts[0].Connectors[0].Start.Index;
                var nodeName = node.GetAstIdentifierForOutputIndex(nodeIndex).Name;
                var mirrorData = engine.GetMirror(nodeName);
                D3jsLib.Report report = mirrorData.GetData().Data as D3jsLib.Report;
                htmlString = report.HtmlString;
            }
            else
            {
                htmlString = string.Empty;
            }
            return htmlString;
        }

        private static bool CanShowMessage(object obj)
        {
            return true;
        }

        private void ShowMessage(object obj)
        {
            this.RequestNewWindow();
        }
    }

    /// <summary>
    ///     Mandrill node customization implementation.
    /// </summary>
    public class MandrillWindowNodeViewCustomization : INodeViewCustomization<MandrillWindowNodeModel>
    {
        private DynamoModel dynamoModel;
        private DynamoViewModel dynamoViewModel;
        private MandrillWindowNodeModel mandrillNode;
        private const string defaultHtml =
                        @"<html>
                    <head>
                    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
                    </head>
                      <div></div>
                    <body>
                    </body>
                    </html>";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="nodeView"></param>
        public void CustomizeView(MandrillWindowNodeModel model, NodeView nodeView)
        {
            this.dynamoModel = nodeView.ViewModel.DynamoViewModel.Model;
            this.dynamoViewModel = nodeView.ViewModel.DynamoViewModel;
            mandrillNode = model;

            // load button control into node, set data context
            var helloDynamoControl = new LaunchWindowButtonControl();
            nodeView.inputGrid.Width = 100;
            nodeView.inputGrid.Children.Add(helloDynamoControl);
            helloDynamoControl.DataContext = model;

            // attach mandrill window to Dynamo control
            MandrillWindowNodeModel.dv = FindUpVisualTree<DynamoView>(nodeView);

            // attach input update and new window events to Dynamo control
            mandrillNode.RequestChangeHtmlString += UpdateHtmlString;
            model.RequestNewWindow += () => CreateNewWindow();
        }

        private void CreateNewWindow()
        {
            if (mandrillNode.isWindowClosed)
            {
                var mandrillWindow = new MandrillWindow();
                if (mandrillNode.MyHtml == string.Empty || mandrillNode.MyHtml == null)
                {
                    mandrillNode.MyHtml = defaultHtml;
                }

                mandrillWindow.DataContext = mandrillNode;
                mandrillWindow.Show();
                mandrillNode.isWindowClosed = false;
            }
        }

        private void UpdateHtmlString()
        {
            var s = dynamoViewModel.Model.Scheduler;
            var t = new DelegateBasedAsyncTask(s, () =>
            {
                mandrillNode.MyHtml = mandrillNode.GetInputString(dynamoModel.EngineController);
            });

            s.ScheduleForExecution(t);
        }

        // Thanks to Hans Hubers for this bit.
        private static T FindUpVisualTree<T>(DependencyObject initial) where T : DependencyObject
        {
            DependencyObject current = initial;
            while (current != null && current.GetType() != typeof(T))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as T;
        }

        /// <summary>
        ///     Dispose of model.
        /// </summary>
        public void Dispose() { }
    }

}