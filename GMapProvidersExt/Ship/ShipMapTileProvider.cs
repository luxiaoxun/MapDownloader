using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapProvidersExt.Ship
{
    public class ShipMapTileProvider:ShipMapProviderBase
    {
        public static readonly ShipMapTileProvider Instance;

        static ShipMapTileProvider()
        {
            Instance = new ShipMapTileProvider();
        }

        readonly Guid id = new Guid("568DFE06-D943-4F5A-A48F-08E7FAF6D089");
        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "ShipTileMap";
        public override string Name
        {
            get
            {
                return name;
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

        //http://m13.shipxy.com/tile.c?l=Na&m=o&z=8&y=111&x=216
        static readonly string UrlFormat = "http://m13.shipxy.com/tile.c?l=Na&m=o&z={0}&y={1}&x={2}";
    }
}
