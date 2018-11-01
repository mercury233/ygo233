using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace YGO233
{
    static class Program
    {
        public static Config Config;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Config = new Config();
            if (!Config.FindYGOProExePath())
            {
                MessageBox.Show("没有找到 YGOPro，请将 YGO233 解压到 YGOPro 文件夹运行。", "YGO233", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
            if (args.Count() > 0)
            {
                if (args[0] == "/assoc")
                {
                    return Utils.AssocFiles() ? 0 : 2;
                }
            }
            Config.Load();
            Application.Run(new frmYGO233Main());
            return 0;
        }
    }
}
