using System;
using System.Collections.Generic;
using System.Text;

namespace ChatData.ChatModel
{
    /// <summary>
    /// 查看用户关系
    /// </summary>
    public class UserRelation
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public Guid  UserId { get; set; }
        /// <summary>
        /// 关系账号
        /// </summary>
        public Guid  FriendId { get; set; }

        public int Status { get; set; }
    }
}
