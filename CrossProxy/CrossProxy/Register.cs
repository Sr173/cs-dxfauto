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
    public partial class Register : Skin_DevExpress
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            tbUser.Text = "";
            tbPwd.Text = "";
            tbSPwd.Text = "";
            tbReCharge.Text = "";
            tbBind.Text = "";

        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            tbUser.Text = "";
            tbPwd.Text = "";
            tbSPwd.Text = "";
            tbReCharge.Text = "";
            tbBind.Text = "";

        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            string sData, sendcmdData;
            sData = "";
            sendcmdData = "";
            sendcmdData = sendcmdData + "<username>" + tbUser.Text.Trim() + "</username>";
            if (tbReCharge.Text.Trim() != "")
            {
                sendcmdData = sendcmdData + "<czkey>" + tbReCharge.Text.Trim() + "</czkey>";
            }

            sendcmdData = sendcmdData + "<password>" + tbPwd.Text.Trim() + "</password>";
            sendcmdData = sendcmdData + "<password2>" + tbSPwd.Text.Trim() + "</password2>";
            sendcmdData = sendcmdData + "<bdinfo>" + tbBind.Text.Trim() + "</bdinfo>";
            sendcmdData = sendcmdData + "<puser>" + "" + "</puser>";
            sData = SoftXLic.KS_CMD("reg", sendcmdData);

            MessageBox.Show(SoftXLic.GD_(sData, "message") + Environment.NewLine + SoftXLic.GD_(sData, "webdata"), "提示");
        }
    }
}
