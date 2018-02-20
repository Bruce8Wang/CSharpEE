var pagesize = 10; //页面大小
var pageCount = 0; //页数
var pageIndex = 1; //页索引
var listType = 0;

function ajaxHelper(uri, method, data) {
    return $.ajax({
        type: method,
        url: uri,
        dataType: 'json',
        cache: false,
        contentType: 'application/json',
        xhrFields: {
            withCredentials: true
        },
        data: data ? JSON.stringify(data) : null
    }).fail(function (jqXHR, textStatus, errorThrown) {

    });
}

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function getFilter() {
    listType = getQueryString("listType");
    //普通用户
    if (sessionStorage.userType == 0) {
        $('.deal').hide();
        //查询报修单，查询所有的报修单
        if (listType == 1) {
            $('#aAdd').show();
            if (sessionStorage.where == undefined)
                sessionStorage.filter = '';
            else
                sessionStorage.filter = sessionStorage.where;
        }
            //我的待处理，过滤出已处理完成，且满意度未填写的 
        else if (listType == 3) {
            $('#spanDisplay').text('我的待处理');
            sessionStorage.filter = 'NextEmployee eq \'' + sessionStorage.userName + '\' and StatusId eq 2 and SatisfactionLevelId eq 1';
        }
            //我的报修单
        else if (listType == 5) {
            $('#spanDisplay').text('我的报修单');
            sessionStorage.filter = 'ApplyEmployee eq \'' + sessionStorage.userName + '\'';
        }
    }

        //管理账号
    else {
        //查询报修单，查询所有的报修单
        if (listType == 1) {
            if (sessionStorage.where == undefined)
                sessionStorage.filter = '';
            else
                sessionStorage.filter = sessionStorage.where;
        }
            //我的待处理，过滤出状态为处理中的
        else if (listType == 3) {
            $('#spanDisplay').text('我的待处理');
            sessionStorage.filter = 'NextEmployee eq \'' + sessionStorage.userName + '\' and StatusId eq 1 ';
        }
            //我的已处理，过滤出状态为已完成的
        else if (listType == 4) {
            $('#spanDisplay').text('我的已处理');
            $('#aDeal').hide();
            $('#aDistribute').hide();
            $('#aSatisfaction').hide();
            sessionStorage.filter = 'BXDealEmployee eq \'' + sessionStorage.userName + '\' and StatusId eq 2 ';
        }
            //我的报修单
        else if (listType == 5) {
            $('#spanDisplay').text('我的报修单');
            $('#aDeal').hide();
            $('#aEdit').hide();
            $('#aDistribute').hide();
            sessionStorage.filter = 'ApplyEmployee eq \'' + sessionStorage.userName + '\'';
        }
    }
}

function getDataList() {
    getFilter();
    getRepairApplyBills();
    getRepairApplyBillsByPage();
}

function getRepairApplyBills() {
    var arr = new Array;
    var filter = sessionStorage.filter;
    arr.push(uri);
    if (filter.length > 0) {
        arr.push('?$filter=');
        arr.push(filter);
    }
    var uri = arr.join("");
    uri = encodeURI(uri);
    ajaxHelper(uri, 'GET').done(function (data) {
        sessionStorage.recordCount = data.length;
        sessionStorage.pageCount = Math.ceil(sessionStorage.recordCount / pagesize);
    });
}


//按页码查询
function getRepairApplyBillsByPage() {
    var uri = '../api/repairApplyBills';
    var arr = new Array;
    var filter = sessionStorage.filter;
    arr.push(uri);
    if (filter.length > 0) {
        arr.push('?$filter=');
        arr.push(filter);
        arr.push('&$top=');
    }
    else {
        arr.push('?$top=');
    }
    arr.push(pagesize);
    arr.push('&$skip=');
    arr.push(pagesize * (pageIndex - 1));
    arr.push('&$orderby=Id desc')
    var uri = arr.join("");
    uri = encodeURI(uri);
    sessionStorage.filter = "";

    var objListView = document.getElementById("ulRepairBillList");
    objListView.innerHTML = "";    //清空ListView原本的内容

    ajaxHelper(uri, 'GET').done(function (data) {
        if (data.length > 0) {
            $("#ulRepairBillList").append(generateHtmlDataList(data, 'repairBillList'));
        }
        else {
            $("#ulRepairBillList").append(' <li data-role="fieldcontain"><label style="color:red;font-size:14px;">暂未查询到数据！</label> </li>');
        }
    });
}

function home() {
    location.href = "/IndexMobile.html";
};

//上一页按钮click事件
function previous() {
    if (pageIndex != 1) {
        pageIndex--;
        $("#lblCurrent").text(pageIndex);
    }
    getRepairApplyBillsByPage();
};
//下一页按钮click事件
function next() {
    var pageCount = $("#lblPageCount").text();
    if (pageIndex != sessionStorage.pageCount) {
        pageIndex++;
        $("#lblCurrent").text(pageIndex);

    }
    else {
        $("#lblCurrent").addClass('disable');
    }
    getRepairApplyBillsByPage();
};

//生成DataList的HTML
function generateHtmlDataList(dataArr, type) {
    var li = "";
    switch (type) {
        case 'repairBillList':
            $.each(dataArr, function (key, val) {

                //li = li + ' <li> <a id="linkRepairBill" href="RepairApplyBillMobile.html?billType=5&id=' + val.Id + '">'

                li = li + ' <li> <a id="linkRepairBill" href="#" onclick="location.href =\'RepairApplyBillMobile.html?billType=5&id=' + val.Id + '\' ">'
                    + '<h2 id="BillNo">提单号：' + val.BillNo + '</h2>'
                    + '<p>'
                    + '    <span>主题：' + val.Title + '</span><br />'
                    + '    <span>报修时间：' + val.BXDate.replace('T', ' ') + '</span><br />'
                    + '    <span>当前处理人：' + val.NextEmployee + '</span><br />'
                    + '    <span>处理状态：' + val.StatusName + '</span>'
                    + '</p></a></li>';

            });
            break;
        default:
            break;
    }
    return li;
}