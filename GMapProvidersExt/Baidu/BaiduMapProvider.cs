using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using GMap.NET;
using GMap.NET.MapProviders;
using NetUtil;
using Newtonsoft.Json.Linq;
using log4net;

namespace GMapProvidersExt.Baidu
{
    public class BaiduMapProvider : BaiduMapProviderBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BaiduMapProvider));
        private static readonly string KEY = "P3YVgujUBDobaVnqqiPqw5Gj";

        private int succeedCount;
        public delegate void QueryProgressDelegate(long completedCount, long total);

        // Fields
        private readonly string cnName;
        public string fm;
        private readonly Guid id = new Guid("5532ECC6-6561-4451-BF2D-22E86D0DC9F8");
        public static readonly BaiduMapProvider Instance;
        private readonly string name;
        public string type;
        public string Version;

        // Methods
        static BaiduMapProvider()
        {
            Instance = new BaiduMapProvider();
            GMapProviders.AddMapProvider(Instance);
        }

        private BaiduMapProvider()
        {
            this.Version = "039";
            this.type = "web";
            this.fm = "44";
            this.name = "BaiduMap";
            this.cnName = "百度普通地图";
        }

        public GeoCoderStatusCode GetPlacemarksByKeywords(string keywords, string region, string rectangle,
            QueryProgressDelegate queryProgressEvent, out List<Placemark> placemarkList, ref int count)
        {
            this.succeedCount = 0;
            //this.totalCount = 0;
            placemarkList = this.GetPlacemarksByKeywords(keywords, region, rectangle, 0, queryProgressEvent, ref count);
            return GeoCoderStatusCode.G_GEO_SUCCESS;
        }

        private List<Placemark> GetPlacemarksByKeywords(string keywords, string region, string rectangle,
            int pageIndex, QueryProgressDelegate queryProgressEvent, ref int totalCount)
        {
            List<Placemark> list = new List<Placemark>();
            int pageSize = 20;
            string keyWordUrlEncode = HttpUtility.UrlEncode(keywords);
            string format = "http://api.map.baidu.com/place/v2/search?q={0}&region={1}&output=json&ak={2}&page_size={3}&page_num={4}&scope=1";
            //"http://api.map.baidu.com/place/v2/search?ak=您的密钥&output=json&query=%E9%93%B6%E8%A1%8C&page_size=10&page_num=0&scope=1&region=%E5%8C%97%E4%BA%AC"
            if (!string.IsNullOrEmpty(region))
            {
                format = string.Format(format, new object[] { keyWordUrlEncode, HttpUtility.UrlEncode(region), KEY, pageSize, pageIndex });
            }
            //Get Cache Json Result if exist
            //string cacheUrl = string.Format("http://api.map.baidu.com/place/v2/search/{0}/{1}/{2}/{3}", new object[] { keyWordUrlEncode, HttpUtility.UrlEncode(region), pageIndex, pageSize });
            //string cacheResult = Singleton<Cache>.Instance.GetContent(cacheUrl, CacheType.UrlCache, TimeSpan.FromHours(360.0));
            //if (string.IsNullOrEmpty(cacheResult))
            //{
            //    cacheResult = HttpUtil.GetData(format);
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
                string message = (string)jsonResult["message"];
                if (message == "ok")
                {
                    if (pageIndex == 0)
                    {
                        totalCount = int.Parse((string)jsonResult["total"]);
                    }
                    if (totalCount <= 0) return list;

                    JArray results = (JArray)jsonResult["results"];
                    if (results != null && results.Count > 0)
                    {
                        for (int i = 0; i < results.Count; ++i)
                        {
                            JObject obj = results[i] as JObject;
                            string name = obj["name"].ToString();
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
                                queryProgressEvent((long)this.succeedCount, (long)totalCount);
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

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = this.MakeTileImageUrl(pos, zoom, GMapProvider.LanguageStr);
            try
            {
                return base.GetTileImageUsingHttp(url);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            long num = pos.X - ((long)Math.Pow(2.0, (double)(zoom - 1)));
            long num2 = (((long)Math.Pow(2.0, (double)(zoom - 1))) - pos.Y) - 1;
            string str = num.ToString();
            string str2 = num2.ToString();
            if (str.StartsWith("-"))
            {
                str = "M" + str.Substring(1);
            }
            if (str2.StartsWith("-"))
            {
                str2 = "M" + str2.Substring(1);
            }
            int serverNum = GMapProvider.GetServerNum(pos, BaiduMapProviderBase.maxServer) + 1;
            return string.Format(BaiduMapProviderBase.UrlFormat, new object[] { serverNum, str, str2, zoom });
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
