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
    public partial class PoiExportForm : Form
    {
        public PoiExportForm()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            DialogResult res = folderDlg.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox1.Text = folderDlg.SelectedPath;
            }
        }

        public string GetPoiExportPath()
        {
            return this.textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string rootPath = this.textBox1.Text;
            if (string.IsNullOrEmpty(rootPath))
            {
                MessageBox.Show("导出路劲不能为空！");
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
