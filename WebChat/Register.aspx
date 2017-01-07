<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebChat.Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户注册</title>
    <link href="Styles/Register.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/Register.js" type="text/javascript"></script>
</head>
<body>
    <form id="myForm" runat="server">
    <div id="register">
        <div id="regTitle">
            用户注册
        </div>
        <div id="regContent">
            <table>
                <tr>
                    <td class="title"  style="text-align: center;">
                        用户名：
                    </td>
                    <td>
                        <input type="text" id="txtUserName" class="txt" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                       昵 称：
                    </td>
                    <td>
                        <input type="text" id="txtNikeName" class="txt" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        密 码：
                    </td>
                    <td>
                        <input type="password" id="txtPassWord" class="txt" />
                    </td>
                </tr>
                 <tr>
                    <td style="text-align: center;">
                        确认密码：
                    </td>
                    <td>
                        <input type="password" id="txtRePassWord" class="txt" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        性 别：
                    </td>
                    <td>
                        <input type="radio" name="sex"  value="男" />男
                        <input type="radio" name="sex"  value="女" />女
                    </td>
                </tr>
                 <tr>
                    <td style="text-align: center;">
                        手 机：
                    </td>
                    <td>
                        <input type="text" id="textPhone" class="txt" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        邮 箱：
                    </td>
                    <td>
                        <input type="text" id="txtEmail" class="txt" />
                    </td>
                </tr>
                 <tr>
                    <td style="text-align: center;">
                        出生日期：
                    </td>
                    <td>
                        <input type="text" id="textBirthday" class="txt" />
                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <div id="btnExit">
                            退出</div>
                        <div id="btnSubmit">
                            注册</div>
                        <div id="msg">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
