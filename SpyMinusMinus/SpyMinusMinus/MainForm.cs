using System;
using System.Windows.Forms;

namespace SpyMinusMinus {
    public partial class MainForm : Form {

        NamedPipeServer pipeMessageListener;

        public MainForm() {
            InitializeComponent();
        }

        private void MainForm_Shown(object sender, EventArgs e) {
            pipeMessageListener = new NamedPipeServer();

            EmbedWindowTree();           
        }


        private void EmbedWindowTree() {
            new TreeForm(panelMain).Show();
        }

        private void menuItemWindows_Click(object sender, EventArgs e) {
            EmbedWindowTree();
        }
        
    }
}
