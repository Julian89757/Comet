using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AspNetComet.Server.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IChatService" in both code and config file together.
    [ServiceContract]
    public interface IChatService
    {

        [OperationContract]
        int Add(int a,int  b);

        [OperationContract]
        bool  InitializeClient(string publicToken, string privateToken, string displayName);

        [OperationContract(Name = "Send")]
        void SendMessage(string clientPrivateToken, string message, string clientPublicToken);

        [OperationContract]
        void SendMessage(string clientPrivateToken, string message);
    }
}

/*
 *  这玩意交互的时候一点都不好用。 尝试使用webapi， restful 风格
 * 
 */
