using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRabbitMQ
{
    public class MessageRecivedEventArgs:EventArgs
    {
        public byte[] MessageBody { get; set; }
    }
}
