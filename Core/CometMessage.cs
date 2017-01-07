using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Comet.Core
{
    /// <summary>
    /// Comet消息报文
    /// </summary>
    [DataContract(Name = "mid")]
    public class CometMessage
    {
        // MessageId， track which message the client last received
        [DataMember(Name = "mid")]
        public long MessageId { get; set; }

        // Message Content
        [DataMember(Name = "c")]
        public object Content { get; set; }

        // Error messgae if this is a failure
        [DataMember(Name ="n")]
        public string Name { get; set; }
    }
}
