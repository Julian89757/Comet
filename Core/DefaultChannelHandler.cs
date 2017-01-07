using System;
using System.Web;
using System.Diagnostics;

namespace Comet.Core
{
    /// <summary>
    /// 默认消息处理管道
    /// </summary>
    public class DefaultChannelHandler : IHttpAsyncHandler
    {
        public static CometStateManager StateManager { get; }

        static DefaultChannelHandler()
        {
            StateManager = new CometStateManager(new InProcCometStateProvider());
            StateManager.ClientInitialized += new CometClientEventHandler(StateManager_ClientInitialized);
            StateManager.ClientSubscribed += new CometClientEventHandler(StateManager_ClientSubscribed);
            StateManager.IdleClientKilled += new CometClientEventHandler(StateManager_IdleClientKilled);
        }

        static void StateManager_ClientInitialized(object sender, CometClientEventArgs args)
        {
            Debug.WriteLine("Client Initialized: " + args.CometClient.AliasName);
        }

        static void StateManager_ClientSubscribed(object sender, CometClientEventArgs args)
        {
            Debug.WriteLine("Client Subscribed: " + args.CometClient.AliasName);
        }

        static void StateManager_IdleClientKilled(object sender, CometClientEventArgs args)
        {
            Debug.WriteLine("Client Killed: " + args.CometClient.AliasName);
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            return StateManager.BeginSubscribe(context, cb, extraData);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            StateManager.EndSubscribe(result);
        }

        #region IHttpAsyncHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
