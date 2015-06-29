using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Sogou
{
    public abstract class SogouMapProviderBase : GMapProvider
    {
        // Fields
        //private static RectifyTool rectifyTool;

        // Methods
        public SogouMapProviderBase()
        {
            base.MaxZoom = 0x13;
            base.MinZoom = 3;
            base.RefererUrl = "http://map.sogou.com/";
        }

        public GeoCoderStatusCode GetPlacemark(PointLatLng location, out Placemark placemark)
        {
            placemark = new Placemark();
            return GeoCoderStatusCode.G_GEO_SUCCESS;
        }

        public GeoCoderStatusCode GetPlacemarksByKeywords(string keywords, out List<Placemark> placemarks)
        {
            placemarks = new List<Placemark>();
            return GeoCoderStatusCode.G_GEO_SUCCESS;
        }
    }

 

}
