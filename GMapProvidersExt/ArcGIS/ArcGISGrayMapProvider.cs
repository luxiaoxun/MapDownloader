using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapProvidersExt.ArcGIS
{
    public class ArcGISGrayMapProvider : ArcGISMapProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("539B26AB-72E3-471E-B614-C9AFCB0D4CA1");
        public static readonly ArcGISGrayMapProvider Instance;
        private readonly string name;

        // Methods
        static ArcGISGrayMapProvider()
        {
            Instance = new ArcGISGrayMapProvider();
        }

        private ArcGISGrayMapProvider()
        {
            this.name = "ArcGISGrayMap";
            this.cnName = "ArcGIS街道地图(灰色版)";
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = string.Format(ArcGISMapProviderBase.UrlFormat, new object[] { "ChinaOnlineStreetGray", zoom, pos.Y, pos.X });
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
