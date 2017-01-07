using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ChatData.ChatModel
{
    public class UserInfo
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string NikeName { get; set; }

        public string PassWord { get; set; }

        /// <summary>
        /// 性别
        /// 0 未选择
        /// 1 男
        /// 2 女
        /// </summary>
        public int Sex { get; set; }

        public DateTime?  BirthDay { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        /// <summary>
        /// 状态
        /// 0--离线
        /// 1--在线
        /// 2--待定
        /// </summary>
        public int Status { get; set; }

        public DateTime? OnlineTime { get; set; }

        public DateTime? OfflineTime { get; set; }

        /// <summary>
        /// 有效、无效
        /// </summary>
        public int IsValid { get; set; }
    }
}
