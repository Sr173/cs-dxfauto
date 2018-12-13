using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossProxy
{
    class baseAddr
    {
        public static Int32 dwBase_Character = 0x042902F8;
        public static Int32 dwBase_Decryption = 0x042CEF38;
        public static Int32 dwBase_ExpBox = 0x0416E740;

        public static Int32 dwBase_Quest_Instace_Id = 0x41647EC;
        public static Int32 dwBase_Chara_Level = 0x04177D04;

        public static Int32 dwBase_SEND = 0x42C2510;
        public static Int32 dwCall_SEND = 0x02C1C2F0;
        public static Int32 dwCall_HANDLE = 0x02C1AE00;
        public static Int32 dwCall_ADD = 0x2C1AF00;//02C1AF00    55              push ebp

        public static Int32 dwBase_Quest = 0x04195938;
        public static Int32 dwBase_TiaoZhan = 0x41661F4;
        public static Int32 dwBase_Encryption = 0x042A7A88;
        public static Int32 dwBase_Encryption_Param1 = 0x03CC58D0;
        public static Int32 dwBase_Encryption_Param2 = 0x03CC5AD0;
        public static Int32 dwBase_Time = 0x20A024;
        public static Int32 dwBase_Map_ID = 0x4177CB4;
        public static Int32 dwBase_Boss = 0x055C117C;
        public static Int32 dwBase_Mouse = 0x042B9574;

        public static Int32 dwBase_Max_Pl = 0x04290530;
        public static Int32 dwBase_Cur_Pl = 0x04290528;
        //public static Int32 dwBase_44 = 0x03DE56C4;
        //public static Int32 dwCall_44_Handle = 0x02487C90;//02487C90   /E9 098DD702     jmp 0520099E
        //public static Int32 dwCall_44_Add = 0x03DE56C4;
        public static Int32 dwBase_Role = 0x041944F0;
        public static Int32 dwBase_Qq = 0x0410D210;
        public static Int32 dwBase_Bag = 0x04195908;
        public static Int32 dwBase_Shop = 0x04195904;
        public static Int32 dwOffset_Obj_x = 0x1C0;
    }
}
