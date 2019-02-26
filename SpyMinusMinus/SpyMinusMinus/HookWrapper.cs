using System;
using System.Runtime.InteropServices;


namespace SpyMinusMinus {
    class HookWrapper {       

        [DllImport("hook.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Hook(IntPtr targetHandle, IntPtr listenerHandle);

    }
}
