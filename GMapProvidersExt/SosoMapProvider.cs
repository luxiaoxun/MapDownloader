using System;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapProvidersExt
{
    public abstract class SosoMapProviderBase : GMapProvider
    {
        public SosoMapProviderBase()
        {
            MaxZoom = null;
            RefererUrl = "http://map.soso.com";
            //Copyright = string.Format("©{0} Tencent Corporation, ©{0} NAVTEQ, ©{0} Image courtesy of NASA", DateTime.Today.Year);    
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

        //protected override bool CheckTileImageHttpResponse(System.Net.HttpWebResponse response)
        //{
        //    var pass =  base.CheckTileImageHttpResponse(response);
        //    if (!pass)
        //    {
        //         return response.ResponseUri.AbsoluteUri.EndsWith(".png") || response.ResponseUri.AbsoluteUri.EndsWith(".JPG");
        //    }

        //    return true;
        //}
    }

    public class SosoMapProvider: SosoMapProviderBase
    {
        public static readonly SosoMapProvider Instance;
        
        readonly Guid id = new Guid("30D069B3-F6A5-4feb-9B5E-84CBF1E34F34");
        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "SoSoMap";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static SosoMapProvider()
        {
            Instance = new SosoMapProvider();
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
            var f = zoom*4;
            var i = _scope[f++];
            var j = _scope[f++];
            var l = _scope[f++];
            var scope = _scope[f];
            if (pos.X >= i && pos.X <= j && pos.Y >= l && pos.Y <= scope)
            {
                pos.Y = (long) (Math.Pow(2, zoom) - 1 - pos.Y);
            }

            //http://p0.map.soso.com/maptilesv2/11/102/74/1633_1198.png
            string url = string.Format(UrlFormat, zoom, Math.Floor((decimal)(pos.X / 16)), Math.Floor((decimal)(pos.Y / 16)), pos.X, pos.Y);
            return url;
        }

        static readonly string UrlFormat = "http://p0.map.soso.com/maptilesv2/{0}/{1}/{2}/{3}_{4}.png";
    }
}
