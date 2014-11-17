using System;
using System.Collections.Generic;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMap.NET.MapProviders
{
    public abstract class MyMapProviderBase:GMapProvider
    {
        public MyMapProviderBase()
        {
            MaxZoom = null;
            //RefererUrl = "http://www.amap.com/";
            //RefererUrl = "http://192.1.114.11";
            //Copyright = string.Format("©{0} 高德 Corporation, ©{0} NAVTEQ, ©{0} Image courtesy of NASA", DateTime.Today.Year);    
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
