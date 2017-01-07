using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using AspNetComet.Core;

namespace AspNetComet.Server
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // 注册方法，其实就是初始化token
        protected void Login_Click(object sender, EventArgs e)
        {
            try
            {
                // 初始化客户端Comet 连接，参数（私有token。公有token，空闲超时时间，连接超时时间 ）
                DefaultChannelHandler.StateManager.InitializeClient(
                    this.username.Text, this.username.Text, this.username.Text, 5, 5);

                Response.Redirect("chat.aspx?username=" + this.username.Text);
            }
            catch (CometException ce)
            {
                if (ce.MessageId == CometException.CometClientAlreadyExists)
                {
                    // 客户端已经存在，显示一个错误
                    this.errorMessage.Text = "User is already logged into the chat application.";
                }
            }
        }
    }
}
