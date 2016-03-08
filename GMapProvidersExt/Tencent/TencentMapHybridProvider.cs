using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Tencent
{
    public class TencentMapHybridProvider:TencentMapProviderBase
    {
        public static readonly TencentMapHybridProvider Instance;

        private readonly Guid id = new Guid("C1A6F1E3-6107-491B-AEFF-A4E0B23F6CEC");
        public override Guid Id
        {
            get { return id; }
        }

        private readonly string cnName = "腾讯混合地图";
        public string CnName
        {
            get
            {
                return this.cnName;
            }
        }

        private readonly string name = "SoSoMapHybrid";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static TencentMapHybridProvider()
        {
            Instance = new TencentMapHybridProvider();
            GMapProviders.AddMapProvider(Instance);
        }

        GMapProvider[] overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { TencentMapSateliteProvider.Instance, this };
                }
                return overlays;
            }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            long num = (((long)Math.Pow(2.0, (double)zoom)) - 1L) - pos.Y;
            int serverIndex = GMapProvider.GetServerNum(pos, TencentMapProviderBase.maxServer);
            string url = string.Format(UrlFormat, new object[] { serverIndex, base.GetSosoMapTileNo(pos,zoom) });
            return base.GetTileImageUsingHttp(url);
        }

        static readonly string UrlFormat = "http://p{0}.map.gtimg.com/demTranTiles/{1}.png";
    }
}
