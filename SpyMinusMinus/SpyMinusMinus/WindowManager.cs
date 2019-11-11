using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyMinusMinus {
    class WindowManager {

        public List<VirtualWindow> windowHandles;
        public List<VirtualWindow> previousHandles;

        public WindowManager() {
            windowHandles = new List<VirtualWindow>();
            previousHandles = new List<VirtualWindow>();
        }

        public void EnumerateWindows() {
            //todo: use GCHandle for GC safety:
            //docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.gchandle
            previousHandles.Clear();
            previousHandles.AddRange(windowHandles);
            windowHandles.Clear();

            NativeMethods.EnumWindows(new NativeMethods.EnumWindowProc(EnumWindow), IntPtr.Zero);

            //windowHandles.Sort();
            //windowHandles.ForEach(window => Console.WriteLine(window.ToString()));
        }

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam) {
            windowHandles.Add(new VirtualWindow(hWnd));
            return true; //continue enumeration
        }

        internal List<VirtualWindow> GetWindows() {
            return windowHandles;
        }
    }
}
