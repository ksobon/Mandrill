using System;
using System.Windows.Interop;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Mandrill_Resources.Properties;
using System.ComponentModel;

namespace Mandrill_Grasshopper.Components.Report
{
    enum childStatus
    {
        ChildOfGH,
        ChildOfRhino,
        AlwaysOnTop
    };

    public class Mandrill_LaunchWindow : GH_Component, INotifyPropertyChanged
    {
        protected MandrillWindow mw;
        private childStatus winChildStatus = childStatus.ChildOfGH;
        bool shouldBeVisible = true;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MandrillWindow GetWindow()
        {
            if (this.mw != null)
            {
                return this.mw;
            }
            else
            {
                return null;
            }
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
                    NotifyPropertyChanged("MyHtml");
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the Mandrill_LaunchWindow class.
        /// </summary>
        public Mandrill_LaunchWindow()
          : base("LaunchWindow", "Window",
              "Launches Mandrill Window to display charts.",
              Resources.CategoryName, Resources.SubCategoryReport)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Report", "R", Resources.Report_ReportDesc, GH_ParamAccess.item);
            pManager.AddBooleanParameter("Show", "S", Resources.Report_ShowWindowDesc, GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            D3jsLib.Report report = null;
            var show = false;

            if (!DA.GetData(0, ref report)) return;
            if (!DA.GetData(1, ref show)) return;

            if (show)
            {
                shouldBeVisible = true;
                this.mw.Show();
            }
            else
            {
                shouldBeVisible = false;
                this.mw.Hide();
            }

            this.MyHtml = report.HtmlString;
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Resources.Mandrill_ChromeWindow_MandrillChromeWindowNodeModel;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{c96ef0ce-895f-4392-b015-f180360abd25}"); }
        }

        protected override void BeforeSolveInstance()
        {
            if (mw == null || !mw.IsLoaded)
            {
                SetupWin();
            }
            base.BeforeSolveInstance();
        }

        private void SetupWin()
        {
            // try closing the window if its already up
            try
            {
                mw.Close();
            }
            catch { }

            mw = new MandrillWindow();
            mw.Closed += mw_Closed;

            // set ownership based on child status
            switch (winChildStatus)
            {
                case childStatus.ChildOfGH:
                    setOwner(Grasshopper.Instances.DocumentEditor, mw);
                    break;
                case childStatus.AlwaysOnTop:
                    mw.Topmost = true;
                    break;
                case childStatus.ChildOfRhino:
                    setOwner(Rhino.RhinoApp.MainWindowHandle(), mw);
                    break;
                default:
                    break;
            }

            //make sure to hide the window when the user switches active GH document. 
            Grasshopper.Instances.ActiveCanvas.DocumentChanged -= HideWindow;
            Grasshopper.Instances.ActiveCanvas.DocumentChanged += HideWindow;

            // set data context
            mw.DataContext = this;
        }

        void mw_Closed(object sender, EventArgs e)
        {
            //remove the listener
            mw.Closed -= mw_Closed;

            //initialize a brand new window. Once it's closed, you can't get it back. 
            SetupWin();
        }

        static void setOwner(System.Windows.Forms.Form ownerForm, System.Windows.Window window)
        {
            var helper = new WindowInteropHelper(window);
            helper.Owner = ownerForm.Handle;
        }

        static void setOwner(IntPtr ownerPtr, System.Windows.Window window)
        {
            var helper = new WindowInteropHelper(window);
            helper.Owner = ownerPtr;
        }

        private void HideWindow(object sender, Grasshopper.GUI.Canvas.GH_CanvasDocumentChangedEventArgs e)
        {
            if (mw != null)
            {
                if (e.NewDocument == this.OnPingDocument() && e.OldDocument != null) // switching from other document
                {
                    try
                    {
                        if (shouldBeVisible) mw.Show();
                    }
                    catch { }
                }
                else if (e.NewDocument == this.OnPingDocument() && e.OldDocument == null) // fresh window
                {
                    try
                    {
                        if (shouldBeVisible) mw.Show();
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        mw.Hide();
                    }
                    catch { }
                }
            }
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            var toolStripMenuItem = GH_DocumentObject.Menu_AppendItem(menu, "Child of Grasshopper", new System.EventHandler(this.menu_makeChildofGH), true, winChildStatus == childStatus.ChildOfGH);
            toolStripMenuItem.ToolTipText = "When selected, the window is made a child of the Grasshopper window - when the Grasshopper window is hidden or minimized, it will disappear.";
            var toolStripMenuItem1 = GH_DocumentObject.Menu_AppendItem(menu, "Child of Rhino", new System.EventHandler(this.menu_makeChildofRhino), true, winChildStatus == childStatus.ChildOfRhino);
            toolStripMenuItem1.ToolTipText = "When selected, the window is made a child of the Rhino window - when the Rhino window is hidden or minimized, it will disappear.";
            var toolStripMenuItem2 = GH_DocumentObject.Menu_AppendItem(menu, "Always On Top", new System.EventHandler(this.menu_makeAlwaysOnTop), true, winChildStatus == childStatus.AlwaysOnTop);
            toolStripMenuItem2.ToolTipText = "When selected, the window is always on top, floating above other apps.";
            GH_DocumentObject.Menu_AppendSeparator(menu);
        }

        private void menu_makeChildofGH(object sender, System.EventArgs e)
        {
            base.RecordUndoEvent("Child Window Status Change");
            this.winChildStatus = childStatus.ChildOfGH;
            this.SetupWin();
            this.ExpireSolution(true);
        }

        private void menu_makeChildofRhino(object sender, System.EventArgs e)
        {
            base.RecordUndoEvent("Child Window Status Change");
            this.winChildStatus = childStatus.ChildOfRhino;
            this.SetupWin();
            this.ExpireSolution(true);
        }

        private void menu_makeAlwaysOnTop(object sender, System.EventArgs e)
        {
            base.RecordUndoEvent("Child Window Status Change");
            this.winChildStatus = childStatus.AlwaysOnTop;
            this.SetupWin();
            this.ExpireSolution(true);
        }
    }
}