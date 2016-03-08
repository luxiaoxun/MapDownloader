using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapProvidersExt.SoSo
{
    public class SosoMapHybridProvider : SosoMapProviderBase
    {
        public static readonly SosoMapHybridProvider Instance;

        private readonly Guid id = new Guid("441FE314-F379-4566-8FC5-AD62784CF553");
        public override Guid Id
        {
            get { return id; }
        }

        private readonly string cnName = "SOSO混合地图";
        public string CnName
        {
            get
            {
                return this.cnName;
            }
        }

        private readonly string name = "SosoMapHybrid";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static SosoMapHybridProvider()
        {
            Instance = new SosoMapHybridProvider();
            GMapProviders.AddMapProvider(Instance);
        }

        GMapProvider[] overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { SosoMapSateliteProvider.Instance, this };
                }
                return overlays;
            }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = MakeTileImageUrl(pos, zoom, LanguageStr);

            return GetTileImageUsingHttp(url);
        }

        static int[] _scope = new int[]{0, 0, 0, 0, 
                                        0, 3, 0, 3, 
                                        0, 3, 0, 3, 
                                        0, 7, 0, 7, 
                                        0, 15, 0, 15, 
                                        0, 31, 0, 31, 
                                        0, 63, 4, 59, 
                                        0, 127, 12, 115, 
                                        0, 225, 28, 227, 
                                        356, 455, 150, 259, 
                                        720, 899, 320, 469, 
                                        1440, 1799, 650, 929, 
                                        2880, 3589, 1200, 2069, 
                                        5760, 7179, 2550, 3709, 
                                        11520, 14349, 5100, 7999, 
                                        23060, 28689, 10710, 15429, 
                                        46120, 57369, 20290, 29849, 
                                        89990, 124729, 41430, 60689, 
                                        184228, 229827, 84169, 128886};

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            var f = zoom * 4;
            var i = _scope[f++];
            var j = _scope[f++];
            var l = _scope[f++];
            var scope = _scope[f];
            if (pos.X >= i && pos.X <= j && pos.Y >= l && pos.Y <= scope)
            {
                pos.Y = (long)(Math.Pow(2, zoom) - 1 - pos.Y);
            }

            //http://p1.map.gtimg.com/demTranTiles/11/102/74/1633_1198.png
            string url = string.Format(UrlFormat, zoom, Math.Floor((decimal)(pos.X / 16)), Math.Floor((decimal)(pos.Y / 16)), pos.X, pos.Y);
            Console.WriteLine("url:" + url);
            return url;
        }

        static readonly string UrlFormat = "http://p1.map.gtimg.com/demTranTiles/{0}/{1}/{2}/{3}_{4}.png";
    }
}
