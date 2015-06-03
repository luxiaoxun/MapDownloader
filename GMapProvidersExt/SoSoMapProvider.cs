using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GMap.NET;
using GMap.NET.Internals;
using GMap.NET.MapProviders;
using GMapCommonType;
using Newtonsoft.Json.Linq;
using System.Web;
using NetUtil;

namespace GMapProvidersExt
{
    public class SoSoMapProvider : SoSoMapProviderBase, GeocodingProvider
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id =new Guid("8e3e6b8b-59f0-4fca-b6b0-66e63acc3665");
        public static readonly SoSoMapProvider Instance;
        private readonly string KEY;
        private object lockObj;
        private readonly string name;
        public bool RunPoiQuery;
        private int succeedCount;

        // Methods
        static SoSoMapProvider()
        {
            Instance = new SoSoMapProvider();
        }

        private SoSoMapProvider()
        {
            this.KEY = "QFOBZ-PG4R4-FKTUX-XTTGJ-P6NZV-IFBD3";
            //this.id = new Guid("8e3e6b8b-59f0-4fca-b6b0-66e63acc3665");
            this.name = "SoSoMap";
            this.cnName = "搜搜街道地图";
            this.lockObj = new object();
            this.RunPoiQuery = true;
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

        public Placemark? GetPlacemark(PointLatLng location, out GeoCoderStatusCode status)
        {
            status = GeoCoderStatusCode.G_GEO_SUCCESS;
            List<Placemark> placemarksByLocation = this.GetPlacemarksByLocation(location);
            if ((placemarksByLocation != null) && (placemarksByLocation.Count > 0))
            {
                return new Placemark?(placemarksByLocation[0]);
            }
            return null;
        }

        public GeoCoderStatusCode GetPlacemarks(PointLatLng location, out List<Placemark> placemarkList)
        {
            placemarkList = this.GetPlacemarksByLocation(location);
            return GeoCoderStatusCode.G_GEO_SUCCESS;
        }

        public List<Placemark> GetPlacemarksByKeywords(string keywords)
        {
            List<Placemark> list = new List<Placemark>();
            string content = HttpUtil.Request(string.Format("http://api.map.qq.com/?qt=poi&wd={0}&pn=1&rn=10&c=1&output=jsonp&fr=mapapi&cb=SOSOMapLoader.search_service_0", HttpUtility.UrlEncode(keywords)), "gb2312", "get", "", "text/htm");
            JObject obj3 = (JObject)JObject.Parse(this.GetSOSOGeocodJSON(content))["detail"];
            if ((obj3["area"] != null) && (obj3["result"] == null))
            {
                list.Add(this.ParseCity(obj3["city"] as JObject));
                return list;
            }
            if ((obj3["result"] != null) && (obj3.Count == 2))
            {
                JArray array = obj3["result"] as JArray;
                for (int i = 0; i < array.Count; i++)
                {
                    list.Add(this.ParseCity(array[i] as JObject));
                }
                return list;
            }
            if ((obj3["result"] != null) && (obj3["area"] != null))
            {
                JArray array2 = obj3["result"] as JArray;
                for (int j = 0; j < array2.Count; j++)
                {
                    string str4 = this.GetCitySerchKey(array2[j] as JObject) + keywords;
                    if (str4.Length < 20)
                    {
                        list.AddRange(this.GetPlacemarksByKeywords(str4));
                    }
                }
                return list;
            }
            if (obj3["pois"] != null)
            {
                JArray array3 = obj3["pois"] as JArray;
                if (array3 == null)
                {
                    return list;
                }
                for (int k = 0; k < array3.Count; k++)
                {
                    JObject obj4 = array3[k] as JObject;
                    string address = string.Concat(new object[] { obj4["name"], "(", obj4["addr"].ToString(), ")" });
                    Placemark item = new Placemark(address);
                    PointLatLng center = new PointLatLng(double.Parse(obj4["pointy"].ToString()), double.Parse(obj4["pointx"].ToString()));
                    item.LatLonBox = this.GetLatLngBox(center, obj4["level"]);
                    item.Point = center;
                    list.Add(item);
                }
            }
            return list;
        }

        public GeoCoderStatusCode GetPlacemarksByKeywords(string keywords, out List<Placemark> placemarkList)
        {
            placemarkList = this.GetPlacemarksByKeywords(keywords);
            return GeoCoderStatusCode.G_GEO_SUCCESS;
        }

        public delegate void QueryProgressDelegate(long completedCount, long total);

        public GeoCoderStatusCode GetPlacemarksByKeywords(string keywords, string region, string rectangle, 
            string nearby, string key, QueryProgressDelegate queryProgressEvent, out List<Placemark> placemarkList, ref int count)
        {
            this.succeedCount = 0;
            placemarkList = this.GetPlacemarksByKeywords(keywords, region, rectangle, nearby, key, 1, queryProgressEvent, ref count);
            return GeoCoderStatusCode.G_GEO_SUCCESS;
        }

        private List<Placemark> GetPlacemarksByKeywords(string keywords, string region, string rectangle, 
            string nearby, string key, int pageIndex, QueryProgressDelegate queryProgressEvent, ref int count)
        {
            List<Placemark> list = new List<Placemark>();
            if (this.succeedCount <= 5000)//最多5000个
            {
                if (!this.RunPoiQuery)
                {
                    return list;
                }
                if (string.IsNullOrEmpty(key))
                {
                    key = this.KEY;
                }
                int pageNum = 20;
                string keyWordUrlEncode = HttpUtility.UrlEncode(keywords);
                string format = "http://apis.map.qq.com/ws/place/v1/search?keyword={0}&boundary={5}({1})&page_size={2}&page_index={3}&key={4}";
                if (!string.IsNullOrEmpty(region))
                {
                    format = string.Format(format, new object[] { keyWordUrlEncode, HttpUtility.UrlEncode(region) + ",0", pageNum, pageIndex, key, "region" });
                }
                else if (!string.IsNullOrEmpty(rectangle))
                {
                    format = string.Format(format, new object[] { keyWordUrlEncode, rectangle, pageNum, pageIndex, key, "rectangle" });
                }
                else if (!string.IsNullOrEmpty(nearby))
                {
                    format = string.Format(format, new object[] { keyWordUrlEncode, nearby, pageNum, pageIndex, key, "nearby" });
                }
                string url = string.Format("http://apis.map.qq.com/{0}/{1}/{2}/{3}/{4}", new object[] { keyWordUrlEncode, HttpUtility.UrlEncode(region), rectangle, nearby, pageIndex });
                string cacheResult = Singleton<Cache>.Instance.GetContent(url, CacheType.UrlCache, TimeSpan.FromHours(360.0));
                if (string.IsNullOrEmpty(cacheResult))
                {
                    cacheResult = HttpUtil.Request(format, "utf-8", "get", "", "text/htm");
                    Thread.Sleep(200);
                    if (!string.IsNullOrEmpty(cacheResult))
                    {
                        Singleton<Cache>.Instance.SaveContent(url, CacheType.UrlCache, cacheResult);
                    }
                }
                if (string.IsNullOrEmpty(cacheResult))
                {
                    return list;
                }
                JObject obj2 = JObject.Parse(cacheResult);
                JArray array = (JArray)obj2["data"];
                if ((array == null) || (array.Count <= 0))
                {
                    JArray array2 = (JArray)obj2["cluster"];
                    if ((array2 != null) && (array2.Count > 0))
                    {
                        for (int i = 0; i < array2.Count; i++)
                        {
                            JObject obj3 = array2[i] as JObject;
                            string str4 = obj3["title"].ToString();
                            list.AddRange(this.GetPlacemarksByKeywords(keywords, str4, rectangle, nearby, key, 1, queryProgressEvent, ref count));
                        }
                    }
                    return list;
                }
                int.TryParse(obj2["count"].ToString(), out count);
                int arrIndex = 0;
                while (true)
                {
                    if (arrIndex >= array.Count)
                    {
                        break;
                    }
                    try
                    {
                        this.succeedCount++;
                        JObject obj4 = array[arrIndex] as JObject;
                        string address = obj4["address"].ToString();
                        Placemark item = new Placemark(address)
                        {
                            Name = obj4["title"].ToString(),
                            Tel = obj4["tel"].ToString(),
                            Category = obj4["category"].ToString()
                        };
                        PointLatLng lng = new PointLatLng(double.Parse(obj4["location"]["lat"].ToString()), double.Parse(obj4["location"]["lng"].ToString()));
                        item.Point = lng;
                        list.Add(item);
                        if (queryProgressEvent != null)
                        {
                            queryProgressEvent((long)this.succeedCount, (long)count);
                        }
                    }
                    catch
                    {
                    }
                    arrIndex++;
                }
                int allPageNum = (int)Math.Ceiling((double)(((double)count) / ((double)arrIndex)));
                if (pageIndex < allPageNum)
                {
                    list.AddRange(this.GetPlacemarksByKeywords(keywords, region, rectangle, nearby, key, pageIndex + 1, queryProgressEvent, ref count));
                }
            }
            return list;
        }

        private List<Placemark> GetPlacemarksByLocation(PointLatLng location)
        {
            string content = HttpUtil.Request(string.Format("http://api.map.qq.com/rgeoc/?lnglat={0}%2C{1}&output=jsonp&fr=mapapi&cb=SOSOMapLoader.geocoder0", location.Lng, location.Lat), "gb2312", "get", "", "text/htm");
            JObject obj3 = JObject.Parse(this.GetSOSOGeocodJSON(content))["detail"] as JObject;
            List<Placemark> list = new List<Placemark>();
            if (obj3["results"] != null)
            {
                JArray array = obj3["results"] as JArray;
                for (int i = 0; i < array.Count; i++)
                {
                    Placemark placemark;
                    JObject obj4 = array[i] as JObject;
                    string address = obj4["name"].ToString();
                    if (obj4["addr"] != null)
                    {
                        address = address + "(" + obj4["addr"].ToString() + ")";
                    }
                    placemark = new Placemark(address);
                    placemark.Point = new PointLatLng(double.Parse(obj4["pointy"].ToString()),
                        double.Parse(obj4["pointx"].ToString()));
                    placemark.LatLonBox = this.GetLatLngBox(placemark.Point, "11");
                    
                    list.Add(placemark);
                }
            }
            return list;
        }

        GMap.NET.Placemark? GeocodingProvider.GetPlacemark(PointLatLng location, out GeoCoderStatusCode status)
        {
            throw new NotImplementedException();
        }

        PointLatLng? GeocodingProvider.GetPoint(GMap.NET.Placemark placemark, out GeoCoderStatusCode status)
        {
            throw new NotImplementedException();
        }

        public GeoCoderStatusCode GetPlacemarks(PointLatLng location, out List<GMap.NET.Placemark> placemarkList)
        {
            throw new NotImplementedException();
        }

        public GeoCoderStatusCode GetPoints(GMap.NET.Placemark placemark, out List<PointLatLng> pointList)
        {
            throw new NotImplementedException();
        }

        public PointLatLng? GetPoint(Placemark placemark, out GeoCoderStatusCode status)
        {
            throw new Exception("未实现");
        }

        public PointLatLng? GetPoint(string keywords, out GeoCoderStatusCode status)
        {
            throw new Exception("未实现");
        }

        public GeoCoderStatusCode GetPoints(Placemark placemark, out List<PointLatLng> pointList)
        {
            throw new Exception("未实现");
        }

        public GeoCoderStatusCode GetPoints(string keywords, out List<PointLatLng> pointList)
        {
            throw new Exception("未实现");
        }

        private string GetSOSOGeocodJSON(string content)
        {
            int index = content.IndexOf('(');
            string str = content.Substring(index + 1).Trim();
            if (str.EndsWith(")"))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = string.Format(SoSoMapProviderBase.UrlFormat, new object[] { GMapProvider.GetServerNum(pos, SoSoMapProviderBase.maxServer), "maptilesv2", base.GetSosoMapTileNo(pos, zoom), "png" });
            return base.GetTileImageUsingHttp(url);
        }

        private Placemark ParseCity(JObject cityJson)
        {
            JObject obj2 = cityJson;
            if (obj2["city"] != null)
            {
                obj2 = obj2["city"] as JObject;
            }
            string address = obj2["cname"].ToString();
            JArray array = (JArray)cityJson["path"];
            for (int i = 1; i < array.Count; i++)
            {
                address = array[i]["cname"].ToString() + " " + address;
            }
            Placemark placemark = new Placemark(address);
            if (cityJson["ctype"] != null)
            {
                placemark.Accuracy = int.Parse(cityJson["ctype"].ToString());
            }
            PointLatLng center = new PointLatLng(double.Parse(obj2["pointy"].ToString()), double.Parse(obj2["pointx"].ToString()));
            if ((center.Lat > 180.0) && (center.Lng > 180.0))
            {
                center = new Point2D(center.Lng, center.Lat).TransformToWGS84(ProjectionUtil.GetWKTFromEpsgCode(0xf10));
            }
            placemark.Point = center;
            placemark.LatLonBox = this.GetLatLngBox(center, obj2["level"]);
            return placemark;
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
    }
 

}
