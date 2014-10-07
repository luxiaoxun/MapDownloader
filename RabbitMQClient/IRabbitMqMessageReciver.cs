using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitMQClient
{
    public interface IRabbitMqMessageReciver
    {
        bool Start(out string message);
        void Stop();

        bool IsStarted { set; get; }

        event EventHandler<GeoDataReceivedArgs> GeoDataReceiveEvent;
    }
}
