using System;
using System.Windows.Forms;

namespace SpyMinusMinus {
    public partial class MainForm : Form {

        private TreeForm WindowTree = null;


        public MainForm() {
            InitializeComponent();
        }


        private void MainForm_Shown(object sender, EventArgs e) {
            ShowWindowTree();           
        }


        private void ShowWindowTree() {
            if (WindowTree == null || WindowTree.IsDisposed) {
                WindowTree = new TreeForm(panelMain);
            }

            WindowTree.Show();
            WindowTree.Focus();
        }


        private void MenuItemWindows_Click(object sender, EventArgs e) {
            ShowWindowTree();
        }
        
    }
}
