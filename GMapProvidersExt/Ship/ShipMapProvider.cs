using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapProvidersExt.Ship
{
    public class ShipMapProvider:ShipMapProviderBase
    {
        public static readonly ShipMapProvider Instance;

        readonly Guid id = new Guid("96F56540-D5B2-4642-AAC5-591386DE857F");
        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "ShipMap";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        private readonly string cnName = "船舶地图";
        public string CnName
        {
            get
            {
                return this.cnName;
            }
        }

        static ShipMapProvider()
        {
            Instance = new ShipMapProvider();
        }

        GMapProvider[] overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { ShipMapTileProvider.Instance,this };
                }
                return overlays;
            }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = MakeTileImageUrl(pos, zoom, LanguageStr);
                return GetTileImageUsingHttp(url);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            //var num = (pos.X + pos.Y) % 4 + 1;
            //string url = string.Format(UrlFormat, num, pos.X, pos.Y, zoom);
            string url = string.Format(UrlFormat, zoom, pos.Y, pos.X);
            return url;
        }

        //http://r2.shipxy.com/r2/sp.dll?cmd=112&scode=11111111&z=8&y=113&x=212
        static readonly string UrlFormat = "http://r2.shipxy.com/r2/sp.dll?cmd=112&scode=11111111&z={0}&y={1}&x={2}";
    }
}
