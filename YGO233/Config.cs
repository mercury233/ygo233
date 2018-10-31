using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace YGO233
{
    public class Config : ConfParser
    {
        public string YGOProExePath;

        public Config()
        {
        }

        public void Load()
        {
            string filename = "ygo233.conf";
            if (!File.Exists(filename))
                Init();
            else
                Load(filename);
        }

        private void Init()
        {
            string filename = "ygo233.conf";
            File.WriteAllText(filename, "#");
            Load(filename);
            SetBoolValue("ygopro_auto_update", true);
            SetBoolValue("skip_existing_pics_when_updating_ygopro", false);
        }

        public bool FindYGOProExePath(string exeName)
        {
            if (File.Exists(exeName))
            {
                YGOProExePath = Path.GetFullPath(exeName);
                return true;
            }
            return false;
        }

        public bool FindYGOProExePath()
        {
            return FindYGOProExePath("YGOPro.exe");
        }
    }
}
