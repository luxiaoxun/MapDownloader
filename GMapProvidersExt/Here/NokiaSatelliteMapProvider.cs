using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Here
{
    public class NokiaSatelliteMapProvider : NokiaMapProviderBase
    {
        // Fields
        private string cnName;
        private Guid id = new Guid("ce8166a3-4908-45a6-9df4-787bcda49185");
        public static readonly NokiaSatelliteMapProvider Instance;
        private string name;

        // Methods
        static NokiaSatelliteMapProvider()
        {
            Instance = new NokiaSatelliteMapProvider();
        }

        public NokiaSatelliteMapProvider()
        {
            this.name = "NokiaSatelliteMap";
            this.cnName = "Here卫星地图";
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = string.Format(NokiaMapProviderBase.UrlFormat, new object[] { GMapProvider.GetServerNum(pos, NokiaMapProviderBase.maxServer) + 1, "satellite", zoom, pos.X, pos.Y });
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
