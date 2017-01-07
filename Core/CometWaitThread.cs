using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace  Comet.Core
{
    /// <summary>
    /// Comet后台工作线程，负责处理Comet排队请求
    /// </summary>
    public class CometWaitThread
    {
        private object state = new object();
        public List<CometWaitRequest> WaitRequests { get; }
        public CometStateManager StateManager { get; set; }

        public CometWaitThread(CometStateManager stateManager)
        {
            WaitRequests = new List<CometWaitRequest>()
            StateManager = stateManager;

            Thread t = new Thread(new ThreadStart(QueueCometWaitRequest_WaitCallback));
            t.IsBackground = false;
            t.Start();
        }

        internal void QueueCometWaitRequest(CometWaitRequest request)
        {
            lock (state)
            {
                WaitRequests.Add(request);
            }
        }

        internal void DeactivateCometWaitRequest(CometWaitRequest request)
        {
            lock (state)
            {
                request.DateDeactivated = DateTime.Now;
            }
        }

        private void QueueCometWaitRequest_Finished(object target)
        {
            CometWaitRequest request = target as CometWaitRequest;
            request.Result.SetCompleted();
        }

        // 线程核心执行函数
        private void QueueCometWaitRequest_WaitCallback()
        {
            while (true)
            {
                CometWaitRequest[] processRequest;

                lock (state)
                {
                    processRequest = WaitRequests.ToArray();
                }
                //if (processRequest.Length == 0)
                //    break;

                if (processRequest.Length == 0)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    for (int i = 0; i < processRequest.Length; i++)
                    {
                        try
                        {
                            CometClient cometClient = StateManager.StateProvider.GetCometClient(processRequest[i].ClientPrivateToken);

                            if (processRequest[i].Active)
                            {
                                Thread.Sleep(100);

                                if (DateTime.Now.Subtract(processRequest[i].DateTimeAdded).TotalSeconds >= cometClient.ConnectionTimeoutSeconds)
                                { 
                                    DeactivateCometWaitRequest(processRequest[i]);
                                    CometMessage timeoutMessage = new CometMessage()
                                        {
                                            MessageId = 0,
                                            Tip = "aspNetComet.timeout",
                                            Content = null
                                        };
                                    processRequest[i].Result.CometMessages = new CometMessage[] { timeoutMessage };
                                    this.QueueCometWaitRequest_Finished(processRequest[i]);
                                }
                                else
                                {
                                    // 这里是消息响应的根本，响应给客户端的是　消息数组
                                    CometMessage[] messages = CheckForServerPushMessages(processRequest[i]);

                                    if (messages != null && messages.Length > 0)
                                    {
                                        processRequest[i].Result.CometMessages = messages;
                                        DeactivateCometWaitRequest(processRequest[i]);
                                        QueueCometWaitRequest_Finished(processRequest[i]);
                                    }
                                }
                            }
                            else
                            {
                                // 长连接 连接超时，断网或者 离线
                                this.CheckForIdleCometWaitRequest(processRequest[i], cometClient);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (processRequest[i].Active)
                            {
                                //  ok, this one has screwed up, so
                                //  we need to dequeue the request from ASP.NET, basically disable it and return
                                //  dequeue the request 
                                DeactivateCometWaitRequest(processRequest[i]);

                                //  get the message
                                CometMessage errorMessage = new CometMessage()
                                {
                                    MessageId = 0,
                                    Tip = "aspNetComet.error",
                                    Content= ex.Message
                                };

                                //
                                //  ok, we we timeout the message
                                processRequest[i].Result.CometMessages = new CometMessage[] { errorMessage };
                                //  call the message
                                QueueCometWaitRequest_Finished(processRequest[i]);
                            }
                            else
                            {
                                //  this is not an active request, so we dequeue it from the
                                //  thread
                                DequeueCometWaitRequest(processRequest[i].ClientPrivateToken);
                            }
                        }
                    }
                }
            }
        }

        private void CheckForIdleCometWaitRequest(CometWaitRequest request, CometClient cometClient)
        {
            lock (state)
            {
                if (DateTime.Now.Subtract(request.DateDeactivated.Value).TotalSeconds >= cometClient.ConnectionIdleSeconds)
                {
                    //  ok, this dude has timed out, so we remove it
                    StateManager.KillIdleCometClient(cometClient.PrivateToken);
                    //  and deque the request
                    WaitRequests.Remove(request);
                }
            }
        }

        private CometMessage[] CheckForServerPushMessages(CometWaitRequest request)
        {
            //
            //  ok, we we need to do is get the messages 
            //  that are stored in the state provider
            return StateManager.StateProvider.GetMessages(request.ClientPrivateToken, request.LastMessageId);
        }


        internal void DequeueCometWaitRequest(string privateToken)
        {
            lock (state)
            {
                for(int i =0; i < WaitRequests.Count; i ++)
                {
                    CometWaitRequest request = WaitRequests[i];

                    if (request.ClientPrivateToken == privateToken)
                    {
                        WaitRequests.Remove(request);
                        break;
                    }
                }
            }
        }
    }
}
