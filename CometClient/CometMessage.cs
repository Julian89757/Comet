using System;
using System.Collections.Generic;
using System.Text;
#region 命名空间
using System.Web;
using Newtonsoft.Json;
using System.Runtime.Serialization;
#endregion

namespace CometClient
{
    //  Comet推送消息
    [DataContract]
    public class CometMessage
    {
        // 信息提示信息
        [DataMember]
        public string Tip{ get; set; }

        // 记录客户端收到的消息ID
        [DataMember]
        public long MessageId { get; set; }

        // 消息体（ string/消息详细信息）
        [DataMember]
        public object Content{  get; set; }
    }

    //  内容消息
    [DataContract]
    public class ChatMessage
    {
        // 来源
        [DataMember]
        public string From {get; set; }

        // 消息
        [DataMember]
        public string Message  {get; set; }

    }

}
