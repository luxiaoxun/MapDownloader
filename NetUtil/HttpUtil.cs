using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;

namespace NetUtil
{
    public static class HttpUtil
    {
        // Methods
        public static string Request(string url, string charset = "utf-8", string method = "get", string entity = "", string conentType = "text/html")
        {
            WebClient client = new WebClient
            {
                Headers = new WebHeaderCollection()
            };
            client.Headers.Add(HttpRequestHeader.ContentType, conentType);
            client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36 SE 2.X MetaSr 1.0");
            //client.Headers.Add(HttpRequestHeader.ContentEncoding, "utf-8");
            client.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            client.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
            client.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            Encoding encoding = Encoding.GetEncoding(charset);
            //client.Encoding = encoding;
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

        public static string GetData(string url)
        {
            string responseString = null;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseString = reader.ReadToEnd();
                reader.Close();
            }
            return responseString;
        }

        public static string PostData(string url, string data)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] bs = Encoding.UTF8.GetBytes(data);
            Stream reqStream = request.GetRequestStream();
            reqStream.Write(bs, 0, bs.Length);
            reqStream.Close();

            string responseString = null;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseString = reader.ReadToEnd();
                reader.Close();
            }
            return responseString;
        }

    }

}
