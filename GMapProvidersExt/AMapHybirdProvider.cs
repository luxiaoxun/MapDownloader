using System;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapProvidersExt
{
    public class AMapHybirdProvider : AMapProviderBase
    {
        public static readonly AMapHybirdProvider Instance;
   
        readonly Guid id = new Guid("EF3DD303-3F74-4938-BF40-232D0595EE87");
        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "AMapHybird";
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

            //http://webst03.is.autonavi.com/appmaptile?style=8&x=1633&y=848&z=11
            string url = string.Format(UrlFormat, pos.X, pos.Y, zoom);
            Console.WriteLine("url:" + url);
            return url;
        }

        static readonly string UrlFormat = "http://webst03.is.autonavi.com/appmaptile?style=8&x={0}&y={1}&z={2}";
    }
}
