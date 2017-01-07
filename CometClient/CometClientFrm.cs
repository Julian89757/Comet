using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatData.ChatBLL;
using ChatData.ChatModel;
using Newtonsoft.Json;

namespace CometClient
{
    public partial class CometClientFrm : Form
    {
      //  private readonly string host = "http://localhost:2762/DefaultChannel.ashx";
        private readonly string _host = System.Configuration.ConfigurationManager.AppSettings["SubscribeAddress"].Trim().ToLower();
        private readonly string _privateToken = string.Empty;
        private readonly string _publicToken = string.Empty;
        private readonly string _nikeName = string.Empty;


        public CometClientFrm()
        {
            InitializeComponent();
        }

        public CometClientFrm(string privateToken, string publicToken, string nikeName)
        {
            InitializeComponent();

            this._publicToken = publicToken;
            this._privateToken = privateToken;
            this._nikeName = nikeName;

            CometCoreClient client = new CometCoreClient(_host, _privateToken);
            client.AddFailureHandler(FailureHandler);
            client.AddFailureHandler(FailureHandler);
            client.AddSuccessHandler(SuccessHandler);

            // 这里是UI线程
            client.Subscribe();

        }

        private void CometClientFrm_Load(object sender, EventArgs e)
        {
            Guid userGuid = Guid.Parse(_privateToken);

            UserRelationBll relationBll = new UserRelationBll();
            var  userFriends =  relationBll.GetFriends(new UserInfo
            {
                UserId = userGuid,
            });

            //ListBox.ListViewItemCollection lvic = new ListView.ListViewItemCollection(this.lvList);
            //foreach (var ii in userFriends)
            //{
            //    lvic.Add(ii.UserName);
            //}

            foreach (var ii in userFriends)
            {
                this.lvList.Items.Add(ii.UserName);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var itemColl = lvList.SelectedItems;
            if (itemColl.Count == 0)
            {
                MessageBox.Show("未选中任何发送对象", "提示", MessageBoxButtons.OK);
                return;
            }
            CometClient.ServiceReference.ChatServiceClient service = new CometClient.ServiceReference.ChatServiceClient();
            try
            {
                service.Send(this._privateToken, richTextBox2.Text.ToString(), itemColl[0].ToString());
            }
            catch (FaultException exception)
            {
                richTextBox1.Text += exception.Reason.ToString() + "\n";
            }


            richTextBox2.Text = "";
        }

        private void btnBroadCast_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
        }


        public void FailureHandler(object sender, CometClientEventArgs args)
        {
            //CometMessage mess = args.Message;
            //CometCoreClient client = (CometCoreClient)sender;
            //client.LastMessageId = mess.MessageId;
            //Action<string> asyncUiDelegate = delegate(string data) { this.richTextBox1.Text += data + "\n"; };

            //this.Invoke(asyncUiDelegate, new object[] { JsonConvert.SerializeObject(mess) });  // 这个this 是窗体控件，这里的意义是调用窗体的同步委托来 操作窗体UI
        }

        public void TimeOutHandler(object sender, CometClientEventArgs args)
        {
            // Todo 超时处理          
        }

        //  Winform 客户端订阅到成功消息时候的自身处理函数（ 这里还要解决跨线程访问的问题）
        public void SuccessHandler(object sender, CometClientEventArgs args)
        {
            CometMessage mess = args.Message;
            CometCoreClient client = (CometCoreClient)sender;

            client.LastMessageId = mess.MessageId;

            // 解析动态JSon Object对象,dynamic  关键字的用法: 指示编译器不要解析和检查，封装操作信心用于运行时计算
            //frm.richTextBox1.Text = mess.Tip + "\t" + ((dynamic)(Newtonsoft.Json.Linq.JObject)mess.Content).from.Value +
            //                                  "\t" + ((dynamic)(Newtonsoft.Json.Linq.JObject)mess.Content).message.Value;
            //frm.richTextBox1.Text = mess.Tip + JsonConvert.SerializeObject(mess.Content);

            Action<string> asyncUiDelegate = delegate(string data) { this.richTextBox1.Text += data + "\n"; };

            this.Invoke(asyncUiDelegate, new object[] { JsonConvert.SerializeObject(mess) });  // 这个this 是窗体控件，这里的意义是调用窗体的同步委托来 操作窗体UI
        }

        private void lvList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lvList_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }

        private void lvList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var  s  = e.Item;
            s.Checked = true;
        }

        // winform关闭窗体，不一定释放了所有资源和线程
        private void CometClientFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
            this.Dispose();
            this.Close();

        }

        private void CometClientFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 尝试能不能再窗体关闭之后 停止内部线程的运行。TODO
        }

    }
}
