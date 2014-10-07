using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRabbitMQ
{
    public class DataRecivedEventArgs:EventArgs
    {
        public object Data { get; set; }
    }
}
