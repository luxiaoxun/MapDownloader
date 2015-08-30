using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapProvidersExt.SoSo
{
    public abstract class SosoMapProviderBase : GMapProvider
    {
        public SosoMapProviderBase()
        {
            MaxZoom = 18;
            MinZoom = 1;
            RefererUrl = "http://map.soso.com";
            //Copyright = string.Format("©{0} Tencent Corporation, ©{0} NAVTEQ, ©{0} Image courtesy of NASA", DateTime.Today.Year);    
        }

        public override PureProjection Projection
        {
            get { return MercatorProjection.Instance; }
        }

        GMapProvider[] overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { this };
                }
                return overlays;
            }
        }
    }
}
