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
using System.IO;
using System.Diagnostics;

namespace YGO233
{
    public partial class frmUpdate : Form
    {
        private static string checkUrl = @"http://127.0.0.1/test.json";
        private static string dataUrl = @"http://127.0.0.1/data.7z";

        public List<string> filesToDownload;
        public List<string> packagesToDownload ;


        public frmUpdate()
        {
            InitializeComponent();
        }

        public void CheckForYGOProUpdate()
        {
            btnStartUpdate.Visible = false;
            btnCancel.Visible = false;
            btnFinish.Visible = false;
            progressUpdate.Visible = false;
            Downloader.GetStringAsync(checkUrl, CheckYGOProUpdateResult, CheckYGOProUpdateFail);
        }

        public void CheckForYGOProUpdateInBackground()
        {
            Downloader.GetStringAsync(checkUrl, CheckYGOProUpdateResultInBackground, CheckYGOProUpdateFailInBackground);
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
            bool haveYGOProUpdate = UInt64.Parse(ygoproResult["version"].ToString()) > Program.Config.GetUInt64Value("ygopro_version");
            bool havePreUpdate = Program.Config.GetBoolValue("enable_pre_release") && UInt64.Parse(preResult["version"].ToString()) > Program.Config.GetUInt64Value("pre_release_version");
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
            else
            {
                labelUpdate.Text = "没有找到更新。";
                textUpdateDetails.Text = "";
            }
            if (haveYGOProUpdate || havePreUpdate)
            {
                btnStartUpdate.Visible = true;
                btnCancel.Visible = true;
                btnFinish.Visible = false;
            }
            else
            {
                btnStartUpdate.Visible = false;
                btnCancel.Visible = false;
                btnFinish.Visible = true;
            }
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
            btnFinish.Visible = true;
            return 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnStartUpdate_Click(object sender, EventArgs e)
        {
            if (!Utils.CloseYGOPro())
            {
                MessageBox.Show(frmUpdate.ActiveForm, "关闭 YGOPro 失败，请手动关闭或稍后重试。", "YGO233", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            labelUpdate.Text = "正在获取更新信息...";
            btnStartUpdate.Visible = false;
            btnCancel.Visible = false;
            btnFinish.Visible = false;

            progressUpdate.Value = 0;
            progressUpdate.Maximum = 100;
            progressUpdate.Visible = true;

            Directory.CreateDirectory("temp");
            Downloader.DownloadFileAsync(dataUrl, @"temp\data.7z", @"temp\data.7z", ParseUpdateData, DownloadFail);
        }

        private int DownloadFail(Exception e)
        {
            labelUpdate.Text = "下载失败！";
            textUpdateDetails.Text = e.Message;
            btnFinish.Visible = true;
            return 0;
        }

        private int ParseUpdateData(string name)
        {
            progressUpdate.Value = 40;
            Utils.ExtractFile(name, "temp", ParseUpdateDataStep2);
            return 0;
        }

        private int ParseUpdateDataStep2(string name)
        {
            progressUpdate.Value = 50;
            filesToDownload = new List<string>();
            packagesToDownload = new List<string>();

            JObject fileDatas = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(@"temp\files.json"));
            var files = fileDatas["files"];
            files.ToList().ForEach(file=> {
                Application.DoEvents();
                string filename = file["name"].ToString();
                if (!File.Exists(filename) || new FileInfo(filename).Length != Int64.Parse(file["size"].ToString()))
                {
                    filesToDownload.Add(filename);
                }
            });
            var folders = fileDatas["folders"];
            folders.ToList().ForEach(folder=> {
                var foldername = folder.ToObject<JProperty>().Name + @"\";
                var ffiles = folder.First.ToList();
                ffiles.ForEach(file=> {
                    Application.DoEvents();
                    string filename = foldername + file["name"].ToString();
                    //if (!File.Exists(filename) || Utils.HashFile(filename) != file["hash"].ToString())
                    if (!File.Exists(filename) || new FileInfo(filename).Length != Int64.Parse(file["size"].ToString()))
                    {
                        filesToDownload.Add(filename);
                    }
                });
            });
            //Debug.Write(String.Join("\n", filesToDownload));

            JObject packageDatas = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(@"temp\packages.json"));
            var packages = packageDatas["packages"];
            packages.ToList().ForEach(package=> {
                var packageFiles = package["files"].ToList();
                int count = 0;
                packageFiles.ForEach(packageFile=> {
                    if (filesToDownload.Contains(packageFile.ToString()))
                    {
                        count++;
                    }
                });
                if (count >= 100 || count >= packageFiles.Count * 0.66)
                {
                    packagesToDownload.Add(package["filename"].ToString());
                    packageFiles.ForEach(packageFile => {
                        filesToDownload.Remove(packageFile.ToString());
                    });
                }
            });
            //Debug.Write(String.Join("\n", packagesToDownload));

            progressUpdate.Value = 100;
            return 0;
        }
    }
}
