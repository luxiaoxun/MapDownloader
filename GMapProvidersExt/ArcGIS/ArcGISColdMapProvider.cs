using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapProvidersExt.ArcGIS
{
    public class ArcGISColdMapProvider : ArcGISMapProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("5EFCC0BB-AE06-4AEF-86BE-5375C395544C");
        public static readonly ArcGISColdMapProvider Instance;
        private readonly string name;

        // Methods
        static ArcGISColdMapProvider()
        {
            Instance = new ArcGISColdMapProvider();
        }

        private ArcGISColdMapProvider()
        {
            this.name = "ArcGISColdMap";
            this.cnName = "ArcGIS街道地图(冷色版)";
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = string.Format(ArcGISMapProviderBase.UrlFormat, new object[] { "ChinaOnlineStreetCold", zoom, pos.Y, pos.X });
            return base.GetTileImageUsingHttp(url);
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
