using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GMap.NET;
using GMap.NET.Internals;
using GMap.NET.MapProviders;
using Newtonsoft.Json.Linq;
using System.Web;
using NetUtil;
using log4net;

namespace GMapProvidersExt.Tencent
{
    public class TencentMapProvider : TencentMapProviderBase, GeocodingProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TencentMapProvider));

        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("3C1FF6D8-F8AD-4A98-811A-027FB23314BB");
        public static readonly TencentMapProvider Instance;
        private readonly string KEY = "C7PBZ-6QFWJ-EAFFB-F7WK7-THHJO-T6FI3";
        private readonly string name;
        private int succeedCount;

        // Methods
        static TencentMapProvider()
        {
            Instance = new TencentMapProvider();
            GMapProviders.AddMapProvider(Instance);
        }

        private TencentMapProvider()
        {
            this.name = "SoSoMap";
            this.cnName = "腾讯街道地图";
        }

        private string GetCitySerchKey(JObject cityJson)
        {
            if (cityJson == null)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(" ");
            builder.Append(cityJson["cname"]);
            builder.Append(" ");
            if (cityJson["cities"] != null)
            {
                JArray array = cityJson["cities"] as JArray;
                for (int i = 0; i < array.Count; i++)
                {
                    builder.Append(this.GetCitySerchKey(array[i] as JObject));
                }
            }
            return builder.ToString();
        }

        private RectLatLng GetLatLngBox(PointLatLng center, object levelObj)
        {
            int result = 0x10;
            if (levelObj != null)
            {
                int.TryParse(levelObj.ToString(), out result);
            }
            double num2 = 360.0;
            double num3 = num2 / Math.Pow(2.0, (double)result);
            return RectLatLng.FromLTRB(center.Lng - (num3 / 2.0), center.Lat + (num3 / 2.0), center.Lng + (num3 / 2.0), center.Lat - (num3 / 2.0));
        }

        #region Search POI by keywords
        public delegate void QueryProgressDelegate(long completedCount, long total);

        public GeoCoderStatusCode GetPlacemarksByKeywords(string keywords, string region, string rectangle,
            string nearby, QueryProgressDelegate queryProgressEvent, out List<Placemark> placemarkList, ref int count)
        {
            this.succeedCount = 0;
            placemarkList = this.GetPlacemarksByKeywords(keywords, region, rectangle, nearby, 1, queryProgressEvent, ref count);
            return GeoCoderStatusCode.G_GEO_SUCCESS;
        }

        private List<Placemark> GetPlacemarksByKeywords(string keywords, string region, string rectangle,
            string nearby, int pageIndex, QueryProgressDelegate queryProgressEvent, ref int count)
        {
            List<Placemark> list = new List<Placemark>();
            int pageSize = 20;
            string keyWordUrlEncode = HttpUtility.UrlEncode(keywords);
            string format = "http://apis.map.qq.com/ws/place/v1/search?keyword={0}&boundary={5}({1})&page_size={2}&page_index={3}&key={4}";
            if (!string.IsNullOrEmpty(region))
            {
                format = string.Format(format, new object[] { keyWordUrlEncode, HttpUtility.UrlEncode(region) + ",0", pageSize, pageIndex, KEY, "region" });
            }
            else if (!string.IsNullOrEmpty(rectangle))
            {
                format = string.Format(format, new object[] { keyWordUrlEncode, rectangle, pageSize, pageIndex, KEY, "rectangle" });
            }
            else if (!string.IsNullOrEmpty(nearby))
            {
                format = string.Format(format, new object[] { keyWordUrlEncode, nearby, pageSize, pageIndex, KEY, "nearby" });
            }
            //string cacheUrl = string.Format("http://apis.map.qq.com/{0}/{1}/{2}/{3}/{4}", new object[] { keyWordUrlEncode, HttpUtility.UrlEncode(region), rectangle, nearby, pageIndex });
            //string cacheResult = Singleton<Cache>.Instance.GetContent(cacheUrl, CacheType.UrlCache, TimeSpan.FromHours(360.0));
            //if (string.IsNullOrEmpty(cacheResult))
            //{
            //    cacheResult = HttpUtil.GetData(format, "utf-8", "get", "", "text/htm");
            //    Thread.Sleep(200);
            //    if (!string.IsNullOrEmpty(cacheResult))
            //    {
            //        Singleton<Cache>.Instance.SaveContent(cacheUrl, CacheType.UrlCache, cacheResult);
            //    }
            //}
            //if (string.IsNullOrEmpty(cacheResult))
            //{
            //    return list;
            //}
            string cacheResult = HttpUtil.GetData(format);
            JObject result = JObject.Parse(cacheResult);
            string status = (string)result["status"];
            string message = (string)result["message"];
            if (message == "query ok")
            {
                if (pageIndex == 1)
                {
                    int.TryParse(result["count"].ToString(), out count);
                }
                if (count <= 0) return list;
                
                JArray data = (JArray)result["data"];
                if (data != null && data.Count >= 0)
                {
                    for (int i = 0; i < data.Count; ++i)
                    {
                        JObject obj = data[i] as JObject;
                        string name = obj["title"].ToString();
                        string address = obj["address"].ToString();
                        double lat = double.Parse(obj["location"]["lat"].ToString());
                        double lng = double.Parse(obj["location"]["lng"].ToString());
                        Placemark item = new Placemark(address);
                        item.Point = new PointLatLng(lat, lng);
                        item.Name = name;
                        list.Add(item);
                        ++this.succeedCount;
                        if (queryProgressEvent != null)
                        {
                            queryProgressEvent((long)this.succeedCount, (long)count);
                        }
                    }
                }

                int allPageNum = (int)Math.Ceiling((double)(((double)count) / ((double)pageSize)));
                if (pageIndex < allPageNum)
                {
                    list.AddRange(this.GetPlacemarksByKeywords(keywords, region, rectangle, nearby, pageIndex + 1, queryProgressEvent, ref count));
                }
            }
            return list;
        }

        #endregion

        private string GetJsonContent(string content)
        {
            int index = content.IndexOf('(');
            string str = content.Substring(index + 1).Trim();
            if (str.EndsWith(")"))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

        public Placemark GetCenterNameByLocation(PointLatLng location)
        {
            Placemark place = new Placemark();
            place.Address = "";
            try
            {
                string url = string.Format("http://api.map.qq.com/rgeoc/?lnglat={0}%2C{1}&output=jsonp&fr=mapapi&cb=SOSOMapLoader.geocoder0", location.Lng, location.Lat);
                string content = HttpUtil.GetData(url);
                string jsonContent = this.GetJsonContent(content);
                JObject jsonObj = JObject.Parse(jsonContent)["detail"] as JObject;
                List<Placemark> list = new List<Placemark>();
                if (jsonObj["results"] != null)
                {
                    JArray array = jsonObj["results"] as JArray;
                    if (array != null && array.Count > 0)
                    {
                        JObject firstRes = (JObject)array[0];
                        string address = firstRes["name"].ToString();
                        place.Address = address;
                        place.CountryName = firstRes["n"].ToString();
                        place.ProvinceName = firstRes["p"].ToString();
                        place.CityName = firstRes["c"].ToString();
                        place.DistrictName = firstRes["d"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex.Message);
            }
            return place;
        }

        private List<Placemark> GetPlacemarksByLocationBack(PointLatLng location)
        {
            string url = string.Format("http://api.map.qq.com/rgeoc/?lnglat={0}%2C{1}&output=jsonp&fr=mapapi&cb=SOSOMapLoader.geocoder0", location.Lng, location.Lat);
            string content = HttpUtil.GetData(url);
            string jsonContent = this.GetJsonContent(content);
            JObject jsonObj = JObject.Parse(jsonContent)["detail"] as JObject;
            List<Placemark> list = new List<Placemark>();
            if (jsonObj["results"] != null)
            {
                JArray array = jsonObj["results"] as JArray;
                for (int i = 0; i < array.Count; i++)
                {
                    Placemark placemark;
                    JObject objResult = array[i] as JObject;
                    string address = objResult["name"].ToString();
                    if (objResult["addr"] != null)
                    {
                        address = address + "(" + objResult["addr"].ToString() + ")";
                    }
                    placemark = new Placemark(address);
                    placemark.Point = new PointLatLng(double.Parse(objResult["pointy"].ToString()),
                        double.Parse(objResult["pointx"].ToString()));
                    placemark.LatLonBox = this.GetLatLngBox(placemark.Point, "11");

                    list.Add(placemark);
                }
            }
            return list;
        }

        private List<Placemark> GetPlacemarksByLocation(PointLatLng location)
        {
            List<Placemark> list = new List<Placemark>();
            try
            {
                string url = string.Format("http://apis.map.qq.com/ws/geocoder/v1/?location={0}&get_poi={1}&key={2}", location.Lat + "," + location.Lng, 0, KEY);
                string content = HttpUtil.GetData(url);
                JObject jsonObj = JObject.Parse(content);
                if (jsonObj != null && jsonObj["result"] != null)
                {
                    Placemark place = new Placemark();
                    JObject locaton = jsonObj["result"]["location"] as JObject;
                    if (locaton != null)
                    {
                        double lat = double.Parse(locaton["lat"].ToString());
                        double lng = double.Parse(locaton["lng"].ToString());
                        PointLatLng p = new PointLatLng(lat, lng);
                        place.Point = p;
                    }
                    place.Address = jsonObj["result"]["address"].ToString();
                    list.Add(place);
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex.Message);
            }

            return list;
        }

        private List<PointLatLng> GetPointsByPlacemark(Placemark placemark)
        {
            List<PointLatLng> list = new List<PointLatLng>();
            try
            {
                string url = string.Format("http://apis.map.qq.com/ws/geocoder/v1/?region={0}&address={1}&key={2}", placemark.CityName, placemark.Address, KEY);
                string content = HttpUtil.GetData(url);
                JObject jsonObj = JObject.Parse(content);
                if (jsonObj != null && jsonObj["result"] != null)
                {
                    JObject locaton = jsonObj["result"]["location"] as JObject;
                    if (locaton != null)
                    {
                        double lat = double.Parse(locaton["lat"].ToString());
                        double lng = double.Parse(locaton["lng"].ToString());
                        PointLatLng p = new PointLatLng(lat, lng);
                        list.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex.Message);
            }
            return list;
        }

        #region GeocodingProvider Members

        public Placemark? GetPlacemark(PointLatLng location, out GeoCoderStatusCode status)
        {
            List<Placemark> placemarksByLocation = this.GetPlacemarksByLocation(location);
            if ((placemarksByLocation != null) && (placemarksByLocation.Count > 0))
            {
                status = GeoCoderStatusCode.G_GEO_SUCCESS;
                return new Placemark(placemarksByLocation[0]);
            }
            status = GeoCoderStatusCode.Unknow;
            return null;
        }

        public GeoCoderStatusCode GetPlacemarks(PointLatLng location, out List<Placemark> placemarkList)
        {
            placemarkList = this.GetPlacemarksByLocation(location);
            return GeoCoderStatusCode.G_GEO_SUCCESS;
        }

        public GeoCoderStatusCode GetPoints(string keywords, out List<PointLatLng> pointList)
        {
            throw new Exception("未实现");
        }

        public PointLatLng? GetPoint(string keywords, out GeoCoderStatusCode status)
        {
            throw new Exception("未实现");
        }

        public GeoCoderStatusCode GetPoints(Placemark placemark, out List<PointLatLng> pointList)
        {
            pointList = this.GetPointsByPlacemark(placemark);
            return GeoCoderStatusCode.G_GEO_SUCCESS;
        }

        public PointLatLng? GetPoint(Placemark placemark, out GeoCoderStatusCode status)
        {
            List<PointLatLng> points = this.GetPointsByPlacemark(placemark);
            if (points != null && points.Count > 0)
            {
                status = GeoCoderStatusCode.G_GEO_SUCCESS;
                return points[0];
            }

            status = GeoCoderStatusCode.Unknow;
            return null;
        }

        #endregion

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                int serverIndex = GMapProvider.GetServerNum(pos, TencentMapProviderBase.maxServer);
                string url = string.Format(UrlFormat, new object[] { serverIndex, "maptilesv2", base.GetSosoMapTileNo(pos, zoom) });
                return base.GetTileImageUsingHttp(url);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Properties
        public string CnName
        {
            get
            {
                return this.cnName;
            }
        }

        public override Guid Id
        {
            get
            {
                return this.id;
            }
        }

        public override string Name
        {
            get
            {
                return this.name;
            }
        }

        static readonly string UrlFormat = "http://p{0}.map.soso.com/{1}/{2}.png";
    }

}
