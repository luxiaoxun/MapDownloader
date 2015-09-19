using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapProvidersExt.ArcGIS
{
    public class ArcGISMapProvider : ArcGISMapProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("784F8CBF-F0BF-4A22-B3D8-4AAA24FF5EEC");
        public static readonly ArcGISMapProvider Instance;
        private readonly string name;

        // Methods
        static ArcGISMapProvider()
        {
            Instance = new ArcGISMapProvider();
        }

        private ArcGISMapProvider()
        {
            this.name = "ArcGISMap";
            this.cnName = "ArcGIS街道地图";
            base.MaxZoom = 0x11;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = string.Format(ArcGISMapProviderBase.UrlFormat, new object[] { "ChinaOnlineCommunity", zoom, pos.Y, pos.X });
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
