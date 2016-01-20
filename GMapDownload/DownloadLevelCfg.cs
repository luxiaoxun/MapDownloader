using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapDownload
{
    public class DownloadLevelCfg
    {
        [DisplayName("")]
        public bool IsChecked { set; get; }

        [DisplayName("级别")]
        public int ZoomLevel { set; get; }

        [DisplayName("行数")]
        public long ZoomLevelRow { set; get; }

        [DisplayName("列数")]
        public long ZoomLevelCol { set; get; }

        [DisplayName("总数")]
        public long PreCount { set; get; }

        [DisplayName("大小")]
        public string PreSize { set; get; }

        public List<GPoint> TileGPoints { set; get; }

        public GPoint TopLeft { set; get; }

        public GPoint RightBottom { set; get; } 
    }
}
