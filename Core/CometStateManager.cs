using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Runtime.Serialization.Json;

namespace Comet.Core
{
    /// <summary>
    /// Comet状态管理器，提供记录Comet状态和管理Comet客户端的能力
    /// </summary>
    public class CometStateManager
    {
        public ICometStateProvider StateProvider { get; }
        private int workerThreadCount;
        private int maximumTimeSlot;
        private int currentThread = 0;
        private CometWaitThread[] workerThreads;
        private object state = new object();

        #region 定义Comet客户端订阅服务器管道的生命周期事件：初始化，关闭，开始订阅
        public event CometClientEventHandler ClientInitialized;
        public event CometClientEventHandler IdleClientKilled;
        public event CometClientEventHandler ClientSubscribed;

        internal void FireClientInitialized(CometClient cometClient)
        {
            ClientInitialized?.Invoke(this, new CometClientEventArgs(cometClient));
        }

        internal void FireIdleClientKilled(CometClient cometClient)
        {
            IdleClientKilled?.Invoke(this, new CometClientEventArgs(cometClient));
        }

        internal void FireClientSubscribed(CometClient cometClient)
        {
            ClientSubscribed?.Invoke(this, new CometClientEventArgs(cometClient));
        }

        #endregion

        public CometStateManager(ICometStateProvider stateProvider)
            : this(stateProvider, 5, 100)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateProvider">Comet状态记录器</param>
        /// <param name="workerThreadCount">工作线程数量</param>
        /// <param name="maximumTimeSlot">超时时间</param>
        public CometStateManager(ICometStateProvider stateProvider, int workerThreadCount, int maximumTimeSlot)
        {
            if (stateProvider == null)
                throw new ArgumentNullException("stateProvider");
            if (workerThreadCount <= 0)
                throw new ArgumentOutOfRangeException("workerThreadCount");

            StateProvider = stateProvider;
            this.workerThreadCount = workerThreadCount;
            this.maximumTimeSlot = maximumTimeSlot;

            // 初始化指定数量工作线程
            workerThreads = new CometWaitThread[workerThreadCount];
            for (int i = 0; i < workerThreadCount; i++)
            {
                workerThreads[i] = new CometWaitThread(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicToken">Comet客户端公开给外部的token,可以是用户名</param>
        /// <param name="privateToken">Comet客户端私有Token</param>
        /// <param name="aliasName">客户端别名</param>
        /// <param name="connectionTimeoutSeconds">客户端</param>
        /// <param name="connectionIdleSeconds"></param>
        /// <returns></returns>
        public void InitializeClient(string publicToken, string privateToken, string aliasName, int connectionTimeoutSeconds, int connectionIdleSeconds)
        {
            if (string.IsNullOrEmpty(publicToken))
                throw new ArgumentNullException(nameof(publicToken));
            if (string.IsNullOrEmpty(privateToken))
                throw new ArgumentNullException(nameof(privateToken));
            if (string.IsNullOrEmpty(aliasName))
                throw new ArgumentNullException(nameof(aliasName));
            if (connectionIdleSeconds <= 0)
                throw new ArgumentOutOfRangeException("connectionIdleSeconds must be greater than 0");
            if (connectionTimeoutSeconds <= 0)
                throw new ArgumentOutOfRangeException("connectionTimeoutSeconds must be greater than 0");

            var cometClient = new CometClient()
            {
                PublicToken = publicToken,
                PrivateToken = privateToken,
                AliasName = aliasName,
                LastActivity = DateTime.Now,
                ConnectionIdleSeconds = connectionIdleSeconds,
                ConnectionTimeoutSeconds = connectionTimeoutSeconds,
            };
            try
            {
                StateProvider.InitializeClient(cometClient);
            }
            catch (Exception ex)
            {
                return;
            }
            FireClientInitialized(cometClient);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        /// <param name="extraData"></param>
        /// <returns></returns>
        public IAsyncResult BeginSubscribe(HttpContext context, AsyncCallback callback, object extraData)
        {
            try
            {
                long lastMessageId;
                string privateToken = string.Empty;

                if (!long.TryParse(context.Request["lastMessageId"] ?? "-1", out lastMessageId))
                    throw CometException.CometHandlerParametersAreInvalidException();

                privateToken = context.Request["privateToken"];
                if (string.IsNullOrEmpty(privateToken))
                    throw CometException.CometHandlerParametersAreInvalidException();


                this.DebugWriteThreadInfo("BeginSubscribe");

                lock (state)
                {
                    Debug.WriteLine("目前有几个客户端连接" + this.GetCometClients());

                    CometClient cometClient = this.GetCometClient(privateToken);
                    this.FireClientSubscribed(cometClient);
                    // 从所有线程的请求队列中找到该CometClient，将其排除
                    for (int i = 0; i < this.workerThreadCount; i++)
                    {
                        this.workerThreads[i].DequeueCometWaitRequest(privateToken);
                    }

                    CometWaitRequest request = new CometWaitRequest(privateToken, lastMessageId, context, callback, extraData);

                    this.workerThreads[this.currentThread].QueueCometWaitRequest(request);
                    this.currentThread++;

                    if (this.currentThread >= this.workerThreadCount)
                        this.currentThread = 0;

                    return request.Result;
                }
            }
            catch (Exception ex)
            {
                this.WriteErrorToResponse(context, ex.Message);
                return null;
            }
        }

        // 在异步调用结束的时候，调用此业务方法
        public void EndSubscribe(IAsyncResult result)
        {
            this.DebugWriteThreadInfo("EndSubscribe");

            CometAsyncResult cometAsyncResult = result as CometAsyncResult;

            if (cometAsyncResult != null)
            {
                try
                {
                    CometMessage[] messages = cometAsyncResult.CometMessages;

                    if (messages != null && messages.Length > 0)
                    {
                        /*   此方式序列化产生的字符串不是常规JSon定义字符串，暂时不采用  
                        List<Type> knownTypes = new List<Type>();
                        foreach (CometMessage message in messages)
                        {
                            if (message.Contents != null)
                            {
                                Type knownType = message.Contents.GetType();

                                if (!knownTypes.Contains(knownType))
                                {
                                    knownTypes.Add(knownType);
                                }
                            }
                        }
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(messages.GetType(),knownTypes);
                        serializer.WriteObject(((CometAsyncResult)result).Context.Response.OutputStream, messages);
                       */
                        var strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(messages);
                        ((CometAsyncResult)result).Context.Response.Write(strResponse);

                    }
                }
                catch (Exception ex)
                {
                    this.WriteErrorToResponse(((CometAsyncResult)result).Context, new { From = "system", Message = ex.Message });
                }
            }
        }

        // 发送消息
        public void SendMessage(string clientPublicToken, string tip, object contents)
        {
            this.StateProvider.SendMessage(clientPublicToken, tip, contents);
        }

        public void SendMessage(string tip, object contents)
        {
            this.StateProvider.SendMessage(tip, contents);
        }

        // 向page对象注册客户端脚本资源
        public static void RegisterAspNetCometScripts(Page page)
        {
            page.ClientScript.RegisterClientScriptResource(typeof(CometStateManager), "Comet.Core.Scripts.AspNetComet.js");
        }

        /// <summary>
        /// Kill an IdleCometClient
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        public void KillIdleCometClient(string clientPrivateToken)
        {
            //  get the comet client
            CometClient cometClient = this.StateProvider.GetCometClient(clientPrivateToken);
            //  ok, tmie the client out
            this.StateProvider.KillIdleCometClient(clientPrivateToken);
            //  and fire
            this.FireIdleClientKilled(cometClient);
        }

        public CometClient GetCometClient(string clientPrivateToken)
        {
            return this.StateProvider.GetCometClient(clientPrivateToken);
        }

        public int GetCometClients()
        {
            return this.StateProvider.GetCometClients();
        }

        internal void DebugWriteThreadInfo(string message)
        {
            int workerAvailable = 0;
            int completionPortAvailable = 0;
            ThreadPool.GetAvailableThreads(out workerAvailable, out completionPortAvailable);

            Debug.WriteLine(string.Format("{0}: {1} {2} out of {3}/{4}", message, Thread.CurrentThread.IsThreadPoolThread, Thread.CurrentThread.ManagedThreadId, workerAvailable, completionPortAvailable));
        }

        private void WriteErrorToResponse(HttpContext context, object objMessage)
        {   
            CometMessage errorMessage = new CometMessage();

            errorMessage.Tip = "aspNetComet.error";
            errorMessage.MessageId = 0;
            errorMessage.Content = objMessage;

            CometMessage[] messages = new CometMessage[] { errorMessage };

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(messages.GetType());
            serializer.WriteObject(context.Response.OutputStream, messages);

            context.Response.End();
        }
    }
}
