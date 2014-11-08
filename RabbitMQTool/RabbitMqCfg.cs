using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitMQTool
{
    public class RabbitMqCfg
    {
        public string RabbitMqIp { get; set; }

        public int RabbitMqPort { get; set; }

        public string VHost { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ExchangeName { get; set; }

        public string QueueName { get; set; }

        public string RoutingKey { set; get; }

        public const string RabbitMqIpKey = "RabbitMq.IP";

        public const string RabbitMqPortKey = "RabbitMq.Port";

        public const string RabbitMqVHostKey = "RabbitMq.VHost";

        public const string RabbitMqUserNameKey = "RabbitMq.UserName";

        public const string RabbitMqPasswordKey = "RabbitMq.Password";

        public const string RabbitMqExchangeNameKey = "RabbitMq.ExchangeName";

    }
}
