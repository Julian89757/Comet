<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="Server.Chat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body onload="Connect()">
    <script language="javascript" type="text/javascript">
    
    var defaultChannel = null;
    
    // 页面加载的时候 开始订阅消息
    function Connect()
    {
        if(defaultChannel == null)
        {
            defaultChannel = new AspNetComet("/DefaultChannel.ashx", "<%=this.Request["username"] %>", "defaultChannel");
            defaultChannel.addTimeoutHandler(TimeoutHandler);
            defaultChannel.addFailureHandler(FailureHandler);
            defaultChannel.addSuccessHandler(SuccessHandler);
            defaultChannel.subscribe();
        }
    }
    
    function SuccessHandler(privateToken, alias, message)
    {
        document.getElementById("messages").innerHTML += message.c.f + ": " + message.c.m + "<br/>";
    }
    
    function FailureHandler(privateToken, alias, error)
    {
        //document.getElementById("messages").innerHTML += "error" + error + "<br/>";
        alert(error);
    }
    
    function TimeoutHandler(privateToken, alias)
    {
        //alert("timeout");
        //document.getElementById("messages").innerHTML += "timeout<br/>";
    }    
    
    function SendMessage()
    {
        //  这个WCF 怎么可以在这里使用
        var service = new ChatService();
        
        service.SendMessage("<%=this.Request["username"] %>", document.getElementById("message").value,
            function()
            {
                document.getElementById("message").value = '';
            },
            function()
            {
                alert("Send failed");
            });
    }
    
    function onKeyPress(e)
    {
        var keyCode = null;

        if(e.which)
            keyCode = e.which;
        else if(e.keyCode) 
            keyCode = e.keyCode;
            
        if(keyCode == 13) 
        {
            SendMessage();
            return false;
        }
        return true;
    }
    
    </script>
    

    <form id="form1" runat="server">
    <%--必须添加 script manager控件，内部添加一个Service来引用wcf服务--%>
    <asp:ScriptManager ID="ScriptManager" runat="server">
        <Services>
            <asp:ServiceReference Path="~/Channels/ChatService.svc" />
        </Services>
    </asp:ScriptManager>
    
    <div>
        
        <p>
        Messages:
        <div id="messages" style="width:500px;height:300px;border:1px solid silver;overflow:auto">
        </div>
        
        
        <input type="text" id="message" onkeypress="return onKeyPress(event)" />&nbsp;
        <input type="button" onclick="SendMessage()" value="Send Message" />
        
        </p>
    
    
    </div>
    </form>
</body>
</html>
