using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.TianDitu.Fujian
{
    public class TiandituFujianMapProviderWithAnno : TiandituFujianProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("C2C52E3B-1B25-4BAE-B9EF-23B3E1BB5B18");
        public static readonly TiandituFujianMapProviderWithAnno Instance;
        private readonly string name;
        private GMapProvider[] overlays;

        // Methods
        static TiandituFujianMapProviderWithAnno()
        {
            Instance = new TiandituFujianMapProviderWithAnno();
        }

        public TiandituFujianMapProviderWithAnno()
        {
            this.name = "TiandituFujianMapWithAnno";
            this.cnName = "天地图福建街道地图(WGS84)";
        }

        protected override bool CheckTileImageHttpResponse(System.Net.WebResponse response)
        {
            return true;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string str = "cva_fj";
                int serverIndex = GMapProvider.GetServerNum(pos, TiandituFujianProviderBase.maxServer);
                string url = string.Format(TiandituFujianProviderBase.UrlFormat, new object[] { str, zoom, pos.Y, pos.X, "png"});
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
                    this.overlays = new GMapProvider[] { TiandituFujianMapProvider.Instance, this };
                }
                return this.overlays;
            }
        }
    }
}
