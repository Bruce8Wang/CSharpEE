function RegisterJS() {
    this.init();
    this.event();
}
function RegisterJS(registerSuccessCallback) {
    this.init(registerSuccessCallback);
    this.event();
}
RegisterJS.prototype = {
    init: function (registerSuccessCallback) {
        var that = this;
        jQuery.validator.addMethod("surepassword",
        function (value, element, params) {
            return value == $("#register_password").val();
        }, "两次输入的密码不一致");

        //表单验证
        $("#register_form").validate({
            rules: {
                name: { required: true, email: true },
                password: { required: true },
                surepassword: { surepassword: true }
            },
            messages: {
                name: { required: "请输入用户邮箱", email: "请输入正确的用户邮箱地址" },
                password: { required: "请输入密码" },
            },
            submitHandler: function (form) {
                that.form_submit(form, registerSuccessCallback);
            }
        });
    },
    event: function () {
        $("#register_submit").click(function () {
            $("#register_form").submit();
        });
    },
    form_submit: function (form, callback) {
        var that = this;

        that.form_active(false);

        var _name = $("#register_name").val();
        var _password = $("#register_password").val();
        var _surepassword = $("#register_surepassword").val();
        $.ajax({
            type: "post",
            url: "/User/_Register",
            dataType: "json",
            alert: form,
            data: { name: _name, password: _password, surepassword: _surepassword },
            success: function (data) {
                if (data.IsSuccess) {
                    lbGlobal.LoginInfo();
                    if (callback != null) {
                        callback();
                    } else {
                        $("#register_cancel_btn").click();
                    }
                } else {
                    $("#register_result_context").empty();
                    for (var i = 0; i < data.Status.length; i++) {
                        switch (data.Status[i]) {
                            case 0:
                                $("#register_result_context").append('<div><strong>注册异常</strong></div>');
                                break;
                            case 2:
                                $("#register_result_context").append('<div><strong>创建用户失败</strong></div>');
                                break;
                            case 3:
                                $("#register_result_context").append('<div><strong>用户名已存在</strong></div>');
                                break;
                            case 4:
                                $("#register_result_context").append('<div><strong>用户名为空</strong></div>');
                                break;
                            case 5:
                                $("#register_result_context").append('<div><strong>密码为空</strong></div>');
                                break;
                            case 6:
                                $("#register_result_context").append('<div><strong>确认密码为空</strong></div>');
                                break;
                            case 7:
                                $("#register_result_context").append('<div><strong>两次输入的密码不一致</strong></div>');
                                break;
                            default:
                        }
                    }


                    $("#register_popup").popup("open");
                }
                that.form_active(true);
            }
        });
    },
    form_active: function (isAction) {
        var loading = new Loading();
        if (isAction) {
            $("#register_name").attr("disabled", false);
            $("#register_password").attr("disabled", false);
            $("#register_submit").attr("disabled", false);
            $("#register_submit").html("注册");
            loading.Hide();
        } else {
            $("#register_name").attr("disabled", true);
            $("#register_password").attr("disabled", true);
            $("#register_submit").attr("disabled", true);
            $("#register_submit").html("注册中...");
            loading.Show();
        }
    }
}