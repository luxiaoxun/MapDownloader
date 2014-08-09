using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapProvidersExt
{
    public abstract class BaiduMapProviderBase : GMapProvider
    {
        private string ClientKey = "1308e84a0e8a1fc2115263a4b3cf87f1";
        public BaiduMapProviderBase()
        {
            MaxZoom = null;
            RefererUrl = "http://map.baidu.com";
            //Copyright = string.Format(“{0} Baidu Corporation, {0} NAVTEQ, {0} Image courtesy of NASA“, DateTime.Today.Year); 
        }

        public override PureProjection Projection
        {
            //get { return MercatorProjection.Instance; }
            get { return BaiduProjection.Instance; }
        }

        GMapProvider[] overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { this };
                }
                return overlays;
            }
        }
    }

    public class BaiduMapProvider : BaiduMapProviderBase
    {
        public static readonly BaiduMapProvider Instance;
        public string SecureWord = "Galileo";
        private string string_4 = "&s=";

        readonly Guid id = new Guid("708748FC-5FDD-4d3a-9027-356F24A755E5");
        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "BaiduMap";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static BaiduMapProvider()
        {
            Instance = new BaiduMapProvider();
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

        internal void GetSecureWords(GPoint pos, out string sec1, out string sec2)
        {
            sec1 = string.Empty;
            sec2 = string.Empty;
            int length = ((int)((pos.X * 3) + pos.Y)) % 8;
            sec2 = this.SecureWord.Substring(0, length);
            if ((pos.Y >= 0x2710) && (pos.Y < 0x186a0))
            {
                sec1 = string_4;
            }
        }

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            var num = GMapProvider.GetServerNum(pos, 10);
            var x = (pos.X > 0) ? pos.X.ToString() : ("M" + Math.Abs(pos.X).ToString());
            var y = (pos.Y > 0) ? pos.Y.ToString() : ("M" + Math.Abs(pos.Y).ToString());

            string url = string.Format(UrlFormat, num, x, y, zoom);
            return url;
        }
        //http://online1.map.bdimg.com/tile/?qt=tile&x=1615&y=458&z=13&styles=pl&udt=20140314
        static readonly string UrlFormat = "http://online{0}.map.bdimg.com/tile/?qt=tile&x={1}&y={2}&z={3}&styles=pl&udt=20140314";

    }
}
