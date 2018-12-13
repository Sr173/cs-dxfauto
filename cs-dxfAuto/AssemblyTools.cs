using CallTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using cs_dxf;

namespace cs_dxfAuto {
    public class AssemblyTools {

        [DllImport("kernel32.dll")]
        public static extern Int32 VirtualAllocEx(
        Int32 hprocess,
        Int32 lpaddress,
        Int32 dwsize,
        Int32 flallocationtype,
        Int32 flprotect
        );

        [DllImport("User32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("kernel32", EntryPoint = "CreateRemoteThread")]
        public static extern int CreateRemoteThread(
            int hprocess,
            int lpthreadattributes,
            int dwstacksize,
            int lpstartaddress,
            int lpparameter,
            int dwcreationflags,
            int lpthreadid
             );

        [DllImport("kernel32", EntryPoint = "VirtualProtectEx")]
        public static extern int VirtualProtectEx(
    int hprocess,
    int address,
    int size,
    int newprotect,
    int old
     );

        [DllImport("kernel32.dll")]
        public static extern Int32 WaitForSingleObject(
            Int32 hHandle,
            uint Second
            );

        [DllImport("kernel32.dll ")]
        public static extern bool CloseHandle(int hProcess);

        [DllImport("kernel32.dll ")]
        public static extern Int32 GetProcAddress(IntPtr hMouddle, string name);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandleA(string name);

        [DllImport("kernel32.dll")]
        public static extern int LoadLibary(string na);
        [DllImport("user32.dll", EntryPoint = "SendMessageW")]
        private static extern int SendMessageW(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "GetWindowLongW")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        //私有变量
        private MemRWer gMrw;
        public string Code = "";
        private Int32 mhProcess;
        private Int32 virtualAddr = 0;
        private IntPtr hModule = IntPtr.Zero;
        private Int32 CallProcAddress = 0;
        public Int32 GetWindowLongW = 0;
        private Int32 SetWindowLongW = 0;
        private Int32 SetTimer = 0;
        private Int32 KillTimer = 0;
        private Int32 MyGetModuleHandleA = 0;
        public Int32 PostMessage = 0;
        public Int32 GetTickCount = 0;
        public Int32 GetModuleHandleW = 0;
        public Int32 memcpy = 0;
        public Int32 SendMessage = 0;
        public Int32 GetLastErrorC = 0;


        private List<string> Queue = new List<string>();

        
        //常量
        public AssemblyTools(Int32 hProcess, Int32 Lenth, MemRWer gMrwA, Action<string> writeLogLine) {
            if (Lenth == 0)
                Lenth = 0x1000;
            // gMrw = new MemRWer((uint)hProcess);
            mhProcess = hProcess;
            gMrw = gMrwA;

            int twice = 0;

            while (virtualAddr == 0) {
                //0331CCC0    E8 2AA9D25B     call 5F0475EF


                virtualAddr = /*VirtualAllocEx(hProcess, 0, 0x1000, 0x103000, 0x40)*/gMrw.readInt32(0x033560D0 + 1) + 0x033560D0 + 0x100;//033558A0    E8 28203C10     call 137178CD
																																		 //033558A0    E8 28203C10     call 137178CD

				//033560D0    E8 8E181A58 call 5B4F7963






							//virtualAddr = GetProcAddress(GetModuleHandleA("ntdll.dll"), "RtlFreeMemoryBlockLookaside");
							Int32 eid = (Int32)GetLastError();

                if (virtualAddr == 0) {
                    if (eid == 8L) {
                        writeLogLine("由于客户端工作集内存不足，无法分配内存 重试第 " + twice + "次" + "期间最好进行选择角色等操作");
                        Thread.Sleep(500);
                    } else {
                        writeLogLine("289行 错误代码 : " + eid);
                        return;
                    }
                }
                if (twice >= 99) {
                    writeLogLine("等待次数过多 终止重试");
                    return;
                }
            }

            hModule = GetModuleHandleA("User32.dll");

            if (hModule == IntPtr.Zero)
                MessageBox.Show("303 行 句柄错误");
            CallProcAddress = GetProcAddress(hModule, "CallWindowProcW");
            GetWindowLongW = GetProcAddress(hModule, "GetWindowLongW");
            SetWindowLongW = GetProcAddress(hModule, "SetWindowLongW");
            SetTimer = GetProcAddress(hModule, "SetTimer");
            KillTimer = GetProcAddress(hModule, "KillTimer");
            PostMessage = GetProcAddress(hModule, "PostMessageW");
            SendMessage = GetProcAddress(hModule, "SendMessageW");

            hModule = GetModuleHandleA("kernel32.dll");
            GetTickCount = GetProcAddress(hModule, "GetTickCount");
            writeLogLine("GetTickCount = " + GetTickCount);

            hModule = GetModuleHandleA("ntdll.dll");
            memcpy = GetProcAddress(hModule, "memcpy");
            writeLogLine("memcpy = " + memcpy);

            //MyGetModuleHandleA = GetProcAddress(hModule, "KillTimer");

            hModule = GetModuleHandleA("Kernel32.dll");
            MyGetModuleHandleA = GetProcAddress(hModule, "GetModuleHandleA");
            GetModuleHandleW = GetProcAddress(hModule, "GetModuleHandleW");
            GetLastErrorC = GetProcAddress(hModule, "GetLastError");
            //CloseHandle(hProcess);
            if (CallProcAddress == 0)
                MessageBox.Show("306 行 句柄错误");
        }
        public Int32 GetVirtualAddr() {
            return virtualAddr;
        }



        public void HookGetPackage()
        {

        }

        public void MessageBoxText() {
            clear();
            pushad();
            push(0);
            push(0);
            push(0);
            push(0);
            mov_eax(GetProcAddress(hModule, "MessageBoxA"));
            call_eax();
            popad();
            retn();
            RunRempteThreadWithMainThread();

        }

        public void asm_KillTimer() {
            Int32 hWnd = (Int32)FindWindow("地下城与勇士", "地下城与勇士");

            clear();
            pushad();
            push(0x100);
            push(hWnd);
            mov_eax(KillTimer);
            call_eax();
            popad();
            retn();

            RunRemoteThread();
        }

        public void Asm_SetTimer(Action<string> writeLogLine) {

            writeLogLine("VirtualAddr = " + Convert.ToString(virtualAddr, 16));
            gMrw.writeInt32(virtualAddr + 0x990, virtualAddr + 0x9A0);
            gMrw.writeInt8(virtualAddr + 0x9A0, 0xC3);
            Int32 hWnd = (Int32)FindWindow("地下城与勇士", "地下城与勇士");
            Int32 procAddr = virtualAddr + 0xF00;

            clear();
            //mov_addr_XXX(virtualAddr,virtualAddr + 0x1900);
            call_addr(virtualAddr + 0x990);
            writeInt8(0xC3);

            Int32 m = 0;
            foreach (Byte a in Code)
                gMrw.writeInt8((procAddr + m++), a);
            setEvent();


            clear();

            pushad();
            push(0x100);
            push(hWnd);
            mov_eax(KillTimer);
            call_eax();

            push(procAddr);
            push(0x1);
            push(0x100);
            push(hWnd);
            mov_eax(SetTimer);
            call_eax();
            popad();
            retn();
            RunRemoteThread();

            clear();
            gMrw.writeInt32(virtualAddr + 0x500, 1701667143);
            gMrw.writeInt32(virtualAddr + 0x504, 1935896658);
            gMrw.writeInt32(virtualAddr + 0x508, 1819042862);
            gMrw.writeInt32(virtualAddr + 0x50C, 0);
            pushad();
            push(virtualAddr + 0x500);
            mov_eax(MyGetModuleHandleA);
            call_eax();
            mov_xxx_eax(virtualAddr + 0x500);
            popad();
            retn();
            RunRemoteThread();
            while (gMrw.readInt32(virtualAddr + 0x500) == 1701667143) Thread.Sleep(0);

        }

        public void Asm_SetWindowsLong(Action<string> writeLogLine) {

            //clear();
            //pushad();
            //writeInt8(0x6A);
            //writeInt8(0xFC);
            //push((Int32)FindWindow("地下城与勇士", "地下城与勇士"));
            //mov_eax(GetWindowLongW);
            //call_eax();
            //mov_xxx_eax(virtualAddr + 0xD00);
            //popad();
            //retn();
            //RunRemoteThread();
            int pid = Win32.Kernel.GetCurrentProcessId();
            Form1.setPid(gMrw.pid);
            Int32 oldProc = GetWindowLong(FindWindow("地下城与勇士", "地下城与勇士"), -4);
            Form1.setPid(pid);

            Int32 procAddr = virtualAddr + 0x900;
            //Int32 oldProc = gMrw.readInt32(virtualAddr + 0xD00);
            int oldCall = gMrw.readInt32(oldProc + 0x269 + 2);

            writeLogLine("oldProc = " + Convert.ToString(oldProc, 16) + "    " + "procAddr = " + Convert.ToString(oldCall, 16));

            //if (oldProc == procAddr) {
            //    writeLogLine("已经绑定过主线程了");
            //    return;
            //}
            if (gMrw.readInt8(gMrw.readInt32(oldCall)) != 0x55)
            {
                writeLogLine("已经绑定过主线程了");
                return;
            }


            clear();
            //writeInt8(0x55);//push ebp
            //writeInt8(0x8B);
            //writeInt8(0xEC);//mov ebp,esp
            //writeInt8(0x53);//push ebx
            //writeInt8(0x56);//push esi
            //writeInt8(0x57);//push edi
            //mov_eax(virtualAddr + 0x9F0);
            //writeInt8(0x8B);
            //writeInt8(0x00);//mov eax,[eax]
            //writeInt8(0x83);
            //writeInt8(0xF8);
            //writeInt8(0x01);//cmp eax,01
            //writeInt8(0x75);
            //writeInt8(0x11);//jne 7FFD0023
            //pushad();//pushad
            //mov_eax(virtualAddr);
            //call_eax();
            //popad();//popad
            //mov_eax(virtualAddr + 0x9F0);
            //writeInt8(0xC6);
            //writeInt8(0x00);
            //writeInt8(0x00);//mov byte ptr [eax],00
            //writeInt8(0x8B);
            //writeInt8(0x45);
            //writeInt8(0x14);
            //writeInt8(0x8B);
            //writeInt8(0x4D);
            //writeInt8(0x10);
            //writeInt8(0x8B);
            //writeInt8(0x55);
            //writeInt8(0x0C);
            //writeInt8(0x50);
            //writeInt8(0x8B);
            //writeInt8(0x45);
            //writeInt8(0x08);
            //writeInt8(0x51);
            //writeInt8(0x52);
            //writeInt8(0x50);
            //push(oldProc);
            //mov_eax(CallProcAddress);
            //call_eax();
            //writeInt8(0x5F);
            //writeInt8(0x5E);
            //writeInt8(0x5B);
            //writeInt8(0x5D);
            //writeInt8(0xC2);
            //writeInt8(0x10);
            //writeInt8(0x00);

            //writeInt8(0x8B); writeInt8(0x44); writeInt8(0x24); writeInt8(0x08);//mov edx,[esp + C]
            writeInt8(0x83); writeInt8(0x7C); writeInt8(0x24); writeInt8(0x08); writeInt8(0x34);//cmp [esp + C],0x34
            writeInt8(0x75); writeInt8(0x05);//jne 05
            writeInt8(0xE8); writeInt8(0xF4); writeInt8(0xF6); writeInt8(0xFF); writeInt8(0xFF);//call header
            //writeInt8(0x8B); writeInt8(0xC4);//mov eax,esp
            //writeInt8(0x36); writeInt8(0xFF); writeInt8(0x70); writeInt8(0x10);//push [eax + 0x14]
            //writeInt8(0x36); writeInt8(0xFF); writeInt8(0x70); writeInt8(0xC);//push [eax + 0x14]
            //writeInt8(0x36); writeInt8(0xFF); writeInt8(0x70); writeInt8(0x08);//push [eax + 0x14]
            //writeInt8(0x36); writeInt8(0xFF); writeInt8(0x70); writeInt8(0x04);//push [eax + 0x14]
            //push(oldProc);
            //mov_eax(CallProcAddress);
            //call_eax();
            //writeInt8(0xC2); writeInt8(0x10);//retn 0x10
            push(gMrw.readInt32(oldCall));
            retn();
              Int32 m = 0;
            foreach (Byte a in Code)
                gMrw.writeInt8((procAddr + m++), a);
            setEvent();
            gMrw.writeInt32(oldCall, procAddr);
            //clear();
            //pushad();
            //push(procAddr);
            //writeInt8(0x6A);
            //writeInt8(0xFC);
            //push((Int32)FindWindow("地下城与勇士", "地下城与勇士"));
            //mov_eax(SetWindowLongW);
            //call_eax();
            //popad();
            //retn();
            //RunRemoteThread();
        }
        public void RunRemoteThread(bool IsMain = false) {
            RunRempteThreadWithMainThread();
            return;
            Int32 m = 0;
            Int32 hProcess = gMrw.GetHandle();
            foreach (Byte a in Code)
                gMrw.writeInt8((virtualAddr + 0x500 + m++), a);
            Int32 hThread = CreateRemoteThread(hProcess, 0, 0, CallProcAddress, virtualAddr + 0x500, 0, 0);

            if (hThread == 0) {
                int eid = (Int32)GetLastError();
                if (eid == 8L)
                    MessageBox.Show("客户端进程预定的内存空间不足，无法创建远程线程。\r\n这可能是因为你的计算机可用内存不足\r\n请重启游戏后再试");
                else if (eid == 193L)
                    MessageBox.Show("远程线程的代码区不是可执行代码");
                else if (eid == 5L)
                    MessageBox.Show("访问被拒绝");
                else
                    MessageBox.Show("CreateRemoteThread Failed，Error code：" + eid);
            }

            WaitForSingleObject(hThread, 0xFFFFFFFF);
            CloseHandle(hThread);
            //CloseHandle(hProcess);

            m = 0;
            foreach (Byte a in Code)
                gMrw.writeInt8((virtualAddr + 0x500 + m++), 0);
            setEvent();
        }
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

        public void RunRempteThreadWithMainThread() {
            //int hThread = (int)OpenThread(ThreadAccess.PROCESS_ALL_ACCESS, false, (uint)gMrw.tid);
            //Win32.Kernel.ResumeThread((IntPtr)hThread);
            //Win32.Kernel.SuspendThread((IntPtr)hThread);
            //RunRemoteThread();
            //Win32.Kernel.ResumeThread((IntPtr)hThread);
            //CloseHandle(hThread);
            //while (gMrw.readInt32(virtualAddr + 0x990) == virtualAddr)
            //    Thread.Sleep(0);





            Int32 m = 0;
            Byte[] code = new byte[Code.Length];

            foreach (Byte a in Code)
                code[m++] = a;

            gMrw.writedData((uint)(virtualAddr), code, (uint)code.Length);
            SendMessageW(HotKey.bindWindow, 0x34, 0, 0);
            m = 0;
            foreach (Byte a in Code)
                code[m++] = 0;
            gMrw.writedData((uint)(virtualAddr), code, (uint)code.Length);






            //gMrw.writeInt32(virtualAddr + 0x990, virtualAddr);
            //while (gMrw.readInt32(virtualAddr + 0x990) == virtualAddr)
            //{
            //    if (gMrw.readInt32(virtualAddr) == 0)
            //    {
            //        gMrw.writeInt32(virtualAddr + 0x990, virtualAddr + 0x9A0);
            //    }
            //    Thread.Sleep(0);
            //}
            //m = 0;
            //foreach (Byte a in Code)
            //    code[m++] = 0;
            //gMrw.writedData((uint)(virtualAddr), code, (uint)code.Length);

            //while (gMrw.readInt32(virtualAddr + 0x990) == virtualAddr)
            //    Thread.Sleep(0);
            //Int32 m = 0;
            //Byte[] code = new byte[Code.Length];

            //foreach (Byte a in Code)
            //    code[m++] = a;

            //gMrw.writedData((uint)(virtualAddr), code, (uint)code.Length);

            //while (gMrw.readInt32(virtualAddr + 0x9F0) == 1)
            //    Thread.Sleep(0);
            //Int32 m = 0;
            //Byte[] code = new byte[Code.Length];

            //foreach (Byte a in Code)
            //    code[m++] = a;
            //gMrw.writedData((uint)(virtualAddr), code, (uint)code.Length);
            //gMrw.writeInt32(virtualAddr + 0x9F0, 1);
            ////while (gMrw.readInt32(virtualAddr + 0x9F0) == virtualAddr)
            ////{
            ////    if (gMrw.readInt32(virtualAddr) == 0)
            ////    {
            ////        gMrw.writeInt32(virtualAddr + 0x990, virtualAddr + 0x9A0);
            ////    }
            ////    Thread.Sleep(0);
            ////}
            //while (gMrw.readInt32(virtualAddr + 0x9F0) == 1) Thread.Sleep(1);


            //gMrw.writeInt32(virtualAddr + 0x9F0, 0);
            setEvent();

        }
        public void clear() {
            Win32.Kernel.EnterCriticalSection(ref Config.CS);
            Code = "";
        }
        //EventWaitHandle handle = new EventWaitHandle(true, EventResetMode.ManualReset);
        public void setEvent()
        {
            Win32.Kernel.LeaveCriticalSection(ref Config.CS);
            //handle.Set();
        }

        public void checkRetnAddress()
        {
            writeInt8(0xFF);
            writeInt8(0x34);
            writeInt8(0x24);//push [esp]
            writeInt8(0xC7);
            writeInt8(0x04);
            writeInt8(0x24);
            writeInt32(0x004010BD); //004010BD    C3              retn








        }



        public void writeInt8(Byte value) {
            Code += (Char)value;
        }
        public void writeInt16(Int16 value) {
            Byte[] buffer = new Byte[2];
            IntPtr addr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            Marshal.WriteInt16(addr, value);
            Code += (Char)buffer[0];
            Code += (Char)buffer[1];
        }
        public void writeInt32(Int32 value) {
            Byte[] buffer = new Byte[4];
            IntPtr addr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            Marshal.WriteInt32(addr, value);
            Code += (Char)buffer[0];
            Code += (Char)buffer[1];
            Code += (Char)buffer[2];
            Code += (Char)buffer[3];
        }

        public void writeIntData(string value) {
            Code += value;
        }
        public void pushad() {
            writeInt8(0x60);
        }

        public void cmp_esp(Int32 value)
        {
            writeInt8(0x81);
            writeInt8(0x3C);
            writeInt8(0x24);
            writeInt32(value);
        }
        public void jnz_Int8(byte value)
        {
            writeInt8(0x75);
            writeInt8(value);
        }
        public void je_Int8(byte value)
        {
            writeInt8(0x74);
            writeInt8(value);

        }

        public void jne(Int32 value)
        {
            writeInt8(0x0F);
            writeInt8(0x85);

            writeInt32(value);

        }

        public void mov_100100_eax() {
            writeInt8(0xA3);
            writeInt32(virtualAddr + 0x9B0);
        }

        public void mov_virtualAddr_edi()
        {
            writeInt8(0x89);
            writeInt8(0x3D);
            writeInt32(virtualAddr + 0x9B0);
        }

        public void mov_virtualaddr_c3() {
            writeInt8(0xC7);
            writeInt8(0x05);
            writeInt32(virtualAddr + 0x990);
            writeInt32(virtualAddr + 0x9A0);

        }


        public void mov_xxx_eax(Int32 address) {
            writeInt8(0xA3);
            writeInt32(address);
        }

        public void popad() {
            writeInt8(0x61);
        }
        public void mov_eax(Int32 value) {
            writeInt8(0xB8);
            writeInt32(value);
        }

        public void call_addr(Int32 addr) {
            writeInt8(0xFF);
            writeInt8(0x15);

            writeInt32(addr);
        }

        public void mov_ebx(Int32 value) {
            writeInt8(0xBB);
            writeInt32(value);
        }
        public void mov_ecx(Int32 value) {
            writeInt8(0xB9);
            writeInt32(value);
        }
        public void push_ebp() {
            writeInt8(0x55);
        }

        public void push_ebp_addx(byte value)
        {
            writeInt8(0xFF);
            writeInt8(0x75);
            writeInt8(value);
        }

        public void mov_ebp_esp() {
            writeInt8(0x8B);
            writeInt8(0xEC);

        }

        public void mov_esp_ebp() {
            writeInt8(0x8B);
            writeInt8(0xE5);

        }

        public void sub_esp(byte value) {
            writeInt8(0x83);
            writeInt8(0xEC);
            writeInt8(value);
        }

        public void mov_edx(Int32 value) {
            writeInt8(0xBA);
            writeInt32(value);
        }
        public void mov_ebp(Int32 value) {
            writeInt8(0xBD);
            writeInt32(value);
        }
        public void mov_esp(Int32 value) {
            writeInt8(0xB8);
            writeInt32(value);
        }

        public void mov_esp_ptr_addx(byte addx,Int32 value)
        {
            writeInt8(0xC7);
            writeInt8(0x44);
            writeInt8(0x24);
            writeInt8(addx);
            writeInt32(value);
        }

        public void mov_edi(Int32 value) {
            writeInt8(0xBF);
            writeInt32(value);
        }
        public void mov_esi(Int32 value) {
            writeInt8(0xBE);
            writeInt32(value);
        }
        public void mov_eax_ptr_esi() {
            writeInt8(0x8B);
            writeInt8(0x06);
        }
        public void mov_eax_edx() {
            writeInt8(0x8B);
            writeInt8(0xC2);
        }
        public void call_eax() {
            writeInt8(0xFF);
            writeInt8(0xD0);
        }
        public void call_ebx() {
            writeInt8(0xFF);
            writeInt8(0xD3);
        }
        public void call_edx() {
            writeInt8(0xFF);
            writeInt8(0xD2);
        }
        public void call_edi() {
            writeInt8(0xFF);
            writeInt8(0xD7);
        }
        public void mov_ecx_ptr_ecx() {
            writeInt8(0x8B);
            writeInt8(0x09);
        }
        public void lea_esi_edi_addx(Int32 value) {
            writeInt8(0x8D);
            writeInt8(0xB7);
            writeInt32(value);
        }
        public void lea_eax_edi_addx(Int32 value) {
            writeInt8(0x8D);
            writeInt8(0x87);
            writeInt32(value);
        }
        public void lea_edx_ebp_4() {

            writeInt8(0x8D);
            writeInt8(0x55);
            writeInt8(0xFC);

        }
        public void mov_ptr_edx(Int32 value) {
            writeInt8(0xC7);
            writeInt8(0x02);
            writeInt32(value);
        }
        public void mov_edx_ptr_ecx() {
            writeInt8(0x8B);
            writeInt8(0x11);
        }

        public void push_eax() {
            writeInt8(0x50);
        }
        public void push_ebx() {
            writeInt8(0x53);
        }
        public void push_ecx() {
            writeInt8(0x51);
        }
        public void push_edx() {
            writeInt8(0x52);
        }

        public void push_esi() {
            writeInt8(0x56);
        }
        public void push_edi() {
            writeInt8(0x57);
        }
        public void retn() {
            writeInt8(0xC3);
        }

        public void retn(byte value)
        {
            writeInt8(0xC2);
            writeInt8(value);

        }


        public void add_esp(Byte value) {
            writeInt8(0x83);
            writeInt8(0xC4);
            writeInt8(value);
        }

        public void push(Int32 value) {

            writeInt8(0x68);
            writeInt32(value);

        }

        public void mov_edi_eax() {
            writeInt8(0x8B);
            writeInt8(0xF8);
        }

        public void mov_eax_ptr_eax() {
            writeInt8(0x8B);
            writeInt8(0x00);
        }

        public void cmp_eax(Int32 value)
        {
            writeInt8(0x3D);
            writeInt32(value);
        }

        public void mov_ecx_eax()
        {
            writeInt8(0x8B);
            writeInt8(0xC8);
        }

        public void mov_esi_eax()
        {
            writeInt8(0x8B);
            writeInt8(0xF0);
        }
    }

}
