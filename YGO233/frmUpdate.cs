using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YGO233
{
    public partial class frmUpdate : Form
    {
        private static string checkUrl = @"http://127.0.0.1/test.json";

        public frmUpdate()
        {
            InitializeComponent();
        }

        public void CheckForYGOProUpdate()
        {
            btnStartUpdate.Visible = false;
            btnCancel.Visible = false;
            progressUpdate.Visible = false;
            Utils.GetStringAsync(checkUrl, CheckYGOProUpdateResult, CheckYGOProUpdateFail);
        }

        public void CheckForYGOProUpdateInBackground()
        {
            Utils.GetStringAsync(checkUrl, CheckYGOProUpdateResultInBackground, CheckYGOProUpdateFailInBackground);
        }

        private int CheckYGOProUpdateResultInBackground(string res)
        {
            CheckYGOProUpdateResult(res);
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            return 0;
        }

        private int CheckYGOProUpdateResult(string res)
        {
            JObject result = (JObject)JsonConvert.DeserializeObject(res);
            var ygoproResult = result["ygopro"];
            var preResult = result["pre"];
            bool haveYGOProUpdate = UInt64.Parse(ygoproResult["version"].ToString()) > (UInt64)Program.Config.GetIntValue("ygopro_version");
            bool havePreUpdate = Program.Config.GetBoolValue("enable_pre_release") && UInt64.Parse(preResult["version"].ToString()) > (UInt64)Program.Config.GetIntValue("pre_release_version");
            if (haveYGOProUpdate && havePreUpdate)
            {
                labelUpdate.Text = String.Format("YGOPro 已于 {0} 发布了更新，先行卡已于 {1} 发布了更新。", ygoproResult["date"], preResult["date"]);
                textUpdateDetails.Text = Utils.FixCRLF(ygoproResult["txt"].ToString() + "\n-----\n" + preResult["txt"].ToString());
            }
            else if (haveYGOProUpdate)
            {
                labelUpdate.Text = String.Format("YGOPro 已于 {0} 发布了更新。", ygoproResult["date"]);
                textUpdateDetails.Text = Utils.FixCRLF(ygoproResult["txt"].ToString());
            }
            else if (havePreUpdate)
            {
                labelUpdate.Text = String.Format("先行卡已于 {1} 发布了更新。", ygoproResult["date"], preResult["date"]);
                textUpdateDetails.Text = Utils.FixCRLF(preResult["txt"].ToString());
            }
            btnStartUpdate.Visible = true;
            btnCancel.Visible = true;
            progressUpdate.Visible = false;
            return 0;
        }

        private int CheckYGOProUpdateFailInBackground(Exception e)
        {
            Application.Exit();
            return 0;
        }

        private int CheckYGOProUpdateFail(Exception e)
        {
            labelUpdate.Text = "检查更新失败！";
            textUpdateDetails.Text = e.Message;
            btnCancel.Visible = true;
            return 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
