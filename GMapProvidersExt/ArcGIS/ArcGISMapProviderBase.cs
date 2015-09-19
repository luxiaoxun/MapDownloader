using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.ArcGIS
{
    public abstract class ArcGISMapProviderBase : GMapProvider
    {
        // Fields
        private GMapProvider[] overlays;
        public static string UrlFormat;

        // Methods
        static ArcGISMapProviderBase()
        {
            UrlFormat = "http://cache1.arcgisonline.cn/arcgis/rest/services/{0}/MapServer/tile/{1}/{2}/{3}";
        }

        public ArcGISMapProviderBase()
        {
            base.MaxZoom = 0x10;
            base.MinZoom = 0;
            base.RefererUrl = "http://www.arcgisonline.cn/";
        }

        // Properties
        public override GMapProvider[] Overlays
        {
            get
            {
                if (this.overlays == null)
                {
                    this.overlays = new GMapProvider[] { this };
                }
                return this.overlays;
            }
        }

        public override PureProjection Projection
        {
            get
            {
                return SphericalMercatorProjection.Instance;
            }
        }
    }


}
