namespace CrossProxy
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.skinButton1 = new CCWin.SkinControl.SkinButton();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.tbUser = new CCWin.SkinControl.SkinTextBox();
            this.skinTextBox2 = new CCWin.SkinControl.SkinTextBox();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.tbPwd = new CCWin.SkinControl.SkinTextBox();
            this.skinButton2 = new CCWin.SkinControl.SkinButton();
            this.skinButton3 = new CCWin.SkinControl.SkinButton();
            this.cbCheckPwd = new CCWin.SkinControl.SkinCheckBox();
            this.skinCheckBox2 = new CCWin.SkinControl.SkinCheckBox();
            this.tbUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinButton1
            // 
            this.skinButton1.BackColor = System.Drawing.Color.Transparent;
            this.skinButton1.BorderColor = System.Drawing.Color.Cyan;
            this.skinButton1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton1.DownBack = null;
            this.skinButton1.Location = new System.Drawing.Point(128, 193);
            this.skinButton1.MouseBack = null;
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.NormlBack = null;
            this.skinButton1.Size = new System.Drawing.Size(55, 23);
            this.skinButton1.TabIndex = 0;
            this.skinButton1.Text = "登陆";
            this.skinButton1.UseVisualStyleBackColor = false;
            this.skinButton1.Click += new System.EventHandler(this.skinButton1_Click);
            // 
            // skinLabel1
            // 
            this.skinLabel1.AutoSize = true;
            this.skinLabel1.BackColor = System.Drawing.SystemColors.Window;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            this.skinLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel1.Location = new System.Drawing.Point(23, 53);
            this.skinLabel1.Name = "skinLabel1";
            this.skinLabel1.Size = new System.Drawing.Size(56, 17);
            this.skinLabel1.TabIndex = 1;
            this.skinLabel1.Text = "用户名：";
            this.skinLabel1.Click += new System.EventHandler(this.skinLabel1_Click);
            // 
            // tbUser
            // 
            this.tbUser.BackColor = System.Drawing.Color.Transparent;
            this.tbUser.Controls.Add(this.skinTextBox2);
            this.tbUser.DownBack = null;
            this.tbUser.Icon = null;
            this.tbUser.IconIsButton = false;
            this.tbUser.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.tbUser.IsPasswordChat = '\0';
            this.tbUser.IsSystemPasswordChar = false;
            this.tbUser.Lines = new string[0];
            this.tbUser.Location = new System.Drawing.Point(96, 53);
            this.tbUser.Margin = new System.Windows.Forms.Padding(0);
            this.tbUser.MaxLength = 32767;
            this.tbUser.MinimumSize = new System.Drawing.Size(28, 28);
            this.tbUser.MouseBack = null;
            this.tbUser.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.tbUser.Multiline = false;
            this.tbUser.Name = "tbUser";
            this.tbUser.NormlBack = null;
            this.tbUser.Padding = new System.Windows.Forms.Padding(5);
            this.tbUser.ReadOnly = false;
            this.tbUser.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbUser.Size = new System.Drawing.Size(170, 28);
            // 
            // 
            // 
            this.tbUser.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbUser.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbUser.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.tbUser.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.tbUser.SkinTxt.Name = "BaseText";
            this.tbUser.SkinTxt.Size = new System.Drawing.Size(160, 18);
            this.tbUser.SkinTxt.TabIndex = 0;
            this.tbUser.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tbUser.SkinTxt.WaterText = "";
            this.tbUser.TabIndex = 2;
            this.tbUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbUser.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tbUser.WaterText = "";
            this.tbUser.WordWrap = true;
            // 
            // skinTextBox2
            // 
            this.skinTextBox2.BackColor = System.Drawing.Color.Transparent;
            this.skinTextBox2.DownBack = null;
            this.skinTextBox2.Icon = null;
            this.skinTextBox2.IconIsButton = false;
            this.skinTextBox2.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox2.IsPasswordChat = '\0';
            this.skinTextBox2.IsSystemPasswordChar = false;
            this.skinTextBox2.Lines = new string[0];
            this.skinTextBox2.Location = new System.Drawing.Point(-2, 46);
            this.skinTextBox2.Margin = new System.Windows.Forms.Padding(0);
            this.skinTextBox2.MaxLength = 32767;
            this.skinTextBox2.MinimumSize = new System.Drawing.Size(28, 28);
            this.skinTextBox2.MouseBack = null;
            this.skinTextBox2.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox2.Multiline = false;
            this.skinTextBox2.Name = "skinTextBox2";
            this.skinTextBox2.NormlBack = null;
            this.skinTextBox2.Padding = new System.Windows.Forms.Padding(5);
            this.skinTextBox2.ReadOnly = false;
            this.skinTextBox2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.skinTextBox2.Size = new System.Drawing.Size(145, 28);
            // 
            // 
            // 
            this.skinTextBox2.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skinTextBox2.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTextBox2.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.skinTextBox2.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.skinTextBox2.SkinTxt.Name = "BaseText";
            this.skinTextBox2.SkinTxt.Size = new System.Drawing.Size(135, 18);
            this.skinTextBox2.SkinTxt.TabIndex = 0;
            this.skinTextBox2.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinTextBox2.SkinTxt.WaterText = "";
            this.skinTextBox2.TabIndex = 3;
            this.skinTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.skinTextBox2.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinTextBox2.WaterText = "";
            this.skinTextBox2.WordWrap = true;
            // 
            // skinLabel2
            // 
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.SystemColors.Window;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.Location = new System.Drawing.Point(24, 101);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(44, 17);
            this.skinLabel2.TabIndex = 1;
            this.skinLabel2.Text = "密码：";
            // 
            // tbPwd
            // 
            this.tbPwd.BackColor = System.Drawing.Color.Transparent;
            this.tbPwd.DownBack = null;
            this.tbPwd.Icon = null;
            this.tbPwd.IconIsButton = false;
            this.tbPwd.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.tbPwd.IsPasswordChat = '*';
            this.tbPwd.IsSystemPasswordChar = false;
            this.tbPwd.Lines = new string[] {
        "skinTextBox3"};
            this.tbPwd.Location = new System.Drawing.Point(96, 97);
            this.tbPwd.Margin = new System.Windows.Forms.Padding(0);
            this.tbPwd.MaxLength = 32767;
            this.tbPwd.MinimumSize = new System.Drawing.Size(28, 28);
            this.tbPwd.MouseBack = null;
            this.tbPwd.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.tbPwd.Multiline = false;
            this.tbPwd.Name = "tbPwd";
            this.tbPwd.NormlBack = null;
            this.tbPwd.Padding = new System.Windows.Forms.Padding(5);
            this.tbPwd.ReadOnly = false;
            this.tbPwd.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbPwd.Size = new System.Drawing.Size(170, 28);
            // 
            // 
            // 
            this.tbPwd.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPwd.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPwd.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.tbPwd.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.tbPwd.SkinTxt.Name = "BaseText";
            this.tbPwd.SkinTxt.PasswordChar = '*';
            this.tbPwd.SkinTxt.Size = new System.Drawing.Size(160, 18);
            this.tbPwd.SkinTxt.TabIndex = 0;
            this.tbPwd.SkinTxt.Text = "skinTextBox3";
            this.tbPwd.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tbPwd.SkinTxt.WaterText = "";
            this.tbPwd.TabIndex = 3;
            this.tbPwd.Text = "skinTextBox3";
            this.tbPwd.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbPwd.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.tbPwd.WaterText = "";
            this.tbPwd.WordWrap = true;
            // 
            // skinButton2
            // 
            this.skinButton2.BackColor = System.Drawing.Color.Transparent;
            this.skinButton2.BorderColor = System.Drawing.Color.White;
            this.skinButton2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton2.DownBack = null;
            this.skinButton2.Location = new System.Drawing.Point(55, 193);
            this.skinButton2.MouseBack = null;
            this.skinButton2.Name = "skinButton2";
            this.skinButton2.NormlBack = null;
            this.skinButton2.Size = new System.Drawing.Size(55, 23);
            this.skinButton2.TabIndex = 4;
            this.skinButton2.Text = "注册";
            this.skinButton2.UseVisualStyleBackColor = false;
            this.skinButton2.Click += new System.EventHandler(this.skinButton2_Click);
            // 
            // skinButton3
            // 
            this.skinButton3.BackColor = System.Drawing.Color.Transparent;
            this.skinButton3.BorderColor = System.Drawing.Color.White;
            this.skinButton3.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton3.DownBack = null;
            this.skinButton3.Location = new System.Drawing.Point(199, 193);
            this.skinButton3.MouseBack = null;
            this.skinButton3.Name = "skinButton3";
            this.skinButton3.NormlBack = null;
            this.skinButton3.Size = new System.Drawing.Size(55, 23);
            this.skinButton3.TabIndex = 5;
            this.skinButton3.Text = "充值";
            this.skinButton3.UseVisualStyleBackColor = false;
            this.skinButton3.Click += new System.EventHandler(this.skinButton3_Click);
            // 
            // cbCheckPwd
            // 
            this.cbCheckPwd.AutoSize = true;
            this.cbCheckPwd.BackColor = System.Drawing.Color.Transparent;
            this.cbCheckPwd.Checked = true;
            this.cbCheckPwd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCheckPwd.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.cbCheckPwd.DownBack = null;
            this.cbCheckPwd.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbCheckPwd.Location = new System.Drawing.Point(67, 148);
            this.cbCheckPwd.MouseBack = null;
            this.cbCheckPwd.Name = "cbCheckPwd";
            this.cbCheckPwd.NormlBack = null;
            this.cbCheckPwd.SelectedDownBack = null;
            this.cbCheckPwd.SelectedMouseBack = null;
            this.cbCheckPwd.SelectedNormlBack = null;
            this.cbCheckPwd.Size = new System.Drawing.Size(75, 21);
            this.cbCheckPwd.TabIndex = 6;
            this.cbCheckPwd.Text = "记住密码";
            this.cbCheckPwd.UseVisualStyleBackColor = false;
            // 
            // skinCheckBox2
            // 
            this.skinCheckBox2.AutoSize = true;
            this.skinCheckBox2.BackColor = System.Drawing.Color.Transparent;
            this.skinCheckBox2.Checked = true;
            this.skinCheckBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.skinCheckBox2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinCheckBox2.DownBack = null;
            this.skinCheckBox2.Enabled = false;
            this.skinCheckBox2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinCheckBox2.Location = new System.Drawing.Point(157, 147);
            this.skinCheckBox2.MouseBack = null;
            this.skinCheckBox2.Name = "skinCheckBox2";
            this.skinCheckBox2.NormlBack = null;
            this.skinCheckBox2.SelectedDownBack = null;
            this.skinCheckBox2.SelectedMouseBack = null;
            this.skinCheckBox2.SelectedNormlBack = null;
            this.skinCheckBox2.Size = new System.Drawing.Size(75, 21);
            this.skinCheckBox2.TabIndex = 6;
            this.skinCheckBox2.Text = "自动登陆";
            this.skinCheckBox2.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(315, 237);
            this.Controls.Add(this.skinCheckBox2);
            this.Controls.Add(this.cbCheckPwd);
            this.Controls.Add(this.skinButton3);
            this.Controls.Add(this.skinButton2);
            this.Controls.Add(this.tbPwd);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.skinLabel2);
            this.Controls.Add(this.skinLabel1);
            this.Controls.Add(this.skinButton1);
            this.InnerBorderColor = System.Drawing.Color.Black;
            this.Name = "Form1";
            this.Text = "Asuna绿色脚本";
            this.TitleCenter = false;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tbUser.ResumeLayout(false);
            this.tbUser.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinButton skinButton1;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinTextBox tbUser;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinTextBox skinTextBox2;
        private CCWin.SkinControl.SkinTextBox tbPwd;
        private CCWin.SkinControl.SkinButton skinButton2;
        private CCWin.SkinControl.SkinButton skinButton3;
        private CCWin.SkinControl.SkinCheckBox cbCheckPwd;
        private CCWin.SkinControl.SkinCheckBox skinCheckBox2;
    }
}

