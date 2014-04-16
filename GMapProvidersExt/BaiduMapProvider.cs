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
            get { return MercatorProjection.Instance; }
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

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            //zoom = zoom - 1;
            //var offsetX = Math.Pow(2, zoom);
            //var offsetY = offsetX - 1;

            //var numX = pos.X - offsetX;
            //var numY = -pos.Y + offsetY;

            //zoom = zoom + 1;
            var num = (pos.X + pos.Y) % 4 + 1;
            //var x = numX.ToString().Replace("-", "M");
            //var y = numY.ToString().Replace("-", "M");

            var x = pos.X;
            var y = pos.Y;

            string url = string.Format(UrlFormat, num, x, y, zoom);
            return url;
        }
        //http://online1.map.bdimg.com/tile/?qt=tile&x=1615&y=458&z=13&styles=pl&udt=20140314
        static readonly string UrlFormat = "http://online{0}.map.bdimg.com/tile/?qt=tile&x={1}&y={2}&z={3}&styles=pl&udt=20140314";

    }
}
