using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GMapWinFormDemo
{
    public class HistoryGeoData
    {
        [DisplayName("ID")]
        public long ID { get; set; }

        [DisplayName("Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Longitude")]
        public double X { get; set; }

        [DisplayName("Latitude")]
        public double Y { get; set; }

        [DisplayName("Time")]
        public DateTime Time { get; set; }
    }
}
