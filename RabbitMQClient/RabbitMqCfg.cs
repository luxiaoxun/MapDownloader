using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitMQClient
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

        public const string RabbitMqIpKey = "TaskRabbitMq.IP";

        public const string RabbitMqPortKey = "TaskRabbitMq.Port";

        public const string RabbitMqVHostKey = "TaskRabbitMq.VHost";

        public const string RabbitMqUserNameKey = "TaskRabbitMq.UserName";

        public const string RabbitMqPasswordKey = "TaskRabbitMq.Password";

        public const string RabbitMqExchangeNameKey = "TaskRabbitMq.ExchangeName";

    }
}
