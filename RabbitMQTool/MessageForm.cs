using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RabbitMQ.Client;
using RabbitMQ.Client.Content;
using RabbitMQ.Client.Framing.v0_9_1;

namespace RabbitMQTool
{
    public partial class MessageForm : Form
    {
        private RabbitMqCfg rabbitMqCfg = new RabbitMqCfg();

        public MessageForm()
        {
            InitializeComponent();

            InitUI();
        }

        private void InitUI()
        {
            this.textBoxIp.Text = "192.8.125.92";
            this.textBoxPort.Text = "5672";

            this.textBoxExchangeName.Text = "hotdataexchange";
            this.comboBoxExchangeType.Items.Add("direct");
            this.comboBoxExchangeType.Items.Add("topic");
            this.comboBoxExchangeType.Items.Add("fanout");
            this.comboBoxExchangeType.SelectedIndex = 0;
            this.comboBoxExchangeType.DropDownStyle = ComboBoxStyle.DropDownList;

            this.comboBoxExchangeDurable.Items.Add("true");
            this.comboBoxExchangeDurable.Items.Add("false");
            this.comboBoxExchangeDurable.SelectedIndex = 0;
            this.comboBoxExchangeDurable.DropDownStyle = ComboBoxStyle.DropDownList;

            this.comboBoxExchangeAutoDelete.Items.Add("true");
            this.comboBoxExchangeAutoDelete.Items.Add("false");
            this.comboBoxExchangeAutoDelete.SelectedIndex = 1;
            this.comboBoxExchangeAutoDelete.DropDownStyle = ComboBoxStyle.DropDownList;

            this.comboBoxQueueDurable.Items.Add("true");
            this.comboBoxQueueDurable.Items.Add("false");
            this.comboBoxQueueDurable.SelectedIndex = 1;
            this.comboBoxQueueDurable.DropDownStyle = ComboBoxStyle.DropDownList;


            this.comboBoxQueueAutoDelete.Items.Add("true");
            this.comboBoxQueueAutoDelete.Items.Add("false");
            this.comboBoxQueueAutoDelete.SelectedIndex = 0;
            this.comboBoxQueueAutoDelete.DropDownStyle = ComboBoxStyle.DropDownList;


            this.comboBoxQueueExclusive.Items.Add("true");
            this.comboBoxQueueExclusive.Items.Add("false");
            this.comboBoxQueueExclusive.SelectedIndex = 0;
            this.comboBoxQueueExclusive.DropDownStyle = ComboBoxStyle.DropDownList;


            this.textBoxVhost.Text = "/";
            this.textBoxChannel.Text = "1";

            //this.textBoxUserName.Text = "guest";
            //this.textBoxPassword.Text = "guest";
            this.textBoxUserName.Text = "hot";
            this.textBoxPassword.Text = "hot";

            this.textBoxQueueName.Text = "";
            this.textBoxRoutingKey.Text = "";

            //this.textBoxMessage.Text =
            //    "{\"msgType\":0,\"taskMsg\":{\"taskID\":1,\"taskType\":0,\"taskProgress\":100,\"userID\":0,\"taskName\":\"try\",\"taskStatus\":\"1\"}}";
            
            this.textBoxMessage.Text = GetJsonString();
        }

        private string GetJsonString()
        {
            TestData data = new TestData();
            data.equipId = 1;
            data.imei = "3232323";
            data.imsi = "222222";
            data.phoneNo = "13012345678";
            data.timestamp = DateTime.Now;
            data.matchIMEI = true;
            data.matchIMSI = true;

            string str = JsonHelper.ToJson(data);
            return str;
        }

        private int count = 1;

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = this.textBoxMessage.Text;

                ConnectionFactory connFactory = new ConnectionFactory();
                connFactory.HostName = this.textBoxIp.Text;
                connFactory.Port = int.Parse(this.textBoxPort.Text);
                connFactory.UserName = this.textBoxUserName.Text;
                connFactory.Password = this.textBoxPassword.Text;
                connFactory.VirtualHost = this.textBoxVhost.Text;

                using (IConnection connection = connFactory.CreateConnection())
                {
                    using (IModel channnel = connection.CreateModel())
                    {
                        string exchangeType = this.comboBoxExchangeType.SelectedItem as string;

                        #region exchangge pargma

                        bool exchangeDurable = true;
                        bool exchangeAutoDelete = false;

                        if (this.comboBoxExchangeDurable.SelectedIndex == 0)
                        {
                            exchangeDurable = true;
                        }
                        else
                        {
                            exchangeDurable = false;
                        }

                        if (this.comboBoxExchangeAutoDelete.SelectedIndex == 0)
                        {
                            exchangeAutoDelete = true;
                        }
                        else
                        {
                            exchangeAutoDelete = false;
                        }

                        #endregion

                        channnel.ExchangeDeclare(textBoxExchangeName.Text, exchangeType, exchangeDurable, exchangeAutoDelete, null);

                        #region queue pargma

                        //Bind to Queue
                        if (this.textBoxQueueName.Text != "")
                        {
                            bool queueDurable = false;
                            bool queueAutoDelete = true;
                            bool queueExclusive = true;

                            if (this.comboBoxQueueDurable.SelectedIndex == 0)
                            {
                                queueDurable = true;
                            }
                            else
                            {
                                queueDurable = false;
                            }

                            if (this.comboBoxQueueAutoDelete.SelectedIndex == 0)
                            {
                                queueAutoDelete = true;
                            }
                            else
                            {
                                queueAutoDelete = false;
                            }

                            if (this.comboBoxQueueExclusive.SelectedIndex == 0)
                            {
                                queueExclusive = true;
                            }
                            else
                            {
                                queueExclusive = false;
                            }

                            channnel.QueueDeclare(textBoxQueueName.Text, queueDurable, queueExclusive, queueAutoDelete, null);
                            channnel.QueueBind(textBoxQueueName.Text, textBoxExchangeName.Text, textBoxRoutingKey.Text);
                        }

                        #endregion

                        if (exchangeType == "fanout")
                        {
                            //if (count%2 == 1)
                            //{
                            //    IBasicProperties basicProperties = new BasicProperties();
                            //    basicProperties.Headers = new Dictionary<string, object>();
                            //    basicProperties.Headers.Add("msgType", 0X0D);
                            //    string ss = "192.8.125.76:40481";
                            //    basicProperties.Headers.Add("IpAndPort", ss);
                            //    channnel.BasicPublish(textBoxExchangeName.Text, "", basicProperties, new byte[] { 1 });
                            //    ++count;
                            //}
                            //else
                            //{
                            //    IBasicProperties basicProperties = new BasicProperties();
                            //    basicProperties.Headers = new Dictionary<string, object>();
                            //    basicProperties.Headers.Add("msgType", 0X80);
                            //    channnel.BasicPublish(textBoxExchangeName.Text, "", basicProperties, System.Text.Encoding.UTF8.GetBytes(this.textBoxMessage.Text));
                            //    ++count;
                            //}

                            channnel.BasicPublish(textBoxExchangeName.Text, textBoxRoutingKey.Text, null,
                            System.Text.Encoding.UTF8.GetBytes(this.textBoxMessage.Text));
                            
                        }
                        else
                        {
                            channnel.BasicPublish(textBoxExchangeName.Text, textBoxRoutingKey.Text, null,
                            System.Text.Encoding.UTF8.GetBytes(this.textBoxMessage.Text));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
