using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpyMinusMinus {
    public partial class VirtualDesktopForm : Form {

        WindowManager windowManager;

        Rectangle virtualDesktop;

        public VirtualDesktopForm() {
            InitializeComponent();
            DoubleBuffered = true;
            //FormBorderStyle = FormBorderStyle.None;

            windowManager = new WindowManager();
            Screen[] screens = GetScreens();
            foreach (Screen screen in screens) {
                Console.WriteLine($"{screen.DeviceName}, {screen.Primary}, {screen.Bounds}");
            }
            Console.WriteLine("Virtual screen: " + SystemInformation.VirtualScreen);
            virtualDesktop = new Rectangle(0, 0, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
        }

        protected override void OnPaint(PaintEventArgs e) {
            Update(e.Graphics);
        }

        private void Update(Graphics g) {
            windowManager.EnumerateWindows();
            Random rng = new Random();

            g.FillRectangle(Brushes.Black, ClientRectangle);
            
            foreach (VirtualWindow window in windowManager.GetWindows()) {
                NativeMethods.RECT r = window.GetWindowRect();
                Rectangle rect = new Rectangle {
                    X = r.Left,
                    Y = r.Bottom,
                    Width = r.Right,
                    Height = r.Top
                };

                g.DrawRectangle(new Pen(Color.FromArgb(rng.Next(255), rng.Next(255), rng.Next(255))), rect);

            }
            g.DrawEllipse(Pens.Red, ClientRectangle);

            Rectangle scaledRect = new Rectangle(0, 0, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            float scale = scaledRect.Width / ClientRectangle.Width;

            //g.DrawRectangle(Pens.White, scaledRect);
        }

        private void VirtualDesktopForm_Resize(object sender, EventArgs e) {

            //repaint
            Invalidate();
        }

        public Screen[] GetScreens() {
            Screen[] screens = Screen.AllScreens;
            return screens;
        }

    }
}
