using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace QuickRabbitMQ.imp
{
    public class DefaultConnectionParameterImp:IConnectonParameter
    {
        private string _hostName = "localhost";

        public string HostName
        {
            get
            {
                return _hostName;
            }
            set
            {
                _hostName = value;
            }
        }

        private int _port = AmqpTcpEndpoint.UseDefaultPort;

        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        private string _userName = ConnectionFactory.DefaultUser;
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        private string _password = ConnectionFactory.DefaultPass;

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        private string _vhost = ConnectionFactory.DefaultVHost;
        public string VHost
        {
            get
            {
                return _vhost;
            }
            set
            {
                _vhost = value;
            }
        }
    }
}
