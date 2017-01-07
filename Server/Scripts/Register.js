$(function () {

    var now = new Date().toLocaleDateString();
    $("#textBirthday").val(now);

    /*
        使用Form Validate JS做表单验证和表单提交
    */
    $("#btnSubmit").click(function () {
        var  userName = $("#txtUserName").val();
        if (!userName) { $("#msg").text("请输入用户名！"); return; }

        var nikeName = $("#txtNikeName").val();
        if (!nikeName) { $("#msg").text("请输入昵称！"); return; }

        var passWord = $("#txtPassWord").val();
        if (!passWord) { $("#msg").text("请输入密  码！"); return; }
        var rePassWord = $("#txtRePassWord").val();
        if (!rePassWord) { $("#msg").text("请输入确认密码！"); return; }

        var sex = $("input[name='sex']:checked").val();
        var age = $("#selAge").val();
        var email = $("#txtEmail").val();
        if (!email) { $("#msg").text("请输入邮  箱！"); return; }

        $("#myForm").submit();

    });

    //退出
    $("#btnExit").click(function () { window.close(); });
});