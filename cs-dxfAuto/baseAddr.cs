using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_dxfAuto {
    public struct baseAddr {

        public static Int32 dwBase_Team = 0x4C98530;

        public static Int32 dwBase_Character = 0x57B6FB0;
        public static Int32 dwBase_Shop = 0x56A3798;
        public static Int32 dwBase_Decryption = 0x57F8A18;
        public static Int32 dwBase_Bag = 0x56A379C;
        public static Int32 dwBase_Quest = 0x56A37D0;
        public static Int32 dwBase_SSS = 0x56A1B74;
        public static Int32 dwCall_SSS = 0x28B7900;
        public static Int32 dwBase_Time = 0x20A028;
        public static Int32 dwBase_TiaoZhan = 0x2EC;
        public static Int32 dwBase_SEND = 0x57ED8D4;
        public static Int32 dwCall_HANDLE = 0x374F280;
        public static Int32 dwCall_ADD = 0x374F390;
        public static Int32 dwCall_SEND = 0x3750AC0;
        public static Int32 dwBase_Encryption = 0x57F8A78;
        public static Int32 dwBase_Encryption_Param1 = 0x4D41DE0;
        public static Int32 dwBase_Encryption_Param2 = 0x4D41FE0;
        public static Int32 dwBase_Role = 0x56A2250;
        public static Int32 dwBase_Max_Pl = 0x57B7258;
        public static Int32 dwBase_Cur_Pl = 0x57B7250;
        public static Int32 dwBase_Mouse = 0x57E3C20;
        public static Int32 dwBase_Quest_Instace_Id = 0x566FD54;
        public static Int32 dwBase_Chara_Level = 0x568605C;
        public static Int32 dwBase_Map_ID = 0x568600C;
        public static Int32 dwCall_CreateEqui = 0x2A32B30;
        public static Int32 dwCall_Buy = 0;
        public static Int32 dwBase_Role_Id = 0x5686044;
        public static Int32 dwCall_UI = 0;
        public static Int32 dwCall_Encryption1 = 0x37DF130;
        public static Int32 dwCall_Encryption2 = 0x37DF1B0;
        public static Int32 dwCall_Encryption3 = 0x37DF230;
        public static Int32 dwCall_Encryption4 = 0x37DF2B0;
        public static Int32 dwCall_Encryption5 = 0x37DF010;
        public static Int32 dwCall_CreateEmery = 0x023CEDA0;//023CEDA0    55              push ebp


        public static Int32 dwBase_KeyPress = 0x6575E78;
        public static Int32 dwCall_LoadCall = 0x3753AA0;
        public static Int32 dwCall_CompleteAllQuest = 0x1C94700;
        public struct Index
        {
            public static Int32 Monster = 0x6744958;
            public static Int32 Equip = 0x673C678;
            public static Int32 Code = 0x6748B8C;
            public static Int32 Character = 0x6733788;
        }
        public struct GetIndexObj
        {
            public static Int32 Monster = 0x56A36E0;
            public static Int32 Equip = 0x56A36E8;
            public static Int32 Code = 0x56A36E4;
            public static Int32 Atk = 0x56A36F8;
            public static Int32 Character = 0x56A36DC;
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

