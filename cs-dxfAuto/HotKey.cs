using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cs_dxfAuto {
    static class HotKey {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int key);
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetKeyState")]
        public static extern int GetKeyState(int nVirtKey);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        
        //主线程
        static private Thread thread;

        //结构体列表
        static private List<HotKeyEvent> funs = new List<HotKeyEvent>();

        //每次扫描间隔 默认为10
        static public Int32 sleepTime = 10;

        //绑定的窗口 如果为0 则为全局热键
        static public IntPtr bindWindow = (IntPtr)(-1);

        //获取按键状态
        static bool MyGetKeyState(Int32 keys) {
            return ((GetKeyState((int)keys) & 0x8000) != 0) ? true : false;
        }

        //判断按键是否相等
        static bool isKeyEquals(List<Keys> a, List<Keys> b) {
            if (a.Count != b.Count)
                return false;
            for (int i = 0; i < a.Count; i++) {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }

        //事件和按键列表的结构体
        struct HotKeyEvent {
            public HotKeyEvent(Action action, List<Keys> keys) {
                this.action = action;
                this.keys = keys;
            }
            public Action action;
            public List<Keys> keys;//按键列表
        }

        //判断是否按键的主线程
        static void mainThread() {
            bool[] keyState = new bool[255];
            bool[] lastkeyState = new bool[255];

            for (int i = 0; i < 255; i++)
                lastkeyState[i] = MyGetKeyState(i);

            while (true) {
                if (funs.Count == 0) {
                    Thread.Sleep(sleepTime);
                    continue;
                }
                for (int i = 0; i < 255; i++)
                    keyState[i] = MyGetKeyState(i);

                foreach (HotKeyEvent h in funs) {
                    bool flag = true;

                    if (h.keys.Count > 1) {
                        for (int i = 0; i < h.keys.Count - 1; i++) {
                            if (keyState[(Int32)h.keys[i]] == false)
                                flag = false;
                        }
                    } 
                    //else {
                    //    for (int i = 0; i < 255; i++) {
                    //        if (i == (Int32)h.keys[0])
                    //            continue;

                    //        if (keyState[i] == true) {
                    //            flag = false;
                    //            break;
                    //        }
                    //    }
                    //}
                    if (flag && !lastkeyState[(Int32)h.keys[h.keys.Count - 1]] && keyState[(Int32)h.keys[h.keys.Count - 1]] && (bindWindow == IntPtr.Zero ? true : (bindWindow == GetForegroundWindow())))
                        new Thread(new ThreadStart(h.action)).Start();
                }

                for (int i = 0; i < 255; i++)
                    lastkeyState[i] = keyState[i];
                Thread.Sleep(sleepTime);
            }
        }

        static public void initialize(Int32 time = 10) {
            thread = new Thread(mainThread);
            thread.Start();
            sleepTime = time;
        }

        //构造函数 参数1是监视热键的间隔时间 默认10s
        static public bool release() {
            if (thread == null)
                return false;
            thread.Abort();
            thread = null;
            return true;
        }

        static public void registetHotKey(List<Keys> keys, Action fun) {
            thread.Abort();
            funs.Add(new HotKeyEvent(fun, keys));
            thread = new Thread(mainThread);
            thread.Start();
        }

        static public void unRegistetHotKey() {

        }
        public enum HotKeyType {
            CallBackProc,//采用windows回调的方法实现，所有的热键消息由单独的回调函数处理
            Proc//此时这个类接管热键回调 热键的回调函数可以分布到各个线程来注册 最后集中到这个线程来处理。
        }
    }
}
