using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace QuickRabbitMQ.imp
{
    public class DefaultSerializeReciverImp:ISerializeReciver
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (DefaultSerializeReciverImp));

        private IMessageReciver _messageReciver;
        private ISerializer _serializer;

        public DefaultSerializeReciverImp(IMessageReciver messageReciver,ISerializer serializer)
        {
            this._messageReciver = messageReciver;
            this._serializer = serializer;

            this._messageReciver.MessageRecieved+=_messageReciver_MessageRecieved;
        }

        private void  _messageReciver_MessageRecieved(object sender, MessageRecivedEventArgs e)
        {
            try
            {
                if (e != null && e.MessageBody != null)
                {
                    Log.Debug("开始反序列化");
                    object data = _serializer.DeSerialize(e.MessageBody);
                    Log.Debug("收到数据" + data);
                    DataRecivedEventArgs args=new DataRecivedEventArgs();
                    args.Data = data;
                    OnDataRecived(args);
                }
            }
            catch (Exception exp)
            {
                
               Log.Error("数据反序列化失败",exp);
            }
        }

        public event EventHandler<DataRecivedEventArgs> DataRecived;

        public bool Start(out string errorMsg)
        {
            return _messageReciver.Start(out errorMsg);
        }

        public void Stop()
        {
            _messageReciver.Stop();
        }

        private void OnDataRecived(DataRecivedEventArgs args)
        {
            var handle = DataRecived;
            if (handle != null)
            {
                handle(this, args);
            }
        }
    }
}
