using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.TianDitu.Fujian
{
    public class TiandituFujianSatelliteMapProviderWithAnno: TiandituFujianProviderBase
    {
        // Fields
        private string cnName;
        private Guid id = new Guid("5F0CDE68-B897-4658-A2B0-47985A54B89C");
        public static readonly TiandituFujianSatelliteMapProviderWithAnno Instance;
        private string name;
        private GMapProvider[] overlays;

        // Methods
        static TiandituFujianSatelliteMapProviderWithAnno()
        {
            Instance = new TiandituFujianSatelliteMapProviderWithAnno();
        }

        public TiandituFujianSatelliteMapProviderWithAnno()
        {
            //this.id = new Guid("dc276be3-299a-40ee-b79b-e3c0029d2124");
            this.name = "TiandituFujianHybridMap";
            this.cnName = "天地图福建混合地图(WGS84)";
        }

        protected override bool CheckTileImageHttpResponse(System.Net.WebResponse response)
        {
            return true;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string str = "cia_fj";
                int serverIndex = GMapProvider.GetServerNum(pos, TiandituFujianProviderBase.maxServer);
                string url = string.Format(TiandituFujianProviderBase.UrlFormat, new object[] { str, zoom, pos.Y, pos.X, "png" });
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
                    this.overlays = new GMapProvider[] { TiandituFujianSatelliteMapProvider.Instance, this };
                }
                return this.overlays;
            }
        }
    }
}
