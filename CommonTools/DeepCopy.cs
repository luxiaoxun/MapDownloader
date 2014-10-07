using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace CommonTools
{
    //深拷贝类
    public static class DeepCopy
    {
        public static T Copy<T>(T item)
        {
            BinaryFormatter format = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            format.Serialize(stream,item);
            stream.Seek(0, SeekOrigin.Begin);
            T result = (T) format.Deserialize(stream);
            stream.Close();
            return result;
        }
    }
}
