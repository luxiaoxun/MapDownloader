using System;
using System.Collections.Generic;
using System.Text;
using GMap.NET;

namespace GMap.NET.MapProviders
{
    public class MyMapProviderSatelite:MyMapProviderBase
    {
        public static readonly MyMapProviderSatelite Instance;

        readonly Guid id = new Guid("FCA94AF4-3467-47c6-BDA2-6F52E4A145BD");
        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "MySateliteMap";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static string mapServerIP;
        public static string MapServerIP
        {
            set
            {
                mapServerIP = value;
                UrlFormat = "http://" + mapServerIP + ":" + MapServerPort + "/47626774/{0}/{1}/{2}";
            }
            get
            {
                return mapServerIP;
            }
        }

        static string mapServerPort;
        public static string MapServerPort
        {
            set
            {
                mapServerPort = value;
                UrlFormat = "http://" + MapServerIP + ":" + mapServerPort + "/47626774/{0}/{1}/{2}";
            }
            get
            {
                return mapServerPort;
            }
        }

        static MyMapProviderSatelite()
        {
            Instance = new MyMapProviderSatelite();
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
            //var num = (pos.X + pos.Y) % 4 + 1;
            string url = string.Format(UrlFormat, zoom,pos.X, pos.Y);
            return url;
        }

        //static readonly string UrlFormat = "http://192.8.125.92:8844/47626774/{0}/{1}/{2}";
        private static string UrlFormat = "http://" + MapServerIP + ":" + MapServerPort + "/47626774/{0}/{1}/{2}";
    }
}
