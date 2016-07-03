using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.TianDitu
{
    public class TiandituSatelliteMapProvider : TiandituProviderBase
    {
        // Fields
        private string cnName;
        private Guid id = new Guid("5CCE1E7D-41EE-4AD0-B250-5CDEB0DC787B");
        public static readonly TiandituSatelliteMapProvider Instance;
        private string name;

        // Methods
        static TiandituSatelliteMapProvider()
        {
            Instance = new TiandituSatelliteMapProvider();
        }

        public TiandituSatelliteMapProvider()
        {
            this.name = "TiandituSatelliteMap3857";
            this.cnName = "天地图卫星地图(球面墨卡托)";
        }

        protected override bool CheckTileImageHttpResponse(System.Net.WebResponse response)
        {
            return true;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string str = "img_w";
                int serverIndex = GMapProvider.GetServerNum(pos, TiandituProviderBase.maxServer);
                //string url = string.Format(TiandituProviderBase.UrlFormat, new object[] { serverIndex, str, str.Substring(0, str.Length - 2), str.Substring(str.Length - 1), zoom, pos.Y, pos.X });
                string url = string.Format(TiandituProviderBase.UrlFormat, new object[] { serverIndex, str, pos.X, pos.Y, zoom });
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
