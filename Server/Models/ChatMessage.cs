using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Runtime.Serialization;

namespace AspNetComet.Server
{
    //  聊天消息
    [DataContract]
    public class ChatMessage
    {
        [DataMember]
        private string from;
        [DataMember]
        private string message;

        // 来源
        public string From
        {
            get { return this.from; }
            set { this.from = value; }
        }

        // 消息
        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }

    }
}
