using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.ComponentModel;

namespace GMapPOI
{
    public class PoiData
    {
        [DisplayName("名称")] 
        public string Name { set; get; }

        [DisplayName("地址")]
        public string Address { set; get; }

        public string Province { set; get; }

        public string City { set; get; }

        public string Type { set; get; }

        public double Lat { set; get; }

        public double Lng { set; get; }
    }
}
