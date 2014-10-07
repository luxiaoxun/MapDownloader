using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRabbitMQ
{
    public interface IExchangeParameter
    {
        string ExchangeName { get; set; }

        string ExchangeType { get; set; }

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
    }
}
