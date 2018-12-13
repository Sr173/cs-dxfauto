using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_dxfAuto {
    static class Config {
        public static bool IsSellPink = false;
        public static bool IsRoleBig = false;
        public static bool CloseLevelUp = false;
        public static bool Discard = false;
        public static bool IsBindMainThread = true;
        public static Hashtable codeHashHP = new Hashtable();
        public static int way;
        public static Win32.CRITICAL_SECTION CS = new Win32.CRITICAL_SECTION();
        public static bool isHook;
        public static bool sss = false;
        public static bool IsHide = false;
        public static void Init()
        {

            codeHashHP.Add(61272, 2234200);
            codeHashHP.Add(61273, 2234200);
            codeHashHP.Add(61634, 1933919);
            codeHashHP.Add(64022, 2792750);
            codeHashHP.Add(59518, 9793040);
            codeHashHP.Add(56055, 10405100);
            codeHashHP.Add(64016, 2457620);
            codeHashHP.Add(61295, 25135020);

            codeHashHP.Add(56035, 167560);
            codeHashHP.Add(61450, 106200);
            codeHashHP.Add(61454, 80240);
            codeHashHP.Add(56037, 172280);
            codeHashHP.Add(61700, 70800);
            codeHashHP.Add(56038, 177000);
            codeHashHP.Add(61451, 70800);
            codeHashHP.Add(56036, 179360);
            codeHashHP.Add(604, 34475);
            codeHashHP.Add(61806, 420036);

            codeHashHP.Add(1012, 59212);
            codeHashHP.Add(1016, 61060);
            codeHashHP.Add(1013, 62912);
            codeHashHP.Add(50085, 136928);
            codeHashHP.Add(50088, 148032);
            codeHashHP.Add(50089, 251652);
            codeHashHP.Add(50079, 99920);
            codeHashHP.Add(1017, 259044);
            codeHashHP.Add(1014, 333696);
        }

    }
}
