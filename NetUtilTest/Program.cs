using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMapChinaRegion;
using NetUtil;
using GMap.NET;
using GMap.NET.MapProviders;
using GMapProvidersExt;
using GMapProvidersExt.Tencent;
using GMapPositionFix;
using NetUtil;

namespace NetUtilTest
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateNewFile();

            Console.ReadKey();
        }

        private static void GenerateNewFile()
        {
            string filePath  = "F:\\ChinaBoundaryBinary";
            Country china = JsonHelper.JsonDeserializeFromFile<Country>(filePath, Encoding.UTF8);
            foreach (var provice in china.Province)
            {
                if (provice.name == "海南省")
                {
                    string newRings = GetMapRegionInfo();
                    provice.rings = newRings;
                    break;
                }
            }
            JsonHelper.JsonSerializeToBinaryFile(china, "F:\\ChinaBoundary_Province_City");
        }

        static string GetMapRegionInfo()
        {
            string region = null;
            string newValue = "";
            bool success = MapRegion.regionDictionary.TryGetValue("海南", out region);
            if (success)
            {
                
                string[] points = region.Split(';');
                for (int i = 0; i < points.Length; ++i)
                {
                    string[] lnglat = points[i].Split(',');
                    newValue += lnglat[0];
                    newValue += " ";
                    newValue += lnglat[1];
                    if (i != points.Length - 1)
                    {
                        newValue += ",";
                    }
                }
                System.Console.WriteLine(newValue);
            }

            return newValue;
        }

        private static void TestMapProviderService()
        {
            //List<Placemark> placeList = new List<Placemark>();
            //placeList = SoSoMapProvider.Instance.GetPlacemarksByKeywords("东南大学");

            //foreach (var place in placeList)
            //{
            //    Console.WriteLine(place);
            //}
        }

        private static Country GetCountryDataFromFile(string file)
        {
            Country country = XmlHelper.XmlDeserializeFromFile<Country>(file, Encoding.UTF8);
            return country;
        }

        private static string GetPositionFromRings(ref string rings,string name)
        {
            if (rings == null)
            {
                Console.WriteLine(name);
                return "";
            }
            string[] res = rings.Split(';');
            return res[0];
        }

        private static void TestChinaJsonXml()
        {
            Country china = GetCountryDataFromFile(@"F:\GMap\china-province-city-piecearea.xml");
            for (int i = 0; i < china.Province.Count; ++i)
            {
                string ps = china.Province[i].rings;
                china.Province[i].rings = GetPositionFromRings(ref ps, china.Province[i].name);
                for (int j = 0; j < china.Province[i].City.Count; ++j)
                {
                    string cs = china.Province[i].City[j].rings;
                    china.Province[i].City[j].rings = GetPositionFromRings(ref cs, china.Province[i].City[j].name);
                    for (int k = 0; k < china.Province[i].City[j].Piecearea.Count; ++k)
                    {
                        string pps = china.Province[i].City[j].Piecearea[k].rings;
                        china.Province[i].City[j].Piecearea[k].rings = GetPositionFromRings(ref pps,
                            china.Province[i].City[j].Piecearea[k].name);
                    }
                }
            }

            //XmlHelper.XmlSerializeToFile(china, @"F:\GMap\china-province city2.xml", Encoding.UTF8);
            //JsonHelper.JsonSerializeToFile(china, @"F:\GMap\china-province city", Encoding.UTF8);

            //string file = "chinaBoundry";
            //Country china = GMapChinaRegion.ChinaMapRegion.GetChinaRegionFromJsonFile(file);

            //for (int i = 0; i < china.Province.Count; ++i )
            //{
            //    china.Province[i].rings = EncodeDecodeHelper.CompressString(china.Province[i].rings);
            //    for (int j = 0; j < china.Province[i].City.Count; ++j )
            //    {
            //        china.Province[i].City[j].rings = EncodeDecodeHelper.CompressString(china.Province[i].rings);
            //    }
            //}

            //JsonHelper.JsonSerializeToFile(china,"chinaBoundryEncode",Encoding.UTF8);

            JsonHelper.JsonSerializeToBinaryFile(china, "ChinaBoundryBinaryAll");
        }

        private static void TestPositionFix()
        {
            double lat_wgs = 22.873075;
            double lng_wgs = 113.918719;

            double lat_mars;
            double lng_mars;

            Console.WriteLine("WGS84坐标与火星坐标（GCJ02）之间的转换：");
            CoordinateTransform.ConvertWgs84ToGcj02(lng_wgs, lat_wgs, out lng_mars, out lat_mars);
            Console.WriteLine("Original WGS84 lat:{0},lng:{1}\r\nMars lat:{2},lng:{3}\r\n", lat_wgs, lng_wgs, lat_mars, lng_mars);

            double x;
            double y;
            CoordinateTransform.ConvertGcj02ToWgs84(lng_mars, lat_mars, out x, out y);
            Console.WriteLine("Original Mars lat:{0},lng:{1}\r\nWGS84 lat:{2},lng:{3}\r\n", lat_mars, lng_mars, y, x);


            double baidu_lng;
            double baidu_lat;
            Console.WriteLine("WGS84坐标与百度坐标（BD09）之间的转换：");
            CoordinateTransform.ConvertWgs84ToBd09(lng_wgs, lat_wgs, out baidu_lng, out baidu_lat);
            Console.WriteLine("Original WGS84 lat:{0},lng:{1}\r\nBaidu lat:{2},lng:{3}\r\n", lat_wgs, lng_wgs, baidu_lat, baidu_lng);

            double x1;
            double y1;
            CoordinateTransform.ConvertBd09ToWgs84(baidu_lng, baidu_lat, out x1, out y1);
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
