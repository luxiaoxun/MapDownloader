using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace QuickRabbitMQ.imp
{
    public class DefaultSerializerPublisher:ISerializerPublisher
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (DefaultSerializerPublisher));

        private IMessagePublisher _messagePublisher;
        private ISerializer _serializer;

        public DefaultSerializerPublisher(IMessagePublisher messagePublisher,ISerializer serializer)
        {
            this._messagePublisher = messagePublisher;
            this._serializer = serializer;
        }

        public bool Publish(object data, string routingKey, out string errorMsg)
        {
            errorMsg = "";
            try
            {
               
               Log.Debug("准备发送数据"+data);
               byte[] body= _serializer.Serialize(data);
               return _messagePublisher.Publish(body, routingKey, out errorMsg);
            }
            catch (Exception expException)
            {
                errorMsg = expException.Message;
                Log.Error("对象发送失败",expException);
                return false;
            }
           
        }

        public bool Start(out string errorMsg)
        {
            return _messagePublisher.Start(out errorMsg);
        }

        public void Stop()
        {
            _messagePublisher.Stop();
        }
    }
}
