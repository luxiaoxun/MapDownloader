using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.TianDitu
{
    public class TiandituSatelliteMapProviderWithAnno3857 : TiandituProviderBase3857
    {
        // Fields
        private string cnName;
        private Guid id = new Guid("0db67cb4-2248-4328-8ed8-f951d387f6a4");
        public static readonly TiandituSatelliteMapProviderWithAnno3857 Instance;
        private string name;
        private GMapProvider[] overlays;

        // Methods
        static TiandituSatelliteMapProviderWithAnno3857()
        {
            Instance = new TiandituSatelliteMapProviderWithAnno3857();
        }

        public TiandituSatelliteMapProviderWithAnno3857()
        {
            this.name = "TiandituHybridMap3857";
            this.cnName = "天地图混合地图(球面墨卡托)";
        }

        protected override bool CheckTileImageHttpResponse(System.Net.WebResponse response)
        {
            return true;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string str = "cia_w";
                string url = string.Format(TiandituProviderBase3857.UrlFormat, new object[] { GMapProvider.GetServerNum(pos, TiandituProviderBase3857.maxServer), str, str.Substring(0, str.Length - 2), str.Substring(str.Length - 1), zoom, pos.Y, pos.X });
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
                    this.overlays = new GMapProvider[] { TiandituSatelliteMapProvider3857.Instance, this };
                }
                return this.overlays;
            }
        }
    }


}
