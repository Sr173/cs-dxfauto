using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using 用户模式;

namespace CrossProxy
{
    class config
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileStringA(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileStringA(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        static private string ininame = @".\config.ini";

        static public bool IsCoorTp = false;
        public struct speed
        {
            static public int gj = 100;
            static public int yd = 100;
            static public int sf = 100;
        }
        public struct weitiao
        {
            public struct gongji
            {
                static public int wl = 100;
                static public int mf = 100;
                static public int dl = 100;
            }

            public struct shuxing
            {
                static public int ll = 100;
                static public int zl = 100;
                static public int js = 100;
                static public int tl = 100;
            }

            public struct baoji
            {
                static public int wuli = 100;
                static public int mofa = 100;
            }
        }

        static public bool IsItemTogther = false;
        static public bool IsHiding = false;

        static bool LoadConfig()
        {
            if (!File.Exists(ininame))
            {
                return false;
            }


            return true;
        }

        private static void w_ini(string Section, string Key, string Value)
        {
            if (Value == "True")
            {
                Value = "1";
            }
            if (Value == "False")
            {
                Value = "0";
            }
            WritePrivateProfileStringA(Section, Key, Value, ininame);
        }

        //读INI         
        public static string r_ini(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(1024);
            int i = GetPrivateProfileStringA(Section, Key, "", temp, 1024, ininame);
            String flag = temp.ToString();
            return flag;
        }

        public static void ReadConfig()
        {
            string flag = "";
            flag = r_ini("顺图配置", "顺图");
            if (flag == "坐标顺图")
                IsCoorTp = true;
            else
                IsCoorTp = false;

            speed.gj = Int32.Parse(r_ini("微调配置", "攻击速度"));
            speed.sf = Int32.Parse(r_ini("微调配置", "释放速度"));
            speed.yd = Int32.Parse(r_ini("微调配置", "移动速度"));
            weitiao.gongji.wl = Int32.Parse(r_ini("微调配置", "物理攻击"));
            weitiao.gongji.mf = Int32.Parse(r_ini("微调配置", "魔法攻击"));
            weitiao.gongji.dl = Int32.Parse(r_ini("微调配置", "独立攻击"));

            weitiao.shuxing.ll = Int32.Parse(r_ini("微调配置", "力量"));
            weitiao.shuxing.zl = Int32.Parse(r_ini("微调配置", "智力"));
            weitiao.shuxing.ll = Int32.Parse(r_ini("微调配置", "体力"));
            weitiao.shuxing.ll = Int32.Parse(r_ini("微调配置", "精神"));

            weitiao.baoji.wuli = Int32.Parse(r_ini("微调配置", "物理暴击"));
            weitiao.baoji.mofa = Int32.Parse(r_ini("微调配置", "魔法暴击"));

            flag = r_ini("入包配置", "入包");
            if (flag == "吸物入包")
                IsItemTogther = true;
            else
                IsItemTogther = false;

            flag = r_ini("无敌配置", "无敌");
            if (flag == "透明无敌")
                IsHiding = true;
            else
                IsHiding = false;

        }

        public static void SaveConfig()
        {
            string flag = "";
            if (IsCoorTp)
                flag = "坐标顺图";
            else
                flag = "强制顺图";

            w_ini("顺图配置", "顺图", flag);

            w_ini("微调配置", "攻击速度", speed.gj.ToString());
            w_ini("微调配置", "释放速度", speed.sf.ToString());
            w_ini("微调配置", "移动速度", speed.yd.ToString());
            w_ini("微调配置", "物理攻击", weitiao.gongji.wl.ToString());
            w_ini("微调配置", "魔法攻击", weitiao.gongji.mf.ToString());
            w_ini("微调配置", "独立攻击", weitiao.gongji.dl.ToString());
            w_ini("微调配置", "力量", weitiao.shuxing.ll.ToString());
            w_ini("微调配置", "智力", weitiao.shuxing.zl.ToString());
            w_ini("微调配置", "体力", weitiao.shuxing.tl.ToString());
            w_ini("微调配置", "精神", weitiao.shuxing.js.ToString());
            w_ini("微调配置", "物理暴击", weitiao.baoji.wuli.ToString());
            w_ini("微调配置", "魔法暴击", weitiao.baoji.mofa.ToString());

            if (IsItemTogther)
                flag = "吸物入包";
            else
                flag = "直接入包";

            w_ini("入包配置", "入包", flag);

            if (IsHiding)
                flag = "透明无敌";
            else
                flag = "锁血无敌";

            w_ini("无敌配置", "无敌", flag);

        }
    }
}
