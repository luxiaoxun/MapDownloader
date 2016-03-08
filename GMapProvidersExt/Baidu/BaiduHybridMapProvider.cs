using System;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Baidu
{
    public class BaiduHybridMapProvider : BaiduMapProviderBase
    {
        // Fields
        private readonly string cnName;
        public static readonly string HybridUrlFormat;
        private readonly Guid id = new Guid("AF522C29-9F94-4E9B-BDB3-346CD058AE7B");
        public static readonly BaiduHybridMapProvider Instance;
        private readonly string name;
        private GMapProvider[] overlays;
        public string Version;

        // Methods
        static BaiduHybridMapProvider()
        {
            HybridUrlFormat = "http://online{0}.map.bdimg.com/tile/?qt=tile&x={1}&y={2}&z={3}&styles=sl&v={4}&udt=20140314";
            Instance = new BaiduHybridMapProvider();
            GMapProviders.AddMapProvider(Instance);
        }

        private BaiduHybridMapProvider()
        {
            this.Version = "039";
            this.name = "BaiduHybridMap";
            this.cnName = "百度混合地图";
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = this.MakeTileImageUrl(pos, zoom, GMapProvider.LanguageStr);
            return base.GetTileImageUsingHttp(url);
        }

        private string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            long num = pos.X - ((long)Math.Pow(2.0, (double)(zoom - 1)));
            long num2 = (((long)Math.Pow(2.0, (double)(zoom - 1))) - pos.Y) - 1;
            string str = num.ToString();
            string str2 = num2.ToString();
            if (str.StartsWith("-"))
            {
                str = "M" + str.Substring(1);
            }
            if (str2.StartsWith("-"))
            {
                str2 = "M" + str2.Substring(1);
            }
            return string.Format(HybridUrlFormat, new object[] { GMapProvider.GetServerNum(pos, BaiduMapProviderBase.maxServer) + 1, str, str2, zoom, this.Version });
        }

        // Properties
        public string CnName
        {
            get
            {
                return this.cnName;
            }
        }

        public override Guid Id
        {
            get
            {
                return this.id;
            }
        }

        public override string Name
        {
            get
            {
                return this.name;
            }
        }

        public override GMapProvider[] Overlays
        {
            get
            {
                if (this.overlays == null)
                {
                    this.overlays = new GMapProvider[] { BaiduSatelliteMapProvider.Instance, this };
                }
                return this.overlays;
            }
        }
    }
}
