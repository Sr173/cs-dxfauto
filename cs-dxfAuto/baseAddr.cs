using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_dxfAuto {
    public struct baseAddr {

        public static Int32 dwBase_Team = 0x4C98530;

        public static Int32 dwBase_Character = 0x57A7B88;
        public static Int32 dwBase_Shop = 0x5694370;
        public static Int32 dwBase_Decryption = 0x57E9580;
        public static Int32 dwBase_Bag = 0x5694374;
        public static Int32 dwBase_Quest = 0x56943A8;
        public static Int32 dwBase_SSS = 0x569274C;
        public static Int32 dwCall_SSS = 0x28B2990;
        public static Int32 dwBase_Time = 0x20A028;
        public static Int32 dwBase_TiaoZhan = 0x2EC;
        public static Int32 dwBase_SEND = 0x57DC43C;
        public static Int32 dwCall_HANDLE = 0x03745C20;
        public static Int32 dwCall_ADD = 0x03745D30;//03744100    55              push ebp
		public static Int32 dwCall_SEND = 0x03747460;
        public static Int32 dwBase_Encryption = 0x57E95E0;
        public static Int32 dwBase_Encryption_Param1 = 0x4D33D40;
        public static Int32 dwBase_Encryption_Param2 = 0x4D33F40;
        public static Int32 dwBase_Role = 0x5692E28;
        public static Int32 dwBase_Max_Pl = 0x57A7E30;
        public static Int32 dwBase_Cur_Pl = 0x57A7E28;
        public static Int32 dwBase_Mouse = 0x57D4788;
        public static Int32 dwBase_Quest_Instace_Id = 0x566092C;
        public static Int32 dwBase_Chara_Level = 0x5676C34;
        public static Int32 dwBase_Map_ID = 0x5676BE4;
        public static Int32 dwCall_CreateEqui = 0x2A2C130;
        public static Int32 dwCall_Buy = 0;
        public static Int32 dwBase_Role_Id = 0x5676C1C;
        public static Int32 dwCall_UI = 0;
        public static Int32 dwCall_Encryption1 = 0x37D5620;
        public static Int32 dwCall_Encryption2 = 0x37D56A0;
        public static Int32 dwCall_Encryption3 = 0x37D5720;
        public static Int32 dwCall_Encryption4 = 0x37D57A0;
        public static Int32 dwCall_Encryption5 = 0x37D5500;
        public static Int32 dwCall_CreateEmery = 0x0;
        public static Int32 dwBase_KeyPress = 0x65669C0;
        public static Int32 dwCall_LoadCall = 0x374A440;
        public static Int32 dwCall_CompleteAllQuest = 0x1C8C4E0;
        public struct Index
        {
            public static Int32 Monster = 0x6735488;
            public static Int32 Equip = 0x672D1E0;
            public static Int32 Code = 0x67396BC;
            public static Int32 Character = 0x67242F0;
        }
        public struct GetIndexObj
        {
            public static Int32 Monster = 0x56942B8;
            public static Int32 Equip = 0x56942C0;
            public static Int32 Code = 0x56942BC;
            public static Int32 Atk = 0x56942D0;
            public static Int32 Character = 0x56942B4;
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

