using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapProvidersExt.ArcGIS
{
    public class ArcGISSatelliteMapProvider : ArcGISMapProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("47B5B4AC-088A-4D7C-88F1-62BC2444AC4F");
        public static readonly ArcGISSatelliteMapProvider Instance;
        private readonly string name;
        public static string UrlFormatForSat;

        // Methods
        static ArcGISSatelliteMapProvider()
        {
            UrlFormatForSat = "http://services.arcgisonline.com/ArcGIS/services/World_Imagery/MapServer?mapname=Layers&layer=_alllayers&level={0}&row={1}&column={2}&format=PNG";
            Instance = new ArcGISSatelliteMapProvider();
        }

        private ArcGISSatelliteMapProvider()
        {
            this.name = "ArcGISSatelliteMap";
            this.cnName = "ArcGIS卫星地图(无偏移)";
            base.MaxZoom = 0x13;
            base.MinZoom = 0;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = string.Format(UrlFormatForSat, zoom, pos.Y, pos.X);
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
