using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RabbitMQTool
{
    /// <summary>
    /// Json字符串序列化反序列化类
    /// </summary>
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings MyJsonSerializerSettings;

        static JsonHelper()
        {
            MyJsonSerializerSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            IsoDateTimeConverter dateTimeConverter = new IsoDateTimeConverter();
            dateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            MyJsonSerializerSettings.Converters.Add(dateTimeConverter);
        }

        public static T FromJson<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(json, MyJsonSerializerSettings);
        }

        public static string ToJson<T>(T data)
        {
            if (data == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(data, MyJsonSerializerSettings);
        }

        public static void JsonSerializeToFile<T>(T data, string path, Encoding encoding)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("path");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.File.Create(path);
            }

            string jsonStr = ToJson(data);

            System.IO.File.WriteAllText(path, jsonStr, encoding);
        }

        public static T JsonDeSerializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("path");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            string jsonStr = System.IO.File.ReadAllText(path, encoding);
            return FromJson<T>(jsonStr);
        }
    }
}
