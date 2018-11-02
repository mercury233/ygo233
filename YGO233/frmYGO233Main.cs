using System;
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
        private bool YGOProRunning = false;
        private bool loading = false;
        Timer waitYGOProTimer = new Timer();
        YGOProConfig YGOProConfig = new YGOProConfig();

        public frmYGO233Main()
        {
            InitializeComponent();
            LoadConfig();
            WaitYGOProExit(null, null);
            waitYGOProTimer.Interval = 1000;
            waitYGOProTimer.Tick += WaitYGOProExit;
            waitYGOProTimer.Start();
        }

        private void WaitYGOProExit(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("YGOPro");
            if (processes.Length == 0)
            {
                if (YGOProRunning)
                {
                    groupBoxYGOProSettings.Text = "YGOPro 设置";
                    groupBoxYGOProSettings.Enabled = true;
                    YGOProRunning = false;
                    LoadConfig();
                }
            }
            else
            {
                if (!YGOProRunning)
                {
                    groupBoxYGOProSettings.Text = "YGOPro 设置 (YGOPro 关闭时才能修改)";
                    groupBoxYGOProSettings.Enabled = false;
                    YGOProRunning = true;
                }
            }
        }

        private void LoadConfig()
        {
            YGOProConfig.Load();
            loading = true;
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
            chkYGOProAutoUpdate.Checked = Program.Config.GetBoolValue("ygopro_auto_update");
            chkSkipExistingPic.Checked = Program.Config.GetBoolValue("skip_existing_pics_when_updating_ygopro");
            loading = false;
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chkDirect3D_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            YGOProConfig.SetBoolValue("use_d3d", chkDirect3D.Checked);
        }

        private void chkImgScale_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            YGOProConfig.SetBoolValue("use_image_scale", chkImgScale.Checked);
        }

        private void chkErrorlogToScreen_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            YGOProConfig.SetIntValue("errorlog", (chkErrorlogToScreen.Checked ? 1 : 0) | (chkErrorlogToFile.Checked ? 2 : 0));
        }

        private void chkErrorlogToFile_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            chkErrorlogToScreen_CheckedChanged(sender, e);
        }

        private void chkMouseControlMode_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            YGOProConfig.SetBoolValue("control_mode", chkMouseControlMode.Checked);
        }

        private void chkDrawField_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            YGOProConfig.SetBoolValue("draw_field_spell", chkDrawField.Checked);
        }

        private void chkEnableBotMode_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            YGOProConfig.SetBoolValue("enable_bot_mode", chkEnableBotMode.Checked);
        }

        private void chkResizePopupMenu_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            YGOProConfig.SetBoolValue("resize_popup_menu", chkResizePopupMenu.Checked);
        }

        private void comboDefaultOT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            YGOProConfig.SetIntValue("default_ot", comboDefaultOT.SelectedIndex + 1);
        }

        private void comboAntiAlias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            YGOProConfig.SetIntValue("antialias", comboAntiAlias.SelectedIndex);
        }

        private void chkYGOProAutoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            Program.Config.SetBoolValue("ygopro_auto_update", chkYGOProAutoUpdate.Checked);
        }

        private void chkSkipExistingPic_CheckedChanged(object sender, EventArgs e)
        {
            if (loading) return;
            Program.Config.SetBoolValue("skip_existing_pics_when_updating_ygopro", chkSkipExistingPic.Checked);
        }

        private void btnAssocFiles_Click(object sender, EventArgs e)
        {
            if (Utils.AssocFiles())
            {
                MessageBox.Show(frmYGO233Main.ActiveForm, "已完成关联。", "YGO233", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDonate_Click(object sender, EventArgs e)
        {
            Process.Start("https://afdian.net/@ygo233");
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAbout frmAbout = new frmAbout();
            frmAbout.ShowDialog();
        }

        private void btnCleanOutdated_Click(object sender, EventArgs e)
        {
            Utils.CleanOutdatedScriptsAndPics();
        }

        private void btnCheckYGOProUpdate_Click(object sender, EventArgs e)
        {
            frmUpdate frmUpdate = new frmUpdate();
            frmUpdate.CheckForYGOProUpdate();
            frmUpdate.ShowDialog();
        }
    }
}
