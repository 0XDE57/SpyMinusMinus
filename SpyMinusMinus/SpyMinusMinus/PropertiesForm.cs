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

        private VirtualWindow targetWindow;

        public PropertiesForm(VirtualWindow virtualWindow) {
            InitializeComponent();

            targetWindow = virtualWindow;
            
            Text = virtualWindow.ToString();
            
        }

        private static void DrawPOC(VirtualWindow virtualWindow) {
            //proof of concept, draw on other windows
            var g = Graphics.FromHwnd(virtualWindow.handle);
            g.DrawRectangle(Pens.Red, 0, 0, 100, 100);

            g.Dispose();
        }
    }
}
