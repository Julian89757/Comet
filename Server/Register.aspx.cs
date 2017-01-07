using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetComet.Server.Services;
using ChatData;
using ChatData.ChatModel;

namespace AspNetComet.Server
{
    public partial class Register : System.Web.UI.Page
    {
        
        UserService   _userService  = new  UserService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                RegisterUser();
            }
        }

        // 注册用户
        private bool RegisterUser()
        {
            // 3.0  C# 对象初始化器
            UserInfo userInfo = new UserInfo
            {
                UserId =  Guid.NewGuid(),
                UserName  = Request.Form["UserName"],
                  NikeName   =  Request.Form["NikeName"],
                   PassWord =   Request.Form["Password"],
                    Sex = Request.Form["Sex"] == null?0:int.Parse(Request.Form["Sex"]),
                    BirthDay = DateTime.Parse(Request.Form["BirthDay"]),
                    Phone =  Request.Form["Phone"],
                    Email  =  Request.Form["Email"],                     
            };

            _userService.UserRegister(userInfo);
            return true;
        }
    }
}