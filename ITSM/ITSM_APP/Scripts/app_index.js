var loginUri = 'api/Login/';
var superUserUri = '';
localStorage.userName = "";
localStorage.deptName = "";
localStorage.email = "";
localStorage.mobile = "";
localStorage.userType = 1;

//if (getExplorerType() != 'chrome') {
//    alert('请使用Chrome浏览器！');
//    window.opener = null;
//    window.open('', '_self');
//    window.close();
//}


function ajaxHelper(uri, method, data) {
    return jQuery.ajax({
        type: method,
        url: uri,
        async: false,
        dataType: 'json',
        contentType: 'application/json',
        xhrFields: {
            withCredentials: true
        },
        data: data ? JSON.stringify(data) : null
    }).fail(function (jqXHR, textStatus, errorThrown) {
    });
}

//获取浏览器类型
function getExplorerType() {

    var ua = navigator.userAgent.toLowerCase();

    if (ua.indexOf("msie") > 0) {
        return "ie";
    }
    if (isFirefox = ua.indexOf("firefox") > 0) {
        return "firefox";
    }
    if (isFirefox = ua.indexOf("chrome") > 0) {
        return "chrome";
    }
    if (isSafari = ua.indexOf("safari") > 0) {
        return "safari";
    }
    if (isCamino = ua.indexOf("camino") > 0) {
        return "camino";
    }
    if (isMozilla = ua.indexOf("gecko") > 0) {
        return "gecko";
    }
}

//获取当前登陆账号的姓名、部门、邮箱
ajaxHelper(loginUri, 'GET').done(function (data, status) {
    if (status == 'success') {
        $('.pice').show();
        $('#userName').text(data.UserName);
        localStorage.userName = data.UserName;
        $('#deptName').text(data.DeptName);
        localStorage.deptName = data.DeptName;
        localStorage.email = data.EMail;
        localStorage.mobile = data.Mobile;

        //判断当前登陆用户是否为管理账号
        if (data.UserType == 0) {
            localStorage.userType = 0;
            $('.admin').hide();
        }
        else {
            localStorage.userType = 1;
            $('.admin').show();
        }

        if (data.UserName == "杨勇杰" || data.UserName == "张帆" || localStorage.userName == '廖贵健' || data.UserName == "张毅") {
            $('.purchase').show();
        }

        if (data.UserName == "杨勇杰" || data.UserName == "陈阳春" || localStorage.userName == '廖贵健') {
            $('.feedback').show();
        }


    }
    else {
        location.href = "Login.html";
    }
});




//$('#linkAdd').click(function () {
//    location.href = "Page/RepairApplyBillMobile.html?billType=1";
//});

//$('#linkQry').click(function () {
//    location.href = "Page/RepairBillQueryMobile.html?listType=1";
//});

//$('#linkMyPending').click(function () {
//    location.href = "Page/RepairBillQueryMobile.html?listType=3";
//});

//$('#linkMyProcessed').click(function () {
//    location.href = "Page/RepairBillQueryMobile.html?listType=4";
//});

//$('#linkMyBill').click(function () {
//    location.href = "Page/RepairBillQueryMobile.html?listType=5";
//});






