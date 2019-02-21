using CallTool;
using cs_dxf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cs_dxfAuto {
    static class KeyEvent {
        static public Function fun;
        static public MemRWer gMrw;
        static public Form1 fm1;

        static Int32 wd;

        static public void F1() {
            fun.pickUp1();
        }

        static public void F2() {
            fun.CompletingQuest(3868);
            //fm1.writeLogLine("物品代码:" + gMrw.readInt32(baseAddr.dwBase_Mouse, 0x20) + ",物品地址 ：0x" + Convert.ToString(gMrw.readInt32(baseAddr.dwBase_Mouse), 16));

        }

        static public void F3() {
            fun.SendKilled();
            //fun.checkEmery(110110);
            //Thread.Sleep(100);
            fm1.writeLogLine("杀死怪物");
        }

        static public void F4()
        {
            //else if (b == block{ out,in,in,in })
            //				cout << 490008370 << endl;
            string result = "else if (b == block{";
            string[] vs = fm1.getTextBox1Text().Split(' ');
            foreach (var s in vs)
            {
                switch (s)
                {
                    case "0":
                        {
                            result += "in,";
                            break;
                        }
                    case "1":
                        {
                            result += "none,";
                            break;
                        }
                    case "2":
                        {
                            result += "out,";
                            break;
                        }
                }
            }
            result = result.Substring(0,result.Length - 1);
            result += "})\r\n cout << "+ gMrw.readInt32(baseAddr.dwBase_Mouse, 0x20) + " << endl;";
            fm1.writeLogLine(result);

        }
        static public void F5() {
            fm1.writeLogLine(Convert.ToString(fun.LoadCall(baseAddr.GetIndexObj.Equip, gMrw.readString(gMrw.readInt32(fm1.GetEquipDirAddress_test(gMrw.readInt32(baseAddr.dwBase_Mouse, 0x20)) + 4))), 16));

            //fm1.writeLogLine("地图id:" + gMrw.readInt32(0x4BC9D0C).ToString());
            //fm1.writeLogLine("地图id:" + gMrw.readInt32(baseAddr.dwBase_Map_ID).ToString());
            //fm1.writeLogLine("Boss是否死亡:" + gMrw.readInt32(baseAddr.dwBase_SSS, 0xA4C).ToString());

            //fm1.writeLogLine("当前坐标:x=" + fun.GetCurrencyRoom().X + " y=" + fun.GetCurrencyRoom().Y);
        }
        static public void F8() {
            fun.resolveAll();
        }

        static public void Home() {
            Int32 IsOpen = gMrw.readInt32(baseAddr.dwBase_Character, 0x918);
            Int32 _base = gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq + 0x10);
            if (_base == 0) {
                MessageBox.Show("必须穿戴腰带");
                return;
            }

            if (IsOpen == 0) {
                fun.EncryptionCall(_base + 0x000009B0, 1000);
                fun.EncryptionCall(_base + 0x000009B8, 1000);
                fun.EncryptionCall(_base + 0x000009C0, 1000);
                wd = gMrw.readInt32(baseAddr.dwBase_Character, 0xA08);

                gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0x91C, 1);
                gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0xA08, 1);

            } else {
                fun.EncryptionCall(_base + 0x000009B0, 0);
                fun.EncryptionCall(_base + 0x000009B8, 0);
                fun.EncryptionCall(_base + 0x000009C0, 0);

                gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0x91C, 0);
                gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0xA08, wd);
            }
        }

        static public void PageUp() {
            fun.QuitInstance();
        }

        static public void End() {
            fun.QuitInstance();
        }


        static List<Int32> monsterCode = new List<Int32> { };
        static public void B() {
            if (gMrw.readInt32(baseAddr.dwBase_Map_ID) <= 0)
                return;

            int addr;
            int code = 3;
            if ((addr = fun.GetAddressByName("菲尼克斯")) == 0)
            {
                //int atk_addr = fun.LoadCall(baseAddr.GetIndexObj.Atk, "Creature/Phoenix/AttackInfo/skill.atk");
                ////int atk_addr = fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/Creature/zodiac/AttackInfo/magiccircle.atk");

                //int pet_addr = fun.LoadCall(baseAddr.GetIndexObj.Creature, "phoenix/phoenix.cre");
                //fun.EncryptionCall(pet_addr + 0x24, 1);
                //int int_data = gMrw.read<int>(pet_addr + 0x1F4);
                //fun.EncryptionCall(int_data + 0x10, 100);

                //fun.EncryptionCall(atk_addr + 0x20, (int)fm1.code_hurt);
                //fun.EncryptionCall(atk_addr + 0x28, 0);
                fun.createPet(gMrw.readInt32(baseAddr.dwBase_Character), code);
                while ((addr = fun.GetAddressByName("菲尼克斯")) == 0) Thread.Sleep(1);
                if (!Config.isHook)
                {
                    int atk_addr = gMrw.readInt32(addr + 0x7C4);
                    fun.EncryptionCall(atk_addr + 0x20, (int)fm1.code_hurt);
                    fun.EncryptionCall(atk_addr + 0x28, 0);
                }
                //int cd_addr = gMrw.readInt32(addr + 0xFB4);
                //fun.EncryptionCall(cd_addr, 1);
            }
            else
            {
                if (Config.isHook)
                {

                    if (gMrw.readInt32(addr) != fm1.at.GetVirtualAddr() + 0x3001)
                    {

                        Byte[] old = gMrw.readData(gMrw.read<uint>(addr) - 0x1000, 0x3000);
                        gMrw.writedData((uint)fm1.at.GetVirtualAddr() + 0x2001, old, 0x3000);
                        gMrw.writeInt32(fm1.at.GetVirtualAddr() + 0x3001 + 0x468, 0x02E1F9DC );//0314338C    B0 01           mov al,0x1

                        int atk_addr = fm1.fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/actionobject/monster/anton_normal/phase1/blacksmoke/dumy/attackinfo/dumy2_basic.atk");

                        if (fm1.gMrw.readInt32(baseAddr.dwBase_Character, 0, 0x36C) < fm1.at.GetVirtualAddr())
                        {
                            fm1.at.clear();
                            //fm1.at.mov_eax(atk_addr);
                            //fm1.at.retn(4);
                            fm1.at.mov_esp_ptr_addx(4, atk_addr);
                            fm1.at.push(fm1.gMrw.readInt32(addr, 0x36C));
                            fm1.at.retn();
                            int i = 0;
                            foreach (byte a in fm1.at.Code)
                            {
                                fm1.gMrw.writeInt8(fm1.at.GetVirtualAddr() + 0xC50 + i++, a);
                            }
                        }
                        fm1.at.setEvent();
                        fm1.gMrw.writeInt32(fm1.at.GetVirtualAddr() + 0x3001 + 0x36C, fm1.at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1
                        gMrw.writeInt32(addr, fm1.at.GetVirtualAddr() + 0x3001);
                    }
                }
                else
                    fun.SetCharaPos(addr);



                //fun.movCharaPos((int)gMrw.readFloat(
                //    gMrw.readInt32(baseAddr.dwBase_Character , baseAddr.dwOffset_Obj_Pos)),
                //    (int)gMrw.readFloat(gMrw.readInt32(baseAddr.dwBase_Character , baseAddr.dwOffset_Obj_Pos) + 4), 
                //    (int)gMrw.readFloat(gMrw.readInt32(baseAddr.dwBase_Character , baseAddr.dwOffset_Obj_Pos) + 8),
                //    addr);
                fun.petSkillCall(addr, 5);
            }
            //fun.xiguai();
        }

        static public void B1()
        {
            if (gMrw.readInt32(baseAddr.dwBase_Map_ID) <= 0)
                return;

            int addr;
            int code = 3;
            if ((addr = fun.GetAddressByCode(code, 129)) == 0)
            {
                //int atk_addr = fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/creature/phoenix/attackinfo/hellfire.atk");
                //int atk_addr = fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/Creature/zodiac/AttackInfo/magiccircle.atk");


                fun.createPet(gMrw.readInt32(baseAddr.dwBase_Character), code);
            }
            else
            {
                //fun.EncryptionCall(atk_addr + 0x20, (int)fm1.code_hurt);
                //fun.EncryptionCall(atk_addr + 0x28, 0);
                //fun.SetCharaPos(addr);
                //fun.movCharaPos((int)gMrw.readFloat(
                //    gMrw.readInt32(baseAddr.dwBase_Character , baseAddr.dwOffset_Obj_Pos)),
                //    (int)gMrw.readFloat(gMrw.readInt32(baseAddr.dwBase_Character , baseAddr.dwOffset_Obj_Pos) + 4), 
                //    (int)gMrw.readFloat(gMrw.readInt32(baseAddr.dwBase_Character , baseAddr.dwOffset_Obj_Pos) + 8),
                //    addr);
                //fun.petSkillCall(addr, 6);
            }
            //fun.xiguai();
        }

        static public void V() {
            //fun.PickUp();
        }

        static public void Up() {
            if (fm1.IsQzTp())
                fun.InstanceTp(0);
            else
                fun.CoorTp(0);
        } 
        static public void Down() {
            if (fm1.IsQzTp())
                fun.InstanceTp(1);
            else
                fun.CoorTp(1);
        }
        static public void Left() {
            if (fm1.IsQzTp())
                fun.InstanceTp(2);
            else
                fun.CoorTp(2);
        }
        static public void Right() {
            if (fm1.IsQzTp())
                fun.InstanceTp(3);
            else
                fun.CoorTp(3);
        }
    }
}
