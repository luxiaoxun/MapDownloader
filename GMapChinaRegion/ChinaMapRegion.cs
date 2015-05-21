using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using NetUtil;

namespace GMapChinaRegion
{
    public static class ChinaMapRegion
    {
        public static Country GetChinaRegionFromJsonBinaryBytes(byte[] buffer)
        {
            Country china = JsonHelper.JsonDeserializeFromBinaryBytes<Country>(buffer);
            return china;
        }

        public static Country GetChinaRegionFromJsonFile(string filePath)
        {
            Country china = JsonHelper.JsonDeserializeFromFile<Country>(filePath, Encoding.UTF8);
            return china;
        }

        public static Country GetChinaRegionFromXmlFile(string filePath)
        {
            Country china = XmlHelper.XmlDeserializeFromFile<Country>(filePath, Encoding.UTF8);
            return china;
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
