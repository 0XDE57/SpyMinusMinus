using System;
using System.Windows.Forms;

namespace SpyMinusMinus {
    class MessageListener : NativeWindow {

        //use CreateWindowEx(...HWND_MESSAGE...) for a message only window?
        //docs.microsoft.com/en-us/windows/desktop/winmsg/window-features#message-only-windows
        public MessageListener() {
            CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message m) {
            //Console.WriteLine($"{m.HWnd} - {m.Msg} - {m.LParam} - {m.WParam} - {m.Result}");
            base.WndProc(ref m);
        }
    }
}
