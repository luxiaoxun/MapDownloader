using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using GMap.NET;
using GMap.NET.Internals;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Bing
{
    public abstract class BingMapProviderBase : GMapProvider
    {
        // Fields
        public string ClientKey;
        private static readonly string GeocoderUrlFormat;
        private static bool init;
        private GMapProvider[] overlays;
        private static readonly string RouteUrlFormatPointLatLng;
        public bool TryCorrectVersion;
        public string Version;

        // Methods
        static BingMapProviderBase()
        {
            init = false;
            RouteUrlFormatPointLatLng = "http://dev.virtualearth.net/REST/V1/Routes/{0}?o=xml&wp.0={1},{2}&wp.1={3},{4}{5}&optmz=distance&rpo=Points&key={6}";
            GeocoderUrlFormat = "http://dev.virtualearth.net/REST/v1/Locations?{0}&o=xml&key={1}";
        }

        public BingMapProviderBase()
        {
            this.Version = "3025";
            this.TryCorrectVersion = true;
            base.MaxZoom = 0x13;
            base.MinZoom = 1;
            base.RefererUrl = "http://www.bing.com/maps/";
            base.Copyright = string.Format("\x00a9{0} Microsoft Corporation, \x00a9{0} NAVTEQ, \x00a9{0} Image courtesy of NASA", DateTime.Today.Year);
        }

        private bool AddFieldIfNotEmpty(ref string Input, string FieldName, string Value)
        {
            if (string.IsNullOrEmpty(Value))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Input))
            {
                Input = string.Empty;
            }
            else
            {
                Input = Input + "&";
            }
            Input = Input + FieldName + "=" + Value;
            return true;
        }

        protected override bool CheckTileImageHttpResponse(WebResponse response)
        {
            bool flag;
            if (flag = base.CheckTileImageHttpResponse(response))
            {
                string str = response.Headers.Get("X-VE-Tile-Info");
                if (str != null)
                {
                    return !str.Equals("no-tile");
                }
            }
            return flag;
        }

        private GeoCoderStatusCode GetLatLngFromGeocoderUrl(string url, out List<PointLatLng> pointList)
        {
            GeoCoderStatusCode unknow = GeoCoderStatusCode.Unknow;
            pointList = null;
            try
            {
                string contentUsingHttp = Singleton<GMaps>.Instance.UseGeocoderCache ? Singleton<Cache>.Instance.GetContent(url, CacheType.GeocoderCache) : string.Empty;
                bool flag = false;
                if (string.IsNullOrEmpty(contentUsingHttp))
                {
                    contentUsingHttp = base.GetContentUsingHttp(url);
                    if (!string.IsNullOrEmpty(contentUsingHttp))
                    {
                        flag = true;
                    }
                }
                unknow = GeoCoderStatusCode.Unknow;
                if ((string.IsNullOrEmpty(contentUsingHttp) || !contentUsingHttp.StartsWith("<?xml")) || !contentUsingHttp.Contains("<Response"))
                {
                    return unknow;
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(contentUsingHttp);
                XmlNode node = document["Response"];
                switch (node["StatusCode"].InnerText)
                {
                    case "200":
                        pointList = new List<PointLatLng>();
                        node = node["ResourceSets"]["ResourceSet"]["Resources"];
                        foreach (XmlNode node2 in node.ChildNodes)
                        {
                            XmlNode node3 = node2["Point"]["Latitude"];
                            XmlNode node4 = node2["Point"]["Longitude"];
                            pointList.Add(new PointLatLng(double.Parse(node3.InnerText, CultureInfo.InvariantCulture), double.Parse(node4.InnerText, CultureInfo.InvariantCulture)));
                        }
                        if (pointList.Count > 0)
                        {
                            unknow = GeoCoderStatusCode.G_GEO_SUCCESS;
                            if (flag && Singleton<GMaps>.Instance.UseGeocoderCache)
                            {
                                Singleton<Cache>.Instance.SaveContent(url, CacheType.GeocoderCache, contentUsingHttp);
                            }
                            return unknow;
                        }
                        return GeoCoderStatusCode.G_GEO_UNKNOWN_ADDRESS;

                    case "400":
                        return GeoCoderStatusCode.G_GEO_BAD_REQUEST;

                    case "401":
                        return GeoCoderStatusCode.G_GEO_BAD_KEY;

                    case "403":
                        return GeoCoderStatusCode.G_GEO_BAD_REQUEST;

                    case "404":
                        return GeoCoderStatusCode.G_GEO_UNKNOWN_ADDRESS;

                    case "500":
                        return GeoCoderStatusCode.G_GEO_SERVER_ERROR;

                    case "501":
                        return GeoCoderStatusCode.Unknow;
                }
                return GeoCoderStatusCode.Unknow;
            }
            catch (Exception)
            {
                return GeoCoderStatusCode.ExceptionInCode;
            }
        }

        public Placemark GetPlacemark(PointLatLng location, out GeoCoderStatusCode status)
        {
            throw new NotImplementedException();
        }

        public GeoCoderStatusCode GetPlacemarks(PointLatLng location, out List<Placemark> placemarkList)
        {
            throw new NotImplementedException();
        }

        public PointLatLng? GetPoint(Placemark placemark, out GeoCoderStatusCode status)
        {
            List<PointLatLng> list;
            status = this.GetLatLngFromGeocoderUrl(this.MakeGeocoderDetailedUrl(placemark), out list);
            if ((list != null) && (list.Count > 0))
            {
                return new PointLatLng?(list[0]);
            }
            return null;
        }

        public PointLatLng? GetPoint(string keywords, out GeoCoderStatusCode status)
        {
            List<PointLatLng> list;
            status = this.GetPoints(keywords, out list);
            if ((list != null) && (list.Count > 0))
            {
                return new PointLatLng?(list[0]);
            }
            return null;
        }

        public GeoCoderStatusCode GetPoints(Placemark placemark, out List<PointLatLng> pointList)
        {
            return this.GetLatLngFromGeocoderUrl(this.MakeGeocoderDetailedUrl(placemark), out pointList);
        }

        public GeoCoderStatusCode GetPoints(string keywords, out List<PointLatLng> pointList)
        {
            return this.GetLatLngFromGeocoderUrl(this.MakeGeocoderUrl("q=" + keywords), out pointList);
        }

        public MapRoute GetRoute(PointLatLng start, PointLatLng end, bool avoidHighways, bool walkingMode, int Zoom)
        {
            MapRoute route = null;
            string str;
            int num;
            int num2;
            List<PointLatLng> points = this.GetRoutePoints(this.MakeRouteUrl(start, end, GMapProvider.LanguageStr, avoidHighways, walkingMode), Zoom, out str, out num, out num2);
            if (points != null)
            {
                route = new MapRoute(points, str);
            }
            return route;
        }

        public MapRoute GetRoute(string start, string end, bool avoidHighways, bool walkingMode, int Zoom)
        {
            throw new NotImplementedException("check GetRoute(PointLatLng start...");
        }

        private List<PointLatLng> GetRoutePoints(string url, int zoom, out string tooltipHtml, out int numLevel, out int zoomFactor)
        {
            List<PointLatLng> list = null;
            tooltipHtml = string.Empty;
            numLevel = -1;
            zoomFactor = -1;
            try
            {
                string contentUsingHttp = Singleton<GMaps>.Instance.UseRouteCache ? Singleton<Cache>.Instance.GetContent(url, CacheType.RouteCache) : string.Empty;
                if (string.IsNullOrEmpty(contentUsingHttp))
                {
                    contentUsingHttp = base.GetContentUsingHttp(url);
                    if (!string.IsNullOrEmpty(contentUsingHttp) && Singleton<GMaps>.Instance.UseRouteCache)
                    {
                        Singleton<Cache>.Instance.SaveContent(url, CacheType.RouteCache, contentUsingHttp);
                    }
                }
                if (string.IsNullOrEmpty(contentUsingHttp))
                {
                    return list;
                }
                int index = 0;
                int startIndex = contentUsingHttp.IndexOf("<RoutePath><Line>") + 0x11;
                if (startIndex >= 0x11)
                {
                    index = contentUsingHttp.IndexOf("</Line></RoutePath>", (int)(startIndex + 1));
                    if (index > 0)
                    {
                        int length = index - startIndex;
                        if (length > 0)
                        {
                            tooltipHtml = contentUsingHttp.Substring(startIndex, length);
                        }
                    }
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(contentUsingHttp);
                XmlNode node = document["Response"];
                switch (node["StatusCode"].InnerText)
                {
                    case "200":
                        {
                            node = node["ResourceSets"]["ResourceSet"]["Resources"]["Route"]["RoutePath"]["Line"];
                            XmlNodeList childNodes = node.ChildNodes;
                            if (childNodes.Count > 0)
                            {
                                list = new List<PointLatLng>();
                                foreach (XmlNode node2 in childNodes)
                                {
                                    XmlNode node3 = node2["Latitude"];
                                    XmlNode node4 = node2["Longitude"];
                                    list.Add(new PointLatLng(double.Parse(node3.InnerText, CultureInfo.InvariantCulture), double.Parse(node4.InnerText, CultureInfo.InvariantCulture)));
                                }
                            }
                            return list;
                        }
                    case "400":
                        throw new Exception("Bad Request, The request contained an error.");

                    case "401":
                        throw new Exception("Unauthorized, Access was denied. You may have entered your credentials incorrectly, or you might not have access to the requested resource or operation.");

                    case "403":
                        throw new Exception("Forbidden, The request is for something forbidden. Authorization will not help.");

                    case "404":
                        throw new Exception("Not Found, The requested resource was not found.");

                    case "500":
                        throw new Exception("Internal Server Error, Your request could not be completed because there was a problem with the service.");

                    case "501":
                        throw new Exception("Service Unavailable, There's a problem with the service right now. Please try again later.");
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            throw new NotImplementedException();
        }

        private string MakeGeocoderDetailedUrl(Placemark placemark)
        {
            string input = string.Empty;
            if (!this.AddFieldIfNotEmpty(ref input, "countryRegion", placemark.CountryNameCode))
            {
                this.AddFieldIfNotEmpty(ref input, "countryRegion", placemark.CountryName);
            }
            this.AddFieldIfNotEmpty(ref input, "adminDistrict", placemark.DistrictName);
            this.AddFieldIfNotEmpty(ref input, "locality", placemark.LocalityName);
            this.AddFieldIfNotEmpty(ref input, "postalCode", placemark.PostalCodeNumber);
            if (!string.IsNullOrEmpty(placemark.HouseNo))
            {
                this.AddFieldIfNotEmpty(ref input, "addressLine", placemark.ThoroughfareName + " " + placemark.HouseNo);
            }
            else
            {
                this.AddFieldIfNotEmpty(ref input, "addressLine", placemark.ThoroughfareName);
            }
            return this.MakeGeocoderUrl(input);
        }

        private string MakeGeocoderUrl(string keywords)
        {
            return string.Format(CultureInfo.InvariantCulture, GeocoderUrlFormat, new object[] { keywords, this.ClientKey });
        }

        private string MakeRouteUrl(PointLatLng start, PointLatLng end, string language, bool avoidHighways, bool walkingMode)
        {
            string str = avoidHighways ? "&avoid=highways" : string.Empty;
            string str2 = walkingMode ? "Walking" : "Driving";
            return string.Format(CultureInfo.InvariantCulture, RouteUrlFormatPointLatLng, new object[] { str2, start.Lat, start.Lng, end.Lat, end.Lng, str, this.ClientKey });
        }

        //public override void OnInitialized()
        //{
        //    if (!init && this.TryCorrectVersion)
        //    {
        //        string url = "http://cn.bing.com/maps/";
        //        try
        //        {
        //            string contentUsingHttp = Singleton<Cache>.Instance.GetContent(url, CacheType.UrlCache, TimeSpan.FromHours(360.0));
        //            if (string.IsNullOrEmpty(contentUsingHttp))
        //            {
        //                contentUsingHttp = base.GetContentUsingHttp(url);
        //                if (!string.IsNullOrEmpty(contentUsingHttp))
        //                {
        //                    Singleton<Cache>.Instance.SaveContent(url, CacheType.UrlCache, contentUsingHttp);
        //                }
        //            }
        //            if (!string.IsNullOrEmpty(contentUsingHttp))
        //            {
        //                Match match = new Regex(@"tiles.virtualearth.net/tiles/pt.{0,20}g=(\d*)", RegexOptions.IgnoreCase).Match(contentUsingHttp);
        //                if (match.Success)
        //                {
        //                    GroupCollection groups = match.Groups;
        //                    if (groups.Count > 0)
        //                    {
        //                        string str3 = groups[1].Value;
        //                        GMapProviders.BingMap.Version = str3;
        //                    }
        //                }
        //            }
        //            init = true;
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //}

        internal string TileXYToQuadKey(long tileX, long tileY, int levelOfDetail)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = levelOfDetail; i > 0; i--)
            {
                char ch = '0';
                int num2 = ((int)1) << (i - 1);
                if ((tileX & num2) != 0)
                {
                    ch = (char)(ch + '\x0001');
                }
                if ((tileY & num2) != 0)
                {
                    ch = (char)(ch + '\x0001');
                    ch = (char)(ch + '\x0001');
                }
                builder.Append(ch);
            }
            return builder.ToString();
        }

        // Properties
        public override Guid Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override GMapProvider[] Overlays
        {
            get
            {
                if (this.overlays == null)
                {
                    this.overlays = new GMapProvider[] { this };
                }
                return this.overlays;
            }
        }

        public override PureProjection Projection
        {
            get
            {
                return SphericalMercatorProjection.Instance;
            }
        }
    }


}
