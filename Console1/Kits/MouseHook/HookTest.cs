using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Hook
{
    public class HookTest
    {
        private int KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                Debug.WriteLine("A");
                return 1;
            }
            return Win32Api.CallNextHookEx(Win32Api.hKeyboardHook, nCode, wParam, lParam);
        }

        // 安装钩子
        public void HookStart()
        {
            if (Win32Api.hMouseHook == 0)
            {
                // 创建HookProc实例
                var MouseHookProcedure = new Win32Api.HookProc(KeyboardHookProc);
                // 设置线程钩子
                Win32Api.hMouseHook = Win32Api.SetWindowsHookEx(2, Win32Api.KeyboardHookProcedure, IntPtr.Zero, Win32Api.GetCurrentThreadId());
                // 如果设置钩子失败
                if (Win32Api.hMouseHook == 0)
                {
                    HookStop();
                    throw new Exception("SetWindowsHookEx failed.");
                }
            }
        }

        // 卸载钩子
        public void HookStop()
        {
            bool retKeyboard = true;
            if (Win32Api.hKeyboardHook != 0)
            {
                retKeyboard = Win32Api.UnhookWindowsHookEx(Win32Api.hKeyboardHook);
                Win32Api.hKeyboardHook = 0;
            }
            if (!(retKeyboard)) throw new Exception("UnhookWindowsHookEx failed.");
        }
    }
}
