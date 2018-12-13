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
using System.IO;

namespace CrossProxy
{
    public partial class mainForm : Skin_DevExpress
    {
        public mainForm()
        {
            InitializeComponent();
        }

//        #region 热键
//        protected override void WndProc(ref Message m)
//        {
//            try
//            {
//                const int WM_HOTKEY = 0x0312;
//                //按快捷键 
//                switch (m.Msg)
//                {
//                    case WM_HOTKEY:
//                        switch (m.WParam.ToInt32())
//                        {
//                            case (Int32)Keys.V:
//                                {
//                                    fun.GatherItem();
//                                    break;
//                                }
//                            case (Int32)Keys.F1:
//                                {
//                                    fun.CreateMercenary();
//                                    break;
//                                }
//                            case (Int32)Keys.Up:
//                                {
//                                    fun.InstanceTp(0);
//                                    break;
//                                }
//                            case (Int32)Keys.Down:
//                                {
//                                    fun.InstanceTp(1);
//                                    break;
//                                }
//                            case (Int32)Keys.Left:
//                                {
//                                    fun.InstanceTp(2);
//                                    break;
//                                }
//                            case (Int32)Keys.Right:
//                                {
//                                    fun.InstanceTp(3);
//                                    break;
//                                }
//                            case (Int32)Keys.F2:
//                                {
//                                    MyKey.MKeyDownUp(0, MyKey.Chr(0x2B));
//                                    break;
//                                }
//                            case (Int32)Keys.F3:
//                                {
//                                    fun.SendKilled();
//                                    //writeLogLine(gMrw.readInt32(baseAddr.dwBase_Character, 0x6058, 0x58, 0xC,0x20).ToString());//490004248
//                                    //fun.SendKilled();
//                                    Int32 chr = gMrw.readInt32(baseAddr.dwBase_Character);
//                                    Int32 _object = gMrw.readInt32(chr + 0xBC);
//                                    Int32 dest = gMrw.readInt32(_object + 0xC4);
//                                    //Int32 x = gMrw.readInt32(chr + 0x1AC);
//                                    //Int32 y = gMrw.readInt32(chr + 0x1B0);
//                                    //Int32 z = gMrw.readInt32(chr + 0x1B4);
//                                    byte[] data = new byte[12];
//                                    data = gMrw.readData((uint)(chr + baseAddr.dwOffset_Obj_x), 12);


//                                    for (int i = gMrw.readInt32(_object + 0xC0); i < dest; i += 4)
//                                    {
//                                        Int32 onobj = gMrw.readInt32(i);
//                                        string name = gMrw.readString(gMrw.readInt32(onobj + 0x4AC));
//                                        writeLogLine(name);
//                                        if (name.IndexOf("MoveMap") >= 0)
//                                            gMrw.writedData((uint)gMrw.readInt32(onobj + 0xAC) + 0x10, data, 12);

//                                        if (gMrw.readInt32(onobj + 0x98) == 0x211)
//                                        {
//                                            gMrw.writedData((uint)gMrw.readInt32(onobj + 0xAC) + 0x10, data, 12);
//                                        }
//                                    }
//                                    break;
//                                }
//                            case (Int32)Keys.F4:
//                                {
//                                    fun.SendKilled();
//                                    //SellGoal();
//                                    //GetRoleEquit();
//                                    break;
//                                }
//                            case (Int32)Keys.F5:
//                                {
//                                    timer2.Stop();
//                                    /*
//                                    if (CharaPos == 0)
//                                        CharaPos = gMrw.readInt32(gMrw.readInt32(baseAddr.dwBase_Role) + 0x13C);
//                                    fun.ChooseChara();
//                                    while (gMrw.readInt32(baseAddr.dwBase_Character) > 0) Thread.Sleep(100);
//                                    fun.EnterChara(++CharaPos);
//                                    while (gMrw.readInt32(baseAddr.dwBase_Character) == 0) Thread.Sleep(100);
//                                    Thread.Sleep(2000);
//                                    //GetRoleEquit();
                                    
//                                    */
//                                    break;
//                                }

//                            case (Int32)Keys.F8:
//                                {
//                                    textBox5.Text = gMrw.Decryption(gMrw.readInt32(0x04153528) + 0x74).ToString();
//                                    break;
//                                }
//                            case (Int32)Keys.Oem3:
//                                {
//                                    gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0x353C + 8, gMrw.readInt32(baseAddr.dwBase_Character, 0x3784 + 8) + 1000);
//                                    gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0x3540 + 8, 1000);

//                                    //mt.PickUp();
//                                    //gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character, 0x6058, 0x58, 0xC) + 0x20, 490004248);
//                                    break;
//                                }
//                            case (Int32)Keys.End:
//                                {
//                                    fun.QuitInstance();
//                                    break;

//                                }

//                            case (Int32)Keys.Delete:
//                                {
//                                    fun.ChooseCard(0, 0);
//                                    break;

//                                }

//                            case (Int32)Keys.Home:
//                                {

//                                    Int32 IsOpen = gMrw.readInt32(baseAddr.dwBase_Character, 0x8A8);

//                                    Int32 _base = gMrw.readInt32(baseAddr.dwBase_Character, 0x2F00);
//                                    if (_base == 0)
//                                    {
//                                        MessageBox.Show("必须穿戴腰带");
//                                        break;
//                                    }

//                                    if (IsOpen == 0)
//                                    {

//                                        gMrw.Encryption(_base + 0x0000096C, 1000);
//                                        gMrw.Encryption(_base + 0x00000974, 500);
//                                        gMrw.Encryption(_base + 0x0000097C, 1000);
//                                        //gMrw.Encryption(_base + 0x000007DC , 1000);


//                                        wd = gMrw.readInt32(baseAddr.dwBase_Character, 0x994);

//                                        gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0x8A8, 1);
//                                        gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0x994, 1);

//                                    }
//                                    else
//                                    {
//                                        gMrw.Encryption(_base + 0x0000096C, 0);
//                                        gMrw.Encryption(_base + 0x00000974, 0);
//                                        gMrw.Encryption(_base + 0x0000097C, 0);

//                                        gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0x8A8, 0);
//                                        gMrw.writeInt32(gMrw.readInt32(baseAddr.dwBase_Character) + 0x994, wd);


//                                    }



//                                    break;
//                                }


//                        }
//                        break;
//                }
//                base.WndProc(ref m);
//            }
//            catch
//            {

//            }
//        }
//#endregion



        private void mainForm_Load(object sender, EventArgs e)
        {
            this.skinListView1.FullRowSelect = true;//是否可以选择行
            this.skinListView1.GridLines = true; //显示表格线

            if (!File.Exists(@".\config.ini"))
                config.SaveConfig();
            else
                config.ReadConfig();

            
           RB_Zbst.Checked = config.IsCoorTp;
           RB_Qzst.Checked = !config.IsCoorTp;

           Rb_Xw.Checked = config.IsItemTogther;
           RB_Zjrb.Checked = !config.IsItemTogther;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gMrw.mhProcess = (uint)Process.OpenProcessByWindowName("地下城与勇士", "地下城与勇士");


        }
    }
}
