using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comet.Core
{
    /// <summary>
    /// Comet客户端生命周期事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void CometClientEventHandler(object sender, CometClientEventArgs args);

    /// <summary>
    /// Comet客户端生命周期事件参数类
    /// </summary>
    public class CometClientEventArgs : EventArgs
    {
        public CometClient CometClient { get; }

        public CometClientEventArgs(CometClient cometClient)
        {
            CometClient = cometClient;
        }
    }

}
