using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SpyMinusMinus {
    public partial class MessageLogForm : Form {

        private VirtualWindow window;
        private List<string> log = new List<string>();


        public MessageLogForm(VirtualWindow targetWindow) {
            InitializeComponent();

            window = targetWindow;

            UpdateTitle();
        }


        internal void Log(string message) {         
            log.Add(message);
            Console.WriteLine(message);

            if (InvokeRequired) {
                Invoke(new Action(() => AddMessage(message)));
            } else {
                AddMessage(message);
            }
        }


        internal void Log(NativeMethods.CWPSTRUCT cwp) {
            Log($"h:{cwp.hwnd,-10} m:{cwp.message,-10} w:{cwp.wParam,-10} l:{cwp.lParam,-10}");
        }


        private void AddMessage(string message) {
            richTextBoxLog.AppendText(message + "\n");
            if (!Focused) {
                richTextBoxLog.ScrollToCaret();
            }
        }

        private void UpdateTitle() {
            Text = "Messages: " + window.ToString();
        }

    }
}
