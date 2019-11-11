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

        private WindowManager windowManager;

        private Rectangle virtualDesktop, scaledDesktop;

        public VirtualDesktopForm() {
            InitializeComponent();
            DoubleBuffered = true;

           
            windowManager = new WindowManager();
            Screen[] screens = GetScreens();
            foreach (Screen screen in screens) {
                Console.WriteLine($"{screen.DeviceName}, {screen.Primary}, {screen.Bounds}");
            }
            Console.WriteLine("Virtual screen: " + SystemInformation.VirtualScreen);
            virtualDesktop = new Rectangle(0, 0, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            //virtualDesktop = new Rectangle(0, 0, screens[0].Bounds.Width, screens[0].Bounds.Height);
            ResizeDesktop();
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

                //todo: scale windows relative to scaled desktop
                g.DrawRectangle(new Pen(Color.FromArgb(rng.Next(255), rng.Next(255), rng.Next(255))), rect);

            }
            g.DrawEllipse(Pens.Red, ClientRectangle);

            
            g.DrawRectangle(Pens.White, scaledDesktop);
            g.DrawEllipse(Pens.DarkGray, scaledDesktop);
        }

        private void ResizeDesktop() {
            float scaleHor = (float)DisplayRectangle.Width / (float)virtualDesktop.Width;
            float scaleVert = (float)DisplayRectangle.Height / (float)virtualDesktop.Height;
            bool vert = (scaleVert >= scaleHor);
            float scale = vert ? scaleHor : scaleVert;


            scaledDesktop = new Rectangle(0, 0, (int)(virtualDesktop.Width * scale) - 1, (int)(virtualDesktop.Height * scale) - 1);
            if (vert) {
                scaledDesktop.Y += DisplayRectangle.Height / 2 - scaledDesktop.Height / 2;
            } else {
                scaledDesktop.X += DisplayRectangle.Width / 2 - scaledDesktop.Width / 2;
            }
            if (Math.Abs(scaleHor - scaleVert) < 0.001f) {
                //epsilon check, if scale is close enough just set origin 0,0 
                // to keep rect within client draw area
                scaledDesktop.X = 0;
                scaledDesktop.Y = 0;
            }
            Console.WriteLine($"{scaleHor} - {scaleVert}: {scaledDesktop}");
        }

        private void VirtualDesktopForm_Resize(object sender, EventArgs e) {
            ResizeDesktop();

            //repaint
            Invalidate();
        }

        public Screen[] GetScreens() {
            Screen[] screens = Screen.AllScreens;
            return screens;
        }

        bool showBorder;
        private void VirtualDesktopForm_MouseDoubleClick(object sender, MouseEventArgs e) {
            showBorder = !showBorder;
            FormBorderStyle = showBorder ? FormBorderStyle.Sizable : FormBorderStyle.None;
        }
    }
}
