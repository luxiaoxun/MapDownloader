using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Tencent
{
    public abstract class TencentMapProviderBase : GMapProvider
    {
        // Fields
        public static readonly int maxServer;
        private GMapProvider[] overlays;

        // Methods
        static TencentMapProviderBase()
        {
            maxServer = 3;
        }

        public TencentMapProviderBase()
        {
            base.MaxZoom = 18;
            base.MinZoom = 1;
            base.RefererUrl = "http://map.soso.com/";
        }

        public string GetSosoMapTileNo(GPoint pos, int zoom)
        {
            long num = (((long)Math.Pow(2.0, (double)zoom)) - 1) - pos.Y;
            return string.Format("{0}/{1}/{2}/{3}_{4}", new object[] { zoom, (int)(pos.X / 0x10), (int)(num / 0x10), pos.X, num });
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
