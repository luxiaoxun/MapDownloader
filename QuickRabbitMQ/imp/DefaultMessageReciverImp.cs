using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using log4net;
using System.Threading;

namespace QuickRabbitMQ.imp
{
    public class DefaultMessageReciverImp:IMessageReciver
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DefaultMessageReciverImp));

        private readonly IConnectonParameter _connectionParameter;
        private readonly IExchangeParameter _exchangeParameter;
        private readonly IQueueParameter _queueParameter;

        private IConnection _connection;
        private IModel _channnel;

        private Thread _workThread;

        private volatile bool _hasStarted;

        private readonly object _sycRoot=new object();

        private volatile bool _isConnectionOk;
        
        private string _token = "";

        public DefaultMessageReciverImp(IConnectonParameter connectonParameter, IExchangeParameter exchangeParameter,IQueueParameter queueParameter)
        {
            this._connectionParameter = connectonParameter;
            this._exchangeParameter = exchangeParameter;
            this._queueParameter = queueParameter;
        }

        public event EventHandler<MessageRecivedEventArgs> MessageRecieved;

        public bool Start(out string errorMsg)
        {
            errorMsg = "";
            if (_hasStarted)
            {
                return true;
            }
            lock (_sycRoot)
            {
                if (_hasStarted)
                {
                    return true;
                }

                try
                {
                    Stop();
                    _token = Guid.NewGuid().ToString();
                    var myToken = _token;
                   
                    _workThread = new Thread(DoRecive);

                    _workThread.IsBackground = true;
                    _workThread.Start(myToken);

                    _hasStarted = true;

                    return true;
                }
                catch (Exception exp)
                {
                  errorMsg = exp.Message;
                  Stop();
                  Log.Error("rabbitmq接收端创建失败",exp);
                  return false;
                }
            }
        }

        private void DoRecive(object obj)
        {
            string myToken = obj as string;
            if (myToken == null)
            {
                myToken = "";
            }

            Log.Debug("开始rabbitmq发送线程," + Thread.CurrentThread.ManagedThreadId + ",token" + myToken);
            try
            {
                if (!myToken.Equals(this._token))
                {
                    return;
                }

                while (true)
                {
                    if (!myToken.Equals(this._token))
                    {
                        return;
                    }
                    try
                    {
                        MakeConnection();
                        var consumer = new QueueingBasicConsumer(_channnel);
                        _channnel.BasicConsume(_queueParameter.QueueName, true, consumer);

                        while (true)
                        {
                            if (!myToken.Equals(this._token))
                            {
                                return;
                            }
                            try
                            {
                                RabbitMQ.Client.Events.BasicDeliverEventArgs args = consumer.Queue.Dequeue();
                                var body = args.Body;

                                Log.Debug("收到rabbitmq数据");

                                OnMessageRecived(body);
                            }
                            catch (Exception exp)
                            {
                                CloseConnection();
                                Log.Error(exp);
                                break;
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        CloseConnection();
                        Log.Error(exp);
                    }

                    if (!myToken.Equals(this._token))
                    {
                        return;
                    }
                    //休眠10秒
                    Thread.Sleep(10000);
                }
            }
            catch (Exception exp)
            {
                Log.Error(exp);
            }

            Log.Debug("退出rabbitmq发送线程," + Thread.CurrentThread.ManagedThreadId);
        }

        private void MakeConnection()
        {
            if (_isConnectionOk)
            {
                return;
            }
            CloseConnection();
            var connFactory = new ConnectionFactory();
            connFactory.HostName = _connectionParameter.HostName;
            connFactory.Port = _connectionParameter.Port;
            connFactory.UserName = _connectionParameter.UserName;
            connFactory.Password = _connectionParameter.Password;
            connFactory.VirtualHost = _connectionParameter.VHost;

            _connection = connFactory.CreateConnection();
            _connection.ConnectionShutdown += _connection_ConnectionShutdown;

            _channnel = _connection.CreateModel();
            _channnel.ModelShutdown += _channnel_ModelShutdown;
            _channnel.ExchangeDeclare(_exchangeParameter.ExchangeName,
                _exchangeParameter.ExchangeType,
                _exchangeParameter.IsDuarble,
                _exchangeParameter.IsAutoDelete,
                null);

            _channnel.QueueDeclare(_queueParameter.QueueName,
                _queueParameter.IsDuarble,
                _queueParameter.IsExclusive,
                _queueParameter.IsAutoDelete,
                null);

            _channnel.QueueBind(_queueParameter.QueueName,
                _exchangeParameter.ExchangeName,
                _queueParameter.RoutingKey);

            _isConnectionOk = true;
        }

        private void OnMessageRecived(byte[] body)
        {
            var handle = MessageRecieved;

            if (handle != null && body != null)
            {
                var messageArgs = new MessageRecivedEventArgs();
                messageArgs.MessageBody = body;

                ThreadPool.QueueUserWorkItem((x) =>
                {
                    try
                    {
                        Log.Debug("MessageRecieved开始处理rabbitmq收的数据");
                        handle(this, messageArgs);
                        Log.Debug("MessageRecieved处理rabbitmq收的数据成功");
                    }
                    catch (Exception exp)
                    {
                        Log.Error("数据接收处理失败", exp);
                    }
                }, null);
            }
        }

        public void Stop()
        {
            _token = "";
            lock (_sycRoot)
            {
                try
                {
                    if (_workThread != null &&_workThread.IsAlive)
                    {
                        _workThread.Abort();
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    _workThread = null;
                }

                CloseConnection();

                _hasStarted = false;
            }
        }

        private void CloseConnection()
        {
            try
            {
                if (_channnel != null)
                {
                    _channnel.ModelShutdown -= _channnel_ModelShutdown;
                    _channnel.Close();
                }
            }
            catch
            {
            }
            finally
            {
                _channnel = null;
            }

            try
            {
                if (_connection != null)
                {
                    _connection.ConnectionShutdown -= _connection_ConnectionShutdown;
                    _connection.Close();
                }
            }
            catch
            {
            }
            finally
            {
                _connection = null;
            }

            _isConnectionOk = false;
        }

        private void _channnel_ModelShutdown(IModel model, ShutdownEventArgs reason)
        {
            CloseConnection();
            var msg = "发生端channel连接断开";
            if (reason != null)
            {
                msg += ",reason:" + reason.ToString();
            }
            Log.Error(msg);
        }

        private void _connection_ConnectionShutdown(IConnection connection, ShutdownEventArgs reason)
        {
            CloseConnection();
            var msg = "发生端connection连接断开";
            if (reason != null)
            {
                msg += ",reason:" + reason.ToString();
            }
            Log.Error(msg);
        }
        
    }
}
