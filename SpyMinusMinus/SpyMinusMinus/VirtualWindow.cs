using System;
using System.Collections;
using System.Text;

namespace SpyMinusMinus {
    class VirtualWindow : IComparable {

        public IntPtr handle;
        public IntPtr parentHandle;

        public ArrayList children;

        static StringBuilder sb = new StringBuilder(255);
        public string GetWindowText {
            get {
                sb.Clear();
                //could use GetWindowTextLength() for nMaxCount, but we want to minimize the amount of interop chatter
                int nLength = NativeMethods.GetWindowText(handle, sb, sb.Capacity);
                return sb.ToString();
            }
        }

        public string GetWindowClass {
            get {
                sb.Clear();
                int nLength = NativeMethods.GetClassName(handle, sb, sb.Capacity);
                return sb.ToString();
            }
        }

        public VirtualWindow(IntPtr handle) {
            this.handle = handle;
        }

        public VirtualWindow(IntPtr handle, IntPtr parentHandle) : this(handle) {
            this.parentHandle = parentHandle;         
        }

        public void PopulateChildren() {
            children = new ArrayList();
            NativeMethods.EnumChildWindows(handle, new NativeMethods.EnumWindowProc(EnumChildWindow), handle);
        }

        private bool EnumChildWindow(IntPtr hWnd, IntPtr lParam) {
            //in this case: lParam = parent
            children.Add(new VirtualWindow(hWnd, lParam));
            return true; //continue enumeration
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;

            if (obj is VirtualWindow)
                return (obj as VirtualWindow).handle == handle;       

            return false;
        }

        public int CompareTo(object obj) {
            return ((int)handle - (int)(obj as VirtualWindow).handle);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string ToString() {
            return handle.ToString("x8") + " \"" + GetWindowText + "\" " + GetWindowClass;
        }

      
    }
}
