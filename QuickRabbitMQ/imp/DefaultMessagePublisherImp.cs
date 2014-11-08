using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using log4net;

namespace QuickRabbitMQ.imp
{
    public class DefaultMessagePublisherImp:IMessagePublisher
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DefaultMessagePublisherImp));

        private readonly IConnectonParameter _connectionParameter;
        private readonly IExchangeParameter _exchangeParameter;

        private IConnection _connection;
        private IModel _channnel;

        private readonly object _sycRoot=new object();

        private volatile bool _hasStarted;

        private readonly  bool _userLock;

        private readonly object _sendSycRoot=new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectonParameter"></param>
        /// <param name="exchangeParameter"></param>
        /// <param name="userLock">是否同步多线程的发送</param>
        public DefaultMessagePublisherImp(IConnectonParameter connectonParameter,IExchangeParameter exchangeParameter,bool userLock)
        {
            this._connectionParameter = connectonParameter;
            this._exchangeParameter = exchangeParameter;
            this._userLock = userLock;
        }

        public bool Publish(byte[] messageBody, string routingKey, out string errorMsg)
        {
            bool result = true;
            errorMsg = "";
            if (messageBody == null)
            {
                return true;
            }
            try
            {
                if (!Start(out errorMsg))
                {
                    return false;
                }
                else
                {
                    var chan = _channnel;
                    if (chan != null)
                    {
                        Log.Debug("开始发送数据");
                        if (_userLock)
                        {
                            lock (_sendSycRoot)
                            {
                                chan.BasicPublish(_exchangeParameter.ExchangeName, routingKey, null, messageBody);
                            }
                           
                        }
                        else
                        {
                            chan.BasicPublish(_exchangeParameter.ExchangeName, routingKey, null, messageBody);
                        }
                        Log.Debug("发送数据成功");
                    }
                }
            }
            catch (Exception exp)
            {
                Log.Error("发送数据失败",exp);
                Stop();
                result = false;
            }

            return result;
        }

        #region lifeCycle

        public bool Start(out string errorMsg)
        {
            errorMsg = "";

            if (_hasStarted)
            {
                return true;
            }

            Stop();

            lock (_sycRoot)
            {
                if (_hasStarted) //多线程安全
                {
                    return true;
                }
                try
                {
                    var connFactory = new ConnectionFactory();
                    connFactory.HostName = _connectionParameter.HostName;
                    connFactory.Port = _connectionParameter.Port;
                    connFactory.UserName = _connectionParameter.UserName;
                    connFactory.Password = _connectionParameter.Password;
                    connFactory.VirtualHost = _connectionParameter.VHost;

                    _connection = connFactory.CreateConnection();
                    _connection.ConnectionShutdown += _connection_ConnectionShutdown;

                    _channnel = _connection.CreateModel();
                    _channnel.ModelShutdown +=_channnel_ModelShutdown;

                    _channnel.ExchangeDeclare(_exchangeParameter.ExchangeName,
                                              _exchangeParameter.ExchangeType,
                                              _exchangeParameter.IsDuarble,
                                              _exchangeParameter.IsAutoDelete,
                                              null);

                    _hasStarted = true;

                    return true;

                }
                catch (Exception exp)
                {
                    errorMsg = exp.Message;
                    Log.Error("rabbitMq启动连接失败", exp);
                    Stop();

                    return false;
                }
            }
            
        }

        private void _channnel_ModelShutdown(IModel model, ShutdownEventArgs reason)
        {
            Stop();
            var msg = "发生端channel连接断开";
            if (reason != null)
            {
                msg += ",reason:" + reason.ToString();
            }
            Log.Error(msg);
        }

        private void _connection_ConnectionShutdown(IConnection connection, ShutdownEventArgs reason)
        {
            Stop();
            var msg = "发生端connection连接断开";
            if (reason != null)
            {
                msg += ",reason:" + reason.ToString();
            }
            Log.Error(msg);
        }

        public void Stop()
        {
            lock (_sycRoot)
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

                _hasStarted = false;
            }
        }

        #endregion
    }
}
