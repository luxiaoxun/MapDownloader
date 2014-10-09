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

namespace RabbitMQSendTool
{
    public partial class RabbitMQSendForm : Form
    {
        public RabbitMQSendForm()
        {
            InitializeComponent();

            InitUI();
        }

        private void InitUI()
        {
            this.textBoxIp.Text = "192.8.125.202";
            this.textBoxPort.Text = "5672";

            this.textBoxExchangeName.Text = "ExchangeNotify";
            this.comboBoxExchangeType.Items.Add("direct");
            this.comboBoxExchangeType.Items.Add("topic");
            this.comboBoxExchangeType.Items.Add("fanout");
            this.comboBoxExchangeType.SelectedIndex = 0;

            this.textBoxVhost.Text = "/";
            this.textBoxChannel.Text = "1";

            this.textBoxUserName.Text = "guest";
            this.textBoxPassword.Text = "guest";

            this.textBoxQueueName.Text = "";
            this.textBoxRoutingKey.Text = "0";

            this.textBoxMessage.Text =
                "{\"msgType\":0,\"taskMsg\":{\"taskID\":1,\"taskType\":0,\"taskProgress\":100,\"userID\":0,\"taskName\":\"try\",\"taskStatus\":\"1\"}}";
        }

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

                        channnel.ExchangeDeclare(textBoxExchangeName.Text, exchangeType, false, false, null);

                        //channnel.QueueDeclare(textBoxQueueName.Text,false,true,true,null);

                        //channnel.QueueBind(textBoxQueueName.Text,textBoxExchangeName.Text,textBoxRoutingKey.Text);

                        //IMapMessageBuilder mapMessageBuilder = new MapMessageBuilder(channnel);

                        channnel.BasicPublish(textBoxExchangeName.Text, textBoxRoutingKey.Text,null,
                            System.Text.Encoding.UTF8.GetBytes(this.textBoxMessage.Text));
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
