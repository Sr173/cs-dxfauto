using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 用户模式
{
    class SoftXLic
    {
        #region 引用文件
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileStringA(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileStringA(string section, string key, string def, StringBuilder retVal, int size, string filePath);
  
        private const string FileDLL = @"SoftXLic.dll";
        [DllImport(FileDLL,
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr ks_cmd(string v_cmdName, string v_cmdData);

        /// <summary>
        /// SoftXLic.DLL的只有一个API接口： ks_cmd
        /// </summary>
        /// <param name="v_cmdName">要执行的命令名(区分大小写)</param>
        /// <param name="v_cmdData">根据cmdName的不同，需传入的相应的数据单元或单元集，某些命令cmdData可传空字符串</param>
        /// <returns>有两种格式，原始格式和加密格式。原始格式：就是一个（GBK码）单元集文本，主要包括state和message单元 <state>状态号</state><message>数据</message>加密格式：对原始格式的单元集文本进行了一次base64编码和一次__myEncrypt算法加密， ___myEncrypt(base64_encode(原始格式))解密方法是 base64_decode(__myDecrypt(加密的数据去softhead的值)) 最终就得出原始格式（GBK码）单元集文本</returns>
        public static string KS_CMD(string v_cmdName, string v_cmdData)
        {
            IntPtr result = ks_cmd(v_cmdName, v_cmdData);
            return Marshal.PtrToStringAnsi(result);
        }
        #endregion
        #region 字段
        private static string _lickey = "";
        private static string _softcode = "";
        private static string _softhead = "";
        private static string _clientsoftver = "";
        private static string _username = "";
        private static string _password = "";
        private static string _keystr = "";
        private static string _pccode = "";
        private static string _pccodemode = "";
        private static string _ininame = "";
        private static string _is2web = "";
        private static string _clientid = "";
        private static string _bdinfo = "";
        private static string _cpname = "";
        #endregion
        #region Set方法参数
        /// <summary>
        /// 必填，授权码（定制版无）
        /// </summary>
        public static string Lickey
        {
            get { return "<lickey>" + SoftXLic._lickey + "</lickey>"; }
            set { SoftXLic._lickey = value; }
        }
        /// <summary>
        /// 必填，软件编号
        /// </summary>
        public static string Softcode
        {
            get { return "<softcode>" + SoftXLic._softcode + "</softcode>"; }
            set { SoftXLic._softcode = value; }
        }
        /// <summary>
        /// 必填，加密数据头标识需和服务端软件参数设置里的加密数据头标识相同
        /// 建议用5到8个asc字符 例如: [_Data] [_Head] [_Crypt]
        /// </summary>
        public static string Softhead
        {
            get { return "<softhead>" + SoftXLic._softhead + "</softhead>"; }
            set { SoftXLic._softhead = value; }
        }
        /// <summary>
        /// 必填，客户端软件版本，整数
        /// </summary>
        public static string ClientSoftver
        {
            get { return "<softver>" + SoftXLic._clientsoftver.ToString() + "</softver>"; }
            set { SoftXLic._clientsoftver = value; }
        }
        /// <summary>
        /// 用户名（用户模式软件必填，卡号模式登入的软件，不用设置此值）
        /// </summary>
        public static string Username
        {
            get { return "<username>" + SoftXLic._username + "</username>"; }
            set { SoftXLic._username = value; }
        }
        /// <summary>
        /// 用户密码（用户模式软件必填，卡号模式登 入的软件，不用设置此值）
        /// </summary>
        public static string Password
        {
            get { return "<password>" + SoftXLic._password + "</password>"; }
            set { SoftXLic._password = value; }
        }
        /// <summary>
        /// 注册卡号（卡号模式软件必填，帐号密码模式登入的软件，不用设置此值）
        /// </summary>
        public static string Keystr
        {
            get { return "<keystr>" + SoftXLic._keystr + "</keystr>"; }
            set { SoftXLic._keystr = value; }
        }
        /// <summary>
        /// 可选，机器码（默认值请参考pccodemode，如果你想自定义机器码，就需要设置此参数，设置了此参数后pccodemode无效）
        /// </summary>
        public static string PcCode
        {
            get { return "<pccode>" + SoftXLic._pccode + "</pccode>"; }
            set { SoftXLic._pccode = value; }
        }
        /// <summary>
        /// 可选，默认值为0。 定义取哪些机器码使用哪个物理硬件（如果你定义了pccode，那么此参数无效）
        ///可设置值 0,1,2,3 
        ///0：有网卡取网卡，取网卡出错取硬盘系列号
        ///1：仅网卡 
        ///2：仅硬盘系列号 
        ///3：网卡和硬盘系列号
        /// </summary>
        public static string PcCodeMode
        {
            get { return "<pccodemode>" + SoftXLic._pccodemode + "</pccodemode>"; }
            set { SoftXLic._pccodemode = value; }
        }
        /// <summary>
        /// 可选，默认值c:/x1.ini，自定义记录一些必要的数据的ini文件名
        /// </summary>
        public static string IniName
        {
            get { return "<ininame>" + SoftXLic._ininame + "</ininame>"; }
            set { SoftXLic._ininame = value; }
        }
        /// <summary>
        /// 可选，默认值0，是否有备服，有备服需要设置此值为1
        /// </summary>
        public static string Is2Web
        {
            get { return "<is2web>" + SoftXLic._is2web + "</is2web>"; }
            set { SoftXLic._is2web = value; }
        }
        /// <summary>
        /// 可选，通道号,多通道用户或注册卡需要传入此值，默认为1
        /// </summary>
        public static string ClientId
        {
            get { return "<clientid>" + SoftXLic._clientid + "</clientid>"; }
            set { SoftXLic._clientid = value; }
        }
        /// <summary>
        /// 可选，绑定信息，绑定信息不是机器码是用户自定义的一段用户信息
        /// 不设置此单元的话，系统默认值为空字符串
        /// </summary>
        public static string BdInfo
        {
            get { return "<bdinfo>" + SoftXLic._bdinfo + "</bdinfo>"; }
            set { SoftXLic._bdinfo = value; }
        }
        /// <summary>
        /// 可选，保留单元，默认为GBK
        /// </summary>
        public static string CpName
        {
            get { return "<cpname>" + SoftXLic._cpname + "</cpname>"; }
            set { SoftXLic._cpname = value; }
        }
        #endregion
        /// <summary>
        /// 软件信息初始化
        /// </summary>
        /// <returns></returns>
        public static string Initialize()
        {
            string data = SoftXLic.Lickey
                + SoftXLic.Softcode
                + SoftXLic.Softhead
                + SoftXLic.Softhead
                + SoftXLic.ClientSoftver
                + SoftXLic.IniName;
            string result = SoftXLic.Set(data);
            return result;
        }
        /// <summary>
        /// 设置软件信息
        /// </summary>
        /// <param name="cmdData">可包函的单元</param>
        /// <returns></returns>
        public static string Set(string cmdData)
        {
            string rData=SoftXLic.KS_CMD("set", cmdData);
            return rData;
        }
        /// <summary>
        /// 获取软件在服务器上设置的数据
        /// </summary>
        /// <returns></returns>
        public static string Get()
        {
            string rData = SoftXLic.KS_CMD("get", "");
            return rData;
        }
        /// <summary>
        /// check命令为基础验证功能， 返回信息的原始单元集出错时为原文，验证通过为密文
        /// </summary>
        /// <param name="cmdData">可包函的单元</param>
        /// <returns>
        ///当state等于100时返回的是单元集加密后的数据，需解密(未连接服务器本地验证时返回的是明文)
        ///当state大于100时返回的是单元集是明文单元集无需解密
        /// </returns>
        public static string Check(string cmdData)
        {
            string rData = SoftXLic.KS_CMD("check", cmdData);
            return rData;
        }
        /// <summary>
        ///功能：注册用户帐号，仅用户密码模式软件需使用本接口
        /// </summary>
        /// <param name="cmdData">可包函的单元</param>
        /// <returns>返回信息：是明文单元集</returns>
        public static string Reg(string cmdData)
        {
            string rData = SoftXLic.KS_CMD("reg", cmdData);
            return rData;
        }
        /// <summary>
        ///	功能：给帐号充值，仅用户密码模式软件需使用本接口
        /// </summary>
        /// <param name="cmdData">可包函的单元</param>
        /// <returns>返回信息：是明文单元集</returns>
        public static string CZ(string cmdData)
        {
            string rData = SoftXLic.KS_CMD("cz", cmdData);
            return rData;
        }
        /// <summary>
        ///	功能：查询用户或注册卡信息
        /// </summary>
        /// <param name="cmdData">可包函的单元</param>
        /// <returns>返回信息：是明文单元集</returns>
        public static string Search(string cmdData)
        {
            string rData = SoftXLic.KS_CMD("search", cmdData);
            return rData;
        }
        /// <summary>
        ///	服务端主程序用（即登陆器进程），启动IPC进程通讯服务
        /// </summary>
        /// <param name="cmdData">可包函的单元</param>
        /// <returns>返回信息：是明文单元集</returns>
        public static string IPC_Start(string cmdData)
        {
            string rData = SoftXLic.KS_CMD("ipc_start", cmdData);
            return rData;
        }
        public static string advapi(string advapicmd)
        {
            string sData, randomstr, Srandomstr, errinfo;
            randomstr = SoftXLic.GetRandomString();
            sData = SoftXLic.KS_CMD("check", "<randomstr>" + randomstr + "</randomstr><advapi>" + advapicmd + "</advapi>");
            sData = SoftXLic.FD_(sData);
            if (SoftXLic.GD_(sData, "state") != "100")
            {
                errinfo = SoftXLic.GD_(sData, "message");
                errinfo = errinfo+"---"+SoftXLic.GD_(sData, "webdata");
                return errinfo;
            }
            else
            {
                Srandomstr = SoftXLic.GD_(sData, "randomstr");
                if (Srandomstr != randomstr)
                {
                    SoftXLic.KS_CMD("exit", "");
                }
            }
            return SoftXLic.GD_(sData, "advapi");
        }
        /// <summary>
        /// chkPass功能：对ks_cmd的check命令的基本验证的一个包装函数，里边的效验方法可以自己添加或修改
        /// </summary>
        /// <param name="connect">参数connect：为0时自动判断是否连接服务器，为1时强制连接服务器 </param>
        /// <returns></returns>
        public static string chkPass(int connect = 0)
        {
            string sData, randomstr, Srandomstr, errinfo;
            randomstr = SoftXLic.GetRandomString();
            sData = SoftXLic.KS_CMD("check", "<randomstr>" + randomstr + "</randomstr><connect>" + connect .ToString()+ "</connect>");
            sData = SoftXLic.FD_(sData);
            if (SoftXLic.GD_(sData, "state") != "100")
            {
                errinfo = SoftXLic.GD_(sData, "message");
                errinfo = errinfo + "---" + SoftXLic.GD_(sData, "webdata");
                MessageBox.Show(errinfo, "验证失败");
                SoftXLic.KS_CMD("exit", "");
                return "";
            }
            else
            {
                Srandomstr = SoftXLic.GD_(sData, "randomstr");// '服务端返回的randomstr
                if (Srandomstr != randomstr)//'验证成功，要对数据读取和安全效验了
                {
                    SoftXLic.KS_CMD("exit", "");
                    return "";
                }
            }
            return sData;
        }
        /// <summary>
        /// 扣点函数，调用advapi接口实现扣点功能。成功返回剩余的点数。失败返回-1
        /// </summary>
        /// <param name="v_poins"></param>
        /// <param name="v_errinfo"></param>
        /// <returns></returns>
        public static int kpoints(int v_points, ref string v_errinfo)
        {
            string sData;
            int c;
            sData = SoftXLic.advapi("v_points,"+v_points.ToString());
            c = int.Parse(sData);
            if (c == 0)// ' 返回值不是整数或为0就说明扣点失败
            {
                v_errinfo = sData;
                return -1;
            }
            else
            {
                v_errinfo = "扣点成功";
                return c;
            }
        }
        /// <summary>
        /// '标准RC4算法
        /// </summary>
        /// <param name="spwd"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] rc4(string spwd, byte[] data)
        {
            byte[] key = new byte[256];
            byte[] box = new byte[256];
            byte[] pwd, outbuff;
            int pwd_length, data_length;
            pwd = System.Text.Encoding.Default.GetBytes(spwd);
            pwd_length = pwd.Length;
            data_length = data.Length;
            for (int i = 0; i < 256; i++)
            {
                key[i] = pwd[i % pwd_length];
                box[i] = (byte)i;
            }
            int j = 0;
            for (int i =0;i<256;i++){
                j = (j + box[i] + key[i]) % 256;
                byte tmp;
                tmp = box[i];
                box[i] = box[j];
                box[j]=tmp;
            }
            int a = 0;
            int c,k;
            j = 0;
            outbuff = new byte[data_length];
            for (int i = 0; i < data_length; i++)
            {
                a = (a + 1) % 256;
                j = (j + box[a]) % 256;
                byte tmp;
                tmp = box[a];
                box[a] = box[j];
                box[j] = tmp;
                c = box[a] + box[j];
                k = box[c % 256];
                outbuff[i] = (byte)(data[i] ^ k);
            }
            return outbuff;
        }

        public static string RSADecrypt(string enStr, string publicKey)
        {
            //return SoftXLic.KS_CMD("de64rsa", "<rsaData>" +d + "</rsaData><Modulus>" + m + "</Modulus>");
            string deStr=new RSAHelper(publicKey, KeyFormat.XML).Decrypt(enStr);
            return deStr;
        }
        /// <summary>
        /// 获取xml文档节点值
        /// </summary>
        /// <param name="xmlData">xml文档</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns></returns>
        public static string GD_(string xmlData, string nodeName)
        {
            string strRegex = "(?<=<" + nodeName + ">)[\\s\\S]*?(?=</" + nodeName + ">)";
            return  Regex.Match(xmlData, strRegex).Value;
        }
        /// <summary>
        /// 获取一个随机字符串
        /// </summary>
        /// <returns></returns>
        public static string GetRandomString()
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = "";
            str += "0123456789";
            str += "abcdefghijklmnopqrstuvwxyz";
            str +="ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < 10; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        public static string FD_(string ioData)
        {
            string data_s = "";
            byte[] b64buff;
            if(ioData.StartsWith(SoftXLic._softhead))//检查字符串是否是密文
            {
                ioData = ioData.Replace(SoftXLic._softhead, "");//去除密文标识头
                data_s = SoftXLic._myDecrypt(ioData);
                b64buff = SoftXLic.Base64Decode(data_s);
                return System.Text.Encoding.Default.GetString(b64buff); ;
            }
            else
            {
                if (!ioData.StartsWith("<xml>"))
                {
                    return "<xml><state>140</state><message>DLL内部错误，返回的数据异常" + ioData + "</message></xml>";
                }
            }
            return ioData;
        }
        public static string _myDecrypt(string inData)
        {
            //******下面的代码是服务器上的加密方法
            /* function __myEncrypt($data){

                 if(isset($_GET['r'])){		
                    //客户端设置KS_CMD("set", "<rsa>0</rsa>")会执行下列加密算法 
                     $rc4密钥2 = _rs(2);
                     $rc4密钥3 = _rs(3);		
                     $rc4加密 = rc4byte($rc4密钥2,$data);	
                     $rc4加密 = rc4byte($rc4密钥3,$rc4加密);
                     $base64编码数据 = base64_encode($rc4加密);   //必须base64_encode
                     return $base64编码数据;
                 }else{
                     //客户端不设置KS_CMD("set", "<rsa>0</rsa>")就会执行下列加密算法 
                     $rc4密钥 = _rs(2);     //取ADVAPI字符串资源第二行字符串做为rc4密钥，当然也可以使用make_key()随机生成一个。
                     $rsa加密的rc4密钥 = encode_rsa2(_rs(1),$rc4密钥);//对rc4密钥进行rsa加密，
                     $加密后的数据 = base64_encode(rc4byte($rc4密钥,$data));   //对data数据进行rc4加密，然后再base64编码	
                     $返回数据 = $rsa加密的rc4密钥 . ',' . $加密后的数据;使用‘,’合并密钥和加密数据，然后返回客户端 ，
                     return $返回数据;		
                 }
             }*/
            //*****下面的代码是服务器第一种加密方式进行解密的算法 
            //string rc4key = "";
            //byte[] str1, str2;
            //byte[] base64 = SoftXLic.Base64Decode(inData);
            //rc4key = "FC7fx6KbzwmBJ98pAiEArOz2Kej";
            //str1 = SoftXLic.rc4(rc4key, base64);
            //rc4key = "CIQDirubbjUUwuvO86IaeFFI0oXFa";
            //str2 = SoftXLic.rc4(rc4key, str1);
            //return System.Text.Encoding.Default.GetString(str2);

            //*****下面的代码是服务器第二种加密方式进行解密的算法 
            string publicKey = "<RSAKeyValue><Modulus>wkX6jOTuOc+r2d9XuPybqWWXWCySTdQl33zo8HWoSoKuLz8+HKSpslcoy1MMyw07GA7sclODMZ/m1KtHy/FYrw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            string rc4enkey = "";
            string rc4endata = "";
            
            if (inData.Split(',').Length == 2)
            {
                rc4enkey = inData.Split(',')[0];
                rc4endata = inData.Split(',')[1];
            }
            else
            {
                return "开发者自己设置返回的错误";
            }
            byte[] deData = SoftXLic.Base64Decode(rc4endata);//进行base解码
            string rc4key = SoftXLic.RSADecrypt(rc4enkey, publicKey);//对rc4enkey使用rsa进行解密
            byte[] byteData = SoftXLic.rc4(rc4key, deData);//使用rc4key对deData进行解密
            string data = System.Text.Encoding.Default.GetString(byteData);
            return data;
        }
        /// <summary>
        /// Base64 编码
        /// </summary>
        /// <param name="strData">需要编码的字符串</param>
        /// <returns></returns>
        public static string  Base64Encode(string strData)
        {
            byte[] str = System.Text.Encoding.Default.GetBytes(strData);
            string B64_CHAR_DICT = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
            byte[] buf;
            int length;
            int mods;
            mods = (str.Length) % 3;
            length = str.Length - mods;
            int num = length/3*4+ (mods!=0?4:0);
            buf = new byte[num];

            for (int i = 0; i < length - 1; i+=3)
            {
                buf[i / 3 * 4] = (byte)((str[i] & 252) / 4);
                buf[i / 3 * 4 + 1] = (byte)((str[i] & 3) * 16 + (str[i + 1] & 240) / 16);
                buf[i / 3 * 4 + 2] = (byte)((str[i + 1] & 15) * 4 + (str[i + 2] & 192) / 64);
                buf[i / 3 * 4+3] = (byte)(str[i+2] & 63);
            }
            if (mods == 1)
            {
                buf[length/3*4] =(byte)((str[length]&252)/4);
                buf[length / 3 * 4+1] = (byte)((str[length] & 3) *16);
                buf[length / 3 * 4 + 2] = 64;
                buf[length / 3 * 4 + 3] = 64;

            }
            else if (mods == 2)
            {
                buf[length / 3 * 4] = (byte)((str[length] & 252) / 4);
                buf[length / 3 * 4 + 1] = (byte)((str[length] & 3) * 16 + (str[length+1] & 240) /16);
                buf[length / 3 * 4 + 2] = (byte)((str[length+1] & 15) *4); 
                buf[length / 3 * 4 + 3] = 64;
            }
            string base64 = "";
            for (int i = 0; i < buf.Length; i++)
            {
                base64 = base64 + B64_CHAR_DICT.Substring(buf[i], 1);
            }
            return base64;
        }
        /// <summary>
        /// Base64 解码
        /// </summary>
        /// <param name="b64"></param>
        /// <returns></returns>
        public static byte[] Base64Decode(string b64)
        {
            string B64_CHAR_DICT = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
            byte[] outStr;
            if (b64.IndexOf("=") != -1)
            {
                b64 = b64.Substring(0, b64.IndexOf("="));
            }
            int length, mods;
            mods = b64.Length % 4;
            length = b64.Length - mods;
            outStr=new byte[length/4*3+(mods-1>0?mods-1:0)];
            for (int i = 1; i < length; i += 4)
            {
                byte[] buf = new byte[4];
                for (int j = 0; j <= 3; j++)
                {
                    buf[j] = (byte)(B64_CHAR_DICT.IndexOf(b64.Substring(i + j-1, 1)));
                }
                outStr[(i - 1) / 4 * 3] = (byte)(buf[0] * 4 + (buf[1] & 48) / 16);
                outStr[(i - 1) / 4 * 3+1] = (byte)((buf[1]&15) * 16 + (buf[2] & 60) / 4);
                outStr[(i - 1) / 4 * 3+2] = (byte)((buf[2]&3) * 64 + buf[3]);
            }
            if (mods == 2)
            {
                int num1 = B64_CHAR_DICT.IndexOf(b64.Substring(length, 1)) * 4;
                int num2 = (B64_CHAR_DICT.IndexOf(b64.Substring(length + 1, 1))&48)/16;
                outStr[length / 4 * 3] = (byte)((num1) +(num2));
            }
            else if (mods == 3)
            {
                outStr[length / 4 * 3] = (byte)((B64_CHAR_DICT.IndexOf(b64.Substring(length , 1)) * 4) +
                    (B64_CHAR_DICT.IndexOf(b64.Substring(length + 1, 1)) & 48) / 16);
                outStr[length / 4 * 3+1] = (byte)(((B64_CHAR_DICT.IndexOf(b64.Substring(length + 1, 1))&15) * 16) +
                    (B64_CHAR_DICT.IndexOf(b64.Substring(length + 2, 1)) & 60) / 4);
            }
            string str = System.Text.Encoding.Default.GetString(outStr);
            return outStr;
        }
        //写INI
        public static void w_ini(string Section, string Key, string Value)
        {
            if(Value=="True"){
                Value="1";
            } 
            if(Value=="False")
            {
                Value="0";
            } 
            WritePrivateProfileStringA(Section, Key, Value, SoftXLic._ininame);
        }
        
        //读INI         
        public static string r_ini(string Section, string Key, string defval = "")
        {
            StringBuilder temp = new StringBuilder(1024);
            int i = GetPrivateProfileStringA(Section, Key, "", temp, 1024, SoftXLic._ininame); 
            String flag=temp.ToString();
            if(defval=="False" || defval=="True")
            {
                if(flag=="0" || flag=="False")
                {
                    flag="False";
                }
                else if(flag=="1" || flag=="True")
                {
                    flag="True";
                }
                else
                {
                    flag=defval;
                }
            }
            else
            {
                if(flag=="")
                {
                    flag=defval;
                }
            }
            return flag;
        }
    }
}
