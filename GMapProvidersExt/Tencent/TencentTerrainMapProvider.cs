using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Tencent
{
    public class TencentTerrainMapProvider : TencentMapProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("B6756768-9B73-49E6-9902-F4FC713B37CB");
        public static readonly TencentTerrainMapProvider Instance;
        private readonly string name;
        public static string UrlFormat;

        // Methods
        static TencentTerrainMapProvider()
        {
            UrlFormat = "http://p{0}.map.gtimg.com/{1}/{2}.{3}";
            Instance = new TencentTerrainMapProvider();
            GMapProviders.AddMapProvider(Instance);
        }

        private TencentTerrainMapProvider()
        {
            this.name = "SoSoTerrainMap";
            this.cnName = "腾讯地形地图（无注记）";
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = string.Format(UrlFormat, new object[] { GMapProvider.GetServerNum(pos, TencentMapProviderBase.maxServer), "demTiles", base.GetSosoMapTileNo(pos, zoom), "jpg" });
                return base.GetTileImageUsingHttp(url);
            }
            catch (Exception ex)
            {
                return null;
            }
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
