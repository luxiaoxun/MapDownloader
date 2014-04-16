using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMapPositionFix
{
    public static class MarsWGSTransform
    {
        /// <summary>
        /// 火星坐标转换为WGS坐标
        /// </summary>
        /// <param name="xMars">火星坐标经度</param>
        /// <param name="yMars">火星坐标纬度</param>
        /// <param name="xWgs">WGS经度</param>
        /// <param name="yWgs">WGS纬度</param>
        public static void ConvertMars2WGS(double xMars, double yMars, out double xWgs, out double yWgs)
        {
            xWgs = xMars;
            yWgs = yMars;
            double xtry = xMars;
            double ytry = yMars;
            ConvertWGS2Mars(xMars, yMars, out xtry, out ytry);
            double dx = xtry - xMars;
            double dy = ytry - yMars;

            xWgs = xMars - dx;
            yWgs = yMars - dy;
            return;
        }

        /// <summary>
        /// WGS坐标转换为火星坐标
        /// </summary>
        /// <param name="xWgs">WGS经度</param>
        /// <param name="yWgs">WGS纬度</param>
        /// <param name="xMars">火星坐标经度</param>
        /// <param name="yMars">火星坐标纬度</param>
        public static void ConvertWGS2Mars(double xWgs, double yWgs, out double xMars, out double yMars)
        {
            xMars = xWgs;
            yMars = yWgs;

            const double pi = 3.14159265358979324;

            //
            // Krasovsky 1940
            //
            // a = 6378245.0, 1/f = 298.3
            // b = a * (1 - f)
            // ee = (a^2 - b^2) / a^2;
            const double a = 6378245.0;
            const double ee = 0.00669342162296594323;

            if (xWgs < 72.004 || xWgs > 137.8347)
                return;
            if (yWgs < 0.8293 || yWgs > 55.8271)
                return;

            double x = 0, y = 0;
            x = xWgs - 105.0;
            y = yWgs - 35.0;

            double dLon = 300.0 + 1.0 * x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
            dLon += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            dLon += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
            dLon += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0 * pi)) * 2.0 / 3.0;

            double dLat = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
            dLat += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            dLat += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
            dLat += (160.0 * Math.Sin(y / 12.0 * pi) + 320.0 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;

            double radLat = yWgs / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            xMars = xWgs + dLon;
            yMars = yWgs + dLat;
        }
    }
}
