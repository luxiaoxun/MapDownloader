using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GMapProvidersExt.Baidu.HTTPService
{
    /// <summary>
    /// 提供位置建议相关服务
    /// </summary>
    public class PlaceSuggestionService : ServiceBase
    {
        private static string _suggestion_url = "http://api.map.baidu.com/place/v2/suggestion";  //位置建议服务

        /// <summary>
        /// 检索相似位置
        /// </summary>
        /// <param name="query">检索关键字</param>
        /// <param name="region">所在城市</param>
        /// <returns></returns>
        public JObject Suggestion(string query, string region)
        {
            try
            {
                if (_vm == VerificationMode.IPWhiteList)  //IP 白名单校验
                {
                    string url = _suggestion_url + "?query=" + query + "&region=" + region + "&output=json&ak=" + _ak;
                    string json = DownloadString(url);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
                else  //SN校验
                {
                    string url = _suggestion_url + "?query=" + query + "&region=" + region + "&output=json&ak=" + _ak;
                    IDictionary<string, string> param = new Dictionary<string, string> { { "query", query }, { "region", region }, { "output", "json" }, { "ak", _ak } };
                    string sn = AKSNCaculater.CaculateAKSN(_ak, _sk, _suggestion_url.Split(new string[] { ".com" }, StringSplitOptions.None)[1], param);  //计算sn
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
