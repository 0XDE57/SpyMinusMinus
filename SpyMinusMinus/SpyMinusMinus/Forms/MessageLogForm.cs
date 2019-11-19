using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SpyMinusMinus {
    public partial class MessageLogForm : Form {

        protected VirtualWindow window;
        private List<string> log;
        private int messageCount;
        private volatile bool alive;

        public MessageLogForm(VirtualWindow targetWindow) {
            InitializeComponent();

            window = targetWindow;
            log = new List<string>();
            messageCount = 0;
            alive = true;

            UpdateTitle();
        }


        internal void Log(string message) {
            //Console.WriteLine(message);
            if (!alive) {
                return;
            }

            log.Add(message);
            

            if (InvokeRequired) {
                Invoke(new Action(() => AddMessage(message)));
            } else {
                AddMessage(message);
            }
        }


        internal void Log(NativeMethods.CWPSTRUCT cwp) {
            Log($"h:{cwp.hwnd,-10} m:{cwp.message,-10} w:{cwp.wParam,-10} l:{cwp.lParam,-10}");
            messageCount++;
        }


        private void AddMessage(string message) {
            richTextBoxLog.AppendText(message + "\n");
            if (!Focused) {
                //auto scroll
                richTextBoxLog.ScrollToCaret();
            }
        }

        private void UpdateTitle() {
            Text = (window == null) ? "null" : "Messages(" + messageCount + "): " + window.ToString();          
        }

        private void MessageLogForm_FormClosing(object sender, FormClosingEventArgs e) {
            //set flag to prevent ObjectDisposedException when this form is closed/closing and another thread calls log
            alive = false;
        }
    }
}
