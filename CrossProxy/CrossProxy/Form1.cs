using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
using 用户模式;

namespace CrossProxy
{
    public partial class Form1 : Skin_DevExpress

    {

        //==============================================
        //★★★★★★IPC进程通讯模式的guid，
        //不为no的话且长度大于5位，此时登陆后会进入进程通讯服务端模式，客户端也要用相同的IPCGuid
        //等于no的话则是普通模式，会载入你自己的程序窗口
        string IPCGuid = "no";//设置的话，只能用a-zA-Z0-9之间的字符
        //===============================================
        //★★★★★★LicKey请设置，X系列LicKey和以前的不一样，如果没有新的请联系可可(下边默认的是月付版一号服务器)
        string lickey = "UCcNlo4LWxoRH7tKD0U+r4Vm4bSRrHQ2fhQI1LI6Z/cHZZIvcGfytdJ9PSK93rcyLya8+KQfaQXGAxIVgPRRXe1mJ+TQxG9w/Rp+W8EBkE6wlcBFnATiHZQMnN2v4cESZv3RNxbueLOiG7FsjuPSa9C51eqRjV4b0CEPEbk6t7axB1IDnXvTqTjhYDg/a3eINJAFZD+VLaM6ZVnEPPJofpsXt/iDEq0Tpg89Xjz+RlDYJ3vYj3shPUsPjpF9gD2ydy6Yc3QBIHkhWDmfRpuCY75nqr0GkwX/hj7iJVUcL6QXwFxpXsQuSrmQCmBkIgKQUULyr3u8svKwWrwHJaOubA==|";

        //月付版二号服务器的lickey
        //string lickey = "k6U5rwYfy5ocaKZGdJ1HlGiZiY+LcrET7mrofj3VK+u8q2egdxlDCyc/OKQaao2oU32t1iJCr4Bm6MSYp7UOWf+Wom/8B0xMhAWx2kRNGwlBD67nrpZaDmeJdLV8g5/OYPopvqKQO5D2FCVPBoK7fPiPRhbSvfWsiw/uGBCV4xEu7wr+jZAaTM2aKkHYHJkJdX3QOXQioBmL/R53v5c3KFsq6BXGgSBB8O5zFrMpH+a0ew92CHhjxD643J6HcxhCOt+Zh/hvhnYB9a93EvGpBFu2wOM57dd1oPDHNgDLzwnUbg2+ldnRfAb9AYEAn2ta+Ivs/0Al+WqDR4YC1EDQSg==|"

        //===============================================
        // ★★★★★★软件版本号 ，请设置
        int clientSoftver = 1;

        //===============================================
        //★★★★★★软件编号，请设置
        string softcode = "1000101";

        //===============================================
        //★★★★★★软件加密数据头标识 ，请设置
        string softhead = "_Data";

        //=============================================== 
        //★★★★★★软件更新日志的URL，只用改前边的域名即可
        string updatelogurl = "http://v9.hphu.com:8080/kss_io/io.php?updatelog=" + "1000101";
        string ininame = @"c:/Asuna.ini";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbUser.Text = "";
            skinTextBox2.Text = "";
            tbPwd.Text = "";

            //-----------------------------------------------------------------

            //===============================================
            //★★★★★★下面进行softxlic参数初始化
            SoftXLic.Lickey = lickey;
            SoftXLic.Softcode = softcode;
            SoftXLic.Softhead = softhead;
            SoftXLic.ClientSoftver = clientSoftver.ToString();
            SoftXLic.IniName = ininame;
            string result = SoftXLic.Initialize();//软件信息初始化，返回数据集，方便调试
                                                  // '下边一行表示自定义解密v__myDecrypt不支持rsa，配合作者测试帐号的高级API用的，不能删除
                                                  //result = SoftXLic.KS_CMD("set", "<rsa>0</rsa>");//添加rsa解密可以删除此行

            //有记住帐号密码，从ini中读取
            cbCheckPwd.Checked = bool.Parse(SoftXLic.r_ini("config", "Rememberaccount", "False"));
            if (cbCheckPwd.Checked)
            {
                tbUser.Text = SoftXLic.r_ini("config", "username", "");
                tbPwd.Text = SoftXLic.r_ini("config", "password", "");
            }
            else
            {
                tbUser.Focus();
            }

            string rData = SoftXLic.Get();//验证软件信息，返回服务器xml数据，需要解析


            if (SoftXLic.GD_(rData, "state").Equals("100"))
            {
                //验证成功

                /*
                state	成功返回100，失败为其它大于100的状态号
                message	取软件信息成功，或出错的具体信息
                pccode1	机器码
                pccode2	仅供参考的附加机器码
                upset	是否强制更新0或1
                softver	服务端设置的软件版本号
                softdownurl	服务端设置的软件下载地址
                softgg	软件公告
                yzpl	验证频率，单位分钟
                */
                string pccode1 = SoftXLic.GD_(rData, "pccode1");
                string pccode2 = SoftXLic.GD_(rData, "pccode2");
                string upset = SoftXLic.GD_(rData, "upset");
                string seversoftver = SoftXLic.GD_(rData, "softver");
                string softdownurl = SoftXLic.GD_(rData, "softdownurl");
                string softgg = SoftXLic.GD_(rData, "softgg");
                string yzpl = SoftXLic.GD_(rData, "yzpl");

            }

        }

        private void skinLabel1_Click(object sender, EventArgs e)
        {

        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            string tmpstr, Srandomstr, errinfo;
            string randomstr = SoftXLic.GetRandomString();
            SoftXLic.Username = tbUser.Text.Trim();
            SoftXLic.Password = tbPwd.Text.Trim();
            SoftXLic.ClientId = "1".Trim();
            SoftXLic.Set(SoftXLic.Username + SoftXLic.Password + SoftXLic.ClientId);
            string fristData = SoftXLic.Check("<randomstr>" + randomstr + "</randomstr>");
            fristData = SoftXLic.FD_(fristData);
            if (SoftXLic.GD_(fristData, "state") != "100")
            {
                errinfo = SoftXLic.GD_(fristData, "message");
                errinfo = errinfo + Environment.NewLine + SoftXLic.GD_(fristData, "webdata");
                MessageBox.Show(errinfo, "登录失败");
            }
            else
            {
                //验证成功，要对数据读取和安全效验了
                Srandomstr = SoftXLic.GD_(fristData, "randomstr");
                //'这里只是做简单的等于比对，更多效验请充分发挥你的脑洞（在例子里写出来就没有什么安全性可言）
                if (randomstr != Srandomstr)
                {
                    SoftXLic.KS_CMD("exit", "");
                    MessageBox.Show("验证失败", "登录失败");
                    this.Close();
                }
                //如果选择了记住密码功能，登陆成功时就把帐号密码写进ini里
                SoftXLic.w_ini("config", "Rememberaccount", cbCheckPwd.Checked.ToString());
                SoftXLic.w_ini("config", "checkupdate", false.ToString());
                if (cbCheckPwd.Checked)
                {
                    SoftXLic.w_ini("config", "username", tbUser.Text);
                    SoftXLic.w_ini("config", "password", tbPwd.Text);
                    SoftXLic.w_ini("config", "clientid", "1");
                }
                tmpstr = "";
                tmpstr = tmpstr + "advapi返回的数据：　" + SoftXLic.GD_(fristData, "advapi") + Environment.NewLine;
                tmpstr = tmpstr + "效验随机串：　" + SoftXLic.GD_(fristData, "randomstr") + Environment.NewLine;
                tmpstr = tmpstr + "用户名：　" + SoftXLic.GD_(fristData, "username") + Environment.NewLine;
                tmpstr = tmpstr + "返回信息A：　" + SoftXLic.GD_(fristData, "InfoA") + Environment.NewLine;
                tmpstr = tmpstr + "返回信息B：　" + SoftXLic.GD_(fristData, "InfoB") + Environment.NewLine;
                tmpstr = tmpstr + "用户时间剩余秒数：　" + SoftXLic.GD_(fristData, "ShengYuMiaoShu") + Environment.NewLine;
                tmpstr = tmpstr + "用户到期日期：　" + SoftXLic.GD_(fristData, "end iftime") + Environment.NewLine;
                tmpstr = tmpstr + "用户天数：　" + SoftXLic.GD_(fristData, "cday") + Environment.NewLine;
                tmpstr = tmpstr + "用户点数：　" + SoftXLic.GD_(fristData, "points") + Environment.NewLine;
                tmpstr = tmpstr + "用户最大通道号：　" + SoftXLic.GD_(fristData, "linknum") + Environment.NewLine;
                tmpstr = tmpstr + "绑定信息：　" + SoftXLic.GD_(fristData, "bdinfo") + Environment.NewLine;
                tmpstr = tmpstr + "用户标签：　" + SoftXLic.GD_(fristData, "tag") + Environment.NewLine;
                tmpstr = tmpstr + "用户附属性：　" + SoftXLic.GD_(fristData, "keyextattr") + Environment.NewLine;
                tmpstr = tmpstr + "用户备注：　" + SoftXLic.GD_(fristData, "BeiZhu") + Environment.NewLine;
                tmpstr = tmpstr + "所属管理ID：　" + SoftXLic.GD_(fristData, "managerid") + Environment.NewLine;
                tmpstr = tmpstr + "充值次数：　" + SoftXLic.GD_(fristData, "cztimes") + Environment.NewLine;
                tmpstr = tmpstr + "用户私有数据：　" + SoftXLic.GD_(fristData, "SiYouShuJu") + Environment.NewLine;
                tmpstr = tmpstr + "是否是公用帐号：　：" + SoftXLic.GD_(fristData, "IsPubUser") + Environment.NewLine;
                tmpstr = tmpstr + "服务器网址：　" + SoftXLic.GD_(fristData, "shostname") + Environment.NewLine;
                tmpstr = tmpstr + "服务器当前时间戮：　" + SoftXLic.GD_(fristData, "shosttime") + Environment.NewLine;
                tmpstr = tmpstr + "自动解绑扣除的时间：　" + SoftXLic.GD_(fristData, "unbind_changetime") + Environment.NewLine;
                tmpstr = tmpstr + "服务端设置的验证频率：　" + SoftXLic.GD_(fristData, "YanZhengPinLv") + Environment.NewLine;
                tmpstr = tmpstr + "服务端返回的机器码：　" + SoftXLic.GD_(fristData, "pccode") + Environment.NewLine;
                MessageBox.Show(tmpstr);

                // '取得了数据，也效验完了，载入你自己的程序窗口吧
                // '你还可以在你自己的程序里边再次通过check命令获取以上的数据
                //new TestForm().Show();//'改为载入你自己的窗口，这里是载入演示窗口
                this.DialogResult = DialogResult.OK;

                mainForm mf = new mainForm();
                this.Hide();
                mf.ShowDialog();
                Application.Exit();
            }
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Register form = new Register();
            form.ShowDialog();
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            Recharge form = new Recharge();
            form.ShowDialog();
        }
    }
}
