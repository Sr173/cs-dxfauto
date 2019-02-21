using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_dxfAuto {
    public struct baseAddr {

        public static Int32 dwBase_Team = 0x4C98530;

        public static Int32 dwBase_Character = 0x5F60950;
        public static Int32 dwBase_Shop = 0x5E44E84;
        public static Int32 dwBase_Decryption = 0x5FA3098;
        public static Int32 dwBase_Bag = 0x5E44E88;
        public static Int32 dwBase_Quest = 0x5E44EBC;
        public static Int32 dwBase_SSS = 0x5E4326C;
        public static Int32 dwCall_SSS = 0x25594C0;
        public static Int32 dwBase_Time = 0x20A030;
        public static Int32 dwBase_TiaoZhan = 0x2EC;
        public static Int32 dwBase_SEND = 0x5F97E44;
        public static Int32 dwCall_HANDLE = 0x3404E90;
        public static Int32 dwCall_ADD = 0x3404FA0;
        public static Int32 dwCall_SEND = 0x34066D0;
        public static Int32 dwBase_Encryption = 0x5FA30F8;
        public static Int32 dwBase_Encryption_Param1 = 0x524B198;
        public static Int32 dwBase_Encryption_Param2 = 0x524B398;
        public static Int32 dwBase_Role = 0x5E4392C;
        public static Int32 dwBase_Max_Pl = 0x5F60BFC;
        public static Int32 dwBase_Cur_Pl = 0x5F60BF4;
        public static Int32 dwBase_Mouse = 0x5F8E0A0;
        public static Int32 dwBase_Quest_Instace_Id = 0x5E11114;
        public static Int32 dwBase_Chara_Level = 0x5E27404;
        public static Int32 dwBase_Map_ID = 0x5E273B4;
        public static Int32 dwCall_CreateEqui = 0x26D7C60;
        public static Int32 dwCall_Buy = 0;
        public static Int32 dwBase_Role_Id = 0x5E273EC;
        public static Int32 dwCall_UI = 0;
        public static Int32 dwCall_Encryption1 = 0x3490E60;
        public static Int32 dwCall_Encryption2 = 0x3490EE0;
        public static Int32 dwCall_Encryption3 = 0x3490F60;
        public static Int32 dwCall_Encryption4 = 0x3490FE0;
        public static Int32 dwCall_Encryption5 = 0x3490D40;
        public static Int32 dwCall_CreateEmery = 0x0;
        public static Int32 dwBase_KeyPress = 0x5FC4B30;
        public static Int32 dwCall_LoadCall = 0x3407710;
        public static Int32 dwCall_CompleteAllQuest = 0x1913C20;
        public struct Index
        {
            public static Int32 Monster = 0x6EE0B8C;
            public static Int32 Equip = 0x6EE9E20;
            public static Int32 Code = 0x6EF55B0;
            public static Int32 Character = 0x6EDFA8C;
        }
        public struct GetIndexObj
        {
            public static Int32 Monster = 0x5E44DC8;
            public static Int32 Equip = 0x5E44DD0;
            public static Int32 Code = 0x5E44DCC;
            public static Int32 Atk = 0x5E44DE0;
            public static Int32 Character = 0x5E44DC4;
        }



        public static Int32 dwBase_Gabriel = 0x43774A4;
        public static Int32 dwBase_Rpcs = 0;
        public static Int32 dwBase_UserID = 0;
        public static Int32 dwBase_Boss = 0x4320A2C;
        //public static Int32 dwOffset_Obj_Pos = 0x1D0;
        public static Int32 dwOffset_Equip_wq = 0x3244;
        public static Int32 GameRpcs;

    }
}

