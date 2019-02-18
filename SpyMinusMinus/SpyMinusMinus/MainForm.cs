using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpyMinusMinus {
    public partial class MainForm : Form {
     

        public MainForm() {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e) {
            EmbedWindowTree();
        }

        private void EmbedWindowTree() {
            TreeForm tree = new TreeForm();
            //embed form inside main window
            tree.TopLevel = false;
            panelMain.Controls.Add(tree);
            tree.Show();
        }
    }
}
