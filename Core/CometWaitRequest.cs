using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Comet.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class CometWaitRequest
    {
        public CometAsyncResult Result { get; set; }
        public DateTime DateTimeAdded => DateTime.Now;
        public string ClientPrivateToken { get; }
        public long LastMessageId   {get;}
        public DateTime? DateDeactivated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        /// <param name="lastMessageId"></param>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public CometWaitRequest(string clientPrivateToken, long lastMessageId, HttpContext context, AsyncCallback callback, object state)
        {
            ClientPrivateToken = clientPrivateToken;
            LastMessageId = lastMessageId;
            Result = new CometAsyncResult(context, callback, state);
        }

        /// <summary>
        /// Gets a boolean flag indicating if this client is active (has it been disconnected, and is it not idle?)
        /// </summary>
        public bool Active
        {
            get { return !DateDeactivated.HasValue; }
        }
    }
}
