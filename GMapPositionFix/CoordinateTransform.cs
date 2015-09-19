using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMapPositionFix
{
    /// <summary>
    /// 各坐标系统转换 
    /// WGS84坐标系：即地球坐标系，国际上通用的坐标系。设备一般包含GPS芯片或者北斗芯片获取的经纬度为WGS84地理坐标系, 
    /// 谷歌地图采用的是WGS84地理坐标系（中国范围除外）; 
    /// GCJ02坐标系：即火星坐标系，是由中国国家测绘局制订的地理信息系统的坐标系统。由WGS84坐标系经加密后的坐标系。 
    /// 高德地图、腾讯地图、谷歌中国地图采用的是GCJ02地理坐标系; 
    /// BD09坐标系：即百度坐标系，GCJ02坐标系经加密后的坐标系; 
    /// 搜狗坐标系、图吧坐标系等，估计也是在GCJ02基础上加密而成的。
    /// </summary>
    public static class CoordinateTransform
    {
        /// <summary>
        /// 检查坐标是否在中国范围内
        /// </summary>
        /// <param name="lat">latitude 纬度</param>
        /// <param name="lng">longitude 经度</param>
        /// <returns></returns>
        private static bool OutOfChina(double lat, double lng)
        {
            if (lng < 72.004 || lng > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }

        /// <summary>
        /// 火星坐标(GCJ02)转换为WGS坐标
        /// </summary>
        /// <param name="lngMars">火星坐标经度longitude</param>
        /// <param name="latMars">火星坐标纬度latitude</param>
        /// <param name="lngWgs">WGS经度</param>
        /// <param name="latWgs">WGS纬度</param>
        public static void ConvertGcj02ToWgs84(double lngMars, double latMars, out double lngWgs, out double latWgs)
        {
            lngWgs = lngMars;
            latWgs = latMars;
            double lngtry = lngMars;
            double lattry = latMars;
            ConvertWgs84ToGcj02(lngMars, latMars, out lngtry, out lattry);
            double dx = lngtry - lngMars;
            double dy = lattry - latMars;

            lngWgs = lngMars - dx;
            latWgs = latMars - dy;
        }

        /// <summary>
        /// WGS坐标转换为火星坐标(GCJ02)
        /// </summary>
        /// <param name="lngWgs">WGS经度</param>
        /// <param name="latWgs">WGS纬度</param>
        /// <param name="lngMars">火星坐标经度</param>
        /// <param name="latMars">火星坐标纬度</param>
        public static void ConvertWgs84ToGcj02(double lngWgs, double latWgs, out double lngMars, out double latMars)
        {
            lngMars = lngWgs;
            latMars = latWgs;

            const double pi = 3.14159265358979324;

            //
            // Krasovsky 1940
            //
            // a = 6378245.0, 1/f = 298.3
            // b = a * (1 - f)
            // ee = (a^2 - b^2) / a^2;
            const double a = 6378245.0;
            const double ee = 0.00669342162296594323;

            if (lngWgs < 72.004 || lngWgs > 137.8347)
                return;
            if (latWgs < 0.8293 || latWgs > 55.8271)
                return;

            double x = 0, y = 0;
            x = lngWgs - 105.0;
            y = latWgs - 35.0;

            double dLon = 300.0 + 1.0 * x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
            dLon += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            dLon += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
            dLon += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0 * pi)) * 2.0 / 3.0;

            double dLat = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
            dLat += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            dLat += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
            dLat += (160.0 * Math.Sin(y / 12.0 * pi) + 320.0 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;

            double radLat = latWgs / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            lngMars = lngWgs + dLon;
            latMars = latWgs + dLat;
        }

        /// <summary>
        /// 火星坐标(GCJ02)转换为baidu坐标
        /// </summary>
        /// <param name="lngMars"></param>
        /// <param name="latMars"></param>
        /// <param name="lngBaidu"></param>
        /// <param name="latBaidu"></param>
        public static void ConvertGcj02ToBd09(double lngMars, double latMars, out double lngBaidu, out double latBaidu)
        {
            const double x_pi = 3.14159265358979324 * 3000.0 / 180.0;
            //const double x_pi = 3.14159265358979324;
            double x = lngMars;
            double y = latMars;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
            lngBaidu = z * Math.Cos(theta) + 0.0065;
            latBaidu = z * Math.Sin(theta) + 0.006;
        }

        /// <summary>
        /// baidu坐标转换为火星坐标(GCJ02)
        /// </summary>
        /// <param name="lngBaidu"></param>
        /// <param name="latBaidu"></param>
        /// <param name="lngMars"></param>
        /// <param name="latMars"></param>
        public static void ConvertBd09ToGcj02(double lngBaidu, double latBaidu, out double lngMars, out double latMars)
        {
            const double x_pi = 3.14159265358979324 * 3000.0 / 180.0;

            double x = lngBaidu - 0.0065;
            double y = latBaidu - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            lngMars = z * Math.Cos(theta);
            latMars = z * Math.Sin(theta);
        }

        /// <summary>
        /// WGS坐标转换为baidu坐标
        /// </summary>
        /// <param name="lngWgs"></param>
        /// <param name="latWgs"></param>
        /// <param name="lngBaidu"></param>
        /// <param name="latBaidu"></param>
        public static void ConvertWgs84ToBd09(double lngWgs, double latWgs, out double lngBaidu, out double latBaidu)
        {
            double lng;
            double lat;
            ConvertWgs84ToGcj02(lngWgs, latWgs, out lng, out lat);
            ConvertGcj02ToBd09(lng, lat, out lngBaidu, out latBaidu);
        }

        /// <summary>
        /// baidu坐标转换为WGS坐标
        /// </summary>
        /// <param name="lngBaidu"></param>
        /// <param name="latBaidu"></param>
        /// <param name="lngWgs"></param>
        /// <param name="latWgs"></param>
        public static void ConvertBd09ToWgs84(double lngBaidu, double latBaidu,out double lngWgs, out double latWgs)
        {
            double lng;
            double lat;
            ConvertBd09ToGcj02(lngBaidu, latBaidu, out lng, out lat);
            ConvertGcj02ToWgs84(lng, lat, out lngWgs, out latWgs);
        }
    }
}
