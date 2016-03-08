using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Tencent
{
    public class TencentMapSateliteProvider : TencentMapProviderBase
    {
        public static readonly TencentMapSateliteProvider Instance;

        private readonly Guid id = new Guid("26BEB795-3617-403D-8DFF-08022D1DDF1B");
        public override Guid Id
        {
            get { return id; }
        }

        private readonly string cnName = "腾讯卫星地图";
        public string CnName
        {
            get
            {
                return this.cnName;
            }
        }

        private readonly string name = "SoSoMapSatelite";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static TencentMapSateliteProvider()
        {
            
            Instance = new TencentMapSateliteProvider();
            GMapProviders.AddMapProvider(Instance);
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                int serverIndex = GMapProvider.GetServerNum(pos, TencentMapProviderBase.maxServer);
                string url = string.Format(UrlFormat, new object[] { serverIndex, base.GetSosoMapTileNo(pos, zoom) });
                return base.GetTileImageUsingHttp(url);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        static readonly string UrlFormat = "http://p{0}.map.soso.com/sateTiles/{1}.jpg";
    }
}
