using System;
using System.Collections.Generic;
using System.Web;
using System.Text;

using ChatData.ChatBLL;
using ChatData.ChatModel;

namespace AspNetComet.Server.Services
{
    public  class UserService
    {
        readonly UserBll _userBll = new UserBll();
        readonly UserRelationBll _userRelationBll= new UserRelationBll();

        public bool  UserRegister(UserInfo userInfo)
        {
            return _userBll.AddUser(userInfo);
        }

        public string  UserOffline(Guid  userId)
        {
            return null;
            // todo 
        }

        public string  GetFriends( Guid  userId)
        {
            return null;
            // todo 
        }

        public string AddFriend(Guid userId1,Guid userId2)
        {
            return null;
            // todo 
        }

        class Friends
        {
            public List<UserInfo> OnlineFriends { get; set; }
            public List<UserInfo> OfflineFriends { get; set; }
        }

    }
}
