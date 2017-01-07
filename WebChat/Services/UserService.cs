using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
#region 命名空间
using ChatData.ChatBLL;
using ChatData.ChatModel;
#endregion
namespace ChatData
{
    public  class UserService
    {
        readonly UserBll _userBll = new UserBll();
        readonly UserRelationBll _userRelationBll= new UserRelationBll();

        public void Register_User(UserInfo userInfo)
        {
           
        }

        public string  Downline_User(Guid  userId)
        {
         
        }

        public string  Get_Friends( Guid  userId)
        {

        }

        public string Add_Friend(Guid userId1,Guid userId2)
        {
        
        }

        class Friends
        {
            public List<UserInfo> OnlineFriends { get; set; }
            public List<UserInfo> OfflineFriends { get; set; }
        }

    }
}
