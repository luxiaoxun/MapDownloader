using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRabbitMQ
{
    public interface ISerializer
    {
        byte[] Serialize(object data);

        object DeSerialize(byte[] body);
    }
}
