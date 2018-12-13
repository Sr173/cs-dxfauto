using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrossProxy
{
    class at
    {
        [DllImport("kernel32.dll")]
        static public extern Int32 VirtualAllocEx(
       Int32 hprocess,
       Int32 lpaddress,
       Int32 dwsize,
       Int32 flallocationtype,
       Int32 flprotect
       );

        [DllImport("User32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("kernel32", EntryPoint = "CreateRemoteThread")]
        static public extern int CreateRemoteThread(
            int hprocess,
            int lpthreadattributes,
            int dwstacksize,
            int lpstartaddress,
            int lpparameter,
            int dwcreationflags,
            int lpthreadid
             );

        [DllImport("kernel32", EntryPoint = "VirtualProtectEx")]
        static public extern int VirtualProtectEx(
    int hprocess,
    int address,
    int size,
    int newprotect,
    int old
     );

        [DllImport("kernel32.dll")]
        static public extern Int32 WaitForSingleObject(
            Int32 hHandle,
            uint Second
            );

        [DllImport("kernel32.dll ")]
        static public extern bool CloseHandle(int hProcess);

        [DllImport("kernel32.dll ")]
        static public extern Int32 GetProcAddress(IntPtr hMouddle, string name);

        [DllImport("kernel32.dll")]
        static public extern uint GetLastError();

        [DllImport("kernel32.dll")]
        static public extern IntPtr GetModuleHandleA(string name);

        [DllImport("kernel32.dll")]
        static public extern int LoadLibary(string na);


        //私有变量
        static private string Code = "";
        static private Int32 mhProcess;
        static private Int32 virtualAddr = 0;
        static private IntPtr hModule = IntPtr.Zero;
        static private Int32 CallProcAddress = 0;
        static private Int32 GetWindowLongW = 0;
        static private Int32 SetWindowLongW = 0;

        //常量

        static public void Init()
        {
            // gMrw = new MemRWer((uint)hProcess);
            mhProcess = (int)gMrw.mhProcess;

            virtualAddr = GetProcAddress(GetModuleHandleA("ntdll.dll"), "RtlFreeMemoryBlockLookaside");
            Int32 eid = (Int32)GetLastError();
            if (virtualAddr == 0)
            {
                if (eid == 8L)
                    MessageBox.Show("由于客户端工作集内存不足，无法分配内存");
                else
                    MessageBox.Show("289行 错误代码 : " + eid);
            }

            virtualAddr += 0x9B;

            hModule = GetModuleHandleA("User32.dll");
            if (hModule == IntPtr.Zero)
                MessageBox.Show("303 行 句柄错误");
            CallProcAddress = GetProcAddress(hModule, "CallWindowProcW");
            GetWindowLongW = GetProcAddress(hModule, "GetWindowLongW");
            SetWindowLongW = GetProcAddress(hModule, "SetWindowLongW");
            if (CallProcAddress == 0)
                MessageBox.Show("306 行 句柄错误");
        }

        static public void MessageBoxText()
        {
            clear();
            pushad();
            push_Int8(0);
            push_Int8(0);
            push_Int8(0);
            push_Int8(0);
            mov_eax(GetProcAddress(hModule, "MessageBoxA"));
            call_eax();
            popad();
            retn();
            RunRempteThreadWithMainThread();

        }
        static public void Asm_SetWindowsLong(Action<string> writeLogLine)
        {

            clear();
            pushad();
            writeInt8(0x6A);
            writeInt8(0xFC);
            push_Int32((Int32)FindWindow("地下城与勇士", "地下城与勇士"));
            mov_eax(GetWindowLongW);
            call_eax();
            mov_xxx_eax(0x100100);
            popad();
            retn();
            RunRemoteThread();

            Int32 procAddr = virtualAddr + 0xC00;
            Int32 oldProc = gMrw.readInt32(0x100100);
            writeLogLine("oldProc = " + Convert.ToString(oldProc, 16) + "    " + "procAddr = " + Convert.ToString(procAddr, 16));

            if (oldProc == procAddr)
            {
                writeLogLine("已经绑定过主线程了");
                return;
            }

            clear();
            writeInt8(0x55);//push ebp
            writeInt8(0x8B);
            writeInt8(0xEC);//mov ebp,esp
            writeInt8(0x53);//push ebx
            writeInt8(0x56);//push esi
            writeInt8(0x57);//push edi
            mov_eax(virtualAddr + 0x1900);
            writeInt8(0x8B);
            writeInt8(0x00);//mov eax,[eax]
            writeInt8(0x83);
            writeInt8(0xF8);
            writeInt8(0x01);//cmp eax,01
            writeInt8(0x75);
            writeInt8(0x11);//jne 7FFD0023
            pushad();//pushad
            mov_eax(virtualAddr);
            call_eax();
            popad();//popad
            mov_eax(virtualAddr + 0x1900);
            writeInt8(0xC6);
            writeInt8(0x00);
            writeInt8(0x00);//mov byte ptr [eax],00
            writeInt8(0x8B);
            writeInt8(0x45);
            writeInt8(0x14);
            writeInt8(0x8B);
            writeInt8(0x4D);
            writeInt8(0x10);
            writeInt8(0x8B);
            writeInt8(0x55);
            writeInt8(0x0C);
            writeInt8(0x50);
            writeInt8(0x8B);
            writeInt8(0x45);
            writeInt8(0x08);
            writeInt8(0x51);
            writeInt8(0x52);
            writeInt8(0x50);
            push_Int32(oldProc);
            mov_eax(CallProcAddress);
            call_eax();
            writeInt8(0x5F);
            writeInt8(0x5E);
            writeInt8(0x5B);
            writeInt8(0x5D);
            writeInt8(0xC2);
            writeInt8(0x10);
            writeInt8(0x00);

            Int32 m = 0;
            foreach (Byte a in Code)
                gMrw.writeInt8((procAddr + m++), a);
            clear();

            pushad();
            push_Int32(procAddr);
            writeInt8(0x6A);
            writeInt8(0xFC);
            push_Int32((Int32)FindWindow("地下城与勇士", "地下城与勇士"));
            mov_eax(SetWindowLongW);
            call_eax();
            popad();
            retn();
            RunRemoteThread();
            writeLogLine("绑定主线程成功，线程基址 : 0x3A5D198");
        }
        static public void RunRemoteThread()
        {
            Int32 m = 0;
            foreach (Byte a in Code)
                gMrw.writeInt8((virtualAddr + m++), a);
            Int32 hThread = CreateRemoteThread(gMrw.GetHandle(), 0, 0, CallProcAddress, virtualAddr, 0, 0);

            if (hThread == 0)
            {
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

            m = 0;
            foreach (Byte a in Code)
                gMrw.writeInt8((virtualAddr + m++), 0);
        }
        static public void RunRempteThreadWithMainThread()
        {
            Int32 m = 0;
            foreach (Byte a in Code)
                gMrw.writeInt8((virtualAddr + m++), a);
            gMrw.writeInt32(virtualAddr + 0x1900, 1);
            while (gMrw.readInt32(virtualAddr + 0x1900) == 1)
                ;
            m = 0;
            foreach (Byte a in Code)
                gMrw.writeInt8((virtualAddr + m++), 0);
        }
        static public void clear()
        {
            Code = "";
        }
        static public void writeInt8(Byte value)
        {
            Code += (Char)value;
        }
        static public void writeInt16(Int16 value)
        {
            Byte[] buffer = new Byte[2];
            IntPtr addr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            Marshal.WriteInt16(addr, value);
            Code += (Char)buffer[0];
            Code += (Char)buffer[1];
        }
        static public void writeInt32(Int32 value)
        {
            Byte[] buffer = new Byte[4];
            IntPtr addr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            Marshal.WriteInt32(addr, value);
            Code += (Char)buffer[0];
            Code += (Char)buffer[1];
            Code += (Char)buffer[2];
            Code += (Char)buffer[3];
        }

        static public void writeIntData(string value)
        {
            Code += value;
        }
        static public void pushad()
        {
            writeInt8(0x60);
        }

        static public void mov_100100_eax()
        {
            writeInt8(0xA3);
            writeInt32(0x100100);
        }

        static public void mov_xxx_eax(Int32 address)
        {
            writeInt8(0xA3);
            writeInt32(address);
        }

        static public void popad()
        {
            writeInt8(0x61);
        }
        static public void mov_eax(Int32 value)
        {
            writeInt8(0xB8);
            writeInt32(value);
        }
        static public void mov_ebx(Int32 value)
        {
            writeInt8(0xBB);
            writeInt32(value);
        }
        static public void mov_ecx(Int32 value)
        {
            writeInt8(0xB9);
            writeInt32(value);
        }
        static public void mov_edx(Int32 value)
        {
            writeInt8(0xBA);
            writeInt32(value);
        }
        static public void mov_ebp(Int32 value)
        {
            writeInt8(0xBD);
            writeInt32(value);
        }
        static public void mov_esp(Int32 value)
        {
            writeInt8(0xB8);
            writeInt32(value);
        }
        static public void mov_edi(Int32 value)
        {
            writeInt8(0xBF);
            writeInt32(value);
        }
        static public void mov_esi(Int32 value)
        {
            writeInt8(0xBE);
            writeInt32(value);
        }
        static public void mov_eax_ptr_esi()
        {
            writeInt8(0x8B);
            writeInt8(0x06);
        }
        static public void mov_eax_edx()
        {
            writeInt8(0x8B);
            writeInt8(0xC2);
        }
        static public void call_eax()
        {
            writeInt8(0xFF);
            writeInt8(0xD0);
        }
        static public void call_ebx()
        {
            writeInt8(0xFF);
            writeInt8(0xD3);
        }
        static public void call_edx()
        {
            writeInt8(0xFF);
            writeInt8(0xD2);
        }
        static public void call_edi()
        {
            writeInt8(0xFF);
            writeInt8(0xD7);
        }
        static public void mov_ecx_ptr_ecx()
        {
            writeInt8(0x8B);
            writeInt8(0x09);
        }
        static public void lea_esi_edi_addx(Int32 value)
        {
            writeInt8(0x8D);
            writeInt8(0xB7);
            writeInt32(value);
        }
        static public void lea_eax_edi_addx(Int32 value)
        {
            writeInt8(0x8D);
            writeInt8(0x87);
            writeInt32(value);
        }
        static public void lea_edx_ebp_4()
        {

            writeInt8(0x8D);
            writeInt8(0x55);
            writeInt8(0xFC);

        }
        static public void mov_ptr_edx(Int32 value)
        {
            writeInt8(0xC7);
            writeInt8(0x02);
            writeInt32(value);
        }
        static public void mov_edx_ptr_ecx()
        {
            writeInt8(0x8B);
            writeInt8(0x11);
        }

        static public void push_eax()
        {
            writeInt8(0x50);
        }
        static public void push_ebx()
        {
            writeInt8(0x53);
        }
        static public void push_ecx()
        {
            writeInt8(0x51);
        }
        static public void push_edx()
        {
            writeInt8(0x52);
        }
        static public void push_ebp()
        {
            writeInt8(0x55);
        }
        static public void push_esi()
        {
            writeInt8(0x56);
        }
        static public void push_edi()
        {
            writeInt8(0x57);
        }
        static public void retn()
        {
            writeInt8(0xC3);
        }

        static public void add_esp(Byte value)
        {
            writeInt8(0x83);
            writeInt8(0xC4);
            writeInt8(value);
        }

        static public void push_Int8(Byte value)
        {
            writeInt8(0x6A);
            writeInt8(value);
        }

        static public void push_Int32(Int32 value)
        {
            writeInt8(0x68);
            writeInt32(value);
        }

        static public void mov_edi_eax()
        {
            writeInt8(0x8B);
            writeInt8(0xF8);
        }

        static public void mov_eax_ptr_eax()
        {
            writeInt8(0x8B);
            writeInt8(0x00);
        }
    }
}
