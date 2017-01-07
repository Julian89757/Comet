using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
#region 命名空间
using ChatData.ChatDAL;
using ChatData.ChatModel;
#endregion

namespace ChatData.ChatBLL
{
    /// <summary>
    /// 用户关系--业务逻辑类
    /// </summary>
    public class UserRelationBll
    {
 
        private  readonly UserRelationDAL userrelationDAL = new UserRelationDAL();
        private  readonly UserBll userBLL = new UserBll();
        

        #region 根据用户id查询好友
        /// <summary>
        /// 根据用户id查询好友
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        public List<UserInfo> GetFriends(UserInfo user)
        {
            DataTable dt = userrelationDAL.GetFriends(user);
            List<UserInfo> users = new List<UserInfo>();

            Guid GuidUserID = Guid.Empty;
            foreach (DataRow item in dt.Rows)
            {
                UserInfo  u = new UserInfo ();
                Guid.TryParse(item["FriendId"].ToString(), out  GuidUserID);
                if(GuidUserID != user.UserId )
                    u = userBLL.GetUserOne(GuidUserID);
                else
                {
                    Guid.TryParse(item["UserId"].ToString(), out  GuidUserID);
                    u = userBLL.GetUserOne(GuidUserID);
                }
                users.Add(u);
            }
            // TODO  必须解决好友的数据存储问题
            if (users.Count > 0) 
                return users;
            return null;
        }
        #endregion

        #region 新增用户关系
        /// <summary>
        /// 新增用户关系
        /// </summary>
        /// <param name="user">关系信息</param>
        /// <returns></returns>
        public bool AddUserRelation(UserRelation user_relation)
        {
            int AffectRows = userrelationDAL.AddUserRelation(user_relation);
            return AffectRows > 0;
        }
        #endregion
    }
}
