using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapDownloader
{
    public partial class ProxyForm : Form
    {
        public ProxyForm()
        {
            InitializeComponent();
        }

        public string GetProxyIp()
        {
            return this.textBoxIP.Text.Trim();
        }

        public int GetProxyPort()
        {
            return int.Parse(this.textBoxPort.Text.Trim());
        }

        public bool CheckProxyOn()
        {
            return this.checkBoxProxy.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
