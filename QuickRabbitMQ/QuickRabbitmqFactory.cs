using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickRabbitMQ.imp;

namespace QuickRabbitMQ
{
    public static class QuickRabbitmqFactory
    {
        public static IConnectonParameter CreateConnectonParameter()
        {
            return  new DefaultConnectionParameterImp();
        }

        public static IExchangeParameter CreateExchangeParameter()
        {
            return  new DefaultExchangeParameterImp();
        }

        public static IQueueParameter CreateQueueParameter()
        {
            return  new DefaultQueueParameterImp();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectonParameter"></param>
        /// <param name="exchangeParameter"></param>
        /// <param name="useLock">多线程使用同一对象发送，请传递true</param>
        /// <returns></returns>
        public static IMessagePublisher CreaMessagePublisher(IConnectonParameter connectonParameter,
                                                             IExchangeParameter exchangeParameter,bool useLock)
        {
            return new DefaultMessagePublisherImp(connectonParameter,exchangeParameter,useLock);
        }

        public static IMessageReciver CreateMessageReciver(IConnectonParameter connectonParameter,
            IExchangeParameter exchangeParameter, IQueueParameter queueParameter)
        {
            return  new DefaultMessageReciverImp(connectonParameter,exchangeParameter,queueParameter);
        }

        public static ISerializerPublisher CreateSerializerPublisher(IMessagePublisher publisher, ISerializer serializer)
        {
            return  new DefaultSerializerPublisher(publisher,serializer);
        }

        public static ISerializeReciver CreateSerializeReciver(IMessageReciver messageReciver, ISerializer serializer)
        {
            return  new DefaultSerializeReciverImp(messageReciver,serializer);
        }
    }
}
