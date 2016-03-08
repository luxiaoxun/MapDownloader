using System;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapProvidersExt.AMap
{
    public class AMapHybirdProvider : AMapProviderBase
    {
        public static readonly AMapHybirdProvider Instance;
        
        private readonly Guid id = new Guid("EF3DD303-3F74-4938-BF40-232D0595EE87");
        public override Guid Id
        {
            get { return id; }
        }

        private readonly string cnName = "高德混合地图";
        public string CnName
        {
            get
            {
                return this.cnName;
            }
        }

        private readonly string name = "AMapHybird";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static AMapHybirdProvider()
        {
            Instance = new AMapHybirdProvider();
            GMapProviders.AddMapProvider(Instance);
        }

        GMapProvider[] overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { AMapSateliteProvider.Instance, this };
                }
                return overlays;
            }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = MakeTileImageUrl(pos, zoom, LanguageStr);

            return GetTileImageUsingHttp(url);
        }

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            var num = (pos.X + pos.Y) % 4 + 1;
            string url = string.Format(UrlFormat, num, pos.X, pos.Y, zoom);
            return url;
        }

        static readonly string UrlFormat = "http://webst0{0}.is.autonavi.com/appmaptile?style=8&x={1}&y={2}&z={3}";
    }
}
