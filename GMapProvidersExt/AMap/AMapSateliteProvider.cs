using System;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapProvidersExt.AMap
{
    public class AMapSateliteProvider : AMapProviderBase
    {
        public static readonly AMapSateliteProvider Instance;

        private readonly Guid id = new Guid("FCA94AF4-3467-47c6-BDA2-6F52E4A145BC");
        public override Guid Id
        {
            get { return id; }
        }

        private readonly string cnName = "高德卫星地图";
        public string CnName
        {
            get
            {
                return this.cnName;
            }
        }

        private readonly string name = "AMapSatelite";
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
            GMapProviders.AddMapProvider(Instance);
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
            var num = (pos.X + pos.Y) % 4 + 1;
            string url = string.Format(UrlFormat,num, pos.X, pos.Y, zoom);
            return url;
        }

        //http://webst04.is.autonavi.com/appmaptile?style=6&x=27198&y=13305&z=15
        static readonly string UrlFormat = "http://webst0{0}.is.autonavi.com/appmaptile?style=6&x={1}&y={2}&z={3}";
    }
}
