using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Here
{
    public class NokiaHybridMapProvider : NokiaMapProviderBase
    {
        // Fields
        private string cnName;
        private Guid id = new Guid("a61e1468-a645-453a-ab24-7d27a5198a50");
        public static readonly NokiaHybridMapProvider Instance;
        private string name;

        // Methods
        static NokiaHybridMapProvider()
        {
            Instance = new NokiaHybridMapProvider();
        }

        public NokiaHybridMapProvider()
        {
            this.name = "NokiaHybridMap";
            this.cnName = "Here混合地图";
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = string.Format(NokiaMapProviderBase.UrlFormat, new object[] { GMapProvider.GetServerNum(pos, NokiaMapProviderBase.maxServer) + 1, "hybrid", zoom, pos.X, pos.Y });
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
