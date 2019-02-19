using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpyMinusMinus {
    public partial class PropertiesForm : Form {

        private VirtualWindow currentWindow;

        public PropertiesForm(VirtualWindow virtualWindow) {
            InitializeComponent();
            this.currentWindow = virtualWindow;
            var g = Graphics.FromHwnd(virtualWindow.handle);
            g.DrawRectangle(Pens.Red, 0, 0, 100, 100);
            
            g.Dispose();
            Text = virtualWindow.ToString();
        }

    }
}
