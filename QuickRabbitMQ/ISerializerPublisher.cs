using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRabbitMQ
{
    public interface ISerializerPublisher:IRabbitmqLifeCycle
    {
        bool Publish(object data, string routingKey, out string errorMsg);
    }
}
