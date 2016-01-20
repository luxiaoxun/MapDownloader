using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapDownload
{
    public class DownloadLevelTile
    {
        public GPoint TilePoint { set; get; }

        public int TileZoom { set; get; }

        public DownloadLevelTile(GPoint p, int zoom)
        {
            TilePoint = p;
            TileZoom = zoom;
        }
    }
}
