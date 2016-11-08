//using Autodesk.DesignScript.Runtime;
//using Dynamo.Controls;
//using Dynamo.Graph.Nodes;
//using Dynamo.Wpf;
//using System;
//using System.ComponentModel;
//using Dynamo.Models;
//using Dynamo.ViewModels;
//using Dynamo.Scheduler;
//using Dynamo.Engine;
//using System.Linq;
//using System.Drawing.Printing;
//using System.Collections.Generic;
//using ProtoCore.AST.AssociativeAST;
//using System.Xml;
//using Dynamo.Graph;
//using System.Windows.Threading;

//namespace Mandrill.Dropdown
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [IsVisibleInDynamoLibrary(false)]
//    public class PaperSizeWrapper
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public int Index { get; set; }
//        /// <summary>
//        /// 
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="index"></param>
//        /// <param name="name"></param>
//        public PaperSizeWrapper(int index, string name)
//        {
//            this.Index = index;
//            this.Name = name;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public override string ToString()
//        {
//            return this.Name;
//        }
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    [NodeName("Paper Sizes")]
//    [NodeCategory("Archi-lab_Mandrill.Report.Window")]
//    [NodeDescription("Use this node to launch a new window that charts will be displayed in.")]
//    [IsDesignScriptCompatible]
//    public class PaperSizeDropdownNodeModel : NodeModel, INotifyPropertyChanged
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public event Action RequestChangeCollection;

//        /// <summary>
//        /// 
//        /// </summary>
//        protected virtual void OnRequestChangeCollection()
//        {
//            if (RequestChangeCollection != null)
//                RequestChangeCollection();
//        }


//        private IList<PaperSizeWrapper> _availablePaperSizes;

//        /// <summary>
//        /// 
//        /// </summary>
//        public IList<PaperSizeWrapper> AvailablePaperSizes
//        {
//            get { return _availablePaperSizes; }
//            set
//            {
//                if (_availablePaperSizes != value)
//                {
//                    _availablePaperSizes = value;
//                    RaisePropertyChanged("AvailablePaperSizes");
//                }
//            }
//        }

//        private int _selectedPaperSize = 0;
//        /// <summary>
//        /// 
//        /// </summary>
//        public int SelectedPaperSize
//        {
//            get { return _selectedPaperSize; }
//            set
//            {
//                if (_selectedPaperSize != value)
//                {
//                    _selectedPaperSize = value;
//                    RaisePropertyChanged("SelectedPaperSize");
//                    OnNodeModified();
//                }
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public PaperSizeDropdownNodeModel()
//        {
//            InPortData.Add(new PortData("printer", "some description"));
//            OutPortData.Add(new PortData("paper", "selected printer"));

//            RegisterAllPorts();
//            ArgumentLacing = LacingStrategy.Disabled;

//            this.PropertyChanged += Input_PropertyChanged;

//            foreach (var port in InPorts)
//            {
//                port.Connectors.CollectionChanged += Connectors_CollectionChanged;
//            }
//        }

//        void Connectors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
//        {
//            OnRequestChangeCollection();
//        }

//        void Input_PropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            if (e.PropertyName != "CachedValue")
//                return;

//            if (InPorts.Any(x => x.Connectors.Count == 0))
//                return;

//            OnRequestChangeCollection();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="engine"></param>
//        /// <returns></returns>
//        public IList<PaperSizeWrapper> GetAvailablePrinters(EngineController engine)
//        {
//            IList<PaperSizeWrapper> paperSizes;

//            if (HasConnectedInput(0))
//            {
//                var node = InPorts[0].Connectors[0].Start.Owner;
//                var nodeIndex = InPorts[0].Connectors[0].Start.Index;
//                var nodeName = node.GetAstIdentifierForOutputIndex(nodeIndex).Name;
//                var mirrorData = engine.GetMirror(nodeName);
//                string printerName = mirrorData.GetData().Data as string;

//                PrinterSettings settings = new PrinterSettings();
//                settings.PrinterName = printerName;
//                PrinterSettings.PaperSizeCollection ps = settings.PaperSizes;

//                IList<PaperSizeWrapper> list = new List<PaperSizeWrapper>();
//                for (int i = 0; i < ps.Count; i++)
//                {
//                    list.Add(new PaperSizeWrapper(i, ps[i].PaperName));
//                }
//                paperSizes = list;
//            }
//            else
//            {
//                paperSizes = new List<PaperSizeWrapper>() { new PaperSizeWrapper(0, " ")};
//            }

//            return paperSizes;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="inputAstNodes"></param>
//        /// <returns></returns>
//        [IsVisibleInDynamoLibrary(false)]
//        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
//        {
//            if (this.SelectedPaperSize == -1)
//            {
//                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
//            }

//            var paperNode = AstFactory.BuildStringNode(this.AvailablePaperSizes[this.SelectedPaperSize].Name);
//            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), paperNode) };
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="element"></param>
//        /// <param name="context"></param>
//        protected override void SerializeCore(XmlElement element, SaveContext context)
//        {
//            base.SerializeCore(element, context);

//            XmlElement paperSize = element.OwnerDocument.CreateElement("sPaper");
//            paperSize.InnerText = this.SelectedPaperSize.ToString();
//            element.AppendChild(paperSize);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="element"></param>
//        /// <param name="context"></param>
//        protected override void DeserializeCore(XmlElement element, SaveContext context)
//        {
//            base.DeserializeCore(element, context);

//            var paperNode = element.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "sPaper");

//            if (paperNode != null)
//            {
//                this._selectedPaperSize = int.Parse(paperNode.InnerText);
//            }

//        }
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    public class DropdownNodeViewCustomization : INodeViewCustomization<PaperSizeDropdownNodeModel>
//    {
//        private DispatcherSynchronizationContext syncContext;
//        private DynamoModel dynamoModel;
//        private DynamoViewModel dynamoViewModel;
//        private PaperSizeDropdownNodeModel dropdownNode;

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="model"></param>
//        /// <param name="nodeView"></param>
//        public void CustomizeView(PaperSizeDropdownNodeModel model, NodeView nodeView)
//        {
//            syncContext = new DispatcherSynchronizationContext(nodeView.Dispatcher);
//            this.dynamoModel = nodeView.ViewModel.DynamoViewModel.Model;
//            this.dynamoViewModel = nodeView.ViewModel.DynamoViewModel;
//            dropdownNode = model;

//            var dropdownControl = new PaperSizeDropdownControl();
//            nodeView.inputGrid.Width = 100;
//            nodeView.inputGrid.Children.Add(dropdownControl);
//            dropdownControl.DataContext = model;

//            dropdownNode.RequestChangeCollection += UpdatePaperSizeCollection;
//        }

//        private void UpdatePaperSizeCollection()
//        {
//            var s = dynamoViewModel.Model.Scheduler;
//            var t = new DelegateBasedAsyncTask(s, () =>
//            {
//                dropdownNode.AvailablePaperSizes = dropdownNode.GetAvailablePrinters(dynamoModel.EngineController);
//            });

//            //t.ThenSend((_) => {
//            //    dropdownNode.SelectedPaperSize = 0;
//            //}, syncContext);

//            s.ScheduleForExecution(t);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public void Dispose() { }
//    }

//}