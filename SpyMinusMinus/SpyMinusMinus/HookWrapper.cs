using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpyMinusMinus {
    class HookWrapper {       

        [DllImport("hook.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Hook(IntPtr targetHandle, IntPtr listenerHandle);

        [DllImport("hook.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Sum(int x, int y);

    }
}
