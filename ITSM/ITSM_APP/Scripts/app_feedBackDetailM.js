var billId = 0;
var repairApplyBillsUri = '../api/FeedBacks/';
var imageFile = "";
var statusId = 0;

var ViewModel = function () {
    var self = this;
    self.detail = ko.observable();
    self.error = ko.observable();
    self.onwayFlows = ko.observableArray();

    self.newOnwayFlow = {
        CurrentDealer: ko.observable(),
        NextDealer: ko.observable(),
        DealDate: ko.observable(),
        DealNote: ko.observable(),
        RepairApplyBillId: ko.observable()
    };

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
        var time = year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second + "." + millisecond;

        self.newOnwayFlow.DealDate = time;
        $('#inputDealDate').val(time);
    }

    //获取页面参数
    function getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

    function getDetail() {        
        billId = getQueryString('id');
        ajaxHelper(repairApplyBillsUri + billId, 'GET').done(function (data) {
            $('#spanName').text(data.Name);
            $('#spanCompany').text(data.Company);
            $('#spanEMail').text(data.EMail);
            $('#spanMobile').text(data.Mobile);
            $('#spanContent').text(data.Content);          
            $('#spanCreateTime').text(data.CreateTime.replace('T', ' '));
           
            imageFile = data.FilePath;

            if (imageFile != "") {
                $("#img0").attr("src", "/Images/" + imageFile);
                $("#aImg").attr("href", "/Images/" + imageFile);
            }

            //if (vedioFile != "" && vedioFile != null) {
            //    $("#linkFile").attr("href", "/Images/" + vedioFile);
            //    $("#spanVedio").show();
            //}
            ////$('#btnSatisfaction').attr("display", data[0].StatusId == 2 ? "normal" : "none")
            //if (data[0].StatusId != 2) {
            //    $('#btnSatisfaction').hide();
            //}
            //else {
            //    $('#btnDistribute').hide();
            //    $('#btnDeal').hide();
            //}

            self.detail(data);
        });
    }

    //建立一個可存取到該file的url
    function getObjectURL(file) {
        var url = null;
        if (window.createObjectURL != undefined) { // basic
            url = window.createObjectURL(file);
        } else if (window.URL != undefined) { // mozilla(firefox)
            url = window.URL.createObjectURL(file);
        } else if (window.webkitURL != undefined) { // webkit or chrome
            url = window.webkitURL.createObjectURL(file);
        }
        return url;
    }    

    getSystemTime();
    getDetail();
};

ko.applyBindings(new ViewModel);


