using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CometClient
{
    // 客户端待订阅的事件
    public delegate void CometClientEventHandler(object sender, CometClientEventArgs args);

    // 事件参数
    public class CometClientEventArgs
    {
        public CometClientEventArgs(CometMessage message)
        {
            this.Message = message;
        }

        public CometMessage Message { get; set; }

    }

    // 模仿 asp.net JS API 做的WinForm API
    public class CometCoreClient
    {

        //  提供给用户本身的用户认证token
        public string PrivateToken { get; set; }

        //  提供给其他用户的用户认证token
        public string PublicToken { get; set; }

        //  设置显示名称
        public string DisplayName { get; set; }

        public long LastMessageId { get; set; }

        public string HandlerUrl { get; set; }

        

        private event CometClientEventHandler SuccessHandler;
        private event CometClientEventHandler FailureHandler;
        private event CometClientEventHandler TimeoutHandler;

        public CometCoreClient(string handlerUrl, string privateToken)
        {
            this.HandlerUrl = handlerUrl;
            this.PrivateToken = privateToken;
        }
        
        // 注册事件处理函数
        public void AddSuccessHandler(CometClientEventHandler func)
        {
            SuccessHandler += func;
        }

        public void AddFailureHandler(CometClientEventHandler func)
        {
            FailureHandler += func;
        }
        public void AddTimeoutHandler(CometClientEventHandler func)
        {
            TimeoutHandler += func;
        }

        // 调用事件处理函数
        public void FireSuccessHandler(CometCoreClient client, CometMessage cm)
        {
            if (SuccessHandler != null)
            {
                SuccessHandler(client, new CometClientEventArgs(cm));
            }
        }

        public void FireFailureHandler(CometCoreClient client, CometMessage cm)
        {
            if (FailureHandler != null)
            {
                FailureHandler(client, new CometClientEventArgs(cm));
            }
        }

        public void FireTimeOutHandler(CometCoreClient client, CometMessage cm)
        {
            if (TimeoutHandler != null)
            {
                TimeoutHandler(client, new CometClientEventArgs(cm));
            }
        }

        public void Subscribe()
        {
            //  为什么要在主线程中分离出子线程来执行我们的订阅操作呢？ 虽然我们使用的是 异步请求+异步读取，最终订阅成功并操作UI的线程肯定不是在此线程，但是我们要知道
            // BeginGetResponse 方法在变为异步之前需要先完成一些同步设定任务 (例如 DNS 解析、代理检测和 TCP 套接字连接)，因此此方法不能在UI线程上调用，因为它可能需要严重的时间 (到几分钟基于网络设置) 完成初始同步组任务。
            Thread td = new Thread(SubscribeMethod);
            td.Start();
        }
        
         
        public void SubscribeMethod()
        {
            CometAsyncRequest request = new CometAsyncRequest();
            request.Client = this;
            request.HandlerUrl = this.HandlerUrl;
            
            request.SendCometRequest(new string[] { "privateToken", "lastMessageId" }, new string[] { this.PrivateToken, this.LastMessageId.ToString() });
        }
    }
}
