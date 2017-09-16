//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Runtime.InteropServices;

//namespace Console1.Kits.NewFolder1
//{
//    public class MouseHook
//    {

//        private Win32Api.POINT point;
//        private Win32Api.POINT Point
//        {
//            get { return point; }
//            set
//            {
//                if (point != value)
//                {
//                    point = value;
//                    if (MouseMoveEvent != null)
//                    {
//                        var e = new MouseEventArgs(MouseButtons.Left, 0, point.X, point.Y, 0);
//                        MouseMoveEvent(this, e);
//                    }
//                }
//            }
//        }
//        private int hHook;
//        public const int WH_MOUSE_LL = 14;
//        public Win32Api.HookProc hProc;
//        public MouseHook()
//        {
//            Point = new Win32Api.POINT();
//        }
//        public int SetHook()
//        {
//            hProc = new Win32Api.HookProc(MouseHookProc);
//            hHook = Win32Api.SetWindowsHookEx(WH_MOUSE_LL, hProc, IntPtr.Zero, 0);
//            return hHook;
//        }
//        public void UnHook()
//        {
//            Win32Api.UnhookWindowsHookEx(hHook);
//        }
//        private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
//        {
//            Win32Api.MouseHookStruct MyMouseHookStruct = (Win32Api.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(Win32Api.MouseHookStruct));
//            if (nCode < 0)
//            {
//                return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
//            }
//            else
//            {
//                Point = new Win32Api.POINT(MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y);
//                return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
//            }
//        }
//        //委托+事件（把钩到的消息封装为事件，由调用者处理）
//        public delegate void MouseMoveHandler(object sender, MouseEventArgs e);
//        public event MouseMoveHandler MouseMoveEvent;

//        public delegate void MouseClickHandler(object sender, MouseEventArgs e);
//        public event MouseClickHandler MouseClickEvent;
//    }
//}
