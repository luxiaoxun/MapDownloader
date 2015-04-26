using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;

namespace GMapDrawTools
{
    public class GMapDrawLine:GMapRoute
    {
        public GMapDrawLine(List<PointLatLng> points, string name) : base(points, name)
        {
            
        }

        //judge the distance between the point and line is within the given distance
        public bool IsInside(PointLatLng p, double distance)
        {
            if (this.Points.Count >= 2)
            {
                PointLatLng A = this.Points[0];
                PointLatLng B = this.Points[1];

                double minDis = PointToSegDist(p, A, B);
                if(minDis<=distance)
                    return true;

                return false;
            }

            return false;
        }

        public double PointToSegDist(PointLatLng p, PointLatLng A, PointLatLng B)
        {
            double x = p.Lng;
            double y = p.Lat;
            double x1 = A.Lng;
            double y1 = A.Lat;
            double x2 = B.Lng;
            double y2 = B.Lat;
            double cross = (x2 - x1) * (x - x1) + (y2 - y1) * (y - y1);
            if (cross <= 0) 
                //return Math.Sqrt((x - x1) * (x - x1) + (y - y1) * (y - y1));
                return GMapProviders.EmptyProvider.Projection.GetDistance(p, A) * 1000d;
            double d2 = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
            if (cross >= d2) 
                //return Math.Sqrt((x - x2) * (x - x2) + (y - y2) * (y - y2));
                return GMapProviders.EmptyProvider.Projection.GetDistance(p, B) * 1000d;
            double r = cross / d2;
            double px = x1 + (x2 - x1) * r;
            double py = y1 + (y2 - y1) * r;
            //return Math.Sqrt((x - px) * (x - px) + (py - y) * (py - y));
            return GMapProviders.EmptyProvider.Projection.GetDistance(p, new PointLatLng(py,px)) * 1000d;
        }
    }
}
