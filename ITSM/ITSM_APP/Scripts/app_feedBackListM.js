var repairApplyBillsUri = '../api/FeedBacks';
var pagesize = 10;
var pageIndex = 1;
var recordCount = 0;

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function getSumRepairBills() {
    var arr = new Array;

    var faultType = "";
    if (localStorage.faultType > 0) faultType = localStorage.faultType;
    var status = "";
    localStorage.status;
    if (localStorage.status > 0) status = localStorage.status;

    localStorage.faultType = "";
    localStorage.status = "";
    //var keysearch = localStorage.keysearch;


    if (faultType != "") {
        if (localStorage.filter.length > 0) localStorage.filter = localStorage.filter + " and ";
        localStorage.filter = localStorage.filter + "FaultTypeId eq " + faultType;
    }
    if (status != "") {
        if (localStorage.filter.length > 0) localStorage.filter = localStorage.filter + " and ";
        localStorage.filter = localStorage.filter + "StatusId eq " + status;
    }
    //if (keysearch != "") {
    //    if (localStorage.filter.length > 0) localStorage.filter = localStorage.filter + " and ";
    //    localStorage.filter = localStorage.filter + "indexof(Note, " + keysearch + ") ge 0 ";
    //}

    //localStorage.filter = "FaultTypeId eq " + faultType + " and StatusId eq " + status + " and indexof(Note, " + key + ") ge 0 ";

    var filter = localStorage.filter;
    arr.push(repairApplyBillsUri);
    if (filter.length > 0) {

        arr.push('?$filter=');
        arr.push(filter);
    }
    var uri = arr.join("");
    uri = encodeURI(uri);
    ajaxHelper(uri, 'GET').done(function (data) {
        recordCount = data.length;
    });
}

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

function RepairListJS() {
    this.init();
    this.event();
}
RepairListJS.prototype = {
    init: function () {
        var that = this;
        that.GetRepairList();
    },
    event: function () {
        var that = this;
        $("#bs_sch_more").click(function () {
            pageIndex = $("#search_pi").val();
            $("#search_pi").val(parseInt(pageIndex) + 1);
            that.GetRepairList();
        });
    },
    GetRepairList: function () {
        var that = this;
        loading.Show();

        var jc = $("#search_jc").val();
        var c1 = $("#search_c1").val();
        var c2 = $("#search_c2").val();
        var ky = $("#search_ky").val();
        var pi = $("#search_pi").val();
        var ps = $("#search_ps").val();

        var arr = new Array;
        var filter = localStorage.filter;
        arr.push(repairApplyBillsUri);
        if (filter.length > 0) {
            arr.push('?$filter=');
            arr.push(filter);
            arr.push('&$top=');
        }
        else {
            arr.push('?$top=');
        }
        arr.push(pi * pagesize);

        ////arr.push('&$skip=');
        ////arr.push(pagesize * (pageIndex - 1));
        arr.push('&$orderby=Id desc')

        //arr.push(pagesize * pi);

        var uri = arr.join("");
        uri = encodeURI(uri);
        localStorage.filter = "";

        $.ajax({
            type: "Get",
            url: uri,
            contentType: 'application/json',
            data: "jc=" + jc + "&c1=" + c1 + "&c2=" + c2 + "&ky=" + ky + "&pi=" + pi + "&ps=" + ps,
            dataType: "json",
            success: function (data) {
                that.ShowRepairList(data);
                loading.Hide();
            }
        });
    },
    ShowRepairList: function (data) {
        var that = this;
        var repairListElement = $("#bs_schlist_ret");
        for (var i = 0; i < data.length; i++) {

            //if (data[i].Content.length > 100) {
            //    var TitleShort = data[i].Content.substr(0, 40);
            //    TitleShort = TitleShort + "...";
            //} else {
            //    TitleShort = data[i].Title;
            //}

            var html =
                    '<li>' +
                    '<a href="FeedBackDetailM.html?id=' + data[i].Id + '" data-ajax="false">' +
                    '<h1><div>反馈人：' + data[i].Name + '</div></h1>' +
                    '<h2><div>公司：' + '<span style="color:blue">' + data[i].Company + '</span></div></h2>' +
                    '<h2><div>邮箱：' + '<span style="color:blue">' + data[i].EMail + '</span></div></h2>' +
                    '<h2><div>手机号：' + '<span style="color:blue">' + data[i].Mobile + '</span></div></h2>' +
                    '<h2><div>反馈内容：<span style="color:blue">' + data[i].Content + '</span></div></h2>' +
                    '<h3>' + data[i].CreateTime.replace('T', ' ') + '</h3>' +
                    '</a>' +
                    '</li>';
            repairListElement.append(html);
        }

        //if (data.length >= pagesize) {
        if (data.length < recordCount) {
            that.IsShowMore(true);
        } else {
            that.IsShowMore(false);
        }

        if (repairListElement.children("li").length == 0) {
            that.IsShowNoResult(true);
        } else {
            that.IsShowNoResult(false);
        }
    },
    IsShowMore: function (isshow) {
        var morebtn = $("#bs_sch_more");
        if (isshow) {
            morebtn.show();
        } else {
            morebtn.hide();
        }
    },
    IsShowNoResult: function (isshow) {
        var morebtn = $("#no_result");
        if (isshow) {
            morebtn.show();
        } else {
            morebtn.hide();
        }
    }
};

$(function () {
    var listType = getQueryString("listType");
    //普通用户
    if (localStorage.userType == 0) {
        //查询报修单，查询所有的报修单
        if (listType == 1) {
            if (localStorage.where == undefined)
                localStorage.filter = '';
            else
                localStorage.filter = localStorage.where;
        }
            //我的待处理，过滤出已处理完成，且满意度未填写的 
        else if (listType == 3) {
            $('#spanDisplay').text('我的待处理');
            localStorage.filter = 'NextEmployee eq \'' + localStorage.userName + '\' and StatusId eq 2 and SatisfactionLevelId eq 1';
        }
            //我的报修单
        else if (listType == 5) {
            $('#spanDisplay').text('我的报修单');
            localStorage.filter = 'ApplyEmployee eq \'' + localStorage.userName + '\'';
        }
    }

        //管理账号
    else {
        //查询报修单，查询所有的报修单
        if (listType == 1) {
            if (localStorage.where == undefined)
                localStorage.filter = '';
            else
                localStorage.filter = localStorage.where;
        }
            //我的待处理，过滤出状态为处理中的
        else if (listType == 3) {
            $('#spanDisplay').text('我的待处理');
            localStorage.filter = 'NextEmployee eq \'' + localStorage.userName + '\' and StatusId eq 1 ';
        }
            //我的已处理，过滤出状态为已完成的
        else if (listType == 4) {
            $('#spanDisplay').text('我的已处理');
            localStorage.filter = 'BXDealEmployee eq \'' + localStorage.userName + '\' and StatusId eq 2 ';
        }
            //我的报修单
        else if (listType == 5) {
            $('#spanDisplay').text('我的报修单');
            localStorage.filter = 'ApplyEmployee eq \'' + localStorage.userName + '\'';
        }
    }

    getSumRepairBills();
    var repairListJs = new RepairListJS();

});

