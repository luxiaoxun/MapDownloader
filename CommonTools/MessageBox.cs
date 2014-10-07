using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonTools
{
    public static class MessageBox
    {
        public static void ShowTipMessage(string text)
        {
            System.Windows.Forms.MessageBox.Show(text, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public static void ShowExceptionMessage(string text)
        {
            System.Windows.Forms.MessageBox.Show(text, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowErrorMessage(string text)
        {
            System.Windows.Forms.MessageBox.Show(text, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWarningMessage(string text)
        {
            System.Windows.Forms.MessageBox.Show(text, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static DialogResult ShowConformMessage(string text)
        {
            return System.Windows.Forms.MessageBox.Show(text, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }
    }
}
