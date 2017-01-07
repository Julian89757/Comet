using System;
using System.Collections.Generic;
using System.Text;
#region 命名空间
using System.Net;
using ChatData.PHPLibrary;
using ChatData.ChatModel;
using  ChatData.Common;
#endregion

namespace ChatData
{
    public class UserChat
    {
        public UserInfo User { get; set; }

        public UserChat()
        {
            
        }
        
        #region 注册用户
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <returns></returns>
        public void  Register_User()
        {
            //除用户验证参数外其他参数
            PHPArray array = new PHPArray();
            array.Add("UserName", User.UserName);
            array.Add("Sex", User.Sex);
            array.Add("Birthday", User.BirthDay);
            array.Add("Email", User.Email);
            array = GetParam(array);
            new UserService().Register_User(array);
        }
        #endregion

        #region 用户上线
        /// <summary>
        /// 用户上线
        /// </summary>
        /// <returns></returns>
        public string Verify_User()
        {
            PHPArray array = GetParam();
            return  new UserService().Verify_User(array);
        } 
        #endregion

        #region 用户下线
        /// <summary>
        /// 用户下线
        /// </summary>
        /// <returns></returns>
        public string Downline_User()
        {
            PHPArray array = GetParam();
            return new UserService().Downline_User(array);
        }
        #endregion

        #region 获取好友
        /// <summary>
        /// 获取好友
        /// </summary>
        /// <returns></returns>
        public string Get_Friends()
        {
            PHPArray array = GetParam();
            return new UserService().Get_Friends(array);
        }
        #endregion

        #region 添加好友
        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="friend">好友信息</param>
        /// <returns></returns>
        public string Add_Friend(UserInfo friend)
        {
            PHPArray array = new PHPArray();
            array.Add("FriendId", friend.UserId);
            array = GetParam(array);
            return new UserService().Add_Friend(array);
        }
        #endregion

        #region 发送消息
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="friend">发送消息</param>
        /// <returns></returns>
        public string Send_Msg(MessageInfo message)
        {
            PHPArray array = new PHPArray();
            array.Add("ReciveUserId", message.ReciveUserId);
            array.Add("Content", message.Content);
            array =GetParam(array);
            return new MessageService().Send_Msg(array);
        }
        #endregion

        #region 私有方法 
        // 基本参数
        private PHPArray GetParam()
        {
            OAuth o = new OAuth(this);
            return o.GetParameList();
        }
        /// <summary>
        /// 组装参数
        /// </summary>
        /// <param name="array">其他参数</param>
        /// <returns></returns>
        private PHPArray GetParam(PHPArray array)
        {
            OAuth o = new OAuth(this);
            o.Array = array;
            return o.GetParameList();
        }
        #endregion
    }
}
