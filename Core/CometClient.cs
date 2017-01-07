using System;

namespace Comet.Core
{
    /// <summary>
    /// Comet客户端
    /// </summary>
    public class CometClient
    {
        // 设置客户端别名
        public string AliasName { get; set; }

        //  提供给用户本身的用户认证token
        public string PrivateToken { get; set; }
       
        //  提供给其他用户的用户认证token
        public string PublicToken { get; set; }
        
        //  客户端上次活动的时间
        public DateTime LastActivity { get; set; }

        //  客户端在很久没有连上，服务器认为已经彻底离线的空闲时间
        public int ConnectionIdleSeconds { get; set; }

        //  客户端在没有收到消息时候的超时时间
        public int ConnectionTimeoutSeconds { get; set; }
    }
}
