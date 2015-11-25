using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Baidu
{
    public class BaiduMapProviderJS : BaiduMapProviderBaseJS
    {
        public static readonly BaiduMapProviderJS Instance;

        private readonly Guid id = new Guid("608748FC-5FDD-4d3a-9027-356F24A755E5");
        public override Guid Id
        {
            get { return id; }
        }

        private readonly string name = "BaiduMap";
        public override string Name
        {
            get { return name; }
        }

        private readonly string cnName = "百度普通地图";
        public string CnName
        {
            get
            {
                return this.cnName;
            }
        }

        static BaiduMapProviderJS()
        {
            Instance = new BaiduMapProviderJS();
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

        private string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            var offsetX = Math.Pow(2, zoom - 1);
            var offsetY = offsetX - 1;

            var numX = pos.X - offsetX;
            var numY = -pos.Y + offsetY;

            var x = numX.ToString().Replace("-", "M");
            var y = numY.ToString().Replace("-", "M");

            string url = string.Format(UrlFormat, x, y, zoom);
            return url;
        }

        static readonly string UrlFormat = "http://online1.map.bdimg.com/tile/?qt=tile&x={0}&y={1}&z={2}&styles=pl";
    }
}
