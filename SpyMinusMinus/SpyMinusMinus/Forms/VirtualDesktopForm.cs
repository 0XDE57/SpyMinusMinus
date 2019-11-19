using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpyMinusMinus {
    public partial class VirtualDesktopForm : Form {

        private WindowManager windowManager;

        bool showBorder;
        private Rectangle virtualDesktop, scaledDesktop;
        private float scale;

        public VirtualDesktopForm() {
            InitializeComponent();
            DoubleBuffered = true;

           
            windowManager = new WindowManager();
            Screen[] screens = GetScreens();
            foreach (Screen screen in screens) {
                Console.WriteLine($"{screen.DeviceName}, {screen.Primary}, {screen.Bounds}");
            }
            Console.WriteLine("Virtual screen: " + SystemInformation.VirtualScreen);
            //todo: handle multiply monitors and window placement 
            //
            //virtualDesktop = new Rectangle(0, 0, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            virtualDesktop = new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            //virtualDesktop = new Rectangle(0, 0, 1920, 1080);//defualt 
            
            ResizeDesktop();


            Timer refreshTimer = new Timer();
            refreshTimer.Interval = 100;
            refreshTimer.Tick += (e, s) => Invalidate();
            refreshTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e) {
            Update(e.Graphics);
        }

        private void Update(Graphics g) {
            //var time = DateTime.Now;

            windowManager.EnumerateWindows();
          
            g.FillRectangle(Brushes.White, ClientRectangle);
            g.DrawRectangle(Pens.DarkGray, DisplayRectangle);
            
            g.FillRectangle(Brushes.Black, scaledDesktop);
            g.DrawRectangle(Pens.White, scaledDesktop);
            g.DrawEllipse(Pens.DarkGray, scaledDesktop);

            /*
            //test render find notepad
            VirtualWindow window = windowManager.GetWindows().Find(w => w.GetWindowText.Contains("Untitled - Notepad"));
            if (window != null) {
                NativeMethods.RECT windowRect = window.GetWindowRect();
                Rectangle scaledWindow = ScaleWindowToVirtualDesktop(windowRect, scaledDesktop, scale);

                g.DrawRectangle(Pens.Red, scaledWindow);
            }*/

            foreach (VirtualWindow window in windowManager.GetWindows()) {
                NativeMethods.RECT windowRect = window.GetWindowRect();
                Rectangle scaledWindow = ScaleWindowToVirtualDesktop(windowRect, scaledDesktop, scale);
                g.DrawRectangle(Pens.Red, scaledWindow);
            }

            //Console.WriteLine("update & render: " + DateTime.Now.Subtract(time).Milliseconds);
        }

        private static Rectangle ScaleWindowToVirtualDesktop(NativeMethods.RECT windowRect, Rectangle desktopRect, float desktopScale) {
            //convert native rect to drawing rect
            Rectangle scaledRect = new Rectangle(windowRect.Left, windowRect.Bottom, windowRect.Right - windowRect.Left, windowRect.Bottom - windowRect.Top);

            //scale
            scaledRect.X = (int)(scaledRect.X * desktopScale);
            scaledRect.Y = (int)(scaledRect.Y * desktopScale);
            scaledRect.Width = (int)(scaledRect.Width * desktopScale);
            scaledRect.Height = (int)(scaledRect.Height * desktopScale);

            //offset position based on desktop location
            scaledRect.X += desktopRect.X;
            scaledRect.Y += desktopRect.Y - scaledRect.Height;

            return scaledRect;
        }

        private void ResizeDesktop() {
            float scaleHor = (float)DisplayRectangle.Width / (float)virtualDesktop.Width;
            float scaleVert = (float)DisplayRectangle.Height / (float)virtualDesktop.Height;
            bool vert = (scaleVert >= scaleHor);

            scale = vert ? scaleHor : scaleVert;
            scaledDesktop = new Rectangle(0, 0, (int)(virtualDesktop.Width * scale) - 1, (int)(virtualDesktop.Height * scale) - 1);
            if (vert) {
                scaledDesktop.Y += DisplayRectangle.Height / 2 - scaledDesktop.Height / 2;
            } else {
                scaledDesktop.X += DisplayRectangle.Width / 2 - scaledDesktop.Width / 2;
            }
            if (Math.Abs(scaleHor - scaleVert) < 0.001f) {
                //epsilon check, if scale is close enough just set origin 0,0 
                //to keep rect within client draw area
                scaledDesktop.X = 0;
                scaledDesktop.Y = 0;
            }

            Console.WriteLine($"{scale} ({scaleHor} - {scaleVert}): {scaledDesktop}");
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

        
        private void VirtualDesktopForm_MouseDoubleClick(object sender, MouseEventArgs e) {
            showBorder = !showBorder;
            FormBorderStyle = showBorder ? FormBorderStyle.Sizable : FormBorderStyle.None;
        }
    }
}
