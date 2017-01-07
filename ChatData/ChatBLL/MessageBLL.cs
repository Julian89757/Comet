using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ChatData.ChatDAL;
using ChatData.ChatModel;

namespace ChatData.ChatBLL
{
    /// <summary>
    /// 聊天信息--业务逻辑类
    /// </summary>
    public class MessageBll
    {
        private  readonly MessageDAL  _messageDal = new MessageDAL();


        #region 新增聊天信息
        /// <summary>
        /// 新增聊天信息记录
        /// </summary>
        /// <param name="message">聊天信息</param>
        /// <returns></returns>
        public bool AddMessage(MessageInfo message)
        {
            int affectRows = _messageDal.AddMessage(message);
            return affectRows > 0;
        }
        #endregion

        #region 修改聊天信息
        /// <summary>
        /// 修改聊天信息记录(根据id)
        /// </summary>
        /// <param name="message">聊天信息</param>
        /// <returns></returns>
        public bool UpdateMessage(MessageInfo message)
        {
            int affectRows = _messageDal.UpdateMessage(message);
            return affectRows > 0;
        }
        #endregion
    }
}
