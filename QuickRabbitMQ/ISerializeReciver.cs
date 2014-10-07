using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRabbitMQ
{
    public interface ISerializeReciver:IRabbitmqLifeCycle
    {
        event EventHandler<DataRecivedEventArgs> DataRecived;
    }
}
