using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapDownload
{
    public class TileDownloaderArgs
    {
        public List<DownloadLevelTile> DownloadTiles { set; get; }

        public GMapProvider MapProvider { set; get; }

        public RectLatLng DownloadArea { set; get; }
    }
}
