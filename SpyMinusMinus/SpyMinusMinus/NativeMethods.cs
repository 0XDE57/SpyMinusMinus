using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SpyMinusMinus {
    public class NativeMethods {

        [StructLayout(LayoutKind.Sequential)]
        internal struct COPYDATASTRUCT {
            /*
            public IntPtr dwData;
            public ulong cbData;
            public IntPtr lpData;*/
            public uint dwData;
            public int cbData;
            public IntPtr lpData;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct CWPSTRUCT {
            public IntPtr lParam;
            public IntPtr wParam;
            public uint message;
            public IntPtr hwnd;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int Left;   // x position of upper-left corner
            public int Top;    // y position of upper-left corner
            public int Right;  // x position of lower-right corner
            public int Bottom; // y position of lower-right corner
        }

        //WinUser.h
        //Window field offsets for GetWindowLong()
        public enum WindowLongFlags : int {
            GWL_WNDPROC = -4,
            GWL_HINSTANCE = -6,
            GWL_HWNDPARENT = -8,
            GWL_STYLE = -16,
            GWL_EXSTYLE = -20,
            GWL_USERDATA = -21,
            GWL_ID = -12,
            /*if _WIN64 
            GWLP_WNDPROC = -4,
            GWLP_HINSTANCE = -6,
            GWLP_HWNDPARENT = -8,
            GWLP_USERDATA = -21,
            GWLP_ID = -12
            */        
        }

        //Class field offsets for GetClassLong()
        public enum ClassLongFlags : int {
            GCL_MENUNAME = -8,
            GCL_HBRBACKGROUND = -10,
            GCL_HCURSOR = -12,
            GCL_HICON = -14,
            GCL_HMODULE = -16,
            GCL_CBWNDEXTRA = -18,
            GCL_CBCLSEXTRA = -20,
            GCL_WNDPROC = -24,
            GCL_STYLE = -26,
            GCW_ATOM = -32,
            GCL_HICONSM = 34//if (WINVER >= 0x0400)
        }

        public const int WM_COPYDATA = 0x004A;

        internal delegate bool EnumWindowProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumWindows(EnumWindowProc lpEnumFunc, IntPtr lParam);


        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowProc lpEnumFunc, IntPtr lParam);


        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetDesktopWindow();


        [DllImport("user32", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);


        [DllImport("user32", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);


        public static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex) {
            if (IntPtr.Size == 4) {
                return new IntPtr(GetClassLongPtr32(hWnd, nIndex));
            } 
            return GetClassLongPtr64(hWnd, nIndex);
        }

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        private static extern uint GetClassLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        private static extern IntPtr GetClassLongPtr64(IntPtr hWnd, int nIndex);


        public static IntPtr GetWindowLong(IntPtr hWnd, int nIndex) {
            if (IntPtr.Size == 4) {
                return GetWindowLong32(hWnd, nIndex);
            }
            return GetWindowLongPtr64(hWnd, nIndex);
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        private static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
    }
}
