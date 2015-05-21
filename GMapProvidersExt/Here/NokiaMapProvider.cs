using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Here
{
    public class NokiaMapProvider : NokiaMapProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("c901bfde-fb23-4693-97a5-10b225142752");
        public static readonly NokiaMapProvider Instance;
        private readonly string name;

        // Methods
        static NokiaMapProvider()
        {
            Instance = new NokiaMapProvider();
        }

        private NokiaMapProvider()
        {
            this.name = "NokiaMap";
            this.cnName = "Here街道地图";
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = string.Format(NokiaMapProviderBase.UrlFormat, new object[] { GMapProvider.GetServerNum(pos, NokiaMapProviderBase.maxServer) + 1, "normal", zoom, pos.X, pos.Y });
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
