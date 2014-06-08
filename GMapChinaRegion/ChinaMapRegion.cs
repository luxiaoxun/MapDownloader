using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using NetUtilityLib;

namespace GMapChinaRegion
{
    public static class ChinaMapRegion
    {
        public static Country GetChinaRegionFromFile(string filePath)
        {
            Country china = JsonHelper.JsonDeserializeFromFile<Country>(filePath, Encoding.UTF8);
            return china;
        }

        public static RectLatLng GetRegionMaxRect(GMapPolygon polygon)
        {
            double latMin = 90;
            double latMax = -90;
            double lngMin = 180;
            double lngMax = -180;
            foreach (var point in polygon.Points)
            {
                if (point.Lat < latMin)
                {
                    latMin = point.Lat;
                }
                else if (point.Lat > latMax)
                {
                    latMax = point.Lat;
                }
                else if (point.Lng < lngMin)
                {
                    lngMin = point.Lng;
                }
                else if (point.Lng > lngMax)
                {
                    lngMax = point.Lng;
                }
            }

            return new RectLatLng(latMax, lngMin, lngMax - lngMin, latMax - latMin);
        }

        public static GMapPolygon GetRegionPolygon(string name, string rings)
        {
            if (string.IsNullOrEmpty(rings))
            {
                return null;
            }
            else
            {
                List<PointLatLng> pointList = new List<PointLatLng>();
                string[] pairPoints = rings.Split(',');
                foreach (var points in pairPoints)
                {
                    string[] point = points.Split(' ');
                    if (point.Length == 2)
                    {
                        PointLatLng p = new PointLatLng(double.Parse(point[1]), double.Parse(point[0]));
                        pointList.Add(p);
                    }
                }
                GMapPolygon polygon = new GMapPolygon(pointList, name);
                polygon.Fill = new SolidBrush(Color.FromArgb(0, Color.White));
                return polygon;
            }
        }
    }
}
