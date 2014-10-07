using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRabbitMQ
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConnectonParameter
    {
        string HostName { get; set; }

        int Port { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        string VHost { get; set; }
    }
}
