using System;
using System.Windows.Forms;

namespace SpyMinusMinus {
    public partial class MainForm : Form {

        private static NamedPipeServer pipeMessageListener;

        public TreeForm WindowTreeForm { get; set; } = null;

        public MainForm() {
            InitializeComponent();

            pipeMessageListener = new NamedPipeServer();
        }


        private void MainForm_Shown(object sender, EventArgs e) {
            ShowWindowTree();
        }


        private void ShowWindowTree() {
            if (WindowTreeForm == null || WindowTreeForm.IsDisposed) {
                WindowTreeForm = new TreeForm(panelMain);
            }

            WindowTreeForm.Show();
            WindowTreeForm.Focus();
        }


        private void MenuItemWindows_Click(object sender, EventArgs e) {
            ShowWindowTree();
        }
        
        public static NamedPipeServer GetNamedPipeServer() {
            return pipeMessageListener;
        }

    }
}
