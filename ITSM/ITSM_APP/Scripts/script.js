function Loading() {

}
Loading.prototype = {
    Show: function () {
        var $this = $(this),
            theme = $this.jqmData("theme") || $.mobile.loader.prototype.options.theme,
            msgText = $this.jqmData("msgtext") || $.mobile.loader.prototype.options.text,
            textVisible = $this.jqmData("textvisible") || $.mobile.loader.prototype.options.textVisible,
            textonly = !!$this.jqmData("textonly");
        html = $this.jqmData("html") || "";
        $.mobile.loading("show", {
            text: msgText,
            textVisible: textVisible,
            theme: theme,
            textonly: textonly,
            html: html
        });
    },
    Hide: function () {
        $.mobile.loading("hide");
    }
};

function LBGlobal() {

}
LBGlobal.prototype = {
    LoginInfoCallBack: function (callback) {
        $.ajax({
            type: "post",
            url: "/User/_LoginUserInfo",
            dataType: "json",
            success: function (data) {
                callback(data);
            }
        });
    },
    LoginInfo: function () {
        $.ajax({
            type: "post",
            url: "/User/_LoginUserInfo",
            dataType: "json",
            success: function (data) {
                var loginInfo = $("#menu_login_info");

                if (loginInfo != null) {
                    loginInfo.empty();
                    var loginInfoHtml = "<li>";
                    if (data.IsLogin) {
                        loginInfoHtml += '<a href="#menu_login_usermenu" data-rel="popup" data-position-to="origin" data-transition="pop" >';
                        if (data.IconPath == null || data.IconPath == '') {
                            loginInfoHtml += '<img src="/Src/images/default_head.png" height="80" width="80" />';
                        } else {
                            loginInfoHtml += '<img src="' + data.IconPath + '" height="80" width="80" />';
                        }
                        loginInfoHtml += '<h2>' + data.Name + '</h2>';
                        loginInfoHtml += '<p>' + data.UserName + '</p>';
                        loginInfoHtml += '</a>';
                    } else {
                        loginInfoHtml += '<a href="#page_login" data-transition="slideup">';
                        loginInfoHtml += '<img src="/Src/images/default_head.png" height="80" width="80" />';
                        loginInfoHtml += '<h2>登录 / 注册</h2>';
                        loginInfoHtml += '<p>Please sign in</p>';
                        loginInfoHtml += '</a>';
                    }
                    loginInfoHtml += "</li>";
                    loginInfo.append(loginInfoHtml);
                    $("#main").page();
                    loginInfo.listview("refresh");

                    //------------------------------------------------------------------------

                    var userMenu = $("#menu_login_usermenu_ul");
                    userMenu.empty();
                    userMenu.append('<li ><a id="menu_login_usermenu_logout" href="#" data-rel="back">退出登录</a></li>');

                    $("#main").page();
                    userMenu.listview("refresh");

                    $("#menu_login_usermenu_logout").click(function () {
                        lbGlobal.Logout();
                    });
                }
            }
        });
    },
    Logout: function () {
        var that = this;
        $.ajax({
            type: "post",
            url: "/User/_Logout",
            dataType: "json",
            success: function (data) {
                if (data) {
                    that.LoginInfo();
                }
            }
        });
    },
    IsWeixinBrowse: function () {
        var ua = navigator.userAgent.toLowerCase();
        if (ua.match(/MicroMessenger/i) == "micromessenger") {
            return true;
        } else {
            return false;
        }
    },
    IsDebug: function () {
        return true;
    }
};
var lbGlobal = new LBGlobal();
var loading = new Loading();




