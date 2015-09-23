using System;
using GMap.NET;
using System.Collections.Generic;
using System.Web;
using GMap.NET.MapProviders;
using GMap.NET.Projections;
using GMap.NET.WindowsForms;
using NetUtil;
using Newtonsoft.Json.Linq;

namespace GMapProvidersExt.AMap
{
    public class AMapProvider : AMapProviderBase, RoutingProvider
    {
        private readonly string KEY = "26144cb5dbe74ea6c1410777feb646de";
        
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
        }

        #region GMapRoutingProvider Members

        public GMapRoute GetRoute(PointLatLng start, PointLatLng end,string cityName)
        {
            List<PointLatLng> points = new List<PointLatLng>();
            string origin = string.Format("{0},{1}", start.Lng, start.Lat);
            string destination = string.Format("{0},{1}", end.Lng, end.Lat);
            string city = HttpUtility.UrlEncode(cityName);
            string url = string.Format("http://restapi.amap.com/v3/direction/transit/integrated?origin={0}&destination={1}&city={2}&output=json&key={3}", origin, destination, city, KEY);
            string result = HttpUtil.Request(url, "utf-8");
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
            string result = HttpUtil.Request(url, "utf-8");
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
        static readonly string UrlFormat = "http://webrd0{0}.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=7&x={1}&y={2}&z={3}";
    }
}
