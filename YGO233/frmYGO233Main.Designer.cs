namespace YGO233
{
    partial class frmYGO233Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmYGO233Main));
            this.btnFinish = new System.Windows.Forms.Button();
            this.tabPageTools = new System.Windows.Forms.TabPage();
            this.groupBoxTools = new System.Windows.Forms.GroupBox();
            this.btnDonate = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnDeletePre = new System.Windows.Forms.Button();
            this.btnCleanOutdated = new System.Windows.Forms.Button();
            this.btnAssocFiles = new System.Windows.Forms.Button();
            this.btnCheckAllFiles = new System.Windows.Forms.Button();
            this.btnEnablePre = new System.Windows.Forms.Button();
            this.tabPageYGO233Settings = new System.Windows.Forms.TabPage();
            this.groupBoxYGO233Settings = new System.Windows.Forms.GroupBox();
            this.chkYGOProAutoUpdate = new System.Windows.Forms.CheckBox();
            this.chkSkipExistingPic = new System.Windows.Forms.CheckBox();
            this.tabPageYGOProSettings = new System.Windows.Forms.TabPage();
            this.groupBoxYGOProSettings = new System.Windows.Forms.GroupBox();
            this.comboAntiAlias = new System.Windows.Forms.ComboBox();
            this.comboDefaultOT = new System.Windows.Forms.ComboBox();
            this.labelAntiAlias = new System.Windows.Forms.Label();
            this.labelDefaultOT = new System.Windows.Forms.Label();
            this.chkResizePopupMenu = new System.Windows.Forms.CheckBox();
            this.chkEnableBotMode = new System.Windows.Forms.CheckBox();
            this.chkDrawField = new System.Windows.Forms.CheckBox();
            this.chkMouseControlMode = new System.Windows.Forms.CheckBox();
            this.chkErrorlogToFile = new System.Windows.Forms.CheckBox();
            this.chkErrorlogToScreen = new System.Windows.Forms.CheckBox();
            this.chkImgScale = new System.Windows.Forms.CheckBox();
            this.chkDirect3D = new System.Windows.Forms.CheckBox();
            this.tabControlYGO233Main = new System.Windows.Forms.TabControl();
            this.tabPageTools.SuspendLayout();
            this.groupBoxTools.SuspendLayout();
            this.tabPageYGO233Settings.SuspendLayout();
            this.groupBoxYGO233Settings.SuspendLayout();
            this.tabPageYGOProSettings.SuspendLayout();
            this.groupBoxYGOProSettings.SuspendLayout();
            this.tabControlYGO233Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFinish
            // 
            this.btnFinish.Location = new System.Drawing.Point(185, 355);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 23);
            this.btnFinish.TabIndex = 1;
            this.btnFinish.Text = "完成";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // tabPageTools
            // 
            this.tabPageTools.Controls.Add(this.groupBoxTools);
            this.tabPageTools.Location = new System.Drawing.Point(4, 22);
            this.tabPageTools.Name = "tabPageTools";
            this.tabPageTools.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTools.Size = new System.Drawing.Size(246, 320);
            this.tabPageTools.TabIndex = 2;
            this.tabPageTools.Text = "功能";
            this.tabPageTools.UseVisualStyleBackColor = true;
            // 
            // groupBoxTools
            // 
            this.groupBoxTools.Controls.Add(this.btnDonate);
            this.groupBoxTools.Controls.Add(this.btnAbout);
            this.groupBoxTools.Controls.Add(this.btnDeletePre);
            this.groupBoxTools.Controls.Add(this.btnCleanOutdated);
            this.groupBoxTools.Controls.Add(this.btnAssocFiles);
            this.groupBoxTools.Controls.Add(this.btnCheckAllFiles);
            this.groupBoxTools.Controls.Add(this.btnEnablePre);
            this.groupBoxTools.Location = new System.Drawing.Point(6, 6);
            this.groupBoxTools.Name = "groupBoxTools";
            this.groupBoxTools.Size = new System.Drawing.Size(234, 221);
            this.groupBoxTools.TabIndex = 1;
            this.groupBoxTools.TabStop = false;
            this.groupBoxTools.Text = "功能";
            // 
            // btnDonate
            // 
            this.btnDonate.Location = new System.Drawing.Point(121, 175);
            this.btnDonate.Name = "btnDonate";
            this.btnDonate.Size = new System.Drawing.Size(93, 23);
            this.btnDonate.TabIndex = 6;
            this.btnDonate.Text = "捐助";
            this.btnDonate.UseVisualStyleBackColor = true;
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(19, 175);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(93, 23);
            this.btnAbout.TabIndex = 5;
            this.btnAbout.Text = "关于";
            this.btnAbout.UseVisualStyleBackColor = true;
            // 
            // btnDeletePre
            // 
            this.btnDeletePre.Location = new System.Drawing.Point(121, 27);
            this.btnDeletePre.Name = "btnDeletePre";
            this.btnDeletePre.Size = new System.Drawing.Size(93, 23);
            this.btnDeletePre.TabIndex = 4;
            this.btnDeletePre.Text = "删除先行卡";
            this.btnDeletePre.UseVisualStyleBackColor = true;
            // 
            // btnCleanOutdated
            // 
            this.btnCleanOutdated.Location = new System.Drawing.Point(19, 138);
            this.btnCleanOutdated.Name = "btnCleanOutdated";
            this.btnCleanOutdated.Size = new System.Drawing.Size(196, 23);
            this.btnCleanOutdated.TabIndex = 3;
            this.btnCleanOutdated.Text = "清理过期的卡图和脚本";
            this.btnCleanOutdated.UseVisualStyleBackColor = true;
            // 
            // btnAssocFiles
            // 
            this.btnAssocFiles.Location = new System.Drawing.Point(19, 101);
            this.btnAssocFiles.Name = "btnAssocFiles";
            this.btnAssocFiles.Size = new System.Drawing.Size(196, 23);
            this.btnAssocFiles.TabIndex = 2;
            this.btnAssocFiles.Text = "设置双击打开卡组和录像文件";
            this.btnAssocFiles.UseVisualStyleBackColor = true;
            // 
            // btnCheckAllFiles
            // 
            this.btnCheckAllFiles.Location = new System.Drawing.Point(19, 64);
            this.btnCheckAllFiles.Name = "btnCheckAllFiles";
            this.btnCheckAllFiles.Size = new System.Drawing.Size(196, 23);
            this.btnCheckAllFiles.TabIndex = 1;
            this.btnCheckAllFiles.Text = "校验 YGOPro 文件完整性";
            this.btnCheckAllFiles.UseVisualStyleBackColor = true;
            // 
            // btnEnablePre
            // 
            this.btnEnablePre.Location = new System.Drawing.Point(19, 27);
            this.btnEnablePre.Name = "btnEnablePre";
            this.btnEnablePre.Size = new System.Drawing.Size(93, 23);
            this.btnEnablePre.TabIndex = 0;
            this.btnEnablePre.Text = "启用先行卡";
            this.btnEnablePre.UseVisualStyleBackColor = true;
            // 
            // tabPageYGO233Settings
            // 
            this.tabPageYGO233Settings.Controls.Add(this.groupBoxYGO233Settings);
            this.tabPageYGO233Settings.Location = new System.Drawing.Point(4, 22);
            this.tabPageYGO233Settings.Name = "tabPageYGO233Settings";
            this.tabPageYGO233Settings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageYGO233Settings.Size = new System.Drawing.Size(246, 320);
            this.tabPageYGO233Settings.TabIndex = 1;
            this.tabPageYGO233Settings.Text = "YGO233 设置";
            this.tabPageYGO233Settings.UseVisualStyleBackColor = true;
            // 
            // groupBoxYGO233Settings
            // 
            this.groupBoxYGO233Settings.Controls.Add(this.chkYGOProAutoUpdate);
            this.groupBoxYGO233Settings.Controls.Add(this.chkSkipExistingPic);
            this.groupBoxYGO233Settings.Location = new System.Drawing.Point(6, 6);
            this.groupBoxYGO233Settings.Name = "groupBoxYGO233Settings";
            this.groupBoxYGO233Settings.Size = new System.Drawing.Size(234, 73);
            this.groupBoxYGO233Settings.TabIndex = 4;
            this.groupBoxYGO233Settings.TabStop = false;
            this.groupBoxYGO233Settings.Text = "YGO233 设置";
            // 
            // chkYGOProAutoUpdate
            // 
            this.chkYGOProAutoUpdate.AutoSize = true;
            this.chkYGOProAutoUpdate.Location = new System.Drawing.Point(11, 20);
            this.chkYGOProAutoUpdate.Name = "chkYGOProAutoUpdate";
            this.chkYGOProAutoUpdate.Size = new System.Drawing.Size(156, 16);
            this.chkYGOProAutoUpdate.TabIndex = 0;
            this.chkYGOProAutoUpdate.Text = "自动检查 YGOPro 的更新";
            this.chkYGOProAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // chkSkipExistingPic
            // 
            this.chkSkipExistingPic.AutoSize = true;
            this.chkSkipExistingPic.Location = new System.Drawing.Point(11, 43);
            this.chkSkipExistingPic.Name = "chkSkipExistingPic";
            this.chkSkipExistingPic.Size = new System.Drawing.Size(180, 16);
            this.chkSkipExistingPic.TabIndex = 1;
            this.chkSkipExistingPic.Text = "更新卡图时不更新已有的卡图";
            this.chkSkipExistingPic.UseVisualStyleBackColor = true;
            // 
            // tabPageYGOProSettings
            // 
            this.tabPageYGOProSettings.Controls.Add(this.groupBoxYGOProSettings);
            this.tabPageYGOProSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageYGOProSettings.Name = "tabPageYGOProSettings";
            this.tabPageYGOProSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageYGOProSettings.Size = new System.Drawing.Size(246, 320);
            this.tabPageYGOProSettings.TabIndex = 0;
            this.tabPageYGOProSettings.Text = "YGOPro 设置";
            this.tabPageYGOProSettings.UseVisualStyleBackColor = true;
            // 
            // groupBoxYGOProSettings
            // 
            this.groupBoxYGOProSettings.Controls.Add(this.comboAntiAlias);
            this.groupBoxYGOProSettings.Controls.Add(this.comboDefaultOT);
            this.groupBoxYGOProSettings.Controls.Add(this.labelAntiAlias);
            this.groupBoxYGOProSettings.Controls.Add(this.labelDefaultOT);
            this.groupBoxYGOProSettings.Controls.Add(this.chkResizePopupMenu);
            this.groupBoxYGOProSettings.Controls.Add(this.chkEnableBotMode);
            this.groupBoxYGOProSettings.Controls.Add(this.chkDrawField);
            this.groupBoxYGOProSettings.Controls.Add(this.chkMouseControlMode);
            this.groupBoxYGOProSettings.Controls.Add(this.chkErrorlogToFile);
            this.groupBoxYGOProSettings.Controls.Add(this.chkErrorlogToScreen);
            this.groupBoxYGOProSettings.Controls.Add(this.chkImgScale);
            this.groupBoxYGOProSettings.Controls.Add(this.chkDirect3D);
            this.groupBoxYGOProSettings.Location = new System.Drawing.Point(6, 6);
            this.groupBoxYGOProSettings.Name = "groupBoxYGOProSettings";
            this.groupBoxYGOProSettings.Size = new System.Drawing.Size(234, 308);
            this.groupBoxYGOProSettings.TabIndex = 5;
            this.groupBoxYGOProSettings.TabStop = false;
            this.groupBoxYGOProSettings.Text = "YGOPro 设置";
            // 
            // comboAntiAlias
            // 
            this.comboAntiAlias.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAntiAlias.FormattingEnabled = true;
            this.comboAntiAlias.Items.AddRange(new object[] {
            "关",
            "中",
            "高"});
            this.comboAntiAlias.Location = new System.Drawing.Point(118, 231);
            this.comboAntiAlias.Name = "comboAntiAlias";
            this.comboAntiAlias.Size = new System.Drawing.Size(85, 20);
            this.comboAntiAlias.TabIndex = 13;
            this.comboAntiAlias.SelectedIndexChanged += new System.EventHandler(this.comboAntiAlias_SelectedIndexChanged);
            // 
            // comboDefaultOT
            // 
            this.comboDefaultOT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDefaultOT.FormattingEnabled = true;
            this.comboDefaultOT.Items.AddRange(new object[] {
            "OCG",
            "TCG",
            "OCG&TCG"});
            this.comboDefaultOT.Location = new System.Drawing.Point(118, 204);
            this.comboDefaultOT.Name = "comboDefaultOT";
            this.comboDefaultOT.Size = new System.Drawing.Size(85, 20);
            this.comboDefaultOT.TabIndex = 12;
            this.comboDefaultOT.SelectedIndexChanged += new System.EventHandler(this.comboDefaultOT_SelectedIndexChanged);
            // 
            // labelAntiAlias
            // 
            this.labelAntiAlias.AutoSize = true;
            this.labelAntiAlias.Location = new System.Drawing.Point(27, 235);
            this.labelAntiAlias.Name = "labelAntiAlias";
            this.labelAntiAlias.Size = new System.Drawing.Size(65, 12);
            this.labelAntiAlias.TabIndex = 10;
            this.labelAntiAlias.Text = "抗锯齿等级";
            // 
            // labelDefaultOT
            // 
            this.labelDefaultOT.AutoSize = true;
            this.labelDefaultOT.Location = new System.Drawing.Point(27, 208);
            this.labelDefaultOT.Name = "labelDefaultOT";
            this.labelDefaultOT.Size = new System.Drawing.Size(53, 12);
            this.labelDefaultOT.TabIndex = 9;
            this.labelDefaultOT.Text = "默认规则";
            // 
            // chkResizePopupMenu
            // 
            this.chkResizePopupMenu.AutoSize = true;
            this.chkResizePopupMenu.Location = new System.Drawing.Point(11, 181);
            this.chkResizePopupMenu.Name = "chkResizePopupMenu";
            this.chkResizePopupMenu.Size = new System.Drawing.Size(132, 16);
            this.chkResizePopupMenu.TabIndex = 7;
            this.chkResizePopupMenu.Text = "弹出菜单随窗口放大";
            this.chkResizePopupMenu.UseVisualStyleBackColor = true;
            this.chkResizePopupMenu.CheckedChanged += new System.EventHandler(this.chkResizePopupMenu_CheckedChanged);
            // 
            // chkEnableBotMode
            // 
            this.chkEnableBotMode.AutoSize = true;
            this.chkEnableBotMode.Location = new System.Drawing.Point(11, 158);
            this.chkEnableBotMode.Name = "chkEnableBotMode";
            this.chkEnableBotMode.Size = new System.Drawing.Size(96, 16);
            this.chkEnableBotMode.TabIndex = 6;
            this.chkEnableBotMode.Text = "启用人机模式";
            this.chkEnableBotMode.UseVisualStyleBackColor = true;
            this.chkEnableBotMode.CheckedChanged += new System.EventHandler(this.chkEnableBotMode_CheckedChanged);
            // 
            // chkDrawField
            // 
            this.chkDrawField.AutoSize = true;
            this.chkDrawField.Location = new System.Drawing.Point(11, 135);
            this.chkDrawField.Name = "chkDrawField";
            this.chkDrawField.Size = new System.Drawing.Size(132, 16);
            this.chkDrawField.TabIndex = 5;
            this.chkDrawField.Text = "显示场地魔法卡背景";
            this.chkDrawField.UseVisualStyleBackColor = true;
            this.chkDrawField.CheckedChanged += new System.EventHandler(this.chkDrawField_CheckedChanged);
            // 
            // chkMouseControlMode
            // 
            this.chkMouseControlMode.AutoSize = true;
            this.chkMouseControlMode.Location = new System.Drawing.Point(11, 112);
            this.chkMouseControlMode.Name = "chkMouseControlMode";
            this.chkMouseControlMode.Size = new System.Drawing.Size(120, 16);
            this.chkMouseControlMode.TabIndex = 4;
            this.chkMouseControlMode.Text = "使用鼠标控制连锁";
            this.chkMouseControlMode.UseVisualStyleBackColor = true;
            this.chkMouseControlMode.CheckedChanged += new System.EventHandler(this.chkMouseControlMode_CheckedChanged);
            // 
            // chkErrorlogToFile
            // 
            this.chkErrorlogToFile.AutoSize = true;
            this.chkErrorlogToFile.Location = new System.Drawing.Point(11, 89);
            this.chkErrorlogToFile.Name = "chkErrorlogToFile";
            this.chkErrorlogToFile.Size = new System.Drawing.Size(120, 16);
            this.chkErrorlogToFile.TabIndex = 3;
            this.chkErrorlogToFile.Text = "记录脚本错误信息";
            this.chkErrorlogToFile.UseVisualStyleBackColor = true;
            this.chkErrorlogToFile.CheckedChanged += new System.EventHandler(this.chkErrorlogToFile_CheckedChanged);
            // 
            // chkErrorlogToScreen
            // 
            this.chkErrorlogToScreen.AutoSize = true;
            this.chkErrorlogToScreen.Location = new System.Drawing.Point(11, 66);
            this.chkErrorlogToScreen.Name = "chkErrorlogToScreen";
            this.chkErrorlogToScreen.Size = new System.Drawing.Size(120, 16);
            this.chkErrorlogToScreen.TabIndex = 2;
            this.chkErrorlogToScreen.Text = "显示脚本错误信息";
            this.chkErrorlogToScreen.UseVisualStyleBackColor = true;
            this.chkErrorlogToScreen.CheckedChanged += new System.EventHandler(this.chkErrorlogToScreen_CheckedChanged);
            // 
            // chkImgScale
            // 
            this.chkImgScale.AutoSize = true;
            this.chkImgScale.Location = new System.Drawing.Point(11, 43);
            this.chkImgScale.Name = "chkImgScale";
            this.chkImgScale.Size = new System.Drawing.Size(108, 16);
            this.chkImgScale.TabIndex = 1;
            this.chkImgScale.Text = "高画质缩放卡图";
            this.chkImgScale.UseVisualStyleBackColor = true;
            this.chkImgScale.CheckedChanged += new System.EventHandler(this.chkImgScale_CheckedChanged);
            // 
            // chkDirect3D
            // 
            this.chkDirect3D.AutoSize = true;
            this.chkDirect3D.Location = new System.Drawing.Point(11, 20);
            this.chkDirect3D.Name = "chkDirect3D";
            this.chkDirect3D.Size = new System.Drawing.Size(156, 16);
            this.chkDirect3D.TabIndex = 0;
            this.chkDirect3D.Text = "使用 Direct3D 图像模式";
            this.chkDirect3D.UseVisualStyleBackColor = true;
            this.chkDirect3D.CheckedChanged += new System.EventHandler(this.chkDirect3D_CheckedChanged);
            // 
            // tabControlYGO233Main
            // 
            this.tabControlYGO233Main.Controls.Add(this.tabPageYGOProSettings);
            this.tabControlYGO233Main.Controls.Add(this.tabPageYGO233Settings);
            this.tabControlYGO233Main.Controls.Add(this.tabPageTools);
            this.tabControlYGO233Main.Location = new System.Drawing.Point(6, 6);
            this.tabControlYGO233Main.Name = "tabControlYGO233Main";
            this.tabControlYGO233Main.SelectedIndex = 0;
            this.tabControlYGO233Main.Size = new System.Drawing.Size(254, 346);
            this.tabControlYGO233Main.TabIndex = 0;
            // 
            // frmYGO233Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 385);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.tabControlYGO233Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmYGO233Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YGO233 设置";
            this.tabPageTools.ResumeLayout(false);
            this.groupBoxTools.ResumeLayout(false);
            this.tabPageYGO233Settings.ResumeLayout(false);
            this.groupBoxYGO233Settings.ResumeLayout(false);
            this.groupBoxYGO233Settings.PerformLayout();
            this.tabPageYGOProSettings.ResumeLayout(false);
            this.groupBoxYGOProSettings.ResumeLayout(false);
            this.groupBoxYGOProSettings.PerformLayout();
            this.tabControlYGO233Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.TabPage tabPageTools;
        private System.Windows.Forms.GroupBox groupBoxTools;
        private System.Windows.Forms.Button btnEnablePre;
        private System.Windows.Forms.TabPage tabPageYGO233Settings;
        private System.Windows.Forms.GroupBox groupBoxYGO233Settings;
        private System.Windows.Forms.CheckBox chkYGOProAutoUpdate;
        private System.Windows.Forms.CheckBox chkSkipExistingPic;
        private System.Windows.Forms.TabPage tabPageYGOProSettings;
        private System.Windows.Forms.GroupBox groupBoxYGOProSettings;
        private System.Windows.Forms.ComboBox comboAntiAlias;
        private System.Windows.Forms.ComboBox comboDefaultOT;
        private System.Windows.Forms.Label labelAntiAlias;
        private System.Windows.Forms.Label labelDefaultOT;
        private System.Windows.Forms.CheckBox chkResizePopupMenu;
        private System.Windows.Forms.CheckBox chkEnableBotMode;
        private System.Windows.Forms.CheckBox chkDrawField;
        private System.Windows.Forms.CheckBox chkMouseControlMode;
        private System.Windows.Forms.CheckBox chkErrorlogToFile;
        private System.Windows.Forms.CheckBox chkErrorlogToScreen;
        private System.Windows.Forms.CheckBox chkImgScale;
        private System.Windows.Forms.CheckBox chkDirect3D;
        private System.Windows.Forms.TabControl tabControlYGO233Main;
        private System.Windows.Forms.Button btnDeletePre;
        private System.Windows.Forms.Button btnCleanOutdated;
        private System.Windows.Forms.Button btnAssocFiles;
        private System.Windows.Forms.Button btnCheckAllFiles;
        private System.Windows.Forms.Button btnDonate;
        private System.Windows.Forms.Button btnAbout;
    }
}

