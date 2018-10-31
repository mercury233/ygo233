using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace YGO233
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!Config.FindYGOProExePath())
            {
                MessageBox.Show("没有找到 YGOPro，请将 YGO233 解压到 YGOPro 文件夹运行。", "YGO233", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.Run(new frmYGO233Main());
        }
    }
}
