var billId = 0;
var onwayFlowUri = '../api/onwayFlows/';
var repairApplyBillsUri = '../api/repairApplyBills/';
var imageFile = "";
var vedioFile = "";
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
        $('#dealModule').hide();
        billId = getQueryString('id');
        ajaxHelper(repairApplyBillsUri + billId, 'GET').done(function (data) {

            $('#inputDealer').val(data[0].ApplyEmployee);

            self.newOnwayFlow.NextDealer = data[0].ApplyEmployee;
            self.newOnwayFlow.RepairApplyBillId = data[0].Id;
            self.newOnwayFlow.CurrentDealer = localStorage.userName;

            $('#spanTitle').text(data[0].Title);
            $('#spanBillNo').text(data[0].BillNo);
            $('#spanApplyEmployee').text(data[0].ApplyEmployee);
            $('#spanApplyDept').text(data[0].ApplyDept);
            $('#spanBXEmployee').text(data[0].BXEmployee);
            $('#spanBXDept').text(data[0].BXDept);
            $('#spanPhone').text(data[0].Phone);
            $('#spanApplyEmail').text(data[0].EMail);
            $('#spanBXDate').text(data[0].BXDate.replace('T', ' '));
            $('#spanAssetCode').text(data[0].AssetCode);
            $('#spanComputerName').text(data[0].ComputerName);
            $('#spanFaultType').text(data[0].FaultTypeName);
            $('#spanHopeTime').text(data[0].HopeTime.replace('T', ' '));
            $('#spanDealTime').text(data[0].BXDealTime.replace('T', ' ').replace('9999-12-30 00:00:00', ''));
            $('#spanDealEmployee').text(data[0].BXDealEmployee);
            $('#prevOperation').text(data[0].PrevOperation);
            $('#preNote').text(data[0].Note);
            $('#preDealNote').text(data[0].BXDealNote);
            $('#spanStatus').text(data[0].StatusName)
            $('#spanCurDealer').text(data[0].NextEmployee);
            $('#spanSatisfaction').text(data[0].SatisfactionLevelName);
            $('#billId').val(data[0].Id);

            statusId = data[0].StatusId;
            vedioFile = data[0].VedioPath;
            imageFile = data[0].ImagePath;

            //var arrayFile = imageFile.split('|');
            //if (arrayFile.length > 0) {
            //    $("#img0").attr("src", "/Images/" + arrayFile[1]);
            //    $("#aImg").attr("href", "/Images/" + arrayFile[1]);
            //}

            //显示处理方法
            if (statusId == 2) {
                $('.deal').show();
            }

            if (imageFile != "") {
                $("#img0").attr("src", "/Images/" + imageFile);
                $("#aImg").attr("href", "/Images/" + imageFile);
            }

            if (vedioFile != "" && vedioFile != null) {
                $("#linkFile").attr("href", "/Images/" + vedioFile);
                $("#spanVedio").show();
            }
            //$('#btnSatisfaction').attr("display", data[0].StatusId == 2 ? "normal" : "none")
            if (data[0].StatusId != 2) {
                $('#btnSatisfaction').hide();
            }
            else {
                $('#btnDistribute').hide();
                $('#btnDeal').hide();
            }

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

    self.addOnwayFlow = function (formElement) {
        //更新报修单状态
        var repairApplyBill = {
            BillNo: self.detail()[0].BillNo,
            Title: self.detail()[0].Title,
            Note: self.detail()[0].Note,
            FaultTypeId: self.detail()[0].FaultTypeId,
            ApplyEmployee: self.detail()[0].ApplyEmployee,
            ApplyDept: self.detail()[0].ApplyDept,
            BXEmployee: self.detail()[0].BXEmployee,
            BXDealTime: self.detail()[0].BXDealTime,
            HopeTime: self.detail()[0].HopeTime,
            PriorityId: self.detail()[0].PriorityId,
            PrevOperation: self.detail()[0].PrevOperation,
            ImagePath: self.detail()[0].ImagePath,
            NextEmployee: self.detail()[0].NextEmployee,
            BXDealEmployee: localStorage.userName,
            BXDealNote: self.newOnwayFlow.DealNote(),
            NextEmployee: self.detail()[0].BXEmployee,
            BXDept: self.detail()[0].BXDept,
            BXDate: self.detail()[0].BXDate,
            BXDealTime: self.newOnwayFlow.DealDate,
            AssetCode: self.detail()[0].AssetCode,
            Phone: self.detail()[0].Phone,
            EMail: self.detail()[0].EMail,
            ComputerName: self.detail()[0].ComputerName,
            SatisfactionLevelId: self.detail()[0].SatisfactionLevelId,
            StatusId: 2,
            Id: self.detail()[0].Id
        };

        ajaxHelper(repairApplyBillsUri + self.detail()[0].Id, 'PUT', repairApplyBill).done();

        //新增一条在途流程
        var onwayFlow = {
            CurrentDealer: localStorage.userName,
            NextDealer: self.newOnwayFlow.NextDealer,
            DealDate: self.newOnwayFlow.DealDate,
            DealNote: self.newOnwayFlow.DealNote(),
            RepairAppyBillId: self.newOnwayFlow.RepairApplyBillId,
            DealMethodId: 2
        };

        ajaxHelper(onwayFlowUri, 'POST', onwayFlow).done(function (item) {
            //self.onwayFlows.push(item);
        }).success(function () {
            alert('提交成功！');
            window.close();
            location.reload();
        });
    }

    getSystemTime();
    getDetail();
};

function showDealerSelect() {
    window.open('DealerSelect.html?level=1&id=' + billId, null, "dialogWidth:500;dialogHeight:auto;status:no;help:no;center:yes");
}

function showDealModule() {
    $('#dealModule').show();
    $('.whenAddHide').show();
    $('#inputDealNote').focus();
}

function showEdit() {
    location.href = "RepairBillM.html?billType=2&id=" + billId;
}

function showSatisfaction() {
    location.href = "RepairBillM.html?billType=4&id=" + billId;
}


//var objUrl = getObjectURL('file:///E:/Code/ESB/自主研发/ESB/App_Data/c03c66e1-bac4-4767-a969-c32dfd62a6fd.jpg');
//$("#img0").attr("src", objUrl);


ko.applyBindings(new ViewModel);


