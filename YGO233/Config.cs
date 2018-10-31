using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace YGO233
{
    public static class Config
    {
        public static string YGOProExePath;

        public static bool FindYGOProExePath(string exeName)
        {
            if (File.Exists(exeName))
            {
                YGOProExePath = Path.GetFullPath(exeName);
                return true;
            }
            return false;
        }

        public static bool FindYGOProExePath()
        {
            return FindYGOProExePath("YGOPro.exe");
        }
    }
}
