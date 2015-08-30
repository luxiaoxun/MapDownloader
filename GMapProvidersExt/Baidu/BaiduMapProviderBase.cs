using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GMap.NET;
using GMap.NET.Internals;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Baidu
{
    public abstract class BaiduMapProviderBase : GMapProvider
    {
        // Fields
        private static bool init;
        public static readonly int maxServer;
        private GMapProvider[] overlays;
        public static readonly string UrlFormat;

        // Methods
        static BaiduMapProviderBase()
        {
            maxServer = 9;
            UrlFormat = "http://online{0}.map.bdimg.com/tile/?qt=tile&x={1}&y={2}&z={3}&styles=pl&udt=20150213";
            init = false;
        }

        public BaiduMapProviderBase()
        {
            base.MaxZoom = 18;
            base.MinZoom = 3;
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
                return BaiduProjection.Instance;
            }
        }

        public override void OnInitialized()
        {
            if (!init)
            {
                string url = "http://map.baidu.com";
                try
                {
                    string contentUsingHttp = Singleton<Cache>.Instance.GetContent(url, CacheType.UrlCache, TimeSpan.FromHours(24.0));
                    if (string.IsNullOrEmpty(contentUsingHttp))
                    {
                        contentUsingHttp = base.GetContentUsingHttp(url);
                        if (!string.IsNullOrEmpty(contentUsingHttp))
                        {
                            Singleton<Cache>.Instance.SaveContent(url, CacheType.UrlCache, contentUsingHttp);
                        }
                    }
                    if (!string.IsNullOrEmpty(contentUsingHttp))
                    {
                        Regex regex = new Regex("{\"version\":\"(\\d*)\",\"updateDate\":\".{6,8}\"},\"satellite\":", RegexOptions.IgnoreCase);
                        Match match = regex.Match(contentUsingHttp);
                        if (match.Success)
                        {
                            GroupCollection groups3 = match.Groups;
                            if (groups3.Count > 0)
                            {
                                string str4 = groups3[1].Value;
                                BaiduMapProvider.Instance.Version = str4;
                            }
                        }
                        regex = new Regex("{\"version\":\"(\\d*)\",\"updateDate\":\".{6,8}\"},\"normalTraffic\":", RegexOptions.IgnoreCase);
                        match = regex.Match(contentUsingHttp);
                        if (match.Success)
                        {
                            GroupCollection groups2 = match.Groups;
                            if (groups2.Count > 0)
                            {
                                string str5 = groups2[1].Value;
                                BaiduSatelliteMapProvider.Instance.Version = str5;
                            }
                        }
                        match = new Regex("{\"version\":\"(\\d*)\",\"updateDate\":\".{6,8}\"},\"dem\":", RegexOptions.IgnoreCase).Match(contentUsingHttp);
                        if (match.Success)
                        {
                            GroupCollection groups = match.Groups;
                            if (groups.Count > 0)
                            {
                                string str3 = groups[1].Value;
                                BaiduHybridMapProvider.Instance.Version = str3;
                            }
                        }
                    }
                    init = true;
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
