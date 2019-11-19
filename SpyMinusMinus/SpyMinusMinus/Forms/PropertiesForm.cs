using System.Drawing;
using System.Windows.Forms;

namespace SpyMinusMinus {
    public partial class PropertiesForm : Form {

        private VirtualWindow window;

        public PropertiesForm(VirtualWindow virtualWindow) {
            InitializeComponent();

            window = virtualWindow;
            UpdateTitle();
        }

        private void UpdateTitle() {
            Text = "Properties: " + window.ToString();
        }

        private static void DrawPOC(VirtualWindow virtualWindow) {
            //proof of concept, draw on other windows
            var g = Graphics.FromHwnd(virtualWindow.handle);
            g.DrawRectangle(Pens.Red, 0, 0, 100, 100);

            g.Dispose();
        }
    }
}
