using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Baidu
{
    public class BaiduSatelliteMapProvider : BaiduMapProviderBase
    {
        // Fields
        private readonly string cnName;
        public string fm;
        private readonly Guid id = new Guid("EA7925B1-29A0-45A8-BD3C-5CC96C2ACCA4");
        public static readonly BaiduSatelliteMapProvider Instance;
        private readonly string name;
        public string type;
        public static readonly string UrlFormat;
        public string Version;

        // Methods
        static BaiduSatelliteMapProvider()
        {
            UrlFormat = "http://shangetu{0}.map.bdimg.com/it/u=x={1};y={2};z={3};v={4};type={5}&fm={6}&udt=20140929";
            Instance = new BaiduSatelliteMapProvider();
            GMapProviders.AddMapProvider(Instance);
        }

        private BaiduSatelliteMapProvider()
        {
            this.Version = "009";
            this.type = "sate";
            this.fm = "46";
            this.name = "BaiduSatelliteMap";
            this.cnName = "百度卫星地图";
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
            return string.Format(UrlFormat, new object[] { GMapProvider.GetServerNum(pos, BaiduMapProviderBase.maxServer) + 1, str, str2, zoom, this.Version, this.type, this.fm });
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
    }
}
