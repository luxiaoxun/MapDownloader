using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GMapProvidersExt.Baidu.HTTPService
{
    /// <summary>
    /// 提供位置相关服务
    /// </summary>
    public class PlaceService : ServiceBase
    {
        private static string _search_url = "http://api.map.baidu.com/place/v2/search";  //v2 place区域检索POI服务
        private static string _detail_url = "http://api.map.baidu.com/place/v2/detail";   //v2 POI详情服务
        private static string _eventsearch_url = "http://api.map.baidu.com/place/v2/eventsearch"; //v2 团购信息检索服务
        private static string _eventdetail_url = "http://api.map.baidu.com/place/v2/eventdetail"; //v2 商家团购信息查询

        /// <summary>
        /// 城市内检索POI
        /// </summary>
        /// <param name="query">检索关键字</param>
        /// <param name="region">城市名、代码</param>
        /// <returns></returns>
        public JObject SearchInCity(string query, string region)
        {
            try
            {
                if (_vm == VerificationMode.IPWhiteList)  //IP 白名单校验
                {                    
                    string url = _search_url + "?query=" + query + "&region=" + region + "&output=json&page_size=20&scope=2&ak=" + _ak;
                    string json = DownloadString(url);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
                else  //SN校验
                {
                    string url = _search_url + "?query=" + query + "&region=" + region + "&output=json&page_size=20&scope=2&ak=" + _ak;
                    IDictionary<string, string> param = new Dictionary<string, string> { { "query", query }, { "region", region }, { "output", "json" }, {"page_size","20"}, { "scope", "2" }, { "ak", _ak } };
                    string sn = AKSNCaculater.CaculateAKSN(_ak, _sk, _search_url.Split(new string[] { ".com" }, StringSplitOptions.None)[1], param);  //计算sn
                    string json = DownloadString(url + "&sn=" + sn);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 矩形区域内检索POI
        /// </summary>
        /// <param name="query">检索关键字</param>
        /// <param name="bounds">矩形区域，左下角纬度,经度,右上角纬度,经度</param>
        /// <returns></returns>
        public JObject SearchInBound(string query,string bounds)
        {
            try
            {
                if (_vm == VerificationMode.IPWhiteList) //IP 白名单校验
                {
                    string url = _search_url + "?query=" + query + "&bounds=" + bounds + "&output=json&page_size=20&scope=2&ak=" + _ak;
                    string json = DownloadString(url);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
                else  //SN校验
                {
                    string url = _search_url + "?query=" + query + "&bounds=" + bounds + "&output=json&page_size=20&scope=2&ak=" + _ak;
                    IDictionary<string, string> param = new Dictionary<string, string> { { "query", query }, { "bounds", bounds }, { "output", "json" }, {"page_size","20"}, { "scope", "2" }, { "ak", _ak } };
                    string sn = AKSNCaculater.CaculateAKSN(_ak, _sk, _search_url.Split(new string[] { ".com" }, StringSplitOptions.None)[1], param);  //计算sn
                    string json = DownloadString(url + "&sn=" + sn);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 圆形区域内检索POI
        /// </summary>
        /// <param name="query">检索关键字</param>
        /// <param name="location">中心点纬度,经度</param>
        /// <param name="radius">半径，单位为m</param>
        /// <returns></returns>
        public JObject SearchInCircle(string query, string location, int radius)
        {
            try
            {
                if (_vm == VerificationMode.IPWhiteList) //IP 白名单校验
                {
                    string url = _search_url + "?query=" + query + "&location=" + location + "&radius=" + radius + "&output=json&page_size=20&scope=2&ak=" + _ak;
                    string json = DownloadString(url);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
                else  //SN校验
                {
                    string url = _search_url + "?query=" + query + "&location=" + location + "&radius=" + radius + "&output=json&page_size=20&scope=2&ak=" + _ak;
                    IDictionary<string, string> param = new Dictionary<string, string> { { "query", query }, { "location",location }, {"radius",radius.ToString()}, { "output", "json" }, {"page_size","20"},{ "scope", "2" }, { "ak", _ak } };
                    string sn = AKSNCaculater.CaculateAKSN(_ak, _sk, _search_url.Split(new string[] { ".com" }, StringSplitOptions.None)[1], param);  //计算sn
                    string json = DownloadString(url + "&sn=" + sn);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 查询POI详细信息
        /// </summary>
        /// <param name="uid">POI的唯一标识</param>
        /// <returns></returns>
        public JObject Detail(string uid)
        {
            try
            {
                if (_vm == VerificationMode.IPWhiteList) //IP 白名单校验
                {
                    string url = _detail_url + "?uid=" + uid + "&output=json&scope=2&ak=" + _ak;
                    string json = DownloadString(url);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
                else  //SN校验
                {
                    string url = _detail_url + "?uid=" + uid + "&output=json&scope=2&ak=" + _ak;
                    IDictionary<string, string> param = new Dictionary<string, string> { { "uid", uid }, { "output", "json" }, { "scope", "2" }, { "ak", _ak } };
                    string sn = AKSNCaculater.CaculateAKSN(_ak, _sk, _detail_url.Split(new string[] { ".com" }, StringSplitOptions.None)[1], param);  //计算sn
                    string json = DownloadString(url + "&sn=" + sn);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 矩形区域内检索商业活动POI
        /// </summary>
        /// <param name="query">检索关键字</param>
        /// <param name="events">事件名称，可以是团购、打折，全部（groupon、discount、all） 目前只支持团购</param>
        /// <param name="region">城市名、代码</param>
        /// <param name="bounds">矩形区域，左下角纬度,经度,右上角纬度,经度</param>
        /// <returns></returns>
        public JObject EventSearchInBound(string query, string events, string region, string bounds)
        {
            try
            {
                if (_vm == VerificationMode.IPWhiteList) //IP 白名单校验
                {
                    string url = _eventsearch_url + "?query=" + query + "&event=" + events + "&region=" + region + "&bounds=" + bounds + "&output=json&page_size=20&scope=2&ak=" + _ak;
                    string json = DownloadString(url);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
                else  //SN校验
                {
                    string url = _eventsearch_url + "?query=" + query + "&event=" + events + "&region=" + region + "&bounds=" + bounds + "&output=json&page_size=20&scope=2&ak=" + _ak;
                    IDictionary<string, string> param = new Dictionary<string, string> { { "query", query }, { "event", events }, { "region", region }, { "bounds", bounds }, { "output", "json" }, {"page_size","20"}, { "scope", "2" }, { "ak", _ak } };
                    string sn = AKSNCaculater.CaculateAKSN(_ak, _sk, _eventsearch_url.Split(new string[] { ".com" }, StringSplitOptions.None)[1], param);  //计算sn
                    string json = DownloadString(url + "&sn=" + sn);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 圆形区域内检索商业活动POI
        /// </summary>
        /// <param name="query">检索关键字</param>
        /// <param name="events">事件名称，可以是团购、打折，全部（groupon、discount、all） 目前只支持团购</param>
        /// <param name="region">城市名、代码</param>
        /// <param name="location">中心点，纬度,经度</param>
        /// <param name="radius">半径，单位为m</param>
        /// <returns></returns>
        public JObject EventSearchInCircle(string query, string events, string region, string location, int radius)
        {
            try
            {
                if (_vm == VerificationMode.IPWhiteList) //IP 白名单校验
                {
                    string url = _eventsearch_url + "?query=" + query + "&event=" + events + "&region=" + region + "&location=" + location + "&radius=" + radius + "&output=json&page_size=20&scope=2&ak=" + _ak;
                    string json = DownloadString(url);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
                else  //SN校验
                {
                    string url = _eventsearch_url + "?query=" + query + "&event=" + events + "&region=" + region + "&location=" + location + "&radius=" + radius + "&output=json&page_size=20&scope=2&ak=" + _ak;
                    IDictionary<string, string> param = new Dictionary<string, string> { { "query", query }, { "event", events }, { "region", region }, { "location", location }, {"radius",radius.ToString()}, { "output", "json" }, {"page_size","20"}, { "scope", "2" }, { "ak", _ak } };
                    string sn = AKSNCaculater.CaculateAKSN(_ak, _sk, _eventsearch_url.Split(new string[] { ".com" }, StringSplitOptions.None)[1], param);  //计算sn
                    string json = DownloadString(url + "&sn=" + sn);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 查询商业活动POI详细信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public JObject EventDetail(string uid)
        {
            try
            {
                if (_vm == VerificationMode.IPWhiteList) //IP 白名单校验
                {
                    string url = _eventdetail_url + "?uid=" + uid + "&output=json&scope=2&ak=" + _ak;
                    string json = DownloadString(url);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
                else  //SN校验
                {
                    string url = _eventdetail_url + "?uid=" + uid + "&output=json&scope=2&ak=" + _ak;
                    IDictionary<string, string> param = new Dictionary<string, string> { { "uid", uid }, { "output", "json" }, { "scope", "2" }, { "ak", _ak } };
                    string sn = AKSNCaculater.CaculateAKSN(_ak, _sk, _eventdetail_url.Split(new string[] { ".com" }, StringSplitOptions.None)[1], param);  //计算sn
                    string json = DownloadString(url + "&sn=" + sn);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
