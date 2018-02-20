function MenuJS() {
    this.init();
    this.event();
}

MenuJS.prototype = {
    init: function () {
        var that = this;
        lbGlobal.LoginInfo();
        that.show_user_menu();
        that.show_function_menu();
    },
    event: function () {
        $("#menu_login_usermenu_logout").click(function () {
            lbGlobal.Logout();
        });
    },
    show_user_menu: function () {
        var userMenu = $("#menu_login_usermenu_ul");
        userMenu.empty();
        userMenu.append('<li ><a id="menu_login_usermenu_logout" href="#" data-rel="back">退出登录</a></li>');

        $("#main").page();
        userMenu.listview("refresh");
    }, 

    show_function_menu: function () {
        var functionMenu = $("#menu_function_menu");
        functionMenu.empty();

        functionMenu.append('<li><a rel="external" onclick="location.href =\'RepairBillM.html?billType=1\' ">' + '新增报修单' + '</a></li>');
        functionMenu.append('<li><a rel="external" href="' + '/Page/RepairListM.html?listType=1' + '">' + '查看报修单' + '</a></li>');
        functionMenu.append('<li><a rel="external" href="' + '/Page/RepairListM.html?listType=3' + '">' + '我的待处理' + '</a></li>');
        functionMenu.append('<li><a rel="external" href="' + '/Page/RepairListM.html?listType=4' + '">' + '我的已处理' + '</a></li>');
        functionMenu.append('<li><a rel="external" href="' + '/Page/RepairListM.html?listType=5' + '">' + '我的报修单' + '</a></li>');

        $("#main").page();
        functionMenu.listview("refresh");
    }
};