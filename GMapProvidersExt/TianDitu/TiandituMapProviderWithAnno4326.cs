using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.TianDitu
{
    public class TiandituMapProviderWithAnno4326 : TiandituProviderBase4326
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("7231df6b-d3b5-4478-84a4-b9de2c17dde3");
        public static readonly TiandituMapProviderWithAnno4326 Instance;
        private readonly string name;
        private GMapProvider[] overlays;

        // Methods
        static TiandituMapProviderWithAnno4326()
        {
            Instance = new TiandituMapProviderWithAnno4326();
        }

        public TiandituMapProviderWithAnno4326()
        {
            //this.id = new Guid("7231df6b-d3b5-4478-84a4-b9de2c17dde3");
            this.name = "TiandituMap4326";
            this.cnName = "天地图街道地图(WGS84)";
        }

        protected override bool CheckTileImageHttpResponse(System.Net.WebResponse response)
        {
            return true;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string str = "cva_c";
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
                    this.overlays = new GMapProvider[] { TiandituMapProvider4326.Instance, this };
                }
                return this.overlays;
            }
        }
    }
}
