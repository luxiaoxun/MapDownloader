using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GMapProvidersExt.Baidu.HTTPService
{
    /// <summary>
    /// 提供坐标转换相关服务
    /// </summary>
    public class CoordinateTransService:ServiceBase
    {
        private static string _coordinate_url = "http://api.map.baidu.com/geoconv/v1/?";

        /// <summary>
        /// 将指定坐标转换成另一种指定坐标
        /// 1：GPS设备获取的角度坐标
        /// 2：GPS获取的米制坐标、sogou地图所用坐标
        /// 3：google地图、soso地图、aliyun地图、mapabc地图和amap地图所用坐标
        /// 4：3中列表地图坐标对应的米制坐标
        /// 5：百度地图采用的经纬度坐标
        /// 6：百度地图采用的米制坐标
        /// 7：mapbar地图坐标;
        /// 8：51地图坐标
        /// </summary>
        /// <param name="source1">原坐标1（经度、X值）</param>
        /// <param name="source2">原坐标2（纬度、Y值）</param>
        /// <param name="from">原坐标类型</param>
        /// <param name="to">转换类型</param>
        /// <returns></returns>
        public JObject CoordinateTransform(string source1, string source2, int from, int to)
        {
            try
            {
                if (_vm == VerificationMode.IPWhiteList)  //IP 白名单校验
                {
                    string url = _coordinate_url + "coords=" + source1 + "," + source2 + "&from=" + from + "&to=" + to + "&output=json&ak=" + _ak;
                    string json = DownloadString(url);
                    return JsonConvert.DeserializeObject(json) as JObject;
                }
                else  //SN校验
                {
                    string url = _coordinate_url + "coords=" + source1 + "," + source2 + "&from=" + from + "&to=" + to + "&output=json&ak=" + _ak;
                    IDictionary<string, string> param = new Dictionary<string, string> { { "coords", source1+","+source2 }, { "from", from.ToString() }, { "to", to.ToString() }, { "output", "json" }, { "ak", _ak } };

                    string sn = AKSNCaculater.CaculateAKSN(_ak, _sk, _coordinate_url.Split(new string[] { ".com" }, StringSplitOptions.None)[1], param);  //计算sn
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
