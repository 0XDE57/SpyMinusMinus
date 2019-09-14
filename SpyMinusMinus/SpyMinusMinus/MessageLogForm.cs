using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SpyMinusMinus {
    public partial class MessageLogForm : Form {

        private List<string> log = new List<string>();

        public MessageLogForm() {
            InitializeComponent();
        }

        internal void Log(string message) {         
            log.Add(message);
            Console.WriteLine(message);

            Invoke(new Action(() => AddMessage(message)));
        }

        private void AddMessage(string message) {
            richTextBoxLog.AppendText(message + "\n");
            if (!Focused) {
                richTextBoxLog.ScrollToCaret();
            }
        }
    }
}
