using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GMapPOI
{
    public partial class PoiKeyWordForm : Form
    {
        private string keyWord;

        public PoiKeyWordForm()
        {
            InitializeComponent();
        }

        public string GetKeyWord()
        {
            return keyWord;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                keyWord = this.textBoxKeyWord.Text.Trim();
                if (string.IsNullOrEmpty(keyWord))
                {
                    throw new Exception("查询关键字不能为空");
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                CommonTools.MessageBox.ShowTipMessage(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
