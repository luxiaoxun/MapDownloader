using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GMapProvidersExt.Baidu.HTTPService
{
    /// <summary>
    /// 提供地址解析相关服务
    /// </summary>
    public class GeocodingService:ServiceBase
    {
        private static string _geocoding_url = "http://api.map.baidu.com/geocoder/v2/";  //地理编码

        /// <summary>
        /// 地址编码 地址转坐标
        /// </summary>
        /// <param name="address">地址</param>
        /// <returns></returns>
        public JObject Geocoding(string address)
        {
            try
            {
                if (_vm == VerificationMode.IPWhiteList)  //IP 白名单校验
                {
                    string url = _geocoding_url + "?address=" + address +  "&output=json&ak=" + _ak;
                    string json = DownloadString(url);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
                else  //SN校验
                {
                    string url = _geocoding_url + "?address=" + address + "&output=json&ak=" + _ak;
                    IDictionary<string, string> param = new Dictionary<string, string> { { "address", address }, { "output", "json" }, { "ak", _ak } };
                    string sn = AKSNCaculater.CaculateAKSN(_ak, _sk, _geocoding_url.Split(new string[] { ".com" }, StringSplitOptions.None)[1], param);  //计算sn
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
        /// 逆地址编码 坐标转地址
        /// </summary>
        /// <param name="location">坐标 维度,经度</param>
        /// <returns></returns>
        public JObject DeGeocoding(string location)
        {
            try
            {
                if (_vm == VerificationMode.IPWhiteList)  //IP 白名单校验
                {
                    string url = _geocoding_url + "?location=" + location + "&pois=1&output=json&ak=" + _ak;
                    string json = DownloadString(url);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
                else  //SN校验
                {
                    string url = _geocoding_url + "?location=" + location + "&pois=1&output=json&ak=" + _ak;
                    IDictionary<string, string> param = new Dictionary<string, string> { { "location", location }, { "pois", "1" }, { "output", "json" }, { "ak", _ak } };
                    string sn = AKSNCaculater.CaculateAKSN(_ak, _sk, _geocoding_url.Split(new string[] { ".com" }, StringSplitOptions.None)[1], param);  //计算sn
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
