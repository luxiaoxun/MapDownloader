using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace CommonTools
{
    public class PromptingMessage
    {
        // Methods
        public static void ErrorMessage(Control control, string message, int interval = 5, eToastPosition position = eToastPosition.MiddleCenter)
        {
            ToastNotification.Show(control, message, interval * 1000, position);
        }

        public static void PromptMessage(Control control, string message, int interval = 3, eToastPosition position = eToastPosition.MiddleCenter)
        {
            ToastNotification.Show(control, message, interval * 1000, position);
        }

        public static void WarnMessage(Control control, string message, int interval = 5, eToastPosition position = eToastPosition.MiddleCenter)
        {
            ToastNotification.Show(control, message, interval * 1000, position);
        }
    }
}
