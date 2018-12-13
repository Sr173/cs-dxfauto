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
    public partial class Recharge : Skin_DevExpress
    {
        public Recharge()
        {
            InitializeComponent();
        }

        private void Recharge_Load(object sender, EventArgs e)
        {

        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            string sData, sendcmdData;
            sData = "";
            sendcmdData = "";
            sendcmdData = sendcmdData + "<username>" + skinTextBox1.Text.Trim() + "</username>";
            sendcmdData = sendcmdData + "<czkey>" + skinTextBox2.Text.Trim() + "</czkey>";
            sData = SoftXLic.KS_CMD("cz", sendcmdData);
            MessageBox.Show(SoftXLic.GD_(sData, "message") + Environment.NewLine + SoftXLic.GD_(sData, "webdata"), "提示");


        }
    }
}
