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
        public static string Request(string url, string charset = "gb2312", string method = "get", string entity = "", string conentType = "text/htm")
        {
            WebClient client = new WebClient
            {
                Headers = new WebHeaderCollection()
            };
            client.Headers.Add("Content-Type", conentType + ";charset=utf-8");
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
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
