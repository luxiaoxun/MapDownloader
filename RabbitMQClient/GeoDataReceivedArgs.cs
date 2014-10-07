using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitMQClient
{
    public class GeoDataReceivedArgs : EventArgs
    {
        public GeoData GeoData { set; get; }

        public GeoDataReceivedArgs(GeoData data)
        {
            GeoData = data;
        }
    }
}
