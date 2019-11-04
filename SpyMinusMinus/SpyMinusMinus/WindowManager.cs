using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyMinusMinus {
    class WindowManager {

        private List<VirtualWindow> windowHandles;
        private List<VirtualWindow> previousHandles;

        public WindowManager() {
            windowHandles = new List<VirtualWindow>();
            previousHandles = new List<VirtualWindow>();
        }

        private void EnumerateWindows() {
            //todo: use GCHandle for GC safety:
            //docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.gchandle
            windowHandles.Clear();
            NativeMethods.EnumWindows(new NativeMethods.EnumWindowProc(EnumWindow), IntPtr.Zero);
        }

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam) {
            windowHandles.Add(new VirtualWindow(hWnd));
            return true; //continue enumeration
        }

        internal List<VirtualWindow> GetWindowHandles() {
            return windowHandles;
        }
    }
}
