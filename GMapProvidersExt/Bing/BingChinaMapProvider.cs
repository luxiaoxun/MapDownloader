using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Bing
{
    public class BingChinaMapProvider : BingMapProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("D0CEB371-F10A-4E12-A2C1-DF617D6674A9");
        public static readonly BingChinaMapProvider Instance;
        private readonly string name;
        private const string UrlFormat = "http://r{0}.tiles.ditu.live.com/tiles/r{1}.png?g={2}&mkt={3}";

        // Methods
        static BingChinaMapProvider()
        {
            Instance = new BingChinaMapProvider();
        }

        private BingChinaMapProvider()
        {
            this.name = "BingChinaMap";
            this.cnName = "必应普通地图";
            base.MaxZoom = 0x12;
            base.MinZoom = 1;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = this.MakeTileImageUrl(pos, zoom, GMapProvider.LanguageStr);
                return base.GetTileImageUsingHttp(url);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            string str = base.TileXYToQuadKey(pos.X, pos.Y, zoom);
            return string.Format("http://r{0}.tiles.ditu.live.com/tiles/r{1}.png?g={2}&mkt={3}", new object[] { GMapProvider.GetServerNum(pos, 4), str, base.Version, language, !string.IsNullOrEmpty(base.ClientKey) ? ("&key=" + base.ClientKey) : string.Empty });
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
