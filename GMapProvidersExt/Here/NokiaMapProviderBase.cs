using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Here
{
    public abstract class NokiaMapProviderBase : GMapProvider
    {
        // Fields
        public static readonly int maxServer;
        private GMapProvider[] overlays;
        public static string UrlFormat;

        // Methods
        static NokiaMapProviderBase()
        {
            maxServer = 3;
            UrlFormat = "http://{0}.maps.nlp.nokia.com.cn/maptile/2.1/maptile/5b33fc2110/{1}.day/{2}/{3}/{4}/256/png8?lg=CHI&app_id=90oGXsXHT8IRMSt5D79X&token=JY0BReev8ax1gIrHZZoqIg&xnlp=CL_JSMv2.5.3.2";
        }

        public NokiaMapProviderBase()
        {
            base.MaxZoom = 20;
            base.MinZoom = 0;
            base.RefererUrl = "http://heremaps.cn/";
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
