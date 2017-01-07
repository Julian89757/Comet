using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AspNetComet.Core;

namespace AspNetComet.Server.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ChatService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ChatService.svc or ChatService.svc.cs at the Solution Explorer and start debugging.
    public class ChatService : IChatService
    {

        public int Add(int a,int  b)
        {
            return a + b;
        }
        //  初始化客户端连接
        public bool  InitializeClient(string publicToken,string privateToken,string displayName)
        {
           return   DefaultChannelHandler.StateManager.InitializeClient(publicToken, privateToken,displayName, 5, 5) !=null ? true:false;

        }

        public void SendMessage(string clientPrivateToken, string message, string clientPublicToken)
        {
            ChatMessage chatMessage = new ChatMessage();
            CometClient cometClient = DefaultChannelHandler.StateManager.GetCometClient(clientPrivateToken);

            chatMessage.From = cometClient.DisplayName;
            chatMessage.Message = message;
            try
            {
                DefaultChannelHandler.StateManager.SendMessage(clientPublicToken, "PushMessage", chatMessage);
            }
            catch (CometException exception)
            {
                throw new  FaultException(exception.Message);
            }
        }

        // 发送广播消息
        public void SendMessage(string clientPrivateToken, string message)
        {
            ChatMessage chatMessage = new ChatMessage();
            CometClient cometClient = DefaultChannelHandler.StateManager.GetCometClient(clientPrivateToken);

            chatMessage.From = cometClient.DisplayName;
            chatMessage.Message = message;

            DefaultChannelHandler.StateManager.SendMessage("ChatMessage", chatMessage);

            // Add your operation implementation here
            return;
        }


    }
}
