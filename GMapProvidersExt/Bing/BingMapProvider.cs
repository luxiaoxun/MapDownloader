using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Bing
{
    public class BingMapProvider : BingMapProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("D0CEB371-F10A-4E12-A2C1-DF617D6674A8");
        public static readonly BingMapProvider Instance;
        private readonly string name;
        private const string UrlFormat = "http://ecn.t{0}.tiles.virtualearth.net/tiles/r{1}?g={2}&mkt={3}&lbl=l1&stl=h&shading=hill&n=z{4}";

        // Methods
        static BingMapProvider()
        {
            Instance = new BingMapProvider();
        }

        private BingMapProvider()
        {
            this.name = "BingMap";
            this.cnName = "必应普通地图（国外）";
            base.MaxZoom = 0x11;
            base.MinZoom = 1;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = this.MakeTileImageUrl(pos, zoom, GMapProvider.LanguageStr);
            return base.GetTileImageUsingHttp(url);
        }

        private string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            string str = base.TileXYToQuadKey(pos.X, pos.Y, zoom);
            return string.Format("http://ecn.t{0}.tiles.virtualearth.net/tiles/r{1}?g={2}&mkt={3}&lbl=l1&stl=h&shading=hill&n=z{4}", new object[] { GMapProvider.GetServerNum(pos, 4), str, base.Version, language, !string.IsNullOrEmpty(base.ClientKey) ? ("&key=" + base.ClientKey) : string.Empty });
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
