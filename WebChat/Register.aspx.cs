using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChatData;

namespace WebChat
{
    public partial class Register : System.Web.UI.Page
    {
        private readonly UserChat  _userChat = new UserChat();

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
            _userChat.User.UserName = Request.Form["UserName"];

            _userChat.Register_User();

            return true;
        }
    }
}