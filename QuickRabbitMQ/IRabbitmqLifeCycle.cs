using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickRabbitMQ
{
    /// <summary>
    /// 生命周期接口，可以启动和停止 
    /// </summary>
    public interface IRabbitmqLifeCycle
    {
        bool Start(out string errorMsg);
        void Stop();
    }
}
