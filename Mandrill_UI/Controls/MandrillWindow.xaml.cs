using System;

namespace Mandrill.Window
{
    public partial class MandrillWindow : System.Windows.Window
    {
        public MandrillWindow()
        {
            InitializeComponent();

            // attach window to dynamo
            this.Owner = MandrillWindowNodeModel.dv;

            // add closing event
            this.Closing += MandrillWindowNodeModel.OnWindowClosing;
        }
    }
}