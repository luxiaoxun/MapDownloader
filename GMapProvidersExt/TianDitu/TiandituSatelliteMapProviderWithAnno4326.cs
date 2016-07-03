using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.TianDitu
{
    public class TiandituSatelliteMapProviderWithAnno4326 : TiandituProviderBase4326
    {
        // Fields
        private string cnName;
        private Guid id = new Guid("dc276be3-299a-40ee-b79b-e3c0029d2124");
        public static readonly TiandituSatelliteMapProviderWithAnno4326 Instance;
        private string name;
        private GMapProvider[] overlays;

        // Methods
        static TiandituSatelliteMapProviderWithAnno4326()
        {
            Instance = new TiandituSatelliteMapProviderWithAnno4326();
        }

        public TiandituSatelliteMapProviderWithAnno4326()
        {
            //this.id = new Guid("dc276be3-299a-40ee-b79b-e3c0029d2124");
            this.name = "TiandituHybridMap4326";
            this.cnName = "天地图混合地图(WGS84)";
        }

        protected override bool CheckTileImageHttpResponse(System.Net.WebResponse response)
        {
            return true;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string str = "cia_c";
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

        public override GMapProvider[] Overlays
        {
            get
            {
                if (this.overlays == null)
                {
                    this.overlays = new GMapProvider[] { TiandituSatelliteMapProvider4326.Instance, this };
                }
                return this.overlays;
            }
        }
    }


}
