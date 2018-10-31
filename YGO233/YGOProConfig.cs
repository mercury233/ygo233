using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YGO233
{
    public class YGOProConfig : ConfParser
    {
        public YGOProConfig()
        {
        }

        public void Load()
        {
            Load("system.conf");
        }
    }
}
