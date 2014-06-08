using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapTools
{
    public static class GMapHelper
    {
        public static double GetDistanceInMeter(PointLatLng p1, PointLatLng p2)
        {
            return GMapProviders.EmptyProvider.Projection.GetDistance(p1, p2) * 1000d;
        }
    }
}
