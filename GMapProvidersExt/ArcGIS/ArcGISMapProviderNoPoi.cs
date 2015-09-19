using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapProvidersExt.ArcGIS
{
    public class ArcGISMapProviderNoPoi : ArcGISMapProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("DF3494B2-C13B-40FB-9B7F-DAE47269027B");
        public static readonly ArcGISMapProviderNoPoi Instance;
        private readonly string name;

        // Methods
        static ArcGISMapProviderNoPoi()
        {
            Instance = new ArcGISMapProviderNoPoi();
        }

        private ArcGISMapProviderNoPoi()
        {
            this.name = "ArcGISMapNoPOI";
            this.cnName = "ArcGIS街道地图(无POI)";
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = string.Format(ArcGISMapProviderBase.UrlFormat, new object[] { "ChinaOnlineStreetColor", zoom, pos.Y, pos.X });
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
