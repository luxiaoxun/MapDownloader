using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.Internals;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Baidu
{
    public abstract class BaiduMapProviderBase1 : GMapProvider
    {
        // Fields
        private static bool init;
        public static readonly int maxServer;
        private GMapProvider[] overlays;
        public static readonly string UrlFormat;

        // Methods
        static BaiduMapProviderBase1()
        {
            maxServer = 9;
            UrlFormat = "http://online{0}.map.bdimg.com/tile/?qt=tile&x={1}&y={2}&z={3}&styles=pl&udt=20150213";
            init = false;
        }

        public BaiduMapProviderBase1()
        {
            base.MaxZoom = 0x13;
            base.MinZoom = 1;
            base.RefererUrl = string.Format("http://q{0}.baidu.com/", maxServer.ToString());
            base.Copyright = string.Format("\x00a9 Baidu! Inc. - Map data & Imagery \x00a9{0} NAVTEQ", DateTime.Today.Year);
        }

        public override GMapProvider[] Overlays
        {
            get
            {
                if (this.overlays == null)
                {
                    this.overlays = new GMapProvider[] { this };
                }
                return this.overlays;
            }
        }

        public override PureProjection Projection
        {
            get
            {
                return BaiduProjection1.Instance;
            }
        }
    }
}
