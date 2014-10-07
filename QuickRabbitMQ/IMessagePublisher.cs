using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRabbitMQ
{
    public interface IMessagePublisher:IRabbitmqLifeCycle
    {
        bool Publish(byte[] messageBody, string routingKey,out string errorMsg);
    }
}
