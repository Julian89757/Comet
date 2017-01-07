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
    public partial class Chat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //  请求网页的时候，asp.net 服务器向浏览器客户端注册脚本
          //  CometStateManager.RegisterAspNetCometScripts(this);
        }
    }
}
