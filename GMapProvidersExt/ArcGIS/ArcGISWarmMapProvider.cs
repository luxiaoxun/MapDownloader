using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapProvidersExt.ArcGIS
{
    public class ArcGISWarmMapProvider : ArcGISMapProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("47B92F1C-EC5F-45E5-B4C6-05EF77631B93");
        public static readonly ArcGISWarmMapProvider Instance;
        private readonly string name;

        // Methods
        static ArcGISWarmMapProvider()
        {
            Instance = new ArcGISWarmMapProvider();
        }

        private ArcGISWarmMapProvider()
        {
            this.name = "ArcGISWarmMap";
            this.cnName = "ArcGIS街道地图(暖色版)";
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = string.Format(ArcGISMapProviderBase.UrlFormat, new object[] { "ChinaOnlineStreetWarm", zoom, pos.Y, pos.X });
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
