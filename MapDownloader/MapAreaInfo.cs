using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace MapDownloader
{
    class MapAreaInfo
    {
        public int Zoom { set; get; }

        public RectLatLng Area { set; get; }

        public MapAreaInfo(int zoom, RectLatLng area)
        {
            Zoom = zoom;
            Area = area;
        }
    }
}
