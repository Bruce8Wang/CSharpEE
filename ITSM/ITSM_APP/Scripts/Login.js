
function LoginJS() {
    this.init();
    this.event();
}
function LoginJS(loginSuccessCallback) {
    this.init(loginSuccessCallback);
    this.event();
}
LoginJS.prototype = {
    init: function (loginSuccessCallback) {
        var that = this;
        //表单验证
        $("#login_form").validate({
            rules: {
                name: { required: true, email: true },
                password: { required: true }
            },
            messages: {
                name: { required: "请输入用户邮箱", email: "请输入正确的用户邮箱地址" },
                password: { required: "请输入密码" }
            },
            submitHandler: function (form) {
                that.form_submit(form, loginSuccessCallback);
            }
        });
    },
    event: function () {
        $("#login_submit").click(function () {
            $("#login_form").submit();
        });
    },
    form_submit: function (form, callback) {
        var that = this;
        that.form_active(false);

        var _name = $("#login_name").val();
        var _password = $("#login_password").val();
        $.ajax({
            type: "post",
            url: "/User/_Login",
            dataType: "json",
            alert: form,
            data: { name: _name, password: _password },
            success: function (data) {
                if (data.IsSuccess) {
                    lbGlobal.LoginInfo();
                    if (callback != null) {
                        callback();
                    } else {
                        $("#login_cancel_btn").click();
                    }
                } else {
                    $("#login_result_context").empty();
                    for (var i = 0; i < data.Status.length; i++) {
                        switch (data.Status[i]) {
                            case 0:
                                $("#login_result_context").append('<div><strong>登录异常</strong></div>');
                                break;
                            case 2:
                                $("#login_result_context").append('<div><strong>用户名或密码错误</strong></div>');
                                break;
                            case 3:
                                $("#login_result_context").append('<div><strong>用户名为空</strong></div>');
                                break;
                            case 4:
                                $("#login_result_context").append('<div><strong>密码为空</strong></div>');
                                break;
                            default:
                        }
                    }


                    $("#login_popup").popup("open");
                }
                that.form_active(true);
            }
        });
    },
    form_active: function (isAction) {
        var loading = new Loading();
        if (isAction) {
            $("#login_name").attr("disabled", false);
            $("#login_password").attr("disabled", false);
            $("#login_submit").attr("disabled", false);
            $("#login_submit").html("登录");
            loading.Hide();
        } else {
            $("#login_name").attr("disabled", true);
            $("#login_password").attr("disabled", true);
            $("#login_submit").attr("disabled", true);
            $("#login_submit").html("登录中...");
            loading.Show();
        }
    }
}