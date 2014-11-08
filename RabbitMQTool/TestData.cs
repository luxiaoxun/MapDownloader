using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace RabbitMQTool
{
    public class TestData
    {
        [JsonProperty("equipId")]
        public int equipId
        {
            get;
            set;
        }

        [JsonProperty("timestamp")]
        public DateTime timestamp
        {
            get;
            set;
        }

        [JsonProperty("imsi")]
        public String imsi
        {
            get;
            set;
        }

        [JsonProperty("imei")]
        public String imei
        {
            get;
            set;
        }

        [JsonProperty("phoneNo")]
        public String phoneNo { set; get; }

        [JsonProperty("matchIMSI")]
        public bool matchIMSI { set; get; }

        [JsonProperty("matchIMEI")]
        public bool matchIMEI { set; get; }
    }
}
