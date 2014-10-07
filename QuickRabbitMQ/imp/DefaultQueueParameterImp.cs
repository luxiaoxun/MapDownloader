using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRabbitMQ.imp
{
    public class DefaultQueueParameterImp:IQueueParameter
    {
        private string _queueName = Guid.NewGuid().ToString();
        public string QueueName
        {
            get
            {
                return _queueName;
            }
            set
            {
                _queueName = value;

                if (string.IsNullOrEmpty(_queueName))
                {
                    _queueName = Guid.NewGuid().ToString();
                }
            }
        }

        public bool IsAutoDelete
        {
            get; set;
        }

        public bool IsDuarble
        {
            get; set;
        }

        public bool IsExclusive { get; set; }

        public string RoutingKey
        {
            get; set;
        }
    }
}
