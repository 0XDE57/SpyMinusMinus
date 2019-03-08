using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static SpyMinusMinus.NativeMethods;

namespace SpyMinusMinus {
    class MessageListener : NativeWindow {

        //use CreateWindowEx(...HWND_MESSAGE...) for a message only window?
        //docs.microsoft.com/en-us/windows/desktop/winmsg/window-features#message-only-windows
        public MessageListener() {
            CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message m) {
            //Console.WriteLine($"{m.HWnd} - {m.Msg} - {m.LParam} - {m.WParam} - {m.Result}");
            if (m.Msg == WM_COPYDATA) {
                try {
                    //Console.WriteLine($"{m.HWnd} - {m.Msg} - {m.LParam} - {m.WParam} - {m.Result}");

                    COPYDATASTRUCT copydata = Marshal.PtrToStructure<COPYDATASTRUCT>(m.LParam);
                    Console.WriteLine($"{copydata.dwData} - {copydata.cbData} - {copydata.lpData}");

                    Message mTest = Marshal.PtrToStructure<Message>(copydata.lpData);
                    //CWPSTRUCT message = Marshal.PtrToStructure<CWPSTRUCT>(copydata.lpData);
                    //Console.WriteLine($"{message.hwnd} - {message.message} - {message.lParam} - {message.wParam} - {m.Result}");
                    Console.WriteLine($"{mTest.HWnd} - {mTest.Msg} - {mTest.LParam} - {mTest.WParam} - {m.Result}");
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
                return;
            }

            base.WndProc(ref m);
        }
    }
}
