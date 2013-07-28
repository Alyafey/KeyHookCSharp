using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyHookCSharp
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYUP = 257;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        static void Main(string[] args)
        {
            _hookID = SetHook(_proc);
            Application.Run();
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (wParam == (IntPtr)WM_KEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Console.WriteLine("The Pressed key is : " + GetKey(vkCode));
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static string GetKey(int key)
        {
            string result = "";

            switch (key)
            {
                case 0:
                    result = "Null";
                    break;
                case 8:
                    result = "Back Space";
                    break;
                case 9:
                    result = "Tap";
                    break;
                case 13:
                    result = "Enter";
                    break;
                case 19:
                    result = "Pause Break";
                    break;
                case 20:
                    result = "CAPSE LOCK";
                    break;
                case 27:
                    result = "Escape";
                    break;
                case 32:
                    result = "Space";
                    break;
                case 33:
                    result = "Page UP";
                    break;
                case 34:
                    result = "Page Down";
                    break;
                case 35:
                    result = "End";
                    break;
                case 36:
                    result = "Home";
                    break;
                case 37:
                    result = "Left";
                    break;
                case 38:
                    result = "Up";
                    break;
                case 39:
                    result = "Right";
                    break;
                case 40:
                    result = "Down";
                    break;
                case 44:
                    result = "PRSTSC SYSRQ";
                    break;
                case 45:
                    result = "INS";
                    break;
                case 46:
                    result = "DEL";
                    break;
                case 91:
                    result = "Windows Key";
                    break;
                case 93:
                    result = "Menu";
                    break;
                case 160:
                    result = "Left Shift";
                    break;
                case 161:
                    result = "Right Shift";
                    break;
                case 162:
                    result = "Left Control";
                    break;
                case 163:
                    result = "Right Control";
                    break;
                case 164:
                    result = "Left Alt";
                    break;
                case 165:
                    result = "Righ Alt";
                    break;
                case 186:
                    result = "Semicolon";
                    break;
                case 187:
                    result = "Plus";
                    break;
                case 188:
                    result = "Comma";
                    break;
                case 189:
                    result = "Minus";
                    break;
                case 190:
                    result = "Period";
                    break;
                case 191:
                    result = "Question";
                    break;
                case 192:
                    result = "Tilde";
                    break;
                case 219:
                    result = "Open Brackets";
                    break;
                case 220:
                    result = "BAck Slash";
                    break;
                case 221:
                    result = "Close Brackets";
                    break;
                case 222:
                    result = "Quotation";
                    break;
                default:
                    if (key >= 48 && key <= 90)
                    {
                        result = ((char)key).ToString();
                    }
                    else if (key >= 112 && key <= 123)
                    {
                        int temp = key - 63;
                        if (temp >= 49 && temp <= 57)
                        {
                            result = "F";
                            result += ((char)temp).ToString();
                        }
                        else if (temp >= 58 && temp <= 71)
                        {
                            result = "F";
                            result += ((char)49).ToString();
                            result += ((char)(temp - 10)).ToString(); ;
                        }
                    }
                    else
                    {
                        result = "Undefined";
                    }
                    break;
            }

            return result;
        }
    }
}
