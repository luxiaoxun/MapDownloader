using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace GMapProvidersExt.Baidu.HTTPService
{
    /// <summary>
    /// 服务基类
    /// </summary>
    public class ServiceBase
    {
        protected static string _ak = Properties.BMap.Default.ServiceAK;  //AK
        protected static string _sk = Properties.BMap.Default.ServiceSK;  //SK
        protected static VerificationMode _vm = (VerificationMode)Properties.BMap.Default.VerificationMode;  //校验方式 0表示IP白名单校验（忽略SK）   1表示SN校验（需要SK）
        /// <summary>
        /// 从服务器上下载字符串
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected string DownloadString(string url)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                client.Encoding = Encoding.UTF8;
                return client.DownloadString(url);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 从服务器上下载字节流
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected byte[] DownloadData(string url)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                return client.DownloadData(url);
            }
            catch
            {
                return null;
            }
        }
    }
}
