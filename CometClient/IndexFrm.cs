using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatData.ChatBLL;
using ChatData.ChatModel;

namespace CometClient
{
    public partial class IndexFrm : Form
    {
        UserBll  _userbll  = new  UserBll();

        // 本页面只做登陆功能
        public IndexFrm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string txtUserName = cbText.Text.ToString();
            string txtPassword = txtPwd.Text.ToString();
            bool   bLogin =  _userbll.UserLogin(txtUserName, txtPassword);

            string privateToken = string.Empty;
            string publicToken = string.Empty;
            string nikeName = string.Empty;

            if (bLogin == false)
                return;
            else
            {
                var  user  =_userbll.GetUserOne(new UserInfo() {UserName = txtUserName});
                privateToken = user.UserId.ToString();
                publicToken = user.UserName;
                nikeName = user.NikeName;
            }

            // 初始化客户端
            CometClient.ServiceReference.ChatServiceClient service = new CometClient.ServiceReference.ChatServiceClient();
            bool   ret= service.InitializeClient(publicToken,privateToken,nikeName);
            if (ret == true)
            {
                CometClientFrm client = new CometClientFrm(privateToken, publicToken, nikeName);
                this.Hide();
                client.Show();
            }
            else
            {
                // TODO
            }
            
           
        }

        private void cbText_Enter(object sender, EventArgs e)
        {
            cbText.Text = "";
            txtPwd.Text = "";
        }
    }
}
