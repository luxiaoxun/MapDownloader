using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using QuickRabbitMQ;
using RabbitMQ.Client;
using Newtonsoft.Json;
using NetUtil;

namespace RabbitMQClient
{
    public class RbbitMqMessageReciver : IRabbitMqMessageReciver
    {
        private IMessageReciver reciver;

        public event EventHandler<GeoDataReceivedArgs> GeoDataReceiveEvent;

        private ILog log = LogManager.GetLogger(typeof(RbbitMqMessageReciver));

        public bool IsStarted { set; get; }

        public bool Start(out string message)
        {
            bool ret = reciver.Start(out message);
            if (ret)
            {
                IsStarted = true;
            }
            return ret;
        }

        public void Stop()
        {
            reciver.Stop();
            IsStarted = false;
        }

        public RbbitMqMessageReciver()
        {
            IConnectonParameter connectonParameter = QuickRabbitmqFactory.CreateConnectonParameter();
            connectonParameter.VHost = CfgFaced.RabbitMqCfg.VHost;
            connectonParameter.Port = CfgFaced.RabbitMqCfg.RabbitMqPort;
            connectonParameter.HostName = CfgFaced.RabbitMqCfg.RabbitMqIp;
            connectonParameter.UserName = CfgFaced.RabbitMqCfg.UserName;
            connectonParameter.Password = CfgFaced.RabbitMqCfg.Password;

            IExchangeParameter exchangeParameter = QuickRabbitmqFactory.CreateExchangeParameter();
            exchangeParameter.ExchangeName = CfgFaced.RabbitMqCfg.ExchangeName;

            exchangeParameter.ExchangeType = ExchangeType.Direct;
            exchangeParameter.IsAutoDelete = false;
            exchangeParameter.IsDuarble = false;

            IQueueParameter quqQueueParameter = QuickRabbitmqFactory.CreateQueueParameter();
            quqQueueParameter.IsAutoDelete = true;
            quqQueueParameter.IsDuarble = false;
            quqQueueParameter.IsExclusive = true;
            quqQueueParameter.QueueName = CfgFaced.RabbitMqCfg.QueueName;
            quqQueueParameter.RoutingKey = CfgFaced.RabbitMqCfg.RoutingKey;

            reciver = QuickRabbitmqFactory.CreateMessageReciver(connectonParameter, exchangeParameter, quqQueueParameter);
            reciver.MessageRecieved += new EventHandler<QuickRabbitMQ.MessageRecivedEventArgs>(reciver_MessageRecieved);
        }

        private void reciver_MessageRecieved(object sender, QuickRabbitMQ.MessageRecivedEventArgs e)
        {
            try
            {
                if (e != null && e.MessageBody != null)
                {
                    string json = System.Text.Encoding.UTF8.GetString(e.MessageBody);
                    log.Debug(" 收到消息:" + json);

                    if (!string.IsNullOrEmpty(json))
                    {
                        Newtonsoft.Json.Linq.JObject jsonJobject = Newtonsoft.Json.JsonConvert.DeserializeObject(json) as Newtonsoft.Json.Linq.JObject;
                        if (jsonJobject == null)
                        {
                            return;
                        }

                        Newtonsoft.Json.Linq.JToken typeJToken = jsonJobject["msgType"];
                        if (typeJToken == null)
                        {
                            log.Debug("未解析到messageType");
                            return;
                        }
                        int messageType = (int)typeJToken;

                        switch (messageType)
                        {
                            case 0:
                                GeoData geoData = JsonHelper.JsonDeserialize<GeoData>(json);
                                if (geoData != null)
                                {
                                    var handler = GeoDataReceiveEvent;
                                    if (handler != null)
                                    {
                                        handler(this, new GeoDataReceivedArgs(geoData));
                                    }
                                }
                                break;
                            default:
                                log.Error("未知的消息类型," + messageType);
                                break;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                log.Error(exp);
            }
        }
    }
}