using CallTool;
using cs_dxf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win32;

namespace cs_dxfAuto
{
    public class MemRWer
    {
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(uint hProcess, uint lpBaseAddr, IntPtr lpBuffer, uint size, uint lpNumber);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(uint hProcess, uint lpBaseAddr, float[] lpBuffer, uint size, uint lpNumber);
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(uint hProcess, uint lpBaseAddr, IntPtr lpBuffer, uint size, uint lpNumber);
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(uint hProcess, uint lpBaseAddr, float[] lpBuffer, uint size, uint lpNumber);

        [DllImport("dll.dll")]
        public static extern bool readProcessMemory(int pid, long lpBaseAddr, int size, IntPtr lpBuffer ) ;
        [DllImport("dll.dll")]
        public static extern bool writeProcessMemory(int pid, long lpBaseAddr, int size , IntPtr lpBuffer);

        private uint myhProcess;
        public int pid;
        public int tid;

        public MemRWer(uint hProcess,int pid)
        {
            this.pid = pid;
            //int handle = Win32.Kernel.OpenProcess(0x1, false, pid);
            //Form1.promoteHandle(handle, 0x7FFFFFFF);
            //Form1.unlinkHandleTable();
            //myhProcess = (uint)handle;
            //myhProcess = hProcess;
        }
        public Int32 GetHandle()
        {

            //return Win32.Kernel.OpenProcess(0x1F0FFF, false, pid);
            return (Int32)myhProcess;
            //return (Int32)myhProcess;
        }
        public byte readInt8(long lpBaseAddr)
        {
            //uint mhProcess = (uint)GetHandle();
            //Byte result = 0;
            //Byte[] buffer = new Byte[1];
            //IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            //ReadProcessMemory(mhProcess, (uint)lpBaseAddr, vBytesAddress, 1, 0);
            //result = Marshal.ReadByte(vBytesAddress);
            ////MemRWer.closeHandle(mhProcess);
            return read<Byte>(lpBaseAddr);
        }
        public UInt16 readUInt16(long lpBaseAddr)
        {
            //uint mhProcess = (uint)GetHandle();
            //UInt16 result = 0;
            //Byte[] buffer = new Byte[2];
            //IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            //ReadProcessMemory(mhProcess, (uint)lpBaseAddr, vBytesAddress, 2, 0);
            //result = (ushort)((ushort)(Marshal.ReadByte(vBytesAddress)) + (ushort)(Marshal.ReadByte(vBytesAddress + 1) << 8));
            ////MemRWer.closeHandle(mhProcess);
            return read<UInt16>(lpBaseAddr);
        }

        public Int16 readInt16(long lpBaseAddr)
        {
            //uint mhProcess = (uint)GetHandle();
            //Int16 result = 0;
            //Byte[] buffer = new Byte[2];
            //IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            //ReadProcessMemory(mhProcess, (uint)lpBaseAddr, vBytesAddress, 2, 0);
            //result = Marshal.ReadInt16(vBytesAddress);
            return read<Int16>(lpBaseAddr);
        }
        public Int32 readInt32(long lpBaseAddr)
        {

            //uint mhProcess = (uint)GetHandle();
            //Int32 result = 0;
            //Byte[] buffer = new Byte[4];
            //IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            //ReadProcessMemory(mhProcess, (uint)lpBaseAddr, vBytesAddress, 4, 0);
            //result = Marshal.ReadInt32(vBytesAddress);
            ////MemRWer.closeHandle(mhProcess);
            return read<Int32>(lpBaseAddr);
        }
        public float readFloat(long lpBaseAddr)
        {
            //uint mhProcess = (uint)GetHandle();
            //float[] buffer = new float[] { 0 };
            //ReadProcessMemory(mhProcess, (uint)lpBaseAddr, buffer, 4, 0);
            ////MemRWer.closeHandle(mhProcess);
            return read<float>(lpBaseAddr);
        }
        public Int64 readInt64(long lpBaseAddr)
        {
            //uint mhProcess = (uint)GetHandle();
            //Int64 result = 0;
            //Byte[] buffer = new Byte[8];
            //IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            //ReadProcessMemory(mhProcess, (uint)lpBaseAddr, vBytesAddress, 8, 0);
            //result = Marshal.ReadInt64(vBytesAddress);
            ////MemRWer.closeHandle(mhProcess);
            return read<Int64>(lpBaseAddr);
        }
        public Int32 readInt32(long lpBaseAddr, params int[] ofe)
        {
            

            uint result = read<uint>(lpBaseAddr);

            foreach (Int32 offset in ofe)
            {
                result = read<uint>((int)(result + (uint)offset));
            }
            return (int)result;
        }

        public T read<T>(long lpBaseAddr)
        {
            if (lpBaseAddr < 0)
            {
                lpBaseAddr = ((uint)lpBaseAddr & 0xFFFFFFFF);
            };
            uint mhProcess = (uint)GetHandle();
            T[] t = new T[1];
            int size = Marshal.SizeOf(t[0]);
            IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(t, 0);
            //ReadProcessMemory(mhProcess, (uint)lpBaseAddr, vBytesAddress, (uint)size, 0);
            readProcessMemory(pid, lpBaseAddr, size, vBytesAddress);
            MemRWer.closeHandle(mhProcess);
            return t[0];
        }

        public byte[] readData(uint lpBaseAddr, uint lenth, byte[] temp = null)
        {
            if (lpBaseAddr < 0)
            {
                lpBaseAddr = ((uint)lpBaseAddr & 0xFFFFFFFF);
            };

            uint mhProcess = (uint)GetHandle();
            byte[] result;
            if (temp == null)
                result = new byte[lenth];
            else
                result = temp;
            IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(result, 0);
            //ReadProcessMemory(mhProcess, lpBaseAddr, vBytesAddress, lenth, 0);

            readProcessMemory(pid, lpBaseAddr, (int)lenth, vBytesAddress);

            MemRWer.closeHandle(mhProcess);
            return result;
        }

        public void write<T>(uint lpBaseAddr,T data)
        {
            if (lpBaseAddr < 0)
            {
                lpBaseAddr = ((uint)lpBaseAddr & 0xFFFFFFFF);
            };

            uint mhProcess = (uint)GetHandle();
            T[] t = new T[1];
            t[0] = data;

            int size = Marshal.SizeOf(t[0]);
            IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(t, 0);
           // WriteProcessMemory(mhProcess, (uint)lpBaseAddr, vBytesAddress, (uint)size, 0);
            writeProcessMemory(pid, lpBaseAddr, (int)size, vBytesAddress);
            //MessageBox.Show(Win32.Kernel.GetLastError().ToString());
            MemRWer.closeHandle(mhProcess);
        }



        public void writeInt8(int lpBaseAddr, Byte Data)
        {
            //uint mhProcess = (uint)GetHandle();

            //byte[] buffer = new Byte[1];
            //buffer[0] = Data;
            //IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            //WriteProcessMemory(mhProcess, (uint)lpBaseAddr, vBytesAddress, 1, 0);
            //MemRWer.closeHandle(mhProcess);

            write<byte>((uint)lpBaseAddr, Data);
        }
        public void writeInt64(int lpBaseAddr, Int64 Data)
        {
            //uint mhProcess = (uint)GetHandle();

            //Byte[] buffer = new Byte[8];
            //IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            //Marshal.WriteInt64(vBytesAddress, Data);
            //WriteProcessMemory(mhProcess, (uint)lpBaseAddr, vBytesAddress, 8, 0);
            //MemRWer.closeHandle(mhProcess);
            write<Int64>((uint)lpBaseAddr, Data);
        }
        public void writedData(uint lpBaseAddr, Byte[] Data, uint lenth)
        {
            if (lpBaseAddr < 0)
            {
                lpBaseAddr = ((uint)lpBaseAddr & 0xFFFFFFFF);
            };
            uint mhProcess = (uint)GetHandle();

            IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(Data, 0);
            //WriteProcessMemory(mhProcess, lpBaseAddr, vBytesAddress, lenth, 0);
            writeProcessMemory(pid, lpBaseAddr, (int)lenth, vBytesAddress);
        
            MemRWer.closeHandle(mhProcess);

        }
        public void writeInt32(int lpBaseAddr, Int32 Data,bool IsCheckProtect = false)
        {
            uint mhProcess = (uint)GetHandle();

            //uint mhProcess = (uint)GetHandle();

            //Byte[] buffer = new Byte[4];
            //IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            //Marshal.WriteInt32(vBytesAddress, Data);
            int oldProtect = 0;
            if (IsCheckProtect)
                Win32.Kernel.VirtualProtectEx((IntPtr)mhProcess, (IntPtr)lpBaseAddr, 4, 4, ref oldProtect);
            write<Int32>((uint)lpBaseAddr, Data);
            if (IsCheckProtect)
                Win32.Kernel.VirtualProtectEx((IntPtr)mhProcess, (IntPtr)lpBaseAddr, 4, oldProtect, ref oldProtect);
            //MemRWer.closeHandle(mhProcess);
            //MemRWer.closeHandle(mhProcess);

            MemRWer.closeHandle(mhProcess);

            //return result;

        }
        public void writeUInt32(int lpBaseAddr, UInt32 Data)
        {
            //uint mhProcess = (uint)GetHandle();

            //Byte[] buffer = new Byte[4];
            //IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            //Marshal.WriteIntPtr(vBytesAddress, (IntPtr)Data);
            //WriteProcessMemory(mhProcess, (uint)lpBaseAddr, vBytesAddress, 4, 0);
            //MemRWer.closeHandle(mhProcess);

            write((uint)lpBaseAddr, Data);


        }
        public void writeInt16(int lpBaseAddr, Int16 Data)
        {
            //uint mhProcess = (uint)GetHandle();

            //Byte[] buffer = new Byte[2];
            //IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            //Marshal.WriteInt16(vBytesAddress, Data);
            //WriteProcessMemory(mhProcess, (uint)lpBaseAddr, vBytesAddress, 2, 0);
            //MemRWer.closeHandle(mhProcess);
            write((uint)lpBaseAddr, Data);
        }
        public void writeFloat(int lpBaseAddr, float value)
        {
            //uint mhProcess = (uint)GetHandle();

            //WriteProcessMemory(mhProcess, (uint)lpBaseAddr, new float[] { value }, 4, 0);
            //MemRWer.closeHandle(mhProcess);
            write((uint)lpBaseAddr, value);

        }
        public string readString(int LPWCHAR)
        {
            string result = "";
            for (int i = 0; i < 200; i++)
            {
                Char t = (Char)readInt16(LPWCHAR + i * 2);
                if (t == 0)
                    break;
                result += t;
            }
            return result;
        }
        public void writeString(int addr, string s)
        {
            //uint mhProcess = (uint)GetHandle();

            for (int i = 0; i < s.Length; i++)
            {
                writeInt16(addr + i * 2, (short)s[i]);
            }
            writeInt16(addr + s.Length * 2, 0);
            //MemRWer.closeHandle(mhProcess);

        }
        public Int32 Decryption(Int32 address)
        {
            Int32 a = readInt32(address);
            if (a == -1)
                return -1;
            Int32 b = readInt32(readInt32(baseAddr.dwBase_Decryption) + (a >> 16) * 4 + 36);
            if (b == -1)
                return -1;
            a = readInt32(((a & 0xFFFF) * 4) + b + 8468);
            if (a == -1)
                return -1;
            a &= 0xFFFF;

            return ((a << 16) | a) ^ readInt32(address + 4);
        }
        public void Encryption1(Int32 addr, Int32 value)
        {
            Int32 ecx, eax, esi, edx, over;
            eax = readInt32(baseAddr.dwBase_Encryption);
            eax = eax + 1;
            writeInt32(baseAddr.dwBase_Encryption, eax);
            edx = (eax >> 8) & 255;
            ecx = readInt32(baseAddr.dwBase_Encryption_Param1 + edx * 2) & 65535;
            eax = eax & 255;
            eax = readInt32(baseAddr.dwBase_Encryption_Param2 + eax * 2) & 65535;
            eax = (ecx ^ eax) & 65535;
            over = addr & 15;
            if (over == 0)
            {
                ecx = value >> 16;
                ecx = ecx - eax;
                ecx += value & 65535;
            }
            else if (over == 4)
            {
                ecx = value & 65535;
                ecx = ecx - value >> 16;
            }
            else if (over == 8)
            {
                ecx = value >> 16;
                ecx = ecx * value & 65535;
            }
            else if (over == 12)
            {
                ecx = value >> 16;
                ecx = ecx + value & 65535;
                ecx = ecx + eax;
            }
            else
            {
                ecx = value >> 16;
                ecx = ecx + value & 65535;
            }

            esi = (ecx ^ eax) & 65535;
            ecx = eax;

            eax = (eax << 16) | ecx;
            eax = eax ^ value;
            writeInt32(addr + 4, eax);
            eax = readInt32(addr);
            edx = esi << 16;
            esi = readInt32(baseAddr.dwBase_Decryption);
            edx = edx | ecx;
            ecx = eax >> 16;
            ecx = readInt32(esi + ecx * 4 + 36);
            eax = eax & 65535;
            writeInt32(ecx + eax * 4 + 8468, edx);
        }
        public void writeDnfType(Int32 lpBaseAddress, Int32 addr, Int32 types)
        {
            Byte[] a = new Byte[12];
            Byte[] temp;
            temp = BitConverter.GetBytes(addr);
            for (int i = 0; i < 4; i++)
            {
                a[i] = temp[i];
            }
            temp = BitConverter.GetBytes(addr + types);

            for (int i = 4; i < 12; i++)
            {
                a[i] = temp[i % 4];
            }
            IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(a, 0);
            WriteProcessMemory((uint)lpBaseAddress, (uint)addr, vBytesAddress, 12, 0);
        }

        public static void closeHandle(uint h)
        {
            
        }

        public void Encryption(Int32 addr, Int32 value)
        {


            Int32 ID = readInt32(addr);

            Int32 para = readInt32(readInt32(baseAddr.dwBase_Decryption) + (ID >> 16) * 4 + 36);
            Int32 Paddress = para + (65535 & ID) * 4 + 8468;
            para = readInt32(Paddress);
            Int32 si = 0;

            Int32 data = (para & 65535);
            data = data + (data << 16);
            Int16 ax = (Int16)(para & 65535);

            Int32 addr_over = (addr & 15);
            if (addr_over == 0)
            {
                si = (data >> 16);
                si = si - ax;
                si += value;
            }

            if (addr_over == 4)
            {
                si = ((value & 65535) - (value >> 16));
            }

            if (addr_over == 8)
            {
                si = (value >> 16);
                si = si * value;
            }

            if (addr_over == 12)
            {
                si = (value >> 16);
                si = si + value;
                si = si + ax;
            }

            ax = (Int16)(si ^ ax);
            writeInt16(Paddress + 2, ax);
            writeInt32(addr + 4, (data ^ value));
        }

        public Int32 SearchSignature(Byte[] code, string szMask, int dwStartAddress, int ID)
        {
            return 0;
            uint mhProcess = (uint)GetHandle();

            int dwResult = 0;
            int lpAddress = dwStartAddress;
            int dwBestResult = 0;
            MEMORY_BASIC_INFORMATION lpBuffer = new MEMORY_BASIC_INFORMATION();
            Byte[] temp = Form1.temp;

            while (Win32.Kernel.VirtualQueryEx(mhProcess, (uint)lpAddress, ref lpBuffer, System.Runtime.InteropServices.Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != 0)
            {

                if (lpBuffer.Protect == 0x20 || lpBuffer.Protect == 0x80)
                {
                    readData((uint)lpBuffer.BaseAddress, (uint)lpBuffer.RegionSize, temp);
                    IntPtr vBytesAddress = Marshal.UnsafeAddrOfPinnedArrayElement(temp, 0);

                    for (int i = 0; i < lpBuffer.RegionSize - szMask.Length; i++)
                    {
                        IntPtr t = vBytesAddress + i;
                        bool flag = true;
                        for (int j = 0; j < szMask.Length; j++)
                        {
                            if (szMask[j] == 'x' && Marshal.ReadByte(t + j) != code[j])
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag == true)
                        {
                            //MemRWer.closeHandle(mhProcess);
                            return lpAddress + i;
                        }
                    }

                    if (dwResult > dwBestResult)
                    {
                        dwBestResult = dwResult;
                        break;
                    }
                    lpAddress = lpAddress + lpBuffer.RegionSize;
                }
            }
            //MemRWer.closeHandle(mhProcess);
            return dwBestResult;
        }

    }

}
