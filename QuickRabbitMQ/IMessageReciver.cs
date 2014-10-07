using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRabbitMQ
{
    public interface IMessageReciver:IRabbitmqLifeCycle
    {
        event EventHandler<MessageRecivedEventArgs> MessageRecieved;
    }
}
