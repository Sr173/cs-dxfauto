using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cs_dxfAuto {
    class Process {
        [DllImport("User32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll")]
        public static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out int id);
        [DllImport("kernel32.dll")]
        public static extern Int32 OpenProcess(Int32 dwAccessFlag, bool handle, Int32 dwProcessID);
        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        public Int32 OpenProcessByWindowName(string lpClassName, string lpWindowName) {
            IntPtr hWnd = FindWindow(lpClassName, lpWindowName);

            if (hWnd == IntPtr.Zero)
                MessageBox.Show("找不到窗口，打开进程失败");

            if (hWnd == IntPtr.Zero)
                return -1;
            Int32 ID;
            GetWindowThreadProcessId(hWnd, out ID);
            Int32 handle = OpenProcess(0x1F0FFF, false, ID);
            return handle;
        }

        public Int32 OpenProcessByWindow(IntPtr hWnd) {
            Int32 ID;
            GetWindowThreadProcessId(hWnd, out ID);
            Int32 handle = OpenProcess(0x1F0FFF, false, ID);
            return handle;
        }

        public Int32 GetProcessIdByWindow(IntPtr hWnd,out int tid)
        {
            Int32 ID;
            tid = GetWindowThreadProcessId(hWnd, out ID);
            return ID;
        }

    }

}
