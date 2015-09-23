using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GMapProvidersExt.Baidu.HTTPService
{
    /// <summary>
    /// 提供IP定位相关服务
    /// </summary>
    public class IPService:ServiceBase
    {
        private static string _ip_url = "http://api.map.baidu.com/location/ip";

        /// <summary>
        /// 根据IP获取对应位置
        /// </summary>
        /// <param name="ip">IP地址 为空表示访问者IP</param>
        /// <returns></returns>
        public JObject LocationByIP(string ip = null)
        {
            try
            {
                if (_vm == VerificationMode.IPWhiteList)  //IP 白名单校验
                {
                    string url = _ip_url + "?ip=" + (ip == null ? "" : ip) + "&coor=bd09ll&output=json&ak=" + _ak;
                    string json = DownloadString(url);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
                else  //SN校验
                {
                    string url = _ip_url + "?ip=" + (ip == null ? "" : ip) + "&coor=bd09ll&output=json&ak=" + _ak;
                    IDictionary<string, string> param = new Dictionary<string, string> { { "ip", ip == null ? "" : ip }, { "coor", "bd09ll" }, { "output", "json" }, { "ak", _ak } };
                    string sn = AKSNCaculater.CaculateAKSN(_ak, _sk, _ip_url.Split(new string[] { ".com" }, StringSplitOptions.None)[1], param);  //计算sn
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
