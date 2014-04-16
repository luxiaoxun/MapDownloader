using System;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapProvidersExt
{
    public class AMapSateliteProvider : AMapProviderBase
    {
        public static readonly AMapSateliteProvider Instance;

        readonly Guid id = new Guid("FCA94AF4-3467-47c6-BDA2-6F52E4A145BC");
        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "AMapSatelite";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static AMapSateliteProvider()
        {
            Instance = new AMapSateliteProvider();
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

            //http://webst04.is.autonavi.com/appmaptile?x=23&y=12&z=5&lang=zh_cn&size=1&scale=1&style=8
            var num = (pos.X + pos.Y) % 4 + 1;
            //string url = string.Format(UrlFormat , num, pos.X, pos.Y, zoom);
            string url = string.Format(UrlFormat, pos.X, pos.Y, zoom);
            return url;
        }

        //http://webst03.is.autonavi.com/appmaptile?style=8&x=1707&y=843&z=11
        //static readonly string UrlFormat = "http://webst04.is.autonavi.com/appmaptile?x={0}&y={1}&z={2}&lang=zh_cn&size=1&scale=1&style=6";
        static readonly string UrlFormat = "http://webst01.is.autonavi.com/appmaptile?style=8&x={0}&y={1}&z={2}";
    }
}
