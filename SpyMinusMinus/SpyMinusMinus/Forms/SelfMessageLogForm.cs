using System;
using System.Windows.Forms;

namespace SpyMinusMinus {
    class SelfMessageLogForm : MessageLogForm {

        public SelfMessageLogForm() : base(null) {
            //ensure we have a handle, monitor our self
            CreateHandle();
            window = new VirtualWindow(Handle);
        }

        protected override void WndProc(ref Message m) {
            Console.WriteLine($"{m.HWnd} - {m.Msg} - {m.LParam} - {m.WParam} - {m.Result}");
            
            //todo: log own messages. problem
            //inifnite loop, logging is creating messages we want to ignore
            //the act of logging message creates more messages...
            //if (source of message not from Log()) {
            //  Log($"{m.HWnd} - {m.Msg} - {m.LParam} - {m.WParam} - {m.Result}");
            
            base.WndProc(ref m);
        }
    }
}
