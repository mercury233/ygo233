using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.IO;
using System.Data.SQLite;
using System.Diagnostics;
using System.Threading;
using SevenZip;

namespace YGO233
{
    public static class Utils
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        private static MD5 Hasher;

        public static void Init()
        {
            SevenZipBase.SetLibraryPath("7z.dll");
            Hasher = MD5.Create();
            System.Net.ServicePointManager.DefaultConnectionLimit = 20;
        }

        public static bool AssocFiles()
        {
            try
            {
                RegistryKey ydkKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\YGOPro.Deck");
                ydkKey.SetValue("", "YGOPro 卡组");
                ydkKey.CreateSubKey("DefaultIcon").SetValue("", Program.Config.YGOProExePath);
                ydkKey.CreateSubKey(@"shell\open\command").SetValue("", "\"" + Program.Config.YGOProExePath + "\" \"%1\"");
                Registry.CurrentUser.CreateSubKey(@"Software\Classes\.ydk").SetValue("", "YGOPro.Deck");

                RegistryKey yrpKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\YGOPro.Replay");
                yrpKey.SetValue("", "YGOPro 录像");
                yrpKey.CreateSubKey("DefaultIcon").SetValue("", Program.Config.YGOProExePath);
                yrpKey.CreateSubKey(@"shell\open\command").SetValue("", "\"" + Program.Config.YGOProExePath + "\" \"%1\"");
                Registry.CurrentUser.CreateSubKey(@"Software\Classes\.yrp").SetValue("", "YGOPro.Replay");

                const uint SHCNE_ASSOCCHANGED = 0x08000000;
                SHChangeNotify(SHCNE_ASSOCCHANGED, 0, IntPtr.Zero, IntPtr.Zero);

                return true;
            }
            catch (Exception) when (!System.Diagnostics.Debugger.IsAttached)
            {
                // TODO: 好像HKCU一般不需要管理员权限的
                MessageBox.Show(frmYGO233Main.ActiveForm.Visible ? frmYGO233Main.ActiveForm : null, "更新注册表失败，请以管理员权限运行。", "YGO233", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        public static bool CloseYGOPro()
        {
            Process[] processes = Process.GetProcessesByName("YGOPro");
            int tryCount = 0;
            while (tryCount < 30 && processes.Length > 0)
            {
                tryCount++;
                processes[0].CloseMainWindow();
                Thread.Sleep(100);
                processes = Process.GetProcessesByName("YGOPro");
            }
            return processes.Length == 0;
        }

        public static void ExtractFile(string name, string path, Func<string, int> callback)
        {
            SevenZipExtractor ex = new SevenZipExtractor(name);
            ex.ExtractionFinished += (sender, e)=>
            {
                callback(name);
            };
            ex.BeginExtractArchive(path);
        }

        public static string HashFile(string name)
        {
            var hash = Hasher.ComputeHash(File.OpenRead(name));
            return BitConverter.ToString(hash).Replace("-", "").Substring(0, 8).ToLowerInvariant();
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target, Func<int> one)
        {
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            bool skip = Program.Config.GetBoolValue("skip_existing_pics_when_updating_ygopro");
            foreach (FileInfo fi in source.GetFiles())
            {
                one();
                if (!skip || fi.Extension.ToLower() != ".jpg")
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), overwrite: true);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir, one);
            }
        }

        public static void CopyToYGOPro(string source, Func<int> one)
        {
            CopyAll(new DirectoryInfo(source), new DirectoryInfo("temp"), one);
        }

        private static void GetIDsFromCDB(string cdbPath, HashSet<int> list)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + cdbPath);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand("select id from datas", conn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader.GetInt32(0));
            }
        }

        public static void CleanOutdatedScriptsAndPics()
        {
            HashSet<int> knownIDs = new HashSet<int> { };
            GetIDsFromCDB("cards.cdb", knownIDs);
            foreach(string cdb in Directory.GetFiles("expansions", "*.cdb"))
            {
                GetIDsFromCDB(cdb, knownIDs);
            }
            // TODO: 需要知道哪些脚本是先行卡需要的，先鸽了
        }

        public static bool DeleteTempFolder()
        {
            if (File.Exists("ygo233temp"))
            {
                return false;
            }
            if (!Directory.Exists("ygo233temp"))
            {
                return true;
            }
            try
            {
                Directory.Delete("ygo233temp", recursive: true);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static string FixCRLF(string txt)
        {
            return txt.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
        }
    }
}
