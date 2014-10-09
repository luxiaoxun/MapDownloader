using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RegexTestTool
{
    public partial class RegexTestForm : Form
    {
        public RegexTestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pattern = this.textBoxRegex.Text;
            string inout = this.textBoxString.Text;

            bool isMatch = Regex.IsMatch(inout, pattern);

            this.textBoxResult.Text = isMatch.ToString();
        }
    }
}
