using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using System.Text;
using System.Windows.Forms;

namespace YGO233
{
    public static class Utils
    {
        public static void AssocFiles()
        {
            try
            {
                RegistryKey ydkKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\YGOPro.Deck");
                ydkKey.SetValue("", "YGOPro 卡组");
                ydkKey.CreateSubKey("DefaultIcon").SetValue("", Config.YGOProExePath);
                ydkKey.CreateSubKey(@"shell\open\command").SetValue("", "\"" + Config.YGOProExePath + "\" \"%1\"");
                Registry.CurrentUser.CreateSubKey(@"Software\Classes\.ydk").SetValue("", "YGOPro.Deck");

                RegistryKey yrpKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\YGOPro.Replay");
                yrpKey.SetValue("", "YGOPro 录像");
                yrpKey.CreateSubKey("DefaultIcon").SetValue("", Config.YGOProExePath);
                yrpKey.CreateSubKey(@"shell\open\command").SetValue("", "\"" + Config.YGOProExePath + "\" \"%1\"");
                Registry.CurrentUser.CreateSubKey(@"Software\Classes\.yrp").SetValue("", "YGOPro.Replay");
            }
            catch (Exception) when (!System.Diagnostics.Debugger.IsAttached)
            {
                // TODO: 好像HKCU一般不需要管理员权限的
                MessageBox.Show(frmYGO233Main.ActiveForm.Visible ? frmYGO233Main.ActiveForm : null, "更新注册表失败，请以管理员权限运行。", "YGO233", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
