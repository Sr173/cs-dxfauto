using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using Win32;
using cs_dxf;
using cs_dxfAuto;
using System.Collections;

namespace CallTool {
    public class Function {

        private AssemblyTools at;
        private MemRWer gMrw;
        private Action<string> writeLogLine;
        private byte[] equipData;
        private byte[] equipData1;


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern int SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out Rect lpRect);

        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public struct Pos
        {
            public int x;
            public int y;
            public int z;
        }

        public struct TypeInteger {
            public int pCount;
            public int[] pram;
            TypeInteger(Int32 a = 0) {
                pCount = 0;
                pram = new int[32];
            }
        };


        public struct TypeFloat {
            public int pCount;
            public TypeInteger[] pram;
            TypeFloat(Int32 a = 0) {
                pCount = 0;
                pram = new TypeInteger[32];
            }
        };
        public struct DnfType {
            public int iCount;
            public TypeInteger[] integer;
            public int fCount;
            public TypeFloat[] _float;
            DnfType(Int32 a = 0) {
                integer = new TypeInteger[32];
                _float = new TypeFloat[32];
                iCount = 0;
                fCount = 0;
            }
        };


        public void SSS() {
            //at.clear();
            //at.checkRetnAddress();
            //at.pushad();
            //at.mov_ecx(gMrw.readInt32(baseAddr.dwBase_Character));
            //at.mov_edx(gMrw.readInt32(baseAddr.dwBase_Character, 0, 80));
            //at.call_edx();
            //at.push_eax();
            //at.push(18);
            //at.mov_ecx(gMrw.readInt32(baseAddr.dwBase_SSS));
            //at.push(5201314);
            //at.mov_eax(baseAddr.dwCall_SSS);
            //at.call_eax();
            //at.popad();
            //at.retn();
            //at.RunRemoteThread();

            // gMrw.Encryption(gMrw.readInt32(0x0416CB0C) + 0x117, 5201314);
            gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_SSS) + 0xC0C, 1234567);
        }



        public void OpenMjShop() {
            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.mov_ecx(gMrw.readInt32(baseAddr.dwBase_Shop));
            at.push(0);
            at.push(0);
            at.push(4);
            at.mov_eax(baseAddr.dwCall_UI);
            at.call_eax();
            at.popad();
            at.retn();
            at.RunRemoteThread();

            // gMrw.Encryption(gMrw.readInt32(0x0416CB0C) + 0x117, 5201314);
        }

        public Function(AssemblyTools ParamAt, MemRWer ParamGmrw, Int32 a) {
            at = ParamAt;
            gMrw = ParamGmrw;
        }
        public void WareCall(Int32 addr) {
            at.clear();
            at.pushad();
            at.mov_eax(1);
            at.mov_ecx(1);
            at.mov_edx(-1);
            at.mov_edi(gMrw.readInt32(baseAddr.dwBase_Character));
            at.mov_esi(addr);
            at.push_eax();
            at.push_ecx();
            at.push_edx();
            at.push_esi();
            at.mov_ecx(gMrw.readInt32(baseAddr.dwBase_Character));
            at.mov_eax(0x01F3FD00);//01963D40    55              push ebp
            at.call_eax();
            at.popad();
            at.retn();

            at.RunRemoteThread();
        }

        public Int32 GetItemPoint(Int32 pItem) {
            gMrw.writeInt32(0x100100, 0);
            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.mov_ecx(pItem);
            at.mov_eax(gMrw.readInt32(pItem, 0x150));
            at.call_eax();
            at.mov_100100_eax();
            at.popad();
            at.retn();
            at.RunRemoteThread();
            while (gMrw.readInt32(0x100100) == 0)
                Thread.Sleep(0);
            return (gMrw.readInt32(0x100100));
        }

        public Int32 GetPointer(Int32 code)
        {
            gMrw.writeInt32(at.GetVirtualAddr() + 0x9B0, 0);
            at.clear();
            at.pushad();
            at.push(0);
            at.push(0);
            at.push(0);
            at.push(0);
            at.push(0);
            at.push(0);
            at.push(0);
            at.push(0);
            at.push(0);
            at.push(0);
            at.push(0);
            at.push(0);
            at.push(0);
            at.push(0);
            at.push(0);
            at.push(0xFF);
            at.push(0);
            at.push(0);
            at.push(0);
            at.push(0xFF);
            at.push(0);
            at.push(0);
            at.push(1);
            at.mov_ecx(at.GetVirtualAddr() + 0xE00);
            at.mov_eax(0x2185E90);
            at.call_eax();
            at.push(0);
            at.push(0);
            at.push_eax();
            at.push(code);
            at.mov_ebx(0x2769560);
            at.mov_100100_eax();
            at.add_esp(0x10);
            at.popad();
            at.retn();
            at.RunRemoteThread();
            while (gMrw.readInt32(at.GetVirtualAddr() + 0x9B0) == 0)
                Thread.Sleep(0);
            return (gMrw.readInt32(at.GetVirtualAddr() + 0x9B0));
        }



       public Int32 CreateEquit(Int32 code) {
            gMrw.writeInt32(at.GetVirtualAddr() + 0x9B0, 0);
            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.push(1);
            at.push(0);
            at.push(code);
            at.mov_edx(baseAddr.dwCall_CreateEqui);//01F3E5F0    55              push ebp
            at.call_edx();
            at.mov_100100_eax();
            at.add_esp(0xC);
            at.popad();
            at.retn();
            at.RunRempteThreadWithMainThread();
            return (gMrw.readInt32(at.GetVirtualAddr() + 0x9B0));
        }

        public void createPet(int addr, int code)
        {
            at.clear();
            at.pushad();
            at.push(0);
            at.push(0);
            at.push(code);
            at.mov_ecx(addr);
            at.mov_eax(0x0276A360);//0276A360    55              push ebp
            at.call_eax();
            at.popad();
            at.retn();
            at.RunRempteThreadWithMainThread();
        }

        public void petSkillCall(int addr,int param = 6)
        {
            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.mov_ecx(addr);
            at.push(1);
            at.push(1);
            at.push(0);
            at.push(param);
            at.mov_eax(gMrw.readInt32(addr, 0x410));
            at.call_eax();
            at.popad();
            at.retn();
            at.RunRempteThreadWithMainThread();
        }

        public Function(AssemblyTools at, MemRWer gMrw, Action<string> writeLogLine) {
            this.at = at;
            this.gMrw = gMrw;
            this.writeLogLine = writeLogLine;
        }


        private void Send44DataStart(Int32 dataID, Int32 type) {
            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_Shop - 8);
            Int32 addr = gMrw.readInt32(baseAddr.dwBase_Shop - 8, 0, 0x8);

            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.mov_ecx(pSend);
            at.push(type);
            at.push(dataID);
            at.mov_eax(addr);
            at.call_eax();
        }

        private void Send44DataAddInt8(Int32 data) {
            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_Shop - 8);
            Int32 addr = gMrw.readInt32(baseAddr.dwBase_Shop - 8, 0, 0x24);

            at.mov_ecx(pSend);
            at.push(data);
            at.mov_eax(addr);
            at.call_eax();
        }

        private void Send44DataAddInt16(Int32 data) {
            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_Shop - 8);
            Int32 addr = gMrw.readInt32(baseAddr.dwBase_Shop - 8, 0, 0x28);

            at.mov_ecx(pSend);
            at.push(data);
            at.mov_eax(addr);
            at.call_eax();
        }

        private void Send44DataAddFloat(Int32 data)
        {
            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_Shop - 8);
            Int32 addr = gMrw.readInt32(baseAddr.dwBase_Shop - 8, 0, 0x2C);

            at.mov_ecx(pSend);
            at.push(data);
            at.mov_eax(addr);
            at.call_eax();
        }

        private void Send44DataAddInt32(Int32 data) {
            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_Shop - 8);
            Int32 addr = gMrw.readInt32(baseAddr.dwBase_Shop - 8, 0, 0x30);

            at.mov_ecx(pSend);
            at.push(data);
            at.mov_eax(addr);
            at.call_eax();
        }
        private void Send44DataAddInt64(Int32 ebp_4,Int32 ebp_8)
        {
            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_Shop - 8);
            Int32 addr = gMrw.readInt32(baseAddr.dwBase_Shop - 8, 0, 0x34);

            at.mov_ecx(pSend);
            at.push(ebp_8);
            at.push(ebp_4);
            at.mov_eax(addr);
            at.call_eax();
        }

        private void Send44DataEnd(int p1, int p2) {
            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_Shop - 8);
            Int32 addr = gMrw.readInt32(baseAddr.dwBase_Shop - 8, 0, 0x48);

            at.mov_ecx(pSend);
            at.push(3);
            at.push(p1);
            at.push(-1);
            at.push(p2);
            at.mov_eax(addr);
            at.call_eax();
            at.mov_virtualaddr_c3();
            at.popad();
            at.retn();

            at.RunRempteThreadWithMainThread();
        }

        public Int32 LoadCall(Int32 addr,string s)
        {
            gMrw.writeInt32(at.GetVirtualAddr() + 0x9B0, 0);
            gMrw.writeString(at.GetVirtualAddr() + 0xE00, s);

            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.mov_ecx(gMrw.readInt32(addr));
            at.push(1);
            at.push(at.GetVirtualAddr() + 0xE00);
            at.mov_eax(baseAddr.dwCall_LoadCall);
            at.call_eax();
            //at.mov_virtualaddr_c3();
            at.mov_100100_eax();
            at.popad();
            at.retn();
            at.RunRempteThreadWithMainThread();
            gMrw.writedData((uint)at.GetVirtualAddr() + 0xE00, new byte[s.Length * 2], (uint)s.Length * 2);
            return gMrw.readInt32(at.GetVirtualAddr() + 0x9B0);

        }

        public Int32 GetAtk(Int32 addr, int index) { 

            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.mov_ecx(addr);
            at.mov_esi(addr);
            at.push(index);
            at.mov_eax(gMrw.readInt32(baseAddr.dwBase_Character, 0, 0x834));
            at.call_eax();
            //at.mov_virtualaddr_c3();
            at.mov_100100_eax();
            at.popad();
            at.retn();
            at.RunRempteThreadWithMainThread();
            return gMrw.readInt32(at.GetVirtualAddr() + 0x9B0);

        }

        public void checkAtk()
        {
            for (int i = 0; i < 3; i++)
            {
                int atk = GetAtk(gMrw.readInt32(baseAddr.dwBase_Character), i);
                if (atk != 0)
                {
                    EncryptionCall(atk + 0x20, 1000000);
                    EncryptionCall(atk + 0x28, 0);
                }
            }
        }
        public void CommplteAllQuest() {
            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.mov_ecx(gMrw.readInt32(baseAddr.dwBase_Quest));
            at.push(-1);
            at.push(1);
            at.mov_eax(baseAddr.dwCall_CompleteAllQuest);//
            at.call_eax();
            at.mov_virtualaddr_c3();
            at.popad();
            at.retn();

            at.RunRempteThreadWithMainThread();

        }

        public void requestTeam(Int32 id) {
            SendGameDataStart(0xA);
            SendGameDataAddInt16(id);
            SendGameDataAddInt8(0);
            SendGameDataAddInt32(0);//复活币
            SendGameDataAddInt16(0xFFFF);//抗魔值
            SendGameDataAddInt32(0);
            SendGameDataEnd();
        }

        public void acceptTeam(Int32 id) {
            SendGameDataStart(0xB);
            SendGameDataAddInt16(id);
            SendGameDataAddInt8(0);
            SendGameDataAddInt32(0);
            SendGameDataEnd();
        }

        public void CloseGabriel() {
            SendGameDataStart(0x129);
            SendGameDataAddInt8(0);
            SendGameDataEnd();
        }

        public void OpenStore(int param)
        {
            SendGameDataStart(0x2CB);
            SendGameDataAddInt32(param);
            SendGameDataEnd();
        }

        public void test() {
            Send44DataStart(0x13, 2);
            Send44DataAddInt16(129);
            Send44DataAddInt16(gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Character) + 0xAC));
            Send44DataAddInt16(529);
            Send44DataAddInt16(51371);
            Send44DataAddInt32(-2);
            Send44DataAddInt8(0);
            Send44DataAddInt16(0);
            Send44DataAddInt8(0);
            Send44DataEnd(0, 1);
        }

        public Int32 GetEquipPoint(Int32 code) {
            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.mov_ecx(0x5403188);
            at.push(code);
            at.mov_eax(0x02ED2C20);//
            at.call_eax();
            at.push(1);
            at.push_eax();
            at.mov_eax(0x02D57790);//02D57790    55              push ebp
            at.mov_ecx(gMrw.readInt32(0x437C748));
            at.mov_esi(gMrw.readInt32(0x437C748));
            at.call_eax();
            at.mov_100100_eax();
            at.mov_virtualaddr_c3();
            at.popad();
            at.retn();

            at.RunRempteThreadWithMainThread();
            return gMrw.readInt32(0x100100);
        }

        public Int32 GetEquipPoint(string dir) {
            //"Monster/LordofLight/LOL/Armor.equ"
            gMrw.writeString(0x100100, dir);
            at.clear();
            at.pushad();
            at.mov_eax(0x100100);
            at.push(1);
            at.push_eax();
            at.mov_eax(0x02D57790);//02D57790    55              push ebp
            at.mov_ecx(gMrw.readInt32(0x437C748));
            at.mov_esi(gMrw.readInt32(0x437C748));
            at.call_eax();
            at.mov_100100_eax();
            at.mov_virtualaddr_c3();
            at.popad();
            at.retn();

            at.RunRempteThreadWithMainThread();
            return gMrw.readInt32(0x100100);
        }

        public void Killed44() {
            //Send44DataStart(0x19, 2);
            //Send44DataAddInt16(273);
            //Send44DataAddInt16(gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Character) + 0xAC));
            //Send44DataAddInt32(80026);
            //Send44DataAddInt16(26);
            //Send44DataAddInt8(1);
            //Send44DataAddInt32(64253);
            //Send44DataAddInt8(1698304);
            //Send44DataAddInt8(0);
            //Send44DataAddInt8(0);
            //Send44DataEnd(0, 1);



            Send44DataStart(0x15, 2);
            Send44DataAddInt8(0);
            Send44DataAddInt8(0xFF);
            Send44DataAddInt64(0, 0);
            Send44DataAddInt8(0);
            Send44DataAddInt16(273);
            Send44DataAddInt16(gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Character) + 0xAC));
            Send44DataAddInt16(65535);
            Send44DataAddInt16(0);
            Send44DataAddInt8(1);
            Send44DataAddInt8(1);
            Send44DataAddInt8(1);
            Send44DataAddInt8(0);
            Send44DataAddInt16(65535);
            Send44DataAddInt16(0);
            Send44DataAddInt32(3);
            Send44DataAddInt8(0);
            Send44DataAddInt16(1);
            Send44DataAddInt16(0);
            Send44DataAddInt32(230);

            Send44DataAddInt8(0);
            Send44DataAddInt16(1);
            Send44DataAddInt32(39002);
            Send44DataAddInt8(0);
            Send44DataAddInt32(230);

            Send44DataAddInt16(61140);
            Send44DataAddInt16(115);
            Send44DataAddInt8(0);
            Send44DataAddInt8(0);
            Send44DataAddInt8(1);
            Send44DataAddInt32(392);
            Send44DataAddInt32(223);
            Send44DataAddInt32(0);
            Send44DataAddInt32(11);

            Send44DataAddInt16(0);
            Send44DataAddInt8(0);
            Send44DataAddFloat(0);
            //Send44DataAddInt64(0, 0);
            Send44DataAddInt8(1);
            Send44DataAddInt8(0);
            Send44DataEnd(0, 1);
        }

        public void Discard(Int32 pos,Int32 num = 1)
        {
            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.push(1);
            at.push(num);
            at.push(pos);
            at.mov_ecx(gMrw.readInt32(baseAddr.dwBase_Bag));
            at.mov_eax(0x020939D0);
            at.call_eax();
            at.popad();
            at.mov_virtualaddr_c3();
            at.retn();
            at.RunRempteThreadWithMainThread();
        }

        public void CreateRole(Int32 addr, Int32 fID, Int32 code, Int32 zy = 0, Int32 level = 0) {
            if (level == 0)
                level = GetCharaLevel();

            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.mov_ecx(addr);
            at.push(zy);
            at.push(fID);
            at.push(level);
            at.push(code);
            at.mov_eax(0x02A7C700);//02A7C6C4    C3              retn
            at.call_eax();
            at.popad();
            at.mov_virtualaddr_c3();
            at.retn();

            at.RunRempteThreadWithMainThread();
        }

        public void CreateEmery(Int32 addr, Int32 fID, Int32 code,Int32 level = 0) {

            level = level == 0 ? GetCharaLevel() : level;

            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.mov_ecx(addr);
            at.push(fID);
            at.push(GetCharaLevel());
            at.push(code);
            at.mov_eax(baseAddr.dwCall_CreateEmery);//02393300    55              push ebp
            at.call_eax();
            at.popad();
            at.mov_virtualaddr_c3();
            at.retn();
            at.RunRempteThreadWithMainThread();
        }

        public void OpenHG(Int32 addr = 0) {
            if (addr == 0)
                addr = gMrw.readInt32(baseAddr.dwBase_Character);
            Send44DataStart(0x27, 3);
            Send44DataAddInt16(273);
            Send44DataAddInt16(gMrw.Decryption(addr + 0xAC));
            Send44DataAddInt8(12);
            Send44DataAddInt8(0xC);
            Send44DataAddInt16(65535);
            Send44DataAddInt16(65535);
            Send44DataAddInt8(3);
            Send44DataAddInt8(0);
            Send44DataAddInt8(0);
            Send44DataAddInt8(0);
            Send44DataAddInt32(gMrw.readInt32(addr + baseAddr.dwOffset_Equip_wq, 0x20));
            Send44DataAddInt8(0);
            Send44DataAddInt32(2000);
            Send44DataAddInt32(3);
            Send44DataAddInt32(0);
            Send44DataEnd(0, 0);
        }

        public void Openth(Int32 addr = 0) {
            if (addr == 0)
                addr = gMrw.readInt32(baseAddr.dwBase_Character);
            Send44DataStart(0x27, 3);
            Send44DataAddInt16(273);
            Send44DataAddInt16(gMrw.Decryption(addr + 0xAC));
            Send44DataAddInt8(12);
            Send44DataAddInt8(0xC);
            Send44DataAddInt16(65535);
            Send44DataAddInt16(65535);
            Send44DataAddInt8(0);
            Send44DataAddInt8(0);
            Send44DataAddInt8(0);
            Send44DataAddInt8(0);
            Send44DataAddInt32(gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq, 0x20));
            Send44DataAddInt8(0);
            Send44DataAddInt32(2000);
            Send44DataAddInt32(0);
            Send44DataAddInt16(39649);
            Send44DataEnd(0, 0);
        }

        public void F5(Int32 addr = 0) {
            if (addr == 0)
                addr = gMrw.readInt32(baseAddr.dwBase_Character);
            Send44DataStart(0x85, 1);
            Send44DataAddInt16(273);
            Send44DataAddInt16(gMrw.Decryption(addr + 0xAC));
            Send44DataAddInt8(12);
            Send44DataAddInt8(0xC);
            Send44DataAddInt16(65535);
            Send44DataAddInt16(65535);
            Send44DataAddInt8(0);
            Send44DataAddInt8(0);
            Send44DataAddInt8(0);
            Send44DataAddInt8(0);
            Send44DataAddInt32(gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq, 0x20));
            Send44DataAddInt8(0);
            Send44DataAddInt32(2000);
            Send44DataAddInt32(0);
            Send44DataAddInt16(39649);
            Send44DataEnd(0, 0);
        }

        public void OpenBX(Int32 addr = 0) {
            if (addr == 0)
                addr = gMrw.readInt32(baseAddr.dwBase_Character);
            Send44DataStart(0x14, 2);
            Send44DataAddInt16(61440);
            Send44DataAddInt16(29);
            Send44DataAddInt32(0);
            Send44DataAddInt8(1);
            Send44DataAddInt16(273);
            Send44DataAddInt16(gMrw.Decryption(addr + 0xAC));
            Send44DataAddInt16(61440);
            Send44DataAddInt16(27);
            Send44DataAddInt8(0);
            Send44DataAddInt8(0);
            Send44DataAddInt32(0);
            Send44DataAddInt8(0);
            Send44DataAddInt8(0);
            Send44DataAddInt8(0);
            Send44DataAddInt32(0);//41900000
            Send44DataAddInt16(0);
            Send44DataAddInt8(1);
            Send44DataAddInt8(9);
            Send44DataAddInt8(1);
            Send44DataAddInt32(0x41900000);//41900000
            Send44DataAddInt8(0);
            Send44DataAddInt8(0);
            Send44DataAddInt8(0);
            Send44DataEnd(0, 0);
        }

        private void SendGameDateAddString(Int32 data)
        {
            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_SEND);

            at.push(data);
            at.mov_ecx(pSend);
            //at.mov_eax(baseAddr.dwCall_Text_ADD);
            at.call_eax();
        }

        private void SendGameDataStart(Int32 dataID) {
            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_SEND);

            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.push(dataID);
            at.mov_ecx(pSend);
            at.mov_eax(baseAddr.dwCall_HANDLE);
            at.call_eax();
        }
        private void SendGameDataAddInt8(Int32 data) {

            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_SEND);

            at.push(data);
            at.mov_ecx(pSend);
            at.mov_eax(baseAddr.dwCall_ADD);
            at.call_eax();
        }
        private void SendGameDataAddInt16(Int32 data) {

            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_SEND);

            at.push(data);
            at.mov_ecx(pSend);
            at.mov_eax(baseAddr.dwCall_ADD + 0x30);
            at.call_eax();
        }
        private void SendGameDataAddInt32(Int32 data) {

            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_SEND);

            at.push(data);
            at.mov_ecx(pSend);
            at.mov_eax(baseAddr.dwCall_ADD + 0x60);
            at.call_eax();
        }

        private void SendGameDataAddInt64(Int32 a, Int32 b) {

            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_SEND);

            at.push(a);//0xC
            at.push(b);//0x8
            at.mov_ecx(pSend);
            at.mov_eax(baseAddr.dwCall_ADD + 0x90);
            at.call_eax();
        }
        private void SendGameDataEnd() {

            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_SEND);
            at.mov_ecx(pSend);
            at.mov_eax(baseAddr.dwCall_SEND);
            at.call_eax();
            at.mov_virtualaddr_c3();
            at.popad();
            at.retn();
            if (Config.IsBindMainThread)
                at.RunRempteThreadWithMainThread();
            else
                at.RunRemoteThread();
        }

        private void SendGameDataEndWithFlushBag()
        {

            Int32 pSend = gMrw.readInt32(baseAddr.dwBase_SEND);
            at.mov_ecx(pSend);
            at.mov_eax(baseAddr.dwCall_SEND);
            at.call_eax();

            at.push(0x400500);
            at.mov_ecx(gMrw.readInt32(baseAddr.dwBase_Bag, 0x68));
            at.mov_eax(0x65D690);
            at.call_eax();
            at.mov_virtualaddr_c3();
            at.popad();
            at.retn();

            at.RunRempteThreadWithMainThread();
        }

        public void texiao(int param)
        {
            gMrw.writeInt32(at.GetVirtualAddr() + 0xD00, gMrw.readInt32(baseAddr.dwBase_Decryption));
            gMrw.writeInt32(at.GetVirtualAddr() + 0xD04, gMrw.readInt32(baseAddr.dwBase_Decryption,  0x1032));

            at.clear();
            //at.checkRetnAddress();
            at.pushad();
            at.mov_ecx(at.GetVirtualAddr() + 0xD00);
            at.mov_eax(0x03CDB200);
            at.call_eax();//特效缓冲;
            at.push(1);
            at.mov_ecx(at.GetVirtualAddr() + 0xD04);
            at.mov_eax(0x03799C00);//007560B0    55              push ebp
            at.call_eax();//特效释放
            at.push(2);
            at.mov_ecx(at.GetVirtualAddr() + 0xD04);
            at.mov_eax(0x03799C00);//
            at.call_eax();//特效释放
            at.push(at.GetVirtualAddr() + 0xD04);
            at.push(param);
            at.mov_eax(0x01B1A0A0);//018AE080    55              push ebp
            at.call_eax();//特效call1
            at.mov_ecx_eax();
            at.mov_eax(0x01B16DF0);//01B16DF0    55              push ebp
            at.call_eax();//特效call2
            at.mov_esi_eax();
            at.push_eax();
            at.mov_eax(0x01B1A0A0);
            at.call_eax();//特效call1
            at.mov_ecx_eax();
            at.mov_eax(0x01B12C50);//01B12C50    55              push ebp
            at.call_eax();//特效call3
            at.mov_eax(0x01B1A0A0);
            at.call_eax();//特效call1
            at.popad();
            at.retn();

            at.RunRemoteThread();
        }

        public void attack(int addr,int obj)
        {
            int call_addr = gMrw.readInt32(addr, 0x43c);
            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.mov_esi(addr);
            at.mov_ecx(addr);
            at.push(0);
            at.push(0);
            at.push(1);
            at.push(obj);
            at.mov_edi(obj);
            at.mov_eax(call_addr);
            at.call_eax();
            at.popad();
            at.retn();
            at.RunRemoteThread();
        }
        
//------包结构开始-------
//包头id:0x1a0,返回地址:0x23322c1
//      Int8:2
//      Int16:71
//      Int8:0
//      Int16:9
//包长:6,包尾返回地址:0x233235d

            //附魔 1
            //炼金 3
            //控偶 4
            //分解 2
        public void SendSpacialResovle(int itemPos,int pos,int type)
        {
            SendGameDataStart(0x1a0);
            SendGameDataAddInt8(type);
            SendGameDataAddInt16(pos);
            SendGameDataAddInt8(0);
            SendGameDataAddInt16(itemPos);
            SendGameDataEnd();

        }

        public void GetSyMem(Int32 p) {

            at.clear();
            at.pushad();
            at.mov_ecx(p);
            at.mov_eax(0x01A78B10);//01A78B10    55              push ebp
            at.call_eax();
            at.popad();
            at.retn();

            at.RunRemoteThread();

        }

        public void MouseTp() {
            Int32 baseAddr1 = gMrw.readInt32(baseAddr.dwBase_Shop) + 0xAC38;
            Int32 baseAddr2 = 0x4C64AB8;

            Int32 blackMemory = 0x100150;

            at.clear();
            at.pushad();
            at.push(gMrw.readInt32(baseAddr2 + 0x1C));
            at.push(gMrw.readInt32(baseAddr2 + 0x18));
            at.push(gMrw.readInt32(baseAddr1 + 0xF8));
            at.push(blackMemory + 12);
            at.push(blackMemory + 8);
            at.push(blackMemory + 4);
            at.push(blackMemory);
            at.mov_esi(gMrw.readInt32(baseAddr1));
            at.mov_ecx(gMrw.readInt32(baseAddr1, 0x2C));
            at.mov_eax(0x1F9C000);
            at.call_eax();
            at.mov_virtualaddr_c3();
            at.popad();
            at.retn();
            at.RunRempteThreadWithMainThread();
            Thread.Sleep(500);
            CityTp(gMrw.readInt32(blackMemory), gMrw.readInt32(blackMemory + 4), gMrw.readInt32(blackMemory + 8), gMrw.readInt32(blackMemory + 12));
            writeLogLine(gMrw.readInt32(blackMemory) + "," + gMrw.readInt32(blackMemory + 4) + "," + gMrw.readInt32(blackMemory + 8) + "," + gMrw.readInt32(blackMemory + 12) + ",");
        }

        public void EnterCSK(Int32 a, Int32 b, Int32 c, Int32 d)
        {
            SendGameDataStart(36);
            SendGameDataAddInt8(a);
            SendGameDataAddInt8(b);
            SendGameDataAddInt16(c);
            SendGameDataAddInt16(d);
            SendGameDataAddInt8(5);
            SendGameDataAddInt16(98);
            SendGameDataAddInt16(27);
            SendGameDataAddInt32(0);
            SendGameDataAddInt8(0);
            SendGameDataEnd();

        }

        public void CityTp(Int32 a, Int32 b, Int32 c, Int32 d) {
            SendGameDataStart(36);
            SendGameDataAddInt8(a);
            SendGameDataAddInt8(b);
            SendGameDataAddInt16(c);
            SendGameDataAddInt16(d);
            SendGameDataAddInt8(5);
            SendGameDataAddInt16(9);
            SendGameDataAddInt16(2);
            SendGameDataAddInt32(0);
            SendGameDataAddInt8(0);
            SendGameDataEnd();
        }

        public void GiveUpSecondaryJob()
        {
            SendGameDataStart(0xEF);
            SendGameDataEnd();
        }

        public void ChooseInstance(Int32 code = 0) {
            SendGameDataStart(0xF);
            SendGameDataAddInt32(code);
            SendGameDataEnd();
        }
        public void EnterInstanceTrue() {
            SendGameDataStart(0x25);
            SendGameDataAddInt32(0);
            SendGameDataAddInt32(0);
            SendGameDataEnd();
        }

        public void EnterQuestInstance(Int32 id, Int32 level,Int32 Qid) {
            SendGameDataStart(0x10);
            SendGameDataAddInt32(id);
            SendGameDataAddInt8(level);
            SendGameDataAddInt16(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt16(65535);
            SendGameDataAddInt32(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt32(Qid);
            SendGameDataEnd();
        }

        public void EnterInstance(Int32 id, Int32 level, bool IsYj = false, bool IsYg = false) {
            //SendGameDataStart(0x10);
            //SendGameDataAddInt32(id);
            //SendGameDataAddInt8(level);
            //SendGameDataAddInt16(0);
            //SendGameDataAddInt8(0);
            //SendGameDataAddInt8(0);
            //SendGameDataAddInt16(65535);
            //SendGameDataAddInt32(0);
            //SendGameDataAddInt8(0);
            //SendGameDataAddInt8(0);
            //SendGameDataEnd();

            if (IsYj == false) {
                SendGameDataStart(0x10);
                SendGameDataAddInt32(id);
                SendGameDataAddInt8(level);
                SendGameDataAddInt16(0);
                SendGameDataAddInt8(0);
                SendGameDataAddInt8(0);
                SendGameDataAddInt16(65535);
                SendGameDataAddInt32(0);
                SendGameDataAddInt8(0);
                SendGameDataAddInt32(0);
                SendGameDataAddInt8(0);
                SendGameDataEnd();
            } else {
                SendGameDataStart(0x10);
                SendGameDataAddInt32(id);
                if (IsYg)
                    SendGameDataAddInt8(level);
                else
                    SendGameDataAddInt8(0);

                SendGameDataAddInt16(3);
                SendGameDataAddInt8(0);
                SendGameDataAddInt8(0);
                SendGameDataAddInt16(65535);
                SendGameDataAddInt32(0);
                SendGameDataAddInt8(0);
                SendGameDataAddInt32(12871);
                SendGameDataAddInt8(0);
                SendGameDataEnd();
            }
        }

        public void EnterSy(Int32 id, Int32 level) {

            SendGameDataStart(0x10);
            SendGameDataAddInt32(id);
            SendGameDataAddInt8(level);
            SendGameDataAddInt16(0);
            SendGameDataAddInt8(1);
            SendGameDataAddInt8(0);
            SendGameDataAddInt16(65535);
            SendGameDataAddInt32(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt32(0);
            SendGameDataAddInt8(0);
            SendGameDataEnd();
        }

        public void EnterInstance(Int32 id, Int32 level, Int32 LoopID) {
            SendGameDataStart(0x308);
            SendGameDataAddInt32(id);
            SendGameDataAddInt32(LoopID);
            SendGameDataEnd();
            SendGameDataStart(0x1F);
            SendGameDataAddInt16(31);
            SendGameDataAddInt16(LoopID);
            SendGameDataEnd();
            SendGameDataStart(0x1FB);
            SendGameDataAddInt8(1);
            SendGameDataAddInt32(LoopID);
            SendGameDataEnd();
            SendGameDataStart(0x10);
            SendGameDataAddInt32(id);
            SendGameDataAddInt8(level);
            SendGameDataAddInt16(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt16(65535);
            SendGameDataAddInt32(LoopID);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataEnd();
        }
        public void InstanceTp(Int32 x, Int32 y) {
            SendGameDataStart(0x2D);
            SendGameDataAddInt8(x);
            SendGameDataAddInt8(y);
            SendGameDataAddInt32(113);
            SendGameDataAddInt32(311);
            SendGameDataAddInt8(0);
            SendGameDataAddInt16(29610);
            SendGameDataAddInt16(30);
            for (Int32 i = 0; i < 7; i++)
                SendGameDataAddInt16(0);
            SendGameDataAddInt32(1);
            for (Int32 i = 0; i < 7; i++)
                SendGameDataAddInt32(0);
            for (Int32 i = 0; i < 7; i++)
                SendGameDataAddInt16(0);
            SendGameDataAddInt32(-1);
            SendGameDataAddInt16(0);
            SendGameDataAddInt16(34);
            SendGameDataAddInt16(8);
            SendGameDataAddInt64(0, 8854);
            SendGameDataAddInt32(572);
            SendGameDataAddInt16(65535);
            SendGameDataAddInt8(0);
            SendGameDataEnd();
        }
        public void QuitInstanceNoMainThread() {
            SendGameDataStart(0x2A);
            //for (Int32 i = 0; i < 7; i++)
            //    SendGameDataAddInt32(0);
            SendGameDataEnd();
        }

        public void QuitInstance() {
            SendGameDataStart(0x2A);
            //for (Int32 i = 0; i < 7; i++)
            //    SendGameDataAddInt32(0);
            SendGameDataEnd();
        }

        public void QuitTeamInstance() {
            SendGameDataStart(0x48);
            SendGameDataAddInt8(1);
            SendGameDataAddInt8(2);
            SendGameDataEnd();
        }

        public void QuitTeam()
        {
            SendGameDataStart(0xD);
            SendGameDataEnd();

        }

        public void ChallengeAgain() {
            SendGameDataStart(0x48);
            SendGameDataAddInt8(1);
            SendGameDataAddInt8(0);
            SendGameDataEnd();
        }

        private Int32 GetSkillDir(Int32 sid) {

            gMrw.writeInt32(0x100100, 99);
            at.clear();
            at.pushad();
            at.mov_ecx(0x05227370);//目录基址
            at.mov_ebx(sid);
            at.push_ebx();
            at.mov_eax(0x02D9DF70);//路径call
            at.call_eax();
            at.mov_100100_eax();
            at.popad();
            at.retn();
            at.RunRemoteThread();

            Int32 result = gMrw.readInt32(0x100100);
            while ((result = gMrw.readInt32(0x100100)) == 99)
                Thread.Sleep(0);
            return result;
        }
        private Int32 GetSkillAddress(Int32 addr) {


            gMrw.writeInt32(0x100100, 99);
            at.clear();
            at.pushad();
            at.mov_ecx(0x0419F9A0);
            at.mov_ecx_ptr_ecx();
            at.push(1);
            at.push(addr);
            at.mov_eax(0x02C29060);
            at.call_eax();
            at.mov_edi_eax();
            at.lea_esi_edi_addx(0xB8);
            at.lea_eax_edi_addx(0xFC);
            at.push_esi();
            at.push_eax();
            at.lea_edx_ebp_4();
            at.push_edx();
            at.mov_ptr_edx(0);
            at.mov_eax(0x02C9DC60);
            at.call_eax();
            at.add_esp(0xc);
            at.mov_eax_ptr_eax();
            at.mov_ecx(0x0419F9B4);
            at.mov_ecx_ptr_ecx();
            at.push(1);
            at.push_eax();
            at.mov_eax(0x02D9DF70);
            at.call_eax();
            at.mov_100100_eax();
            at.popad();
            at.retn();
            at.RunRemoteThread();

            Int32 result = gMrw.readInt32(0x100100);
            while ((result = gMrw.readInt32(0x100100)) == 99)
                Thread.Sleep(0);
            return result;
        }

        //        Int32 GetSkillAddr(Int32 addr) {
        //            VMP_BEGIN

        //            Int32 MAP_DATA = 副本信息;
        //            Int32 LOAD_CALL = 加载call;
        //            __asm
        //{
        //                mov ecx, MAP_DATA
        //mov ecx, [ecx]
        //push 1
        //push addr
        //call LOAD_CALL
        //}

        private Int32 GetSkillAddr(Int32 addr) {
            gMrw.writeInt32(0x100100, 99);
            at.clear();
            at.pushad();
            at.mov_ecx(0x3CE9B90);
            at.mov_ecx_ptr_ecx();
            at.push(1);
            at.push(addr);
            at.mov_eax(0x290B400);//路径call
            at.call_eax();
            at.mov_100100_eax();
            at.popad();
            at.retn();
            at.RunRemoteThread();

            Int32 result = gMrw.readInt32(0x100100);
            while ((result = gMrw.readInt32(0x100100)) == 99)
                Thread.Sleep(0);
            return result;
        }
        private Int32 GetInitSkillAddr(Int32 addr) {
            gMrw.writeInt32(0x100100, 99);
            at.clear();
            at.pushad();
            at.mov_edi(addr);
            at.lea_esi_edi_addx(0xb4);
            at.lea_eax_edi_addx(0xf4);
            at.push_esi();
            at.push_eax();
            at.lea_edx_ebp_4();
            at.push_edx();

            at.mov_eax(0x2977FB0);//路径call
            at.call_eax();
            at.add_esp(0xC);
            at.mov_eax_ptr_eax();
            at.mov_100100_eax();
            at.popad();
            at.retn();
            at.RunRemoteThread();

            Int32 result = gMrw.readInt32(0x100100);
            while ((result = gMrw.readInt32(0x100100)) == 99)
                Thread.Sleep(0);
            return result;
        }
        private Int32 GetSkillValueAddress(Int32 addr) {
            gMrw.writeInt32(0x100100, 99);
            at.clear();
            at.pushad();
            at.mov_ecx(0x3CE9BA4);
            at.mov_ecx_ptr_ecx();
            at.push(1);
            at.push(addr);
            at.mov_eax(0x290B400);//路径call
            at.call_eax();
            at.mov_100100_eax();
            at.popad();
            at.retn();
            at.RunRemoteThread();

            Int32 result = gMrw.readInt32(0x100100);
            while ((result = gMrw.readInt32(0x100100)) == 99)
                Thread.Sleep(0);
            return result;
        }

        public void atkHurt(Int32 code, Int32 hurt) {

            Int32 temp = GetSkillDir(code);
            writeLogLine(temp.ToString());
            if (temp != 0) {
                temp = GetSkillAddr(temp);
                writeLogLine(temp.ToString());

                if (temp != 0) {
                    temp = GetInitSkillAddr(temp);
                    writeLogLine(temp.ToString());

                    if (temp != 0) {
                        temp = GetSkillValueAddress(temp);
                        writeLogLine(temp.ToString());
                        if (temp != 0) {
                            gMrw.Encryption1(temp + 0x20, hurt);
                            gMrw.Encryption1(temp + 0x28, 0);
                        }
                    }
                }
            }
        }

        public void CheckCodeHurt(Int32 code, Int32 hurt) {
            Int32 obj_lujing = GetSkillDir(code);
            writeLogLine(obj_lujing.ToString());

            if (obj_lujing > 0) {
                Int32 atk = GetSkillAddress(obj_lujing);
                writeLogLine(atk.ToString());
                if (atk > 0) {
                    gMrw.Encryption(atk + 0x20, hurt);
                    gMrw.Encryption(atk + 0x28, 0);

                }
            }
        }

        uint MAKELONG(ushort x, ushort y)
        {
            return ((((uint)x) << 16) | y); //low order WORD 是指标的x位置； high order WORD是y位置.
        }

        public void MouseClick(IntPtr dnf, ushort x, ushort y, bool LR)
        {
            Rect r = new Rect();
            GetWindowRect(dnf,out r);
            SetCursorPos(r.Left + x, r.Top + y);

            IntPtr lParam = (IntPtr)((y << 16) | x); // The coordinates 
            if (LR)
            {
                SendMessage(dnf, 0x201, (IntPtr)0, lParam);
                Thread.Sleep(80);
                SendMessage(dnf, 0x202, (IntPtr)0, lParam);
                return;
            }
            SendMessage(dnf, 0x204, (IntPtr)2, lParam);
            Thread.Sleep(80);
            SendMessage(dnf, 0x205, (IntPtr)2, lParam);
        }

        public void ChooseChara() {
            SendGameDataStart(0x7);
            SendGameDataEnd();
        }
        public void EnterChara(Int32 pos) {
            gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Role) + 0x15C, pos);

            SendGameDataStart(0x4);
            SendGameDataAddInt16(pos);
            SendGameDataEnd();
        }
        public void _PickUp(Int32 id) {
            Random r = new Random();
            SendGameDataStart(0x2B);
            SendGameDataAddInt32(id);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(1);
            SendGameDataAddInt16(getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).x + r.Next(1, 10));
            SendGameDataAddInt16(getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).y + r.Next(1, 10));
            SendGameDataAddInt16(0);
            SendGameDataAddInt16(getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).x + r.Next(1, 10));
            SendGameDataAddInt16(getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).y + r.Next(1, 10));
            SendGameDataAddInt16(0);
            SendGameDataAddInt16(0);
            SendGameDataEnd();
        }
        public void AcceptQuest(Int32 qid) {
            SendGameDataStart(31);
            SendGameDataAddInt16(31);
            SendGameDataAddInt16(qid);
            SendGameDataEnd();
        }
        public void CompletingQuest(Int32 qid) {
            SendGameDataStart(33);
            SendGameDataAddInt16(33);
            SendGameDataAddInt16(qid);
            SendGameDataAddInt8(-1);
            SendGameDataAddInt8(-1);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataEnd();
        }

        public void CompletingQuest1(Int32 qid) {
            SendGameDataStart(33);
            SendGameDataAddInt16(33);
            SendGameDataAddInt16(qid);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataEnd();
        }

        public void CompleteTrue() {
            SendGameDataStart(0x2E);
            SendGameDataAddInt8(0);
            SendGameDataAddInt16(100);
            SendGameDataAddInt8(1);
            SendGameDataAddInt16(50151);
            SendGameDataAddInt32(12);
            SendGameDataAddInt8(90);
            SendGameDataAddInt8(90);

            SendGameDataAddInt32(5);
            SendGameDataAddInt32(0);
            SendGameDataAddInt32(-12);
            SendGameDataAddInt32(808);
            SendGameDataAddInt32(1016);
            SendGameDataAddInt32(808);
            SendGameDataAddInt32(1016);
            SendGameDataAddInt8(777);
            SendGameDataAddInt32(0);
            SendGameDataAddInt32(0);

            SendGameDataEnd();
        }

        public void CommittingQuest(Int32 qid) {
            SendGameDataStart(34);
            SendGameDataAddInt16(34);
            SendGameDataAddInt16(qid);
            SendGameDataAddInt16(65535);
            SendGameDataAddInt16(1);
            SendGameDataAddInt16(65535);
            SendGameDataEnd();
        }
        public void GetTzItem(Int32 Code) {
            SendGameDataStart(0x2BC);
            SendGameDataAddInt32(Code);
            SendGameDataEnd();
        }
        public void SendUseConsumables(Int32 iPos,Int32 code) {

            SendGameDataStart(0x2C);
            SendGameDataAddInt16(iPos);
            SendGameDataAddInt8(0);
            SendGameDataAddInt32(0);
            SendGameDataAddInt32(code);
            SendGameDataAddInt32(0);
            SendGameDataEnd();
        }
        public void PickUp() {
            Int32 map = gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 End = gMrw.readInt32(map + 0xC4);
            Int32[] temp = new Int32[100];
            Int32 Count = 0;


            for (Int32 i = gMrw.readInt32(map + 0xC0); i < End; i += 4) {
                Int32 b = gMrw.readInt32(i);

                if (gMrw.readInt32(b + 0xA4) == 0x121) {
                    temp[Count++] = gMrw.Decryption(b + 0xAC);
                }
            }
            for (Int32 i = Count - 1; i >= 0; i--) {
                _PickUp(temp[i]);
            }
        }

        public bool waitForItemOnFloor()
        {
            Int32 map = gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 End = gMrw.readInt32(map + 0xC4);

            while (true)
            {
                bool flag = true;
                for (Int32 i = gMrw.readInt32(map + 0xC0); i < End; i += 4)
                {
                    Int32 b = gMrw.readInt32(i);

                    if (gMrw.readInt32(b + 0xA4) == 0x121)
                    {
                        float z = getObjPos(b).z;
                        if (z != 0)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag) return true;
                Thread.Sleep(500);

            }
        }

        public void checkItemSx()
        {
            string cname = gMrw.readString(gMrw.readInt32(baseAddr.dwBase_Character, 0x400));

            Int32 map = gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 End = gMrw.readInt32(map + 0xC4);

            for (Int32 i = gMrw.readInt32(map + 0xC0); i < End; i += 4)
            {
                Int32 b = gMrw.readInt32(i);
                if (gMrw.readInt32(b + 0xA4) == 0x121)
                {
                    int point = gMrw.readInt32(b + 0x16C0);

                    string name = gMrw.readString(gMrw.readInt32(point + 0x24));
                    int item_pj = gMrw.readInt32(point + 0x178);

                    if (!name.Contains("碎片") && !name.Contains("怨念"))
                    {
                        if (item_pj >= 3 || name.Contains("卡片"))
                            KeyEvent.fm1.writeSpacilLine(DateTime.Now.ToLongTimeString().ToString() + " " + cname + ":获得 [" + name + "]");
                    }

                    gMrw.writeInt32(point + 32, 490003723);
                }
            }
        }

        public void pickUp1()
        {
            checkItemSx();
            waitForItemOnFloor();
            Thread.Sleep(500);
            GatherItem();
        }

        public void pickUp2()
        {

            checkItemSx();
            GatherItem1();

        }


        public void PickUpInBuildItem() {
            Int32 map = gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 End = gMrw.readInt32(map + 0xC4);
            Int32[] temp = new Int32[100];


            for (Int32 i = gMrw.readInt32(map + 0xC0); i < End; i += 4) {
                Int32 b = gMrw.readInt32(i);

                if (gMrw.readInt32(b + 0xA4) == 1057) {
                    Int32 End1 = gMrw.readInt32(b + 0x1734);
                    for (Int32 m = gMrw.readInt32(b + 0x1730); m < End1; m += 8) {
                        Int32 iAddr = gMrw.readInt32(m + 4);
                        Int32 destAddr = gMrw.readInt32(m);
                        _PickUp(destAddr);

                    }
                }
            }

        }

        public void SaleSearch(Int32 ItemID) {
            SendGameDataStart(0xBA);
            SendGameDataAddInt8(0);
            SendGameDataAddInt32(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(31);
            SendGameDataAddInt8(1);
            SendGameDataAddInt16(40119);
            SendGameDataAddInt32(ItemID);
            SendGameDataAddInt16(0);
            SendGameDataAddInt16(0);
            SendGameDataAddInt16(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(8);
            SendGameDataEnd();
        }

        public void SaleSearchCoin(Int32 ItemID) {
            SendGameDataStart(0xBA);
            SendGameDataAddInt8(1);
            SendGameDataAddInt32(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(31);
            SendGameDataAddInt8(1);
            SendGameDataAddInt16(40100);
            SendGameDataAddInt32(ItemID);
            SendGameDataEnd();
        }


        public void Buy(Int32 code, Int32 param1, Int32 param2) {
            SendGameDataStart(0x15);
            SendGameDataAddInt32(code);
            SendGameDataAddInt32(1);
            SendGameDataAddInt32(param1);
            SendGameDataAddInt32(param2);
            SendGameDataEnd();

        }

        public void BuyCall(Int32 line, Int32 count) {
            Int32 nEcx = gMrw.readInt32(baseAddr.dwBase_Shop, 0x88, 0x130);//55 8B EC 83 EC 10 56 8B F1 8B 86 48 05 00 00 8B 8C C6 8C 03 00 00 8B 89 38 04 00 00


            gMrw.writeInt32(nEcx + 0x578, line);

            at.clear();
            at.pushad();
            at.mov_edx(count);
            at.mov_ecx(nEcx);
            at.push_edx();
            at.mov_eax(baseAddr.dwCall_Buy); // 2.27日
            at.call_eax();
            at.mov_virtualaddr_c3();
            at.popad();
            at.retn();

            at.RunRempteThreadWithMainThread();
        }

        public void BuyCall(Int32 line) {
            Int32 nEcx = gMrw.readInt32(baseAddr.dwBase_Shop, 0x8C, 0x120);
            gMrw.writeInt32(nEcx + 0x4F4, line);

            at.clear();
            at.pushad();
            at.mov_ecx(nEcx);
            at.mov_eax(0x1A39810);//019E9E30    55              push ebp
            at.call_eax();
            at.popad();
            at.mov_virtualaddr_c3();
            at.retn();

            at.RunRempteThreadWithMainThread();
        }

        public void SendItemEnterStore(int pos, int Spos, int id, int Count) {
            SendGameDataStart(0x13);
            SendGameDataAddInt8(0);
            SendGameDataAddInt16(pos);
            SendGameDataAddInt32(id);
            SendGameDataAddInt32(Count);
            SendGameDataAddInt8(0xC);
            SendGameDataAddInt16(Spos);
            SendGameDataAddInt32(id);
            SendGameDataAddInt32(0);
            SendGameDataAddInt32(-1);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataEnd();
        }


        public void _Sell(Int32 index, Int32 Count = 1) {
            SendGameDataStart(0x16);
            SendGameDataAddInt8(0);
            SendGameDataAddInt16(index);
            SendGameDataAddInt32(Count);
            SendGameDataAddInt32(index + Count);
            SendGameDataAddInt32(0);
            SendGameDataEnd();
        }
        public void HideCall(Int32 obj = 0) {
            if (obj == 0)
                obj = gMrw.readInt32(baseAddr.dwBase_Character);
            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.mov_ecx(obj);
            at.mov_esi(obj);
            at.push(-1);
            at.push(1);
            at.push(1);
            at.push(1);
            at.mov_edx(0x037286F0);//037286F0    55              push ebp
            at.call_edx();
            at.popad();
            at.retn();

            at.RunRempteThreadWithMainThread();

        }

        public void GetExpBox() {
            SendGameDataStart(0x25B);
            SendGameDataAddInt32(0);
            SendGameDataEnd();
        }


        public Int32 IsHaveItemForName(string Name) {
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);

            Int32 first = gMrw.readInt32(bag + 88);
            Int32 iteam = first + 36;

            for (Int32 i = 0; i < 56 * 5; i++) {
                Int32 point = gMrw.readInt32(iteam + i * 4);
                if (point != 0) {
                    Int32 iteam_l = gMrw.readInt32(point + 0x160);
                    string name = gMrw.readString(gMrw.readInt32(point + 0x24));
                    if (name.IndexOf(Name) >= 0)
                        return (i + 9);
                }
            }

            return 0;
        }



        public Int32 GetItemNum(string Name) {
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);

            Int32 first = gMrw.readInt32(bag + 88);
            Int32 iteam = first + 36;

            for (Int32 i = 0; i < 56 * 5; i++) {
                Int32 point = gMrw.readInt32(iteam + i * 4);
                if (point != 0) {
                    Int32 iteam_l = gMrw.readInt32(point + 0x160);
                    string name = gMrw.readString(gMrw.readInt32(point + 0x24));
                    if (name.Contains(Name) == true)
                        return (gMrw.readInt32(iteam_l + 0x2E4));
                }
            }

            return 0;
        }

        public void SkillWithFj() {
            Int32 ePoint = CreateEquit(100050202);

            Int32 head = gMrw.readInt32(ePoint + 0x10E4);
            Int32 cPoint = gMrw.readInt32(baseAddr.dwBase_Character);

            head = gMrw.readInt32(head + 0x1C, 0x80, 0x4E8);

            gMrw.writeFloat(gMrw.readInt32(head + 0x18, 0x4, 0x2C) + 0xC, (float)9999999);//概率
            gMrw.writeFloat(gMrw.readInt32(head + 0x18, 0x4, 0x18) + 0x4, (float)100);//概率

            for (Int32 i = 0x2CD0; i <= 0x2CE0; i += 4) {
                if (gMrw.readInt32(cPoint + i) == 0) {
                    MessageBox.Show("请穿戴三件首饰");
                    return;
                }
                gMrw.Encryption(gMrw.readInt32(cPoint + i) + 0x10F8, 11197);
                gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0x1108, 1);
            }
            //CreateSkill(70102, 1500);
            ePoint = CreateEquit(100310528);

            head = gMrw.readInt32(ePoint + 0x10E4);
            cPoint = gMrw.readInt32(baseAddr.dwBase_Character);

            head = gMrw.readInt32(head + 0x1C, 0x50, 0x4C0);

            gMrw.writeInt32(head, 1);
            gMrw.writeInt32(head + 4, 1);
            gMrw.writeInt32(head + 8, 1500);
            gMrw.writeInt32(head + 0xC, 0);
            for (Int32 i = 0x2CE4; i <= 0x2CEC; i += 4) {
                if (gMrw.readInt32(cPoint + i) == 0) {
                    MessageBox.Show("请穿戴三件首饰");
                    return;
                }

                gMrw.Encryption(gMrw.readInt32(cPoint + i) + 0x10F8, 11126);
                gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0x1108, 1);
            }
        }
        public void Sell() {
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);
            Int32 first = gMrw.readInt32(bag + 88);
            Int32 item = first + 36;
            OpenStore(317);
            for (Int32 i = 0; i < 56; i++) {
                Int32 point = gMrw.readInt32(item + i * 4);
                string name = gMrw.readString(gMrw.readInt32(point + 0x24));

                if (!Config.IsSellPink) {
                    if (name.Contains("密制钛胸甲") == true)
                        continue;
                    if (name.Contains("密制锯齿合金链甲") == true)
                        continue;
                }

                if (point != 0) {
                    Int32 item_1 = gMrw.readInt32(point + 0x178);
                    if (item_1 == 0 || item_1 == 1 || item_1 == 2 || (Config.IsSellPink && item_1 == 3)) {
                        //_Sell(i + 9);
                        MyKey.MMouseClick(0);
                        sellPrompt(point);
                    }

                }
            }
        }

        public void SkillCallFollowSelf(int addr,int code,int hurt)
        {
            at.clear();
            at.pushad();
            at.push(0);
            at.push(0x49);
            at.push(0x1);
            at.push(0x41);
            at.push(0x0);
            at.push(code);
            at.mov_ecx(addr);
            at.mov_esi(addr);
            at.mov_eax(0x02183AE0);
            at.call_eax();
            at.popad();
            at.retn();
            at.RunRemoteThread();

        }

        public void SkillCall(int addr,int code,int hurt)
        {
            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);
            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);

            if (addr == 0)
            {
                for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4)
                {
                    Int32 onobj = gMrw.readInt32(i);
                    Int32 grope = gMrw.readInt32(onobj + 0x828);
                    if (grope == 200)
                    {
                        addr = onobj;
                        break;
                    }
                }
            }

     
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4)
            {
                Int32 onobj = gMrw.readInt32(i);


                Int32 type = gMrw.readInt32(onobj + 0xA4);
                Int32 grope = gMrw.readInt32(onobj + 0x828);

                if (grope == 0)
                    continue;
                if (onobj == gMrw.readInt32(baseAddr.dwBase_Character))
                    continue;
                if (gMrw.readInt32(onobj + 0x3AE4) == 0)
                    continue;

                if (type == 0x211 || type == 273)
                {
                    int x, y;
                    
                    x = getObjPos(onobj).x;
                    y = getObjPos(onobj).y;
                    SkillCallFollowSelf(addr, code, hurt);
                    return;
                }
            }



        }

        public void SkillCall(int addr,int code ,int hurt,int x,int y,int z)
        {
            at.clear();
            at.pushad();
            at.push(0);
            at.push(z);
            at.push(y);
            at.push(x);
            at.push(hurt);
            at.push(code);
            at.push(addr);
            at.mov_ecx(addr);
            at.mov_eax(0x20EC1F0);
            at.call_eax();
            at.add_esp(0x1C);
            at.popad();
            at.retn();

            at.RunRemoteThread();

        }

        public void Discard()
        {
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);
            Int32 first = gMrw.readInt32(bag + 88);
            Int32 item = first + 36;
            for (Int32 i = 0; i < 56; i++)
            {
                Int32 point = gMrw.readInt32(item + i * 4);
                string name = gMrw.readString(gMrw.readInt32(point + 0x24));

                if (point != 0)
                {
                    Int32 item_1 = gMrw.readInt32(point + 0x178);
                    if (item_1 == 0 || item_1 == 1 || item_1 == 2 || item_1 == 3)
                    {
                        Discard(i + 9);
                        Thread.Sleep(100);
                    }
                }
            }
        }

        public void DealNoUse() {
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);
            Int32 first = gMrw.readInt32(bag + 88);
            Int32 item = first + 36;
            for (Int32 i = 0; i < 56 * 5; i++) {
                Int32 point = gMrw.readInt32(item + i * 4);
                string name = gMrw.readString(gMrw.readInt32(point + 0x24));
                bool flag = false;

                if (point != 0) {
                    Int32 item_1 = gMrw.readInt32(point + 0x178);

                    if (name.Contains("设计图"))
                        flag = true;
                    if (name.Contains("HP"))
                        flag = true;
                    if (name.Contains("MP"))
                        flag = true;
                    if (name.Contains("魔法封印"))
                        flag = true;
                    if (name.Contains("生命"))
                        flag = true;
                    if (name.Contains("印章"))
                        flag = true;
                    if (name.Contains("陨石"))
                        flag = true;
                    if (name.Contains("转移晶石"))
                        flag = true;
                    if (name.Contains("金刚石"))
                        flag = true;
                    if (name.Contains("海蓝宝石"))
                        flag = true;
                    if (name.Contains("破旧的皮革"))
                        flag = true;
                    if (name.Contains("风化的碎骨"))
                        flag = true;
                    if (name.Contains("最下级硬化剂"))
                        flag = true;
                    if (name.Contains("生锈的铁片"))
                        flag = true;
                    if (name.Contains("魔力溶解剂"))
                        flag = true;
                    if (name.Contains("达人HP"))
                        flag = true;
                    if (name.Contains("达人MP"))
                        flag = true;

                    if (item_1 > 2)
                        flag = false;

                    if (flag == true) {
                        int count = gMrw.Decryption(point + 0x2A0);
                        _Sell(i + 9, count);
                        writeLogLine("出售：[" + name + "]" + count + "个");
                        Thread.Sleep(100);
                    }
                }
            }
        }

        public void SendGetStoreItem(Int32 code, Int32 num) {
            SendGameDataStart(0x13);
            SendGameDataAddInt8(12);
            SendGameDataAddInt16(0);
            SendGameDataAddInt32(code);
            SendGameDataAddInt32(num);
            SendGameDataAddInt8(0);
            SendGameDataAddInt16(147);
            SendGameDataAddInt32(0);
            SendGameDataAddInt32(0);
            SendGameDataAddInt32(-1);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataEnd();
        }

        private void getCS(int pos, int type, int code) {
            SendGameDataStart(160);
            SendGameDataAddInt16(pos);
            SendGameDataAddInt8(type);
            SendGameDataAddInt8(0);
            SendGameDataAddInt32(code);
            SendGameDataAddInt8(0);
            SendGameDataEnd();
        }

        public void OpenCS() {
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);
            Int32 first = gMrw.readInt32(bag + 88);
            Int32 item = first + 36;

            for (Int32 i = 0; i < 56 * 2; i++) {
                Int32 point = gMrw.readInt32(item + i * 4);

                if (point != 0) {
                    Int32 e_code = 0;

                    Int32 code = gMrw.readInt32(point + 0x20);
                    if (code == 10155176)
                        e_code = 100090350;
                    if (code == 10155177)
                        e_code = 100090312;
                    if (code == 10155178)
                        e_code = 100090351;
                    if (code == 10155179)
                        e_code = 100090352;
                    if (code == 10155180)
                        e_code = 100090313;
                    if (code == 10155181)
                        e_code = 100090353;
                    if (code == 10155182)
                        e_code = 100090354;
                    if (code == 10159545)
                        e_code = 10159545;
                    if (code == 10159550)
                        e_code = 10159550;

                    if (e_code != 0) {
                        int twice = gMrw.Decryption(point + 0x2A0);
                        if (e_code == 10159545)
                        {
                            for (int j = 0; j < twice; j++)
                            {
                                SendOpenPackage(i + 9);
                                Thread.Sleep(100);
                            }
                        }
                        else if (e_code == 10159550)
                        {
                            for (int j = 0; j < twice; j++)
                            {
                                SendOpenJar(i + 9);
                                Thread.Sleep(100);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < twice; j++)
                            {
                                getCS(i + 9, 4, e_code);
                                Thread.Sleep(100);
                            }
                        }
                    }
                }
            }


        }

        public BagItem GetItem(string _name)
        {
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);
            Int32 first = gMrw.readInt32(bag + 88);
            Int32 item = first + 36;
            BagItem I = new BagItem();
            I.Count = 0;
            I.Point = 0;
            I.Pos = 0;
            I.name = "";

            for (int i = 3; i < 9; i++)
            {
                Int32 point = gMrw.readInt32(baseAddr.dwBase_Character, 0x649C, 0x58, i * 4);
                if (point != 0)
                {
                    string name = gMrw.readString(gMrw.readInt32(point + 0x24));
                    if (name.Contains(_name))
                    {

                        I.Count = gMrw.Decryption(point + 0x2A0);
                        I.Point = point;
                        I.Pos = i;
                        I.name = name;
                        return I;
                    }
                }
            }

            for (Int32 i = 0; i < 56 * 5; i++)
            {
                Int32 point = gMrw.readInt32(item + i * 4);

                if (point != 0)
                {
                    string name = gMrw.readString(gMrw.readInt32(point + 0x24));
                    if (name.Contains(_name))
                    {

                        I.Count = gMrw.Decryption(point + 0x2A0);
                        I.Point = point;
                        I.Pos = i + 9;
                        I.name = name;
                        return I;
                    }

                }
            }
            return I;
        }

        public BagItem GetItem(Int32 Code) {
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);
            Int32 first = gMrw.readInt32(bag + 88);
            Int32 item = first + 36;
            BagItem I = new BagItem();
            I.Count = 0;
            I.Point = 0;
            I.Pos = 0;

            for (int i = 3; i < 9; i++) {
                Int32 point = gMrw.readInt32(baseAddr.dwBase_Character, 0x649C, 0x58, i * 4);
                if (point != 0) {
                    Int32 code = gMrw.readInt32(point + 0x20);

                    if (code == Code) {

                        I.Count = gMrw.Decryption(point + 0x2A0);
                        I.Point = point;
                        I.Pos = i;
                        return I;
                    }
                }
            }

            for (Int32 i = 0; i < 56 * 5; i++) {
                Int32 point = gMrw.readInt32(item + i * 4);

                if (point != 0) {
                    Int32 code = gMrw.readInt32(point + 0x20);

                    if (code == Code) {

                        I.Count = gMrw.Decryption(point + 0x2A0);
                        I.Point = point;
                        I.Pos = i + 9;
                        return I;
                    }

                }
            }
            return I;
        }

        public void OpenMr() {
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);
            Int32 first = gMrw.readInt32(bag + 88);
            Int32 item = first + 36;

            for (Int32 i = 0; i < 56 * 2; i++) {
                Int32 point = gMrw.readInt32(item + i * 4);

                if (point != 0) {
                    Int32 e_code = 0;

                    Int32 code = gMrw.readInt32(point + 0x20);
                    if (code == 10099412)
                        e_code = 10099412;
                    if (code == 10101863)
                        e_code = 100090312;
                    if (code == 10099398)
                        e_code = 100090351;
                    if (code == 10101862)
                        e_code = 100090352;
                    if (code == 10099411)
                        e_code = 100090313;


                    if (e_code != 0) {
                        int twice = gMrw.Decryption(point + 0x2A0);


                        for (int j = 0; j < twice; j++) {
                            SendOpenPackage(i + 9);
                            Thread.Sleep(50);
                        }

                    }
                }
            }
        }
        public int SendEnterStore(int ItemCode, int pos) { //经验胶囊 10147584  //3332
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);
            Int32 first = gMrw.readInt32(bag + 88);
            Int32 item = first + 36;

            for (int i = 0; i < 56 * 3; i++) {
                int point = gMrw.readInt32(item + i * 4);
                if (point == 0)
                    continue;
                int Count = gMrw.Decryption(point + 0x2A0);
                int Code = gMrw.readInt32(point + 0x20);

                if (Code == ItemCode) {
                    SendItemEnterStore(i + 9, pos, Code, Count);
                    sum += Count;
                }
            }
            for (int i = 0xC; i < 0x20; i++) {
                int point = gMrw.readInt32(baseAddr.dwBase_Character, 0x649C, 0x58, i);
                int Count = gMrw.Decryption(point + 0x2A0);
                int Code = gMrw.readInt32(point + 0x20);

                if (Code == ItemCode) {
                    SendItemEnterStore(i / 4, pos, Code, Count);
                    sum += Count;
                }
            }
            return sum;
        }

        public void OpenJx() {
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);
            Int32 first = gMrw.readInt32(bag + 88);
            Int32 item = first + 36;

            for (Int32 i = 0; i < 56 * 2; i++) {
                Int32 point = gMrw.readInt32(item + i * 4);

                if (point != 0) {

                    Int32 code = gMrw.readInt32(point + 0x20);

                    if (code == 10100301) {
                        int twice = gMrw.Decryption(point + 0x2A0);
                        for (int j = 0; j < twice; j++) {
                            SendOpenPackage(i + 9);
                            Thread.Sleep(100);
                        }

                    }
                }
            }

            for (int i = 0xC; i < 0x20; i++) {
                int point = gMrw.readInt32(baseAddr.dwBase_Character, 0x649C, 0x58, i);
                if (point != 0) {

                    Int32 code = gMrw.readInt32(point + 0x20);

                    if (code == 10100301) {
                        int twice = gMrw.Decryption(point + 0x2A0);
                        for (int j = 0; j < twice; j++) {
                            SendOpenPackage(i / 4);
                            Thread.Sleep(100);
                        }

                    }
                }
            }
        }

        public void spacialResolve()
        {
            BagItem bi = GetItem("提取器");
            if (bi.Pos == 0)
            {
                writeLogLine("找不到提取器");
                return;
            }

            int type = 2;
            if (bi.name.Contains("灵魂"))
                type = 4;
            else if (bi.name.Contains("魔力"))
                type = 1;
            else if (bi.name.Contains("炼金"))
                type = 2;

            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);
            Int32 first = gMrw.readInt32(bag + 88);
            Int32 item = first + 36;
            for (Int32 i = 0; i < 56; i++)
            {
                Int32 point = gMrw.readInt32(item + i * 4);
                string name = gMrw.readString(gMrw.readInt32(point + 0x24));
                if (name.Contains("密制钛胸甲") == true)
                    continue;

                if (point != 0)
                {
                    Int32 item_1 = gMrw.readInt32(point + 0x178);
                    if (item_1 == 0 || item_1 == 1 || item_1 == 2)
                    {
                        SendSpacialResovle(i + 9, bi.Pos,type);
                        writeLogLine("炼金：[" + name + "]");
                    }
                }
            }
        }

        public void Resolve() {
            //ArrangeBag(1);
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);
            Int32 first = gMrw.readInt32(bag + 88);
            Int32 item = first + 36;
            for (Int32 i = 0; i < 56; i++) {
                Int32 point = gMrw.readInt32(item + i * 4);
                string name = gMrw.readString(gMrw.readInt32(point + 0x24));
                if (name.Contains("密制钛胸甲") == true)
                    continue;

                if (point != 0) {
                    Int32 item_1 = gMrw.readInt32(point + 0x178);
                    if (item_1 == 0 || item_1 == 1 || item_1 == 2) {
                        SendResolve(i + 9);
                        writeLogLine("分解：[" + name + "]");
                    }
                }
            }
            //ArrangeBag(1);
        }

        public System.Drawing.Point GetCurrencyRoom() {
            System.Drawing.Point p = new System.Drawing.Point();
            p.X = gMrw.readInt32(baseAddr.dwBase_Shop - 8, baseAddr.dwBase_Time, 0xC8, 0xBCC);
            p.Y = gMrw.readInt32(baseAddr.dwBase_Shop - 8, baseAddr.dwBase_Time, 0xC8, 0xBD0);
            return p;
        }

        public void SendOpenCoin() {
            SendGameDataStart(215);
            SendGameDataAddInt32(1);
            SendGameDataAddInt32(1);
            SendGameDataAddInt16(3);
            SendGameDataEnd();
        }

        public void BuyCoin(Int32 num, Int32 code) {
            SendGameDataStart(0xB9);
            SendGameDataAddInt8(1);
            SendGameDataAddInt32(num);
            SendGameDataAddInt32(code);
            SendGameDataEnd();
        }

        public void GoleEnterBag(Int32 Num) {
            SendGameDataStart(0x133);
            SendGameDataAddInt32(Num);
            SendGameDataEnd();
        }

        public Int32 GetCharaNum() {
            return gMrw.readInt32(0x03C51FB0, 4, 16); //03C51FB0 - C0
        }

        public Int32 GetCharaGole() {
            return gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Bag, 0x58, 0) + 0x2A0);
        }
        void WriteDnfType(DnfType[] pta, int blackmem, int tCount, int addr) {

            int typeEnd = addr + tCount * 0x50;

            for (int i = 0; i < tCount; i++) {
                int dataAddr = typeEnd + pta[i].iCount * 0x14; // 把数据放在头部后面
                for (int j = 0; j < pta[i].iCount; j++) {

                    for (int k = 0; k < pta[i].integer[j].pCount; k++) {
                        gMrw.writeInt32(dataAddr, pta[i].integer[j].pram[k]);
                        dataAddr += 4;
                    }
                    gMrw.writeInt32(blackmem, typeEnd);
                    gMrw.writeInt32(blackmem + 4, 0);
                    gMrw.writeInt32(typeEnd, blackmem);
                    blackmem += 8;

                    gMrw.writeInt32(typeEnd + 0x04, dataAddr - pta[i].integer[j].pCount * 4); // 数据地址
                    gMrw.writeInt32(typeEnd + 0x08, dataAddr); // 数据结束
                    gMrw.writeInt32(typeEnd + 0x0C, dataAddr);
                    gMrw.writeInt32(typeEnd + 0x10, 0);

                    typeEnd += 0x14;
                }
                // 整数段头部
                gMrw.writeInt32(blackmem, addr + i * 0x50);
                gMrw.writeInt32(blackmem + 4, 0);
                gMrw.writeInt32(addr + i * 0x50, blackmem);
                blackmem += 8;

                gMrw.writeInt32(addr + i * 0x50 + 0x04, typeEnd - pta[i].iCount * 0x14);
                gMrw.writeInt32(addr + i * 0x50 + 0x08, typeEnd);
                gMrw.writeInt32(addr + i * 0x50 + 0x0C, typeEnd);
                gMrw.writeInt32(addr + i * 0x50 + 0x10, 0); // 某种验证

                typeEnd = dataAddr;

                int dataAddr2 = typeEnd + pta[i].fCount * 0x14;
                for (int j = 0; j < pta[i].fCount; j++) {
                    dataAddr = dataAddr2 + pta[i]._float[j].pCount * 0x14;
                    for (int k = 0; k < pta[i]._float[j].pCount; k++) {
                        for (int l = 0; l < pta[i]._float[j].pram[k].pCount; l++) {
                            gMrw.writeFloat(dataAddr, (float)pta[i]._float[j].pram[k].pram[l]);
                            dataAddr += 4;
                        }
                        gMrw.writeInt32(blackmem, dataAddr2);
                        gMrw.writeInt32(blackmem + 4, 0);
                        gMrw.writeInt32(dataAddr2, blackmem);
                        blackmem += 8;

                        gMrw.writeInt32(dataAddr2 + 0x04, dataAddr - pta[i]._float[j].pram[k].pCount * 4);
                        gMrw.writeInt32(dataAddr2 + 0x08, dataAddr);
                        gMrw.writeInt32(dataAddr2 + 0x0C, dataAddr);
                        gMrw.writeInt32(dataAddr2 + 0x10, 0);
                        dataAddr2 += 0x14;
                    }
                    gMrw.writeInt32(blackmem, typeEnd);
                    gMrw.writeInt32(blackmem + 4, 0);
                    gMrw.writeInt32(typeEnd, blackmem);
                    blackmem += 8;

                    gMrw.writeInt32(typeEnd + 0x04, dataAddr2 - pta[i]._float[j].pCount * 0x14);
                    gMrw.writeInt32(typeEnd + 0x08, dataAddr2);
                    gMrw.writeInt32(typeEnd + 0x0C, dataAddr2);
                    gMrw.writeInt32(typeEnd + 0x10, 0);
                    typeEnd += 0x14;
                }

                // 浮点段头部
                gMrw.writeInt32(blackmem, addr + 0x14 + i * 0x50);
                gMrw.writeInt32(blackmem + 4, 0);
                gMrw.writeInt32(addr + i * 0x50 + 0x14, blackmem);
                blackmem += 8;
                gMrw.writeInt32(addr + i * 0x50 + 0x18, typeEnd - pta[i].fCount * 0x14);
                gMrw.writeInt32(addr + i * 0x50 + 0x1C, typeEnd);
                gMrw.writeInt32(addr + i * 0x50 + 0x20, typeEnd);
                gMrw.writeInt32(addr + i * 0x50 + 0x24, 0);

                // 这是一个未知的段
                gMrw.writeInt32(blackmem, addr + 0x14 + i * 0x50);
                gMrw.writeInt32(blackmem + 4, 0);
                gMrw.writeInt32(addr + i * 0x50 + 0x28, blackmem);
                blackmem += 8;

                gMrw.writeInt32(addr + i * 0x50 + 0x2C, 0);
                gMrw.writeInt32(addr + i * 0x50 + 0x30, 0);
                gMrw.writeInt32(addr + i * 0x50 + 0x34, 0);
                gMrw.writeInt32(addr + i * 0x50 + 0x38, 0); // 验证

                gMrw.writeInt32(addr + i * 0x50 + 0x3C, 0);

                gMrw.writeInt32(addr + i * 0x50 + 0x40, 0); // 验证

                gMrw.writeInt32(blackmem, addr + i * 0x50 + 0x44);
                gMrw.writeInt32(blackmem + 4, 0);
                gMrw.writeInt32(addr + i * 0x50 + 0x44, blackmem);
                blackmem += 8;

                gMrw.writeInt32(addr + i * 0x50 + 0x48, 0);
                gMrw.writeInt32(addr + i * 0x50 + 0x4C, 0);
                gMrw.writeInt32(addr + i * 0x50 + 0x50, 0);
                gMrw.writeInt32(addr + i * 0x50 + 0x54, 0);
                typeEnd = dataAddr;
            }
        }
        public void InstanceTp(Int32 coor) {
            if (coor == 0) {
                InstanceTp(GetCurrencyRoom().X, GetCurrencyRoom().Y - 1);
            } else if (coor == 1) {
                InstanceTp(GetCurrencyRoom().X, GetCurrencyRoom().Y + 1);

            } else if (coor == 2) {
                InstanceTp(GetCurrencyRoom().X - 1, GetCurrencyRoom().Y);

            } else if (coor == 3) {
                InstanceTp(GetCurrencyRoom().X + 1, GetCurrencyRoom().Y);
            }

        }
        public Int32 GetCharaLevel() {
            return gMrw.readInt32(baseAddr.dwBase_Chara_Level);
        }
        Int32[] MapID = { 0,144,144,144,144,145,145,145,146,146,146,147,147,148,148,149,149,156,156,157,157
        ,158,158,159,159,159,159,     153,153,153,153,153,153,153,153,153,153,
        164,164,164,164,164,164,164,164,164,
        171,171,167,167,167,167,168,168,168,0,0,00,0,0,0,0,0,00,0,0,0,0,0,00,0,0,0,00,0,0,0,00,0,0,0,0,0,0,00,0,0,0,0,00,0,0,0,0,0,0,0,0,0,00,0,0,0,00,0,0,0,00,0,0,0,0,0,0,00,0,0,0,0,00,0,0,0,0,0,0,0,0,0,0
    };
        public Int32 GetLevleInstanceMapId() {
            MapID[69] = 87;
            MapID[68] = 87;
            MapID[67] = 87;
            MapID[66] = 87;
            MapID[65] = 87;
            MapID[64] = 87;


            return MapID[GetCharaLevel()];
        }

        public void sellPrompt(int address)
        {
            at.clear();
            at.pushad();
            at.mov_esi(address);
            at.mov_edi(gMrw.readInt32(address));
            at.push(1);
            at.push(0);
            at.push(1);
            at.push_esi();
            at.push(0x3E);
            at.mov_ecx(0x57C3B50);
            at.mov_eax(0x032C8880);//032951B0    55              push ebp
            at.call_eax();
            at.popad();
            at.retn();
            at.RunRempteThreadWithMainThread();

            int num = gMrw.readInt32(baseAddr.dwBase_Shop, 0xC4B8, 0x1B0, 0x10C);
            int time = 0;
            while (gMrw.readInt32(address + 0x20) > 0 && time < 2000)
            {
                time += 1;
                gMrw.writeInt32(0x559C95C, num);
            }

        }
        public void movCharaPos(Int32 x, Int32 y, Int32 z, Int32 address = 0) {
            if (address == 0)
                address = gMrw.readInt32(baseAddr.dwBase_Character);
            if (gMrw.readInt32(address + 0xA4) == 273)
            {
                at.clear();
                at.pushad();
                at.mov_esi(address);
                at.mov_edi(gMrw.readInt32(address));

                at.mov_eax(z);
                at.mov_ecx(y);
                at.mov_edx(x);
                at.push_eax();
                at.push_ecx();
                at.push_edx();
                at.writeInt8(0x8B);
                at.writeInt8(0x87);
                at.writeInt8(0xB0);
                at.writeInt8(0x00);
                at.writeInt8(0x00);
                at.writeInt8(0x00);//mov eax, [edi + 0xAC];
                at.mov_ecx(address);
                at.call_eax();
                at.popad();
                at.retn();

                at.RunRempteThreadWithMainThread();
            }
            else
            {
                gMrw.writeFloat(gMrw.readInt32(address + 0xAC) + 0x10, (float)x);
                gMrw.writeFloat(gMrw.readInt32(address + 0xAC) + 0x14, (float)y);
            }
        }
        public System.Drawing.Point GetBossRoom() {
            System.Drawing.Point p = new System.Drawing.Point();
            p.X = gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Shop - 8, baseAddr.dwBase_Time, 0xC8) + 0xC70);
            p.Y = gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Shop - 8, baseAddr.dwBase_Time, 0xC8) + 0xC78);
            return p;
        }

        public System.Drawing.Point GetSyRoom()
        {
            System.Drawing.Point p = new System.Drawing.Point();
            p.X = gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Shop - 8, baseAddr.dwBase_Time, 0xC8) + 0xC94);
            p.Y = gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Shop - 8, baseAddr.dwBase_Time, 0xC8) + 0xC9C);
            return p;
        }

        public Int32 GetAddressByName(string Name) {
            Int32 map = gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 End = gMrw.readInt32(map + 0xC4);

            for (Int32 i = gMrw.readInt32(map + 0xC0); i < End; i += 4) {
                Int32 addr = gMrw.readInt32(i);
                string name = gMrw.readString(gMrw.readInt32(addr + 0x404));
                if (name.Contains(Name) == true)
                    return addr;
            }
            return 0;
        }

        public void TermidateObj(Int32 addr) {
            if (addr != 0)
                EncryptionCall(addr + 0x1AC, 0);
        }

        public Int32 GetAddressByCode(int Code,int _type = 0) {
            Int32 map = gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 End = gMrw.readInt32(map + 0xC4);

            for (Int32 i = gMrw.readInt32(map + 0xC0); i < End; i += 4) {
                Int32 addr = gMrw.readInt32(i);
                Int32 type = gMrw.readInt32(addr + 0xA4);

                int code = gMrw.readInt32(addr + 0x400);
                if (code == Code)
                {
                    if (_type != 0)
                    {
                        if (_type == type)
                        {
                            return addr;
                        }
                    }
                    else return addr;
                }
            }
            return 0;
        }

        public bool IsHaveSyEmery() {
            Int32 map = gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 End = gMrw.readInt32(map + 0xC4);

            for (Int32 i = gMrw.readInt32(map + 0xC0); i < End; i += 4) {
                Int32 addr = gMrw.readInt32(i);
                int code = gMrw.readInt32(addr + 0x400);
                if (code == 65746 || code == 65747 || code == 65745 || code == 65749 || code == 65751)
                    return true;
            }
            return false;
        }

        public Int32 IsAddressByAddress(int Address) {
            Int32 map = gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 End = gMrw.readInt32(map + 0xC4);

            for (Int32 i = gMrw.readInt32(map + 0xC0); i < End; i += 4) {
                Int32 addr = gMrw.readInt32(i);
                if (addr == Address)
                    return addr;
            }
            return 0;
        }

        public void renouLoad(Int32 code = 0) {
            if (code == 0)
                code = 140157;
            Int32 equipAddress = CreateEquit(100090073);
            Int32 head = gMrw.readInt32(equipAddress + 0x10E4);
            head = gMrw.readInt32(head + 0x1C, 0x80) + 0x4EC;
            equipData = gMrw.readData((uint)head, 12);
            gMrw.writeInt32(0x100104, 301);
            gMrw.writeInt32(0x100108, 85);
            gMrw.writeInt32(0x10010C, 85);
            gMrw.writeInt32(0x100110, 3000000);
            head = gMrw.readInt32(head);


            gMrw.writeInt32(gMrw.readInt32(head + 4, 0x4) + 0, 25);//触发
            gMrw.writeFloat(gMrw.readInt32(head + 0x18, 0x4, 0x4) + 4, (float)4);//频率
            gMrw.writeInt32(gMrw.readInt32(head + 4, 0x18) + 4, 100);//触发
            gMrw.writeFloat(gMrw.readInt32(head + 0x18, 0x4, 0x2C) + 8, (float)code);//频率

            //Int32 addr = CreateEquit(26564);
            //writeLogLine(addr.ToString());
            //gMrw.writeInt32(gMrw.readInt32(addr + 0xB5C, 4, 4) + 4, 500);//冷却  写0不稳定
            //gMrw.writeInt32(gMrw.readInt32(addr + 0xB5C, 4, 0x18) + 0, 20);

            //gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x4) + 4, (float)4);//范围
            //gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x40) + 4, (float)2);//异常
            //gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x40) + 0xC, (float)9999999);//伤害
            //gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x40) + 8, (float)99);//等级
            //gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x2C) + 4, (float)99);//几率
            //equipData = gMrw.readData((uint)addr + 0xB5c, 12);
        }
        public void renouOpen() {
            Int32 rAddr = GetAddressByName("雷剑");
            equipData1 = gMrw.readData((uint)(gMrw.readInt32(rAddr + 0x2CD0, 0x10E4, 0x1C, 0x80) + 0x4EC), 12);
            gMrw.writedData((uint)(gMrw.readInt32(rAddr + 0x2CD0, 0x10E4, 0x1C, 0x80) + 0x4EC), equipData, 12);
            HideCall(rAddr);
        }
        public void renouHy() {
            Int32 rAddr = GetAddressByName("雷剑");
            gMrw.writedData((uint)(gMrw.readInt32(rAddr + 0x2CD0, 0x10E4, 0x1C, 0x80) + 0x4EC), equipData1, 12);

        }
        public void QuitChooseInstance() {
            SendGameDataStart(0x84);
            SendGameDataEnd();
        }

        #region bfs
        /// <bfs>
        /// 
        struct Node {
            public int parent_id;          //保存父节点的位置
            public int node_id;            //当前节点的序号，以便传递给孩子节点
            public int x, y;                //当前结点对应的坐标
        }
        Node[] Q = new Node[400];
        Int32 sum;
        Int32 Width, Height;
        Int32 nX = 0, nY = 0;//现在的坐标
        Int32 targetX, targetY;//目标坐标
        Int32[,] Result = new Int32[200, 200];
        Int32[,] Map;
        Int32[,] table;
        struct DIRECTION {
            public Int32 right;
            public Int32 down;
            public Int32 up;
            public Int32 left;
        };
        void GetMap() {

            Int32 nMapNum = gMrw.readInt32(baseAddr.dwBase_Quest_Instace_Id);//0061F4BC    F605 00D48803 0>test byte ptr ds:[0x388D400],0x1
            writeLogLine("地图序号 : " + nMapNum);
            front = 0; rear = 0;
            sum = 0;

            Width = gMrw.readInt32(baseAddr.dwBase_Shop - 8, baseAddr.dwBase_Time, 0xC8, 0x2AC, 0 + 8 * nMapNum);
            Height = gMrw.readInt32(baseAddr.dwBase_Shop - 8, baseAddr.dwBase_Time, 0xC8, 0x2AC, 4 + 8 * nMapNum);//获取当前地图宽高

            if (Width <= 0 || Height <= 0)
                return;

            writeLogLine("房间宽 " + Width + "房间高 " + Height);


            Map = new Int32[Width * 3, Height * 3];
            table = new Int32[Width * 3, Height * 3];

            for (Int32 i = 0; i < Width * 3; i++) {
                for (Int32 j = 0; j < Height * 3; j++) {
                    Map[i, j] = 0; table[i, j] = 0;
                }
            }//清零数组

            System.Drawing.Point Begin = GetCurrencyRoom();
            System.Drawing.Point End = GetBossRoom();

            Int32 Temp = gMrw.readInt32(baseAddr.dwBase_Shop - 8, baseAddr.dwBase_Time, 0xC8, 0x2C0, 4 + 0x14 * nMapNum);

            Begin.X = Begin.X * 3 + 1;
            Begin.Y = Begin.Y * 3 + 1;

            nX = Begin.X;
            nY = Begin.Y;

            targetX = GetBossRoom().X * 3 + 1;
            targetY = GetBossRoom().Y * 3 + 1;

            //SendGameNotice(L"当前x = %d,y = %d 目标x = %d，y = %d", nX, nY, targetX, targetY);
            Int32 RoomData = 0;

            for (Int32 i = 0; i < Height; i++) {
                for (Int32 j = 0; j < Width; j++) {
                    DIRECTION T;

                    Int32 cy = i * 3 + 1;
                    Int32 cx = j * 3 + 1;

                    RoomData = gMrw.readInt32(Temp + 4 * i * Width + j * 4);

                    T = GetRoomDirection(RoomData);

                    if (RoomData != 0)
                        Map[cx, cy] = 1;

                    if (T.left == 1) {
                        Map[cx - 1, cy] = 1;
                    }
                    if (T.right == 1) {
                        Map[cx + 1, cy] = 1;
                    }
                    if (T.up == 1) {
                        Map[cx, cy - 1] = 1;
                    }
                    if (T.down == 1) {
                        Map[cx, cy + 1] = 1;
                    }
                }
            }
        }

        Int32[,] DT = {
        { 0, 0, 0, 0 },
        { 0, 1, 0, 0 },
        { 0, 0, 1, 0 },
        { 0, 1, 1, 0 },
        { 1, 0, 0, 0 },
        { 1, 1, 0, 0 },
        { 1, 0, 1, 0 },
        { 1, 1, 1, 0 },
        { 0, 0, 0, 1 },
        { 0, 1, 0, 1 },
        { 0, 0, 1, 1 },
        { 0, 1, 1, 1 },
        { 1, 0, 0, 1 },
        { 1, 1, 0, 1 },
        { 1, 0, 1, 1 },
        { 1, 1, 1, 1 },
    };
        DIRECTION GetRoomDirection(Int32 data) {
            DIRECTION a = new DIRECTION();

            a.left = DT[data, 0];
            a.right = DT[data, 1];
            a.up = DT[data, 2];
            a.down = DT[data, 3];

            return a;
        }
        int front, rear;
        private void DeQueue(ref int i, ref int j, ref int k) {
            i = Q[front].x;
            j = Q[front].y;
            k = Q[front].node_id;
            front++;                        //出列一个节点
        }
        private void EnQueue(int i, int j, int k) {
            Q[rear].x = i;
            Q[rear].y = j;                  //保存当前节点对应的坐标位置
            Q[rear].parent_id = k;          //保存父节点的序号
            Q[rear].node_id = rear;         //保存当前节点序号
            rear++;
        }
        bool GetNextPos(ref int i, ref int j, int count) {
            switch (count) {
                case 1:
                    (j)++;
                    return true;      //右
                case 2:
                    (i)++;
                    return true;      //下
                case 3:
                    (j)--;
                    return true;      //左
                case 4:
                    (i)--;
                    return true;      //上
                default:
                    return false;
            }
        }
        Int32 GetD() {
            for (int i = 0; i < 200; i++) {
                for (int j = 0; j < 200; j++)
                    Result[i, j] = 0;
            }

            Int32 x = nX, y = nY;
            for (Int32 i = 0; i < (sum - 1) / 3; i++) {
                Map[x, y] = 0;
                if (x + 3 < Width * 3 && Map[x + 1, y] == 2) {
                    Result[(x - 1) / 3, (y - 1) / 3] = 4;
                    x += 3;
                    Map[x - 1, y] = 0;
                } else if (x - 3 >= 0 && Map[x - 1, y] == 2) {
                    Result[(x - 1) / 3, (y - 1) / 3] = 3;
                    x -= 3;
                    Map[x + 1, y] = 0;
                } else if (y - 3 >= 0 && Map[x, y - 1] == 2) {
                    Result[(x - 1) / 3, (y - 1) / 3] = 1;
                    Result[(x - 1) / 3, (y - 1) / 3] = 1;
                    y -= 3;
                    Map[x, y + 1] = 0;
                } else if (y + 3 < Height * 3 && Map[x, y + 1] == 2) {
                    Result[(x - 1) / 3, (y - 1) / 3] = 2;
                    y += 3;
                    Map[x, y - 1] = 0;
                }
            }
            return 1;
        }
        void ShortestPath_BFS(int i, int j, int x, int y) {
            int count, m, n, k = 0;
            EnQueue(i, j, -1);
            Map[i, j] = 1;      //起点入队,标记起点已走过
            while (true) {
                count = 1;
                DeQueue(ref i, ref j, ref k);
                n = i; m = j;                         //保存当前位置
                while (GetNextPos(ref i, ref j, count)) {
                    count++;//
                    if (Map[i, j] == 1) {
                        EnQueue(i, j, k);
                        Map[i, j] = 0;
                        if (i == x && j == y)
                            return;       //到达终点,(8,9)是默认终点，可以任意修改
                    }
                    i = n;
                    j = m;                           //保证遍历当前坐标的所有相邻位置
                }
            }
        }
        void ShortestPath() {
            int i, j, k;
            k = rear - 1;
            while (k != -1) {
                i = Q[k].x;
                j = Q[k].y;
                Map[i, j] = 2;
                k = Q[k].parent_id;
            }
            for (int a = 0; a < Height * 3; a++) {
                for (int s = 0; s < Width * 3; s++)
                    if (Map[s, a] == 2) {
                        sum++;
                    }
            }
            writeLogLine("消耗最少疲劳值： " + ((sum - 1) / 3 + 1));

        }
        public Int32 GetNextRoom(Point p) {
            return (Result[p.X, p.Y] - 1);
        }
        public void bfs_load() {
            GetMap();
            //SendGameNotice(L"地图打印成功");
            ShortestPath_BFS(nX, nY, targetX, targetY);
            //SendGameNotice(L"最短路径搜索成功");
            ShortestPath();
            //SendGameNotice(L"最短路径搜索成功1111111111");
            GetD();
        }

        public bool isNextRoomBoss()
        {
            int p = GetNextRoom(GetCurrencyRoom());
            Point curr = GetCurrencyRoom();
            Point boss = GetBossRoom();

            if (   (p == 0 && curr.X == boss.X && curr.Y - 1 == boss.Y)
                || (p == 1 && curr.X == boss.X && curr.Y + 1 == boss.Y)
                || (p == 2 && curr.Y == boss.Y && curr.X - 1 == boss.X)
                || (p == 3 && curr.Y == boss.Y && curr.X + 1 == boss.X)
                )
                return true;
            return false;
        }

        public void resolveAll() {
            Int32 bag = gMrw.readInt32(baseAddr.dwBase_Bag);
            Int32 first = gMrw.readInt32(bag + 88);
            Int32 item = first + 36;
            for (Int32 i = 0; i < 56; i++) {
                Int32 point = gMrw.readInt32(item + i * 4);
                if (point != 0)
                    SendResolve(i + 9);
            }
        }
        #endregion

        public void SendResolve(Int32 iPos) {
            SendGameDataStart(26);
            SendGameDataAddInt16(iPos);
            SendGameDataAddInt8(0);
            SendGameDataAddInt16(65535);
            SendGameDataAddInt32(354);
            SendGameDataEnd();
        }

        public void dealSpacial() {

            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);
            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);
            Int32 x, y;
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);

                string name = gMrw.readString(gMrw.readInt32(onobj + 0x400));
                string Obj_name = gMrw.readString(gMrw.readInt32(onobj + 0x4EC));

                if (Obj_name.IndexOf("small_generator") >= 0) {
                    int addr = GetAddressByName("巨人波");
                    if (addr > 0) {
                        x = (Int32)gMrw.readFloat(gMrw.readInt32(addr + 0xAC) + 0x10);
                        y = (Int32)gMrw.readFloat(gMrw.readInt32(addr + 0xAC) + 0x14);

                        gMrw.writeFloat(gMrw.readInt32(onobj + 0xAC) + 0x10, (float)x);
                        gMrw.writeFloat(gMrw.readInt32(onobj + 0xAC) + 0x14, (float)y);
                    }
                }

                if (Obj_name.IndexOf("gate_strap1") >= 0) {
                    int addr = GetAddressByName("派普");
                    if (addr > 0) {
                        x = (Int32)gMrw.readFloat(gMrw.readInt32(addr + 0xAC) + 0x10);
                        y = (Int32)gMrw.readFloat(gMrw.readInt32(addr + 0xAC) + 0x14);

                        gMrw.writeFloat(gMrw.readInt32(onobj + 0xAC) + 0x10, (float)x);
                        gMrw.writeFloat(gMrw.readInt32(onobj + 0xAC) + 0x14, (float)y);
                    }
                }
            }
        }

        public void SetCharaMoveMap() {
            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);
            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);
            Int32 x, y, z;
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);

                string temp = gMrw.readString(gMrw.readInt32(onobj + 0x4EC));

                if (temp.Contains("MoveMap") || temp.Contains("Crevasse")) {
                    x = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x10);
                    y = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x14);
                    z = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x18);

                    movCharaPos(x, y, 0);

                    break;
                }
            }
        }

        public void setCharaPosSpacialEmery(Int32 address = 0) {
            Int32[] ecode = { 62516, 62513, 62124, 56135, 61320, 65022, 65023, 48015, 38762, 62323, 107000924, 61323,701,700 ,84};


            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);
            if (address == 0)
                address = chr;
            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);
            Int32 x, y, z;
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);
                Int32 zy = gMrw.readInt32(onobj + 0x828);
                //if (zy == 0 || zy == 0xC8) {
                //    continue;
                //}

                Int32 type = gMrw.readInt32(onobj + 0xA4);
                Int32 grope = gMrw.readInt32(onobj + 0x828);

                if (grope == 0)
                    continue;
                if (onobj == gMrw.readInt32(baseAddr.dwBase_Character))
                    continue;

                Int32 code = gMrw.readInt32(onobj + 0x400);
                foreach (Int32 c in ecode) {
                    if (code == c) {
                        x = getObjPos(onobj).x;
                        y = getObjPos(onobj).y;
                        z = getObjPos(onobj).z;
                        if (x == 0 || y == 0)
                            continue;

                        if (gMrw.readInt32(address + 0xA4) == 273)
                            movCharaPos(x, y, 0, address);
                        else {
                            gMrw.writeFloat(gMrw.readInt32(address + 0xAC) + 0x10, (float)x);
                            gMrw.writeFloat(gMrw.readInt32(address + 0xAC) + 0x14, (float)y);
                        }

                        return;
                    }
                }
            }
        }

        public void lockWear() {
            for (int i = baseAddr.dwOffset_Equip_wq;i <= baseAddr.dwOffset_Equip_wq + 0x18; i += 4) {
                if (gMrw.readInt32(baseAddr.dwBase_Character, i) == 0)
                    continue;
                EncryptionCall(gMrw.readInt32(baseAddr.dwBase_Character, i) + 0x60C, 0);
            }
        }

        public void SetCharaPos(Int32 address = 0) {

            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);
            if (address == 0)
                address = chr;
            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);
            Int32 x, y, z;
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);
                Int32 zy = gMrw.readInt32(onobj + 0x828);
                //if (zy == 0 || zy == 0xC8) {
                //    continue;
                //}

                Int32 type = gMrw.readInt32(onobj + 0xA4);
                Int32 grope = gMrw.readInt32(onobj + 0x828);

                //if (type == 1057)
                //{
                //                        x = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + baseAddr.dwOffset_Obj_Pos));
                //    y = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + baseAddr.dwOffset_Obj_Pos) + 4);
                //    z = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + baseAddr.dwOffset_Obj_Pos) + 8);
                //    if (x == 0 || y == 0)
                //        continue;

                //    Int32 px;
                //    if (gMrw.readInt8(gMrw.readInt32(address + 0xAC,4) + 0x40) == 1)
                //        px = -50;
                //    else
                //        px = 50;

                //    if (gMrw.readInt32(address + 0xA4) == 273)
                //        movCharaPos(x + px, y, z, address);
                //    else {
                //        gMrw.writeFloat(gMrw.readInt32(address + 0xAC) + 0x10, (float)x);
                //        gMrw.writeFloat(gMrw.readInt32(address + 0xAC) + 0x14, (float)y);
                //    }

                //    return ;
                //}
                

                    if (grope == 0)
                        continue;
                    if (onobj == gMrw.readInt32(baseAddr.dwBase_Character))
                        continue;
                    if (gMrw.readInt32(onobj + 0x3AE4) == 0)
                        continue;
                

                if (type == 0x211 || type == 545 || type == 529) {
                    x = getObjPos(onobj).x;
                    y = getObjPos(onobj).y;
                    z = getObjPos(onobj).z;
                    if (x == 0 || y == 0)
                        continue;

                    Int32 px;
                    if (gMrw.readInt8(gMrw.readInt32(address + 0xB8, 4) + 0x40) == 1)
                        px = -50;
                    else
                        px = 50;

                    if (gMrw.readInt32(address + 0xA4) == 273)
                        movCharaPos(x + px, y, z, address);
                    else {
                        gMrw.writeFloat(gMrw.readInt32(address + 0xB8) + 0x10, (float)x);
                        gMrw.writeFloat(gMrw.readInt32(address + 0xB8) + 0x14, (float)y);
                    }

                    return ;
                }
            }
        }


        public void SetCharaPosWithSd() {
            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);
            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);
            Int32 x, y, z;
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);
                Int32 zy = gMrw.readInt32(onobj + 0x828);
                //if (zy == 0 || zy == 0xC8) {
                //    continue;
                //}

                Int32 type = gMrw.readInt32(onobj + 0xA4);
                Int32 grope = gMrw.readInt32(onobj + 0x828);


                if (grope == 0)
                    continue;
                if (onobj == gMrw.readInt32(baseAddr.dwBase_Character))
                    continue;
                if (gMrw.readInt32(onobj + 0x3AE4) == 0)
                    continue;

                Int32 code = (Int32)gMrw.readInt32(onobj + 0x400);

                if (code == 69018 || code == 64067 || code == 64068 || code == 64069 || code == 64069 || code == 64070) {
                    x = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x10);
                    y = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x14);
                    z = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x18);
                    movCharaPos(x, y, 0);

                }

            }
        }

        public void SetObjPos() {
            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);
            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);
            Int32 x, y, z;
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);
                Int32 zy = gMrw.readInt32(onobj + 0x828);
                if (zy == 0 || zy == 0xC8) {
                    continue;
                }

                Int32 type = gMrw.readInt32(onobj + 0xA4);
                if (type == 0x211) {
                    x = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x10);
                    y = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x14);
                    z = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x18);
                    movCharaPos(x, y, 0);
                    break;
                }
            }
        }

        public void GatherItem() {
            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);
            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);
            Int32 x, y;
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);

                Int32 type = gMrw.readInt32(onobj + 0xA4);
                if (type == 0x121) {
                    x = getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).x;
                    y = getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).y;

                    gMrw.writeFloat(gMrw.readInt32(onobj + 0xAC) + 0x10, (float)x);
                    gMrw.writeFloat(gMrw.readInt32(onobj + 0xAC) + 0x14, (float)y);

                }
            }
        }

        public void GatherItem1()
        {
            //gMrw.writedData(0x100100, new byte[] { 0x2F, 0x00, 0x2F, 0x00, 0xFB, 0x79, 0xA8, 0x52, 0x69, 0x72, 0xC1, 0x54, 0x00, 0x00 }, 14);
            bool bflag = false;

            while (!bflag) {
                bool flag = true;
                Int32 map = gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
                Int32 End = gMrw.readInt32(map + 0xC4);

                for (Int32 i = gMrw.readInt32(map + 0xC0); i < End; i += 4)
                {
                    Int32 b = gMrw.readInt32(i);
                    if (gMrw.readInt32(b + 0xA4) == 0x121)
                    {
                        //int point = gMrw.readInt32(b + 0x16C0);
                        int x = getObjPos(b).x;
                        if (x != getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).x)
                            flag = false;

                        x = getObjPos(b).x;
                        if (x != getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).x)
                            flag = false;
                    }
                }
                bflag = flag;
                if (!bflag)
                {
                    EncryptionCall(gMrw.readInt32(baseAddr.dwBase_Character) + 0x64f4, 0);

                    at.clear();
                    at.pushad();
                    //at.mov_eax(0x100100);
                    //at.push_eax();
                    at.mov_ecx(gMrw.readInt32(baseAddr.dwBase_Character));
                    at.mov_ebx(0x027344C0);//0271D790    55              push ebp

                    at.call_ebx();
                    //at.add_esp(0x4);
                    at.popad();
                    at.retn();

                    at.RunRempteThreadWithMainThread();
                    Thread.Sleep(100);
                }
             
            }


            //Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);
            //Int32 map = gMrw.readInt32(chr + 0xC8);
            //Int32 dest = gMrw.readInt32(map + 0xC4);
            //Int32 x, y;
            //for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4)
            //{
            //    Int32 onobj = gMrw.readInt32(i);

            //    Int32 type = gMrw.readInt32(onobj + 0xA4);
            //    if (type == 0x121)
            //    {
            //        x =  getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).x;
            //        y = getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).y;

            //        gMrw.writeFloat(gMrw.readInt32(onobj + 0xAC) + 0x10, (float)x);
            //        gMrw.writeFloat(gMrw.readInt32(onobj + 0xAC) + 0x14, (float)y);

            //    }
            //}
        }

        public void xiguaiWithCode(Int32 Code,Int32 addr = 0) {
            if (addr == 0)
                addr = gMrw.readInt32(baseAddr.dwBase_Character);

            Int32 map = gMrw.readInt32(addr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);
            Int32 x, y;
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);


                Int32 type = gMrw.readInt32(onobj + 0xA4);
                Int32 grope = gMrw.readInt32(onobj + 0x828);


                    Int32 code = (Int32)gMrw.readInt32(onobj + 0x400);

                if (code == Code) {
                    x = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x10);
                    y = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x14);

                    if (x == 0 || y == 0)
                        continue;
                    x = (Int32) getObjPos(addr).x;
                    y = (Int32) getObjPos(addr).y;

                    Random r = new Random();

                    gMrw.writeFloat(gMrw.readInt32(onobj + 0xAC) + 0x10, (float)(x));
                    gMrw.writeFloat(gMrw.readInt32(onobj + 0xAC) + 0x14, (float)(y));

                    Thread.Sleep(100);
                }
            }
        }

        public void xiguaiWithSD(Int32 addr = 0) {

            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);

            if (addr == 0)
                addr = chr;

            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);
            Int32 x, y;
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);


                Int32 type = gMrw.readInt32(onobj + 0xA4);
                Int32 grope = gMrw.readInt32(onobj + 0x828);


                if (type == 0x211 || type == 273) {
                    Int32 code = (Int32)gMrw.readInt32(onobj + 0x400);

                    if (code == 69018
                        || code == 64067
                        || code == 64068
                        || code == 64069
                        || code == 64069
                        || code == 64070
                        || code == 58017
                        || code == 69540

                        ) {
                        x = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x10);
                        y = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x14);

                        if (x == 0 || y == 0)
                            continue;
                        x = (Int32) getObjPos(addr).x;
                        y = (Int32) getObjPos(addr).y;

                        Random r = new Random();

                        gMrw.writeFloat(gMrw.readInt32(onobj + 0xAC) + 0x10, (float)(x));
                        gMrw.writeFloat(gMrw.readInt32(onobj + 0xAC) + 0x14, (float)(y));

                        Thread.Sleep(100);
                    }
                }
            }
        }


        public void ItemCallWithSD() {

            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);


            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);
            Int32 x, y;
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);


                Int32 type = gMrw.readInt32(onobj + 0xA4);
                Int32 grope = gMrw.readInt32(onobj + 0x828);


                if (type == 0x211 || type == 273) {
                    Int32 code = (Int32)gMrw.readInt32(onobj + 0x400);

                    if (code == 69018
                        || code == 64067
                        || code == 64068
                        || code == 64069
                        || code == 64069
                        || code == 64070
                        || code == 58017


                        ) {
                        x = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x10);
                        y = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x14);

                        if (x == 0 || y == 0)
                            continue;

                        ItemCall(onobj, 1079);

                        Thread.Sleep(100);
                    }
                }
            }
        }

        public void EncryptionCall(int addr, int data,bool IsMainThread = false) {
            int eax = addr & 0xF;
            int ebp_c = at.GetVirtualAddr() + 0xD00;
            gMrw.writeInt32(ebp_c, data);
            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.mov_ecx(addr);
            at.push(addr + 4);
            at.push(ebp_c);
            if (eax == 0)
                eax = baseAddr.dwCall_Encryption1;
            else if (eax == 4)
                eax = baseAddr.dwCall_Encryption2;
            else if (eax == 8)
                eax = baseAddr.dwCall_Encryption3;
            else if (eax == 0xC)
                eax = baseAddr.dwCall_Encryption4;
            else
                eax = baseAddr.dwCall_Encryption5;
            at.mov_eax(eax);
            at.call_eax();
            if (IsMainThread)
                at.mov_virtualaddr_c3();
            at.popad();
            at.retn();
            at.RunRempteThreadWithMainThread();

        }
        public void ArrangeBag(Int32 pos)
        {

            gMrw.writeInt32(at.GetVirtualAddr() + 0xE00, 0x41cc6b4);
            gMrw.writeInt32(at.GetVirtualAddr() + 0xE04, 0x624d410);
            gMrw.writeInt32(at.GetVirtualAddr() + 0xE08, 0);
            gMrw.writeInt32(at.GetVirtualAddr() + 0xE0C, 14);
            gMrw.writeInt32(at.GetVirtualAddr() + 0xE10, 0);
            gMrw.writeInt32(at.GetVirtualAddr() + 0xE14, 0);
            gMrw.writeInt32(at.GetVirtualAddr() + 0xE18, pos);
            gMrw.writeInt32(at.GetVirtualAddr() + 0xE1C, 0);
            gMrw.writeInt32(at.GetVirtualAddr() + 0xE20, 0x14);

            SendGameDataStart(0x14);
            at.push(at.GetVirtualAddr() + 0xE00);
            at.mov_ecx(gMrw.readInt32(baseAddr.dwBase_SEND));
            at.mov_eax(0x03455320);//03455320    55              push ebp
            at.call_eax();
            SendGameDataEnd();
        }
        public void xiguai(Int32 addr = 0, Int32 xcode = 0) {

            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);

            if (addr == 0)
                addr = chr;

            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);
            Int32 x, y;
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);


                Int32 type = gMrw.readInt32(onobj + 0xA4);
                Int32 grope = gMrw.readInt32(onobj + 0x828);
                Int32 code = gMrw.readInt32(onobj + 0x400);

                if (grope == 0)
                    continue;
                if (onobj == gMrw.readInt32(baseAddr.dwBase_Character))
                    continue;
                if (gMrw.readInt32(onobj + 0x3AE4) == 0)
                    continue;
                if (xcode != 0 && code != xcode)
                    continue;

                if (type == 0x211 || type == 273) {
                    x = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x10);
                    y = (Int32)gMrw.readFloat(gMrw.readInt32(onobj + 0xAC) + 0x14);

                    if (x == 0 || y == 0)
                        continue;
                    x = getObjPos(onobj).x;
                    y = getObjPos(onobj).y;

                    Random r = new Random();

                    gMrw.writeFloat(gMrw.readInt32(onobj + 0xAC) + 0x10, (float)(x + r.Next(-20, 20)));
                    gMrw.writeFloat(gMrw.readInt32(onobj + 0xAC) + 0x14, (float)(y + r.Next(-20, 20)));

                }
            }
        }

        public void checkGrope() {

            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);


            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);


                Int32 type = gMrw.readInt32(onobj + 0xA4);
                Int32 grope = gMrw.readInt32(onobj + 0x828);
                Int32 code = gMrw.readInt32(onobj + 0x400);

                if (grope == 0)
                    continue;
                if (onobj == gMrw.readInt32(baseAddr.dwBase_Character))
                    continue;
                if (gMrw.readInt32(onobj + 0x3AE4) == 0)
                    continue;
                if (code == 107000904 && getObjPos(onobj).x != 801) {
                    continue;
                }
                if (code == 107000904 && getObjPos(onobj).x == 801) {
                    gMrw.writeInt32(onobj + 0x828, 0);
                    return;
                }

                if (type == 0x211 || type == 273) {
                    gMrw.writeInt32(onobj + 0x828, 0);
                }
            }
        }


        public void checkEmery(Int32 cCode = 69540, bool IsCleaarEmery = true,bool IsMainThread = false) {


            Int32[] id = new int[100];
            Int32[] level = new int[100];

            int n = 0;

            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);
            Int32 addr = 0;

            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);

            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);
                Int32 grope = gMrw.readInt32(onobj + 0x828);
                if (grope == 200) {
                    addr = onobj;
                    break;
                }
            }
            if (addr == 0) return;

            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4) {
                Int32 onobj = gMrw.readInt32(i);


                Int32 type = gMrw.readInt32(onobj + 0xA4);
                Int32 grope = gMrw.readInt32(onobj + 0x828);
                Int32 code = gMrw.readInt32(onobj + 0x400);

                if (grope == 0)
                    continue;
                if (onobj == gMrw.readInt32(baseAddr.dwBase_Character))
                    continue;
                if (gMrw.readInt32(onobj + 0x3AE4) == 0)
                    continue;
                if (code == cCode)
                    continue;
                if (code == 107000904)
                    continue;

                if (type == 0x211 || type == 273) {
                    id[n++] = onobj;
                    
                }
            }



            if (n != 0) {

                Array.Resize(ref id, n);
                if (n > 100)
                    MessageBox.Show("怪物过多");

                for (int i = 0; i < n; i++) {

                    //56016 恶灵艾维斯 出来就死
                    //56030 派普·乔 直接毒死自己
                    //56114死掉的魔剑
                    //61413蜥蜴之魂死掉 - -
                    //61423 伪装者 死掉- -
                    //61542黑豹罗敦 直接自杀
                    //61543 黑暗幽灵 直接自杀
                    //63004木质人偶 消失
                    //63005 萨斯的仆人 消失
                    //61786 萨斯的仆人 瞬间消失
                    //61787木质人偶，瞬间消失
                    //64014安德莱斯的左臂
                    //64015安德莱斯的右臂
                    CreateEmery(addr, gMrw.Decryption(id[i] + 0xAC), cCode);//69540 机器人 
                    if (IsCleaarEmery)
                        TermidateObj(id[i]);
                    else {
                        EncryptionCall(id[i] + 0xAC, 0,IsMainThread);
                        gMrw.writeInt32(id[i] + 0x828, 0);
                    }

                }
            }
        }


        public void FreezeWithSuit() {

        }

        int pEquip = 0;
        int DataHead = 0;


        public void CheckSkill(Int32 cPoint, Int32 SkillID, Int32 CharaID, Int32 lpara1_1, Int32 lpara1_2, Int32 lpara2_1, Int32 lpara2_2, Int32 lpara3_1, Int32 lpara3_2, Int32 hurt_1, Int32 hurt_2, Int32 hurt_3) {
            if (gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) == 0)
            {
                MessageBox.Show("请穿戴武器");
            }
            //DataHead = gMrw.readInt32(pEquip + 0xac8);
            writeLogLine("初始化创建");
            pEquip = CreateEquit(108000383);
            DataHead = gMrw.readInt32(pEquip + 0xac8);

            //writeLogLine(DataHead.ToString());
            //gMrw.writedData((uint)gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0xAC8, gMrw.readData((uint)pEquip + 0xac8, 12), 12);
            gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0xAC8, gMrw.readInt32(pEquip + 0xac8));
            gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0xAC8 + 4, gMrw.readInt32(pEquip + 0xac8 + 4));
            gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0xAC8 + 8, gMrw.readInt32(pEquip + 0xac8 + 8));

            if (DataHead == 0)
            {
                MessageBox.Show("请将鼠标对准冥焰穿刺");
                return;
            }
            Int32 num = 0;
            //for (Int32 i = baseAddr.dwOffset_Equip_wq; i <= baseAddr.dwOffset_Equip_wq + 0x2C; i += 4) {
            //    if (gMrw.readInt32(cPoint + i) == 0)
            //        continue;
            //    if (i == baseAddr.dwOffset_Equip_wq + 0x4)
            //        continue;
            //    writeLogLine(gMrw.Decryption(gMrw.readInt32(cPoint + i) + 0xF8C).ToString());
            //    EncryptionCall(gMrw.readInt32(cPoint + i) + 0xF8C, 114);
            //    gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0xF9C, 1);

            //    if (num++ >= 3)
            //        break;
            //}

            //if (num < 3) {
            //    MessageBox.Show("请至少穿戴三件以上装备");
            //    return;
            //}
            //writeLogLine(Convert.ToString(DataHead, 16));
            //DataHead = gMrw.readInt32(DataHead + 0xC, 0xC, 0, 0x480);
            writeLogLine(Convert.ToString(pEquip, 16));
            writeLogLine(Convert.ToString(DataHead, 16));


            writeLogLine("技能id" + SkillID.ToString());
            int temp = DataHead;

            //Int32 lpara2 = Convert.ToInt32(EDIT_SK_2_SX.Text);
            //Int32 lpara3 = Convert.ToInt32(EDIT_SK_3_SX.Text);
            //return;
            temp += (40 * 0);
            gMrw.writeInt32(temp, CharaID);
            EncryptionCall(temp + 4, SkillID);
            gMrw.writeInt32(temp + 12, 0);
            EncryptionCall(temp + 16, 1);
            EncryptionCall(temp + 24, -100);
            gMrw.writeInt32(temp + 32, 2);
            gMrw.writeInt32(temp + 36, 28);

            temp += (40);
            gMrw.writeInt32(temp, CharaID);
            EncryptionCall(temp + 4, SkillID);
            gMrw.writeInt32(temp + 12, lpara1_1);
            EncryptionCall(temp + 16, 1);
            EncryptionCall(temp + 24, hurt_1);
            gMrw.writeInt32(temp + 32, lpara1_2);
            gMrw.writeInt32(temp + 36, 28);

            temp += (40);
            gMrw.writeInt32(temp, CharaID);
            EncryptionCall(temp + 4, SkillID);
            gMrw.writeInt32(temp + 12, lpara2_1);
            EncryptionCall(temp + 16, 1);
            EncryptionCall(temp + 24, hurt_2);
            gMrw.writeInt32(temp + 32, lpara2_2);
            gMrw.writeInt32(temp + 36, 28);

            temp += (40);
            gMrw.writeInt32(temp, CharaID);
            EncryptionCall(temp + 4, SkillID);
            gMrw.writeInt32(temp + 12, 0);
            EncryptionCall(temp + 16, 1);
            EncryptionCall(temp + 24, -100);
            gMrw.writeInt32(temp + 32, 3);
            gMrw.writeInt32(temp + 36, 28);

            temp += (40);
            gMrw.writeInt32(temp, CharaID);
            EncryptionCall(temp + 4, SkillID);
            gMrw.writeInt32(temp + 12, 0);
            EncryptionCall(temp + 16, 1);
            EncryptionCall(temp + 24, -100);
            gMrw.writeInt32(temp + 32, 5);
            gMrw.writeInt32(temp + 36, 28);

            //DataHead = gMrw.readInt32(pEquip + 0xF78);
            //if (DataHead == 0 || gMrw.readInt32(pEquip + 0x20) != 105006) {
            //    writeLogLine("初始化创建");
            //    pEquip = CreateEquit(105006);
            //    DataHead = gMrw.readInt32(pEquip + 0xF78);
            //}
            ////writeLogLine(DataHead.ToString());

            //if (DataHead == 0) {
            //    MessageBox.Show("请将鼠标对准冥焰穿刺");
            //    return;
            //}
            //Int32 num = 0;
            //for (Int32 i = baseAddr.dwOffset_Equip_wq; i <= baseAddr.dwOffset_Equip_wq + 0x2C; i += 4) {
            //    if (gMrw.readInt32(cPoint + i) == 0)
            //        continue;
            //    if (i == baseAddr.dwOffset_Equip_wq + 0x4)
            //        continue;
            //    writeLogLine(gMrw.Decryption(gMrw.readInt32(cPoint + i) + 0xF8C).ToString());
            //    gMrw.Encryption(gMrw.readInt32(cPoint + i) + 0xF8C, 114);
            //    gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0xF9C, 1);

            //    if (num++ >= 3)
            //        break;
            //}

            //if (num < 3) {
            //    MessageBox.Show("请至少穿戴三件以上装备");
            //    return;
            //}
            //writeLogLine(Convert.ToString(DataHead, 16));
            //DataHead = gMrw.readInt32(DataHead + 0xC, 0xC, 0, 0x480);
            //writeLogLine(Convert.ToString(DataHead, 16));

            //int temp = DataHead;

            ////cd
            //gMrw.writeInt32(temp, CharaID);
            //EncryptionCall(temp + 4, SkillID);
            //gMrw.writeInt32(temp + 12, 0);
            //EncryptionCall(temp + 16, 1);
            //EncryptionCall(temp + 24, -100);
            //gMrw.writeInt32(temp + 32, 2);
            //gMrw.writeInt32(temp + 36, 26);

            ////1 0
            //temp += (40);
            //gMrw.writeInt32(temp, CharaID);
            //EncryptionCall(temp + 4, SkillID);
            //gMrw.writeInt32(temp + 12, lpara1_1);
            //EncryptionCall(temp + 16, 1);
            //EncryptionCall(temp + 24, hurt_1);
            //gMrw.writeInt32(temp + 32, lpara1_2);
            //gMrw.writeInt32(temp + 36, 26);

            ////2 0
            //temp += (40);
            //gMrw.writeInt32(temp, CharaID);
            //EncryptionCall(temp + 4, SkillID);
            //gMrw.writeInt32(temp + 12, lpara2_1);
            //EncryptionCall(temp + 16, 1);
            //EncryptionCall(temp + 24, hurt_2);
            //gMrw.writeInt32(temp + 32, lpara2_2);
            //gMrw.writeInt32(temp + 36, 26);

            ////3 0
            //temp += (40);
            //gMrw.writeInt32(temp, CharaID);
            //EncryptionCall(temp + 4, SkillID);
            //gMrw.writeInt32(temp + 12, lpara3_1);
            //EncryptionCall(temp + 16, 1);
            //EncryptionCall(temp + 24, hurt_3);
            //gMrw.writeInt32(temp + 32, lpara3_2);
            //gMrw.writeInt32(temp + 36, 26);
        }

        public Point getNextRoomDoorPoint(Int32 rg)
        {
            Int32 map = gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 end = gMrw.readInt32(map + 0xC4);

            Point[] p = new Point[4];

            for (int i = gMrw.readInt32(map + 0xC0); i < end; i += 4)
            {
                Int32 addr = gMrw.readInt32(i);
                Int32 type = gMrw.readInt32(addr + 0xA4);

                if (type == 4129)
                {
                    Int32 x = getObjPos(addr).x;
                    Int32 y = getObjPos(addr).y;

                    string temp = gMrw.readString(gMrw.readInt32(addr + 0x4EC));

                    if (temp.IndexOf("Meltdown") >= 0)
                    {
                        temp = temp.Substring(temp.IndexOf("Meltdown") + 1);
                    }

                    if (temp.IndexOf("wall") < 0)
                    {

                        if (temp.IndexOf("Up") >= 0 || temp.IndexOf("up") >= 0)
                        {
                            p[0].X = x;
                            p[0].Y = y;
                        }

                        if (temp.IndexOf("Down") >= 0 || temp.IndexOf("down") >= 0)
                        {

                            p[1].X = x;
                            p[1].Y = y;
                        }

                        if (temp.IndexOf("Left") >= 0 || temp.IndexOf("left") >= 0)
                        {

                            p[2].X = x;
                            p[2].Y = y;
                        }

                        if (temp.IndexOf("Right") >= 0 || temp.IndexOf("right") >= 0)
                        {

                            p[3].X = x;
                            p[3].Y = y;
                        }
                    }

                }
            }
            return p[rg];
        }


        public void CoorTp(Int32 rg,int raddr = 0) {

            raddr = raddr == 0 ? gMrw.readInt32(baseAddr.dwBase_Character) : raddr;

            Int32 map = gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 end = gMrw.readInt32(map + 0xC4);

            Point[] p = new Point[4];

            for (int i = gMrw.readInt32(map + 0xC0); i < end; i += 4) {
                Int32 addr = gMrw.readInt32(i);
                Int32 type = gMrw.readInt32(addr + 0xA4);

                if (type == 4129) {
                    Int32 x = getObjPos(addr).x;
                    Int32 y = getObjPos(addr).y;

                    string temp = gMrw.readString(gMrw.readInt32(addr + 0x4EC));

                    if (temp.IndexOf("Meltdown") >= 0) {
                        temp = temp.Substring(temp.IndexOf("Meltdown") + 1);
                    }

                    if (temp.IndexOf("wall") < 0) {

                        if (temp.IndexOf("Up") >= 0 || temp.IndexOf("up") >= 0) {
                            p[0].X = x;
                            p[0].Y = y;
                        }

                        if (temp.IndexOf("Down") >= 0 || temp.IndexOf("down") >= 0) {

                            p[1].X = x;
                            p[1].Y = y;
                        }

                        if (temp.IndexOf("Left") >= 0 || temp.IndexOf("left") >= 0) {

                            p[2].X = x;
                            p[2].Y = y;
                        }

                        if (temp.IndexOf("Right") >= 0 || temp.IndexOf("right") >= 0) {

                            p[3].X = x;
                            p[3].Y = y;
                        }
                    }

                }
            }

            if (rg == 0) {
                movCharaPos(p[0].X, p[0].Y + 30, 0, raddr);
                Thread.Sleep(100);
                movCharaPos(p[0].X, p[0].Y + 15, 0, raddr);
            }

            if (rg == 1) {
                movCharaPos(p[1].X, p[1].Y - 30, 0, raddr);
                Thread.Sleep(100);
                movCharaPos(p[1].X, p[1].Y - 15, 0, raddr);
            }

            if (rg == 2) {
                movCharaPos(p[2].X + 180, p[2].Y - 30, 0, raddr);
                Thread.Sleep(100);
                movCharaPos(p[2].X + 80, p[2].Y - 30, 0, raddr);
            }

            if (rg == 3) {
                movCharaPos(p[3].X - 180, p[3].Y + 30, 0, raddr);
                Thread.Sleep(100);
                movCharaPos(p[3].X - 80, p[3].Y + 30, 0, raddr);
            }


        }

        public void KeyPress_NoUse(Int32 VK_CODE, Int32 pressTime = 0) {
            if (pressTime == 0)
                pressTime = 50;

            Int32 keyPara = -1;

            switch (VK_CODE) {
                //==========F1~F12==============
                case (Int32)Keys.F1:
                    keyPara = 328;
                    break;
                case (Int32)Keys.CapsLock:
                    keyPara = 0x247;
                    break;

                case (Int32)Keys.F2:
                    keyPara = 329;
                    break;
                case (Int32)Keys.F3:
                    keyPara = 330;
                    break;
                case (Int32)Keys.F4:
                    keyPara = 331;
                    break;
                case (Int32)Keys.F5:
                    keyPara = 332;
                    break;
                case (Int32)Keys.F6:
                    keyPara = 333;
                    break;
                case (Int32)Keys.F7:
                    keyPara = 334;
                    break;
                case (Int32)Keys.F8:
                    keyPara = 335;
                    break;
                case (Int32)Keys.F9:
                    keyPara = 336;
                    break;
                case (Int32)Keys.F10:
                    keyPara = 337;
                    break;
                case (Int32)Keys.F11:
                    keyPara = 338;
                    break;
                case (Int32)Keys.F12:
                    keyPara = 339;
                    break;
                //===========0~9==============
                case '0':
                    keyPara = 280;
                    break;
                case '1':
                    keyPara = 271;
                    break;
                case '2':
                    keyPara = 272;
                    break;
                case '3':
                    keyPara = 273;
                    break;
                case '4':
                    keyPara = 274;
                    break;
                case '5':
                    keyPara = 275;
                    break;
                case '6':
                    keyPara = 276;
                    break;
                case '7':
                    keyPara = 277;
                    break;
                case '8':
                    keyPara = 278;
                    break;
                case '9':
                    keyPara = 279;
                    break;
                //===========Q~P==============
                case 'Q':
                    keyPara = 285;
                    break;
                case 'W':
                    keyPara = 286;
                    break;
                case 'E':
                    keyPara = 287;
                    break;
                case 'R':
                    keyPara = 288;
                    break;
                case 'T':
                    keyPara = 289;
                    break;
                case 'Y':
                    keyPara = 290;
                    break;
                case 'U':
                    keyPara = 291;
                    break;
                case 'I':
                    keyPara = 292;
                    break;
                case 'O':
                    keyPara = 293;
                    break;
                case 'P':
                    keyPara = 294;
                    break;
                //===========A~L==============
                case 'A':
                    keyPara = 299;
                    break;
                case 'S':
                    keyPara = 300;
                    break;
                case 'D':
                    keyPara = 301;
                    break;
                case 'F':
                    keyPara = 302;
                    break;
                case 'G':
                    keyPara = 303;
                    break;
                case 'H':
                    keyPara = 304;
                    break;
                case 'J':
                    keyPara = 305;
                    break;
                case 'K':
                    keyPara = 306;
                    break;
                case 'L':
                    keyPara = 307;
                    break;
                //===========Z~M==============
                case 'Z':
                    keyPara = 313;
                    break;
                case 'X':
                    keyPara = 314;
                    break;
                case 'C':
                    keyPara = 315;
                    break;
                case 'V':
                    keyPara = 316;
                    break;
                case 'B':
                    keyPara = 317;
                    break;
                case 'N':
                    keyPara = 318;
                    break;
                case 'M':
                    keyPara = 319;
                    break;
                //========小键盘0~9===========
                case 96:
                    keyPara = 348;
                    break;
                case 97:
                    keyPara = 349;
                    break;
                case 98:
                    keyPara = 350;
                    break;
                case 99:
                    keyPara = 344;
                    break;
                case 100:
                    keyPara = 345;
                    break;
                case 101:
                    keyPara = 346;
                    break;
                case 102:
                    keyPara = 340;
                    break;
                case 103:
                    keyPara = 341;
                    break;
                case 104:
                    keyPara = 342;
                    break;
                case 105:
                    keyPara = 351;
                    break;

                case (int)Keys.Tab:
                    keyPara = 0x11C;
                    break;
                //=====↑↓←→space alt=====
                case 38:
                    keyPara = 469;
                    break;
                case 40:
                    keyPara = 477;
                    break;
                case 37:
                    keyPara = 472;
                    break;
                case 39:
                    keyPara = 474;
                    break;
                case (Int32)Keys.Space:
                    keyPara = 326;
                    break;
                case 18:
                    keyPara = 325;
                    break;
                case (Int32)Keys.Enter:
                    keyPara = 0x229;
                    break;
                case (Int32)Keys.Escape:
                    keyPara = 0x21B;
                    break;
            }

            gMrw.writeInt8(gMrw.readInt32(baseAddr.dwBase_KeyPress) + keyPara, 0x80);
            Thread.Sleep(10);
            gMrw.writeInt8(gMrw.readInt32(baseAddr.dwBase_KeyPress) + keyPara, 0x0);

        }

        public bool IsHaveQuest(int id)
        {
            Int32 qAddr;
            Int32 fAddr = 0;
            Int32 lAddr = 0;

            qAddr = gMrw.readInt32(baseAddr.dwBase_Quest);
            writeLogLine(qAddr.ToString());
            fAddr = gMrw.readInt32(qAddr + 0x8);
            lAddr = gMrw.readInt32(qAddr + 0xC);

            for (int i = fAddr; i < lAddr; i += 0xC)
            {
                Int32 destAddr = gMrw.readInt32(i);
                int _id = gMrw.readInt32(destAddr);
                if (_id == id)
                    return true;
            }
            return false;
        }

        public int GetQuestCount(int id)
        {
            Int32 qAddr;
            Int32 fAddr = 0;
            Int32 lAddr = 0;

            qAddr = gMrw.readInt32(baseAddr.dwBase_Quest);
            writeLogLine(qAddr.ToString());
            fAddr = gMrw.readInt32(qAddr + 0x8);
            lAddr = gMrw.readInt32(qAddr + 0xC);

            for (int i = fAddr; i < lAddr; i += 0xC)
            {
                Int32 destAddr = gMrw.readInt32(i);
                int _id = gMrw.readInt32(destAddr);
                if (_id == id)
                    return gMrw.Decryption(i + 4);
            }
            return 0;
        }

        public void __SendKilled(Int32 id,Int32 code)
        {
            Int32 atk;
            if (Config.codeHashHP.ContainsKey(code))
                atk = (Int32)Config.codeHashHP[code] + new Random(Win32.Kernel.GetTickCount()).Next(100000, 1000000) + new Random(Win32.Kernel.GetTickCount()).Next(100000, 1000000);
            else
                atk = gMrw.readInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0x3AE4) * 100 + new Random(Win32.Kernel.GetTickCount()).Next(100000, 1000000);

            SendGameDataStart(39);
            SendGameDataAddInt32(id);
            SendGameDataAddInt16(gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Character) + 0xAC));
            SendGameDataAddInt64(0, 0);
            SendGameDataAddInt32(0);
            SendGameDataAddInt64(0, atk);
            SendGameDataAddInt32(6179);
            SendGameDataAddInt8(0);
            //SendGameDataAddInt16(273);
            //SendGameDataAddInt16(gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Character) + 0xAC));
            //SendGameDataAddInt32(atk - new Random(Win32.Kernel.GetTickCount()).Next(1000, 10000));
            //SendGameDataAddInt16(1);
            SendGameDataAddInt16(getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).y + new Random(Win32.Kernel.GetTickCount()).Next(-100, 100));
            SendGameDataAddInt16(getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).y + new Random(Win32.Kernel.GetTickCount()).Next(-100, 100));
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt16(16506);
            SendGameDataAddInt8(0);
            SendGameDataAddInt32(88);
            SendGameDataAddInt64(0, Config.codeHashHP.ContainsKey(code) ? (Int32)Config.codeHashHP[code] : atk);
            SendGameDataAddInt16( getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).x);
            SendGameDataAddInt16( getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).y);
            SendGameDataAddInt16( getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).x + new Random(Win32.Kernel.GetTickCount()).Next(-100, 100));
            SendGameDataAddInt16( getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).y + new Random(Win32.Kernel.GetTickCount()).Next(-100, 100));
            SendGameDataAddInt16(DateTime.Now.Hour);
            SendGameDataAddInt16(DateTime.Now.Minute);
            SendGameDataAddInt16(DateTime.Now.Second);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataEnd();
        }
        public void _SendKilled(Int32 addr) {

            Int32 atk = gMrw.readInt32(addr + 0x3AE4) + new Random(Win32.Kernel.GetTickCount()).Next(100000, 1000000);

            SendGameDataStart(39);
            SendGameDataAddInt32(gMrw.Decryption(addr + 0xAC));
            SendGameDataAddInt16(gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Character) + 0xAC));
            SendGameDataAddInt64(0, 0);
            SendGameDataAddInt32(0);
            SendGameDataAddInt64(0, atk);
            SendGameDataAddInt32(6179);
            SendGameDataAddInt8(0);
            //SendGameDataAddInt16(273);
            //SendGameDataAddInt16(gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Character) + 0xAC));
            //SendGameDataAddInt32(atk - new Random(Win32.Kernel.GetTickCount()).Next(1000, 10000));
            //SendGameDataAddInt16(1);
            SendGameDataAddInt16(getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).y + new Random(Win32.Kernel.GetTickCount()).Next(-100, 100));
            SendGameDataAddInt16(getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).y + new Random(Win32.Kernel.GetTickCount()).Next(-100, 100));
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataAddInt16(16506);
            SendGameDataAddInt8(0);
            SendGameDataAddInt32(88);
            SendGameDataAddInt64(0, gMrw.readInt32(addr + 0x3AE4));
            SendGameDataAddInt16(getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).x);
            SendGameDataAddInt16(getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).y);
            SendGameDataAddInt16((Int32)getObjPos(addr).x);
            SendGameDataAddInt16((Int32)getObjPos(addr).y);
            SendGameDataAddInt16(DateTime.Now.Hour);
            SendGameDataAddInt16(DateTime.Now.Minute);
            SendGameDataAddInt16(DateTime.Now.Second);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(0);
            SendGameDataEnd();

            //SendGameDataStart(39);
            //SendGameDataAddInt32(gMrw.Decryption(addr + 0xAC));
            //SendGameDataAddInt16(gMrw.Decryption(gMrw.readInt32(baseAddr.dwBase_Character) + 0xAC));
            //SendGameDataAddInt32(4129);
            //SendGameDataAddInt32(1);
            //SendGameDataAddInt32(atk);
            //SendGameDataAddInt32(4692);
            //SendGameDataAddInt8(0);
            //SendGameDataAddInt16(273);
            //SendGameDataAddInt16(297);
            //SendGameDataAddInt8(0);
            //SendGameDataAddInt8(0);
            //SendGameDataAddInt8(0);
            //SendGameDataAddInt8(0);
            //SendGameDataAddInt16(28479);
            //SendGameDataAddInt8(1);
            //SendGameDataAddInt32(88);
            //SendGameDataAddInt64(0, atk);
            //SendGameDataAddInt16((Int32)getObjPos(addr).x);
            //SendGameDataAddInt16((Int32)getObjPos(addr).y);
            //SendGameDataAddInt16(0);
            //SendGameDataAddInt16(0);
            //SendGameDataAddInt16(0);
            //SendGameDataAddInt16(0);
            //SendGameDataAddInt16(0);
            //SendGameDataAddInt8(0);
            //SendGameDataAddInt8(0);
            //SendGameDataEnd();
        }
        public void SendOpenPackage(Int32 iPos) {
            SendGameDataStart(0xA0);
            SendGameDataAddInt16(iPos);
            SendGameDataEnd();
        }
        public void SendOpenJar(Int32 iPos)
        {
            SendGameDataStart(27);
            SendGameDataAddInt16(0);
            SendGameDataAddInt16(iPos);
            SendGameDataEnd();
        }

        public void SendBuyGrope() {
            SendGameDataStart(0x300);
            SendGameDataAddInt32(gMrw.readInt32(baseAddr.dwBase_Character));
            SendGameDataAddInt32(15);
            SendGameDataEnd();
        }


        //        包头id:0x2ef,返回地址:0x123c567
        //      Int32:1
        //      Int32:490008381
        //      Int16:66
        //      Int32:1
        //包长:14,包尾返回地址:0x123c5b6
        //------包结构结束-------
        public void SendJigsaw(int jigsaw_pos,int bag_pos,int code,int num)
        {
            SendGameDataStart(0x2ef);
            SendGameDataAddInt32(jigsaw_pos);
            SendGameDataAddInt32(code);
            SendGameDataAddInt16(bag_pos);
            SendGameDataAddInt32(num);
            SendGameDataEnd();
        }
        public void SendArrangeBag() {
            SendGameDataStart(20);
            SendGameDataAddInt8(0);
            SendGameDataAddInt8(1);
            SendGameDataAddInt8(0);
            SendGameDataEnd();
        }
        public void TzYc() {
            Int32 ePoint = CreateEquit(20145);

            Int32 head = gMrw.readInt32(ePoint + 0x10E4);
            Int32 cPoint = gMrw.readInt32(baseAddr.dwBase_Character);


            head = gMrw.readInt32(head + 0x1C, 0x50, 0x4E8);

            gMrw.writeInt32(gMrw.readInt32(head + 0x4, 0x18) + 0, 31);//触发
            gMrw.writeInt32(gMrw.readInt32(head + 0x4, 0x4) + 4, 0);//触发方式

            gMrw.writeFloat(gMrw.readInt32(head + 0x18, 0x4, 0x4) + 4, (float)4);//概率
            gMrw.writeFloat(gMrw.readInt32(head + 0x18, 0x4, 0x2C) + 4, (float)100);//概率



            for (Int32 i = 0x2CE4; i <= 0x2CEC; i += 4) {
                if (gMrw.readInt32(cPoint + i) == 0) {
                    MessageBox.Show("请穿戴五件装备");
                    return;
                }
                gMrw.Encryption(gMrw.readInt32(cPoint + i) + 0x10F8, 11171);
                gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0x1108, 1);
            }
        }
        public void WareCallEx() {
            Int32 addr = CreateEquit(26564);
            gMrw.writeInt32(gMrw.readInt32(addr + 0xB5C, 4, 4) + 4, 500);//冷却  写0不稳定
            gMrw.writeInt32(gMrw.readInt32(addr + 0xB5C, 4, 0x18) + 0, 31);

            gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x4) + 4, (float)4);//范围
            gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x40) + 4, (float)2);//异常
            gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x40) + 0xC, (float)999);//伤害
            gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x40) + 8, (float)60);//等级
            gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x2C) + 4, (float)99);//几率

            WareCall(addr);
        }
        public void WareCallForSkill() {
            Int32 addr = CreateEquit(100352616);

            gMrw.writeInt32(gMrw.readInt32(addr + 0xB5C, 4, 4) + 4, 1000);//冷却  写0不稳定
            gMrw.writeInt32(gMrw.readInt32(addr + 0xB5C, 4, 4) + 8, 0);//冷却  写0不稳定

            gMrw.writeInt32(gMrw.readInt32(addr + 0xB5C, 4, 0x18) + 0, 31);

            gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x18) + 4, (float)99);//几率
            gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x4) + 4, (float)4);//几率

            //gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x40) + 4, (float)2);//异常
            //gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x40) + 0xC, (float)999);//伤害
            //gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x40) + 8, (float)60);//等级
            //gMrw.writeFloat(gMrw.readInt32(addr + 0xB5C, 0x18, 0x4, 0x2C) + 4, (float)99);//几率

            WareCall(addr);
        }
        public void SendKilled() {
            Int32 map = gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 End = gMrw.readInt32(map + 0xC4);
            Int32 num = 0;

            Int32[] temp = new Int32[100];

            for (Int32 i = gMrw.readInt32(map + 0xC0); i < End; i += 4) {
                Int32 addr = gMrw.readInt32(i);
                Int32 camp = gMrw.readInt32(addr + 0x828);
                Int32 type = gMrw.readInt32(addr + 0xA4);

                if (camp != 0) {
                    if (type == 529 || type == 545 || type == 273) {
                        temp[num++] = addr;
                    }
                }
            }
            for (Int32 i = num - 1; i >= 0; i--) {
                //__SendKilled(gMrw.Decryption(temp[i] + 0xAC));
                _SendKilled(temp[i]);

            }
        }

        public void EncryptionKilled() {
            Int32 map = gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 End = gMrw.readInt32(map + 0xC4);
            Int32 num = 0;

            Int32[] temp = new Int32[100];

            for (Int32 i = gMrw.readInt32(map + 0xC0); i < End; i += 4) {
                Int32 addr = gMrw.readInt32(i);
                Int32 camp = gMrw.readInt32(addr + 0x828);
                Int32 type = gMrw.readInt32(addr + 0xA4);
                Int32 x = (Int32) getObjPos(addr).x;

                if (camp != 0) {
                    if (type == 529 || type == 545 || type == 273) {
                        if (x == 801)
                            temp[num++] = addr;
                    }
                }
            }
            for (Int32 i = num - 1; i >= 0; i--) {
                EncryptionCall(temp[i] + 0x317C, 1);
            }
        }

        public void ItemCall(Int32 addr, Int32 code) {
            Int32 call_addr = gMrw.readInt32(baseAddr.dwBase_Character, 0, 0x7EC);
            at.clear();
            at.checkRetnAddress();
            at.pushad();
            at.mov_ecx(addr);
            at.push(code);
            at.mov_eax(call_addr);
            at.call_eax();
            at.popad();
            at.retn();

            at.RunRemoteThread();
        }

        public void UseItem(Int32 iPos) {
            gMrw.writeInt8(gMrw.readInt32(baseAddr.dwBase_Shop, 0x4C) + 0x400 + iPos, 1);
        }
        public void Encryption(Int32 data, Int32 addr) {


        }
        public void SendEnterJxInstance() {
            SendGameDataStart(0x2B6);
            SendGameDataAddInt32(25);
            SendGameDataAddInt32(14116);
            SendGameDataEnd();
        }
        public Int32 GetPL() {
            return gMrw.Decryption(baseAddr.dwBase_Max_Pl) - gMrw.Decryption(baseAddr.dwBase_Cur_Pl);
        }
        public void ChooseCard(int x, int y) {
            SendGameDataStart(0x45);
            SendGameDataEnd();
            SendGameDataStart(0x46);
            SendGameDataEnd();
            SendGameDataStart(0x47);
            SendGameDataAddInt8(x);
            SendGameDataAddInt8(y);
            SendGameDataEnd();
        }

        public void GetCardTrue() {
            SendGameDataStart(1433);
            SendGameDataEnd();
        }

        public void GiveUpQuest(Int32 id) {
            SendGameDataStart(32);
            SendGameDataAddInt16(32);
            SendGameDataAddInt16(id);
            SendGameDataEnd();
        }

        public Pos getObjPos(int addr)
        {
            Pos p;
            if (gMrw.readInt32(addr + 0xA4) == 273)
            {
                p.x = (int)gMrw.readFloat(gMrw.readInt32(addr + 0x1D4));
                p.y = (int)gMrw.readFloat(gMrw.readInt32(addr + 0x1D4) + 4);
                p.z = (int)gMrw.readFloat(gMrw.readInt32(addr + 0x1D4) + 8);
            }
            else
            {
                p.x = (int)gMrw.readFloat(gMrw.readInt32(addr + 0xB8) + 0x10);
                p.y = (int)gMrw.readFloat(gMrw.readInt32(addr + 0xB8) + 0x14);
                p.z = (int)gMrw.readFloat(gMrw.readInt32(addr + 0xB8) + 0x18);
            }
            return p;
        }

        public void Wave() {

            Int32 ePoint = CreateEquit(100210107);

            Int32 head = gMrw.readInt32(ePoint + 0x10E4);
            Int32 cPoint = gMrw.readInt32(baseAddr.dwBase_Character);

            head = gMrw.readInt32(head + 0x1C, 0x80, 0x4E8);

            gMrw.writeInt32(gMrw.readInt32(head + 0x40, 0x18) + 0, 31);//触发
            gMrw.writeInt32(gMrw.readInt32(head + 0x40, 0x4) + 0, 0);//触发方式
            gMrw.writeInt32(gMrw.readInt32(head + 0x40, 0x2C) + 4, 100);//频率
            gMrw.writeFloat(gMrw.readInt32(head + 0x54, 0x4, 0x18) + 4, (float)100);//概率

            //gMrw.writeInt32(gMrw.readInt32(head + 4, 0x18) + 0, 31);//触发
            //gMrw.writeInt32(gMrw.readInt32(head + 0x40, 0x2C) + 4, 1);//频率


            for (Int32 i = 0x2CD0; i <= 0x2CE0; i += 4) {
                if (gMrw.readInt32(cPoint + i) == 0) {
                    MessageBox.Show("请穿戴五件装备");
                    return;
                }
                gMrw.Encryption(gMrw.readInt32(cPoint + i) + 0x10F8, 11597);
                gMrw.writeInt32(gMrw.readInt32(cPoint + i) + 0x1108, 1);
            }
        }

        public bool IsHaveTask(Int32 code)
        {
            Int32 quest = gMrw.readInt32(baseAddr.dwBase_Quest);
            Int32 desk = gMrw.readInt32(quest + 0x64 + 8);

            for (Int32 i = gMrw.readInt32(quest + 0x64 + 4); i < desk; i += 4)
            {
                Int32 onobj = gMrw.readInt32(i);
                Int32 lenth = gMrw.readInt32(onobj + 0x1C);

                if (gMrw.readInt32(onobj) == code)
                    return true;
            }
            return false;

        }

        public Int32 GetMainQuestBase() {
            Int32 quest = gMrw.readInt32(baseAddr.dwBase_Quest);
            Int32 desk = gMrw.readInt32(quest + 0x64 + 8);

            for (Int32 i = gMrw.readInt32(quest + 0x64 + 4); i < desk; i += 4) {
                Int32 onobj = gMrw.readInt32(i);
                Int32 lenth = gMrw.readInt32(onobj + 0x1C);

                if (gMrw.readInt32(onobj + 0x134) == 0)
                    return onobj;
            }


            quest = gMrw.readInt32(baseAddr.dwBase_Quest);
            desk = gMrw.readInt32(quest + 0xC);


            for (Int32 i = gMrw.readInt32(quest + 0x8); i < desk; i += 0xC) {
                Int32 onobj = gMrw.readInt32(i);
                Int32 lenth = gMrw.readInt32(onobj + 0x1C);

                if (gMrw.readInt32(onobj + 0x134) == 0) {
                    return onobj;
                }
            }

            return 0;
        }

        public Int32 GetAcceptMainQuestBase() {
            Int32 quest = gMrw.readInt32(baseAddr.dwBase_Quest);
            Int32 desk = gMrw.readInt32(quest + 0xC);

            for (Int32 i = gMrw.readInt32(quest + 0x8); i < desk; i += 0xC) {
                Int32 onobj = gMrw.readInt32(i);
                Int32 lenth = gMrw.readInt32(onobj + 0x1C);

                if (gMrw.readInt32(onobj + 0x134) == 0) {
                    return onobj;
                }
            }
            return 0;
        }

        public Int32 GetChildQuest() {
            Int32 quest = gMrw.readInt32(baseAddr.dwBase_Quest);
            Int32 desk = gMrw.readInt32(quest + 0xC);

            for (Int32 i = gMrw.readInt32(quest + 0x8); i < desk; i += 0xC) {
                Int32 onobj = gMrw.readInt32(i);

                if (gMrw.readInt32(onobj + 0x134) == 11) {
                    return onobj;
                }
            }
            return 0;
        }

        public Int32 GetAcceptMainQuestTwice() {
            Int32 quest = gMrw.readInt32(baseAddr.dwBase_Quest);
            Int32 desk = gMrw.readInt32(quest + 0xC);

            Int32 twice = 0;

            for (Int32 i = gMrw.readInt32(quest + 0x8); i < desk; i += 0xC) {
                Int32 onobj = gMrw.readInt32(i);
                Int32 lenth = gMrw.readInt32(onobj + 0x1C);

                if (gMrw.readInt32(onobj + 0x134) == 0) {
                    Int32 count = gMrw.Decryption(i + 4);
                    while (count >= 512) {
                        if (count % 512 > twice)
                            twice = count % 512;
                        count /= 512;
                    }
                    if (count > twice)
                        twice = count;
                    return twice;
                }
            }
            return 0;
        }

        public Int32 GiveUpMainQuest() {
            Int32 quest = gMrw.readInt32(baseAddr.dwBase_Quest);
            Int32 desk = gMrw.readInt32(quest + 0xC);

            for (Int32 i = gMrw.readInt32(quest + 8); i < desk; i += 0xC) {
                Int32 onobj = gMrw.readInt32(i);
                Int32 lenth = gMrw.readInt32(onobj + 0x1C);

                if (gMrw.readInt32(onobj + 0x134) == 0)
                    GiveUpQuest(gMrw.readInt32(onobj));
            }
            return -1;
        }

        public Point GetCharaCoordinate() {
            Point p = new Point();
            p.X =  getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).x;
            p.Y = getObjPos(gMrw.readInt32(baseAddr.dwBase_Character)).y;
            return p;
        }



        public string GetCharaName() {
            if (gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                return "";
            return gMrw.readString(gMrw.readInt32(baseAddr.dwBase_Character, 0x400));
        }

        //public void movToPos(Int32 targetX, Int32 targetY, Int32 firstDire)
        //{
        //    Point chrPos = GetCharaCoordinate();

        //    if (firstDire == 0)
        //    {
        //        if () ;
        //            MyKey.MKeyDown(0, MyKey.Chr(0x));
        //    }
        //}


        public Int32 CitySmartTp(Int32 level) {
            if (level < 17 && level >= 1)
                CityTp(38, 2, 850, 300);
            if (level >= 17 && level <= 23)
                CityTp(40, 3, 550, 200);
            if (level >= 24 && level <= 29)
                CityTp(40, 4, 300, 300);
            if (level >= 30 && level <= 35)
                CityTp(41, 2, 550, 300);
            if (level >= 36 && level <= 39)
                CityTp(42, 3, 600, 150);
            if (level >= 40 && level <= 45)
                CityTp(43, 1, 300, 450);




            if (level >= 55 && level <= 63)
                CityTp(6, 3, 900, 200);
            if (level >= 63 && level <= 69)
                CityTp(9, 1, 700, 200);
            if (level >= 70 && level <= 78)
                CityTp(12, 0, 200, 300);
            if (level >= 79 && level <= 84)
                CityTp(14, 2, 800, 300);
            if (level == 85)
                CityTp(22, 2, 800, 300);

            return 1;
        }
        public void Equip() {

        }
    }


    public class MyKey
    {
        [DllImport("KeyCall.dll", EntryPoint = "GetKeyDev", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int MGetKeyDev();
        [DllImport("KeyCall.dll", EntryPoint = "KeySendChar", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int KeySendChar(string AData);
        [DllImport("KeyCall.dll", EntryPoint = "MouseDown", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int MMouseDown(byte AKey);
        [DllImport("KeyCall.dll", EntryPoint = "MouseMove", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int MMouseMove(byte AKey, int x, int y);
        [DllImport("KeyCall.dll", EntryPoint = "MouseMoveTo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int MMouseMoveTo(byte AKey, int x, int y);
        [DllImport("KeyCall.dll", EntryPoint = "MouseClick", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int MMouseClick(byte AKey);
        [DllImport("KeyCall.dll", EntryPoint = "MouseDbClick", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int MMouseDbClick(byte AKey);
        [DllImport("KeyCall.dll", EntryPoint = "KeyDown", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int MKeyDown(byte AKey, string AData);
        [DllImport("KeyCall.dll", EntryPoint = "KeyDownEx", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int MKeyDownEx(string AData);
        [DllImport("KeyCall.dll", EntryPoint = "KeyUp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int MKeyUp();
        [DllImport("KeyCall.dll", EntryPoint = "KeyDownUp", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int MKeyDownUp(byte AKey, string AData);
        [DllImport("KeyCall.dll", EntryPoint = "KeyDownUpEx", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int MKeyDownUpEx(string AData);

        [DllImport("KeyCall.dll", EntryPoint = "SetWaitTick", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWaitTick(int AWaitTick, int AMoveValue, int AClickTick, int AInputTick);

        public static string Chr(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                throw new Exception("ASCII Code is not valid.");
            }
        }
    }

    public struct BagItem {
        public Int32 Count;
        public Int32 Point;
        public Int32 Pos;
        public string name;

    }

}