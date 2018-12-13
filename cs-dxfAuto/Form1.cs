using CallTool;
using cs_dxfAuto;
using SufeiUtil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Win32;


namespace cs_dxf {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(uint hProcess, uint lpBaseAddr, int[] lpBuffer, uint size, uint lpNumber);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(uint hProcess, uint lpBaseAddr, float[] lpBuffer, uint size, uint lpNumber);
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(uint hProcess, uint lpBaseAddr, int[] lpBuffer, uint size, uint lpNumber);

        [DllImport("kernel32.dll ")]
        public static extern bool CloseHandle(int hProcess);

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileStringA(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileStringA(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32.dll")]
        public static extern Int32 VirtualFreeEx(
int hprocess,
int lpaddress,
int dwsize,
int flallocationtype
);
        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        public static extern int GetWindowText(
int hwnd,
StringBuilder lpString,
int cch
);

        [DllImport("kernel32.dll")]
        public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);
        [DllImport("user32.dll", EntryPoint = "keybd_event")]

        public static extern void keybd_event(
        byte bVk, //虚拟键值  
        byte bScan,// 一般为0  
        int dwFlags, //这里是整数类型 0 为按下，2为释放  
        int dwExtraInfo //这里是整数类型 一般情况下设成为0  
        );
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
           IntPtr hWnd,                //要定义热键的窗口的句柄
           int id,                     //定义热键ID（不能与其它ID重复）           
           KeyModifiers fsModifiers,   //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效
           Keys vk                     //定义热键的内容
           );
        [DllImport("register.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void getResult_3(ref bool result);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd,                //要取消热键的窗口的句柄
            int id                      //要取消热键的ID
            );
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int key);

        [DllImport("user32.dll")]
        static extern byte MapVirtualKey(byte wCode, int wMap);

        [DllImport("user32")]
        static extern int SetForegroundWindow(IntPtr hwnd);
        [Flags()]
        public enum KeyModifiers {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        public static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);
        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]
        static extern IntPtr WindowFromPoint(System.Drawing.Point Point);
        [DllImport("user32.dll")]
        public static extern void SendMessageA(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern void PostMessageW(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("kernel32")]
        public static extern int GetModuleHandle(Int32 s);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern long SetWindowLongPtr(int hwnd, int index, long dwNewLong);


        public MemRWer gMrw;
        public AssemblyTools at;
        public Function fun;
        public MainThread mt;

        public void writeLogLine(string a) {
            // EDIT_Log.Text += a + "\r\n";
            EDIT_Log.AppendText(a + "\r\n");

            // EDIT_Log.SelectionStart = EDIT_Log.Text.Length;
            // EDIT_Log.ScrollToCaret();
        }

        public void completeJxTask() {
            GetTask(1);
            Thread.Sleep(500);
            foreach (ListViewItem lvi in listView1.Items) {
                string name = lvi.SubItems[2].Text;

                if (name.Contains("[镜像]")) {
                    string temp = lvi.SubItems[5].Text;
                    string[] d = temp.Split(',');
                    Int32[] ilist = new Int32[d.Length];
                    Int32 m = 0;
                    foreach (string s in d) {
                        ilist[m++] = Convert.ToInt32(s);
                    }
                    int count = ilist.Max();

                    for (int i = 0; i < count; i++) {
                        fun.CompletingQuest(Int32.Parse(lvi.SubItems[0].Text));
                        Thread.Sleep(100);
                    }
                    Thread.Sleep(100);
                    fun.CommittingQuest(Int32.Parse(lvi.SubItems[0].Text));
                }
            }
        }

        private void GetTask(Int32 qtype) {//0全部 1已接
            listView1.Items.Clear();
            Int32 qAddr;
            Int32 fAddr = 0;
            Int32 lAddr = 0;
            if (qtype == 0 || qtype == 1) {
                qAddr = gMrw.readInt32(baseAddr.dwBase_Quest);
                writeLogLine(qAddr.ToString());
                fAddr = gMrw.readInt32(qAddr + (qtype == 0 ? 0x68 : 0x8));
                lAddr = gMrw.readInt32(qAddr + (qtype == 0 ? 0x6C : 0xC));
            } else if (qtype == 2) {
                fAddr = gMrw.readInt32(baseAddr.dwBase_TiaoZhan);
                lAddr = gMrw.readInt32(baseAddr.dwBase_TiaoZhan + 4);
            }
            writeLogLine(fAddr.ToString());
            writeLogLine(lAddr.ToString());

            for (int i = fAddr; i < lAddr; i += (qtype == 0 ? 4 : 0xC)) {
                ListViewItem lvi = new ListViewItem();
                Int32 destAddr = gMrw.readInt32(i);
                Int32 type = gMrw.readInt32(destAddr + 0x134);
                lvi.Text = gMrw.readInt32(destAddr).ToString();
                lvi.SubItems.Add(Convert.ToString(destAddr, 16));
                Int32 Lenth = gMrw.readInt32(destAddr + 0x18);
                string name = "";
                if (Lenth <= 7)
                    name = gMrw.readString(destAddr + 0x8);
                else
                    name = gMrw.readString(gMrw.readInt32(destAddr + 0x08));
                lvi.SubItems.Add(name);
                lvi.SubItems.Add(gMrw.readString(gMrw.readInt32(destAddr + 0x2D0)));

                string sType = "未知";
                switch (type) {
                    case 0: {
                            sType = "主线";
                            break;
                        }
                    case 1: {
                            sType = "修炼";
                            break;
                        }
                    case 2: {
                            sType = "成就";
                            break;
                        }
                    case 3: {
                            sType = "每日";
                            break;
                        }
                    case 4: {
                            sType = "重复";
                            break;
                        }
                    case 5: {
                            sType = "普通";
                            break;
                        }
                    case 10: {
                            sType = "其他";
                            break;
                        }
                    case 11: {
                            sType = "子任务";
                            break;
                        }
                    case 12: {
                            sType = "系统";
                            break;
                        }
                    case 13: {
                            sType = "外传";
                            break;
                        }
                    case 14: {
                            sType = "镜像";
                            break;
                        }
                    case 15: {
                            sType = "循环";
                            break;
                        }
                    case 16: {
                            sType = "挑战";
                            break;
                        }
                    default: {
                            sType = type.ToString();
                            break;
                        }
                }

                //lvi.SubItems.Add(gMrw.Decryption(destAddr + 4).ToString());
                lvi.SubItems.Add(sType);
                if (qtype == 0)
                    lvi.SubItems.Add("null");
                else {
                    Int32 count = gMrw.Decryption(i + 4);
                    string ctemp = "";
                    while (count >= 512) {
                        ctemp += count % 512;
                        count /= 512;
                        ctemp += ",";
                    }
                    ctemp += count;
                    lvi.SubItems.Add(ctemp);

                }
                listView1.Items.Add(lvi);
            }
        }

        public void writeYwLsLogLine(string text) {
        }

        public void writeYwChLogLine(string text) {
            // EDIT_Yw_Ch.Text +=  LBL_Name.Text + ":"+ text + "\r\n";
        }

        public void setCharaInfomation() {
            //LBL_Qq.Text = ((uint)gMrw.readInt32(baseAddr.dwBase_Qq)).ToString();
        }

       

        void load(int hWnd = 0) {
            check();

            if (gMrw != null)
                CloseHandle(gMrw.GetHandle());
            writeLogLine("开始初始化辅助");

            Process a = new Process();
            Int32 hProcess = 0;
            Int32 Pid;
            if (hWnd == 0)
                hWnd = (Int32)Process.FindWindow("地下城与勇士", "地下城与勇士");
            int Tid;
            //hProcess = a.OpenProcessByWindow((IntPtr)hWnd);
            Pid = a.GetProcessIdByWindow((IntPtr)hWnd,out Tid);


            //if (hProcess == 0 || hProcess == -1) {
            //    EDIT_Log.Text += "打开进程失败" + "\n";
            //    return;
            //}
            writeLogLine("初始化成功 进程句柄" + Pid);
            writeLogLine("初始化成功 线程句柄" + Tid);

            gMrw = new MemRWer((uint)hProcess, Pid);
            gMrw.pid = Pid;
            gMrw.tid = Tid;

            at = new AssemblyTools(gMrw.GetHandle(), 0, gMrw, writeLogLine);
            
            fun = new Function(at, gMrw, writeLogLine);
            at.Asm_SetWindowsLong(writeLogLine);
            //at.Asm_SetTimer(writeLogLine);


            KeyEvent.fun = fun;
            KeyEvent.gMrw = gMrw;
            HotKey.bindWindow = (IntPtr)hWnd;

            //at.clear();
            //at.pushad();
            //gMrw.writeString(at.GetVirtualAddr() + 0xD00, "GameRpcs.dll");
            //at.push(at.GetVirtualAddr() + 0xD00);
            //at.mov_eax(at.GetModuleHandleW);
            //at.call_eax();
            //at.mov_xxx_eax(at.GetVirtualAddr() + 0xD00);
            //at.popad();
            //at.retn();
            //at.RunRempteThreadWithMainThread();

            //baseAddr.GameRpcs = gMrw.readInt32(at.GetVirtualAddr() + 0xD00);
            //writeLogLine("GameRpcs = 0x" + Convert.ToString(baseAddr.GameRpcs, 16));


            //if(m_thread == null)
            //{
            //    m_thread = new Thread(monite_thread);
            //    m_thread.Start();
            //}
        }

        private void BN_Load_Click(object sender, EventArgs e) {
            load();
            //MyKey.MKeyUp();
        }

        private void BN_Tset_Click(object sender, EventArgs e) {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {

            if (Process.FindWindow("地下城与勇士", "地下城与勇士").ToInt32() != 0 && gMrw != null) {
                int mhProcess = gMrw.GetHandle();

                //at.asm_KillTimer();
                //VirtualFreeEx(mhProcess, at.GetVirtualAddr(), 0, 0x00008000);
                CloseHandle(mhProcess);
            }
            //this.Close();
            TerminateProcess(IntPtr.Add(IntPtr.Zero, -1), 0);
            //MyKey.MKeyUp();
        }

        public void WriteInAlgorithm(int writtenAddress, int writtenValue) {

        }

        private void button1_Click(object sender, EventArgs e) {
            //mt.StartMS();
            //fun.TzYc();
            //mt.StartJL();
            Int32 ePoint = fun.CreateEquit(100300666);
            writeLogLine(Convert.ToString(ePoint, 16));

            Int32 head = gMrw.readInt32(ePoint + 0x10E4);
            Int32 cPoint = gMrw.readInt32(baseAddr.dwBase_Character);

            head = gMrw.readInt32(head + 0x1C, 0x50, 0x4E8);
            gMrw.writeInt32(gMrw.readInt32(head + 4, 4) + 4, 2000);//CD
            gMrw.writeInt32(gMrw.readInt32(head + 4, 0x18) + 0, 31);//触发
            gMrw.writeInt32(gMrw.readInt32(head + 0x40, 0x2C) + 4, 1000);//频率
            gMrw.writeFloat(gMrw.readInt32(head + 0x54, 0x4, 0x18) + 8, (float)49004.0);//频率


            for (Int32 i = 0x2CE4; i <= 0x2CEC; i += 4) {
                if (gMrw.readInt32(cPoint + i) == 0) {
                    MessageBox.Show("请穿戴三件首饰");
                    return;
                }
                gMrw.Encryption(gMrw.readInt32(cPoint + i) + 0x10F8, 12556);
                gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0x1108, 1);
            }
            //fun.CreateSkill(0, 100);
            //gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character, 0x5C18, 0x58, 0xC) + 0x4EC, 0x100100);
            //gMrw.writeInt32(0x100100, 15407);
            //gMrw.writeInt32(0x100104, -1);
            //gMrw.writeInt32(0x100108, -1);
            //gMrw.writeInt32(0x10010C, 456456);
            //gMrw.writeInt32(0x100110, 456);
            //gMrw.writeInt32(0x100114, 1);
            //fun.UseItem(0);
            //Thread.Sleep(1000);
            //gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character, 0x5C18, 0x58, 0xC) + 0x4EC, 0x100100);
            //Int32 Addr = 0;
            //while ((Addr = fun.GetAddressByName("团员")) == 0)
            //    ;

            //fun.Encryption(gMrw.readInt32(baseAddr.dwBase_Character) + baseAddr.dwOffset_Equip_wq, 0);
            //fun.HideCall(Addr);
            //fun.CheckSkill(Addr, 16, 3, 1, 6, 11, 80000);
        }

        private void button2_Click(object sender, EventArgs e) {
        }
        Int32 wd = 0;

        const int WM_PICKUP = 0x1000;
        const int WM_ENTERINSTANCETRUE = 0x1001;
        const int WM_GETPACKAGE = 0x1004;


        protected override void DefWndProc(ref Message m) {
            try {
                const int WM_HOTKEY = 0x0312;
                //按快捷键 
                switch (m.Msg) {
                    
                    case WM_HOTKEY:
                        switch (m.WParam.ToInt32()) {
                            case (Int32)Keys.V: {
                                    fun.PickUp();
                                    break;
                                }
                            case (Int32)Keys.F1: {
                                    fun.MouseTp();
                                    writeLogLine(Int32.Parse(textBox2.Text).ToString());
                                    break;
                                }
                            case (Int32)Keys.Up: {
                                    if (checkBox8.Checked)
                                        fun.InstanceTp(0);
                                    else
                                        fun.CoorTp(0);
                                    break;
                                }
                            case (Int32)Keys.Down: {

                                    if (checkBox8.Checked)
                                        fun.InstanceTp(1);
                                    else
                                        fun.CoorTp(1);
                                    break;
                                }
                            case (Int32)Keys.Left: {
                                    if (checkBox8.Checked)
                                        fun.InstanceTp(2);
                                    else
                                        fun.CoorTp(2);
                                    break;
                                }
                            case (Int32)Keys.Right: {
                                    if (checkBox8.Checked)
                                        fun.InstanceTp(3);
                                    else
                                        fun.CoorTp(3);
                                    break;
                                }
                            case (Int32)Keys.F2: {
                                    textBox34.Text = gMrw.readInt32(baseAddr.dwBase_Mouse, 0x20).ToString();
                                    break;
                                }
                            case (Int32)Keys.F3: {
                                    fun.checkEmery(110110);
                                    Thread.Sleep(100);
                                    writeLogLine("杀死怪物");
                                    break;
                                }
                            case (Int32)Keys.F6: {
                                    writeLogLine(gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Mouse) + 0xF8C).ToString());

                                    //textBox1.Text = gMrw.readInt32(0x4BC9D0C).ToString();
                                    //writeLogLine("当前坐标:x=" + fun.GetCurrencyRoom().X + " y=" + fun.GetCurrencyRoom().Y);
                                    //for (int i = 0; i < 0x7000; i += 4)
                                    //{
                                    //    writeYjLine(Convert.ToString(i,16) + " = " + gMrw.Decryption(addr + i));

                                    //}
                                    //SellGoal();
                                    //GetRoleEquit();
                                    break;
                                }
                            case (Int32)Keys.F5: {
                                    writeLogLine("地图id:" + gMrw.readInt32(0x4BC9D0C).ToString());
                                    writeLogLine("当前坐标:x=" + fun.GetCurrencyRoom().X + " y=" + fun.GetCurrencyRoom().Y);
                                    break;
                                }

                            case (Int32)Keys.F8: {
                                    textBox5.Text = gMrw.Decryption(gMrw.readInt32(0x4371EBC) + 0x6C).ToString();
                                    break;
                                }
                            case (Int32)Keys.Oem3: {
                                    gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0x376C, gMrw.readInt32(baseAddr.dwBase_Character, 0x3ABC) + 1000);
                                    gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0x3770C, 1000);

                                    //mt.PickUp();
                                    //gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character, 0x6058, 0x58, 0xC) + 0x20, 490004248);
                                    break;
                                }
                            case (Int32)Keys.End: {
                                    fun.QuitInstance();
                                    break;

                                }

                            case (Int32)Keys.Delete: {
                                    GetEquipTz(0);
                                    break;

                                }
                            case (Int32)Keys.PageUp: {
                                    writeLogLine("1");
                                    fun.MouseTp();
                                    break;

                                }
                            case (Int32)Keys.Insert: {
                                    GetEquip();
                                    break;

                                }

                            case (Int32)Keys.Home: {

                                    Int32 IsOpen = gMrw.readInt32(baseAddr.dwBase_Character, 0x918);
                                    Int32 _base = gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq + 0x10);
                                    if (_base == 0) {
                                        MessageBox.Show("必须穿戴腰带");
                                        break;
                                    }

                                    if (IsOpen == 0) {
                                        fun.EncryptionCall(_base + 0x000009D8, 1000);
                                        fun.EncryptionCall(_base + 0x000009E0, 1000);
                                        fun.EncryptionCall(_base + 0x000009E8, 1000);

                                        gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0x918, 1);
                                        gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0xA04, 1);

                                    } else {
                                        fun.EncryptionCall(_base + 0x000009D8, 0);
                                        fun.EncryptionCall(_base + 0x000009E0, 0);
                                        fun.EncryptionCall(_base + 0x000009E8, 0);

                                        gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0x918, 0);
                                        gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0xA04, wd);
                                    }



                                    break;
                                }
                        }
                        break;
                    case WM_GETPACKAGE:
                        {
                            getPackageHook((int)m.LParam);
                            break;
                        }
                    case WM_ENTERINSTANCETRUE:
                        {
                            fun.EnterInstanceTrue();
                            break;
                        }
                    case 0x1007:
                        {
                            writeLogLine("1");
                            
                            break;
                        }


                }
                base.DefWndProc(ref m);
            } catch {
            }
        }

        Int32 currentGole;
        public Int32 twice = 1;
        Int32 Goles = 0;
        Int32 tz_num = 0;
        Int32 tz_get_num = 0;


        public void GetIncomeBegin() {
            currentGole = fun.GetCharaGole();
        }

        public void GetNBIncomeBegin() {
            currentGole = fun.GetCharaGole();
            tz_num = fun.GetItemNum("深渊派对挑战书");
        }

        public void GetIncome() {
            if ((fun.GetCharaGole() - currentGole) != 0) {
                EDIT_Get.AppendText("当前刷图次数 :" + twice++ + "总收益 :" + (Goles += (fun.GetCharaGole() - currentGole)) + "\n");
                //EDIT_Get.AppendText("本次收益 :" + (fun.GetCharaGole() - currentGole) + "\n");
            }
        }

        public void GetNBIncome() {
            if ((fun.GetCharaGole() - currentGole) != 0) {
                EDIT_Get.AppendText("当前南部次数 :" + twice++ + "\n");
                EDIT_Get.AppendText("本次收益 :" + (fun.GetCharaGole() - currentGole) + "\n");
                EDIT_Get.AppendText("获得挑战 :" + (fun.GetItemNum("深渊派对挑战书") - tz_num) + "\n");
                EDIT_Get.AppendText("总收益 :" + (Goles += (fun.GetCharaGole() - currentGole)) + "金币   " + "挑战书：" + (tz_get_num += (fun.GetItemNum("深渊派对挑战书") - tz_num)) + "\n");
            }
        }

        private void GetRoleEquit() {

            EDIT_RO_Equit.Text = "";
            for (int i = 0; i < 1000; i += 4)
                EDIT_RO_Equit.AppendText(Convert.ToString(i + Int32.Parse(textBox4.Text), 16) + "=" + gMrw.Decryption(Int32.Parse(EDIT_RO_Name.Text) + i + Int32.Parse(textBox4.Text)) + "\r\n");


            //Int32 addr = fun.GetAddressByName(EDIT_RO_Name.Text);
            //EDIT_RO_Equit.AppendText("人偶地址：" + Convert.ToString(addr, 16) + "\n");
            //for (Int32 i = 0x2CC8; i <= 0x2CF0; i += 4) {
            //    if (gMrw.readInt32(addr + i) == 0)
            //        continue;

            //    EDIT_RO_Equit.AppendText(gMrw.readString(gMrw.readInt32(addr + i, 0x24)) + "\n");
            //    EDIT_RO_Equit.AppendText("套装代码：" + gMrw.Decryption(gMrw.readInt32(addr + i) + 0x10F8).ToString() + "\n");
            //    EDIT_RO_Equit.AppendText("装备代码：" + gMrw.readInt32(gMrw.readInt32(addr + i) + 0x20).ToString() + "\n");

            //}
        }

        private void writeYjLine(string t) {
            textBox3.AppendText(t + "\r\n");
        }

        private void GetYjEquip() {
            if (gMrw.readInt32(baseAddr.dwBase_Mouse) == 0) {
                EDIT_Tz.AppendText("请将鼠标对准装备" + "\n");
            }

            //writeYjLine("[装备代码] $ " + gMrw.readInt32(baseAddr.dwBase_Mouse, 0x20));
            //writeYjLine("[套装代码] $ " + gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Mouse) + 0x10F8));



            //Int32 head = gMrw.readInt32(baseAddr.dwBase_Mouse, 0x10E4);
            //head = gMrw.readInt32(head + 0x1C, 0x50);

            //Int32 start = gMrw.readInt32(head + 0x480);
            //Int32 end = gMrw.readInt32(head + 0x438);

            Int32 start = gMrw.readInt32(baseAddr.dwBase_Mouse, 0xAA8);
            Int32 end = gMrw.readInt32(baseAddr.dwBase_Mouse, 0xAAC);
            writeYjLine("[装备代码] $ " + gMrw.readInt32(baseAddr.dwBase_Mouse, 0x20));
            writeLogLine("指针 : " + Convert.ToString(fun.CreateEquit(gMrw.readInt32(baseAddr.dwBase_Mouse, 0x20)), 16));

            for (Int32 temp = start; temp < end; temp += 40) {
                writeYjLine("开始");
                writeYjLine("0 = " + gMrw.readInt32(temp).ToString());
                writeYjLine("4 = " + gMrw.Decryption(temp + 4).ToString());
                writeYjLine("12 = " + gMrw.readInt32(temp + 12).ToString());
                writeYjLine("16 = " + gMrw.Decryption(temp + 16).ToString());
                writeYjLine("24 = " + gMrw.Decryption(temp + 24).ToString());
                gMrw.Encryption(temp + 24, 10000);
                writeYjLine("32 = " + gMrw.readInt32(temp + 32).ToString());
                writeYjLine("36 = " + gMrw.readInt32(temp + 36).ToString());
                writeYjLine("结束");
                writeYjLine("");

            }
        }

        private void GetEquip() {
            if (gMrw.readInt32(baseAddr.dwBase_Mouse) == 0) {
                EDIT_Tz.AppendText("请将鼠标对准装备" + "\n");
            }

            EDIT_Tz.AppendText("[装备代码] $ " + gMrw.readInt32(baseAddr.dwBase_Mouse, 0x20) + "\n");

            Int32 start = gMrw.readInt32(baseAddr.dwBase_Mouse, 0xB7C);
            Int32 end = gMrw.readInt32(baseAddr.dwBase_Mouse, 0xB7C + 4);

            EDIT_Tz.AppendText((end - start).ToString() + "\n");

            Int32 tx = (end - start) / 60;

            EDIT_Tz.AppendText("特效数量 = " + tx + "\n" + "\n");

            for (Int32 i = 0; i < tx; i++) {
                Int32 is_ = gMrw.readInt32(start + i * 60 + 0x04);
                Int32 ie = gMrw.readInt32(start + i * 60 + 0x08);

                Int32 coun = (ie - is_) / 0x14;
                EDIT_Tz.AppendText("整数段数量 " + coun + "\n");

                for (Int32 j = 0; j < coun; j++) {
                    int js = gMrw.readInt32(is_ + j * 0x14 + 0x04);
                    int je = gMrw.readInt32(is_ + j * 0x14 + 0x08);
                    int coum = (je - js) / 0x04;

                    EDIT_Tz.AppendText("整数段 " + j + "\n");

                    for (Int32 k = 0; k < coum; k++) {
                        int v = gMrw.readInt32(js + k * 0x04);

                        int t = gMrw.readInt32(js);
                        EDIT_Tz.AppendText("B30" + "+" + Convert.ToString(i * 60 + 0x04, 16) + "+" + Convert.ToString(j * 0x14 + 0x04, 16) + "+" + Convert.ToString(k * 0x04, 16) + "=" + v + "\n");
                    }
                }


                is_ = gMrw.readInt32(start + i * 60 + 0x18);
                ie = gMrw.readInt32(start + i * 60 + 0x1C);
                int coub = (ie - is_) / 0x14;
                EDIT_Tz.AppendText("小数数量 " + coub + "\n" + "\n");
                if (coub == 0)
                    coub = 1;
                for (int j = 0; j < coub; j++) {
                    int js = gMrw.readInt32(is_ + j * 0x14 + 0x04);
                    int je = gMrw.readInt32(is_ + j * 0x14 + 0x08);
                    int coun_ = (je - js) / 0x14;
                    for (int k = 0; k < coun_; k++) {
                        int ls = gMrw.readInt32(js + k * 0x14 + 0x04);
                        int le = gMrw.readInt32(js + k * 0x14 + 0x08);
                        int coum = (le - ls) / 0x04;
                        for (int l = 0; l < coum; l++) {
                            int value = (int)gMrw.readFloat(ls + l * 0x04);
                            int t = (int)gMrw.readFloat(ls);
                            EDIT_Tz.AppendText("B30" + "+" + Convert.ToString(i * 60 + 0x18, 16) + "+" + Convert.ToString(j * 0x14 + 0x04, 16) + "+" + Convert.ToString(k * 0x14 + 0x04, 16) + "+" + l * 0x04 + " = " + value + "\n");
                        }
                    }
                }
            }
        }

        private void GetEquipTz(Int32 offset) {
            if (gMrw.readInt32(baseAddr.dwBase_Mouse) == 0) {
                EDIT_Tz.AppendText("请将鼠标对准装备" + "\n");
            }

            EDIT_Tz.AppendText("[装备代码] $ " + gMrw.readInt32(baseAddr.dwBase_Mouse, 0x20) + "\n");
            EDIT_Tz.AppendText("[套装代码] $ " + gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Mouse) + 0xF8C) + "\n");

            Int32 start = gMrw.readInt32(baseAddr.dwBase_Mouse, 0xF78, 0xC, 0xC, Int32.Parse(EDIT_TZ_Offset.Text) * 4, 0x4E8 + 0x44);
            Int32 end = gMrw.readInt32(baseAddr.dwBase_Mouse, 0xF78, 0xC, 0xC, Int32.Parse(EDIT_TZ_Offset.Text) * 4, 0x4E8 + 0x44 + 4);

            EDIT_Tz.AppendText((end - start).ToString() + "\n");

            Int32 tx = (end - start) / 60;

            EDIT_Tz.AppendText("特效数量 = " + tx + "\n" + "\n");

            for (Int32 i = 0; i < tx; i++) {
                Int32 is_ = gMrw.readInt32(start + i * 60 + 0x04);
                Int32 ie = gMrw.readInt32(start + i * 60 + 0x08);

                Int32 coun = (ie - is_) / 0x14;
                EDIT_Tz.AppendText("整数段数量 " + coun + "\n");

                for (Int32 j = 0; j < coun; j++) {
                    int js = gMrw.readInt32(is_ + j * 0x14 + 0x04);
                    int je = gMrw.readInt32(is_ + j * 0x14 + 0x08);
                    int coum = (je - js) / 0x04;

                    EDIT_Tz.AppendText("整数段 " + j + "\n");

                    for (Int32 k = 0; k < coum; k++) {
                        int v = gMrw.readInt32(js + k * 0x04);

                        int t = gMrw.readInt32(js);
                        EDIT_Tz.AppendText("10E4+1C+50+4E8" + "+" + Convert.ToString(i * 60 + 0x04, 16) + "+" + Convert.ToString(j * 0x14 + 0x04, 16) + "+" + Convert.ToString(k * 0x04, 16) + "=" + v + "\n");
                    }
                }


                is_ = gMrw.readInt32(start + i * 60 + 0x18);
                ie = gMrw.readInt32(start + i * 60 + 0x1C);
                int coub = (ie - is_) / 0x14;
                EDIT_Tz.AppendText("小数数量 " + coub + "\n" + "\n");
                if (coub == 0)
                    coub = 1;
                for (int j = 0; j < coub; j++) {
                    int js = gMrw.readInt32(is_ + j * 0x14 + 0x04);
                    int je = gMrw.readInt32(is_ + j * 0x14 + 0x08);
                    int coun_ = (je - js) / 0x14;
                    for (int k = 0; k < coun_; k++) {
                        int ls = gMrw.readInt32(js + k * 0x14 + 0x04);
                        int le = gMrw.readInt32(js + k * 0x14 + 0x08);
                        int coum = (le - ls) / 0x04;
                        for (int l = 0; l < coum; l++) {
                            int value = (int)gMrw.readFloat(ls + l * 0x04);
                            int t = (int)gMrw.readFloat(ls);
                            EDIT_Tz.AppendText("10E4+1C+50+4E8" + "+" + Convert.ToString(i * 60 + 0x18, 16) + "+" + Convert.ToString(j * 0x14 + 0x04, 16) + "+" + Convert.ToString(k * 0x14 + 0x04, 16) + "+" + l * 0x04 + " = " + value + "\n");
                        }
                    }
                }
            }
        }

        //public bool IsTzYc() {
        //    return RB_2_TzYc.Checked == true;
        //}

        //public bool IsFreeze() {
        //    return RB_2_Freeze.Checked == true;
        //}

        //public bool IsCreateSkill() {
        //    return RB_2_CreateSkill.Checked == true;
        //}

        Int32 FunctionID_1 = 3;

        public Int32 GetClearMapFunction() {
            return FunctionID_1;
        }

        public void renouLoad() {
        }

        public void SellGoal() {
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);

            Int32 first = gMrw.readInt32(bag + 88);
            Int32 iteam = first + 36;

            for (Int32 i = 0; i < 56 * 5; i++) {
                Int32 point = gMrw.readInt32(iteam + i * 4);
                if (point != 0) {
                    Int32 iteam_l = gMrw.readInt32(point + 0x160);
                    string name = gMrw.readString(gMrw.readInt32(point + 0x24));
                    Int32 count = gMrw.readInt32(point + 0x2D4);
                    if (name.IndexOf("脏兮兮") >= 0) {
                        writeLogLine("出售 " + name + count + "个");
                        fun._Sell(i + 9, count);
                    }
                }
            }
            Thread.Sleep(1500);
            writeLogLine("存入金币:" + fun.GetCharaGole());
            fun.GoleEnterBag(fun.GetCharaGole());
        }

        DateTime getTime() {
            try {
                //WWW www = new WWW("http://www.hko.gov.hk/cgi-bin/gts/time5a.pr?a=1");
                //yield return www;
                //if (www.text != null)
                //{
                //    string TimeString = www.text;
                //    string time = TimeString.Substring(2);//截取从第三个到最后一个  
                //    System.DateTime dtStart = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                //    long lTime = long.Parse(time);
                //    System.TimeSpan toNow = new System.TimeSpan(lTime);
                //    System.DateTime timeNow_FromNet = dtStart.Add(toNow);
                //}

                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                Byte[] pageData = MyWebClient.DownloadData("http://119.23.8.232/1.php"); //从指定网站下载数据
                string pageHtml = Encoding.UTF8.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句 
                string time = pageHtml;//截取从第三个到最后一个  
                System.DateTime dtStart = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                long lTime = long.Parse(time + "0000000");
                System.TimeSpan toNow = new System.TimeSpan(lTime);
                System.DateTime timeNow_FromNet = dtStart.Add(toNow);
                return timeNow_FromNet;
            } catch {
                MessageBox.Show("服务器故障");
                string time = "9999-12-31 00:00:00";
                DateTime d = DateTime.Parse(time);
                return d;
            }


        }

        bool checkTime() {
            DateTime mb_time = DateTime.Parse("2018-3-8 10:00");
            DateTime dt = getTime();
            if (DateTime.Compare(dt, mb_time) < 0)
                return true;
            return false;
        }




        [DllImport("Kernel32.dll", EntryPoint = "GetProcAddress", SetLastError = true)]
        public static extern int GetProcAddress(int handle, String funcname);
        [DllImport("Kernel32.dll", EntryPoint = "LoadLibraryA", SetLastError = true)]
        public static extern int LoadLibraryA(String funcname);
        [DllImport("Kernel32")]
        public static extern int FreeLibrary(int handle);

        public bool IsGropeHelper() {
            return false;
        }

        public bool IsQzTp() {
            return checkBox8.Checked == true;
        }

        public bool IsMainCharacter() {
            return false;
        }

        public Int32 GetJlNum() {
            return Int32.Parse(textBox18.Text);
        }


        public delegate bool _2();

        public Int32 GetSleepTime() {
            return Int32.Parse(textBox19.Text);
        }

        public bool IsEM() {
            return false;
        }

        public bool IsJbl() {
            return false;
        }

        public void writeGetLine(string s) {
            EDIT_Get.AppendText(s + "\r\n");
        }

        public void writeSpacilLine(string s) {
            EDIT_Get.AppendText(s + "\r\n");
        }

        public bool IsStopWithGobarli() {
            // return checkBox1.Checked;
            return false;
        }

        public bool IsFastGetCard() {
            return checkBox5.Checked;
        }


        public bool IsFastKill() {
            return checkBox6.Checked;
        }

        public Int32 getTargetLevel() {
            return Int32.Parse(textBox9.Text);
        }

        public bool IsUnlimitedWeight() {
            return checkBox10.Checked;
        }

        public bool IsGetGoleCard() {
            return checkBox11.Checked;
        }

        public bool IsNightModule() {
            return checkBox12.Checked;
        }

        string GetRandStr(int num) {
            Random r = new Random(Win32.Kernel.GetTickCount() % 10086);

            string s = "";

            for (int i = 0;i < num; i++) {
                s += (char)r.Next(0x41,0x5A); ;
            }
            return s;
        }

        public void getMouseCode() {
            textBox34.Text = gMrw.readInt32(baseAddr.dwBase_Mouse, 0x20).ToString();
        }

        Byte[] key = new Byte[] { 0xCC, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x31, 0x32, 0x33, 0x34, 0x35 };

        void check() {
            if (File.Exists("GoGoGOSSS.txt"))
                return;

            string rStr = GetRandStr(5);

            Console.WriteLine(key.Length);
            string e_result = StdAes.AesEncrypt(FingerPrint.GetHash(FingerPrint.biosId() + FingerPrint.macId()) + rStr, key);

            e_result = e_result.Replace('+', '_');
            e_result = e_result.Replace('/', '-');

            HttpHelper r = new HttpHelper();
            HttpItem h = new HttpItem {
                URL = "http://119.23.8.232/Get/GetInfo.php",
                Method = "POST",
                Postdata = "m=" + e_result,
                ContentType = "application/x-www-form-urlencoded",
            };

            HttpResult hr = r.GetHtml(h);
            string rm = hr.Html.ToString();
            rm = rm.Substring(1, rm.Length - 1);

            //rm = rm.Substring(0, rm.Length - 1);

            string r_string = StdAes.AesDecrypt(rm, key);

            char state = r_string[0];
            string randomStr = r_string.Substring(1, r_string.Length - 1);


            if (randomStr != rStr) {
                MessageBox.Show("校验和失效");
                TerminateProcess(IntPtr.Subtract(IntPtr.Zero, 1), 0);
                return;
            }

            string notice = "验证失败:";

            if (state != '2') {
                if (state == '0') {
                    notice += "此机器尚未开通";
                }
                if (state == '1') {
                    notice += "此机器已经到期";
                }
                MessageBox.Show(notice);
                TerminateProcess(IntPtr.Subtract(IntPtr.Zero, 1), 0);
                return;
            }
        }

        public string getTextBox1Text()
        {
            string result = "";
            result += radioButton_Up_1.Checked ? "0" : (radioButton_Up_2.Checked ? "1" : "2");
            result += radioButton_down_1.Checked ? " 0" : (radioButton_down_2.Checked ? " 1" : " 2");
            result += radioButton_left_1.Checked ? " 0" : (radioButton_left_2.Checked ? " 1" : " 2");
            result += radioButton_right_1.Checked ? " 0" : (radioButton_right_2.Checked ? " 1" : " 2");


            return result;
        }
        [DllImport("Dll.dll")]
        public static extern int protect();
        [DllImport("Dll.dll")]
        public static extern int promoteHandle(int handle,int access);
        [DllImport("Dll.dll")]
        public static extern int unlinkHandleTable();
        [DllImport("Dll.dll")]
        public static extern int setPid(int pid);

        private void Form1_Load(object sender, EventArgs e) {
            HttpHelper r = new HttpHelper();
            HttpItem h = new HttpItem
            {
                URL = "http://119.23.8.232/test.php",
                Method = "get",
                ContentType = "application/x-www-form-urlencoded",
            };
            //MTIzNDU2
            //MTIzNDU2
            HttpResult hr = r.GetHtml(h);

            string rm = hr.Html;
            rm = rm.Substring(1, rm.Length - 1);

            Byte[] toEncryptArray;
            //MTIzNDVmZHNmZHM0NWdmZGJ2YzY=
            //MTIzNDVmZHNmZHM0NWdmZGJ2YzY=
            //rm = Regex.Replace(rm, @"[^/x21-/x7E]", "");
            //rm.Replace(s = )

            if (rm == "﻿MTIzNDVmZHNmZHM0NWdmZGJ2YzY=")
                toEncryptArray = Convert.FromBase64String("MTIzNDVmZHNmZHM0NWdmZGJ2YzY=");


            if (Win32.Kernel.LoadLibrary("WinIo32.dll") == 0){
                writeLogLine("加载winio失败,将不使用WINIO模拟按键");
            }
            else
            {
                try
                {
                    WinIo.Initialize();
                    writeLogLine("加载winio成功");
                }
                catch
                {

                }

            }



            try
            {
                MyKey.MGetKeyDev();
            }
            catch
            {
                writeLogLine("加载虚拟按键失败");
            }

            if (Win32.Kernel.LoadLibrary("Dll.dll") == 0)
            {
                writeLogLine("加载保护系统失败");

            }
            else
            {

                int status = protect();
                if (status >= 0)
                {
                    writeLogLine("保护进程成功");
                }
                else
                {
                    writeLogLine("进程保护失败:" + status);
                }
            }
            //}
            //catch
            //{
            //    writeLogLine("保护进程异常 可能是找不到dll");
            //}
            //int id = Win32.Kernel.GetCurrentProcessId();
            //System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
            //info.FileName = @"dll.exe";
            //info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            //info.Arguments = id.ToString();
            //System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);
            //process.WaitForExit();
            //writeLogLine(process.ExitCode.ToString());

            Config.sss = checkBox12.Checked;
            Config.isHook = C17.Checked;
            Config.IsHide = checkBox7.Checked;

            Kernel.InitializeCriticalSection(ref Config.CS);
            Control.CheckForIllegalCrossThreadCalls = false;
            KeyEvent.fm1 = this;
            Config.Init();

            check();
            //new Thread(DealQueue).Start();

            //button50.Visible = true;
            this.comboBox4.SelectedIndex = 0;

            mt = new MainThread(this);
            this.listView1.FullRowSelect = true;//是否可以选择行
            this.listView1.GridLines = true; //显示表格线
            this.listView2.FullRowSelect = true;//是否可以选择行
            this.listView2.GridLines = true; //显示表格线

            RB_Accept.Checked = true;
            bool[] result = new bool[10];

            HotKey.initialize();
            HotKey.registetHotKey(new List<Keys> { Keys.F1 }, KeyEvent.F1);
            HotKey.registetHotKey(new List<Keys> { Keys.F2 }, KeyEvent.F2);
            HotKey.registetHotKey(new List<Keys> { Keys.F3 }, KeyEvent.F3);
            HotKey.registetHotKey(new List<Keys> { Keys.F4 }, KeyEvent.F4);
            HotKey.registetHotKey(new List<Keys> { Keys.F5 }, KeyEvent.F5);
            //HotKey.registetHotKey(new List<Keys> { Keys.F8 }, KeyEvent.F8);

            HotKey.registetHotKey(new List<Keys> { Keys.B }, KeyEvent.B);

            HotKey.registetHotKey(new List<Keys> { Keys.Home }, KeyEvent.Home);
            HotKey.registetHotKey(new List<Keys> { Keys.End }, KeyEvent.End);
            HotKey.registetHotKey(new List<Keys> { Keys.V }, KeyEvent.V);

            HotKey.registetHotKey(new List<Keys> { Keys.ControlKey, Keys.PageUp }, KeyEvent.PageUp);


            HotKey.registetHotKey(new List<Keys> { Keys.ControlKey, Keys.Up }, KeyEvent.Up);
            HotKey.registetHotKey(new List<Keys> { Keys.ControlKey, Keys.Down }, KeyEvent.Down);
            HotKey.registetHotKey(new List<Keys> { Keys.ControlKey, Keys.Left }, KeyEvent.Left);
            HotKey.registetHotKey(new List<Keys> { Keys.ControlKey, Keys.Right }, KeyEvent.Right);



            Int32 m = 0;

            MapI[] temp = new MapI[9];
            temp[0] = new MapI("幽暗密林", 3, 1);
            temp[1] = new MapI("雷鸣废墟", 5, 3);
            temp[2] = new MapI("猛毒雷鸣废墟", 6, 5);
            temp[3] = new MapI("冰霜幽暗密林", 9, 8);
            temp[4] = new MapI("格拉卡", 7, 11);
            temp[5] = new MapI("烈焰格拉卡", 8, 13);
            temp[6] = new MapI("暗黑雷鸣废墟", 1000, 14);
            temp[7] = new MapI("比尔马克帝国试验场", 240, 14, MapI.MapType.Normal, false);
            temp[8] = new MapI("幽暗密林", 7164, 14, MapI.MapType.Normal, false);

            MapInfo.mapInfo[m++] = new MapInfomation("格兰之森", 38, 2, 850, 300, 1, 16, 0, temp);


            temp = new MapI[6];
            temp[0] = new MapI("龙人之塔", 10, 17);
            temp[1] = new MapI("人偶玄关", 12, 19);
            temp[2] = new MapI("石巨人塔", 13, 20);
            temp[3] = new MapI("黑暗玄廊", 14, 21);
            temp[4] = new MapI("悬空城", 17, 22);
            temp[5] = new MapI("城主宫殿", 15, 23);
            MapInfo.mapInfo[m++] = new MapInfomation("天空之城", 40, 3, 550, 200, 17, 23, 1, temp);


            temp = new MapI[8];
            temp[0] = new MapI("神殿外围", 21, 24);
            temp[1] = new MapI("极昼", 22, 25);
            temp[2] = new MapI("炼狱", 22, 26);
            temp[3] = new MapI("第一脊椎", 23, 27);
            temp[4] = new MapI("第二脊椎", 24, 28);
            temp[5] = new MapI("天帷禁地", 25, 29);
            temp[6] = new MapI("神殿外围", 26, 29);
            temp[7] = new MapI("树精丛林", 27, 29);
            MapInfo.mapInfo[m++] = new MapInfomation("天维巨兽", 40, 4, 300, 300, 24, 30, 1, temp);

            temp = new MapI[7];
            temp[0] = new MapI("浅栖之地", 31, 30);
            temp[1] = new MapI("蜘蛛洞穴", 32, 31);
            temp[2] = new MapI("蜘蛛王国", 150, 32);
            temp[3] = new MapI("英雄冢", 151, 33);
            temp[4] = new MapI("暗精灵墓地", 35, 34);
            temp[5] = new MapI("熔岩穴", 34, 35);
            temp[6] = new MapI("暗黑城入口", 36, 35);
            MapInfo.mapInfo[m++] = new MapInfomation("阿法利亚", 41, 2, 550, 300, 31, 35, 2, temp);

            temp = new MapI[3];
            temp[0] = new MapI("暴君的祭坛", 152, 36);
            temp[1] = new MapI("黄金矿洞", 153, 37);
            temp[2] = new MapI("远古墓穴深处", 154, 38);
            MapInfo.mapInfo[m++] = new MapInfomation("诺伊佩拉", 42, 3, 600, 150, 36, 38, 2, temp);

            temp = new MapI[1];
            temp[0] = new MapI("远古墓穴深处", 154, 38);
            MapInfo.mapInfo[m++] = new MapInfomation("诺伊佩拉(任务)", 42, 3, 600, 150, 39, 39, 2, temp);

            temp = new MapI[5];
            temp[0] = new MapI("山脊", 40, 40);
            temp[1] = new MapI("冰心", 41, 41);
            temp[2] = new MapI("利库天井", 42, 42);
            temp[3] = new MapI("白色废墟", 43, 43);
            temp[4] = new MapI("布万加修练场", 141, 44);
            MapInfo.mapInfo[m++] = new MapInfomation("雪山", 43, 1, 300, 450, 40, 45, 2, temp);

            temp = new MapI[7];
            temp[0] = new MapI("绿都", 61, 46);
            temp[1] = new MapI("堕落的盗贼", 50, 47);
            temp[2] = new MapI("秘密村庄", 7146, 47, MapI.MapType.Spacial);
            temp[3] = new MapI("迷乱之村", 51, 48);
            temp[4] = new MapI("血蝴蝶之舞", 52, 48);
            temp[5] = new MapI("疑惑之村", 53, 49);
            temp[6] = new MapI("痛苦之村", 7123, 49);
            MapInfo.mapInfo[m++] = new MapInfomation("诺斯玛尔", 39, 5, 400, 300, 46, 49, 2, temp);

            temp = new MapI[6];
            temp[0] = new MapI("炽精森林", 144, 50);
            temp[1] = new MapI("冰晶森林", 145, 51);
            temp[2] = new MapI("精灵之森", 7147, 51, MapI.MapType.Spacial);
            temp[3] = new MapI("水晶矿脉", 146, 52);
            temp[4] = new MapI("幽冥监狱", 148, 53);
            temp[5] = new MapI("次元空间", 7156, 53, MapI.MapType.Normal, false);
            MapInfo.mapInfo[m++] = new MapInfomation("亚诺法森林", 46, 6, 400, 300, 50, 53, 3, temp);

            temp = new MapI[6];
            temp[0] = new MapI("蘑菇庄园", 156, 54);
            temp[1] = new MapI("蚁后的巢穴", 50, 55);
            temp[2] = new MapI("地下水道", 7148, 55, MapI.MapType.Spacial);
            temp[3] = new MapI("腐烂之地", 158, 56);
            temp[4] = new MapI("赫顿玛尔旧街区", 159, 57);
            temp[5] = new MapI("绝望的棋局", 160, 57);
            MapInfo.mapInfo[m++] = new MapInfomation("厄运之城", 46, 5, 500, 250, 54, 57, 3, temp);

            temp = new MapI[5];
            temp[0] = new MapI("鲨鱼栖息地", 161, 58);
            temp[1] = new MapI("人鱼的国度", 162, 59);
            temp[2] = new MapI("GBL女神殿", 163, 60);
            temp[3] = new MapI("天空岛", 7149, 60, MapI.MapType.Spacial);
            temp[4] = new MapI("树精繁殖地", 164, 61);
            MapInfo.mapInfo[m++] = new MapInfomation("逆流瀑布", 46, 4, 600, 250, 58, 62, 3, temp);

            temp = new MapI[9];
            temp[0] = new MapI("根特外围", 80, 63);
            temp[1] = new MapI("根特东门", 81, 64);
            temp[2] = new MapI("根特南门", 82, 65);
            temp[3] = new MapI("哈尔特山", 7150, 65, MapI.MapType.Spacial);
            temp[4] = new MapI("根特北门", 88, 66);
            temp[5] = new MapI("根特防御战", 89, 67);
            temp[6] = new MapI("夜间袭击战", 83, 68);
            temp[7] = new MapI("补给线阻断战", 84, 69);
            temp[8] = new MapI("追击歼灭战", 85, 70);
            MapInfo.mapInfo[m++] = new MapInfomation("根特", 6, 1, 600, 250, 63, 70, 3, temp);

            temp = new MapI[5];
            temp[0] = new MapI("阿登高地", 93, 71);
            temp[1] = new MapI("列车上的海贼", 86, 72);
            temp[2] = new MapI("夺回西部线", 87, 72);
            temp[3] = new MapI("海上航线", 7151, 73, MapI.MapType.Spacial);
            temp[4] = new MapI("雾都赫伊斯", 92, 74);
            MapInfo.mapInfo[m++] = new MapInfomation("海上列车", 9, 1, 800, 250, 71, 74, 3, temp);

            temp = new MapI[8];
            temp[0] = new MapI("格兰之火", 70, 75);
            temp[1] = new MapI("瘟疫之源", 71, 76);
            temp[2] = new MapI("卡勒特之初", 72, 77);
            temp[3] = new MapI("时间界限", 7152, 77, MapI.MapType.Spacial);
            temp[4] = new MapI("绝密区域", 74, 78);
            temp[5] = new MapI("昔日悲鸣", 75, 79);
            temp[6] = new MapI("凛冬", 76, 80);
            temp[7] = new MapI("迷之觉悟", 77, 80);
            MapInfo.mapInfo[m++] = new MapInfomation("时空之门", 12, 0, 150, 300, 75, 80, 4, temp);

            temp = new MapI[5];
            temp[0] = new MapI("克雷发电站", 101, 81);
            temp[1] = new MapI("普鲁兹发电站", 102, 81);
            temp[2] = new MapI("控制塔", 7153, 81, MapI.MapType.Spacial);
            temp[3] = new MapI("特伦斯发电站", 103, 82);
            temp[4] = new MapI("格蓝迪发电站", 104, 83);
            MapInfo.mapInfo[m++] = new MapInfomation("能源中心", 14, 2, 400, 300, 81, 83, 4, temp);

            temp = new MapI[6];
            temp[0] = new MapI("倒悬的瞭望台", 190, 85);
            temp[1] = new MapI("卢克的聚光镜", 191, 85);
            temp[2] = new MapI("钢铁之臂", 192, 85);
            temp[3] = new MapI("不灭回廊", 7154, 85, MapI.MapType.Spacial);
            temp[4] = new MapI("能源熔炉", 193, 86);
            temp[5] = new MapI("光之舞会", 194, 86);
            MapInfo.mapInfo[m++] = new MapInfomation("寂静城", 22, 2, 800, 365, 85, 86, 4, temp);

            temp = new MapI[6];
            temp[0] = new MapI("时间广场", 310, 87);
            temp[1] = new MapI("兽人峡谷", 311, 87);
            temp[2] = new MapI("恐怖的栖息地", 312, 88);
            temp[5] = new MapI("血色防线", 7155, 88, MapI.MapType.Spacial);
            temp[3] = new MapI("疾风地带", 313, 89);
            temp[4] = new MapI("红色魔女之森", 314, 89);
            MapInfo.mapInfo[m++] = new MapInfomation("地轨中心", 30, 2, 500, 400, 87, 90, 4, temp);

            Int32 tt = 0;
            temp = new MapI[13];
            temp[tt++] = new MapI("黑雾之源", 243, 85);
            temp[tt++] = new MapI("震颤的大地", 244, 85);
            temp[tt++] = new MapI("擎天之柱", 245, 85);
            temp[tt++] = new MapI("能量阻截战", 246, 85);
            temp[tt++] = new MapI("黑色火山", 247, 85);
            temp[tt++] = new MapI("黑雾之谜", 1006, 84);
            temp[tt++] = new MapI("破坏关节", 235, 84);
            temp[tt++] = new MapI("舰炮防御战", 227, 84);
            temp[tt++] = new MapI("意志之路", 1005, 84);
            temp[tt++] = new MapI("擎天之柱上部", 236, 84);
            temp[tt++] = new MapI("艰难的攻坚战", 1007, 84);
            temp[tt++] = new MapI("黑色火山内部", 238, 84);
            temp[tt++] = new MapI("安图恩的心脏", 233, 84);
            MapInfo.mapInfo[m++] = new MapInfomation("安图恩", 20, 2, 900, 400, 84, 84, 2, temp);

            temp = new MapI[3];
            temp[0] = new MapI("纳特拉的复仇", 3200, 84, MapI.MapType.Othther);
            temp[1] = new MapI("双子巨人的背叛", 3201, 84, MapI.MapType.Othther);
            temp[2] = new MapI("圣地：龙之魂", 3202, 84, MapI.MapType.Othther);
            MapInfo.mapInfo[m++] = new MapInfomation("月轮山", 18, 0, 750, 350, 84, 84, 1, temp);





            temp = new MapI[7];
            temp[0] = new MapI("王的遗迹", 7101, 85, MapI.MapType.Yuangu);
            temp[1] = new MapI("比尔马克帝国试验场", 7102, 85, MapI.MapType.Yuangu);
            temp[2] = new MapI("悲鸣洞穴", 7103, 85, MapI.MapType.Yuangu);
            temp[3] = new MapI("诺伊佩拉", 7104, 85, MapI.MapType.Yuangu);
            temp[4] = new MapI("幽灵列车", 7105, 85, MapI.MapType.Yuangu);
            temp[5] = new MapI("痛苦之村", 7106, 85, MapI.MapType.Yuangu);
            temp[6] = new MapI("卡勒特指挥部", 7107, 85, MapI.MapType.Yuangu);
            MapInfo.mapInfo[m++] = new MapInfomation("圣者之鸣号【远古】", 17, 3, 600, 300, 0, 0, 0, temp);

            temp = new MapI[1];
            temp[0] = new MapI("异界", 7167, 85, MapI.MapType.Eiji);
            MapInfo.mapInfo[m++] = new MapInfomation("圣者之鸣号【异界】", 17, 2, 700, 300, 0, 0, 0, temp);

            temp = null;
            MapInfo.mapInfo[m++] = new MapInfomation("卢克实验室", 22, 4, 700, 350, 0, 0, 2, temp);

            temp = new MapI[6];
            temp[0] = new MapI("伤城", 8500, 50, MapI.MapType.Othther);
            temp[1] = new MapI("破灭峡谷", 8507, 50, MapI.MapType.Othther);
            temp[2] = new MapI("呐喊之地", 8504, 50, MapI.MapType.Othther);
            temp[5] = new MapI("哀泣之穴", 8503, 50, MapI.MapType.Othther);
            temp[3] = new MapI("失心迷宫", 8505, 50, MapI.MapType.Othther);
            temp[4] = new MapI("永恒殿堂", 8508, 50, MapI.MapType.Othther);
            MapInfo.mapInfo[m++] = new MapInfomation("公会基地", 8, 1, 150, 450, 0, 0, 2, temp);
            temp = null;
            MapInfo.mapInfo[m++] = new MapInfomation("亡者峡谷", 38, 5, 400, 400, 0, 0, 2, temp);
            MapInfo.mapInfo[m++] = new MapInfomation("武斗大会", 11, 2, 750, 300, 0, 0, 0, temp);


            for (int i = 0; i < m; i++)
                comboBox1.Items.Add(MapInfo.mapInfo[i].name);
			comboBox1.SelectedIndex = 13;
			comboBox2.SelectedIndex = 4;
			comboBox3.SelectedIndex = 4;


			//if (result[0] == false)
			//    TerminateProcess(IntPtr.Subtract(IntPtr.Zero, 1), 0);

		}
		private void listView1_Click(object sender, EventArgs e) {
            EDIT_ID.Text = listView1.SelectedItems[0].Text;
            if (listView1.SelectedItems[0].SubItems[5].Text != "null") {
                string temp = listView1.SelectedItems[0].SubItems[5].Text;
                string[] d = temp.Split(',');
                Int32[] ilist = new Int32[d.Length];
                Int32 m = 0;
                foreach (string s in d) {
                    ilist[m++] = Convert.ToInt32(s);
                }
                EDIT_Count.Text = ilist.Max().ToString();
            } else
                EDIT_Count.Text = "1";
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e) {

        }
        private void RB_All_Click(object sender, EventArgs e) {
            if (RB_Accept.Checked)
                GetTask(1);
            else if (RB_All.Checked)
                GetTask(0);
            else if (RB_Tz.Checked)
                GetTask(2);
        }
        private void listView1_DoubleClick(object sender, EventArgs e) {
            fun.AcceptQuest(Int32.Parse(EDIT_ID.Text));
            for (Int32 i = 0; i < Int32.Parse(EDIT_Count.Text); i++)
                fun.CompletingQuest(Int32.Parse(EDIT_ID.Text));
            fun.CommittingQuest(Int32.Parse(EDIT_ID.Text));
            //fun.AcceptQuest(Int32.Parse(EDIT_ID.Text) + 1);

            //Thread.Sleep(1000);

            //if (RB_Accept.Checked)
            //    GetTask(1);
            //else if (RB_All.Checked)
            //    GetTask(0);
            //else if (RB_Tz.Checked)
            //    GetTask(2);
        }

        private void BN_Accept_Click(object sender, EventArgs e) {
            fun.AcceptQuest(Int32.Parse(EDIT_ID.Text));
        }
        private void BN_Wc_Click(object sender, EventArgs e) {
            for (Int32 i = 0; i < Int32.Parse(EDIT_Count.Text); i++)
                fun.CompletingQuest(Int32.Parse(EDIT_ID.Text));
        }
        private void BN_Tj_Click(object sender, EventArgs e) {
            fun.CommittingQuest(Int32.Parse(EDIT_ID.Text));
        }
        private void button2_Click_1(object sender, EventArgs e) {


        }
        private void checkBox11_CheckedChanged(object sender, EventArgs e) {

        }
        private Int32 DataHead = 0;
        private Int32 pEquip = 0;
        private void button2_Click_2(object sender, EventArgs e) {

            DataHead = gMrw.readInt32(fun.CreateEquit(108000383) + 0xF78);

            //writeLogLine(DataHead.ToString());
            Int32 cPoint = gMrw.readInt32(baseAddr.dwBase_Character);

            if (DataHead == 0) {
                MessageBox.Show("请将鼠标对准冥焰穿刺");
                return;
            }
            Int32 num = 0;
            for (Int32 i = baseAddr.dwOffset_Equip_wq; i <= baseAddr.dwOffset_Equip_wq + 0x2C; i += 4) {
                if (gMrw.readInt32(cPoint + i) == 0)
                    continue;
                if (i == baseAddr.dwOffset_Equip_wq + 0x10)
                    continue;
                writeLogLine(gMrw.Decryption(gMrw.readInt32(cPoint + i) + 0xF8C).ToString());
                gMrw.Encryption(gMrw.readInt32(cPoint + i) + 0xF8C, 114);
                gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0xF9C, 1);

                if (num++ >= 3)
                    break;
            }

            if (num < 3) {
                MessageBox.Show("请至少穿戴三件以上装备");
                return;
            }
            writeLogLine(Convert.ToString(DataHead, 16));
            DataHead = gMrw.readInt32(DataHead + 0xC, 0xC, 0, 0x480);
            writeLogLine(Convert.ToString(DataHead, 16));
        }
        private void button5_Click(object sender, EventArgs e) {
            //0xF78

            if (gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) == 0)
            {
                MessageBox.Show("请穿戴武器");
            }
            //DataHead = gMrw.readInt32(pEquip + 0xac8);
            writeLogLine("初始化创建");
            pEquip = fun.CreateEquit(445013);
            DataHead = gMrw.readInt32(pEquip + 0xF58);

            //writeLogLine(DataHead.ToString());
            Int32 cPoint = gMrw.readInt32(baseAddr.dwBase_Character);
            //gMrw.writedData((uint)gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0xAC8, gMrw.readData((uint)pEquip + 0xac8, 12), 12);
            //gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0xAC8, gMrw.readInt32(pEquip + 0xac8));
            //gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0xAC8 + 4, gMrw.readInt32(pEquip + 0xac8 + 4));
            //gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0xAC8 + 8, gMrw.readInt32(pEquip + 0xac8 + 8));

            if (DataHead == 0) {
                MessageBox.Show("请将鼠标对准冥焰穿刺");
                return;
            }
            Int32 num = 0;
            for (Int32 i = baseAddr.dwOffset_Equip_wq; i <= baseAddr.dwOffset_Equip_wq + 0x2C; i += 4)
            {
                if (gMrw.readInt32(cPoint + i) == 0)
                    continue;
                if (i == baseAddr.dwOffset_Equip_wq + 0x4)
                    continue;
                writeLogLine(gMrw.Decryption(gMrw.readInt32(cPoint + i) + 0xF6C).ToString());
                fun.EncryptionCall(gMrw.readInt32(cPoint + i) + 0xF6C, 114);
                gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0xF7C, 1);

                if (num++ >= 3)
                    break;
            }

            if (num < 3)
            {
                MessageBox.Show("请至少穿戴三件以上装备");
                return;
            }
            writeLogLine(Convert.ToString(DataHead, 16));
            DataHead = gMrw.readInt32(DataHead + 0xC, 0xC, 0, 0x478);
            writeLogLine(Convert.ToString(pEquip, 16));
            writeLogLine(Convert.ToString(DataHead, 16));



            Int32 SkillID_1 = Int32.Parse(textBox6.Text);
            Int32 SkillID_2 = Int32.Parse(textBox7.Text);

            Int32 SkillID = gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Character, 0x6420, 0x88, 0) + 0x6C);
            if (SkillID_1 == 0)
                SkillID_1 = SkillID;
            if (SkillID_2 == 0)
                SkillID_2 = SkillID;

            writeLogLine("技能id" + SkillID.ToString());
            int temp = DataHead;
            Int32 CharaID = gMrw.readInt32(baseAddr.dwBase_Role_Id);//dnf.exe+3D5BA84
            writeLogLine("角色id" + CharaID.ToString());
            Int32 lpara1_1 = Convert.ToInt32(EDIT_SK_1_SX_1.Text);
            Int32 lpara1_2 = Convert.ToInt32(EDIT_SK_1_SX_2.Text);

            Int32 lpara2_1 = Convert.ToInt32(EDIT_SK_2_SX_1.Text);
            Int32 lpara2_2 = Convert.ToInt32(EDIT_SK_2_SX_2.Text);

            //Int32 lpara2 = Convert.ToInt32(EDIT_SK_2_SX.Text);
            //Int32 lpara3 = Convert.ToInt32(EDIT_SK_3_SX.Text);

            Int32 spara1 = Convert.ToInt32(EDIT_SK_1_BS.Text);
            Int32 spara2 = Convert.ToInt32(EDIT_SK_2_BS.Text);
            Int32 spara3 = Convert.ToInt32(EDIT_SK_3_BS.Text);
            //return;
            temp += (40 * 0);
            gMrw.writeInt32(temp, CharaID);
            fun.EncryptionCall(temp + 4, SkillID);
            gMrw.writeInt32(temp + 12, 0);
            fun.EncryptionCall(temp + 16, 1);
            fun.EncryptionCall(temp + 24, -100);
            gMrw.writeInt32(temp + 32, 2);
            gMrw.writeInt32(temp + 36, 28);

            temp += (40);
            gMrw.writeInt32(temp, CharaID);
            fun.EncryptionCall(temp + 4, SkillID_1);
            gMrw.writeInt32(temp + 12, lpara1_1);
            fun.EncryptionCall(temp + 16, 1);
            fun.EncryptionCall(temp + 24, spara1);
            gMrw.writeInt32(temp + 32, lpara1_2);
            gMrw.writeInt32(temp + 36, 28);

            temp += (40);
            gMrw.writeInt32(temp, CharaID);
            fun.EncryptionCall(temp + 4, SkillID_2);
            gMrw.writeInt32(temp + 12, lpara2_1);
            fun.EncryptionCall(temp + 16, 1);
            fun.EncryptionCall(temp + 24, spara2);
            gMrw.writeInt32(temp + 32, lpara2_2);
            gMrw.writeInt32(temp + 36, 28);

            temp += (40);
            gMrw.writeInt32(temp, CharaID);
            fun.EncryptionCall(temp + 4, SkillID);
            gMrw.writeInt32(temp + 12, 0);
            fun.EncryptionCall(temp + 16, 1);
            fun.EncryptionCall(temp + 24, -100);
            gMrw.writeInt32(temp + 32, 3);
            gMrw.writeInt32(temp + 36, 28);

            temp += (40);
            gMrw.writeInt32(temp, CharaID);
            fun.EncryptionCall(temp + 4, SkillID);
            gMrw.writeInt32(temp + 12, 0);
            fun.EncryptionCall(temp + 16, 1);
            fun.EncryptionCall(temp + 24, -100);
            gMrw.writeInt32(temp + 32, 5);
            gMrw.writeInt32(temp + 36, 28);

        }

        public static int[] GetRandomSequence1(int total) {
            List<int> input = new List<int>();
            for (int i = 0; i < total; i++) {
                input.Add(i);
            }

            List<int> output = new List<int>();

            Random random = new Random();
            int end = total;
            for (int i = 0; i < total; i++) {
                int num = random.Next(0, end);
                output.Add(input[num]);
                input.RemoveAt(num);
                end--;
            }

            return output.ToArray();
        }

        public void TzThread() {
            GetTask(1);

            foreach (ListViewItem item in listView1.Items) {

                Int32 ID = Int32.Parse(item.Text);

                string name = item.SubItems[2].Text;

                if (checkBox9.Checked) {
                    if (name.Contains("击杀") && item.SubItems[4].Text == "26") {
                        string temp = item.SubItems[5].Text;
                        string[] d = temp.Split(',');
                        Int32[] ilist = new Int32[d.Length];
                        Int32 m = 0;
                        foreach (string s in d) {
                            ilist[m++] = Convert.ToInt32(s);
                        }
                        for (int i = 0; i < ilist.Max(); i++)
                            fun.CompletingQuest(ID);
                        fun.CommittingQuest(ID);
                    }
                }
            }
            Thread.Sleep(1000);
            if (checkBox13.Checked) {
                fun.CityTp(39, 4, 580, 282);
                Thread.Sleep(500);
                fun.Buy(10165028, 249, 564);
                Thread.Sleep(1000);
                Int32 pos = fun.GetItem(10165028).Pos;
                if (pos != 0)
                    fun.SendOpenPackage(pos);
            }

            if (checkBox14.Checked == true) {
                fun.AcceptQuest(8557);
                Thread.Sleep(1000);
                fun.CommittingQuest(8557);
                Thread.Sleep(1000);
                fun.CommittingQuest(8557);

            }

        }

        private void BN_Tz_Click(object sender, EventArgs e) {
            Thread t = new Thread(TzThread);
            t.Start();
        }
        private void RB_All_CheckedChanged(object sender, EventArgs e) {

        }
        private void button6_Click(object sender, EventArgs e) {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e) {



            timer1.Start();
        }
        public void OpenCode() {
        }
        private void button9_Click(object sender, EventArgs e) {


            //OpenCode();
            //fun.CreateSkill(Int32.Parse(EDIT_Code.Text), Int32.Parse(EDIT_Code_Pl.Text), Int32.Parse(EDIT_Code_Sleep.Text), CB_IsCodeAll.Checked);
            //Int32 ePoint = fun.CreateEquit(100310528);
            //writeLogLine(Convert.ToString(ePoint, 16));

            //Int32 head = gMrw.readInt32(ePoint + 0x10E4);
            //Int32 cPoint = gMrw.readInt32(baseAddr.dwBase_Character);

            //for (Int32 i = 0x2CE4; i <= 0x2CEC; i += 4) {
            //    gMrw.Encryption(gMrw.readInt32(cPoint + i) + 0x10F8, 11126);
            //    gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0x1108, 1);
            //    gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0x10E4, head);
            //}
            //writeLogLine(Convert.ToString(head, 16));

            //head = gMrw.readInt32(head + 0x1C, 0x50, 0x4C0);

            //writeLogLine(Convert.ToString(head, 16));
            //gMrw.writeInt32(head + 4, Int32.Parse(EDIT_FREEZE_Hurt.Text));
            //gMrw.writeInt32(head + 8, 25000);
            //gMrw.writeInt32(head + 0xC, Int32.Parse(EDIT_FREEZE_Rate.Text));
        }
        private void BN_SD_Click(object sender, EventArgs e) {

        }
        private void BN_L1_Click(object sender, EventArgs e) {

        }
        private void RB_2_No_CheckedChanged(object sender, EventArgs e) {
            FunctionID_1 = 0;
        }
        private void RB_2_CreateSkill_CheckedChanged(object sender, EventArgs e) {
            FunctionID_1 = 1;
        }
        private void RB_2_TzYc_CheckedChanged(object sender, EventArgs e) {
            FunctionID_1 = 2;
        }
        private void RB_2_Freeze_CheckedChanged(object sender, EventArgs e) {
            FunctionID_1 = 3;
        }
        private void button6_Click_1(object sender, EventArgs e) {

        }
        void KeyThread() {
            while (true) {
                //fun.KeyPress((Int32)Keys.Space);
                Thread.Sleep(500);
            }
        }

        public void timer2Switch(bool s) {
            if (s) timer2.Start();
            else timer2.Stop();
        }
        public void timer4Switch(bool s) {
            if (s) timer4.Start();
            else timer4.Stop();
        }
        //bool IsLevelStart = false;
        private void button7_Click_1(object sender, EventArgs e) {
            //Button temp = (Button)sender;
            //if (temp.Text == "起号")
            //{
            //    temp.Text = "结束";
            //    st.start();
            //}
            //else
            //{
            //    temp.Text = "起号";
            //    st.end();
            //}
        }

        Int32 ePoint;

        public void zy_bd() {
            if (gMrw.readInt32(ePoint + 0x20) != 100310528) {
                ePoint = fun.CreateEquit(100310528);
                writeLogLine("初始化创建");
            }
            writeLogLine(Convert.ToString(ePoint, 16));
            Int32 head = gMrw.readInt32(ePoint + 0xF78);

            Int32 cPoint = IsGropeHelper() == false ? gMrw.readInt32(baseAddr.dwBase_Character, 0x6414, 0x98) : gMrw.readInt32(baseAddr.dwBase_Character, 0x6414, 0x2C); ;
            head = gMrw.readInt32(head + 0xC, 0xC, 0, 0x4C0);

            writeLogLine(Convert.ToString(head, 16));

            //gMrw.writeInt32(head, 1);
            //gMrw.writeInt32(head + 4, hurt);
            gMrw.writeInt32(head + 8, 1500);
            gMrw.writeInt32(head + 0xC, 1);

            for (Int32 i = 0x2F0C; i <= 0x2F14; i += 4) {
                if (gMrw.readInt32(cPoint + i) == 0) {
                    MessageBox.Show("请穿戴三件首饰");
                    return;
                }

                if (gMrw.Decryption(gMrw.readInt32(cPoint + i) + 0xF8C) != 11126) {
                    gMrw.Encryption(gMrw.readInt32(cPoint + i) + 0xF8C, 11126);
                    writeLogLine("加密写入数据");
                }
                gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0xF9C, 1);
                //gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0x10E4, head);
            }
        }


        private void button1_Click_1(object sender, EventArgs e) {

        }

        private void button11_Click(object sender, EventArgs e) {
            fun.ChooseChara();
        }

        private void button12_Click(object sender, EventArgs e) {

        }

        //private void button13_Click(object sender, EventArgs e) {
        //    Int32 ePoint = fun.CreateEquit(100050202);
        //    writeLogLine(Convert.ToString(ePoint, 16));

        //    Int32 head = gMrw.readInt32(ePoint + 0x10E4);
        //    Int32 cPoint = gMrw.readInt32(baseAddr.dwBase_Character);

        //    head = gMrw.readInt32(head + 0x1C, 0x80, 0x4E8);

        //    gMrw.writeFloat(gMrw.readInt32(head + 0x18, 0x4, 0x2C) + 0xC, (float)Int32.Parse(textBox4.Text));//概率
        //    gMrw.writeFloat(gMrw.readInt32(head + 0x18, 0x4, 0x18) + 0x4, (float)100);//概率

        //    for (Int32 i = 0x2CD0; i <= 0x2CE0; i += 4) {
        //        if (gMrw.readInt32(cPoint + i) == 0) {
        //            MessageBox.Show("请穿戴三件首饰");
        //            return;
        //        }
        //        gMrw.Encryption(gMrw.readInt32(cPoint + i) + 0x10F8, 11197);
        //        gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0x1108, 1);
        //    }
        //}

        private void button10_Click(object sender, EventArgs e) {
            OpenCode();
        }

        private void button14_Click(object sender, EventArgs e) {
            if (button14.Text == "深渊") {
                button14.Text = "结束挂机";
                mt.StartSY();
            } else {
                button14.Text = "深渊";
                mt.Stop();
            }
        }

        private void button15_Click(object sender, EventArgs e) {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e) {
            FunctionID_1 = 4;
        }

        private void timer1_Tick(object sender, EventArgs e) {
            Random r = new Random();
            this.Text = (Win32.Kernel.GetTickCount() * r.Next(1, Win32.Kernel.GetTickCount() % 12 + 2)).ToString();

        }

        private void JxThread() {
            Thread.Sleep(1000);

            for (int i = 0; i < 10; i++) {
                if (fun.GetPL() == 0)
                    break;
                if (fun.GetCharaLevel() < 55)
                    break;

                fun.ChooseInstance();
                Thread.Sleep(500);
                fun.SendEnterJxInstance();

                while (gMrw.readInt32(baseAddr.dwBase_Map_ID) <= 0)
                    Thread.Sleep(100);

                completeJxTask();
                fun.QuitInstance();
                while (gMrw.readInt32(baseAddr.dwBase_Map_ID) > 0) Thread.Sleep(100);

            }
            fun.OpenJx();

            Thread.Sleep(1000);
            fun.SendEnterStore(10100300, 0);
            Thread.Sleep(1000);
        }

        private void auto_tz_thread() {
            Int32 CharaPos = gMrw.readInt32(gMrw.readInt32(baseAddr.dwBase_Role) + 0x15C);

            for (int i = CharaPos; i < 48; i++) {
                TzThread();
                Thread.Sleep(1000);
                writeLogLine("挑战执行完毕");


                fun.ChooseChara();
                while (gMrw.readInt32(baseAddr.dwBase_Character) > 0) Thread.Sleep(100);
                fun.EnterChara(i + 1);
                while (gMrw.readInt32(baseAddr.dwBase_Character) == 0) Thread.Sleep(100);
                Thread.Sleep(2000);
            }

        }

        private void BN_Mr_Click(object sender, EventArgs e) {
            Thread t = new Thread(auto_tz_thread);
            t.Start();
        }

        private void button16_Click(object sender, EventArgs e) {
            listView2.Items.Clear();

            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);
            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);
                ListViewItem lvi = new ListViewItem();
                string name;
                if (gMrw.readInt32(onobj + 0xA4) != 0x121) name = gMrw.readString(gMrw.readInt32(onobj + 0x404));
                else
                {
                    Int32 item_point = gMrw.readInt32(onobj + 0x16C0);
                    name = gMrw.readString(gMrw.readInt32(item_point + 0x24));
                }
                lvi.Text = Convert.ToString(onobj, 16);
                lvi.SubItems.Add(name);
                string temp = gMrw.readString(gMrw.readInt32(onobj + 0x4EC));
                lvi.SubItems.Add(temp);
                lvi.SubItems.Add(gMrw.readInt32(onobj + 0xA4).ToString());

                lvi.SubItems.Add((fun.getObjPos(onobj).x).ToString());
                lvi.SubItems.Add((fun.getObjPos(onobj).y).ToString());
                lvi.SubItems.Add((fun.getObjPos(onobj).z).ToString());
                Int32 zy = gMrw.readInt32(onobj + 0x828);

                //lvi.SubItems.Add(((Int32)gMrw.readInt32(onobj + 0x400)).ToString());

                if (gMrw.readInt32(onobj + 0xA4) == 273)
                    lvi.SubItems.Add(gMrw.Decryption(onobj + 0x6794).ToString());
                else
                    lvi.SubItems.Add(((Int32)gMrw.readInt32(onobj + 0x400)).ToString());

                lvi.SubItems.Add(gMrw.Decryption(onobj + 0xAC).ToString());
                lvi.SubItems.Add(gMrw.readInt32(onobj + 0x828).ToString());
                lvi.SubItems.Add(gMrw.readInt64(onobj + 0x3AEC).ToString());

                listView2.Items.Add(lvi);
            }
        }

        private void timer2_Tick(object sender, EventArgs e) {
            if (gMrw.readInt32(baseAddr.dwBase_Map_ID) > 0)
                WinIo.KeyPress(VKKey.VK_SPACE);
        }

        private void timer3_Tick(object sender, EventArgs e) {
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {

        }

        void monite_thread() {
            //while (true) {
            //    if (gMrw.readInt32(baseAddr.dwBase_ExpBox) >= 11691495 && gMrw.readInt32(baseAddr.dwBase_Map_ID) <= 0)
            //    {
            //        fun.GetExpBox();
            //        Thread.Sleep(2000);
            //    }

            //    Thread.Sleep(1000);
            //}
        }

        Thread skill_monit;
        private void button17_Click(object sender, EventArgs e) {
            if (skill_monit == null) {
                skill_monit = new Thread(monite_thread);
            }



        }

        public void check_zy() {


        }

        private void test(Byte[] gg) {
            Coin_Num.Text = gg[2].ToString();

        }

        private void label49_Click(object sender, EventArgs e) {

        }

        public static string r_ini(string Section, string Key, string ininame) {
            StringBuilder temp = new StringBuilder(1024);
            int i = GetPrivateProfileStringA(Section, Key, "", temp, 1024, ininame);
            String flag = temp.ToString();

            return flag;
        }


        public static void w_ini(string Section, string Key, string Value, string ininame) {
            if (Value == "True") {
                Value = "1";
            }
            if (Value == "False") {
                Value = "0";
            }
            WritePrivateProfileStringA(Section, Key, Value, ininame);
        }

        private void button25_Click(object sender, EventArgs e) {
            string resultFile = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = @".\superskill\";
            openFileDialog1.Filter = "ini file (*.ini)|*.ini|All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                resultFile = openFileDialog1.FileName;
            }

            if (resultFile == "") {
                MessageBox.Show("未打开文件");
                return;
            }

            mh_sy.Text = r_ini("装备配置", "上衣代码", resultFile);
            mh_tj.Text = r_ini("装备配置", "头肩代码", resultFile);
            mh_xz.Text = r_ini("装备配置", "下装代码", resultFile);
            mh_yd.Text = r_ini("装备配置", "腰带代码", resultFile);
            mh_xiezi.Text = r_ini("装备配置", "鞋子代码", resultFile);
            mh_xl.Text = r_ini("装备配置", "项链代码", resultFile);
            mh_jz.Text = r_ini("装备配置", "戒指代码", resultFile);
            mh_sz.Text = r_ini("装备配置", "手镯代码", resultFile);
            mh_wq.Text = r_ini("装备配置", "武器代码", resultFile);
            mh_zc.Text = r_ini("装备配置", "左槽代码", resultFile);
            mh_yc.Text = r_ini("装备配置", "右槽代码", resultFile);
            mh_eh.Text = r_ini("装备配置", "耳环代码", resultFile);
            mh_ch.Text = r_ini("装备配置", "称号代码", resultFile);


        }

        private void button24_Click(object sender, EventArgs e) {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@".\superskill");
            di.Create();

            string path = @".\superskill\" + textBox33.Text + ".ini";
            w_ini("装备配置", "上衣代码 ", mh_sy.Text, path);
            w_ini("装备配置", "头肩代码 ", mh_tj.Text, path);
            w_ini("装备配置", "下装代码 ", mh_xz.Text, path);
            w_ini("装备配置", "腰带代码 ", mh_yd.Text, path);
            w_ini("装备配置", "鞋子代码 ", mh_xiezi.Text, path);
            w_ini("装备配置", "项链代码 ", mh_xl.Text, path);
            w_ini("装备配置", "戒指代码 ", mh_jz.Text, path);
            w_ini("装备配置", "手镯代码 ", mh_sz.Text, path);
            w_ini("装备配置", "武器代码 ", mh_wq.Text, path);
            w_ini("装备配置", "左槽代码 ", mh_zc.Text, path);
            w_ini("装备配置", "右槽代码 ", mh_yc.Text, path);
            w_ini("装备配置", "耳环代码 ", mh_eh.Text, path);
            w_ini("装备配置", "称号代码 ", mh_ch.Text, path);


        }

        private void button26_Click(object sender, EventArgs e) {
            progressBar1.Value = 0;

            Int32[] code_yet = new Int32[20];
            Int32[] code_new = new Int32[20];

            int count = 0;

            for (int i = baseAddr.dwOffset_Equip_wq; i <= baseAddr.dwOffset_Equip_wq + 0x2C; i += 4) {
                code_yet[count++] = gMrw.readInt32(baseAddr.dwBase_Character, i, 0x20);
            }
            count = 0;
            code_new[count++] = Int32.Parse(mh_wq.Text);
            code_new[count++] = Int32.Parse(mh_ch.Text);
            code_new[count++] = Int32.Parse(mh_sy.Text);
            code_new[count++] = Int32.Parse(mh_tj.Text);
            code_new[count++] = Int32.Parse(mh_xz.Text);
            code_new[count++] = Int32.Parse(mh_xiezi.Text);
            code_new[count++] = Int32.Parse(mh_yd.Text);
            code_new[count++] = Int32.Parse(mh_xl.Text);
            code_new[count++] = Int32.Parse(mh_sz.Text);
            code_new[count++] = Int32.Parse(mh_jz.Text);
            code_new[count++] = Int32.Parse(mh_zc.Text);
            code_new[count++] = Int32.Parse(mh_yc.Text);

            for (int i = 0; i < count; i++) {
                System.GC.Collect();

                progressBar1.Value += 1;

                if (code_new[i] == 0)
                    continue;
                int new_addr, old_addr;
                new_addr = GetEquipDirAddress_test(code_new[i]);
                old_addr = GetEquipDirAddress_test(code_yet[i]);

                if (new_addr == 0) {
                    MessageBox.Show("new 错误" + i);
                    continue;
                }

                if (old_addr == 0) {
                    MessageBox.Show("old 错误" + i);
                    continue;
                }

                gMrw.writeInt32(old_addr + 4, gMrw.readInt32(new_addr + 4));
            }

        }

        public static byte[] temp = new byte[0x1000000];

        private void button29_Click(object sender, EventArgs e) {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@".\superskill");
            di.Create();

            string path = @".\superskill\" + textBox33.Text + ".ini";

            w_ini("自定义配置", "旧代码", textBox20.Text, path);
            w_ini("自定义配置", "新代码", textBox21.Text, path);
        }

        private void button27_Click(object sender, EventArgs e) {
            string resultFile = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = @".\superskill\";
            openFileDialog1.Filter = "ini file (*.ini)|*.ini|All files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                resultFile = openFileDialog1.FileName;
            }

            if (resultFile == "") {
                MessageBox.Show("未打开文件");
                return;
            }

            textBox20.Text = r_ini("自定义配置", "旧代码", resultFile);
            textBox21.Text = r_ini("自定义配置", "新代码", resultFile);
        }

        private void button28_Click(object sender, EventArgs e) {
            string[] old_code = textBox20.Text.Split(' ');
            string[] new_code = textBox21.Text.Split(' ');

            if (old_code.Length != new_code.Length) {
                MessageBox.Show("数目不匹配");
                return;
            }

            int count = 0;
            foreach (string old in old_code) {
                int i_old = int.Parse(old);
                int i_new = int.Parse(new_code[count++]);

                int new_addr = GetEquipDirAddress(i_new);
                int old_addr = GetEquipDirAddress(i_old);

                if (new_addr == 0) {
                    MessageBox.Show("new 错误");
                    continue;
                }

                if (old_addr == 0) {
                    MessageBox.Show("old 错误");
                    continue;
                }

                gMrw.writeInt32(old_addr + 4, gMrw.readInt32(new_addr + 4));
            }
        }

        struct Equip_Info {
            public int code;
            public int addr;
            public int dir_addr;

            public Equip_Info(int a, int b, int c) {
                code = a;
                addr = b;
                dir_addr = c;
            }
        }

        //int[] doll_equip_old = { 14058, 12063, 10064 };


        Equip_Info[] doll_equip_old =
        {
            new Equip_Info ( 14058,0 ,0),
            new Equip_Info ( 12063 ,0 ,0),
            new Equip_Info ( 10064,0 ,0)
        };

        Equip_Info[] doll_equip_new =
        {
            new Equip_Info ( 14172,0 ,0),
            new Equip_Info ( 12188 ,0 ,0),
            new Equip_Info ( 10191,0 ,0)
        };




        private void button30_Click(object sender, EventArgs e) {

            //fun.CreateEmery(gMrw.readInt32(baseAddr.dwBase_Character), 9836, Int32.Parse(textBox22.Text));

            //ePoint = fun.CreateEquit(100310528);
            //writeLogLine("初始化创建");

            //writeLogLine(Convert.ToString(ePoint, 16));
            //Int32 head = gMrw.readInt32(ePoint + 0xF78);

            //head = gMrw.readInt32(head + 0xC, 0xC, 0, 0x4C0);

            //writeLogLine(Convert.ToString(head, 16));

            ////gMrw.writeInt32(head, 1);
            //gMrw.writeInt32(head + 4, Int32.Parse(textBox22.Text));
            //gMrw.writeInt32(head + 8, 1500);
            //gMrw.writeInt32(head + 0xC, Int32.Parse(textBox23.Text));
        }

        private void button31_Click(object sender, EventArgs e) {
            fun.movCharaPos(fun.getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).x, fun.getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).y, -200);
            //MessageBox.Show(gMrw.read<uint>(0x401000).ToString());
            //writeLogLine(fun.LoadCall(baseAddr.GetIndexObj.Creature, "phoenix/phoenix.cre").ToString());
            //gMrw.write<float>(0x400400, 0.1f);
            //writeLogLine("初始化创建");
            //pEquip = fun.CreateEquit(105006);
            //gMrw.writeInt32(GetCodeDirAddress(24202) + 4, gMrw.readInt32(GetCodeDirAddress(10499) + 4));
            //gMrw.writeInt32(GetCodeDirAddress(10500) + 4, gMrw.readInt32(GetCodeDirAddress(24012) + 4));
        }

        private void label50_Click(object sender, EventArgs e) {

        }

        private void button32_Click(object sender, EventArgs e) {
            int atk_addr = fun.LoadCall(baseAddr.GetIndexObj.Atk, "monster/newmonsters/event/zombi/attackinfo/attack.atk");
            int addr = gMrw.readInt32(baseAddr.dwBase_Character);
            Byte[] old = gMrw.readData(gMrw.read<uint>(addr) - 0x1000, 0x3000);
            gMrw.writedData((uint)at.GetVirtualAddr() + 0x2001, old, 0x3000);
            gMrw.writeInt32(addr, at.GetVirtualAddr() + 0x3001);

            if (gMrw.readInt32(baseAddr.dwBase_Character, 0, 0x35C) < at.GetVirtualAddr())
            {
                at.clear();
                //at.mov_eax(atk_addr);
                //at.retn(4);
                at.mov_esp_ptr_addx(4, atk_addr);
                at.push(gMrw.readInt32(baseAddr.dwBase_Character, 0, 0x35C));
                at.retn();
                int i = 0;
                foreach (byte a in at.Code)
                {
                    gMrw.writeInt8(at.GetVirtualAddr() + 0xC50 + i++, a);
                }
            }


            at.setEvent();
            //gMrw.writeInt32(at.GetVirtualAddr() + 0x3001 + 0x834, at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1
            //gMrw.writeInt32(at.GetVirtualAddr() + 0x3001 + 0x5BC, at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1

            gMrw.writeInt32(at.GetVirtualAddr() + 0x3001 + 0x35C, at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1
            gMrw.writeInt32(at.GetVirtualAddr() + 0x3001 + 0x458, 0x029B6880 );



            //for (int i = 0; i < 40 + 0x30 + 0x30; i += 4)
            //{
            //    writeLogLine(Convert.ToString(i + 0x64f4 - 0x30, 16) + ":" + gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Character) + 0x64f4 - 0x30 + i).ToString());
            //}
            //fun.EncryptionCall(gMrw.readInt32(baseAddr.dwBase_Character) + 0x64f4, 0);
            //writeLogLine(fun.isNextRoomBoss().ToString());
            //fun.createPet(gMrw.readInt32(baseAddr.dwBase_Character), 3);
            return;

            for (int i = baseAddr.dwOffset_Equip_wq; i <= baseAddr.dwOffset_Equip_wq + 0x2C; i += 4) {


                if (gMrw.readInt32(baseAddr.dwBase_Character, i) == 0) continue;
                gMrw.Encryption1(gMrw.readInt32(baseAddr.dwBase_Character, i) + 0xC4C, 30);
                gMrw.Encryption1(gMrw.readInt32(baseAddr.dwBase_Character, i) + 0x7cc, 150);
                gMrw.Encryption1(gMrw.readInt32(baseAddr.dwBase_Character, i) + 0x7dc, 150);


            }

            gMrw.Encryption1(gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0x7cc, 1500);
            gMrw.Encryption1(gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0x7dc, 1500);

            gMrw.Encryption1(gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0xFF0, 17);

        }

        private void 基本功能_Click(object sender, EventArgs e) {

        }

        private void button33_Click(object sender, EventArgs e) {
            int atk_addr = fun.LoadCall(baseAddr.GetIndexObj.Atk, "monster/newmonsters/event/zombi/attackinfo/attack.atk");
            writeLogLine("atk:" + atk_addr);
           int addr = gMrw.readInt32(baseAddr.dwBase_Character);
            Byte[] old = gMrw.readData(gMrw.read<uint>(addr) - 0x1000, 0x3000);
            gMrw.writedData((uint)at.GetVirtualAddr() + 0x2001, old, 0x3000);
            gMrw.writeInt32(addr, at.GetVirtualAddr() + 0x3001);

            if (gMrw.readInt32(baseAddr.dwBase_Character, 0, 0x35C) < at.GetVirtualAddr())
            {
                at.clear();
                //at.mov_eax(atk_addr);
                //at.retn(4);
                at.mov_esp_ptr_addx(4, atk_addr);
                at.push(gMrw.readInt32(baseAddr.dwBase_Character, 0, 0x35C));
                at.retn();
                int i = 0;
                foreach (byte a in at.Code)
                {
                    gMrw.writeInt8(at.GetVirtualAddr() + 0xC50 + i++, a);
                }
            }
            at.setEvent();
            gMrw.writeInt32(at.GetVirtualAddr() + 0x3001 + 0x35C, at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1
            if (Config.isHook)
                gMrw.writeInt32(at.GetVirtualAddr() + 0x3001 + 0x458, 0x029B6880 );//031435BC    B0 01           mov al,0x1
        }

        private int GetEquipDirAddress(int code) {
            return 0;
            try {
                uint sAddress = 0;
                Win32.MEMORY_BASIC_INFORMATION buffer = new MEMORY_BASIC_INFORMATION();
                while (Win32.Kernel.VirtualQueryEx((uint)gMrw.GetHandle(), sAddress, ref buffer, System.Runtime.InteropServices.Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != 0) {
                    if (buffer.Protect == 0x4) {
                        int s = 0;
                        //writeLogLine("baseAddr = " + Convert.ToString(buffer.BaseAddress, 16));

                        gMrw.readData((uint)buffer.BaseAddress, (uint)buffer.RegionSize, temp);
                        IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(temp, 0);

                        if (temp.Length < buffer.RegionSize) {
                            sAddress += (uint)buffer.RegionSize;
                            continue;
                        }


                        while (s < (buffer.RegionSize - 0x8)) {
                            int address = buffer.BaseAddress + s;

                            if (Marshal.ReadInt32(vBytesAddress) == code) {
                                int result = Marshal.ReadInt32(vBytesAddress + 8);

                                result = Marshal.ReadInt32(vBytesAddress + 4);
                                if (result > 0x401000) {

                                    if (gMrw.readInt32(gMrw.readInt32(address + 4)) == 6815843) {
                                        writeLogLine("装备内存" + Convert.ToString(address, 16));
                                        return address;
                                    }
                                }

                            }
                            s += 4;
                            vBytesAddress += 4;
                        }
                        System.GC.Collect();
                    }
                    sAddress += (uint)buffer.RegionSize;

                }

                return 0;
            } catch {
                MessageBox.Show("异常");
                return GetEquipDirAddress(code);
            }
        }

        public int GetEquipDirAddress_test(int code)
        {
            Int32 dir_head = gMrw.readInt32(baseAddr.Index.Equip + 8, 4);
            Int32 dir_next = dir_head;
            while (gMrw.readInt8(dir_next + 0x15) == 0)
            {
                writeLogLine(gMrw.readString(gMrw.readInt32(dir_next + 0x10)));
                Int32 e_code = gMrw.readInt32(dir_next + 0xC);
                if (e_code == code)
                    return (dir_next + 0xC);
                if (e_code > code)
                    dir_next = gMrw.readInt32(dir_next);
                else
                    dir_next = gMrw.readInt32(dir_next + 8);
            }
            return 0;
        }

        private int GetMonsterDirAddress(int code)
        {
            //writeLogLine("Ecode:" + code);
            Int32 dir_head = gMrw.readInt32(baseAddr.Index.Monster + 8, 4);
            Int32 dir_next = dir_head;
            while (gMrw.readInt8(dir_next + 0x15) == 0)
            {
                Int32 e_code = gMrw.readInt32(dir_next + 0xC);
                if (e_code == code)
                    return (dir_next + 0xC);
                if (e_code > code)
                    dir_next = gMrw.readInt32(dir_next);
                else
                    dir_next = gMrw.readInt32(dir_next + 8);
            }
            return 0;
        }

        private int GetCharacterDirAddress(int code)
        {
            Int32 dir_head = gMrw.readInt32(baseAddr.Index.Character + 8, 4);
            Int32 dir_next = dir_head;
            while (gMrw.readInt8(dir_next + 0x15) == 0)
            {
                writeLogLine(gMrw.readString(gMrw.readInt32(dir_next + 0x10)));
                Int32 e_code = gMrw.readInt32(dir_next + 0xC);
                if (e_code == code)
                    return (dir_next + 0xC);
                if (e_code > code)
                    dir_next = gMrw.readInt32(dir_next);
                else
                    dir_next = gMrw.readInt32(dir_next + 8);
            }
            MessageBox.Show("枚举失败");
            return 0;
        }

        private int GetCodeDirAddress(int code)
        {
            Int32 dir_head = gMrw.readInt32(baseAddr.Index.Code + 8, 4);
            Int32 dir_next = dir_head;
            while (gMrw.readInt8(dir_next + 0x15) == 0)
            {
                writeLogLine(gMrw.readString(gMrw.readInt32(dir_next + 0x10)));
                Int32 e_code = gMrw.readInt32(dir_next + 0xC);
                if (e_code == code)
                    return (dir_next + 0xC);
                if (e_code > code)
                    dir_next = gMrw.readInt32(dir_next);
                else
                    dir_next = gMrw.readInt32(dir_next + 8);
            }
            MessageBox.Show("枚举失败");
            return 0;
        }

        private void button34_Click(object sender, EventArgs e) {
            fun.CreateEmery(gMrw.readInt32(baseAddr.dwBase_Character),-1, 110110);

        }

        private Int32 GetMonsterAddr(int code) {
            return 0;
            try {
                uint sAddress = 0;
                Win32.MEMORY_BASIC_INFORMATION buffer = new MEMORY_BASIC_INFORMATION();


                while (Win32.Kernel.VirtualQueryEx((uint)gMrw.GetHandle(), sAddress, ref buffer, System.Runtime.InteropServices.Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != 0) {
                    if (buffer.Protect == 0x4) {
                        int s = 0;
                        //writeLogLine("baseAddr = " + Convert.ToString(buffer.BaseAddress, 16));

                        gMrw.readData((uint)buffer.BaseAddress, (uint)buffer.RegionSize, temp);
                        IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(temp, 0);

                        if (temp.Length < buffer.RegionSize) {
                            sAddress += (uint)buffer.RegionSize;
                            continue;
                        }


                        while (s < (buffer.RegionSize - 0x20)) {
                            int address = buffer.BaseAddress + s;

                            if (Marshal.ReadInt32(vBytesAddress) == code) {

                                int result = Marshal.ReadInt32(vBytesAddress + 4);
                                if (result > 0x401000) {
                                    result = gMrw.readInt32(result);
                                    if (result == 6619214 || result == 6357069 || result == 6619226 || result == 7340115 || result == 6488137 )//6357069
                                    {
                                        writeLogLine("怪物内存 " + Convert.ToString(address, 16));
                                        return address;

                                    }
                                }

                            }
                            s += 4;

                            vBytesAddress += 4;
                        }
                        System.GC.Collect();
                    }
                    sAddress += (uint)buffer.RegionSize;
                }

                return 0;
            } catch {
                MessageBox.Show("异常");
                return 0;
            }
        }

        private Int32 GetCodeAddr(int code) {
            return 0;
            try {
                uint sAddress = 0;
                Win32.MEMORY_BASIC_INFORMATION buffer = new MEMORY_BASIC_INFORMATION();


                while (Win32.Kernel.VirtualQueryEx((uint)gMrw.GetHandle(), sAddress, ref buffer, System.Runtime.InteropServices.Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != 0) {
                    if (buffer.Protect == 0x4) {
                        int s = 0;
                        //writeLogLine("baseAddr = " + Convert.ToString(buffer.BaseAddress, 16));

                        gMrw.readData((uint)buffer.BaseAddress, (uint)buffer.RegionSize, temp);
                        IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(temp, 0);

                        if (temp.Length < buffer.RegionSize) {
                            sAddress += (uint)buffer.RegionSize;
                            continue;
                        }


                        while (s < (buffer.RegionSize - 0x20)) {
                            int address = buffer.BaseAddress + s;

                            if (Marshal.ReadInt32(vBytesAddress) == code) {

                                int result = Marshal.ReadInt32(vBytesAddress + 4);
                                if (result > 0x401000) {
                                    result = gMrw.readInt32(result);
                                    if (result == 0x00680043 || result == 6488129 || result == 7405637 || result == 7733317 )//6357069
                                    {
                                        writeLogLine(code + ":技能内存 " + Convert.ToString(address, 16));
                                        return address;

                                    }
                                }

                            }
                            s += 4;

                            vBytesAddress += 4;
                        }
                        System.GC.Collect();
                    }
                    sAddress += (uint)buffer.RegionSize;
                }

                return 0;
            } catch {
                MessageBox.Show("异常");
                return 0;
            }
        }
        private Int32 GetCodeAddr1(int code) {
            return 0;
            try {
                uint sAddress = 0;
                Win32.MEMORY_BASIC_INFORMATION buffer = new MEMORY_BASIC_INFORMATION();


                while (Win32.Kernel.VirtualQueryEx((uint)gMrw.GetHandle(), sAddress, ref buffer, System.Runtime.InteropServices.Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != 0) {
                    if (buffer.Protect == 0x4) {
                        int s = 0;
                        //writeLogLine("baseAddr = " + Convert.ToString(buffer.BaseAddress, 16));

                        gMrw.readData((uint)buffer.BaseAddress, (uint)buffer.RegionSize, temp);
                        IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(temp, 0);

                        if (temp.Length < buffer.RegionSize) {
                            sAddress += (uint)buffer.RegionSize;
                            continue;
                        }


                        while (s < (buffer.RegionSize - 0x20)) {
                            int address = buffer.BaseAddress + s;

                            if (Marshal.ReadInt32(vBytesAddress) == code) {

                                int result = Marshal.ReadInt32(vBytesAddress + 4);
                                if (result > 0x401000) {
                                    result = gMrw.readInt32(result);
                                    if (result == 7274573)//6357069
                                    {
                                        writeLogLine(code + ":技能内存 " + Convert.ToString(address, 16));
                                        return address;

                                    }
                                }

                            }
                            s += 4;

                            vBytesAddress += 4;
                        }
                        System.GC.Collect();
                    }
                    sAddress += (uint)buffer.RegionSize;
                }

                return 0;
            } catch {
                MessageBox.Show("异常");
                return 0;
            }
        }

        private void checkMonster(int code, int addr, int r) {
            return;
            try {
                uint sAddress = 0;
                Win32.MEMORY_BASIC_INFORMATION buffer = new MEMORY_BASIC_INFORMATION();


                while (Win32.Kernel.VirtualQueryEx((uint)gMrw.GetHandle(), sAddress, ref buffer, System.Runtime.InteropServices.Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != 0) {
                    if (buffer.Protect == 0x4) {
                        int s = 0;
                        //writeLogLine("baseAddr = " + Convert.ToString(buffer.BaseAddress, 16));

                        gMrw.readData((uint)buffer.BaseAddress, (uint)buffer.RegionSize, temp);
                        IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(temp, 0);

                        if (temp.Length < buffer.RegionSize) {
                            sAddress += (uint)buffer.RegionSize;
                            continue;
                        }


                        while (s < (buffer.RegionSize - 0x20)) {
                            int address = buffer.BaseAddress + s;

                            if (Marshal.ReadInt32(vBytesAddress) == code) {

                                int result = Marshal.ReadInt32(vBytesAddress + 4);
                                if (result > 0x401000) {
                                    result = gMrw.readInt32(result);
                                    if (result == 6619214 || result == 6357069 || result == 6619226 || result == 7340115 || result == 6815811)//6357069
                                    {
                                        gMrw.writeInt32(address + 4, addr);
                                        writeLogLine("怪物内存" + Convert.ToString(address, 16));
                                        return;
                                    }
                                }

                            }
                            s += 4;

                            vBytesAddress += 4;
                        }
                        System.GC.Collect();
                    }
                    sAddress += (uint)buffer.RegionSize;
                }

                return;
            } catch {
                MessageBox.Show("异常");
                return;
            }
        }


        Equip_Info[] ms_emeny_old =
{
            new Equip_Info ( 69567,0 ,0),
            new Equip_Info ( 69568 ,0 ,0),
            new Equip_Info ( 69569,0 ,0)
        };

        Equip_Info[] ms_emeny_new =
        {
            new Equip_Info ( 64067,0 ,0),
            new Equip_Info ( 64068 ,0 ,0),
            new Equip_Info ( 64069,0 ,0)
        };



        public void checkWedding(object sender, EventArgs e) {
            Int32[] e_code = { 64760, 64761, 64762 };
            int check = GetMonsterDirAddress(110110);

            foreach (int m in e_code)
            {
                gMrw.writeInt32(GetMonsterDirAddress(m) + 4, gMrw.readInt32(check + 4));

            }

            //checkMonster(64056, addr, result);//RX-78

            //checkMonster(64064, addr, result);//瘟疫小恶魔
            //checkMonster(69018, addr, result);//噩梦之种
            //checkMonster(56499, addr, result);//RX-78



        }

        public void checkBMCode()
        {
            Int32[] e_code = { 61147, 107001019, 107001012, 107001011, 107001013, 107001014, 107001015, 107001018 };
            int check = GetMonsterDirAddress(Int32.Parse(textBox1.Text));

            foreach (int m in e_code)
            {
                //gMrw.writeInt32(GetMonsterDirAddress(m) + 4, gMrw.readInt32(check + 4));
                fun.EncryptionCall(fun.LoadCall(baseAddr.GetIndexObj.Monster, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(m) + 4))) + 0x8E4, -10000000);
                fun.EncryptionCall(fun.LoadCall(baseAddr.GetIndexObj.Monster, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(m) + 4))) + 0x8EC, -10000000);

            }
        }

        private void button35_Click(object sender, EventArgs e) {



            gMrw.writeInt32(GetMonsterDirAddress(61147) + 4, gMrw.readInt32(GetMonsterDirAddress(110110) + 4));
            gMrw.writeInt32(GetMonsterDirAddress(107001019) + 4, gMrw.readInt32(GetMonsterDirAddress(110110) + 4));
            gMrw.writeInt32(GetMonsterDirAddress(107001012) + 4, gMrw.readInt32(GetMonsterDirAddress(110110) + 4));
            gMrw.writeInt32(GetMonsterDirAddress(107001011) + 4, gMrw.readInt32(GetMonsterDirAddress(110110) + 4));
            gMrw.writeInt32(GetMonsterDirAddress(107001013) + 4, gMrw.readInt32(GetMonsterDirAddress(110110) + 4));
            gMrw.writeInt32(GetMonsterDirAddress(107001014) + 4, gMrw.readInt32(GetMonsterDirAddress(110110) + 4));
            gMrw.writeInt32(GetMonsterDirAddress(107001015) + 4, gMrw.readInt32(GetMonsterDirAddress(110110) + 4));
            gMrw.writeInt32(GetMonsterDirAddress(107001018) + 4, gMrw.readInt32(GetMonsterDirAddress(110110) + 4));



            //checkMonster(64056, addr, result);//RX-78
            //checkMonster(64064, addr, result);//瘟疫小恶魔
            //checkMonster(69018, addr, result);//噩梦之种
            //checkMonster(56499, addr, result);//RX-78


        }

        private void button36_Click(object sender, EventArgs e) {
            //writeLogLine(fun.LoadCall(baseAddr.GetIndexObj.Code, gMrw.readString(gMrw.readInt32(GetCodeDirAddress(11501) + 4))).ToString());
            for (int i = 0;i < 10;i++)
                fun.texiao(1203);
           // fun.attack(0x723aa800, 0x72da0000);
            
            //fun.test();
            //fun.ArrangeBag(1);
            //fun.GetBossRoom();
            //int oldProtect = 0;
            ////Win32.Kernel.VirtualProtectEx(IntPtr.Add(IntPtr.Zero, -1), (IntPtr)GetModuleHandle(0), 4, 4, ref oldProtect);
            ////buffer[0] = 99;
            ////WriteProcessMemory(0xFFFFFFFF, (uint)GetModuleHandle(0), buffer, 4, 0);
            ////Win32.Kernel.VirtualProtectEx(IntPtr.Add(IntPtr.Zero,-1), (IntPtr)GetModuleHandle(0), 4, oldProtect, ref oldProtect);




            ////ReadProcessMemory(0xFFFFFFFF, (uint)GetModuleHandle(0), buffer, 4, 0);
            ////writeLogLine(buffer[0].ToString());

            ////fun.CommplteAllQuest();
            //at.clear();
            //at.pushad();
            //at.push(-4);
            //at.push((Int32)AssemblyTools.FindWindow("地下城与勇士", "地下城与勇士"));
            //at.mov_eax(at.GetWindowLongW);
            //at.call_eax();
            //at.mov_100100_eax();
            //at.mov_virtualaddr_c3();
            //at.popad();
            //at.retn();
            //at.RunRempteThreadWithMainThread();

            //writeLogLine(Convert.ToString(gMrw.readInt32(at.GetVirtualAddr() + 0x9B0), 16));
            //writeLogLine("hWNd = " + at.GetWindowLongW);
        }


        public void CheckEquip(int addr = 0) {
            if (addr == 0)
                addr = gMrw.readInt32(baseAddr.dwBase_Character);

            int head = gMrw.readInt32(addr + 0x2EF8, 0x10D8, 0xC, 0xC, 0x0, 0x4E8);

            if (head == 0)
                MessageBox.Show("获取失败");

            gMrw.writeFloat(gMrw.readInt32(head + 0x18, 4, 4) + 4, (float)2);//触发对象
            gMrw.writeFloat(gMrw.readInt32(head + 0x18, 4, 4) + 8, (float)1500);//范围

            gMrw.writeFloat(gMrw.readInt32(head + 0x18, 4, 0x54) + 8, (float)100);//等级
            gMrw.writeFloat(gMrw.readInt32(head + 0x18, 4, 0x18) + 8, (float)300000);//时间
            gMrw.writeFloat(gMrw.readInt32(head + 0x18, 4, 0x54) + 0xC, (float)5000000);//对自己伤害
            gMrw.writeFloat(gMrw.readInt32(head + 0x18, 4, 0x54) + 0x10, (float)520131);//对自己伤害

            gMrw.writeFloat(gMrw.readInt32(head + 0x18, 4, 0x2C) + 4, (float)100);//概率

        }

        private void button37_Click(object sender, EventArgs e) {
            gMrw.writeInt32(GetCodeDirAddress(10676) + 4, gMrw.readInt32(GetCodeDirAddress(19005) + 4));
            gMrw.writeInt32(GetCodeDirAddress(19006) + 4, gMrw.readInt32(GetCodeDirAddress(11501) + 4));

        }

        public void check_doll_equip() {
            for (int i = 0; i < 3; i++) {
                gMrw.writeInt32(doll_equip_old[i].addr + 4, doll_equip_new[i].dir_addr);
            }
            writeLogLine("开始召唤，替换装备");
        }

        public void restore_doll_equip() {
            for (int i = 0; i < 3; i++) {
                gMrw.writeInt32(doll_equip_old[i].addr + 4, doll_equip_old[i].dir_addr);
            }
            writeLogLine("召唤成功，还原装备");

        }

        public void check_ms_emeny() {
            for (int i = 0; i < 3; i++) {
                gMrw.writeInt32(ms_emeny_old[i].addr + 4, ms_emeny_new[i].dir_addr);
            }
            writeLogLine("下个房间就是boss，替换怪物");
        }

        public void restore_ms_emeny() {
            for (int i = 0; i < 3; i++) {
                gMrw.writeInt32(ms_emeny_old[i].addr + 4, ms_emeny_old[i].dir_addr);
            }
            writeLogLine("秒杀成功，还原怪物");

        }

        private void button39_Click(object sender, EventArgs e) {
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);
            Int32 first = gMrw.readInt32(bag + 88);
            Int32 item = first + 36;
            for (Int32 i = 0; i < 56; i++) {
                Int32 point = gMrw.readInt32(item + i * 4);
                string name = gMrw.readString(gMrw.readInt32(point + 0x24));

                if (point != 0) {
                    Int32 item_1 = gMrw.readInt32(point + 0x178);
                    if (item_1 == 6) {
                        fun.SendResolve(i + 9);
                        writeLogLine("分解：[" + name + "]");
                    }
                }
            }

        }

        private void button40_Click(object sender, EventArgs e) {
            int end = gMrw.readInt32(baseAddr.dwBase_Character, 0x7C4);
            for (int i = gMrw.readInt32(baseAddr.dwBase_Character,0x7C0);i < end; i += 0x298)
            {
                fun.EncryptionCall(i + 0x20, (int)code_hurt);
                fun.EncryptionCall(i + 0x28, 0);
            }
            //fun.OpenMjShop();
            //fun.Sell();
        }

        private void button41_Click(object sender, EventArgs e) {
            //fun.OpenCS();
            //int atk_addr = fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/event/bluemarble/attackinfo/fireexplosion_0_0.atk");
            int atk_addr = fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/actionobject/monster/guild_dungeon/defence/sury_attack2/attackinfo/sury_attack2_basic.atk.atk");
            int addr = gMrw.readInt32(baseAddr.dwBase_Character);
            Byte[] old = gMrw.readData(gMrw.read<uint>(addr) - 0x1000, 0x3000);
            gMrw.writedData((uint)at.GetVirtualAddr() + 0x2001, old, 0x3000);
            gMrw.writeInt32(addr, at.GetVirtualAddr() + 0x3001);
            at.clear();
            at.mov_eax(atk_addr);
            at.retn(4);
            int i = 0;
            foreach (byte a in at.Code)
            {
                gMrw.writeInt8(at.GetVirtualAddr() + 0xC50 + i++, a);
            }
            at.setEvent();
            gMrw.writeInt32(at.GetVirtualAddr() + 0x3001 + 0x834, at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1

        }

        private void button42_Click(object sender, EventArgs e) {
            //int atk_addr =fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/actionobject/common/attackinfo/bigboom2.atk");
            int atk_addr = fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/actionobject/monster/dimensiongate/new_pattern/goblinkingdom/attackinfo/bomb.atk");
            int head = gMrw.readInt32(atk_addr + 0x1F0);
            //gMrw.writeInt32(head, 2);
            //gMrw.writeInt32(head + 8, 2);
            fun.EncryptionCall(head + 0xC, 0x43020000);
            fun.EncryptionCall(head + 20, 200);
            fun.EncryptionCall(head + 0x1C, 500);
            head = gMrw.readInt32(head + 0x28);
            fun.EncryptionCall(head, 599999999);

            //fun.EncryptionCall();
            //int atk_addr = fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/equipmentpassiveobject/ancient_legendary/armor/140088_plate_shoe/attackinfo/earthquake_plate.atk");
            int addr = gMrw.readInt32(baseAddr.dwBase_Character);
            Byte[] old = gMrw.readData(gMrw.read<uint>(addr) - 0x1000, 0x3000);
            gMrw.writedData((uint)at.GetVirtualAddr() + 0x2001, old, 0x3000);
            gMrw.writeInt32(addr, at.GetVirtualAddr() + 0x3001);

            if (gMrw.readInt32(baseAddr.dwBase_Character, 0, 0x35C) < at.GetVirtualAddr())
            {
                at.clear();
                //at.mov_eax(atk_addr);
                //at.retn(4);
                at.mov_esp_ptr_addx(4, atk_addr);
                at.push(gMrw.readInt32(baseAddr.dwBase_Character, 0, 0x35C));
                at.retn();
                int i = 0;
                foreach (byte a in at.Code)
                {
                    gMrw.writeInt8(at.GetVirtualAddr() + 0xC50 + i++, a);
                }
            }


            at.setEvent();
            //gMrw.writeInt32(at.GetVirtualAddr() + 0x3001 + 0x834, at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1
            //gMrw.writeInt32(at.GetVirtualAddr() + 0x3001 + 0x5BC, at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1

            gMrw.writeInt32(at.GetVirtualAddr() + 0x3001 + 0x35C, at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1
        }

        private void button43_Click(object sender, EventArgs e) {
            fun.ItemCall(gMrw.readInt32(baseAddr.dwBase_Character), 1079);
        }

        public void checkCode()
        {
            Int32[] e_code = {84,502,280,272,1, 6, 5, 2, 11, 107000903, 5001, 5002, 10, 5003, 5004, 107000905, 8, 14, 107000930, 13, 4, 107000910, 7, 12, 62523, 210, 211, 50000, 107000904, 107000932, 50001, 107000920, 75100, 51, 70, 72, 50, 59523, 81, 84, 80, 82, 38, 39, 83, 50006, 59522, 65484, 77, 33, 34, 37, 50008, 50094, 59521, 50004, 75, 700, 701, 64, 1100, 65, 59520, 50063, 705, 704, 50062, 63534, 63530, 260, 62987, 270, 250, 62991, 62992, 62997, 62988, 500, 200, 501, 50095, 201, 62993, 252, 602, 62994, 262, 263, 272, 603, 280, 271, 253, 1039, 1038, 1040, 1041, 1042, 62995, 273, 261, 420, 421, 107000159, 107000160, 107000163, 65616, 65603, 64044, 59752, 70216, 63514, 65614, 65304, 120148, 65608, 65613, 65609, 64038, 66404, 63508, 58014, 400, 411, 65320, 64036, 107000200, 410, 1000, 1001, 50073, 50077, 65308, 63510, 63511, 66406, 63515, 68002, 70138, 66417, 59072, 59044, 70222, 65322, 65022, 65023, 59762, 70140, 70141, 70142, 70143, 70144, 70145, 70146, 70147, 70152, 62904, 70225, 56336, 56337, 56332, 70223, 70224, 75061, 1012, 1021, 1013, 1016, 50085, 1017, 1014, 50089, 50088, 61454, 61450, 58508, 600, 50081, 50080, 50083, 61703, 50082, 61326, 65499, 1033, 1032, 1031, 56705, 56707, 69543, 56712, 56706, 61453, 56450, 605, 61451, 69550, 61700, 65497, 604, 65498, 56036, 61330, 61583, 61320, 61321, 61325, 61307, 61304, 61202, 61323, 75085, 75086, 75101, };

            for (int i = 0; i < e_code.Length; i++)
            {
                fun.EncryptionCall(fun.LoadCall(baseAddr.GetIndexObj.Monster, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(e_code[i]) + 4))) + 0x8E4, -1000000);
                fun.EncryptionCall(fun.LoadCall(baseAddr.GetIndexObj.Monster, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(e_code[i]) + 4))) + 0x8EC, -1000000);
            }
        }

        public void checkSYCode()
        {
            Int32[] e_code = { 70315, 69710, 71007, 71009, 71003, 71008, 71010 };
            for (int i = 0; i < e_code.Length; i++)
            {
                fun.EncryptionCall(fun.LoadCall(baseAddr.GetIndexObj.Monster, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(e_code[i]) + 4))) + 0x8E4, -1000000000);
                fun.EncryptionCall(fun.LoadCall(baseAddr.GetIndexObj.Monster, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(e_code[i]) + 4))) + 0x8EC, -1000000000);
            }
        }

        public void checkSpacialInstanceCode()
        {
            Int32[] e_code = { 61274  , 61276 ,61280 ,61275, 56051, 61277, 69207, 69222, 69205, 61333, 107005299, 61801, 61494, 63014, 63013, 63010, 59013, 59014, 61495, 61800, 56714, 61710, 61482, 61483, 61437, 61442, 61438, 61443, 61441, 56001, 61458, 61804, 61802, 61306, 65450, 56022, 61236, 61113, 61803, 56106, 61112, 56503, 61709, 61216, 61006, 61005, 61009, 56002, 56005, 61014, 61013, 1039, 1040, 200, 69501, 69502, 63516, 63715, 69571, 59525, 107000178, 63022, 63023, 68009, 59046, 63021, 61603, 61604, 63395, 69575, 69573, 56457, 68007, 69574, 69022, 69020, 65314, 58528, 69572, 64039, 65027, 65641, 63512, 65642, 69117, 65645, 58527, 65643, 63513, 70219, 64040, 59505, 64066, 65028, 65031, 59054, 59053, 65029, 63719, 63716, 65306, 63717, 63718, 59754, 64047, 69270, 65302, 107003979, 65301, 107003980, 107003978, 107003999, 65305, 107003981, 107003982, 107004001, 107003983, 107004002, 65311, 65604, 65611, 58501, 107004004, 62898, 62897, 1070, 1071, 1082, 1069, 1073, 50084, 1072, 61320, 61307, 61325, 61304, 61321, 56302, 61202, 61323, 61328, 61464, 62016, 62015, 62021, 62014, 56009, 62012, 1079, 1097, 50092, 1078, 61187, 61220, 61186, 61218, 61245, 61244, 61268, 61243, 61246, 56029, 61266, 61191, 61219, 56139, 62106, 56008, 61192, 61221, 61222, 56137, 61223, 69228, 69229, 61455, 51, 62519, 62518, 62002, 62517, 56162, 62112, 64004, 64005, 64003, 64002, 64006, 56469, 61742, 63041, 63040, 63038, 63042, 63039, 63026, 63025, 59037, 63024, 62513, 62123, 62122, 62516, 62121, 56159, 63031, 61627, 61628, 61630, 61629, 61631, 64000, 56476, 61614};
            for (int i = 0; i < e_code.Length; i++)
            {
                fun.EncryptionCall(fun.LoadCall(baseAddr.GetIndexObj.Monster, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(e_code[i]) + 4))) + 0x8E4, -1000000000);
                fun.EncryptionCall(fun.LoadCall(baseAddr.GetIndexObj.Monster, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(e_code[i]) + 4))) + 0x8EC, -1000000000);
            }
        }

        private void button43_Click_1(object sender, EventArgs e) {
            Int32[] e_code = { 70038, 70039, 70040, 70037, 70041, 56804, 70034, 56805 };
            int check = GetMonsterDirAddress(Int32.Parse(textBox1.Text));


            foreach (int m in e_code)
            {
                gMrw.writeInt32(GetMonsterDirAddress(m) + 4, gMrw.readInt32(check + 4));

            }


            //writeLogLine(gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Character) + 0xAC).ToString());

            //Int32 addr = fun.LoadCall(baseAddr.GetIndexObj.Monster, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(50501) + 4)));
            //addr = gMrw.readInt32(addr + 0xC94);
            //gMrw.writeInt32(addr + 0xC, 10000000);
            //gMrw.writeInt32(addr + 0x10, 100000);
            //gMrw.writeInt32(addr + 0x14, 100);


            //writeLogLine(gMrw.readInt32(addr + baseAddr.dwOffset_Equip_wq, 0x20).ToString());

            //Int32 old = GetEquipDirAddress_test(29906);
            //gMrw.writeInt32(old + 4, gMrw.readInt32(GetEquipDirAddress_test(102000532) + 4));
            //old = GetCodeDirAddress(140228);
            //gMrw.writeInt32(old + 4, gMrw.readInt32(GetCodeDirAddress(140000) + 4));
            //addr = fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/EquipmentPassiveObject/Anton/140000_tonfa/attackinfo/shout_item.atk");
            //fun.EncryptionCall(addr + 0x20, 4500000);
        }

        private void button45_Click(object sender, EventArgs e) {

        }

        private void groupBox5_Enter(object sender, EventArgs e) {

        }

        public void checkGTKx()
        {
            Int32[] e_code = { 70038, 70039, 70040, 70037, 70041, 56804, 70034,56805 };
            foreach (int m in e_code)
            {
                fun.EncryptionCall(fun.LoadCall(baseAddr.GetIndexObj.Monster, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(m) + 4))) + 0x794, -1000000);
                fun.EncryptionCall(fun.LoadCall(baseAddr.GetIndexObj.Monster, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(m) + 4))) + 0x79C, -1000000);
            }
        }

        public void checkGLDKx()
        {
            Int32[] e_code = { 61272, 61273, 61634, 64022, 59518, 56055, 64016, 61295 };

            //int check = GetMonsterDirAddress(110110);
            int check = GetMonsterDirAddress(Int32.Parse(textBox1.Text));


            foreach (int m in e_code)
            {
                fun.EncryptionCall(fun.LoadCall(baseAddr.GetIndexObj.Monster, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(m) + 4))) + 0x794, -1000000);
                fun.EncryptionCall(fun.LoadCall(baseAddr.GetIndexObj.Monster, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(m) + 4))) + 0x79C, -1000000);
            }
        }

        public void check28()
        {
            int new_addr, old_addr;
            new_addr = GetEquipDirAddress_test(102010595);

            old_addr = GetEquipDirAddress_test(560001);

            if (new_addr == 0)
            {
                MessageBox.Show("new 错误");
            }

            if (old_addr == 0)
            {
                MessageBox.Show("old 错误");
            }
            gMrw.writeInt32(old_addr + 4, gMrw.readInt32(new_addr + 4));
        }

        private void button44_Click(object sender, EventArgs e) {

            Int32[] e_code = { 61272, 61273, 61634, 64022, 59518, 56055, 64016, 61295 };

            //int check = GetMonsterDirAddress(110110);
            int check = GetMonsterDirAddress(Int32.Parse(textBox1.Text));

            
            foreach (int m in e_code)
            {
                gMrw.writeInt32(GetMonsterDirAddress(m) + 4, gMrw.readInt32(check + 4));
            }
        }

        private void Lj() {
            Int32 CharaPos = gMrw.readInt32(gMrw.readInt32(baseAddr.dwBase_Role) + 0x15C);

            while (true) {
                Thread.Sleep(2000);

                this.fun.DealNoUse();
                Thread.Sleep(2000);

                fun.ChooseChara();
                while (gMrw.readInt32(baseAddr.dwBase_Character) > 0)
                    Thread.Sleep(100);
                fun.EnterChara(++CharaPos);
                while (gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                    Thread.Sleep(100);
            }
        }

        Thread Deal = null;

        private void button46_Click(object sender, EventArgs e) {
            if (Deal == null)
                Deal = new Thread(Lj);

            Button temp = (Button)sender;
            if (temp.Text == "处理垃圾") {
                temp.Text = "结束";
                Deal.Start();
            } else {
                temp.Text = "开启";
                Deal.Abort();
                Deal = null;
            }
        }

        private void EDIT_Log_TextChanged(object sender, EventArgs e) {

        }

        void writeSaleLine(string s) {
            textBox24.AppendText(s + "\r\n");
        }

        void scanThread() {
            Int32 BuyPrice = Int32.Parse(textBox25.Text);
            Int32 SalePoint = gMrw.readInt32(baseAddr.dwBase_Shop, 0x88, 0x130);

            while (true) {
                fun.SaleSearch(3037);
                while (gMrw.readInt32(baseAddr.dwBase_Shop, 0x88, 0x130, 0x3BC, 0x468, 0x20) != 3037) Thread.Sleep(1);
                Int32 line = 0;
                while (gMrw.readInt32(SalePoint + 0x3C4 + 8 * line) == 0) Thread.Sleep(1);

                Int32 point = gMrw.readInt32(SalePoint + 0x3BC + 8 * line);
                Int32 price = gMrw.readInt32(point + 0x464);
                Int32 count = gMrw.readInt32(point + 0x3F1);
                Int32 i_point = gMrw.readInt32(point + 0x468);

                while (price * count > 800000000) {
                    point = gMrw.readInt32(SalePoint + 0x3BC + 8 * (++line));
                    price = gMrw.readInt32(point + 0x464);
                    count = gMrw.readInt32(point + 0x3F1);
                }

                if (price * count >= 10000000)
                    count = 10000000 / price;

                if (price < BuyPrice) {
                    fun.BuyCall(line, count);
                    writeSaleLine("[" + DateTime.Now.ToString() + "]" + ": " + "买入[" + gMrw.readString(gMrw.readInt32(i_point + 0x24)) + "]" + count + "个," + "单价:[" + price + "] , 总价:[" + price * count + "]");
                    Thread.Sleep(2000);
                }
                gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Shop, 0x88, 0x130, 0x3BC, 0x468) + 0x20, 0);
                Thread.Sleep(2000);

            }
        }

        struct sale_item {
            public int code;
            public int price;
        }

        struct coin_item {
            public int code;
            public int price;
        }

        int t_coin_zs = 0;
        int t_coin_num = 0;

        void scanThread_coin() {

            coin_item[] data = new coin_item[100];
            int num = 0;

            data[num].code = 2681725;
            data[num++].price = 1;

            data[num].code = 2681729;
            data[num++].price = 5;

            data[num].code = 2681734;
            data[num++].price = 10;


            Int32 SalePoint = gMrw.readInt32(baseAddr.dwBase_Shop, 0x8C, 0x120);
            Int32 BuyPrice = Int32.Parse(textBox26.Text);

            while (true) {

                for (int i = 0; i < num; i++) {



                    fun.SaleSearchCoin(data[i].code);
                    while (gMrw.readInt32(baseAddr.dwBase_Shop, 0x8C, 0x120, 0x3C0, 0x46C, 0x20) != data[i].code) Thread.Sleep(1);
                    Int32 line = 0;
                    Thread.Sleep(100);

                    while (gMrw.readInt32(SalePoint + 0x3C0 + 8 * line) == 0) Thread.Sleep(1);

                    Int32 point = gMrw.readInt32(SalePoint + 0x3C0 + 8 * line);

                    Int32 price = gMrw.readInt32(point + 0x3D1);
                    Int32 i_point = gMrw.readInt32(point + 0x46C);


                    if (price / data[i].price <= BuyPrice) {
                        fun.BuyCall(line);
                        t_coin_zs += price;
                        writeSaleLine("[" + DateTime.Now.ToString() + "]" + ": " + "买入[" + gMrw.readString(gMrw.readInt32(i_point + 0x24)) + "]" + "单价:[" + price + "]," + "总数[" + (t_coin_num += data[i].price) + "百万] " + "均价[" + t_coin_zs / t_coin_num + "]");

                    }
                    //MyKey.MKeyDownUp(0, MyKey.Chr(0x29));
                    Thread.Sleep(2000);


                }

            }
        }

        void scanThread_jz() {
            sale_item[] data = new sale_item[100];
            int num = 0;

            data[num].code = 10008064;
            data[num++].price = 400000;

            data[num].code = 10008065;
            data[num++].price = 600000;


            data[num].code = 10008066;
            data[num++].price = 800000;


            data[num].code = 10008067;
            data[num++].price = 1100000;


            data[num].code = 10008068;
            data[num++].price = 3000000;


            data[num].code = 10008069;
            data[num++].price = 3000000;


            data[num].code = 10008070;
            data[num++].price = 4000000;


            data[num].code = 10008071;
            data[num++].price = 5000000;


            data[num].code = 10008072;
            data[num++].price = 300000;


            data[num].code = 10008073;
            data[num++].price = 1000000;

            data[num].code = 10008074;
            data[num++].price = 2000000;

            data[num].code = 10008075;
            data[num++].price = 9000000;

            data[num].code = 10008076;
            data[num++].price = 9000000;

            data[num].code = 10008077;
            data[num++].price = 9000000;

            data[num].code = 10008078;
            data[num++].price = 9000000;

            data[num].code = 10008079;
            data[num++].price = 300000;

            data[num].code = 10008080;
            data[num++].price = 5000000;

            data[num].code = 10008081;
            data[num++].price = 7000000;

            data[num].code = 10008082;
            data[num++].price = 9000000;


            data[num].code = 10008083;
            data[num++].price = 9000000;


            data[num].code = 10008084;
            data[num++].price = 9000000;


            data[num].code = 10008085;
            data[num++].price = 9000000;

            Int32 SalePoint = gMrw.readInt32(baseAddr.dwBase_Shop, 0x84, 0x130);

            while (true) {
                for (int i = 0; i < num; i++) {
                    writeLogLine(data[i].code.ToString());
                    fun.SaleSearch(data[i].code);
                    while (gMrw.readInt32(baseAddr.dwBase_Shop, 0x84, 0x130, 0x38C, 0x438, 0x20) != data[i].code) Thread.Sleep(1);
                    Int32 line = 0;
                    while (gMrw.readInt32(SalePoint + 0x38C + 8 * line) == 0) Thread.Sleep(1);

                    Int32 point = gMrw.readInt32(SalePoint + 0x38C + 8 * line);
                    Int32 price = gMrw.readInt32(point + 0x480);
                    Int32 count = gMrw.readInt32(point + 0x3C1);
                    Int32 i_point = gMrw.readInt32(point + 0x438);


                    if (price * count >= 10000000)
                        count = 10000000 / price;

                    if (price < data[i].price) {
                        fun.BuyCall(line, count);
                        writeSaleLine("[" + DateTime.Now.ToString() + "]" + ": " + "买入[" + gMrw.readString(gMrw.readInt32(i_point + 0x24)) + "]" + count + "个," + "单价:[" + price + "] , 总价:[" + price * count + "]");
                        Thread.Sleep(2000);
                    }
                    Thread.Sleep(300);

                }

            }
        }

        Thread SaleScan = null;

        private void button46_Click_1(object sender, EventArgs e) {
            if (SaleScan == null)
                SaleScan = new Thread(scanThread);

            Button temp = (Button)sender;
            if (temp.Text == "开始扫拍") {
                SaleScan.Start();
                temp.Text = "结束";

            } else {
                temp.Text = "开始扫拍";
                SaleScan.Abort();
                SaleScan = null;
            }
        }



        private void button48_Click(object sender, EventArgs e) {
            if (SaleScan == null)
                SaleScan = new Thread(scanThread_coin);

            Button temp = (Button)sender;
            if (temp.Text == "扫金币寄售") {
                SaleScan.Start();
                temp.Text = "结束";

            } else {
                temp.Text = "扫金币寄售";
                SaleScan.Abort();
                SaleScan = null;
            }
        }

        void check_grope() {
            int addr = 0;
            while (true) {
                if ((addr = fun.GetAddressByCode(70301)) != 0) {
                    gMrw.writeInt32(addr + 0x828, 0);
                    //writeLogLine("GG");
                }
                Thread.Sleep(100);
            }

        }


        private void button4_Click(object sender, EventArgs e) {
            Button temp = (Button)sender;
            if (temp.Text == "盗墓") {
                //timer2.Start();
                temp.Text = "停止盗墓";
                mt.StartBM();
                if (GetClearMapFunction() == 3)
                    timer2.Start();
            } else {
                //timer2.Stop();
                temp.Text = "盗墓";
                mt.Stop();
                timer2.Stop();

            }
        }

        private void button1_Click_2(object sender, EventArgs e) {

        }

        private void button1_MouseCaptureChanged(object sender, EventArgs e) {
            while ((GetAsyncKeyState(0x1) & 0x8000) != 0)
                ;

            System.Drawing.Point p = new System.Drawing.Point();
            GetCursorPos(ref p);
            IntPtr h = WindowFromPoint(p);
            StringBuilder s = new StringBuilder(512);
            GetWindowText(h.ToInt32(), s, s.Capacity);
            if (s.ToString() == "地下城与勇士") {
                load(h.ToInt32());
            }

        }

        private void button17_Click_1(object sender, EventArgs e) {
            Button temp = (Button)sender;
            if (temp.Text == "时空之门深渊") {
                temp.Text = "结束";
                mt.StartSKZM();
            } else {
                temp.Text = "时空之门深渊";
                mt.Stop();
            }
        }

        void te()
        {
            MessageBox.Show("");
            for (int i = 0x4CA165C; i < 0x4CA16B8; i += 4)
            {
                Int32 r = fun.LoadCall(i, "Priest/AntiairUpper.skl");
                if (r != 0)
                    MessageBox.Show(i.ToString());
            }
        }

        bool GetPackageHook = false;

        [Flags]
        public enum ThreadAccess
        {
            DELETE = 0x10000,
            READ_CONTROL = 0x20000,
            WRITE_DAC = 0x40000,
            WRITE_OWNER = 0x80000,
            SYNCHRONIZE = 0x100000,
            THREAD_DIRECT_IMPERSONATION = 0x200,
            THREAD_GET_CONTEXT = 0x8,
            THREAD_IMPERSONATE = 0x100,
            THREAD_QUERY_INFORMATION = 0x40,
            THREAD_QUERY_LIMITED_INFORMATION = 0x800,
            THREAD_SET_CONTEXT = 0x10,
            THREAD_SET_INFORMATION = 0x20,
            THREAD_SET_LIMITED_INFORMATION = 0x400,
            THREAD_SET_THREAD_TOKEN = 0x80,
            THREAD_SUSPEND_RESUME = 0x2,
            THREAD_TERMINATE = 0x1,
            STANDARD_RIGHTS_REQUIRED = 0x000F0000,
            PROCESS_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF)
        }
        [DllImport("kernel32.dll", EntryPoint = "OpenThread")]
        private static extern IntPtr OpenThread(
    ThreadAccess dwDesiredAccess,
    bool bInheritHandle,
    uint dwThreadId
);

        private void button18_Click(object sender, EventArgs e) {
            //charaPos = gMrw.readInt32(gMrw.readInt32(baseAddr.dwBase_Role) + 0x15C);





            //int addr = gMrw.read<int>(baseAddr.dwBase_Character);

            //if (gMrw.readInt32(at.GetVirtualAddr() + 0x2001) == 0)
            //{
            //    Byte[] old = gMrw.readData(gMrw.read<uint>(addr) - 0x1000, 0x3000);
            //    gMrw.writedData((uint)at.GetVirtualAddr() + 0x2001, old, 0x3000);
            //    gMrw.writeInt32(at.GetVirtualAddr() + 0x3001 + 0x440, 0x2442844);

            //}

            //if (gMrw.readInt32(addr) != at.GetVirtualAddr() + 0x3001)
            //{
            //    gMrw.writeInt32(addr, at.GetVirtualAddr() + 0x3001);
            //}


            //fun.CityTp(MapInfo.mapInfo[comboBox1.SelectedIndex].bigRegionID, MapInfo.mapInfo[comboBox1.SelectedIndex].smallRegionID, MapInfo.mapInfo[comboBox1.SelectedIndex].x, MapInfo.mapInfo[comboBox1.SelectedIndex].y);
            //fun.Killed44();
            //fun.ChooseInstance();
            //fun.SendEnterJxInstance();

            //fun.GiveUpSecondaryJob();
            //at.clear();
            //at.pushad();
            ////at.mov_eax(0x100100);
            ////at.push_eax();
            //at.mov_ecx(gMrw.readInt32(baseAddr.dwBase_Character));
            //at.mov_ebx(0x0271D790);//0271D790    55              push ebp

            //at.call_ebx();
            ////at.add_esp(0x4);
            //at.popad();
            //at.retn();

            //at.RunRemoteThread();
            //fun.EnterCSK(98, 31, 256, 287);
            //int hThread = (int)OpenThread(ThreadAccess.PROCESS_ALL_ACCESS, false, (uint)gMrw.tid);
            //Win32.Kernel.SuspendThread((IntPtr)hThread);
            //Thread.Sleep(1000);
            //Win32.Kernel.ResumeThread((IntPtr)hThread);
            //CloseHandle(hThread);

            //PackageData.eventQueues.Clear();
            //PackageData.tick = DateTime.Now.Ticks / 10000000;

            if (GetPackageHook == false)
            {
                GetPackageHook = true;
                //Config.IsBindMainThread = false;
                //PackageData.MapID = MapInfo.mapInfo[comboBox1.SelectedIndex].MapID[comboBox2.SelectedIndex].ID;
                //PackageData.MapLevel = comboBox3.SelectedIndex;
                //PackageData.mapType = MapInfo.mapInfo[comboBox1.SelectedIndex].MapID[comboBox2.SelectedIndex].mapType;
                //PackageData.CharaPos = gMrw.readInt32(gMrw.readInt32(baseAddr.dwBase_Role) + 0x15C);
                //PackageData.IsBegin = false;
                //PackageData.IsLoad = false;
                //PackageData.IsBfsLoad = false;

                //fun.ArrangeBag(1);
                at.clear();
                at.cmp_esp(baseAddr.GameRpcs + 0x195613 + 6);//10    FF15 44A33310   call dword ptr ds:[<&KERNEL32.GetTickCou>; kernel32.GetTickCount
                at.je_Int8(6);
                at.push(at.GetTickCount);
                at.retn();
                at.pushad();
                at.push_ebp_addx(0x34);
                at.push_ebp_addx(0x34);
                at.push(WM_GETPACKAGE);
                at.push((Int32)this.Handle);
                at.mov_eax(at.SendMessage);
                at.call_eax();
                at.popad();
                at.push(at.GetTickCount);
                at.retn();

                int i = 0;

                foreach (Byte b in at.Code)
                {
                    gMrw.writeInt8(at.GetVirtualAddr() + 0x400 + i++, b);
                }
                writeLogLine(Convert.ToString(gMrw.readInt32(baseAddr.GameRpcs + 0x195613 + 2, 0), 16));
                gMrw.writeInt32(gMrw.readInt32(baseAddr.GameRpcs + 0x195613 + 2), at.GetVirtualAddr() + 0x400, true);
                at.setEvent();

                //if (GetClearMapFunction() == 5)
                //    timer2.Start();

            }
            else
            {
                //if (GetClearMapFunction() == 5)
                //    timer2.Stop();
                gMrw.writeInt32(gMrw.readInt32(baseAddr.GameRpcs + 0x195613 + 2), at.GetTickCount, true);
                GetPackageHook = false;
            }


            //fun.SkillCall(gMrw.readInt32(baseAddr.dwBase_Character), 23500, 0, 200, 200, 0);
            //gMrw.writeInt8(0x2Ded710, 0xE9);

            //int hookAddr = at.GetVirtualAddr() + 0xB00;
            //uint data = (uint)hookAddr - 0x2Ded710 - 5;
            //gMrw.writeUInt32(0x2Ded711, data);

            //byte[] hook = { 0xC7, 0x45, 0x08, 0xF3, 0xD4, 0x01, 0x00, 0xC7, 0x45, 0x0C, 0x4E, 0x00, 0x00, 0x00, 0x53, 0x56, 0x57, 0x50, 0x83, 0xEC, 0x0C, 0x53 };
            //gMrw.writedData((uint)hookAddr, hook, (uint)hook.Length);
            //gMrw.writeInt8(hookAddr + 0x16, 0x68);
            //gMrw.writeInt32(hookAddr + 0x17, 0x2Ded715);
            //gMrw.writeInt8(hookAddr + 0x1B, 0xC3);

            //gMrw.writeInt32(GetCodeDirAddress(140258) + 4, gMrw.readInt32(GetCodeDirAddress(140223) + 4));
        }

        private void button50_Click(object sender, EventArgs e) {


            Button temp = (Button)sender;
            if (temp.Text == "开始")
            {

                mt.LevelStart();
                temp.Text = "结束";
                timer2.Start();
            }
            else
            {
                temp.Text = "开始";
                mt.LevelStart();
                timer2.Stop();
            }
        }

        void fff()
        {
            Int32 CharaPos = gMrw.readInt32(gMrw.readInt32(baseAddr.dwBase_Role) + 0x15C);
            for (; ; )
            {
                fun.AcceptQuest(3868);
                while (!fun.IsHaveQuest(3868)) Thread.Sleep(1);
                fun.CompletingQuest(3868);
                while (fun.GetQuestCount(3868) != 0) Thread.Sleep(1);
                fun.CommittingQuest(3868);
                while (fun.IsHaveQuest(3868)) Thread.Sleep(1);
                fun.ChooseChara();
                while (gMrw.readInt32(baseAddr.dwBase_Character) > 0)
                    Thread.Sleep(1);
                Thread.Sleep(5000);
                fun.EnterChara(CharaPos);
                while (gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                    Thread.Sleep(1);
            }
        }

        private void button51_Click(object sender, EventArgs e) {
            //new Thread(fff).Start();
            int atk_addr = fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/monster/evileye/attackinfo/chargedlaser_item.atk");

            int head = gMrw.readInt32(atk_addr + 0x1F0);
            writeLogLine(Convert.ToString(head, 16));

            int end = gMrw.readInt32(atk_addr + 0x1F4);
            for (int i = head; i <= end; i += 4)
            {
                writeLogLine((i - head) + ":" + Convert.ToString(gMrw.readInt32(i), 16));
                if (gMrw.Decryption(i)  <= 10000)
                {
                    writeLogLine((i - head) + ":" + Convert.ToString(gMrw.Decryption(i), 16));
                }
            }

            //int atk_addr_1 = fun.LoadCall(baseAddr.GetIndexObj.Atk, "actionobject/monster/anton_quest/phase3/bugs/poison/attackinfo/poison_bottom_basic_1.atk");
            //int atk_addr_2 = fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/actionobject/common/attackinfo/archer_atk.atk");

            //writeLogLine(Convert.ToString(atk_addr_1, 16));
            //writeLogLine(Convert.ToString(atk_addr_2, 16));


            //for (int i = 0;i < 0x298; i+=4)
            //{
            //    if (gMrw.readInt32(atk_addr_1 + i) != 0 && gMrw.readInt32(atk_addr_2 + i) == 0)
            //    {
            //        writeLogLine(Convert.ToString(i,16) + ":" + Convert.ToString(gMrw.readInt32(atk_addr_1 + i), 16));
            //    }
            //}

            //fun.CreateEmery(gMrw.readInt32(baseAddr.dwBase_Character), 0, 60004);
            //int addr;
            //while ((addr = fun.GetAddressByCode(60004)) == 0) Thread.Sleep(100);
            //gMrw.writeInt32(addr + 0x828, 0);

            //checkBMCode();

            //writeLogLine(fun.LoadCall(baseAddr.GetIndexObj.Equip, "character/partset/3choroset.equ").ToString());

            //Int32 cPoint = gMrw.readInt32(baseAddr.dwBase_Character);
            //for (Int32 i = baseAddr.dwOffset_Equip_wq; i <= baseAddr.dwOffset_Equip_wq + 0x2C; i += 4)
            //{
            //    Int32 addr = gMrw.readInt32(cPoint + i);
            //    if (addr == 0)
            //        continue;

            //    fun.EncryptionCall(addr + 0xE90, 14);
            //    fun.EncryptionCall(addr + 3776, 3);

            //}
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(61272) + 4))) + 0x8E4, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(61634) + 4))) + 0x8E4, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(61273) + 4))) + 0x8E4, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(56055) + 4))) + 0x8E4, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(64016) + 4))) + 0x8E4, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(64022) + 4))) + 0x8E4, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(61295) + 4))) + 0x8E4, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(61295) + 4))) + 0x8E4, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(59518) + 4))) + 0x8E4, -100000);

            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(59518) + 4))) + 0x8EC, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(61272) + 4))) + 0x8EC, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(61634) + 4))) + 0x8EC, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(61273) + 4))) + 0x8EC, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(56055) + 4))) + 0x8EC, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(64016) + 4))) + 0x8EC, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(64022) + 4))) + 0x8EC, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(61295) + 4))) + 0x8EC, -100000);
            //fun.EncryptionCall(fun.LoadCall(0x4CA1660, gMrw.readString(gMrw.readInt32(GetMonsterDirAddress(61295) + 4))) + 0x8EC, -100000);
            //fun.CreateEmery(gMrw.readInt32(baseAddr.dwBase_Character), 999, 59008);
            //gMrw.writeInt32(GetCodeDirAddress(19006) + 4, gMrw.readInt32(GetCodeDirAddress(26177) + 4));
        }

        private void button52_Click(object sender, EventArgs e) {
            Int32 point = gMrw.readInt32(baseAddr.dwBase_Character, 0x649C, 0x58, 0xC);
            Int32 twice = gMrw.Decryption(point + 0x2A0);
            for (int i = 0; i < twice; i++) {
                fun.SendOpenPackage(3);
                Thread.Sleep(50);
            }
        }

        private void button38_Click(object sender, EventArgs e) {

            gMrw.writeInt32(GetMonsterDirAddress(70000) + 4, gMrw.readInt32(GetMonsterDirAddress(Int32.Parse(textBox1.Text)) + 4));

            //writeLogLine(gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq).ToString());
            //fun.movCharaPos(100, 200, 0);
        }

        private int GetMonsterAddress(int code) {
            return 0;
            try {
                uint sAddress = 0;
                Win32.MEMORY_BASIC_INFORMATION buffer = new MEMORY_BASIC_INFORMATION();


                while (Win32.Kernel.VirtualQueryEx((uint)gMrw.GetHandle(), sAddress, ref buffer, System.Runtime.InteropServices.Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != 0) {
                    if (buffer.Protect == 0x4) {
                        int s = 0;
                        //writeLogLine("baseAddr = " + Convert.ToString(buffer.BaseAddress, 16));

                        gMrw.readData((uint)buffer.BaseAddress, (uint)buffer.RegionSize, temp);
                        IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(temp, 0);

                        if (temp.Length < buffer.RegionSize) {
                            sAddress += (uint)buffer.RegionSize;
                            continue;
                        }


                        while (s < (buffer.RegionSize - 0x20)) {
                            int address = buffer.BaseAddress + s;

                            if (Marshal.ReadInt32(vBytesAddress) == code) {
                                int result = gMrw.readInt32(Marshal.ReadInt32(vBytesAddress + 4));

                                if (result == 6619214 || result == 6357069 || result == 6619226 || result == 7340115 || result == 7667783)//6357069
                                {
                                    writeLogLine("秒杀内存" + Convert.ToString(address, 16));
                                    return address;
                                }
                            }
                            s += 4;
                            vBytesAddress += 4;
                        }
                        System.GC.Collect();
                    }
                    sAddress += (uint)buffer.RegionSize;
                }

                return 0;
            } catch {
                MessageBox.Show("异常");
                return GetEquipDirAddress(code);
            }
        }

        private void button23_Click(object sender, EventArgs e) {


            //fun.EnterInstance(8508, 0);
            //Thread.Sleep(1000);
            //fun.EnterInstance(8508, 1);
            //Thread.Sleep(1000);
            // fun.EnterInstance(8508, 2);
            //Thread.Sleep(1000);
            //fun.EnterInstance(8508, 3);
            //Thread.Sleep(1000);
            //fun.EnterInstance(8508, 4);

            //writeLogLine(gMrw.SearchSignature(new byte[] { 0x56, 0xBE, 0x90, 0xF0, 0x00, 0x00, 0x85, 0xC9 }, "xxxxxxxx", 0x401000, 0).ToString());
            //fun.CheckCodeHurt(8067, 999999999);
            //fun.UseItem(0);

            //test(new byte[]{99,99,99 });
            //Win32.MEMORY_BASIC_INFORMATION b = new MEMORY_BASIC_INFORMATION();
            //int re = Win32.Kernel.VirtualQueryEx((uint)gMrw.GetHandle(), 0, ref b, System.Runtime.InteropServices.Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
            //writeLogLine("返回值 = " + re);
            //writeLogLine("内存块大小 = " + b.RegionSize);
            //writeLogLine("内存块保护属性 = " + b.Protect);

        }

        private void button22_Click(object sender, EventArgs e) {
            for (int i = 0; i < Int32.Parse(Coin_Num.Text); i++) {
                fun.SendOpenCoin();
                Thread.Sleep(50);
            }
        }

        private void timer5_Tick(object sender, EventArgs e) {
            //MyKey.MKeyUp();

            Random r = new Random();
            Int32 re = r.Next(0, 7);

            if (gMrw.readInt32(baseAddr.dwBase_Map_ID) > 0 && gMrw.readInt32(baseAddr.dwBase_Character, 0x918) == 0) {
                gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0x918, 1);
                gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0xA04, 1);
            }

            switch (re) {
                case 1:
                    //fun.KeyPress('D');
                    break;
                case 2:
                    //fun.KeyPress((int)Keys.Space);
                    break;
                case 3:
                    ////fun.KeyPress((int)Keys.I);
                    break;
                case 4:
                    //fun.KeyPress((int)Keys.S);
                    break;
                case 5:
                    //fun.KeyPress((int)Keys.Left);
                    break;
                case 6:
                    //fun.KeyPress((int)Keys.Right);
                    break;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e) {
            FunctionID_1 = 5;
        }

        private void button13_Click(object sender, EventArgs e) {

        }

        private void timer4_Tick(object sender, EventArgs e) {
            //if (gMrw.readInt32(baseAddr.dwBase_Map_ID) >= 0)
                //fun.KeyPress((int)Keys.Space);
        }

        static string ininame = @".\skill.ini";

        public static void w_ini(string Section, string Key, string Value) {
            if (Value == "True") {
                Value = "1";
            }
            if (Value == "False") {
                Value = "0";
            }
            WritePrivateProfileStringA(Section, Key, Value, ininame);
        }

        //读INI         
        public static string r_ini(string Section, string Key) {
            StringBuilder temp = new StringBuilder(1024);
            int i = GetPrivateProfileStringA(Section, Key, "", temp, 1024, ininame);
            String flag = temp.ToString();
            return flag;
        }

        public void checkSkill() {
            DataHead = gMrw.readInt32(pEquip + 0xF78);
            if (DataHead == 0 || gMrw.readInt32(pEquip + 0x20) != 105006) {
                writeLogLine("初始化创建");
                pEquip = fun.CreateEquit(105006);
                DataHead = gMrw.readInt32(pEquip + 0xF78);
            }
            //writeLogLine(DataHead.ToString());
            Int32 cPoint = gMrw.readInt32(baseAddr.dwBase_Character);

            if (DataHead == 0) {
                MessageBox.Show("请将鼠标对准冥焰穿刺");
                return;
            }
            Int32 num = 0;
            for (Int32 i = baseAddr.dwOffset_Equip_wq; i <= baseAddr.dwOffset_Equip_wq + 0x2C; i += 4) {
                if (gMrw.readInt32(cPoint + i) == 0)
                    continue;
                if (i == baseAddr.dwOffset_Equip_wq + 0x4)
                    continue;
                writeLogLine(gMrw.Decryption(gMrw.readInt32(cPoint + i) + 0xF8C).ToString());
                gMrw.Encryption(gMrw.readInt32(cPoint + i) + 0xF8C, 114);
                gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0xF9C, 1);

                if (num++ >= 3)
                    break;
            }

            if (num < 3) {
                MessageBox.Show("请至少穿戴三件以上装备");
                return;
            }
            writeLogLine(Convert.ToString(DataHead, 16));
            DataHead = gMrw.readInt32(DataHead + 0xC, 0xC, 0, 0x480);
            writeLogLine(Convert.ToString(DataHead, 16));


            string name = fun.GetCharaName();
            Int32 SkillID_1 = Int32.Parse(r_ini(name, "1_技能ID"));
            Int32 SkillID_2 = Int32.Parse(r_ini(name, "2_技能ID"));
            Int32 SkillID_3 = Int32.Parse(r_ini(name, "3_技能ID"));


            Int32 SkillID = gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Character, 0x63F0, 0x88, 0) + 0x6C);

            if (SkillID_1 == 0)
                SkillID_1 = SkillID;
            if (SkillID_2 == 0)
                SkillID_2 = SkillID;
            if (SkillID_3 == 0)
                SkillID_3 = SkillID;

            // Int64 SkillID = gMrw.readInt64(gMrw.readInt32(baseAddr.dwBase_Character, 0x5D64, 0x88, 0) + 0x6C);

            writeLogLine("技能id" + SkillID.ToString());
            int temp = DataHead;
            Int32 CharaID = gMrw.readInt32(baseAddr.dwBase_Role_Id);//dnf.exe+3D5BA84
            writeLogLine("角色id" + CharaID.ToString());

            Int32 lpara1_1 = Convert.ToInt32(r_ini(name, "1_属性ID_1"));
            Int32 lpara1_2 = Convert.ToInt32(r_ini(name, "1_属性ID_2"));

            Int32 lpara2_1 = Convert.ToInt32(r_ini(name, "2_属性ID_1"));
            Int32 lpara2_2 = Convert.ToInt32(r_ini(name, "2_属性ID_2"));

            Int32 lpara3_1 = Convert.ToInt32(r_ini(name, "3_属性ID_1"));
            Int32 lpara3_2 = Convert.ToInt32(r_ini(name, "3_属性ID_2"));

            Int32 spara1 = Convert.ToInt32(r_ini(name, "1_技能倍数"));
            Int32 spara2 = Convert.ToInt32(r_ini(name, "2_技能倍数"));
            Int32 spara3 = Convert.ToInt32(r_ini(name, "3_技能倍数"));

            temp += (40 * 0);
            gMrw.writeInt32(temp, CharaID);
            fun.EncryptionCall(temp + 4, SkillID);
            gMrw.writeInt32(temp + 12, 0);
            fun.EncryptionCall(temp + 16, 1);
            fun.EncryptionCall(temp + 24, -100);
            gMrw.writeInt32(temp + 32, 2);
            gMrw.writeInt32(temp + 36, 26);

            temp += (40);
            gMrw.writeInt32(temp, CharaID);
            fun.EncryptionCall(temp + 4, SkillID_1);
            gMrw.writeInt32(temp + 12, lpara1_1);
            fun.EncryptionCall(temp + 16, 1);
            fun.EncryptionCall(temp + 24, spara1);
            gMrw.writeInt32(temp + 32, lpara1_2);
            gMrw.writeInt32(temp + 36, 26);

            temp += (40);
            gMrw.writeInt32(temp, CharaID);
            fun.EncryptionCall(temp + 4, SkillID_2);
            gMrw.writeInt32(temp + 12, lpara2_1);
            fun.EncryptionCall(temp + 16, 1);
            fun.EncryptionCall(temp + 24, spara2);
            gMrw.writeInt32(temp + 32, lpara2_2);
            gMrw.writeInt32(temp + 36, 26);

            temp += (40);
            gMrw.writeInt32(temp, CharaID);
            fun.EncryptionCall(temp + 4, SkillID_3);
            gMrw.writeInt32(temp + 12, lpara3_1);
            fun.EncryptionCall(temp + 16, 1);
            fun.EncryptionCall(temp + 24, spara3);
            gMrw.writeInt32(temp + 32, lpara3_2);
            gMrw.writeInt32(temp + 36, 26);

            temp += (40);
            gMrw.writeInt32(temp, CharaID);
            fun.EncryptionCall(temp + 4, SkillID);
            gMrw.writeInt32(temp + 12, 0);
            fun.EncryptionCall(temp + 16, 1);
            fun.EncryptionCall(temp + 24, -100);
            gMrw.writeInt32(temp + 32, 3);
            gMrw.writeInt32(temp + 36, 26);

            temp += (40);
            gMrw.writeInt32(temp, CharaID);
            fun.EncryptionCall(temp + 4, SkillID);
            gMrw.writeInt32(temp + 12, 0);
            fun.EncryptionCall(temp + 16, 1);
            fun.EncryptionCall(temp + 24, -100);
            gMrw.writeInt32(temp + 32, 5);
            gMrw.writeInt32(temp + 36, 26);
        }

        private void button20_Click(object sender, EventArgs e) {
            string name = fun.GetCharaName();
            string CharaID = gMrw.readInt32(baseAddr.dwBase_Role_Id).ToString();
            w_ini(name, "角色ID", CharaID);

            w_ini(name, "1_技能ID", textBox6.Text);
            w_ini(name, "1_属性ID_1", EDIT_SK_1_SX_1.Text);
            w_ini(name, "1_属性ID_2", EDIT_SK_1_SX_2.Text);
            w_ini(name, "1_技能倍数", EDIT_SK_1_BS.Text);

            w_ini(name, "2_技能ID", textBox7.Text);
            w_ini(name, "2_属性ID_1", EDIT_SK_2_SX_1.Text);
            w_ini(name, "2_属性ID_2", EDIT_SK_2_SX_2.Text);
            w_ini(name, "2_技能倍数", EDIT_SK_2_BS.Text);

            w_ini(name, "3_技能ID", textBox5.Text);
            w_ini(name, "3_属性ID_1", EDIT_SK_3_SX_1.Text);
            w_ini(name, "3_属性ID_2", EDIT_SK_3_SX_2.Text);
            w_ini(name, "3_技能倍数", EDIT_SK_3_BS.Text);

        }

        private void button53_Click(object sender, EventArgs e) {
            checkSkill();
        }

        private void label5_Click(object sender, EventArgs e) {

        }

        private void button2_Click_3(object sender, EventArgs e) {
            GC.Collect();
            Button t = (Button)sender;
            Int32 Index = comboBox1.SelectedIndex;

            string result = MapInfo.mapInfo[Index].bigRegionID.ToString() + "|"
                + MapInfo.mapInfo[Index].smallRegionID.ToString() + "|"
                + MapInfo.mapInfo[Index].x.ToString() + "|"
                + MapInfo.mapInfo[Index].y.ToString();


            if (result == "0|0|0|0")
                MessageBox.Show("索引错误");

            if (t.Text == "开始挂机") {
                mt.StartAuto(result, MapInfo.mapInfo[Index].MapID[comboBox2.SelectedIndex].ID, comboBox3.SelectedIndex, MapInfo.mapInfo[Index].MapID[comboBox2.SelectedIndex].mapType);
                t.Text = "结束挂机";
            } else {
                mt.TermidateAuto();
                t.Text = "开始挂机";

            }
        }

        private void RB_2_TzYc_CheckedChanged_1(object sender, EventArgs e) {
            FunctionID_1 = 2;
        }

        private void RB_2_No_CheckedChanged_1(object sender, EventArgs e) {
            FunctionID_1 = 0;
        }

        private void RB_2_No_Click(object sender, EventArgs e) {

        }

        private void RB_2_CreateSkill_CheckedChanged_1(object sender, EventArgs e) {
            FunctionID_1 = 1;
        }

        private void RB_2_Freeze_CheckedChanged_1(object sender, EventArgs e) {
            FunctionID_1 = 3;
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e) {
            FunctionID_1 = 4;
        }

        private void radioButton3_CheckedChanged_1(object sender, EventArgs e) {
            FunctionID_1 = 5;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            Int32 index = comboBox1.SelectedIndex;
            comboBox2.Items.Clear();
            foreach (MapI m in MapInfo.mapInfo[index].MapID) {
                if(m.Visible) comboBox2.Items.Add(m.name);
            }
            comboBox2.SelectedIndex = 0;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
            comboBox3.Enabled = false;
            Int32 index1 = comboBox1.SelectedIndex;
            Int32 index2 = comboBox2.SelectedIndex;
            comboBox3.Items.Clear();

            string[] nd = new string[5];
            nd[0] = "普通级";
            nd[1] = "冒险级";
            nd[2] = "勇士级";
            nd[3] = "王者级";
            nd[4] = "噩梦级";

            for (int i = 0; i <= MapInfo.mapInfo[index1].maxDiff; i++)
                comboBox3.Items.Add(nd[i]);

            if (MapInfo.mapInfo[index1].MapID[index2].mapType == MapI.MapType.Othther)
                comboBox3.SelectedIndex = MapInfo.mapInfo[index1].maxDiff;
            else if (MapInfo.mapInfo[index1].MapID[index2].mapType == MapI.MapType.Spacial)
                comboBox3.SelectedIndex = 0;
            else if (MapInfo.mapInfo[index1].MapID[index2].mapType == MapI.MapType.Yuangu)
                comboBox3.SelectedIndex = 0;
            else if (MapInfo.mapInfo[index1].MapID[index2].mapType == MapI.MapType.Eiji)
                comboBox3.SelectedIndex = 0;
            else {
                comboBox3.SelectedIndex = MapInfo.mapInfo[index1].maxDiff;
                comboBox3.Enabled = true;
            }
        }

        public int GetTeamCharaID()
        {
            return Int32.Parse(textBox14.Text);
        }

        private void button49_Click(object sender, EventArgs e) {

        }

        private void checkBox4_CheckedChanged_1(object sender, EventArgs e) {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e) {
            Config.IsSellPink = checkBox3.Checked;
        }

        private void button8_Click(object sender, EventArgs e) {
            Int32 CharaPos = gMrw.readInt32(gMrw.readInt32(baseAddr.dwBase_Role) + 0x15C);

            while (true) {
                Thread.Sleep(1000);
                fun.SendGetStoreItem(3332, Int32.Parse(textBox10.Text));

                fun.ChooseChara();
                while (gMrw.readInt32(baseAddr.dwBase_Character) > 0)
                    Thread.Sleep(100);
                Thread.Sleep(1000);

                fun.EnterChara(++CharaPos);
                while (gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                    Thread.Sleep(100);
                Thread.Sleep(1000);
            }
        }

        private void checkBox4_CheckedChanged_2(object sender, EventArgs e) {
            Config.IsRoleBig = checkBox4.Checked;
        }
        
        private void button9_Click_1(object sender, EventArgs e) {

        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            Config.CloseLevelUp = ((CheckBox)sender).Checked;
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            Config.Discard = ((CheckBox)sender).Checked;

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            Config.IsHide = checkBox7.Checked;
        }

        private void button9_Click_2(object sender, EventArgs e)
        {
            WinIo.Initialize();//初始化WinIo
        }

        struct EventQueue
        {
            public Int32 id;//事件id
            public Int32 param;//事件参数
            public Int32 param1;//事件参数

            public EventQueue(Int32 _id,Int32 _param = 0,Int32 __param = 0)
            {
                id = _id;
                param = _param;
                param1 = __param;
            }
               
        }

        struct PackageData
        {
            public static bool IsOffLine = false;
            public static Point Boss;
            public static Point Current;
            public static Int32 EmeryNum = 0;
            public static Int32 KillEmery = 0;
            public static bool IsLoad = false;
            public static Int32 MapID;
            public static Int32 MapLevel;
            public static MapI.MapType mapType;
            public static Int32 CharaPos = 0;
            public static bool IsBegin = false;
            public static Queue<EventQueue> eventQueues = new Queue<EventQueue>();
            public static Int32 twice = 0;
            public static bool IsBfsLoad = false;
            public static bool IsIntoNewRoom = false;
            public static long tick;
        }

        void DealQueue()
        {
            while (true)
            {

                if (PackageData.eventQueues.Count != 0)
                {
                    PackageData.tick = (DateTime.Now.Ticks / 10000000);
              
                    EventQueue eventQueue = PackageData.eventQueues.Dequeue();
                    switch (eventQueue.id)
                    {
                        case 0:
                            {
                                fun.ChooseChara();
                                break;
                            }
                        case 1:
                            {
                                fun.CityTp(MapInfo.mapInfo[comboBox1.SelectedIndex].bigRegionID, MapInfo.mapInfo[comboBox1.SelectedIndex].smallRegionID, MapInfo.mapInfo[comboBox1.SelectedIndex].x, MapInfo.mapInfo[comboBox1.SelectedIndex].y);
                                break;
                            }
                        case 2:
                            {
                                fun.ChooseInstance();
                                break;
                            }
                        case 3:
                            {
                                MapI.MapType mapType = PackageData.mapType;
                                if (mapType == MapI.MapType.Eiji)
                                {
                                    if (fun.GetCharaLevel() >= 85)
                                        fun.EnterInstance(PackageData.MapID, 2, true, true);
                                    else if (fun.GetCharaLevel() >= 80)
                                        fun.EnterInstance(PackageData.MapID, 1, true, true);
                                    else
                                        fun.EnterInstance(PackageData.MapID, 0, true, true);

                                }
                                else if (mapType == MapI.MapType.Yuangu)
                                    fun.EnterInstance(PackageData.MapID, 2, true, true);
                                else
                                    fun.EnterInstance(PackageData.MapID, PackageData.MapLevel);
                                break;
                            }
                        case 4:
                            {
                                fun.EnterInstanceTrue();
                                break;
                            }
                        case 5:
                            {
                                fun._PickUp(eventQueue.param);
                                break;
                            }
                        case 6:
                            {
                                fun.__SendKilled(eventQueue.param, eventQueue.param1);
                                break;
                            }
                        case 7:
                            {
                                fun.EnterChara(eventQueue.param);
                                break;
                            }
                        case 8:
                            {
                                int coor = eventQueue.param;
                                if (coor == 0)
                                {
                                    fun.InstanceTp(PackageData.Current.X, PackageData.Current.Y - 1);
                                }
                                else if (coor == 1)
                                {
                                    fun.InstanceTp(PackageData.Current.X, PackageData.Current.Y + 1);

                                }
                                else if (coor == 2)
                                {
                                    fun.InstanceTp(PackageData.Current.X - 1, PackageData.Current.Y);

                                }
                                else if (coor == 3)
                                {
                                    fun.InstanceTp(PackageData.Current.X + 1, PackageData.Current.Y);
                                }
                                break;
                            }
                        case 9:
                            {
                                fun.ChooseCard(0, 0);
                                break;
                            }
                        case 10:
                            {
                                fun.GetCardTrue();
                                break;
                            }
                        case 11:
                            {
                                fun.QuitInstance();
                                break;
                            }
                        case 12:
                            {
                                fun.Resolve();
                                break;
                            }
                        case 13:
                            {
                                //while (PackageData.IsIntoNewRoom == false)
                                //{
                                //    fun.CoorTp(fun.GetNextRoom(PackageData.Current));
                                //    Thread.Sleep(1000);
                                //}
                                new Thread(new ParameterizedThreadStart(CoorTpThread)).Start(fun.GetNextRoom(PackageData.Current));
                                break;
                            }
                        case 14:
                            {
                                new Thread(ChangeCharacter).Start();

                                break;
                            }
                    }
                }
                Thread.Sleep(1);

            }

        }

        void CoorTpThread(object rg)
        {
            while (PackageData.IsIntoNewRoom == false)
            {
                fun.CoorTp(Int32.Parse(rg.ToString()));
                Thread.Sleep(500);
            }
        }

        void ChangeCharacter()
        {
            Thread.Sleep(2000);
            fun.ChooseChara();
            Thread.Sleep(2000);
            while (gMrw.readInt32(baseAddr.dwBase_Character) > 0)
                Thread.Sleep(100);
            Thread.Sleep(2000);
            fun.EnterChara(++PackageData.CharaPos);
            Thread.Sleep(2000);
            while (gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                Thread.Sleep(100);
            Thread.Sleep(2000);
            fun.CityTp(MapInfo.mapInfo[comboBox1.SelectedIndex].bigRegionID, MapInfo.mapInfo[comboBox1.SelectedIndex].smallRegionID, MapInfo.mapInfo[comboBox1.SelectedIndex].x, MapInfo.mapInfo[comboBox1.SelectedIndex].y);
            fun.ChooseInstance();
        }

        void waitKill()
        {
            //writeLogLine((DateTime.Now.Ticks / 10000000).ToString());


            while (gMrw.readInt32(baseAddr.dwBase_Map_ID) <= 0) Thread.Sleep(0);
            fun.bfs_load();

            Int32 map = gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 End = gMrw.readInt32(map + 0xC4);
            Int32 num = 0;

            Int32[] temp = new Int32[100];

            for (Int32 i = gMrw.readInt32(map + 0xC0); i < End; i += 4)
            {
                Int32 addr = gMrw.readInt32(i);
                Int32 camp = gMrw.readInt32(addr + 0x828);
                Int32 type = gMrw.readInt32(addr + 0xA4);

                if (camp != 0)
                {
                    if (type == 529 || type == 545 || type == 273)
                    {
                        temp[num++] = addr;
                    }
                }
            }
            for (Int32 i = num - 1; i >= 0; i--)
            {
                PackageData.eventQueues.Enqueue(new EventQueue(6, gMrw.Decryption(temp[i] + 0xAC)));
            }
        }

        int charaPos;

        void getPackageHook(Int32 addr)
        {

            Int32 id = gMrw.readInt16(addr + 1);
            Int32 lenth = gMrw.readInt16(addr + 3);
            gMrw.writeInt16(addr + 1, -1);

            //if (id == 0x53b)
            //{
            //    fun.EnterChara(charaPos);
            //    writeLogLine("进入角色");
            //}

            //if (id == 0xd)
            //{
            //    fun.AcceptQuest(3868);
            //    fun.CompletingQuest(3868);
            //    fun.CommittingQuest(3868);
            //    writeLogLine("领取奖励");
            //}

            //if (id == 0x15)
            //{
            //    fun.ChooseChara();
            //}

            //if (id == 0x194 || id == 0x195 )
            //{
            //    gMrw.writeInt16(addr + 1, -1);
            //}

            //Int32 id = gMrw.readInt16(addr + 1);
            //Int32 lenth = gMrw.readInt16(addr + 3);

            //if (PackageData.IsBegin == false)
            //{
            //    PackageData.eventQueues.Enqueue(new EventQueue(1, 1));
            //    PackageData.IsBegin = true;
            //    PackageData.eventQueues.Enqueue(new EventQueue(2, 1));

            //}

            //if (id == 24)
            //{
            //    if (GetClearMapFunction() == 1 && PackageData.IsLoad)
            //    {
            //        gMrw.writeInt16(addr + 1, -1);
            //    }
            //    if (PackageData.mapType == MapI.MapType.Normal && fun.GetPL() > 0)
            //        PackageData.eventQueues.Enqueue(new EventQueue(2, 1));
            //    else if (PackageData.mapType == MapI.MapType.Yuangu && fun.GetPL() >= 6)
            //        PackageData.eventQueues.Enqueue(new EventQueue(2, 1));
            //    else if (PackageData.mapType == MapI.MapType.Spacial && fun.GetPL() >= 8)
            //        PackageData.eventQueues.Enqueue(new EventQueue(2, 1));
            //    else
            //        PackageData.eventQueues.Enqueue(new EventQueue(14, 1));

            //}
            //else if (id == 27)
            //{
            //    if (GetClearMapFunction() == 1 && PackageData.IsLoad)
            //    {
            //        gMrw.writeInt16(addr + 1, -1);
            //    }
            //    PackageData.eventQueues.Enqueue(new EventQueue(3, 1));
            //    writeLogLine("进图");
            //}
            //else if (id == 28)
            //{
            //    if (GetClearMapFunction() == 1 && PackageData.IsLoad)
            //    {
            //        gMrw.writeInt16(addr + 1, -1);

            //    }

            //    if (GetClearMapFunction() != 1)
            //        PackageData.IsLoad = false;

            //    PackageData.eventQueues.Enqueue(new EventQueue(4, 1));
            //    PackageData.Boss.X = gMrw.readInt8(addr + 20 + 4);
            //    PackageData.Boss.Y = gMrw.readInt8(addr + 21 + 4);
            //    writeLogLine("BossX:" + PackageData.Boss.X + ",BossY:" + PackageData.Boss.Y);
            //}
            //else if (id == 29)
            //{
            //    if (GetClearMapFunction() == 1 && PackageData.IsLoad)
            //    {

            //        gMrw.writeInt16(addr + 1, -1);
            //    }

            //    PackageData.IsIntoNewRoom = true;
            //    PackageData.Current.X = gMrw.readInt8(addr + 16);
            //    PackageData.Current.Y = gMrw.readInt8(addr + 16 + 1);
            //    PackageData.EmeryNum = gMrw.readInt8(addr + 16 + 16 + 2);
            //    PackageData.KillEmery = 0;

            //    writeLogLine("怪物数量" + PackageData.EmeryNum + "CurrentX:" + PackageData.Current.X + ",CurrentY:" + PackageData.Current.Y);


            //    if (GetClearMapFunction() == 1 && !PackageData.IsLoad)
            //    {
            //        PackageData.IsLoad = true;
            //        new Thread(waitKill).Start();
            //        return;
            //    }


            //    Int32 tmp = 21 * (PackageData.EmeryNum - 1);
            //    Int32 bNum = gMrw.readInt8(addr + 56 + tmp);
            //    for (int i = 0; i < bNum; i++)
            //    {
            //        Int32 BID = gMrw.readInt32(addr + 58 + tmp + 21 * i);
            //        PackageData.eventQueues.Enqueue(new EventQueue(5, BID));
            //        writeLogLine("建筑物品编号" + BID);
            //    }
            //    if (PackageData.IsOffLine || GetClearMapFunction() == 1)
            //    {
            //        for (int i = 0; i < PackageData.EmeryNum; i++)
            //        {
            //            Int32 EID = (UInt16)gMrw.readUInt16(addr + 41 + 21 * i);
            //            Int32 code = gMrw.readInt32(addr + 43 + 21 * i);
            //            PackageData.eventQueues.Enqueue(new EventQueue(6, EID, code));
            //            //writeLogLine("组包秒杀" + EID);
            //        }
            //    }
            //}
            //else if (id == 38)
            //{
            //    if (GetClearMapFunction() == 1 && PackageData.IsLoad)
            //    {
            //        gMrw.writeInt16(addr + 1, -1);
            //    }
            //    if (!PackageData.IsOffLine && GetClearMapFunction() != 1 && PackageData.IsLoad == false)
            //    {
            //        PackageData.IsLoad = true;
            //        if (PackageData.IsBfsLoad == false)
            //        {
            //            fun.bfs_load();
            //            PackageData.IsBfsLoad = true;
            //        }
            //        fun.SSS();
            //        if (GetClearMapFunction() == 5)
            //            checkBMCode();
            //    }
            //    //writeLogLine("怪物死亡");
            //    PackageData.KillEmery++;
            //    if (lenth != 27)
            //    {
            //        int gNum = gMrw.readInt8(addr + 18);
            //        for (int i = 0;i < gNum; i++)
            //        {
            //            Int32 GID = gMrw.readInt32(addr + 19 + 127 * i);
            //            Int32 GCode = gMrw.readInt32(addr + 19 + 6 + 127 * i);
            //            PackageData.eventQueues.Enqueue(new EventQueue(5, GID));
            //            if (GCode == 3784)
            //            {
            //                string cname = gMrw.readString(gMrw.readInt32(baseAddr.dwBase_Character, 0x400));
            //                writeGetLine(cname + "获得：【魂：虫王】");
            //            }
            //        }
            //    }
            //    if (PackageData.KillEmery == PackageData.EmeryNum)
            //    {
            //        if (PackageData.Current.X == PackageData.Boss.X && PackageData.Current.Y == PackageData.Boss.Y)
            //            return;
            //        if (PackageData.IsOffLine || GetClearMapFunction() == 1)
            //        {
            //            int coor = fun.GetNextRoom(PackageData.Current);
            //            if (PackageData.mapType == MapI.MapType.Spacial)
            //                coor = 3;
            //            PackageData.eventQueues.Enqueue(new EventQueue(8, coor));
            //        }
            //        else
            //        {
            //            PackageData.IsIntoNewRoom = false;
            //            PackageData.eventQueues.Enqueue(new EventQueue(13, 0));
            //        }
            //        writeLogLine("顺图");
            //    }
            //}
            //else if (id == 34)
            //{
            //    PackageData.eventQueues.Enqueue(new EventQueue(9, 0));
            //    PackageData.eventQueues.Enqueue(new EventQueue(10, 0));

            //    if (PackageData.mapType == MapI.MapType.Normal && fun.GetPL() > 0)
            //        ;
            //    else if (PackageData.mapType == MapI.MapType.Yuangu && fun.GetPL() >= 6)
            //        ;
            //    else if (PackageData.mapType == MapI.MapType.Spacial && fun.GetPL() >= 8)
            //        ;
            //    else
            //        PackageData.IsLoad = false;

            //    //fun.ChooseCard(0, 0);
            //}

            //if (id == 261)
            //{
            //    if (GetClearMapFunction() == 1 && PackageData.IsLoad)
            //    {
            //        gMrw.writeInt16(addr + 1, -1);
            //    }
            //    PackageData.eventQueues.Enqueue(new EventQueue(11, 0));
            //    PackageData.eventQueues.Enqueue(new EventQueue(12, 0));
            //    writeGetLine("站街刷图:" + ++PackageData.twice + "次");

            //    //fun.QuitInstance();
            //    //fun.Resolve();
            //}
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            string r_text = textBox11.Text;
            string[] r_arry_text = r_text.Split('\n');

            for (int i = 0;i < r_arry_text.Length; i++)
            {
                int code = Int32.Parse(r_arry_text[i]);
                if (code != 0)
                {
                    int pos = fun.GetItem(code).Pos;
                    if (pos != 0)
                        fun.SendJigsaw(i, pos, code, Int32.Parse(textBox12.Text));
                    else
                        writeLogLine("未找到拼图:" + code);
                }
            }
        }

        public uint code_hurt = 49999999;

        private void textBox13_TextChanged(object _sender, EventArgs _e)
        {
            if (((TextBox)_sender).Text != "")
            {
                try
                {
                    code_hurt = uint.Parse(((TextBox)_sender).Text);
                }
                catch
                {
                    code_hurt = Int32.MaxValue;
                    ((TextBox)_sender).Text = code_hurt.ToString();
                }
            }

        }

        private void textBox13_KeyPress(object _sender, KeyPressEventArgs e)
        {
            TextBox sender = (TextBox)_sender;

            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click_3(object sender, EventArgs e)
        {
            WinIo.Initialize();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.way = comboBox4.SelectedIndex;
        }

        private void comboBox4_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Config.way = comboBox4.SelectedIndex;
        }

        private void C17_CheckedChanged(object sender, EventArgs e)
        {
            Config.isHook = C17.Checked;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            Config.sss = checkBox12.Checked;
        }
    }
}
