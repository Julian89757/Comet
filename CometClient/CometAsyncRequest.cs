using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;

namespace CometClient
{
    public class CometAsyncRequest : CometRequest
    {
        private bool stop = false;
        private int requests = 0;
        private int responses = 0;
        
        //public  CometAsyncRequest(CometCoreClient Client) :base(client)
        //{

        //}
      
        public void SendCometRequest(string[] paramers, string[] values)
        {
            if (!this.stop)
            {
                this.BeginWaitRequest<object>(this.HandlerUrl, paramers, values);
                requests++;
            }
        }

        protected  override void EndWaitRequest()
        {
            this.Client.Subscribe();
        }

        public bool Stop
        {
            get { return this.stop; }
            set { this.stop = value; }
        }

        public int Requests
        {
            get { return this.requests; }
        }

        public int Responses
        {
            get { return this.responses; }
        }
    }
}
