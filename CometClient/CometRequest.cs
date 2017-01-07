using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Diagnostics;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CometClient
{
    public abstract class CometRequest
    {
        public CometCoreClient Client {get;set;}
        public  string HandlerUrl       {get;set;}

        //public CometRequest(CometCoreClient client)
        //{
        //    this.client =client ;
        //    this.HandlerUrl=  client.HandlerUrl;
        //}
        private class RequestState
        {
            public const int BUFFER_SIZE = 1024;
            public byte[] BufferRead;
            public HttpWebRequest Request;
            public HttpWebResponse Response;
            public Stream StreamResponse;
            public Type ResponseType;
            public MemoryStream RequestData;

            public RequestState()
            {
                this.BufferRead = new byte[BUFFER_SIZE];                    // 响应数据的缓冲区
                this.Request = null;                                        // HttpWebRequest 对象
                this.Response = null;                                       // HttpWebResponse 对象
                this.StreamResponse = null;                                 // 响应流对象
                this.ResponseType = typeof(String);                                   // 响应数据类型
                this.RequestData = new MemoryStream();                      // 请求的数据 内存存储
               
            }
        }

        private RequestState requestState = null;
        private string httpUsername;
        private string httpPassword;

        protected T SendCommand<T>(string url, string[] parameters, string[] values)
        {
            T responseObject;
            using(WebResponse response = this.SendCommand(url, parameters, values))
            {
                using (Stream stream = response.GetResponseStream())
                {
                    //  here is the response stream, time to parse it into an array
                    //  ok, we need to JSON Deserialize this method
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                    responseObject = (T)serializer.ReadObject(stream);
                }
            }

            return responseObject;
        }

        protected WebResponse SendCommand(string url, string[] parameters, string[] values) 
        {
            //  ok, create the request object that is going to perform this
            //  request
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);


            //  and create the post data
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (this.httpUsername != null)
            {
                request.Credentials = new NetworkCredential(this.httpUsername, this.httpPassword);
            }            

            //
            //  ok, parse the data from the parameters into 
            //  the content data
            if (parameters != null && values != null && parameters.Length == values.Length)
            {
                //  ok, everything is ok, so craete the post data
                StringBuilder postData = new StringBuilder();

                for (int i = 0; i < parameters.Length; i++)
                {
                    if(i != 0)
                        postData.Append("&");

                    //  append for values
                    postData.AppendFormat("{0}={1}", parameters[i], HttpUtility.UrlEncode(values[i]));
                }

                //  and now create a postdata array
                byte [] content = ASCIIEncoding.ASCII.GetBytes(postData.ToString());
                //  and the content length
                request.ContentLength = content.Length;

                //
                //  ok, sorted, so lets get the request strema and write the content
                using (Stream requestStream = request.GetRequestStream())
                {
                    //  write the content
                    requestStream.Write(content, 0, content.Length);
                }
            }

            //
            //  ok, now get the response
            return request.GetResponse();
        }

        protected   void BeginWaitRequest<T>(string url, string[] parameters, string[] values)
        {
            if (this.requestState == null)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = 0;

                // 设置网络认证信息, 本项目使用privateToken，可以不使用验证
                if (this.httpUsername != null)
                {
                    request.Credentials = new NetworkCredential(this.httpUsername, this.httpPassword);
                }

                if (parameters != null && values != null && parameters.Length == values.Length)
                {
                    StringBuilder postData = new StringBuilder("");

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        postData.AppendFormat("&{0}={1}", parameters[i], HttpUtility.UrlEncode(values[i]));
                    }

                    byte[] content = ASCIIEncoding.ASCII.GetBytes(postData.ToString());
                    request.ContentLength = content.Length;
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(content, 0, content.Length);
                    }
                }

                this.requestState = new RequestState();

                this.requestState.Request = request;
                this.requestState.ResponseType = typeof(T);
                
                request.BeginGetResponse(new AsyncCallback(BeginGetResponse_Completed), this.requestState);

            }
        }

        // 将开启新线程处理异步操作的结果
        private void BeginGetResponse_Completed(IAsyncResult result)
        {
            RequestState requestState = (RequestState)result.AsyncState;
            HttpWebRequest request = requestState.Request;

           
            requestState.Response = (HttpWebResponse)request.EndGetResponse(result);

           
            requestState.StreamResponse = requestState.Response.GetResponseStream();

          
            requestState.StreamResponse.BeginRead(requestState.BufferRead, 0, RequestState.BUFFER_SIZE, new AsyncCallback(BeginRead_Completed), requestState);            
        }

        // 将开启新线程处理异步回调的结果
        private void BeginRead_Completed(IAsyncResult result)
        {
            RequestState requestState = (RequestState)result.AsyncState;
            Stream responseStream = requestState.StreamResponse;

            int read = responseStream.EndRead(result);

            if (read > 0)
            {
                requestState.RequestData.Write(requestState.BufferRead, 0, read);
                requestState.StreamResponse.BeginRead(requestState.BufferRead, 0, RequestState.BUFFER_SIZE, new AsyncCallback(BeginRead_Completed), requestState);            
            }
            else
            {
                // 设置流检索的位置（偏移量，起始位置）
                requestState.RequestData.Seek(0, SeekOrigin.Begin);

                CometMessage[]  objResult =  null;
                string  responseText = Encoding.UTF8.GetString(requestState.RequestData.ToArray());

                
                objResult = (responseText == "" ? null : JsonConvert.DeserializeObject<CometMessage[]>(responseText));
                

                if (objResult == null || objResult.Length == 0)
                {
                    this.Client.FireFailureHandler(this.Client, null);
                }
                else
                {
                    var  cometMessage =  objResult[0];
                    switch(cometMessage.Tip)
                    {
                        case "aspNetComet.error":
                            this.Client.FireFailureHandler(this.Client, cometMessage);
                            break;
                        case "aspNetComet.timeout":
                            this.Client.FireFailureHandler(this.Client, null);
                            break;
                        default:
                            foreach(var  cm  in objResult )
                            {
                                this.Client.FireSuccessHandler(this.Client, cm);
                            }
                            break;
                    }
                }
                responseStream.Close();
                requestState.Response.Close();
                requestState.RequestData.Close();
                this.requestState = null;

                Debug.WriteLine("响应数据为：" + responseText + "\t" + DateTime.Now);
                // 重新发起连接
                this.Client.SubscribeMethod();
            }
        }

        protected virtual void EndWaitRequest()
        {
        }
    }
}
