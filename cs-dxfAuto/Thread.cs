using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cs_dxfAuto;
using cs_dxf;
using CallTool;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace cs_dxfAuto
{
    public class MainThread
    {

        [DllImport("user32")]
        static extern int SetForegroundWindow(IntPtr hwnd);

        Thread t_yw;
        Form1 fm1;

        public MainThread(Form1 fm)
        {
            fm1 = fm;
        }
        public bool IsRoomOpen()
        {
            return fm1.gMrw.Decryption(fm1.gMrw.readInt32(baseAddr.dwBase_Shop - 8, baseAddr.dwBase_Time, 0xC4) + 0x118) == 0;
        }
        //public Int32 GetCharaLevel() {
        //    return fm1.gMrw.Decryption(fm1.gMrw.readInt32(baseAddr.dwBase_Character) + 0xD68);
        //}
        public void PickUp()
        {
            string[] item = new string[8];

            int chr = fm1.gMrw.readInt32(baseAddr.dwBase_Character);
            Int32 ob = fm1.gMrw.readInt32(chr + 0xC8);
            Int32 dest = fm1.gMrw.readInt32(ob + 0xC4);

            for (int i = fm1.gMrw.readInt32(ob + 0xC0); i < dest; i += 4)
            {
                Int32 b = fm1.gMrw.readInt32(i);

                if (fm1.gMrw.readInt32(b + 0xA4) == 1057)
                {

                    //fm1.writeYwChLogLine(Convert.ToString(b, 16));
                    Int32 End = fm1.gMrw.readInt32(b + 0x161C);

                    for (Int32 m = fm1.gMrw.readInt32(b + 0x1618); m < End; m += 8)
                    {
                        Int32 iAddr = fm1.gMrw.readInt32(m + 4);
                        Int32 pName = fm1.gMrw.readInt32(iAddr + 0x24);//01D693F0    55              push ebp

                        string Name = fm1.gMrw.readString(pName);

                        if (Name.IndexOf("[") != -1)
                        {
                            Int32 destAddr = fm1.gMrw.readInt32(m);
                            fm1.fun._PickUp(destAddr);
                            fm1.writeYwChLogLine(Name);
                        }
                        else if (Name.IndexOf("锭") != -1 || Name.IndexOf("晶石袋") != -1 || Name.IndexOf("邀请函") != -1)
                        {
                            Int32 destAddr = fm1.gMrw.readInt32(m);
                            fm1.fun._PickUp(destAddr);
                            fm1.writeYwLsLogLine(Name);
                        }
                        else if (Name.IndexOf("佩鲁斯") != -1)
                        {
                            Int32 destAddr = fm1.gMrw.readInt32(m);
                            fm1.fun._PickUp(destAddr);
                            fm1.writeYwChLogLine(Name);
                        }
                        else if (Name.IndexOf("艾尔文") != -1)
                        {
                            Int32 destAddr = fm1.gMrw.readInt32(m);
                            fm1.fun._PickUp(destAddr);
                            fm1.writeYwChLogLine(Name);
                        }
                        else if (Name.IndexOf("珍珠") != -1)
                        {
                            Int32 destAddr = fm1.gMrw.readInt32(m);
                            fm1.fun._PickUp(destAddr);
                            fm1.writeYwChLogLine(Name);
                        }
                    }
                }
            }
        }
        bool nb_cz = false;
        public Int32 GetNextRoom(System.Drawing.Point p, Int32 mapid = 0)
        {
            if (mapid == 0)
                mapid = fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID);

            if (mapid == 100)
            {
                if (p.X == 4 && p.Y == 1)
                    return 2;
                if (p.X == 3 && p.Y == 1)
                    return 2;
                if (p.X == 2 && p.Y == 1 && nb_cz == false)
                    return 0;
                if (p.X == 2 && p.Y == 1 && nb_cz == true)
                    return 2;
                if (p.X == 2 && p.Y == 0)
                    return 1;
                if (p.X == 1 && p.Y == 1)
                    return 1;
                if (p.X == 1 && p.Y == 2)
                    return 2;
            }

            if (mapid == 141)
            {
                return 3;
            }

            if (mapid == 40)
            {
                if ((p.X == 0 || p.X == 1) && p.Y == 3)
                    return 3;
                if (p.X == 2 && p.Y == 3)
                    return 0;
                if (p.X == 2 && p.Y == 2)
                    return 2;
                if (p.X == 1 && (p.Y == 2 || p.Y == 1))
                    return 0;
                if (p.X == 1 && p.Y == 0)
                    return 2;
            }

            if (mapid == 192)
            {
                if (p.X == 0 && p.Y == 0)
                    return 3;
                if (p.X == 1 && p.Y == 0)
                    return 3;
                if (p.X == 2 && p.Y == 0)
                    return 1;
                if (p.X == 2 && p.Y == 1)
                    return 2;
                if (p.X == 1 && p.Y == 1)
                    return 2;
                if (p.X == 0 && p.Y == 1)
                    return 1;
                if (p.X == 0 && p.Y == 2)
                    return 1;
                if (p.X == 0 && p.Y == 3)
                    return 3;
                if (p.X == 1 && p.Y == 3)
                    return 0;
                if (p.X == 1 && p.Y == 2)
                    return 3;
                if (p.X == 2 && p.Y == 2)
                    return 3;
            }

            if (mapid == 8004)
            {
                if (p.X == 2 && p.Y == 1)
                    return 2;
                if (p.X == 1 && p.Y == 1)
                    return 0;
                if (p.X == 1 && p.Y == 0)
                    return 2;
            }

            if (mapid == 8011)
            {
                if (p.X == 1 && p.Y == 1)
                {
                    return 1;
                }
                else if (p.X == 1 && p.Y == 2)
                {
                    return 2;
                }
                else if (p.X == 0 && (p.Y == 2 || p.Y == 1))
                {
                    return 0;
                }
                else if ((p.X == 0 || p.X == 1) && p.Y == 0)
                {
                    return 3;
                }
                else if (p.X == 2 && p.Y == 0)
                {
                    return 1;
                }
                else if (p.X == 2 && p.Y == 1)
                {
                    return 3;
                }
                else if (p.X == 3 && (p.Y == 1 || p.Y == 2))
                {
                    return 1;
                }
                else if ((p.X == 3 || p.X == 2) && p.Y == 3)
                {
                    return 2;
                }
            }

            if (mapid == 92)
            {
                // 凛冬
                if (p.X == 1 && p.Y == 0)
                {
                    return 1;
                }
                else if (p.X == 1 && p.Y == 1)
                {
                    return 2;
                }
                else if (p.X == 0 && (p.Y == 1 || p.Y == 2 || p.Y == 3 || p.Y == 4))
                {
                    return 1;
                }
                else if ((p.X == 0 || p.X == 1) && p.Y == 5)
                {
                    return 3;
                }

            }


            if (mapid == 104)
            {
                // 凛冬
                if ((p.X == 0 || p.X == 1) && p.Y == 0)
                {
                    return 3;
                }
                else if (p.X == 2 && p.Y == 0)
                {
                    return 1;
                }
                else if (p.X == 2 && p.Y == 1)
                {
                    return 2;
                }
                else if (p.X == 1 && p.Y == 1)
                {
                    return 1;
                }
                else if ((p.X == 1 || p.X == 2) && p.Y == 2)
                {
                    return 3;
                }
                else if (p.X == 3 && p.Y == 2)
                {
                    return 0;
                }

            }

            return 0;
        }

        bool IsBossRoom()
        {
            if (fm1.fun.GetCurrencyRoom().X == fm1.fun.GetBossRoom().X && fm1.fun.GetCurrencyRoom().Y == fm1.fun.GetBossRoom().Y)
                return true;
            return false;
        }

        public Int32 GettHaerteNexRoom(System.Drawing.Point p, bool flag)
        {

            if (p.X == 0 && p.Y == 2)
                return 3;
            if (p.X == 1 && p.Y == 2 && flag == false)
                return 0;
            if (p.X == 1 && p.Y == 2 && flag == true)
                return 3;
            if (p.X == 1 && p.Y == 1)
                return 0;
            if (p.X == 1 && p.Y == 0)
                return 3;
            if (p.X == 2 && p.Y == 0)
                return 1;
            if (p.X == 2 && p.Y == 1)
                return 3;
            if (p.X == 2 && p.Y == 1)
                return 3;
            if (p.X == 3 && p.Y == 1)
                return 1;
            if (p.X == 3 && p.Y == 2)
                return 1;
            if (p.X == 3 && p.Y == 3)
                return 2;
            if (p.X == 2 && p.Y == 3)
                return 1;
            if (p.X == 2 && p.Y == 4)
                return 2;
            if (p.X == 1 && p.Y == 4)
                return 0;
            if (p.X == 1 && p.Y == 3)
                return 0;

            return 5;
        }
        public Int32 GetYwDir(System.Drawing.Point p)
        {

            if (yw_twice == 0)
            {
                if (p.X == 9 && p.Y == 6)
                    return 2;
                if (p.X == 8 && p.Y == 6)
                    return 1;
                if ((p.X == 8 || p.X == 7) && p.Y == 7)
                    return 2;
                if (p.X == 6 && p.Y == 7)
                    return 0;
                if ((p.X == 6 || p.X == 5 || p.X == 4 || p.X == 3 || p.X == 2) && p.Y == 6)
                    return 2;
            }

            return 0;

        }
        private void movMap()
        {
            Int32 chr = fm1.gMrw.readInt32(baseAddr.dwBase_Character);
            Int32 _object = fm1.gMrw.readInt32(chr + 0xC8);
            Int32 dest = fm1.gMrw.readInt32(_object + 0xC4);
            //Int32 x = fm1.gMrw.readInt32(chr + 0x1B0);
            //Int32 y = fm1.gMrw.readInt32(chr + 0x1B0);
            //Int32 z = fm1.gMrw.readInt32(chr + 0x1B4);
            byte[] data = new byte[12];
            //data = fm1.gMrw.readData((uint)(fm1.gMrw.readInt32(chr + baseAddr.dwOffset_Obj_Pos)), 12);


            for (int i = fm1.gMrw.readInt32(_object + 0xC0); i < dest; i += 4)
            {
                Int32 onobj = fm1.gMrw.readInt32(i);
                string name = fm1.gMrw.readString(fm1.gMrw.readInt32(onobj + 0x4F4));
                if (name.IndexOf("MoveMap") >= 0)
                    fm1.gMrw.writedData((uint)fm1.gMrw.readInt32(onobj + 0xAC) + 0x10, data, 12);

                //if (fm1.gMrw.readInt32(onobj + 0xA4) == 0x211)
                //{
                //    fm1.gMrw.writedData((uint)fm1.gMrw.readInt32(onobj + 0xAC) + 0x10, data, 12);
                //}
            }
        }
        int yw_twice = 0;
        public void yw_zb_thread()
        {
            Int32 CharaPos = fm1.gMrw.readInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Role) + 0x160);
            while (true)
            {
                Thread.Sleep(2000);
                fm1.setCharaInfomation();

                //if (fm1.IsAutoTz() && GetCharaLevel() >= 55) {
                //    fm1.writeLogLine("角色等级 : " + GetCharaLevel() + "，大于55 执行挑战");
                //    //JL();
                //    fm1.TzThread();
                //    fm1.writeLogLine("挑战执行完毕");
                //}

                //if (fm1.IsAutoSell()) {
                //    fm1.SellGoal();
                //}

                //fm1.fun.Freeze(5000, 1);
                Thread.Sleep(5000);
                fm1.fun.CityTp(2, 2, 505, 288);
                Thread.Sleep(3000);
                fm1.fun.ChooseInstance();
                Thread.Sleep(3000);
                fm1.fun.EnterInstance(4000, 0);
                Int32 time = 0;
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) < 0)
                {
                    Thread.Sleep(100);
                    if ((time += 100) > 8000)
                    {
                        fm1.writeLogLine("进入超时 goto end");
                        fm1.fun.QuitChooseInstance();
                        Thread.Sleep(1000);

                    }

                }


                Thread.Sleep(15000);
                Random r = new Random();
                System.Drawing.Point p = fm1.fun.GetCurrencyRoom();
                while (!(p.X == 2 && p.Y == 6))
                {
                    p = fm1.fun.GetCurrencyRoom();
                    //PickUp();
                    Thread.Sleep(r.Next(1500, 3500));
                    p = fm1.fun.GetCurrencyRoom();
                    fm1.fun.CoorTp(GetYwDir(p));
                    Thread.Sleep(r.Next(1000, 3500));

                }
                Thread.Sleep(1500);
                PickUp();
                fm1.fun.QuitInstanceNoMainThread();

                Thread.Sleep(1500);

                fm1.fun.ChooseChara();
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) > 0)
                    Thread.Sleep(100);
                Thread.Sleep(5000);
                fm1.fun.EnterChara(++CharaPos);
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                    Thread.Sleep(100);

            }
        }
        private Int32 GetYWNextRoom(System.Drawing.Point p, Int32 flag)
        {
            if (flag == 0)
            {
                if (p.X == 9 && p.Y == 6)
                    return 2;
                if (p.X == 8 && p.Y == 6)
                    return 1;
                if ((p.X == 8 || p.X == 7) && p.Y == 7)
                    return 2;
                if (p.X == 6 && p.Y == 7)
                    return 0;
                if ((p.X == 6 || p.X == 5 || p.X == 4 || p.X == 3) && p.Y == 6)
                    return 2;
                if (p.X == 2 && p.Y == 6)
                    return 5;
                if (p.X == 3 && p.Y == 9)
                    return 2;
            }
            if (flag == 1)
            {
                if (p.X == 2 && p.Y == 9)
                    return 3;
                if (p.X == 3 && p.Y == 9)
                    return 5;
                if (p.X == 2 && p.Y == 6)
                    return 3;
                if (p.X == 3 && (p.Y == 6 || p.Y == 7))
                    return 1;
                if (p.X == 3 && p.Y == 8)
                    return 2;
                if (p.X == 2 && p.Y == 8)
                    return 5;
                if (p.X == 5 && p.Y == 9)
                    return 3;
            }
            if (flag == 2)
            {
                if (p.X == 6 && p.Y == 9)
                    return 2;
                if (p.X == 5 && p.Y == 9)
                    return 5;
                if ((p.X == 2 || p.X == 3 || p.X == 4 || p.X == 5) && p.Y == 8)
                    return 3;
                if (p.X == 6 && (p.Y == 8 || p.Y == 7))
                    return 0;
                if (p.X == 6 && p.Y == 6)
                    return 2;
                if (p.X == 5 && p.Y == 6)
                    return 0;
                if (p.X == 5 && p.Y == 5)
                    return 2;
                if (p.X == 4 && (p.Y == 5 || p.Y == 4 || p.Y == 3 || p.Y == 2 || p.Y == 1))
                    return 0;
                if ((p.X == 4 || p.X == 3 || p.X == 2 || p.X == 1) && p.Y == 0)
                    return 2;
                if (p.X == 0 && p.Y == 0)
                    return 1;
                if (p.X == 0 && p.Y == 1)
                    return 3;
            }
            if (flag == 3)
            {

                if (p.X == 4 && (p.Y == 0 || p.Y == 4 || p.Y == 3 || p.Y == 2 || p.Y == 1))
                    return 1;
                if ((p.X == 0 || p.X == 3 || p.X == 2 || p.X == 1) && p.Y == 0)
                    return 3;
                if (p.X == 0 && p.Y == 1)
                    return 0;
                if (p.X == 1 && p.Y == 1)
                    return 2;
                if ((p.X == 4 || p.X == 5) && p.Y == 5)
                    return 3;
                if (p.X == 6 && p.Y == 5)
                    return 0;
                if ((p.X == 6 || p.X == 7) && p.Y == 4)
                    return 3;
                if (p.X == 8 && (p.Y == 4 || p.Y == 3 || p.Y == 2))
                    return 0;
                if (p.X == 8 && p.Y == 1)
                    return 2;
                if (p.X == 7 && p.Y == 1)
                    return 2;
                if (p.X == 6 && p.Y == 1)
                    return 1;
                if (p.X == 6 && p.Y == 2)
                    return 5;
                if (p.X == 7 && p.Y == 5)
                    return 6;
            }

            return -1;
        }
        public void ywThread()
        {
            Int32 CharaPos = fm1.gMrw.readInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Role) + 0x160);
            while (true)
            {
                Thread.Sleep(2000);

                //if (fm1.IsAutoTz() && GetCharaLevel() >= 95) {
                //    fm1.writeLogLine("角色等级 : " + GetCharaLevel() + "，大于55 执行挑战");
                //    //JL();
                //    fm1.TzThread();
                //    fm1.writeLogLine("挑战执行完毕");
                //}


                fm1.fun.CityTp(40, 0, 780, 270);

                //if (CharaPos != 0 && (CharaPos % 6 == 0 || CharaPos % 4 == 0))
                //    Thread.Sleep(100 * 60 * 25);

                Thread.Sleep(3000);
                fm1.fun.ChooseInstance();
                Thread.Sleep(3000);
                fm1.fun.EnterInstance(4000, 0);
                Int32 time = 0;
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) < 0)
                {
                    Thread.Sleep(100);
                    if ((time += 100) > 8000)
                    {
                        fm1.writeLogLine("进入超时 goto end");
                        fm1.fun.QuitChooseInstance();
                        Thread.Sleep(1000);
                        goto end;
                    }
                }


                if (!fm1.IsQzTp())
                {
                    Thread.Sleep(5000);
                    fm1.fun.InstanceTp(8, 6);
                    fm1.fun.InstanceTp(8, 5);
                    fm1.fun.InstanceTp(7, 5); //雪山隐藏BOSS
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(7, 4);
                    fm1.fun.InstanceTp(6, 4);
                    fm1.fun.InstanceTp(5, 4); //雪山隐藏房间
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(6, 4);
                    fm1.fun.InstanceTp(6, 3);//雪山隐藏房间
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(6, 2);
                    fm1.fun.InstanceTp(5, 2);//雪山篝火箱子
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(6, 2);
                    fm1.fun.InstanceTp(6, 1);//雪山上BOSS
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(7, 1);
                    fm1.fun.InstanceTp(8, 1);
                    fm1.fun.InstanceTp(9, 1);//雪山隐藏箱子
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(8, 1);
                    fm1.fun.InstanceTp(8, 2);
                    fm1.fun.InstanceTp(8, 3);
                    fm1.fun.InstanceTp(8, 4);
                    fm1.fun.InstanceTp(9, 4);//雪山隐藏箱子
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(8, 4);
                    fm1.fun.InstanceTp(8, 5);
                    fm1.fun.InstanceTp(8, 6);
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(7, 6);
                    fm1.fun.InstanceTp(6, 6);
                    fm1.fun.InstanceTp(5, 6);
                    fm1.fun.InstanceTp(4, 6);
                    fm1.fun.InstanceTp(3, 6);
                    fm1.fun.InstanceTp(2, 6);
                    fm1.fun.InstanceTp(1, 6);
                    fm1.fun.InstanceTp(1, 5);
                    fm1.fun.InstanceTp(1, 4);
                    fm1.fun.InstanceTp(1, 3);
                    fm1.fun.InstanceTp(1, 2);
                    //	 Sleep(2000);
                    fm1.fun.InstanceTp(0, 2);//隐藏房间4个箱子
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(1, 2);
                    fm1.fun.InstanceTp(1, 1);//第二个boss魔女
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(2, 1);
                    fm1.fun.InstanceTp(3, 1);//魔女中间箱子
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(2, 1);
                    fm1.fun.InstanceTp(1, 1);
                    fm1.fun.InstanceTp(0, 1);//魔女左边箱子1个
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(0, 0);//魔女左上边箱子1个
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(0, 1);
                    fm1.fun.InstanceTp(1, 1);
                    fm1.fun.InstanceTp(1, 2);//第三个boss博士
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(1, 3);
                    fm1.fun.InstanceTp(1, 4);
                    fm1.fun.InstanceTp(1, 5);
                    fm1.fun.InstanceTp(1, 6);
                    fm1.fun.InstanceTp(1, 7);
                    fm1.fun.InstanceTp(2, 7);
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(2, 8);
                    fm1.fun.InstanceTp(2, 9);//第四个boss哥布林
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(3, 9);//哥布林右边2箱子
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(2, 9);
                    fm1.fun.InstanceTp(1, 9);
                    fm1.fun.InstanceTp(1, 8);
                    fm1.fun.InstanceTp(1, 7);
                    fm1.fun.InstanceTp(1, 6);
                    fm1.fun.InstanceTp(1, 5);
                    fm1.fun.InstanceTp(2, 5);
                    fm1.fun.InstanceTp(3, 5);
                    fm1.fun.InstanceTp(4, 5);
                    fm1.fun.InstanceTp(5, 5);
                    fm1.fun.InstanceTp(6, 5);
                    fm1.fun.InstanceTp(7, 5);
                    fm1.fun.InstanceTp(8, 5);
                    fm1.fun.InstanceTp(8, 6);
                    fm1.fun.InstanceTp(8, 7);
                    fm1.fun.InstanceTp(8, 8);
                    fm1.fun.InstanceTp(8, 9);
                    fm1.fun.InstanceTp(7, 9);//天空之海2个箱子
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(7, 8);//第五个boss天空之海
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(7, 9);
                    fm1.fun.InstanceTp(6, 9);//第六个boss牛头
                    Thread.Sleep(1500);
                    PickUp();
                    fm1.fun.InstanceTp(5, 9);//第六个boss牛头
                    Thread.Sleep(1500);
                    PickUp();
                }
                else
                {
                    Thread.Sleep(5000);

                    fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character) + 0xA04, 1);
                    int flag = 0;
                    while (true)
                    {
                        System.Drawing.Point p = fm1.fun.GetCurrencyRoom();
                        if (p.X == 2 && p.Y == 9)
                            flag = 1;
                        if (p.X == 6 && p.Y == 9)
                            flag = 2;
                        if (p.X == 1 && p.Y == 1)
                            flag = 3;
                        Int32 result = GetYWNextRoom(p, flag);

                        PickUp();
                        if (result >= 0 && result <= 3)
                        {
                            if (fm1.IsStopWithGobarli())
                                fm1.fun.checkEmery(60025);
                            fm1.fun.CoorTp(result);
                            Thread.Sleep(1000);
                        }
                        if (result == 5)
                        {
                            if (fm1.IsStopWithGobarli())
                                fm1.fun.checkEmery(60025);
                            fm1.fun.SetCharaMoveMap();
                            Thread.Sleep(2000);
                        }
                        if (result == 6)
                            break;
                        if (result == -1)
                        {
                            fm1.writeLogLine("坐标出错，出错坐标 ： x=" + p.X + " y=" + p.Y);
                            break;
                        }
                        Thread.Sleep(100);
                    }
                }
                fm1.fun.QuitInstanceNoMainThread();
                end:
                //fm1.fun.CityTp(2, 7, 300, 300);

                Thread.Sleep(1000);
                //fm1.fun.ChooseInstance();
                //fm1.fun.EnterInstance(151, 2);

                //while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) <= 0)
                //    Thread.Sleep(100);

                //fm1.fun.QuitInstance();
                //Thread.Sleep(5000);

                fm1.fun.ChooseChara();
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) > 0)
                    Thread.Sleep(100);
                fm1.fun.EnterChara(++CharaPos);
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                    Thread.Sleep(100); ;
            }
        }
        Int32 LevelTwice = 0, TickTime = 0;
        public bool IsSpacialRoom(Int32 mapid, Int32 Questid, System.Drawing.Point p)
        {

            if (mapid == 70 && Questid == 2 && p.X == 2 && p.Y == 1)
                return true;
            else if (mapid == 88 && Questid == 1 && p.X == 0 && p.Y == 1)
                return true;
            else if (mapid == 80 && Questid == 1 && p.X == 2 && p.Y == 0)
                return true;
            else if (mapid == 81 && Questid == 2 && p.X == 2 && p.Y == 0)
                return true;
            else if (mapid == 76 && Questid == 4 && p.X == 2 && p.Y == 0)
                return true;
            else if (mapid == 35 && Questid == 3 && p.X == 0 && p.Y == 1)
                return true;
            else if (mapid == 235 && p.X == 1 && p.Y == 1)
                return true;
            else if (mapid == 7123)
                return true;
            return false;
        }
        public void AutoLevel()
        {
            Int32 CharaPos = fm1.gMrw.readInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Role) + 0x160);
            bool IsLoad = false;
            if (fm1.GetClearMapFunction() == 4)
                fm1.checkCode();

            while (true)
            {
                //fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character) + 0x918, 1);
                //fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character) + 0xA04, 1);


                while (fm1.fun.GetPL() > 0 && ((fm1.getTargetLevel() == -1 ? 9999 : fm1.getTargetLevel()) > fm1.fun.GetCharaLevel()))
                {
                    Thread.Sleep(2000);
                    //获取当前角色的区域
                    Int32 charaLevel = fm1.fun.GetCharaLevel();
                    MapInfomation currentRagion = new MapInfomation();

                    foreach (MapInfomation m in MapInfo.mapInfo)
                    {
                        if (m.minLevel <= charaLevel && m.maxLevel >= charaLevel)
                        {
                            currentRagion = m;
                            break;
                        }
                    }

                    Int32 mapid = 0, Level = 0, questid = 0;

                    if (fm1.fun.GetMainQuestBase() != 0)
                    {

                        Int32 QuestLevel = fm1.gMrw.readInt32(fm1.fun.GetMainQuestBase() + 0x1C4);

                        questid = fm1.gMrw.readInt32(fm1.fun.GetMainQuestBase());

                        if (QuestLevel < currentRagion.minLevel)
                        {
                            fm1.writeLogLine("当前任务区域不是最佳任务区域,开始清理任务");
                            fm1.fun.CommplteAllQuest();
                            Thread.Sleep(1000);
                            fm1.writeLogLine("清理完成");
                            continue;
                        }
                        fm1.fun.AcceptQuest(questid);
                        while (fm1.fun.GetAcceptMainQuestBase() == 0) Thread.Sleep(100);

                        if ((fm1.gMrw.readInt32(fm1.fun.GetMainQuestBase() + 0x2B8, 0) <= 0
            || fm1.gMrw.readString(fm1.gMrw.readInt32(fm1.fun.GetMainQuestBase() + 0x2D0)).Contains("index")
            || fm1.gMrw.readString(fm1.gMrw.readInt32(fm1.fun.GetMainQuestBase() + 0x2D0)).Contains("hunt")
            || fm1.gMrw.readString(fm1.gMrw.readInt32(fm1.fun.GetMainQuestBase() + 0x2D0)).Contains("meet")
            || fm1.gMrw.readString(fm1.gMrw.readInt32(fm1.fun.GetMainQuestBase() + 0x2D0)).Contains("quest clear")
      )
                            )
                        {
                            for (int i = 0; i < fm1.fun.GetAcceptMainQuestTwice(); i++)
                                fm1.fun.CompletingQuest(questid);
                            fm1.fun.CommittingQuest(questid);
                            Thread.Sleep(1000);
                            continue;
                        }

                        if (questid == 3775
                        || questid == 3777
                        || questid == 3658
                        || questid == 3418
                         )
                        {
                            fm1.fun.CompletingQuest(questid);
                            Thread.Sleep(1000);
                            if (fm1.fun.GetAcceptMainQuestTwice() == 0)
                            {
                                fm1.fun.CommittingQuest(questid);
                                Thread.Sleep(1000);
                                continue;
                            }
                        }

                        if (fm1.fun.GetChildQuest() != 0)
                        {
                            if (fm1.gMrw.readInt32(fm1.fun.GetMainQuestBase() + 0x2F4, 0) != fm1.gMrw.readInt32(fm1.fun.GetChildQuest()))
                            {
                                fm1.writeLogLine("检测到子任务和主线不匹配,重新选择角色");
                                fm1.fun.ChooseChara();
                                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) > 0)
                                    Thread.Sleep(100);
                                Thread.Sleep(1000);
                                fm1.fun.EnterChara(CharaPos);
                                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                                    Thread.Sleep(100);
                                continue;
                            }
                        }

                        mapid = fm1.gMrw.readInt32(fm1.fun.GetMainQuestBase() + 0x2B8, 0);

                        bool flag = false;
                        foreach (MapInfomation n in MapInfo.mapInfo)
                        {
                            if (n.MapID != null)
                            {
                                foreach (MapI m in n.MapID)
                                {
                                    if (m.ID == mapid)
                                    {
                                        currentRagion = n;
                                        flag = true;
                                        break;
                                    }
                                }
                                if (flag) break;
                            }
                        }

                        fm1.writeLogLine("当前角色任务区域:" + currentRagion.name);
                        Level = 0;
                    }
                    else
                    {

                        if (fm1.fun.GetChildQuest() != 0 && fm1.fun.GetMainQuestBase() == 0)
                        {
                            fm1.writeLogLine("检测到子任务和主线不匹配,重新选择角色");
                            fm1.fun.ChooseChara();
                            while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) > 0)
                                Thread.Sleep(100);
                            Thread.Sleep(1000);
                            fm1.fun.EnterChara(CharaPos);
                            while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                                Thread.Sleep(100);
                            continue;
                        }

                        Int32 len = currentRagion.MapID.Length;
                        for (int i = 0; i < len; i++)
                        {
                            if (i == len - 1)
                            {
                                mapid = currentRagion.MapID[i].ID;
                                break;
                            }
                            else if (currentRagion.MapID[i].minLevel == charaLevel)
                            {
                                mapid = currentRagion.MapID[i].ID;
                                break;
                            }
                            else if (currentRagion.MapID[i].minLevel > charaLevel && currentRagion.MapID[i - 1].minLevel < charaLevel)
                            {
                                mapid = currentRagion.MapID[i - 1].ID;
                                break;
                            }
                            else
                            {
                                mapid = currentRagion.MapID[0].ID;
                            }
                        }
                        Level = currentRagion.maxDiff;
                    }

                    fm1.fun.CityTp(currentRagion.bigRegionID, currentRagion.smallRegionID, currentRagion.x, currentRagion.y);
                    fm1.fun.ChooseInstance();
                    Thread.Sleep(1000);
                    //fm1.fun.EnterInstance(mapid, Level);
                    fm1.fun.EnterQuestInstance(mapid, Level, questid);

                    while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) <= 0)
                        Thread.Sleep(100);
                    if (Config.sss)
                        fm1.fun.SSS();
                    if (Config.IsHide)
                        fm1.fun.HideCall();
                    Thread.Sleep(1000);
                    fm1.fun.bfs_load();
                    TickTime = Win32.Kernel.GetTickCount();

                    if (!IsLoad && fm1.GetClearMapFunction() == 4)
                    {
                        fm1.checkCode();
                        IsLoad = true;
                    }

                    if (fm1.GetClearMapFunction() == 3)
                    {
                        fm1.gMrw.Encryption(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 8, 0);
                        //fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq, 0xB94, 0xB8, 4) + 4, 1000);
                        //fm1.gMrw.writeFloat(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq, 0xB94, 0xCC, 4 , 0x18) + 8, (float)1000.0);
                        fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq, 0xB94, 0xB8, 4) + 4, 1000);
                        fm1.gMrw.writeFloat(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq, 0xB94, 0x108, 4, 0x2C) + 0x1C, (float)1000.0);
                        // fm1.gMrw.writeFloat(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq, 0xB94, 0x144, 4, 0x2C) + 0x24, (float)0.0);//伤害
                        fm1.gMrw.writeFloat(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq, 0xB94, 0x108, 4, 0x2C) + 0x24, (float)1000.0);
                    }

                    int oldVirtualTable = 0;
                    if (fm1.GetClearMapFunction() == 5)
                    {
                        //
                        //fm1.fun.CheckSkill(fm1.gMrw.readInt32(baseAddr.dwBase_Character), 174, fm1.gMrw.readInt32(baseAddr.dwBase_Role_Id), 0, 1, 1, 1, 2, 1, 100000, 100000, 100000);
                        //if (Config.isHook)
                        //{
                        //    int addr = fm1.gMrw.readInt32(baseAddr.dwBase_Character);

                        //    Byte[] old = fm1.gMrw.readData(fm1.gMrw.read<uint>(addr) - 0x1000, 0x3000);
                        //    fm1.gMrw.writedData((uint)fm1.at.GetVirtualAddr() + 0x2001, old, 0x3000);
                        //    fm1.gMrw.writeInt32(fm1.at.GetVirtualAddr() + 0x3001 + 0x468, 0x02E1F9DC );//0314338C    B0 01           mov al,0x1
                        //    fm1.gMrw.writeInt32(addr, fm1.at.GetVirtualAddr() + 0x3001);
                        //}
                        //fm1.fun.EncryptionCall(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0x5F4, 0);
                        //fm1.writeLogLine("锁定耐久成功");
                        //if (fm1.gMrw.readInt32(addr) != fm1.at.GetVirtualAddr() + 0x3001)
                        //{
                        //}

                        oldVirtualTable = fm1.gMrw.readInt32(baseAddr.dwBase_Character, 0);
                        //int atk_addr = fm1.fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/actionobject/common/attackinfo/bigboom2.atk");
                        int atk_addr = fm1.fun.LoadCall(baseAddr.GetIndexObj.Atk, "monster/newmonsters/event/zombi/attackinfo/attack.atk");
                        //int head = fm1.gMrw.readInt32(atk_addr + 0x1F0);
                        //fm1.gMrw.writeInt32(head , 2);
                        //fm1.gMrw.writeInt32(head + 8 , 2);
                        //fm1.fun.EncryptionCall(head + 0x1C, 2000);
                        //head = fm1.gMrw.readInt32(head + 0x28);

                        //fm1.fun.EncryptionCall(head, 30000000);

                        //fm1.fun.EncryptionCall();
                        //int atk_addr = fm1.fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/equipmentpassiveobject/ancient_legendary/armor/140088_plate_shoe/attackinfo/earthquake_plate.atk");
                        int addr = fm1.gMrw.readInt32(baseAddr.dwBase_Character);
                        Byte[] old = fm1.gMrw.readData(fm1.gMrw.read<uint>(addr) - 0x1000, 0x3000);
                        fm1.gMrw.writedData((uint)fm1.at.GetVirtualAddr() + 0x2001, old, 0x3000);
                        fm1.gMrw.writeInt32(addr, fm1.at.GetVirtualAddr() + 0x3001);

                        if (fm1.gMrw.readInt32(baseAddr.dwBase_Character, 0, 0x36C) < fm1.at.GetVirtualAddr())
                        {
                            fm1.at.clear();
                            //fm1.at.mov_eax(atk_addr);
                            //fm1.at.retn(4);
                            fm1.at.mov_esp_ptr_addx(4, atk_addr);
                            fm1.at.push(fm1.gMrw.readInt32(baseAddr.dwBase_Character, 0, 0x36C));
                            fm1.at.retn();
                            int i = 0;
                            foreach (byte a in fm1.at.Code)
                            {
                                fm1.gMrw.writeInt8(fm1.at.GetVirtualAddr() + 0xC50 + i++, a);
                            }
                        }


                        fm1.at.setEvent();
                        //fm1.gMrw.writeInt32(fm1.at.GetVirtualAddr() + 0x3001 + 0x834, fm1.at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1
                        //fm1.gMrw.writeInt32(fm1.at.GetVirtualAddr() + 0x3001 + 0x5BC, fm1.at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1

                        fm1.gMrw.writeInt32(fm1.at.GetVirtualAddr() + 0x3001 + 0x36C, fm1.at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1
                        fm1.gMrw.writeInt32(fm1.at.GetVirtualAddr() + 0x3001 + 0x468, 0x02E1F9DC );//02E1F9DC    B0 01           mov al,0x1




                    }


                    while (!IsBossDie())
                    {
                        ////fun.KeyPress((Int32)Keys.Space);

                        if (mapid == 88)
                        {
                            fm1.fun.TermidateObj(fm1.fun.GetAddressByCode(10274));
                            fm1.fun.TermidateObj(fm1.fun.GetAddressByCode(10273));
                            fm1.fun.TermidateObj(fm1.fun.GetAddressByCode(10274));
                            fm1.fun.TermidateObj(fm1.fun.GetAddressByCode(10273));
                            fm1.fun.TermidateObj(fm1.fun.GetAddressByCode(10274));
                            fm1.fun.TermidateObj(fm1.fun.GetAddressByCode(10273));
                        }


                        while (IsRoomOpen() == false)
                        {
                            if (mapid == 42)
                            {
                                fm1.fun.TermidateObj(fm1.fun.GetAddressByCode(8104));
                            }
                            if (currentRagion.name == "雪山")
                                fm1.fun.TermidateObj(fm1.fun.GetAddressByCode(817));
                            //if (Config.CloseLevelUp)
                            //    KeyEvent.B();
                            ////fun.KeyPress((Int32)Keys.Space);
                            switch (fm1.GetClearMapFunction())
                            {
                                case 0:
                                    {

                                        if (questid == 3151 && fm1.fun.GetCurrencyRoom().X == 3)
                                        {
                                            fm1.fun.checkGrope();
                                            fm1.fun.CreateEmery(fm1.gMrw.readInt32(baseAddr.dwBase_Character), 0, 100010);
                                            Thread.Sleep(2000);

                                        }
                                        else if (questid == 3339 && fm1.fun.GetCurrencyRoom().X == 1 && fm1.fun.GetCurrencyRoom().Y == 0)
                                        {
                                            fm1.fun.checkGrope();
                                            fm1.fun.CreateEmery(fm1.gMrw.readInt32(baseAddr.dwBase_Character), 0, 100010);
                                            Thread.Sleep(2000);
                                        }
                                        else if (questid == 3265)
                                        {

                                        }
                                        else
                                        {
                                            if (fm1.IsFastKill())
                                                fm1.fun.checkEmery(60025);
                                            else
                                                fm1.fun.checkEmery(70301);
                                        }

                                        break;
                                    }

                                case 1:
                                    {
                                        Int32 addr = 0;
                                        //fm1.fun.checkGrope();
                                        fm1.fun.CreateEmery(fm1.gMrw.readInt32(baseAddr.dwBase_Character), 0, 100010);
                                        while ((addr = fm1.fun.GetAddressByCode(100010)) == 0) Thread.Sleep(1);
                                        fm1.gMrw.writeInt32(addr + 0x870, 0);
                                        while ((addr = fm1.fun.GetAddressByCode(100010)) != 0) Thread.Sleep(1);
                                        break;
                                    }
                                case 2:
                                    {
                                        ////fun.KeyPress('A');
                                        fm1.fun.setCharaPosSpacialEmery(fm1.gMrw.readInt32(baseAddr.dwBase_Character));
                                        if (mapid == 227 && fm1.fun.GetCurrencyRoom().X == 0)
                                            fm1.fun.checkEmery();
                                        if (mapid == 236 && fm1.fun.GetCurrencyRoom().Y == 0)
                                            fm1.fun.checkEmery();
                                        break;
                                    }
                                case 3:
                                    fm1.fun.SetCharaPos();
                                    break;
                                case 4:
                                    {
                                        fm1.fun.setCharaPosSpacialEmery();
                                        break;
                                    }
                                case 5:
                                    {
                                        ////fun.KeyPress('D');

                                        WinIo.KeyPress(VKKey.VK_X, 50);
                                        WinIo.KeyPress(VKKey.VK_Z, 50);
                                        MyKey.MKeyDownUp(0, MyKey.Chr(0x1D));
                                        MyKey.MKeyDownUp(0, MyKey.Chr(0x1B));
                                        fm1.fun.SetCharaPos();
                                        break;

                                        //MyKey.MKeyDownUp(0, MyKey.Chr(0x4));
                                        //fm1.fun.SetCharaPos();
                                        //KeyEvent.B1();
                                        //Thread.Sleep(500);
                                    }
                            }

                            Thread.Sleep(500);
                        }

                        System.Drawing.Point p = fm1.fun.GetCurrencyRoom();
                        PickUpWithSy();
                        if (!IsBossDie())
                        {
                            if (fm1.fun.GetNextRoom(p) == -1 && !IsBossDie() && (fm1.fun.GetCurrencyRoom().X != fm1.fun.GetBossRoom().X || fm1.fun.GetCurrencyRoom().Y != fm1.fun.GetBossRoom().Y))
                            {
                                fm1.writeLogLine("偏离正常路线，重新计算新的路线");
                                fm1.fun.bfs_load();
                            }
                            if (!fm1.IsQzTp() && !IsSpacialRoom(mapid, fm1.gMrw.readInt32(baseAddr.dwBase_Quest_Instace_Id), p))
                                fm1.fun.CoorTp(fm1.fun.GetNextRoom(p));
                            else
                                fm1.fun.InstanceTp(fm1.fun.GetNextRoom(p));
                            Int32 f = 0;
                            while (IsRoomOpen() == true)
                            {
                                if (f++ >= 1000)
                                    break;
                                Thread.Sleep(1);
                            }
                        }
                        Thread.Sleep(1000);
                    }

                    if (fm1.GetClearMapFunction() == 5)
                    {
                        fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character), oldVirtualTable);
                    }
                    //if (fm1.fun.GetAcceptMainQuestTwice() != 0)
                    //    fm1.fun.checkEmery(60025);

                    fm1.writeGetLine("自动剧情" + ++LevelTwice + "，本次耗时:" + ((Win32.Kernel.GetTickCount() - TickTime) / 1000.0) + "s");
                    Int32 gole = fm1.fun.GetCharaGole();
                    Thread.Sleep(500);
                    //if (fm1.IsMainCharacter())
                    //    fm1.fun.ChooseCard(0, 0);
                    //else
                    //    fm1.fun.ChooseCard(0, 1);

                    //if (fm1.IsGetGoleCard())
                    //{
                    //    if (fm1.IsMainCharacter())
                    //        fm1.fun.ChooseCard(1, 0);
                    //    else
                    //        fm1.fun.ChooseCard(1, 1);
                    //}
                    //else
                    //    fm1.fun.GetCardTrue();


                    Int32 time = 0;
                    while (gole == fm1.fun.GetCharaGole())
                    {
                        WinIo.KeyPress(VKKey.VK_ESCAPE);
                        MyKey.MKeyDownUp(0, MyKey.Chr(0x29));
                        Thread.Sleep(100);
                        if ((time += 100) >= 15000)
                            break;
                    }


                    //if (mapType != MapI.MapType.Eiji)
                    //    fm1.fun.Resolve();
                    //else
                    //    fm1.fun.spacialResolve();
                    //fm1.fun.CloseGabriel();
                    //fm1.fun.ChallengeAgain();
                    Thread.Sleep(2000);
                    switch (Config.way)
                    {
                        case 0:
                            {
                                fm1.fun.Sell();
                                break;
                            }
                        case 1:
                            {
                                fm1.fun.Resolve();
                                break;
                            }
                    }

                    fm1.fun.CommittingQuest(questid);
                    Thread.Sleep(1000);
                    fm1.fun.QuitInstanceNoMainThread();

                    while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) > 0)
                    {
                        WinIo.KeyPress(VKKey.VK_ESCAPE);
                        MyKey.MKeyDownUp(0, MyKey.Chr(0x29));
                    }

                }
                fm1.fun.ChooseChara();
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) > 0)
                    Thread.Sleep(100);

                Thread.Sleep(5000);
                fm1.fun.EnterChara(++CharaPos);
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                    Thread.Sleep(100); ;
                Thread.Sleep(2000);
            }

        }
        bool a = false;
        public void LevelStart()
        {
            if (a == false)
            {
                t_yw = new Thread(AutoLevel);
                t_yw.Start();
            }
            else
            {
                t_yw.Abort();
            }
            a = !a;
        }

        public void dealEijiItem()
        {
            
        }

        public void PickUpWithSy()
        {
            string cname = fm1.gMrw.readString(fm1.gMrw.readInt32(baseAddr.dwBase_Character, 0x408));

            Int32 map = fm1.gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 End = fm1.gMrw.readInt32(map + 0xC4);
            Int32[] temp = new Int32[100];
            Int32 Count = 0;


            for (Int32 i = fm1.gMrw.readInt32(map + 0xC0); i < End; i += 4)
            {
                Int32 b = fm1.gMrw.readInt32(i);

                Int32 item_point = fm1.gMrw.readInt32(b + 0x16C4);
                string name = fm1.gMrw.readString(fm1.gMrw.readInt32(item_point + 0x24));
                int item_pj = fm1.gMrw.readInt32(item_point + 0x178);

                if (fm1.gMrw.readInt32(b + 0xA4) == 0x121)
                {
                    if (!name.Contains("转换书"))
                    {
                        if (!name.Contains("碎片") && !name.Contains("怨念"))
                        {
                            if (item_pj >= 3 || name.Contains("卡片"))
                                fm1.writeSpacilLine(DateTime.Now.ToLongTimeString().ToString() + " " + cname + ":获得 [" + name + "]");
                        }
                        temp[Count++] = fm1.gMrw.Decryption(b + 0xAC);
                    }

                }
            }

            for (Int32 i = Count - 1; i >= 0; i--)
            {
                fm1.fun._PickUp(temp[i]);
            }
        }
        public void PickUpWithYj()
        {
            string cname = fm1.gMrw.readString(fm1.gMrw.readInt32(baseAddr.dwBase_Character, 0x408));

            Int32 map = fm1.gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 End = fm1.gMrw.readInt32(map + 0xC4);
            Int32[] temp = new Int32[100];
            Int32 Count = 0;


            for (Int32 i = fm1.gMrw.readInt32(map + 0xC0); i < End; i += 4)
            {
                Int32 b = fm1.gMrw.readInt32(i);

                Int32 item_point = fm1.gMrw.readInt32(b + 0x16C4);
                string name = fm1.gMrw.readString(fm1.gMrw.readInt32(item_point + 0x24));
                int item_pj = fm1.gMrw.readInt32(item_point + 0x178);

                if (fm1.gMrw.readInt32(b + 0xA4) == 0x121)
                {
                    fm1.gMrw.writeInt32(item_point + 0x178, 0);

                    //if (name.Contains("裂魂"))
                    //{
                    //    fm1.fun.TermidateObj(b);
                    //}
                    //else
                    //{
                    if (fm1.gMrw.readInt32(item_point + 0x314) == 0)
                    {
                        temp[Count++] = fm1.gMrw.Decryption(b + 0xAC);
                    }
                    else
                    {
                        fm1.writeLogLine("异界气息装备，跳过拾取");

                    }

                    //}
                    if (name.Contains("浓缩") || name.Contains("卡片"))
                    {
                        fm1.writeSpacilLine(DateTime.Now.ToLongTimeString().ToString() + " " + cname + ":获得 [" + name + "]");
                    }
                }
            }

            for (Int32 i = Count - 1; i >= 0; i--)
            {
                fm1.fun._PickUp(temp[i]);
            }
        }
        public void PickUpWithNb()
        {
            Int32 map = fm1.gMrw.readInt32(baseAddr.dwBase_Character, 0xC8);
            Int32 End = fm1.gMrw.readInt32(map + 0xC4);
            Int32[] temp = new Int32[100];
            Int32 Count = 0;


            for (Int32 i = fm1.gMrw.readInt32(map + 0xC0); i < End; i += 4)
            {
                Int32 b = fm1.gMrw.readInt32(i);

                Int32 item_point = fm1.gMrw.readInt32(b + 0x141C);
                string name = fm1.gMrw.readString(fm1.gMrw.readInt32(item_point + 0x24));
                int item_pj = fm1.gMrw.readInt32(item_point + 0x178);

                if (fm1.gMrw.readInt32(b + 0xA4) == 0x121)
                {

                    if (item_pj >= 2)
                    {
                        fm1.writeGetLine(fm1.gMrw.readString(fm1.gMrw.readInt32(baseAddr.dwBase_Character, 0x408)) + ": 获得 [" + name + "]");
                        temp[Count++] = fm1.gMrw.Decryption(b + 0xAC);

                    }

                }
            }
            for (Int32 i = Count - 1; i >= 0; i--)
            {
                fm1.fun._PickUp(temp[i]);
            }
        }
        void t_checkID_1()
        {
            while (true)
            {
                Thread.Sleep(100);
            }
        }
        void t_checkID()
        {
            while (true)
            {

                Thread.Sleep(100);
            }
        }
        private void SY_SKZM()
        {
            Int32 CharaPos = fm1.gMrw.readInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Role) + 0x160);

            while (true)
            {
                Thread.Sleep(2000);
                fm1.fun.CityTp(12, 0, 200, 300);
                Thread.Sleep(1000);

                fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character) + 0xA04, 1);


                while (fm1.fun.GetPL() >= 1 && fm1.fun.GetCharaLevel() <= 90)
                {
                    Thread.Sleep(1000);
                    fm1.fun.ChooseInstance();
                    Thread.Sleep(1000);

                    fm1.fun.EnterSy(75, 0);

                    Random r = new Random();

                    //fm1.fun.EnterIacnstance(2009, 1);
                    while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) <= 0)
                        Thread.Sleep(100);

                    Thread.Sleep(1000);

                    if (fm1.GetClearMapFunction() == 0)
                        new Thread(t_checkID_1).Start();


                    if (fm1.GetClearMapFunction() == 1)
                    {
                        new Thread(t_checkID_1).Start();
                    }

                    if (fm1.GetClearMapFunction() == 2)
                    {
                        new Thread(t_checkID_1).Start();
                    }
                    if (fm1.GetClearMapFunction() == 3)
                    {
                        new Thread(t_checkID_1).Start();
                    }

                    mo:

                    //fm1.fun.InstanceTp(1, 0);
                    bool flag = false;

                    while (flag == false)
                    {

                        while (IsRoomOpen() == false)
                        {

                            if (fm1.GetClearMapFunction() == 0)
                            {
                                fm1.fun.checkEmery(70301);
                                if (fm1.fun.GetCurrencyRoom().Y == 1)
                                    flag = true;
                                Thread.Sleep(50);
                            }

                            if (fm1.GetClearMapFunction() == 1)
                            {
                                fm1.fun.xiguai();
                                if (fm1.fun.GetCurrencyRoom().Y == 1)
                                    flag = true;
                            }
                            if (fm1.GetClearMapFunction() == 2)
                            {
                                ////fun.KeyPress('A');
                                if (fm1.fun.GetCurrencyRoom().Y == 1)
                                    flag = true;
                                Thread.Sleep(500);
                            }
                            if (fm1.GetClearMapFunction() == 3)
                            {
                                if (fm1.fun.GetCurrencyRoom().Y == 1)
                                    flag = true;
                            }

                            Thread.Sleep(50);
                        }
                        System.Drawing.Point p = fm1.fun.GetCurrencyRoom();
                        PickUpWithSy();

                        if (p.Y == 2)
                        {
                            if (p.X < 2)
                                fm1.fun.CoorTp(3);
                            else if (p.X > 2)
                                fm1.fun.CoorTp(2);
                            else if (p.X == 2)
                                fm1.fun.CoorTp(0);

                            Int32 f = 0;
                            while (IsRoomOpen() == true)
                            {
                                if (f++ >= 2000)
                                    break;
                                Thread.Sleep(1);
                            }
                        }
                        Thread.Sleep(1000);
                    }
                    PickUpWithSy();

                    Thread.Sleep(1000);

                    if (!IsRoomOpen())
                        goto mo;

                    //if (fm1.IsJbl())
                    //{
                    //    if (fm1.fun.GetAddressByCode(10093) != 0)
                    //    {
                    //        return;
                    //    }
                    //}

                    fm1.fun.QuitInstanceNoMainThread();
                    while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) > 0) Thread.Sleep(100);
                }

                fm1.fun.ChooseChara();
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) > 0)
                    Thread.Sleep(100);
                fm1.fun.EnterChara(++CharaPos);
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                    Thread.Sleep(100);
                ;
            }
        }
        private void SY()
        {
            Int32 CharaPos = fm1.gMrw.readInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Role) + 0x160);
            if (fm1.GetClearMapFunction() == 1)
                fm1.checkSYCode();
            bool IsLoad = false;

            while (true)
            {
                Thread.Sleep(2000);
                fm1.fun.CityTp(30, 2, 400, 300);
                Thread.Sleep(1000);

                fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character) + 0xA04, 1);

                BagItem b = fm1.fun.GetItem(10100021);
                for (int i = 0; i < b.Count; i++)
                {
                    fm1.fun.SendOpenPackage(b.Pos);
                    Thread.Sleep(100);
                }

                if (fm1.GetClearMapFunction() == 2 || fm1.GetClearMapFunction() == 3)
                {
                    //fm1.checkSkill();
                }

                fm1.writeLogLine("深渊票数量: " + fm1.fun.GetItem(3330).Count + " 巨龙票数量:" + fm1.fun.GetItem(490000338).Count);
                while (fm1.fun.GetPL() >= 1 && (fm1.fun.GetItem(490000338).Count != 0 || fm1.fun.GetItem(3330).Count >= 28))
                {
                    Thread.Sleep(1000);
                    fm1.fun.ChooseInstance();
                    Thread.Sleep(1000);


                    fm1.fun.EnterSy(310, 4);
                    if (!IsLoad && fm1.GetClearMapFunction() == 1)
                    {
                        fm1.checkSYCode();
                        IsLoad = true;
                    }
                    Random r = new Random();

                    while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) <= 0)
                        Thread.Sleep(100);

                    Thread.Sleep(1000);
                    mo:

                    //fm1.fun.InstanceTp(1, 0);
                    bool flag = false;

                    while (flag == false)
                    {

                        while (IsRoomOpen() == false || (!flag && fm1.fun.GetCurrencyRoom().X == 3))
                        {

                            if (fm1.fun.IsHaveSyEmery() == true)
                                flag = true;

                            if (fm1.GetClearMapFunction() == 0)
                            {
                                fm1.fun.checkEmery(60025);
                                Int32 addr;
                                if (fm1.fun.GetCurrencyRoom().X == 3)
                                {
                                    if ((addr = fm1.fun.GetAddressByCode(30528)) != 0)
                                    {
                                        Thread.Sleep(200);

                                        int start = fm1.gMrw.readInt32(addr + 0x1750);
                                        int end = fm1.gMrw.readInt32(addr + 0x1754);

                                        Int32[] id_old = new Int32[(end - start) / 20];
                                        Int32 count = 0;


                                        for (int i = start; i < end; i += 20)
                                        {
                                            id_old[count++] = fm1.gMrw.readInt32(i + 8);
                                            if (fm1.GetClearMapFunction() == 0)
                                                fm1.fun.CreateEmery(addr, fm1.gMrw.readInt32(i + 8), 60025);
                                        }

                                        fm1.gMrw.writeInt32(addr + 0x1750, 0);
                                        fm1.gMrw.writeInt32(addr + 0x1754, 0);
                                    }
                                    flag = true;
                                }
                                Thread.Sleep(500);
                            }

                            if (fm1.GetClearMapFunction() == 1)
                            {
                                if (fm1.fun.GetCurrencyRoom().X == 3)
                                {
                                    Int32 addr;
                                    Int32[] id_old = null;

                                    //                                    02A8BC74    8B440F 08       mov eax, dword ptr ds:[edi+ecx+0x8]
                                    //02A8BC78    8B540F 04       mov edx, dword ptr ds:[edi+ecx+0x4]
                                    //02A8BC7C    50              push eax
                                    //02A8BC7D    8B040F mov eax,dword ptr ds:[edi+ecx]
                                    //02A8BC80    52              push edx
                                    //02A8BC81    50              push eax



                                    if ((addr = fm1.fun.GetAddressByCode(30528)) != 0)
                                    {
                                        Thread.Sleep(200);

                                        int start = fm1.gMrw.readInt32(addr + 0x1750);
                                        int end = fm1.gMrw.readInt32(addr + 0x1754);

                                        id_old = new Int32[(end - start) / 20];
                                        Int32 num = 0;

                                        for (int i = start; i < end; i += 20)
                                        {
                                            id_old[num++] = fm1.gMrw.readInt32(i + 8);
                                        }
                                        flag = true;
                                        Int32 count = 0;
                                        Int32 chr = fm1.gMrw.readInt32(baseAddr.dwBase_Character);
                                        Int32 map = fm1.gMrw.readInt32(chr + 0xC8);
                                        Int32 dest = fm1.gMrw.readInt32(map + 0xC4);
                                        for (Int32 i = fm1.gMrw.readInt32(map + 0xC0); i < dest; i += 4)
                                        {
                                            Int32 onobj = fm1.gMrw.readInt32(i);
                                            Int32 zy = fm1.gMrw.readInt32(onobj + 0x870);
                                            Int32 type = fm1.gMrw.readInt32(onobj + 0xA4);
                                            Int32 grope = fm1.gMrw.readInt32(onobj + 0x870);
                                            if (grope == 0)
                                                continue;
                                            if (onobj == fm1.gMrw.readInt32(baseAddr.dwBase_Character))
                                                continue;
                                            if (type == 545 || type == 529)
                                            {
                                                fm1.fun.EncryptionCall(onobj + 0xAC, id_old[count++]);
                                                if (count >= id_old.Length)
                                                    break;
                                            }
                                        }
                                        fm1.fun.TermidateObj(addr);
                                    }

                                }

                            }
                            if (fm1.GetClearMapFunction() == 2)
                            {
                                ////fun.KeyPress('A');
                                MyKey.MKeyDownUp(0, MyKey.Chr(0x4));
                                ////fun.KeyPress('A');

                                if (fm1.fun.GetCurrencyRoom().X == 3)
                                {
                                    Int32 addr;
                                    Int32[] id_old = null;

                                    if ((addr = fm1.fun.GetAddressByCode(30528)) != 0)
                                    {
                                        Thread.Sleep(200);

                                        int start = fm1.gMrw.readInt32(addr + 0x1750);
                                        int end = fm1.gMrw.readInt32(addr + 0x1754);

                                        id_old = new Int32[(end - start) / 20];
                                        Int32 num = 0;

                                        for (int i = start; i < end; i += 20)
                                        {
                                            id_old[num++] = fm1.gMrw.readInt32(i + 8);
                                        }
                                        flag = true;
                                        Int32 count = 0;
                                        Int32 chr = fm1.gMrw.readInt32(baseAddr.dwBase_Character);
                                        Int32 map = fm1.gMrw.readInt32(chr + 0xC8);
                                        Int32 dest = fm1.gMrw.readInt32(map + 0xC4);
                                        for (Int32 i = fm1.gMrw.readInt32(map + 0xC0); i < dest; i += 4)
                                        {
                                            Int32 onobj = fm1.gMrw.readInt32(i);
                                            Int32 zy = fm1.gMrw.readInt32(onobj + 0x870);
                                            Int32 type = fm1.gMrw.readInt32(onobj + 0xA4);
                                            Int32 grope = fm1.gMrw.readInt32(onobj + 0x870);
                                            if (grope == 0)
                                                continue;
                                            if (onobj == fm1.gMrw.readInt32(baseAddr.dwBase_Character))
                                                continue;
                                            if (type == 545 || type == 529)
                                            {
                                                fm1.fun.EncryptionCall(onobj + 0xAC, id_old[count++]);
                                                if (count >= id_old.Length)
                                                    break;
                                            }
                                        }
                                        fm1.fun.TermidateObj(addr);
                                    }

                                }

                            }
                            if (fm1.GetClearMapFunction() == 3)
                            {
                                ////fun.KeyPress('A');

                                if (fm1.fun.IsHaveSyEmery())
                                    flag = true;
                            }

                            Thread.Sleep(100);
                        }
                        System.Drawing.Point p = fm1.fun.GetCurrencyRoom();
                        PickUpWithSy();

                        if (p.X < 3)
                        {
                            fm1.fun.CoorTp(3);
                            Int32 f = 0;
                            while (IsRoomOpen() == true)
                            {
                                if (f++ >= 2000)
                                    break;
                                Thread.Sleep(1);
                            }
                        }
                        Thread.Sleep(1000);
                    }
                    PickUpWithSy();

                    Thread.Sleep(2000);

                    if (!IsRoomOpen())
                        goto mo;

                    //if (fm1.IsJbl())
                    //{
                    //    if (fm1.fun.GetAddressByCode(10093) != 0)
                    //    {
                    //        return;
                    //    }
                    //}

                    if (fm1.IsStopWithGobarli())
                    {
                        if (fm1.gMrw.readInt32(0x44FB30C, 0x198) == 28)
                            return;
                    }

                    fm1.fun.QuitInstanceNoMainThread();
                    while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) > 0) Thread.Sleep(100);
                }

                fm1.fun.ChooseChara();
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) > 0)
                    Thread.Sleep(100);
                fm1.fun.EnterChara(++CharaPos);
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                    Thread.Sleep(100);
                ;
            }
        }
        public void StartSY()
        {
            t_yw = new Thread(SY);
            t_yw.Start();
        }
        public bool IsBossDie()
        {

            if (fm1.gMrw.readInt8(fm1.gMrw.readInt32(baseAddr.dwBase_SSS) + 0xA4C) == 0)
                return false;
            return true;
        }
        private void BM()
        {
            Int32 CharaPos = fm1.gMrw.readInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Role) + 0x160);
            if (fm1.GetClearMapFunction() == 4)
                fm1.checkBMCode();
            bool IsLoad = false;

            for (int i = 0; i < 99; i++)
            {
                Thread.Sleep(2000);
                fm1.GetIncomeBegin();

                fm1.fun.CityTp(17, 3, 435, 350);
                Thread.Sleep(1000);

                Int32 count = fm1.fun.GetItem(3332).Count;
                fm1.writeLogLine("当前无尽数量:" + count);
                if (count < 330)
                {
                    fm1.writeLogLine("无尽数量不足:" + (330 - count));
                    fm1.fun.SendGetStoreItem(3332, 330 - count);
                }

                //if (fm1.GetClearMapFunction() == 0)
                //{
                //    Int32 _base = fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq);
                //    fm1.fun.EncryptionCall(_base + 0x000009D0, 3000);
                //}


                if (fm1.GetClearMapFunction() == 3)
                {
                    fm1.fun.lockWear();
                    if (Config.IsRoleBig)
                        fm1.gMrw.writeFloat(fm1.gMrw.readInt32(baseAddr.dwBase_Character) + 0x2F0, 4.0f);
                    Int32 bs = fm1.gMrw.Decryption(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0x810);
                    fm1.writeLogLine("当前武器物理攻击力：" + bs + "智能倍功开启");

                    bs = (int)((800.0 / bs) * 150000);

                    fm1.fun.CheckSkill(fm1.gMrw.readInt32(baseAddr.dwBase_Character), 174, fm1.gMrw.readInt32(baseAddr.dwBase_Role_Id), 0, 1, 1, 1, 2, 1, bs, bs, bs);
                    Int32 _base = fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq);
                    fm1.fun.EncryptionCall(_base + 0x000009D0, 1000);
                    fm1.fun.EncryptionCall(_base + 0x000009E0, 1000);
                    fm1.fun.EncryptionCall(_base + 0x000009D8, 1000);
                }


                //if (fm1.GetClearMapFunction() == 2)
                //{
                //    fm1.checkSkill();
                //}

                //fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character) + 0xA04, 1);
                //fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character) + 0x918, 1);

                while (fm1.fun.GetPL() >= 6)
                {
                    Thread.Sleep(1000);
                    fm1.fun.ChooseInstance();
                    Thread.Sleep(2000);
                    fm1.fun.EnterInstance(7103, 2);
                    fm1.GetIncome();
                    while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) <= 0)
                        Thread.Sleep(100);

                    fm1.GetIncomeBegin();



                    if (fm1.GetClearMapFunction() == 5)
                    {
                        fm1.gMrw.Encryption(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 8, 0);

                        //fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq, 0xB94, 0xB8, 4) + 4, 1000);
                        //fm1.gMrw.writeFloat(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq, 0xB94, 0xCC, 4 , 0x18) + 8, (float)1000.0);

                        fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq, 0xB94, 0xB8, 4) + 4, 1000);
                        fm1.gMrw.writeFloat(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq, 0xB94, 0x108, 4, 0x2C) + 0x1C, (float)1000.0);

                        // fm1.gMrw.writeFloat(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq, 0xB94, 0x144, 4, 0x2C) + 0x24, (float)0.0);//伤害
                        fm1.gMrw.writeFloat(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq, 0xB94, 0x108, 4, 0x2C) + 0x24, (float)1000.0);

                    }


                    //fm1.fun.SSS();
                    fm1.fun.bfs_load();

                    if (!IsLoad && fm1.GetClearMapFunction() == 4)
                    {
                        fm1.checkBMCode();
                        IsLoad = true;
                    }

                    while (!IsBossDie())
                    {

                        Int32 delayTime = 0;

                        //fm1.fun.movCharaPos(rP[fm1.fun.GetCurrencyRoom().X, fm1.fun.GetCurrencyRoom().Y].X, rP[fm1.fun.GetCurrencyRoom().X, fm1.fun.GetCurrencyRoom().Y].Y, 0);
                        while (IsRoomOpen() == false)
                        {


                            if (fm1.GetClearMapFunction() == 0)
                            {
                                if (!fm1.IsFastKill())
                                    fm1.fun.checkEmery(70301);
                                else
                                    fm1.fun.checkEmery(60025);
                            }

                            if (fm1.GetClearMapFunction() == 3)
                            {
                                fm1.fun.SetCharaPos();
                                ////fun.KeyPress('X');
                            }
                            if (fm1.GetClearMapFunction() == 1)
                            {
                                if ((delayTime += 100) > 30000)
                                {
                                    delayTime = 0;
                                    fm1.fun.TermidateObj(fm1.fun.GetAddressByCode(50501));
                                    Int32 addr = 0;
                                    //KeyEvent.fm1.fun.checkGrope();
                                    KeyEvent.fm1.fun.CreateEmery(KeyEvent.fm1.gMrw.readInt32(baseAddr.dwBase_Character), 1, 50501);
                                    while ((addr = KeyEvent.fm1.fun.GetAddressByCode(50501)) == 0) Thread.Sleep(0);
                                    KeyEvent.fm1.gMrw.writeInt32(addr + 0x870, 0);
                                    KeyEvent.fm1.gMrw.writeInt32(addr + 0x8C8, 1);
                                }
                                if (fm1.fun.GetAddressByCode(69540) == 0)
                                {
                                    fm1.fun.CreateEmery(fm1.gMrw.readInt32(baseAddr.dwBase_Character), -1, 69540);
                                    Int32 addr;
                                    while ((addr = fm1.fun.GetAddressByCode(69540)) == 0) Thread.Sleep(0);
                                    fm1.gMrw.writeFloat(addr + 0x2F0, 20.0f);
                                    fm1.gMrw.writeInt32(addr + 0x870, 0);
                                }
                                else
                                {
                                    Int32 addr = fm1.fun.GetAddressByCode(69540);
                                    fm1.fun.movCharaPos(fm1.fun.getObjPos(addr).x, fm1.fun.getObjPos(addr).y, 0);
                                    Thread.Sleep(1000);
                                }
                            }
                            if (fm1.GetClearMapFunction() == 2)
                            {
                                //fun.KeyPress((Int32)System.Windows.Forms.Keys.Space);
                                //fun.KeyPress((Int32)System.Windows.Forms.Keys.A);
                            }
                            if (fm1.GetClearMapFunction() == 5)
                            {
                                //fun.KeyPress((Int32)System.Windows.Forms.Keys.F1);
                            }

                            if (fm1.GetClearMapFunction() == 4)
                                fm1.fun.SetCharaPos();
                            Thread.Sleep(100);

                        }

                        System.Drawing.Point p = fm1.fun.GetCurrencyRoom();
                        PickUpWithSy();
                        Thread.Sleep(500);

                        if (!IsBossDie())
                        {
                            //fm1.fun.CoorTp(fm1.fun.GetNextRoom(p));

                            //MyKey.MKeyDown(0, MyKey.Chr(0xE0));


                            if (!fm1.IsMainCharacter())
                            {
                                fm1.fun.CoorTp(fm1.fun.GetNextRoom(p));
                            }
                            else
                            {
                                //Int32 x = (Int32)fm1.gMrw.readFloat(fm1.gMrw.readInt32(baseAddr.dwBase_Character , baseAddr.dwOffset_Obj_Pos));
                                //while (x < ((Int32)fm1.gMrw.readFloat(fm1.gMrw.readInt32(baseAddr.dwBase_Character , baseAddr.dwOffset_Obj_Pos)) + 200))
                                //    WinIo.KeyPressEx(VKKey.VK_LEFT);//按下左键一次

                                Int32 rg = fm1.fun.GetNextRoom(p);
                                System.Drawing.Point rgp = fm1.fun.getNextRoomDoorPoint(rg);
                                fm1.fun.movCharaPos(fm1.fun.getObjPos(fm1.gMrw.readInt32(baseAddr.dwBase_Character)).x + 200, rgp.Y + 50, 0);
                                Thread.Sleep(50);
                                WinIo.KeyPressEx(VKKey.VK_LEFT);
                                Thread.Sleep(50);
                                WinIo.KeyDownEx(VKKey.VK_LEFT);
                            }

                            //{
                            //    Int32 x = (Int32)fm1.gMrw.readFloat(fm1.gMrw.readInt32(baseAddr.dwBase_Character , baseAddr.dwOffset_Obj_Pos));
                            //    while (x < ((Int32)fm1.gMrw.readFloat(fm1.gMrw.readInt32(baseAddr.dwBase_Character , baseAddr.dwOffset_Obj_Pos)) + 200))
                            //        MyKey.MKeyDownUp(0, MyKey.Chr(0x50));

                            //    Int32 rg = fm1.fun.GetNextRoom(p);
                            //    System.Drawing.Point rgp = fm1.fun.getNextRoomDoorPoint(rg);

                            //    fm1.fun.movCharaPos((Int32)fm1.gMrw.readFloat(fm1.gMrw.readInt32(baseAddr.dwBase_Character , baseAddr.dwOffset_Obj_Pos)), rgp.Y + 50, 0);

                            //    Thread.Sleep(50);
                            //    MyKey.MKeyDownUp(0, MyKey.Chr(0x50));
                            //    Thread.Sleep(50);
                            //    switch (rg)
                            //    {
                            //        case 0:
                            //            {
                            //                MyKey.MKeyDown(0, MyKey.Chr(0x52));
                            //                break;
                            //            }
                            //        case 1:
                            //            {
                            //                MyKey.MKeyDown(0, MyKey.Chr(0x51));
                            //                break;
                            //            }
                            //        case 2:
                            //            {
                            //                MyKey.MKeyDown(0, MyKey.Chr(0x50));
                            //                break;
                            //            }
                            //        case 3:
                            //            {
                            //                MyKey.MKeyDown(0, MyKey.Chr(0x4F));
                            //                break;
                            //            }
                            //    }
                            //}
                            Int32 f = 0;
                            while (IsRoomOpen() == true)
                            {
                                if (f++ >= 4000)
                                    break;
                                Thread.Sleep(1);
                            }
                            WinIo.KeyUpEx(VKKey.VK_LEFT);

                        }
                        //MyKey.MKeyUp();

                        Thread.Sleep(100);
                    }


                    PickUpWithSy();


                    //fm1.fun.ChooseCard(0, 0);

                    Int32 gole = fm1.fun.GetCharaGole();
                    Thread.Sleep(5500);

                    WinIo.KeyPress(VKKey.VK_ESCAPE);
                    //MyKey.MKeyDownUp(0, MyKey.Chr(0x29));

                    Int32 time = 0;
                    //fm1.fun.GetCardTrue();
                    while (gole == fm1.fun.GetCharaGole())
                    {
                        Thread.Sleep(50);
                        if ((time += 50) >= 7000)
                            break;
                    }

                    Thread.Sleep(1000);
                    fm1.fun.Resolve();
                    Thread.Sleep(1000);
                    fm1.fun.QuitInstance();

                    //if (!Config.IsSellPink)
                    //    fm1.fun.Sell();
                    //else
                    //    fm1.fun.Resolve();
                    //Int32 time = 0;
                    //while (gole == fm1.fun.GetCharaGole() && fm1.gMrw.readInt32(baseAddr.dwBase_Shop, 0xAB14, 0x8) <= 5)
                    //{
                    //    Thread.Sleep(50);
                    //    if ((time += 50) >= 7000)
                    //        break;
                    //}
                    //fm1.fun.Resolve();

                    //time = 0;
                    //while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) > 0)
                    //{
                    //    Thread.Sleep(100);
                    //    if (fm1.fun.GetPL() > 0)
                    //        //fun.KeyPress((int)Keys.F10);
                    //    else
                    //        //fun.KeyPress((int)Keys.F9);

                    //    if ((time += 100) > 10000)
                    //        fm1.fun.CloseGabriel();
                    //}
                }

                Thread.Sleep(1000);

                //int num = fm1.gMrw.readInt32(0x4DEA9D8, 0x94);
                //while (fm1.gMrw.readInt32(0x4DEA9D8, 0x94) != (num + 1)) { fm1.fun.MouseClick(HotKey.bindWindow, 760, 860, true); Thread.Sleep(1000); }
                //while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) > 0) {fm1.fun.MouseClick(HotKey.bindWindow, 700, 670, true); Thread.Sleep(1000); }


                //CharaPos = fm1.gMrw.readInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Role) + 0x160);
                //SetForegroundWindow(HotKey.bindWindow);
                //Thread.Sleep(1000);
                ////fun.KeyPress((int)Keys.Right); 

                //while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) == 0) {
                //    //int x = 300 + (CharaPos % 6) * 180;
                //    //int y = 300 + (CharaPos / 6) * 315;

                //    //fm1.fun.MouseClick(HotKey.bindWindow, (ushort)x, (ushort)y, true);
                //    //fm1.fun.MouseClick(HotKey.bindWindow, (ushort)x, (ushort)y, true);
                //    //Thread.Sleep(10);
                //    //fun.KeyPress((int)Keys.Space);
                //};
                //while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) == 0) //fun.KeyPress((int)Keys.Space);

                Thread.Sleep(2000);
                fm1.fun.ChooseChara();
                Thread.Sleep(2000);
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) > 0)
                    Thread.Sleep(100);
                Thread.Sleep(2000);
                fm1.fun.EnterChara(++CharaPos);
                Thread.Sleep(2000);
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                    Thread.Sleep(100);
                Thread.Sleep(2000);
            }
        }
        public void Jx()
        {
            Int32 CharaPos = fm1.gMrw.readInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Role) + 0x160);

            while (true)
            {
                Thread.Sleep(1000);

                for (int i = 0; i < 10; i++)
                {
                    if (fm1.fun.GetPL() == 0)
                        break;
                    if (fm1.fun.GetCharaLevel() < 55)
                        break;

                    fm1.fun.ChooseInstance();
                    Thread.Sleep(500);
                    fm1.fun.SendEnterJxInstance();

                    while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) <= 0)
                        Thread.Sleep(100);

                    fm1.completeJxTask();
                    fm1.fun.QuitInstance();
                    while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) > 0) Thread.Sleep(100);

                }
                fm1.fun.OpenJx();

                Thread.Sleep(1000);
                fm1.fun.SendEnterStore(10100300, 0);
                Thread.Sleep(1000);

                fm1.fun.ChooseChara();
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) > 0)
                    Thread.Sleep(100);
                fm1.fun.EnterChara(++CharaPos);
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                    Thread.Sleep(100);
            }
        }
        Int32[] City_ID = new Int32[4];
        Int32 Map_ID;
        Int32 Level;
        MapI.MapType mapType;

        private void mainThread()
        {

            bool IsLoad = false;
            Int32 CharaPos = fm1.gMrw.readInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Role) + 0x160);
            //if (fm1.GetClearMapFunction() == 4)
            //{
            //    fm1.checkGLDKx();
            //}

            int oldVirtualTable = 0;

            SimulationKeys.getsk().init(fm1.gMrw, fm1.fun);

            while (true)
            {
                //fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character) + 0x918, 1);
                //if (Config.IsHide)
                //    fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character) + 0xA08, 1);

                //if (fm1.GetClearMapFunction() == 2)
                //{
                //    fm1.checkSkill();
                //}



                //if (fm1.GetClearMapFunction() == 3)
                //{

                //    fm1.fun.lockWear();
                //    Int32 bs = fm1.gMrw.Decryption(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0x810);
                //    fm1.writeLogLine("当前武器物理攻击力：" + bs + "智能倍功开启");

                //    bs = (int)((800.0 / 1000) * 100000);

                //    fm1.fun.CheckSkill(fm1.gMrw.readInt32(baseAddr.dwBase_Character), 174, fm1.gMrw.readInt32(baseAddr.dwBase_Role_Id), 0, 1, 1, 1, 2, 1, bs, bs, bs);
                //    Int32 _base = fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq);
                //    fm1.fun.EncryptionCall(_base + 0x000009D8, 1000);
                //    fm1.fun.EncryptionCall(_base + 0x000009E0, 1000);
                //    fm1.fun.EncryptionCall(_base + 0x000009E8, 1000);
                //}

                //fm1.fun.CommplteAllQuest();
                if (mapType == MapI.MapType.Eiji)
                {
                    Thread.Sleep(4000);
                    if (fm1.fun.IsHaveTask(6928))
                    {
                        fm1.fun.AcceptQuest(6928);
                        fm1.fun.CompletingQuest(6928);
                        fm1.fun.CommittingQuest(6928);

                    }
                    int pos;
                    if (fm1.IsUnlimitedWeight() && ( pos = fm1.fun.GetItem("提取器").Pos) == 0)
                    {
                        fm1.fun.GiveUpSecondaryJob();
                        Thread.Sleep(500);
                        fm1.fun.AcceptQuest(2702);
                        Thread.Sleep(500);
                        fm1.fun.CompletingQuest(2702);
                        Thread.Sleep(500);
                        fm1.fun.CommittingQuest(2702);

                        fm1.fun.AcceptQuest(11007);
                        Thread.Sleep(500);
                        fm1.fun.CompletingQuest(11007);
                        Thread.Sleep(500);
                        fm1.fun.CommittingQuest(11007);
                    }
     
                }

                Thread.Sleep(1000);

                fm1.GetIncomeBegin();
                fm1.fun.CityTp(City_ID[0], City_ID[1], City_ID[2], City_ID[3]);
                Thread.Sleep(1000);

                if (mapType == MapI.MapType.Yuangu || mapType == MapI.MapType.Eiji)
                {
                    Int32 count = fm1.fun.GetItem(3332).Count;
                    fm1.writeLogLine("当前无尽数量:" + count);
                    int need = mapType == MapI.MapType.Yuangu ? 330 : 25;

                 
                    if (count < need)
                    {
                        fm1.writeLogLine("无尽数量不足:" + (need - count));
                        fm1.fun.SendGetStoreItem(3332, need - count);
                    }
                }

                //if (fm1.IsUnlimitedWeight())
                //{
                //    if (fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) != 0)
                //        fm1.fun.EncryptionCall(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0x9B0, 1000000);
                //}





                int minPL = mapType == MapI.MapType.Normal ? 0 : (mapType == MapI.MapType.Yuangu ? 6 : 8);

                int eijiTwice = 0;
                while ((mapType == MapI.MapType.Eiji ? true :
                    (fm1.fun.GetPL() > minPL))
                    && eijiTwice < 5)
                {

                    Thread.Sleep(1000);
                    fm1.fun.ChooseInstance();
                    if (mapType == MapI.MapType.Eiji)
                        eijiTwice++;

                    if (mapType == MapI.MapType.Eiji)
                    {
                        if (fm1.fun.GetCharaLevel() >= 85)
                            fm1.fun.EnterInstance(Map_ID, 2, true, true);
                        else if (fm1.fun.GetCharaLevel() >= 80)
                            fm1.fun.EnterInstance(Map_ID, 1, true, true);
                        else
                            fm1.fun.EnterInstance(Map_ID, 0, true, true);

                    }
                    else if (mapType == MapI.MapType.Yuangu)
                        fm1.fun.EnterInstance(Map_ID, 2, true, true);
                    else
                        fm1.fun.EnterInstance(Map_ID, Level);

                    while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) <= 0)
                        Thread.Sleep(100);
                    fm1.GetIncome();
                    Int32 raddr = fm1.gMrw.readInt32(baseAddr.dwBase_Character);

                    if (fm1.GetClearMapFunction() == 4)
                    {
                        fm1.fun.checkAtk();

                        //fm1.fun.EncryptionCall(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0xEA0, 3774);
                    }
                    //Thread.Sleep(fm1.GetSleepTime());
                    if (Config.sss)
                        fm1.fun.SSS();
                    
                    int tick = Win32.Kernel.GetTickCount();
                    //


                    if (fm1.GetClearMapFunction() == 5)
                    {
                        //
                        //fm1.fun.CheckSkill(fm1.gMrw.readInt32(baseAddr.dwBase_Character), 174, fm1.gMrw.readInt32(baseAddr.dwBase_Role_Id), 0, 1, 1, 1, 2, 1, 100000, 100000, 100000);
                        //if (Config.isHook)
                        //{
                        //    int addr = fm1.gMrw.readInt32(baseAddr.dwBase_Character);

                        //    Byte[] old = fm1.gMrw.readData(fm1.gMrw.read<uint>(addr) - 0x1000, 0x3000);
                        //    fm1.gMrw.writedData((uint)fm1.at.GetVirtualAddr() + 0x2001, old, 0x3000);
                        //    fm1.gMrw.writeInt32(fm1.at.GetVirtualAddr() + 0x3001 + 0x468, 0x02E1F9DC );//0314338C    B0 01           mov al,0x1
                        //    fm1.gMrw.writeInt32(addr, fm1.at.GetVirtualAddr() + 0x3001);
                        //}
                        //fm1.fun.EncryptionCall(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0x5F4, 0);
                        //fm1.writeLogLine("锁定耐久成功");
                        //if (fm1.gMrw.readInt32(addr) != fm1.at.GetVirtualAddr() + 0x3001)
                        //{
                        //}

                        oldVirtualTable = fm1.gMrw.readInt32(baseAddr.dwBase_Character, 0);
                        //int atk_addr = fm1.fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/actionobject/common/attackinfo/bigboom2.atk");
                        //int atk_addr = fm1.fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/actionobject/monster/anton_quest/phase3/bugs/poison/attackinfo/poison_bottom_basic_1.atk");
                        //int head = fm1.gMrw.readInt32(atk_addr + 0x1F0);
                        ////fm1.gMrw.writeInt32(head , 2);
                        ////fm1.gMrw.writeInt32(head + 8 , 2);
                        //fm1.fun.EncryptionCall(head + 0x1C, 2000);
                        //head = fm1.gMrw.readInt32(head + 0x28);

                        //fm1.fun.EncryptionCall(head, 30000000);

                        int atk_addr = fm1.fun.LoadCall(baseAddr.GetIndexObj.Atk, "monster/newmonsters/event/zombi/attackinfo/attack.atk");
                        fm1.writeLogLine("atk:" + atk_addr);
                        //int atk_addr = fm1.fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/monster/evileye/attackinfo/chargedlaser_item.atk");
                        //int head = fm1.gMrw.readInt32(atk_addr + 0x1F0);
                        //fm1.fun.EncryptionCall(head + 0xC, 0x43020000);
                        //fm1.fun.EncryptionCall(head + 20, 120);
                        //fm1.fun.EncryptionCall(head + 0x1C, 500);
                        //head = fm1.gMrw.readInt32(head + 0x28);
                        //fm1.fun.EncryptionCall(head, 50000000);

                        //int atk_addr = fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/monster/evileye/attackinfo/chargedlaser_item.atk");
                        //int head = gMrw.readInt32(atk_addr + 0x1F0);
                        ////gMrw.writeInt32(head, 2);
                        ////gMrw.writeInt32(head + 8, 2);
                        //fun.EncryptionCall(head + 0xC, 0x43020000);
                        //fun.EncryptionCall(head + 20, 200);
                        //fun.EncryptionCall(head + 0x1C, 500);
                        //head = gMrw.readInt32(head + 0x28);
                        //fun.EncryptionCall(head, 599999999);

                        //fm1.fun.EncryptionCall();
                        //int atk_addr = fm1.fun.LoadCall(baseAddr.GetIndexObj.Atk, "passiveobject/equipmentpassiveobject/ancient_legendary/armor/140088_plate_shoe/attackinfo/earthquake_plate.atk");
                        int addr = fm1.gMrw.readInt32(baseAddr.dwBase_Character);
                        Byte[] old = fm1.gMrw.readData(fm1.gMrw.read<uint>(addr) - 0x1000, 0x3000);
                        fm1.gMrw.writedData((uint)fm1.at.GetVirtualAddr() + 0x2001, old, 0x3000);
                        fm1.gMrw.writeInt32(addr, fm1.at.GetVirtualAddr() + 0x3001);

                        if (fm1.gMrw.readInt32(baseAddr.dwBase_Character, 0, 0x36C) < fm1.at.GetVirtualAddr())
                        {
                            fm1.at.clear();
                            //fm1.at.mov_eax(atk_addr);
                            //fm1.at.retn(4);
                            fm1.at.mov_esp_ptr_addx(4, atk_addr);
                            fm1.at.push(fm1.gMrw.readInt32(baseAddr.dwBase_Character, 0, 0x36C));
                            fm1.at.retn();
                            int i = 0;
                            foreach (byte a in fm1.at.Code)
                            {
                                fm1.gMrw.writeInt8(fm1.at.GetVirtualAddr() + 0xC50 + i++, a);
                            }
                        }


                        fm1.at.setEvent();
                        //fm1.gMrw.writeInt32(fm1.at.GetVirtualAddr() + 0x3001 + 0x834, fm1.at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1
                        //fm1.gMrw.writeInt32(fm1.at.GetVirtualAddr() + 0x3001 + 0x5BC, fm1.at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1

                        fm1.gMrw.writeInt32(fm1.at.GetVirtualAddr() + 0x3001 + 0x36C, fm1.at.GetVirtualAddr() + 0xC50);//0314338C    B0 01           mov al,0x1
                        if (Config.isHook)
                            fm1.gMrw.writeInt32(fm1.at.GetVirtualAddr() + 0x3001 + 0x468, 0x02E1F9DC);//0318427C    B0 01           mov al,0x1

                    }
                    if (!Config.IsRoleBig)
                        fm1.fun.bfs_load();
                    fm1.GetIncomeBegin();
                    //if (IsLoad == false)
                    //{
                    //    if (fm1.GetClearMapFunction() == 4)
                    //    {
                    //        fm1.checkGLDKx();
                    //    }
                    //    IsLoad = true;
                    //}
                    Thread.Sleep(1000);

                    while (!IsBossDie())
                    {
                        if (Map_ID == 8508)
                            fm1.fun.TermidateObj(fm1.fun.GetAddressByCode(74089));



                        while (IsRoomOpen() == false && !IsBossDie())
                        {

                            if (Map_ID == 104)
                                fm1.fun.xiguaiWithCode(10660);
                            //if (fm1.GetClearMapFunction() == 0)
                            //{
                            //    if (fm1.IsFastKill())
                            //        fm1.fun.checkEmery(60004);
                            //    else
                            //        fm1.fun.checkEmery(60004);
                            //    Thread.Sleep(1000);
                            //}
                            if (fm1.GetClearMapFunction() == 1)
                            {
                                fm1.fun.SendKilled();
                            }


                            if (fm1.GetClearMapFunction() == 2)
                            {
                                //fun.KeyPress('A');
                            }

                            if (fm1.GetClearMapFunction() == 3)
                            {
                                if (fm1.fun.getObjPos(fm1.gMrw.readInt32(baseAddr.dwBase_Character)).z != -200)
                                {
                                    fm1.fun.movCharaPos(fm1.fun.getObjPos(fm1.gMrw.readInt32(baseAddr.dwBase_Character)).x, fm1.fun.getObjPos(fm1.gMrw.readInt32(baseAddr.dwBase_Character)).y, -200);
                                }
                                WinIo.KeyPress(VKKey.VK_V, 50);
                                KeyEvent.B();
                                MyKey.MKeyDownUp(0, MyKey.Chr(0x19));
                                //fm1.fun.SetCharaPos();
                                //Thread.Sleep(100);
                            }

                            if (fm1.GetClearMapFunction() == 4)
                            {
                                fm1.fun.SetCharaPos();
                                WinIo.KeyPress(VKKey.VK_X, 50);
                                MyKey.MKeyDownUp(0, MyKey.Chr(0x1D));
                                MyKey.MKeyDownUp(0, MyKey.Chr(0x1B));

                                //Int32 r = new Random(Win32.Kernel.GetTickCount()).Next() % 5;

                                //switch (r)
                                //{

                                //    case 1:
                                //        {
                                //            //fun.KeyPress((byte)'S');
                                //            break;
                                //        }
                                //    case 2:
                                //        {
                                //            //fun.KeyPress((byte)'D');
                                //            break;
                                //        }
                                //    case 3:
                                //        {
                                //            //fun.KeyPress((byte)'F');
                                //            break;
                                //        }
                                //    case 4:
                                //        {
                                //            //fun.KeyPress((byte)'X');
                                //            break;
                                //        }
                                //}
                                //fm1.fun.SetCharaPos();

                            }
                            if (fm1.GetClearMapFunction() == 5)
                            {
                                if (fm1.fun.getObjPos(fm1.gMrw.readInt32(baseAddr.dwBase_Character)).z != -200)
                                {
                                    fm1.fun.movCharaPos(fm1.fun.getObjPos(fm1.gMrw.readInt32(baseAddr.dwBase_Character)).x, fm1.fun.getObjPos(fm1.gMrw.readInt32(baseAddr.dwBase_Character)).y, -200);
                                }
                                WinIo.KeyPress(VKKey.VK_X, 50);
                                WinIo.KeyPress(VKKey.VK_Z, 50);
                                MyKey.MKeyDownUp(0, MyKey.Chr(0x1D));
                                MyKey.MKeyDownUp(0, MyKey.Chr(0x1B));

                                //MyKey.MKeyDownUp(0, MyKey.Chr(0x4));
                                //fm1.fun.SetCharaPos();
                                //KeyEvent.B1();
                                //Thread.Sleep(500);

                            }

                            Thread.Sleep(100);
                        }
                        Thread.Sleep(200);
                        System.Drawing.Point p = fm1.fun.GetCurrencyRoom();
                        if (mapType == MapI.MapType.Eiji)
                            PickUpWithYj();
                        else
                        {
                            //MyKey.MKeyDownUp(0, MyKey.Chr(0xCD));
                            //fm1.fun.pickUp2();
                            fm1.fun.PickUp();
                            fm1.fun.PickUpInBuildItem();
                            //Thread.Sleep(1000);

                        }
                        //Thread.Sleep(1000);
                        //

                        //while (fm1.fun.isNextRoomBoss() && Win32.Kernel.GetTickCount() - tick < fm1.GetSleepTime())
                        //    Thread.Sleep(500);
                        if (!IsBossDie())
                        {
                            int timerout = 3000;

                            if (!fm1.IsQzTp())
                            {
                                if (!fm1.IsMainCharacter())
                                {
                                    int coor = fm1.fun.GetNextRoom(p);

                                    string s = "";

                                    switch (coor)
                                    {
                                        case 0:
                                            {
                                                s += MyKey.MKeyDown(0, MyKey.Chr(0x52));
                                                WinIo.KeyDownEx(VKKey.VK_UP);

                                                break;
                                            }
                                        case 1:
                                            {
                                                WinIo.KeyDownEx(VKKey.VK_DOWN);
                                                s += MyKey.MKeyDown(0, MyKey.Chr(0x51));
                                                break;
                                            }
                                        case 2:
                                            {
                                                WinIo.KeyDownEx(VKKey.VK_LEFT);
                                                s += MyKey.MKeyDown(0, MyKey.Chr(0x50));
                                                break;
                                            }
                                        case 3:
                                            {
                                                WinIo.KeyDownEx(VKKey.VK_RIGHT);
                                                s += MyKey.MKeyDown(0, MyKey.Chr(0x4F));
                                                break;
                                            }
                                    }
                                    MyKey.MKeyDown(0, s);
                                    //fm1.fun.CoorTp(fm1.fun.GetNextRoom(p), fm1.fun.GetAddressByCode(107000902));


                                    if (Config.IsRoleBig)
                                        fm1.fun.CoorTp(GetNextRoom(p), fm1.fun.GetAddressByCode(107000902));
                                    else
                                        fm1.fun.CoorTp(fm1.fun.GetNextRoom(p), fm1.fun.GetAddressByCode(107000902));
                                    Thread.Sleep(500);

                                }
                                else
                                {
                                    //timerout = 15000;
                                    //int rg = fm1.fun.GetNextRoom(p);
                                    //if (rg >= 0)
                                    //{

                                    //    var pNextRoom = fm1.fun.getNextRoomDoorPoint(rg);
                                    //    if (rg == 2)
                                    //        pNextRoom.Y += 30;
                                    //    if (rg == 3)
                                    //        pNextRoom.Y += 30;
                                    //    SimulationKeys.getsk().move(pNextRoom.X, pNextRoom.Y, 10, 10, rg / 2 == 0 ? false : true);
                                    //}
                                }

                            }
                            else
                                fm1.fun.InstanceTp(fm1.fun.GetNextRoom(p));
                            Int32 f = 0;
                            while (IsRoomOpen() == true)
                            {
                                if (fm1.IsMainCharacter())
                                {
                                    //SimulationKeys.getsk().moveThread();
                                }
                                if ((f += 100) >= timerout)
                                    break;
                                Thread.Sleep(100);
                            }

                            WinIo.KeyUpEx(VKKey.VK_UP);
                            WinIo.KeyUpEx(VKKey.VK_DOWN);
                            WinIo.KeyUpEx(VKKey.VK_LEFT);
                            WinIo.KeyUpEx(VKKey.VK_RIGHT);
                            MyKey.MKeyUp();

                        }
                        Thread.Sleep(300);
                    }
                    if (mapType == MapI.MapType.Eiji)
                        PickUpWithYj();
                    else
                    {
                        WinIo.KeyPress(VKKey.VK_SPACE);
                        MyKey.MKeyDownUp(0, MyKey.Chr(0x1B));
                        //fm1.fun.pickUp2();
                        fm1.fun.PickUp();
                    }
                    Int32 gole = fm1.fun.GetCharaGole();
                    Thread.Sleep(500);
                    //if (fm1.IsMainCharacter())
                    //    fm1.fun.ChooseCard(0, 0);
                    //else
                    //    fm1.fun.ChooseCard(0, 1);

                    //if (fm1.IsGetGoleCard())
                    //{
                    //    if (fm1.IsMainCharacter())
                    //        fm1.fun.ChooseCard(1, 0);
                    //    else
                    //        fm1.fun.ChooseCard(1, 1);
                    //}
                    //else
                    //    fm1.fun.GetCardTrue();
                    
                    if (fm1.GetClearMapFunction() == 5)
                        fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character), oldVirtualTable);

                    Int32 time = 0;
                    while (gole == fm1.fun.GetCharaGole())
                    {
                        WinIo.KeyPress(VKKey.VK_ESCAPE);
                        MyKey.MKeyDownUp(0, MyKey.Chr(0x29));
                        Thread.Sleep(100);
                        if ((time += 100) >= 15000)
                            break;
                    }


                    //if (mapType != MapI.MapType.Eiji)
                    //    fm1.fun.Resolve();
                    //else
                    //    fm1.fun.spacialResolve();
                    //fm1.fun.CloseGabriel();
                    //fm1.fun.ChallengeAgain();

                    Thread.Sleep(2000);
                    if (mapType != MapI.MapType.Eiji)
                    {
                        switch (Config.way)
                        {
                            case 0:
                                {
                                    fm1.fun.Sell();
                                    break;
                                }
                            case 1:
                                {
                                    fm1.fun.Resolve();
                                    break;
                                }
                        }
                    }
                    else
                    {
                        fm1.fun.spacialResolve();
                    }


                while (fm1.gMrw.readInt32(baseAddr.dwBase_Map_ID) > 0)
                    {
                        //WinIo.KeyPress(VKKey.VK_ESCAPE);
                        //MyKey.MKeyDownUp(0, MyKey.Chr(0x29));
                        //if (fm1.fun.GetPL() <= minPL || eijiTwice >= 5)
                        //{
                        //    MyKey.MKeyDownUp(0, MyKey.Chr(0x45));
                        //    WinIo.KeyPress(VKKey.VK_F12);
                        //}
                        //else
                        //{
                        //    WinIo.KeyPress(VKKey.VK_F10);
                        //    //fm1.fun.QuitTeamInstance();
                        //    MyKey.MKeyDownUp(0, MyKey.Chr(0x43));
                        //}
                        fm1.fun.QuitInstanceNoMainThread();
                        Thread.Sleep(500);
                    }


                    //if (fm1.twice % 5 == 0)
                    //{
                    //    
                    //}

                }
                //if (fm1.GetClearMapFunction() == 5)
                //{
                //    fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0xAC8, 0);
                //    fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0xAC8 + 4, 0);
                //    fm1.gMrw.writeInt32(fm1.gMrw.readInt32(baseAddr.dwBase_Character, baseAddr.dwOffset_Equip_wq) + 0xAC8 + 8, 0);

                //}
                Thread.Sleep(3000);
                fm1.fun.ChooseChara();
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) > 0)
                    Thread.Sleep(100);
                Thread.Sleep(3000);

                fm1.fun.EnterChara(++CharaPos);
                while (fm1.gMrw.readInt32(baseAddr.dwBase_Character) == 0)
                    Thread.Sleep(100);
                Thread.Sleep(500);
            }
        }

        public void StartYw()
        {

            t_yw = new Thread(ywThread);
            t_yw.Start();
        }


        public void StartYw_zb()
        {
            t_yw = new Thread(yw_zb_thread);
            t_yw.Start();
        }

        public void Stop()
        {
            t_yw.Abort();
        }

        public void StartJx()
        {
            t_yw = new Thread(Jx);
            t_yw.Start();
        }


        public void StartBM()
        {
            t_yw = new Thread(BM);
            t_yw.Start();
        }


        public void StartSKZM()
        {
            //fm1.fun.CityTp(2, 7, 300, 300);
            t_yw = new Thread(SY_SKZM);
            t_yw.Start();
        }

        Thread main;
        public void StartAuto(string city_data, Int32 mapid, Int32 level, MapI.MapType type)
        {
            string[] data = city_data.Split('|');
            for (int i = 0; i < 4; i++) City_ID[i] = Int32.Parse(data[i]);
            Map_ID = mapid;
            Level = level;
            mapType = type;
            main = new Thread(mainThread);
            main.Start();
        }

        public void TermidateAuto()
        {
            main.Abort();
            main = null;
        }
    }
}
