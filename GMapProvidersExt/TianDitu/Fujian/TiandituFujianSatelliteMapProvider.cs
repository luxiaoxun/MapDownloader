using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.TianDitu.Fujian
{
    public class TiandituFujianSatelliteMapProvider:TiandituFujianProviderBase
    {
        // Fields
        private string cnName;
        private Guid id = new Guid("711468BE-7258-4C41-840D-5C34E9795552");
        public static readonly TiandituFujianSatelliteMapProvider Instance;
        private string name;

        // Methods
        static TiandituFujianSatelliteMapProvider()
        {
            Instance = new TiandituFujianSatelliteMapProvider();
        }

        public TiandituFujianSatelliteMapProvider()
        {
            this.name = "TiandituFujianSatelliteMap";
            this.cnName = "天地图福建卫星地图(WGS84)";
        }

        protected override bool CheckTileImageHttpResponse(System.Net.WebResponse response)
        {
            return true;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string str = "img_fj";
                int serverIndex = GMapProvider.GetServerNum(pos, TiandituFujianProviderBase.maxServer);
                string url = string.Format(TiandituFujianProviderBase.UrlFormat, new object[] { str, zoom, pos.Y, pos.X, "tile" });
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

        public bool IsEmptyAreaNotReturnData
        {
            get
            {
                return true;
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
