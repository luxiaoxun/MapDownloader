using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapProvidersExt.TianDitu.Fujian
{
    public abstract class TiandituFujianProviderBase : GMapProvider
    {
        // Fields
        public static readonly int maxServer;
        private GMapProvider[] overlays;
        public static string UrlFormat;

        // Methods
        static TiandituFujianProviderBase()
        {
            maxServer = 5;
            //UrlFormat = "http://t{0}.tianditu.com/{1}/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER={2}&TILEMATRIXSET={3}&TILEMATRIX={4}&TILEROW={5}&TILECOL={6}&FORMAT=tiles";
            UrlFormat = "http://service.fjmap.net/{0}/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER={0}&STYLE={0}&TILEMATRIXSET=Matrix_0&TILEMATRIX={1}&TILEROW={2}&TILECOL={3}&FORMAT=image%2F{4}";
        }

        public TiandituFujianProviderBase()
        {
            base.MaxZoom = 0x12;
            base.MinZoom = 1;
            base.RefererUrl = "http://www.tianditu.com";
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
                //return MercatorProjection.Instance;
                return WGS84Projection.Instance;
            }
        }
    }


}
