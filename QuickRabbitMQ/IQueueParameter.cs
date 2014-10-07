using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRabbitMQ
{
    public interface IQueueParameter
    {
        string QueueName { get; set; }

        bool IsAutoDelete
        {
            get;
            set;
        }

        bool IsDuarble
        {
            get;
            set;
        }

        bool IsExclusive { get; set; }

        string RoutingKey { get; set; }
    }
}
