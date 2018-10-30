﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace YGO233
{
    public partial class frmYGO233Main : Form
    {
        private bool YGOProRunning = true;
        Timer Timer = new Timer();

        public frmYGO233Main()
        {
            InitializeComponent();
            LoadConfig();
            WaitYGOProExit(null, null);
            Timer.Interval = 1000;
            Timer.Tick += WaitYGOProExit;
            Timer.Start();
        }

        private void WaitYGOProExit(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("YGOPro");
            if (processes.Length == 0)
            {
                groupBoxYGOProSettings.Text = "YGOPro 设置";
                groupBoxYGOProSettings.Enabled = true;
                if (YGOProRunning)
                {
                    YGOProRunning = false;
                    LoadConfig();
                }
            }
            else
            {
                groupBoxYGOProSettings.Text = "YGOPro 设置 (YGOPro 关闭时才能修改)";
                groupBoxYGOProSettings.Enabled = false;
                YGOProRunning = true;
            }
        }

        private void LoadConfig()
        {
            YGOProConfig.Load();
            chkDirect3D.Checked = YGOProConfig.GetBoolValue("use_d3d");
            chkImgScale.Checked = YGOProConfig.GetBoolValue("use_image_scale");
            chkErrorlogToScreen.Checked = (YGOProConfig.GetIntValue("errorlog") & 1) == 1;
            chkErrorlogToFile.Checked = (YGOProConfig.GetIntValue("errorlog") & 2) == 2;
            chkMouseControlMode.Checked = YGOProConfig.GetBoolValue("control_mode");
            chkDrawField.Checked = YGOProConfig.GetBoolValue("draw_field_spell");
            chkEnableBotMode.Checked = YGOProConfig.GetBoolValue("enable_bot_mode");
            chkResizePopupMenu.Checked = YGOProConfig.GetBoolValue("resize_popup_menu");
            comboDefaultOT.SelectedIndex = YGOProConfig.GetIntValue("default_ot") - 1;
            comboAntiAlias.SelectedIndex = YGOProConfig.GetIntValue("antialias");
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chkDirect3D_CheckedChanged(object sender, EventArgs e)
        {
            YGOProConfig.SetBoolValue("use_d3d", chkDirect3D.Checked);
        }

        private void chkImgScale_CheckedChanged(object sender, EventArgs e)
        {
            YGOProConfig.SetBoolValue("use_image_scale", chkImgScale.Checked);
        }

        private void chkErrorlogToScreen_CheckedChanged(object sender, EventArgs e)
        {
            YGOProConfig.SetIntValue("errorlog", (chkErrorlogToScreen.Checked ? 1 : 0) | (chkErrorlogToFile.Checked ? 2 : 0));
        }

        private void chkErrorlogToFile_CheckedChanged(object sender, EventArgs e)
        {
            chkErrorlogToScreen_CheckedChanged(sender, e);
        }

        private void chkMouseControlMode_CheckedChanged(object sender, EventArgs e)
        {
            YGOProConfig.SetBoolValue("control_mode", chkMouseControlMode.Checked);
        }

        private void chkDrawField_CheckedChanged(object sender, EventArgs e)
        {
            YGOProConfig.SetBoolValue("draw_field_spell", chkDrawField.Checked);
        }

        private void chkEnableBotMode_CheckedChanged(object sender, EventArgs e)
        {
            YGOProConfig.SetBoolValue("enable_bot_mode", chkEnableBotMode.Checked);
        }

        private void chkResizePopupMenu_CheckedChanged(object sender, EventArgs e)
        {
            YGOProConfig.SetBoolValue("resize_popup_menu", chkResizePopupMenu.Checked);
        }

        private void comboDefaultOT_SelectedIndexChanged(object sender, EventArgs e)
        {
            YGOProConfig.SetIntValue("default_ot", comboDefaultOT.SelectedIndex + 1);
        }

        private void comboAntiAlias_SelectedIndexChanged(object sender, EventArgs e)
        {
            YGOProConfig.SetIntValue("antialias", comboAntiAlias.SelectedIndex);
        }
    }
}