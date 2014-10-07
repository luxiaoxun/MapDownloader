using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRabbitMQ.imp
{
    public class DefaultExchangeParameterImp:IExchangeParameter
    {
        public string ExchangeName
        {
            get; set;
        }

        public string ExchangeType
        {
            get; set;
        }

        public bool IsAutoDelete
        {
            get; set;
        }

        public bool IsDuarble
        {
            get; set;
        }
    }
}
