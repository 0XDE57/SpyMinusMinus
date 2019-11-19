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
    public partial class LogMeForm : Form {

        private MessageLogForm loggerForm;

        public LogMeForm() {
            InitializeComponent();

            CreateHandle();//callingHandle  property should garantee creation, but lets be sure
            loggerForm = new MessageLogForm(new VirtualWindow(Handle)); //new MessageLogForm(null);
            loggerForm.Log("init: " + Handle.ToString());
            //loggerForm. new VirtualWindow(Handle);
            loggerForm.Show();
        }
        

        protected override void WndProc(ref Message m) {
            Console.WriteLine($"{m.HWnd} - {m.Msg} - {m.LParam} - {m.WParam} - {m.Result}");

            //todo: log own messages. problem
            //inifnite loop, logging is creating messages we want to ignore
            //the act of logging message creates more messages...
            //if (source of message not from Log()) {
            
            loggerForm?.Log($"{m.HWnd} - {m.Msg} - {m.LParam} - {m.WParam} - {m.Result}");


            base.WndProc(ref m);
        }
    }
}
