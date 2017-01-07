using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Threading;

namespace Comet.Core
{
    /// <summary>
    /// 封装Comet异步结果
    /// </summary>
    public class CometAsyncResult : IAsyncResult
    {
        private AsyncCallback callback;
        private object asyncState;
        private bool isCompleted = false;
        private CometMessage[] messages;
        private HttpContext context;

        /// <summary>
        /// 封装异步执行结果
        /// </summary>
        /// <param name="context">异步请求</param>
        /// <param name="callback"> 异步回调</param>
        /// <param name="asyncState">附加参数</param>
        public CometAsyncResult(HttpContext context, AsyncCallback callback, object asyncState)
        {
            this.context = context;
            this.callback = callback;
            this.asyncState = asyncState;
        }

        // 保持 Comet长连接获得的响应消息
        public CometMessage[] CometMessages
        {
            get { return this.messages; }
            set { this.messages = value; }
        }

        internal void SetCompleted()
        {
            this.isCompleted = true;

            if (callback != null)
                callback(this);
        }

        #region IAsyncResult Members

        /// <summary>
        /// Gets or Sets the extra data associated with this async operation
        /// </summary>
        public object AsyncState
        {
            get { return asyncState; }
        }

        /// <summary>
        /// Not Implemented: will throw InvalidOperationException("ASP.NET Should never use this property"); }
        /// </summary>
        public WaitHandle AsyncWaitHandle
        {
            get { throw new InvalidOperationException("ASP.NET Should never use this property"); }
        }

        /// <summary>
        /// Gets a boolean indicating if the operation completed synchronously (always returns false)
        /// </summary>
        public bool CompletedSynchronously
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a boolean indicating if the operation has completed
        /// </summary>
        public bool IsCompleted
        {
            get { return this.isCompleted; }
        }

        /// <summary>
        /// Gets the HttpContext associaetd with this async operation
        /// </summary>
        public HttpContext Context
        {
            get { return this.context; }
        }

        #endregion
    }
}
