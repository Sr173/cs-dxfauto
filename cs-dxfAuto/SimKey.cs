using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cs_dxfAuto {
    class SimulationKeys
    {
        static SimulationKeys skk;
        public static SimulationKeys getsk()
        {
            if (skk == null)
            {
                skk = new SimulationKeys(null, null);
            }
            return skk;
        }

        int pCurrentAttackEmery;//当前攻击敌人的指针
        MemRWer gMrw;
        CallTool.Function fun;
        int targetX, targetY, alwX = 20, alwY = 20;//目标x 目标y 容差x 容差y
        bool firstMovDir;//假为上下 真为左右 优先移动顺序
        Thread t;
        public SimulationKeys(MemRWer g, CallTool.Function f)
        {
            WinIo.KeyUpEx(VKKey.VK_DOWN);
            WinIo.KeyUpEx(VKKey.VK_UP);
            WinIo.KeyUpEx(VKKey.VK_LEFT);
            WinIo.KeyUpEx(VKKey.VK_RIGHT);
            gMrw = g;
            fun = f;
        }
        ~SimulationKeys()
        {
            KeyEvent.fm1.writeLogLine("结束按键");
            t.Abort();
            WinIo.KeyUpEx(VKKey.VK_DOWN);
            WinIo.KeyUpEx(VKKey.VK_UP);
            WinIo.KeyUpEx(VKKey.VK_LEFT);
            WinIo.KeyUpEx(VKKey.VK_RIGHT);
        }
        public void init(MemRWer g, CallTool.Function f) {
            gMrw = g;
            fun = f;
        }

        public void move(int targetX,int targetY,int alwX = 20,int alwY = 20,bool first = false)
        {
            this.targetX = targetX;
            this.targetY = targetY;
            this.alwX = alwX;
            this.alwY = alwY;
            firstMovDir = first;
        }

        int getCurrentAttackEmery()
        {
            Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);
            Int32 map = gMrw.readInt32(chr + 0xC8);
            Int32 dest = gMrw.readInt32(map + 0xC4);
            Int32 x, y, z;
            for (Int32 i = gMrw.readInt32(map + 0xC0); i < dest; i += 4)
            {
                Int32 onobj = gMrw.readInt32(i);
                Int32 zy = gMrw.readInt32(onobj + 0x870);

                Int32 type = gMrw.readInt32(onobj + 0xA4);
                Int32 grope = gMrw.readInt32(onobj + 0x870);

                if (grope == 0)
                    continue;
                if (onobj == gMrw.readInt32(baseAddr.dwBase_Character))
                    continue;
                if (gMrw.readInt32(onobj + 0x3AE4) == 0)
                    continue;
                x = fun.getObjPos(onobj).x;
                y = fun.getObjPos(onobj).y;
                z = fun.getObjPos(onobj).z;
                if (x == 0 || y == 0)
                    continue;
                return onobj;
            }
            return 0;
        }

        public void moveThread()
        {

            if (targetX > 0 && targetY > 0)
            {
                int addr = gMrw.readInt32(baseAddr.dwBase_Character);
                int currX, currY;//当前x ， y
                if (firstMovDir) //上下优先模式
                {
                    if (Math.Abs((currY = fun.getObjPos(addr).y) - targetY) > alwY)//当y轴当前坐标小于容差
                    {
                        if (currY - targetY > 0)// 当前坐标位于目标坐标上边
                            WinIo.KeyPressEx(VKKey.VK_UP);
                        else WinIo.KeyPressEx(VKKey.VK_DOWN);
                    }

                    if (Math.Abs((currX = fun.getObjPos(addr).x) - targetX) > alwX)//当x轴当前坐标小于容差
                    {
                        if (currX - targetX > 0)// 当前坐标位于目标坐标上边
                            WinIo.KeyPressEx(VKKey.VK_LEFT);
                        else WinIo.KeyPressEx(VKKey.VK_RIGHT);
                    }

                    WinIo.KeyUpEx(VKKey.VK_LEFT);
                    WinIo.KeyUpEx(VKKey.VK_RIGHT);
                    WinIo.KeyUpEx(VKKey.VK_DOWN);
                    WinIo.KeyUpEx(VKKey.VK_UP);
                }
                else
                {
                    if (Math.Abs((currX = fun.getObjPos(addr).x) - targetX) > alwX)//当x轴当前坐标小于容差
                    {
                        if (currX - targetX > 0)// 当前坐标位于目标坐标上边
                            WinIo.KeyPressEx(VKKey.VK_LEFT);
                        else WinIo.KeyPressEx(VKKey.VK_RIGHT);
                    }
                    if (Math.Abs((currY = fun.getObjPos(addr).y) - targetY) > alwY)//当y轴当前坐标小于容差
                    {
                        if (currY - targetY > 0)// 当前坐标位于目标坐标上边
                            WinIo.KeyPressEx(VKKey.VK_UP);
                        else WinIo.KeyPressEx(VKKey.VK_DOWN);
                    }
                    WinIo.KeyUpEx(VKKey.VK_LEFT);
                    WinIo.KeyUpEx(VKKey.VK_RIGHT);
                    WinIo.KeyUpEx(VKKey.VK_DOWN);
                    WinIo.KeyUpEx(VKKey.VK_UP);
                }
            }
        }

    }
}
