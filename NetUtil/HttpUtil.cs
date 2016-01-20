using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace NetUtil
{
    public static class HttpUtil
    {
        // Methods
        public static string Request(string url, string charset = "gb2312", string method = "get", string entity = "", string conentType = "text/html")
        {
            WebClient client = new WebClient
            {
                Headers = new WebHeaderCollection()
            };
            client.Headers.Add("Content-Type", conentType + ";charset=utf-8");
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36 SE 2.X MetaSr 1.0");
            client.Headers.Add("Accept-Encoding","gzip,deflate,sdch");
            client.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            client.Headers.Add("Cache-Control","max-age=0");
            Encoding encoding = Encoding.GetEncoding(charset);
            if (method.ToLower().Equals("get"))
            {
                byte[] bytes = client.DownloadData(url);
                return encoding.GetString(bytes);
            }
            if (method.ToLower().Equals("post"))
            {
                byte[] buffer2 = client.UploadData(url, Encoding.UTF8.GetBytes(entity));
                return encoding.GetString(buffer2);
            }
            if (method.ToLower().Equals("delete"))
            {
                return client.UploadString(url, "DELETE", entity);
            }
            return null;
        }

        public static string RequestByJSON(string url, string method="get", string entity="")
        {
            WebClient client = new WebClient
            {
                Headers = new WebHeaderCollection()
            };
            client.Headers.Add("Content-Type", "application/json");
            if (method.ToLower().Equals("get"))
            {
                byte[] bytes = client.DownloadData(url);
                return Encoding.UTF8.GetString(bytes);
            }
            if (method.ToLower().Equals("post"))
            {
                byte[] buffer2 = client.UploadData(url, Encoding.UTF8.GetBytes(entity));
                return Encoding.UTF8.GetString(buffer2);
            }
            if (method.ToLower().Equals("delete"))
            {
                return client.UploadString(url, "DELETE", entity);
            }
            return null;
        }
    }


}
