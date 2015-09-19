using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Tencent
{
    public class SoSoTerrainMapAnnoProvider : SoSoMapProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("B6756768-9B73-49E6-9902-F4FC713B37CB");
        public static readonly SoSoTerrainMapAnnoProvider Instance;
        private readonly string name;
        private GMapProvider[] overlays;
        public static string UrlFormat;

        // Methods
        static SoSoTerrainMapAnnoProvider()
        {
            UrlFormat = "http://rt{0}.map.gtimg.com/realtimerender?z={1}&x={2}&y={3}&type=vector&style=1&v=1.1";
            Instance = new SoSoTerrainMapAnnoProvider();
        }

        private SoSoTerrainMapAnnoProvider()
        {
            this.name = "SoSoHybridMapWithAnno";
            this.cnName = "腾讯地形地图";
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                long num = (((long)Math.Pow(2.0, (double)zoom)) - 1) - pos.Y;
                string url = string.Format(UrlFormat, new object[] { GMapProvider.GetServerNum(pos, SoSoMapProviderBase.maxServer), zoom, pos.X, num });
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

        public override GMapProvider[] Overlays
        {
            get
            {
                if (this.overlays == null)
                {
                    this.overlays = new GMapProvider[] { SoSoTerrainMapProvider.Instance, this };
                }
                return this.overlays;
            }
        }
    }
}
