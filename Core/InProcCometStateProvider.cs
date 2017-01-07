using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace  Comet.Core
{
    // 以内存的方式实现 记录Comet状态
    public class InProcCometStateProvider : ICometStateProvider
    {
        private static object state = new object();

        //  维护单个Client的Comet 消息
        private class InProcCometClient
        {
            public CometClient CometClient;
            public Dictionary<long, CometMessage> Messages = new Dictionary<long, CometMessage>();
            public long NextMessageId = 1;
        }
        
        // 
        private Dictionary<string, InProcCometClient> publicClients = new Dictionary<string, InProcCometClient>();
        private Dictionary<string, InProcCometClient> privateClients = new Dictionary<string, InProcCometClient>();

        #region ICometStateProvider Members

        public void InitializeClient(CometClient cometClient)
        {
            if (cometClient == null)
                throw new ArgumentNullException(nameof(cometClient));

            lock (state)
            {
                if (publicClients.ContainsKey(cometClient.PublicToken) || privateClients.ContainsKey(cometClient.PrivateToken))
                    throw CometException.CometClientAlreadyExistsException();

                var inProcCometClient = new InProcCometClient()
                {
                    CometClient = cometClient
                };
                publicClients.Add(cometClient.PublicToken, inProcCometClient);
                privateClients.Add(cometClient.PrivateToken, inProcCometClient);
            }

        }

        public CometMessage[] GetMessages(string clientPrivateToken, long lastMessageId)
        {
            if(string.IsNullOrEmpty(clientPrivateToken))
                throw new ArgumentNullException("clientPrivateToken");

            lock (state)
            {
                if (!privateClients.ContainsKey(clientPrivateToken))
                    throw CometException.CometClientDoesNotExistException();

                InProcCometClient cometClient = privateClients[clientPrivateToken];

                List<long> toDelete = new List<long>();
                List<long> toReturn = new List<long>();

                // 根据lastMessageId 机制可以得到离线消息
                foreach(long key in cometClient.Messages.Keys)
                {
                    if(key <= lastMessageId)
                        toDelete.Add(key);
                    else
                        toReturn.Add(key);
                }

                foreach (long key in toDelete)
                {
                    cometClient.Messages.Remove(key);
                }

                List<CometMessage> cometMessages = new List<CometMessage>();
                foreach (long key in toReturn)
                {
                    cometMessages.Add(cometClient.Messages[key]);
                }

                return cometMessages.ToArray();

            }
        }

        /// <summary>
        /// Send a message to a specific client
        /// </summary>
        /// <param name="clientPublicToken"></param>
        /// <param name="name"></param>
        /// <param name="contents"></param>
        public void SendMessage(string clientPublicToken, string tip, object content)
        {
            if (string.IsNullOrEmpty(clientPublicToken))
                throw new ArgumentNullException("clientPublicToken");
            if (content == null)
                throw new ArgumentNullException("content");

            lock (state)
            {
                if (!publicClients.ContainsKey(clientPublicToken))
                    throw CometException.CometClientDoesNotExistException();

                InProcCometClient cometClient = publicClients[clientPublicToken];

                CometMessage message = new CometMessage();

                message.Content = content;
                message.Tip = tip;
                message.MessageId = cometClient.NextMessageId;

                cometClient.NextMessageId++;
                cometClient.Messages.Add(message.MessageId, message);
            }

        }

        /// <summary>
        ///  广播给所有用户
        /// </summary>
        /// <param name="tip">发送提示</param>
        /// <param name="contents">发送内容</param>
        public void SendMessage(string tip, object contents)
        {
            if (contents == null)
                throw new ArgumentNullException("contents");

            lock (state)
            {
                foreach (InProcCometClient cometClient in publicClients.Values)
                {
                    // 将消息放进数组
                    CometMessage message = new CometMessage()
                    {
                           Tip = tip,
                            Content = contents,
                            MessageId = cometClient.NextMessageId
                    };

                    cometClient.NextMessageId++;
                    cometClient.Messages.Add(message.MessageId, message);
                }
            }
        }

        /// <summary>
        /// Get the client from the state provider
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        /// <returns></returns>
        public CometClient GetCometClient(string clientPrivateToken)
        {
          //  Debug.WriteLine("目前有几个客户端连接"+this.publicClients.Count);

            if (!this.privateClients.ContainsKey(clientPrivateToken))
                throw CometException.CometClientDoesNotExistException();

            //  return the client private token
            return this.privateClients[clientPrivateToken].CometClient;
        }

        public int GetCometClients()
        {
            if (this.publicClients.Count != this.privateClients.Count)
            {
                Debug.WriteLine("这里有BUG，请注意。");
                throw new  Exception("内部异常");
            }
            else
            {
                return this.publicClients.Count;
            }
        }

        /// <summary>
        /// Remove an idle client from the memory
        /// </summary>
        /// <param name="clientPrivateToken"></param>
        public void KillIdleCometClient(string clientPrivateToken)
        {
            if (!this.privateClients.ContainsKey(clientPrivateToken))
                throw CometException.CometClientDoesNotExistException();

            //  get the client
            InProcCometClient ipCometClient = this.privateClients[clientPrivateToken];

            //  and remove the dictionarys
            this.privateClients.Remove(ipCometClient.CometClient.PrivateToken);
            this.publicClients.Remove(ipCometClient.CometClient.PublicToken);
            Debug.WriteLine(ipCometClient.CometClient.PublicToken +" has  been removed  from  memmory.");
        }

        #endregion
    }
}
