using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.TianDitu
{
    public class TiandituSatelliteMapProvider4326 : TiandituProviderBase4326
    {
        // Fields
        private string cnName;
        private Guid id = new Guid("5506cabb-60ee-4b85-aef6-f7f48bdc464a");
        public static readonly TiandituSatelliteMapProvider4326 Instance;
        private string name;

        // Methods
        static TiandituSatelliteMapProvider4326()
        {
            Instance = new TiandituSatelliteMapProvider4326();
        }

        public TiandituSatelliteMapProvider4326()
        {
            //this.id = new Guid("5506cabb-60ee-4b85-aef6-f7f48bdc464a");
            this.name = "TiandituSatelliteMap4326";
            this.cnName = "天地图卫星地图(WGS84)";
        }

        protected override bool CheckTileImageHttpResponse(System.Net.WebResponse response)
        {
            return true;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string str = "img_c";
                int serverIndex = GMapProvider.GetServerNum(pos, TiandituProviderBase4326.maxServer);
                //string url = string.Format(TiandituProviderBase4326.UrlFormat, new object[] { serverIndex, str, str.Substring(0, str.Length - 2), str.Substring(str.Length - 1), zoom, pos.Y, pos.X });
                string url = string.Format(TiandituProviderBase4326.UrlFormat, new object[] { serverIndex, str, pos.X, pos.Y, zoom });
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
