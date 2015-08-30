using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Baidu
{
    public class BaiduMapProvider : BaiduMapProviderBase
    {
        // Fields
        private readonly string cnName;
        public string fm;
        private readonly Guid id = new Guid("5532ECC6-6561-4451-BF2D-22E86D0DC9F8");
        public static readonly BaiduMapProvider Instance;
        private readonly string name;
        public string type;
        public string Version;

        // Methods
        static BaiduMapProvider()
        {
            Instance = new BaiduMapProvider();
        }

        private BaiduMapProvider()
        {
            this.Version = "039";
            this.type = "web";
            this.fm = "44";
            this.name = "BaiduMap";
            this.cnName = "百度地图";
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = this.MakeTileImageUrl(pos, zoom, GMapProvider.LanguageStr);
            try
            {
                return base.GetTileImageUsingHttp(url);
            }
            catch (Exception)
            {
                return null;
            }
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
            int serverNum = GMapProvider.GetServerNum(pos, BaiduMapProviderBase.maxServer) + 1;
            return string.Format(BaiduMapProviderBase.UrlFormat, new object[] { serverNum, str, str2, zoom });
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
