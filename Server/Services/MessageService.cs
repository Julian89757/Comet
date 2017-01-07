using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using ChatData.ChatBLL;
using ChatData.ChatModel;

namespace AspNetComet.Server.Services
{
    public class MessageService
    {
        readonly MessageBll  _messageBll = new MessageBll();

        #region 发送消息
        /// <summary>
        /// 发送消息
        /// </summary>
        public string Send_Msg(MessageInfo  mess)
        {
            return null;
            // TODO 
        }
        #endregion
    }
}
