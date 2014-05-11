using System;
using GMapPositionFix;

namespace GMapFixTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //double lat_wgs = 34.121;
            //double lng_wgs = 115.21212;

            double lat_wgs = 22.873075;
            double lng_wgs = 113.918719;

            double lat_mars;
            double lng_mars;
           
            Console.WriteLine("WGS84坐标与火星坐标（GCJ02）之间的转换：");
            CoordinateTransform.ConvertWGSToMars(lng_wgs, lat_wgs, out lng_mars, out lat_mars);
            Console.WriteLine("Original WGS84 lat:{0},lng:{1}\r\nMars lat:{2},lng:{3}\r\n", lat_wgs, lng_wgs, lat_mars, lng_mars);

            double x;
            double y;
            CoordinateTransform.ConvertMarsToWGS(lng_mars, lat_mars, out x, out y);
            Console.WriteLine("Original Mars lat:{0},lng:{1}\r\nWGS84 lat:{2},lng:{3}\r\n", lat_mars, lng_mars, y, x);


            double baidu_lng;
            double baidu_lat;
            Console.WriteLine("WGS84坐标与百度坐标（BD09）之间的转换：");
            CoordinateTransform.ConvertWGSToBD09(lng_wgs, lat_wgs, out baidu_lng, out baidu_lat);
            Console.WriteLine("Original WGS84 lat:{0},lng:{1}\r\nBaidu lat:{2},lng:{3}\r\n", lat_wgs, lng_wgs, baidu_lat, baidu_lng);

            double x1;
            double y1;
            CoordinateTransform.ConvertBD09ToWGS(baidu_lng, baidu_lat, out x1, out y1);
            Console.WriteLine("Original Baidu lat:{0},lng:{1}\r\nWGS84 lat:{2},lng:{3}\r\n", baidu_lat, baidu_lng, y1, x1);

            //CoordinateTransform.ConvertMarsToBD09(lng,lat,out x,out y);
            //Console.WriteLine("Original Mars lat:{0},lng:{1}\r\nBaidu lat:{2},lng:{3}\r\n", lat, lng, y, x);

            //double baidu_x;
            //double baidu_y;
            //CoordinateTransform.ConvertBD09ToMars(x, y, out baidu_x, out baidu_y);
            //Console.WriteLine("Original Baidu lat:{0},lng:{1}\r\nMars lat:{2},lng:{3}\r\n", y, x,baidu_y,baidu_x);


            Console.ReadKey();
        }
    }
}
