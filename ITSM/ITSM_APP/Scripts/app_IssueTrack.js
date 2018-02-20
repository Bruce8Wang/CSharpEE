var url = '/api/IssueTracks';
var billNo, currentUser, currentDept, currentTime;

function ajaxHelper(uri, method, data) {
    return $.ajax({
        type: method,
        url: uri,
        async: false,
        dataType: 'json',
        contentType: 'application/json',
        //xhrFields: {
        //    withCredentials: true
        //},
        data: data ? JSON.stringify(data) : null
    }).fail(function (jqXHR, textStatus, errorThrown) {

    });
}


function getSystemTime() {
    var now = new Date();
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    if (month < 10)
        month = '0' + month.toString();
    var date = now.getDate();
    if (date < 10)
        date = '0' + date.toString();
    var hour = now.getHours();
    if (hour < 10)
        hour = '0' + hour.toString();
    var minute = now.getMinutes();
    if (minute < 10)
        minute = '0' + minute
    var second = now.getSeconds();
    if (second < 10)
        second = '0' + second
    var millisecond = now.getMilliseconds();
    currentTime = year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second + "." + millisecond;
    billNo = year.toString() + month.toString() + date.toString() + hour.toString() + minute.toString() + second.toString() + millisecond.toString();
    currentUser = localStorage.userName;
    currentDept = localStorage.deptName;
}

//获取页面参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

var Id = getQueryString("Id");

//初始化界面
function SetUI() {
    getSystemTime();
    getData('center');

    if (Id != null) {
        var Url = url + '?$filter=Id eq ' + Id;
        ajaxHelper(Url, 'GET').done(function (data) {
            if (data.length > 0) {
                $("#inputBillNo").val(data[0].BillNo);
                $("#selPriority").val(data[0].Priority);
                $("#selModule").val(data[0].Module);
                $("#selIssueType").val(data[0].IssueType);
                $("#inputSubmitter").val(data[0].Submitter);
                $("#selSubmitDept").val(data[0].SubmitDept);
                $("#inputFeedbackDate").val(data[0].FeedbackDate.replace('T', ' '));
                $("#inputHopeTime").val(data[0].HopeTime.replace('T', ' '));
                $('#selStatus').val(data[0].Status);
                $('#inputDealTime').val(data[0].DealTime.replace('T', ' '));
                $('#inputDesciption').val(data[0].Description);
                $('#inputSolution').val(data[0].Solution);
            }
        });
    }
    else {
        $('#inputBillNo').val(billNo);
        $('#inputSubmitter').val(currentUser);
        $('#selSubmitDept').val(currentDept);
        $('#inputFeedbackDate').val(currentTime);
    }


}

function getData(type) {
    var objList = '';
    switch (type) {
        case 'center':
            var data = [
            { "FD_ID": "1490d5fad0867db5c743703413790ce1", "FD_NAME": "公司领导" },
            { "FD_ID": "1490d5fad2875e959ab47bc4123ab0c3", "FD_NAME": "证券中心", },
            { "FD_ID": "1490d5fad66bbb1c3a932704ef6a64a4", "FD_NAME": "投融资中心" },
            { "FD_ID": "1490d5fad0836f9651e0ede46bb9fa9c", "FD_NAME": "设计院" },
            { "FD_ID": "1490d5fad08a1ac18afc64245ce80891", "FD_NAME": "总裁办公室" },
            { "FD_ID": "1490d5fad08c35cae32cc1d4006b3c8f", "FD_NAME": "总工办公室" },
            { "FD_ID": "14de57c007802c8be8d72aa452e81531", "FD_NAME": "品牌文化办公室" },
            { "FD_ID": "1490d5fad0811f1377d8c334458be5e8", "FD_NAME": "市场运营中心" },
            { "FD_ID": "1490d5fad188e335eb9ffec41bd90526", "FD_NAME": "供应链事业部" },
            { "FD_ID": "1490d5fad18dde6dff5cc4945eeb14be", "FD_NAME": "苗圃事业部" },
            { "FD_ID": "1490d5fad66fbd0ee59175340eb94ca7", "FD_NAME": "采购中心" },
            { "FD_ID": "1490d5fad28cae3fd3086a1466288e1c", "FD_NAME": "研发中心" },
            { "FD_ID": "1490d5fad3763d4a6868a1a49068c28a", "FD_NAME": "工程中心" },
            { "FD_ID": "1490d5fad76de546810130b47e28e5ee", "FD_NAME": "生态事业部" },
            { "FD_ID": "14d7592c161663e772d715444b4acf74", "FD_NAME": "生态旅游事业部" },
            { "FD_ID": "14e23ac8ffa4cd2c37b090745b8924ba", "FD_NAME": "生态金融发展中心" },
            { "FD_ID": "1490d5fad182733f4ef25524cec9848d", "FD_NAME": "预算管理中心" },
            { "FD_ID": "1490d5fad18dd04847327454a918c29b", "FD_NAME": "财务中心" },
            { "FD_ID": "1490d5fad37de0145520a1946bcafb6f", "FD_NAME": "审计监察中心" },
            { "FD_ID": "14eac9f5bf89a79c95a6d7c4984abfd0", "FD_NAME": "法务中心" },
            { "FD_ID": "14e4ac68697806795f57a084b9b88a52", "FD_NAME": "家庭园艺事业部" },
            { "FD_ID": "1490d5fad664690902fed844e7a9d600", "FD_NAME": "人力资源中心" },
            { "FD_ID": "1490d5fad08edcb4b2aff8c45d3b6afc", "FD_NAME": "流程与信息中心" },
            { "FD_ID": "1490d5fad37597ddcfd17134f61bde27", "FD_NAME": "北京区域中心" },
            { "FD_ID": "15090b523018dd7e50b728e484abc251", "FD_NAME": "海南区域中心" },
            { "FD_ID": "150af9b4b9ce4f171d8ad734551a805b", "FD_NAME": "珠海区域中心" },
            { "FD_ID": "1490d5fad183d285b165b4b402e804de", "FD_NAME": "行政中心" }
            ];
            objList = document.getElementById("selSubmitDept");
            objList.innerHTML = "";
            if (data.length > 0) {
                $("#selSubmitDept").append(generateHtmlOption(data, type));
            }
            break;

    }
}

//生成下拉框的HTML
function generateHtmlOption(dataArr, type) {
    var option = '';
    var arr = []; //定义一个数组
    var length = 0; //dataArr.length;
    switch (type) {
        case 'center':
            $.each(dataArr, function (key, val) {
                option = option + '<option value="' + val.FD_NAME + '">' + val.FD_NAME + '</option>';
            });
            break;
    }
    return option;
}


$(function () {
    SetUI();

    $("form").submit(function (e) {
        if (Id == null) {
            var info = {
                BillNo: billNo,
                Priority: $('#selPriority').val(),
                Module: $("#selModule").val(),
                IssueType: $('#selIssueType').val(),
                Submitter: $('#inputSubmitter').val(),
                SubmitDept: $('#selSubmitDept').val(),
                FeedbackDate: $('#inputFeedbackDate').val(),
                HopeTime: $('#inputHopeTime').val(),
                DealTime: $('#inputDealTime').val(),
                Status: $('#selStatus').val(),
                Description: $('#inputDesciption').val(),
                Solution: $('#inputSolution').val(),
                ImagePath: ''
            };
            ajaxHelper(url, 'POST', info).done(function (data, status) {
                if (status == 'success') {
                    alert('提交成功!');
                }
            });
        }
        else {
            var info = {
                Id: Id,
                BillNo: billNo,
                Priority: $('#selPriority').val(),
                Module: $("#selModule").val(),
                IssueType: $('#selIssueType').val(),
                Submitter: $('#inputSubmitter').val(),
                SubmitDept: $('#selSubmitDept').val(),
                FeedbackDate: $('#inputFeedbackDate').val(),
                HopeTime: $('#inputHopeTime').val(),
                DealTime: $('#inputDealTime').val(),
                Status: $('#selStatus').val(),
                Description: $('#inputDesciption').val(),
                Solution: $('#inputSolution').val(),
                ImagePath: ''
            };
            ajaxHelper(url + '/' + Id, 'PUT', info).done(function (data, status) {
                if (status == 'nocontent') {
                    alert('提交成功!');
                    location.href = '/Page/IssueTrackList.html';
                }
            });
        }

    });

});

