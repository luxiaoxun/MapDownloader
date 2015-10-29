using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapDownloader
{
    public class POISearchArgument
    {
        public string Region { set; get; }

        public string Rectangle { set; get; }

        public string KeyWord { set; get; }

        public int MapIndex { set; get; }
    }
}
