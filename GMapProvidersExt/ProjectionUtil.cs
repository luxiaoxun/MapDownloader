using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using OSGeo.OSR;

namespace GMapProvidersExt
{
    public static class ProjectionUtil
    {
        // Fields
        private static MemoryCache cache;

        // Methods
        public static double GetAxis(string wkt)
        {
            try
            {
                SpatialReference reference = new SpatialReference(wkt);
                return double.Parse(reference.GetAttrValue("SPHEROID", 1));
            }
            catch (Exception)
            {
                return 0.0;
            }
        }

        public static int GetEpsgCodeFromCRSString(string crs)
        {
            if (crs.ToLower().Contains("crs84"))
            {
                return 0x10e6;
            }
            string pattern = @"urn:ogc:def:crs:epsg:.*:(\w?)";
            Match match = new Regex(pattern).Match(crs);
            int result = 0xf11;
            if (match.Success && int.TryParse(match.Groups[1].Value, out result))
            {
                return result;
            }
            return result;
        }

        public static int GetEpsgCodeFromWKT(string wkt)
        {
            SpatialReference reference = new SpatialReference(wkt);
            if (reference.IsProjected() == 1)
            {
                string s = reference.GetAuthorityCode("PROJCS");
                if (s == null)
                {
                    return -1000;
                }
                return int.Parse(s);
            }
            if (reference.IsGeographic() != 1)
            {
                return -1000;
            }
            string authorityCode = reference.GetAuthorityCode("GEOGCS");
            if (authorityCode == null)
            {
                return -1000;
            }
            return int.Parse(authorityCode);
        }

        public static double GetFlattening(string wkt)
        {
            try
            {
                SpatialReference reference = new SpatialReference(wkt);
                return (1.0 / double.Parse(reference.GetAttrValue("SPHEROID", 2)));
            }
            catch (Exception)
            {
                return 0.0;
            }
        }

        public static MapUnit GetMapUnitFromWKT(string wkt)
        {
            string attrValue = new SpatialReference(wkt).GetAttrValue("UNIT", 0);
            if (attrValue.Equals("metre"))
            {
                attrValue = "meter";
            }
            try
            {
                return (MapUnit)Enum.Parse(typeof(MapUnit), attrValue, true);
            }
            catch (Exception)
            {
                string text1 = "通过WKT获取地图单位失败，返回默认单位：degree。\nwkt：" + wkt;
                return MapUnit.Degree;
            }
        }

        public static string GetWKTFromEpsgCode(int epsgCode)
        {
            string key = "epsgCode:" + epsgCode;
            object obj2 = Cache.Get(key, null);
            if (obj2 != null)
            {
                return (obj2 as string);
            }
            using (SpatialReference reference = new SpatialReference(""))
            {
                reference.ImportFromEPSG(epsgCode);
                string argout = string.Empty;
                reference.ExportToWkt(out argout);
                lock (Cache)
                {
                    if (Cache.Get(key, null) == null)
                    {
                        Cache.Add(new CacheItem(key, argout), new CacheItemPolicy());
                    }
                }
                return argout;
            }
        }

        public static string smethod_0(string sourceWkt)
        {
            string dstWkt = sourceWkt.ToString();
            if (sourceWkt.Contains("ESRI"))
            {
                string msg = string.Empty;
                if (!TryToTransESRIToWkt(sourceWkt, ref dstWkt, ref msg))
                {
                    dstWkt = sourceWkt.Replace("ESRI", "EPSG");
                }
            }
            return dstWkt;
        }

        public static bool TryToTransESRIToWkt(string esriProj, ref string dstWkt, ref string msg)
        {
            bool flag;
            try
            {
                using (SpatialReference reference = new SpatialReference(""))
                {
                    reference.ImportFromESRI(new string[] { esriProj });
                    if (reference.ExportToWkt(out dstWkt) == 1)
                    {
                        return false;
                    }
                    flag = true;
                }
            }
            catch (Exception exception)
            {
                msg = exception.Message;
                flag = false;
            }
            return flag;
        }

        // Properties
        public static MemoryCache Cache
        {
            get
            {
                if (cache == null)
                {
                    cache = MemoryCache.Default;
                }
                return cache;
            }
        }
    }

}
