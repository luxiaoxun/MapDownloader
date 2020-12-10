using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Tencent
{
    public class TencentMapHybridProvider : TencentMapProviderBase
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
            string url = string.Format(UrlFormat, serverIndex, zoom, pos.X, num);
            return base.GetTileImageUsingHttp(url);
        }

        //static readonly string UrlFormat = "http://p{0}.map.gtimg.com/demTranTiles/{1}.png";
        //styleid改为其他数字有多种不同风格 0/1为普通地图(河流天蓝色) 2为路网（官方为配合卫星底图用） 3河流蓝色路网（官方为配合地形底图用） 4暗黑风格
        static readonly string UrlFormat = "https://rt{0}.map.gtimg.com/tile?z={1}&x={2}&y={3}&styleid=2&version=597"; //卫星路网用 styleid=2默认为2
    }
}
