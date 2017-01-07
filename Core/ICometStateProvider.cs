using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Comet.Core
{
    // Comet状态记录器 适配器模式
    public interface ICometStateProvider
    {
        void InitializeClient(CometClient cometClient);
        
        /// <summary>
        /// 获取指定客户端的消息队列中消息，只关注消息ID>最近一次的消息Id
        /// </summary>
        /// <param name="clientPrivateToken">私有Token</param>
        /// <param name="lastMessageId">最近一次的消息Id</param>
        /// <returns></returns>
        CometMessage[] GetMessages(string clientPrivateToken, long lastMessageId);

        // 给消息排队，name 是发送方
        void SendMessage(string clientPublicToken, string name, object content);
        
        //  广播给其他客户端
        void SendMessage(string name, object contents);
       
        // 返回 CometClient
        CometClient GetCometClient(string clientPrivateToken);

        int GetCometClients();

        void KillIdleCometClient(string clientPrivateToken);
    }
}
