using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NetUtilityLib
{
    public sealed class JsonHelper
    {
        private static JsonSerializerSettings setting = new JsonSerializerSettings()
        {
            DateFormatString = "yyyy-MM-dd,HH:mm:ss",
        };
        
        public static string JsonSerialize(Object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, setting);
            }
        }

        public static void JsonSerializeToFile(Object obj, string path, Encoding encoding)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            string result = JsonSerialize(obj);
            System.IO.FileInfo info = new System.IO.FileInfo(path);
            System.IO.Directory.CreateDirectory(info.Directory.FullName);
            File.WriteAllText(path, result, encoding);
        }

        public static T JsonDeserialize<T>(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str, setting);
            }
        }

        public static T JsonDeserializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            string json = File.ReadAllText(path, encoding);
            return JsonDeserialize<T>(json);
        }
    }
}
