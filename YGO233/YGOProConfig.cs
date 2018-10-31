using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IniParser;
using IniParser.Model;

namespace YGO233
{
    public static class YGOProConfig
    {
        private static FileIniDataParser parser;
        private static IniData data;

        public static void Load()
        {
            parser = new FileIniDataParser();
            parser.Parser.Configuration.CommentString = "#";
            parser.Parser.Configuration.NewLineStr = "\n";
            data = parser.ReadFile("system.conf");
        }

        private static void Save()
        {
            parser.WriteFile("system.conf", data);
        }

        public static string GetStringValue(string key, string _default = "")
        {
            return data.Global[key] ?? _default;
        }

        public static void SetStringValue(string key, string value)
        {
            data.Global[key] = value;
            Save();
        }

        public static bool GetBoolValue(string key, bool _default = false)
        {
            int val;
            if (int.TryParse(data.Global[key], out val))
                return val > 0;
            else
                return _default;
        }

        public static void SetBoolValue(string key, bool value)
        {
            data.Global[key] = value ? "1" : "0";
            Save();
        }

        public static int GetIntValue(string key, int _default = 0)
        {
            int val;
            if (int.TryParse(data.Global[key], out val))
                return val;
            else
                return _default;
        }

        public static void SetIntValue(string key, int value)
        {
            data.Global[key] = value.ToString();
            Save();
        }
    }
}
