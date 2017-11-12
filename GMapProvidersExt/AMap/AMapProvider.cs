using System;
using GMap.NET;
using System.Collections.Generic;
using System.Web;
using GMap.NET.MapProviders;
using GMap.NET.Projections;
using GMap.NET.WindowsForms;
using NetUtil;
using Newtonsoft.Json.Linq;
using log4net;

namespace GMapProvidersExt.AMap
{
    public class AMapProvider : AMapProviderBase, RoutingProvider,GeocodingProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AMapProvider));
        private readonly string KEY = "26144cb5dbe74ea6c1410777feb646de";
        private int succeedCount;
        public delegate void QueryProgressDelegate(long completedCount, long total);

        public static readonly AMapProvider Instance;
   
        private readonly Guid id = new Guid("EF3DD303-3F74-4938-BF40-232D0595EE88");
        public override Guid Id
        {
            get { return id; }
        }

        private readonly string cnName = "高德地图";
        public string CnName
        {
            get
            {
                return this.cnName;
            }
        }

        private readonly string name = "AMap";
        public override string Name
        {
            get
            {
                return name;
            }
        }

        static AMapProvider()
        {
            Instance = new AMapProvider();
            GMapProviders.AddMapProvider(Instance);
        }

        public Placemark GetCenterNameByLocation(PointLatLng location)
        {
            GeoCoderStatusCode statusCode = new GeoCoderStatusCode();
            Placemark? place = this.GetPlacemark(location, out statusCode);
            if (place.HasValue)
            {
                return place.Value;
            }

            return Placemark.Empty;
        }

        public GeoCoderStatusCode GetPlacemarksByKeywords(string keywords, string region, string rectangle,
            QueryProgressDelegate queryProgressEvent, out List<Placemark> placemarkList, ref int count)
        {
            this.succeedCount = 0;
            placemarkList = this.GetPlacemarksByKeywords(keywords, region, rectangle, 1, queryProgressEvent, ref count);
            return GeoCoderStatusCode.G_GEO_SUCCESS;
        }

        private List<Placemark> GetPlacemarksByKeywords(string keywords, string region, string rectangle,
            int pageIndex, QueryProgressDelegate queryProgressEvent, ref int totalCount)
        {
            List<Placemark> list = new List<Placemark>();
            int pageSize = 50;
            string keyWordUrlEncode = HttpUtility.UrlEncode(keywords);
            string format = "http://restapi.amap.com/v3/place/text?&keywords={0}&city={1}&key={2}&output=json&offset={3}&page={4}";
            //http://restapi.amap.com/v3/place/text?&keywords=%E5%8C%97%E4%BA%AC%E5%A4%A7%E5%AD%A6&city=beijing&output=json&offset=5&page=1&key=26144cb5dbe74ea6c1410777feb646de&extensions=base
            if (!string.IsNullOrEmpty(region))
            {
                format = string.Format(format, new object[] { keyWordUrlEncode, HttpUtility.UrlEncode(region), KEY, pageSize, pageIndex });
            }
            //Get Cache Json Result if exist
            //string cacheUrl = string.Format("http://restapi.amap.com/v3/place/text/{0}/{1}/{2}/{3}", new object[] { keyWordUrlEncode, HttpUtility.UrlEncode(region), pageIndex, pageSize });
            //string cacheResult = Singleton<Cache>.Instance.GetContent(cacheUrl, CacheType.UrlCache, TimeSpan.FromHours(360.0));
            //if (string.IsNullOrEmpty(cacheResult))
            //{
            //    cacheResult = HttpUtil.RequestByJSON(format, "get", "");
            //    if (!string.IsNullOrEmpty(cacheResult))
            //    {
            //        Singleton<Cache>.Instance.SaveContent(cacheUrl, CacheType.UrlCache, cacheResult);
            //    }
            //}
            //if (string.IsNullOrEmpty(cacheResult))
            //{
            //    return list;
            //}
            try
            {
                string cacheResult = HttpUtil.GetData(format);
                JObject jsonResult = JObject.Parse(cacheResult);
                string info = (string)jsonResult["info"];
                if (info == "OK")
                {
                    if (pageIndex == 1)
                    {
                        string countStr = (string)jsonResult["count"];
                        totalCount = int.Parse(countStr);
                    }
                    if (totalCount <= 0) return list;

                    JArray results = (JArray)jsonResult["pois"];
                    if (results != null && results.Count > 0)
                    {
                        for (int i = 0; i < results.Count; ++i)
                        {
                            JObject obj = results[i] as JObject;
                            string name = obj["name"].ToString();
                            string address = obj["address"].ToString();
                            string location = obj["location"].ToString();
                            string[] points = location.Split(',');
                            if (points != null && points.Length == 2)
                            {
                                double lat = double.Parse(points[1]);
                                double lng = double.Parse(points[0]);
                                Placemark item = new Placemark(address);
                                item.Point = new PointLatLng(lat, lng);
                                item.Name = name;
                                //item.ProvinceName = obj["pname"].ToString();
                                //item.CityName = obj["cityname"].ToString();
                                //item.Category = obj["type"].ToString();
                                list.Add(item);
                                ++this.succeedCount;
                                if (queryProgressEvent != null)
                                {
                                    queryProgressEvent((long)this.succeedCount, (long)totalCount);
                                }
                            }
                        }
                    }
                    int allPageNum = (int)Math.Ceiling((double)(((double)totalCount) / ((double)pageSize)));
                    if (pageIndex < allPageNum)
                    {
                        list.AddRange(this.GetPlacemarksByKeywords(keywords, region, rectangle, pageIndex + 1, queryProgressEvent, ref totalCount));
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

        private List<Placemark> GetPlacemarksByLocation(PointLatLng location)
        {
            List<Placemark> list = new List<Placemark>();
            try
            {
                string reqUrl = string.Format("http://restapi.amap.com/v3/geocode/regeo?output=json&location={0}&key={1}", location.Lng + "," + location.Lat,KEY);
                string content = HttpUtil.GetData(reqUrl);
                JObject jsonObj = JObject.Parse(content);
                if (jsonObj != null && jsonObj["info"].ToString() == "OK")
                {
                    JObject regeocode = jsonObj["regeocode"] as JObject;
                    if (regeocode != null)
                    {
                        Placemark place = new Placemark();
                        place.Address = regeocode["formatted_address"].ToString();
                        JObject addressComponent = regeocode["addressComponent"] as JObject;
                        if (addressComponent != null)
                        {
                            place.ProvinceName = addressComponent["province"].ToString();
                            place.CityName = addressComponent["city"].ToString();
                            place.DistrictName = addressComponent["district"].ToString();
                        }
                        JObject streetNumber = regeocode["streetNumber"] as JObject;
                        if(streetNumber!=null)
                        {
                            place.StreetNumber = streetNumber["street"].ToString() + streetNumber["number"].ToString();
                            string loc = streetNumber["location"].ToString();
                            string[] points = loc.Split(',');
                            if (points != null && points.Length == 2)
                            {
                                double lat = double.Parse(points[1]);
                                double lng = double.Parse(points[0]);
                                PointLatLng p = new PointLatLng(lat, lng);
                                place.Point = p;
                            }
                        }
                        list.Add(place);
                    }
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
                string content = "";
                if (placemark.CityName != null)
                {
                    content = HttpUtil.GetData(string.Format("http://restapi.amap.com/v3/geocode/geo?city={0}&address={1}&output=json&key={2}", placemark.CityName, placemark.Address, KEY));
                }
                else
                {
                    content = HttpUtil.GetData(string.Format("http://restapi.amap.com/v3/geocode/geo?address={0}&output=json&key={1}", placemark.Address, KEY));
                }
                JObject jsonObj = JObject.Parse(content);
                if (jsonObj != null && jsonObj["info"].ToString() == "OK")
                {
                    int count = int.Parse(jsonObj["count"].ToString());
                    JArray geocodes = jsonObj["geocodes"] as JArray;
                    if (geocodes != null)
                    {
                        foreach (JObject geo in geocodes)
                        {
                            string location = geo["location"].ToString();
                            string[] points = location.Split(',');
                            if (points != null && points.Length == 2)
                            {
                                double lat = double.Parse(points[1]);
                                double lng = double.Parse(points[0]);
                                PointLatLng p = new PointLatLng(lat, lng);
                                list.Add(p);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Warn(ex.Message);
            }
            return list;
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

        public GeoCoderStatusCode GetPlacemarks(PointLatLng location, out List<Placemark> placemarkList)
        {
            placemarkList = this.GetPlacemarksByLocation(location);
            return GeoCoderStatusCode.G_GEO_SUCCESS;
        }

        public Placemark? GetPlacemark(PointLatLng location, out GeoCoderStatusCode status)
        {
            List<Placemark> placemarksByLocation = this.GetPlacemarksByLocation(location);
            if (placemarksByLocation != null && placemarksByLocation.Count > 0)
            {
                status = GeoCoderStatusCode.G_GEO_SUCCESS;
                return new Placemark(placemarksByLocation[0]);
            }
            status = GeoCoderStatusCode.Unknow;
            return null;
        }

        #endregion

        #region RoutingProvider Members

        public GMapRoute GetRoute(PointLatLng start, PointLatLng end,string cityName)
        {
            List<PointLatLng> points = new List<PointLatLng>();
            string origin = string.Format("{0},{1}", start.Lng, start.Lat);
            string destination = string.Format("{0},{1}", end.Lng, end.Lat);
            string city = HttpUtility.UrlEncode(cityName);
            string url = string.Format("http://restapi.amap.com/v3/direction/transit/integrated?origin={0}&destination={1}&city={2}&output=json&key={3}", origin, destination, city, KEY);
            string result = HttpUtil.GetData(url);
            if (!string.IsNullOrEmpty(result))
            {
                JObject resJosn = JObject.Parse(result);
                string isOk = resJosn["info"].ToString();
                if (isOk == "OK")
                {
                    JObject route = (JObject)resJosn["route"];
                    string ori = route["origin"].ToString();
                    string des = route["destination"].ToString();
                    string distance = route["distance"].ToString();
                    JObject transit = (JObject)route["transits"][0];//公交换乘方案
                    JArray segments = (JArray)transit["segments"]; //换乘路段列表
                    JObject firstSeg = (JObject)segments[0];
                    JObject walk = (JObject)firstSeg["walking"];
                    if (walk != null)
                    {
                        JArray steps = (JArray)walk["steps"];
                        foreach (JObject step in steps)
                        {
                            string polyline = step["polyline"].ToString();
                            string[] pointsString = polyline.Split(';');
                            foreach (string pStr in pointsString)
                            {
                                string[] pointStr = pStr.Split(',');
                                PointLatLng p = new PointLatLng(double.Parse(pointStr[1]), double.Parse(pointStr[0]));
                                points.Add(p);
                            }
                        }
                    }
                    JObject bus = (JObject)firstSeg["bus"];
                    if (bus != null)
                    {
                        JArray steps = (JArray)bus["buslines"];
                        //foreach (JObject step in steps)
                        if (steps != null && steps.Count>0)
                        {
                            JObject step = (JObject)steps[0];
                            string polyline = step["polyline"].ToString();
                            string[] pointsStr = polyline.Split(';');
                            foreach (string pStr in pointsStr)
                            {
                                string[] pointStr = pStr.Split(',');
                                PointLatLng p = new PointLatLng(double.Parse(pointStr[1]), double.Parse(pointStr[0]));
                                points.Add(p);
                            }
                        }
                    }
                }
            }
            
            GMapRoute mapRoute = points != null ? new GMapRoute(points, "") : null;
            return mapRoute;
        }

        public MapRoute GetRoute(PointLatLng start, PointLatLng end, bool avoidHighways, bool walkingMode, int Zoom)
        {
            string url = MakeRouteUrl(start,end);
            string result = HttpUtil.GetData(url);
            List<PointLatLng> points = new List<PointLatLng>();
            MapRoute route = points != null ? new MapRoute(points, "") : null;
            return route;
        }

        public MapRoute GetRoute(string start, string end, bool avoidHighways, bool walkingMode, int Zoom)
        {
            throw new NotImplementedException("Not Implemented !");
        }

        private string MakeRouteUrl(PointLatLng start, PointLatLng end)
        {
            //http://restapi.amap.com/v3/direction/transit/integrated?
            //origin=116.481499,39.990475&destination=116.465063,39.999538&city=010&output=xml&key=ea2c35e297482f2e57e1763168b4d522
            string origin = "116.481499,39.990475";
            string destination = "116.465063,39.999538";
            string city = HttpUtility.UrlEncode("北京市");
            string url = string.Format("http://restapi.amap.com/v3/direction/transit/integrated?origin={0}&destination={1}&city={2}&output=json&key={3}", origin,destination,city,KEY); 
            return url;
        }
        
        #endregion

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = MakeTileImageUrl(pos, zoom, LanguageStr);
                return GetTileImageUsingHttp(url);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            var num = (pos.X + pos.Y) % 4 + 1;
            string url = string.Format(UrlFormat, num, pos.X, pos.Y, zoom);
            return url;
        }
        //http://webrd01.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=7&x=3399&y=1664&z=12
        static readonly string UrlFormat = "http://webrd0{0}.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=8&x={1}&y={2}&z={3}";
    }
}
