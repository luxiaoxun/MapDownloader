using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapProvidersExt.TianDitu
{
    public abstract class TiandituProviderBase : GMapProvider
    {
        // Fields
        public static readonly int maxServer;
        private GMapProvider[] overlays;
        public static string UrlFormat;

        // Methods
        static TiandituProviderBase()
        {
            maxServer = 7;
            UrlFormat = "http://t{0}.tianditu.com/{1}/wmts?SERVICE=WMTS&REQUEST=GetTile&VERSION=1.0.0&LAYER={2}&TILEMATRIXSET={3}&TILEMATRIX={4}&TILEROW={5}&TILECOL={6}&FORMAT=tiles";
        }

        public TiandituProviderBase()
        {
            base.MaxZoom = 0x12;
            base.MinZoom = 2;
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
                //return SphericalMercatorProjection.Instance;
                return MercatorProjection.Instance;
            }
        }
    }


}
