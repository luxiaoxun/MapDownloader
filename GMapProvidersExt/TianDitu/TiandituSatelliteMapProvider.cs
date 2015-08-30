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
        private Guid id = new Guid("B8A31747-2D7E-4EE3-9E71-4A314F26E6CC");
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
                string url = string.Format(TiandituProviderBase.UrlFormat, new object[] { GMapProvider.GetServerNum(pos, TiandituProviderBase.maxServer), str, str.Substring(0, str.Length - 2), str.Substring(str.Length - 1), zoom, pos.Y, pos.X });
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
