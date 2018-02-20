var statusId = 0;
sessionStorage.imagePath = "";
sessionStorage.vedioPath = "";
var imageFile = "";
var bIsAdd = 0;
var bIsDeal = 0;

var ViewModel = function () {
    var self = this;
    self.repairApplyBills = ko.observableArray();
    self.curTime = ko.observable();
    self.error = ko.observable();
    self.satisfactionLevels = ko.observableArray();
    self.faultTypes = ko.observableArray();
    self.onwayFlows = ko.observableArray();
    self.prioritys = ko.observableArray();
    self.detail = ko.observable();

    self.newRepairApplyBill = {
        BillNo: ko.observable(),
        Title: ko.observable(),
        Note: ko.observable(),
        FaultType: ko.observable(),
        Status: ko.observable(),
        ApplyEmployee: ko.observable(),
        ApplyDept: ko.observable(),
        BXDate: ko.observable(),
        BXEmployee: ko.observable(),
        NextEmployee: ko.observable(),
        BXDealTime: ko.observable(),
        BXDept: ko.observable(),
        AssetCode: ko.observable(),
        ComputerName: ko.observable(),
        Phone: ko.observable(),
        ApplyEMail: ko.observable(),
        SatisfactionLevel: ko.observable(),
        Priority: ko.observable(),
        PrevOperation: ko.observable(),
        HopeTime: ko.observable(),
        ImagePath: ko.observable(),
        Id: ko.observable()
    };

    self.newOnwayFlow = {
        CurrentDealer: ko.observable(),
        NextDealer: ko.observable(),
        DealDate: ko.observable(),
        DealNote: ko.observable(),
        RepairApplyBillId: ko.observable()
    };
    var repairApplyBillsUri = '../api/repairApplyBills/';
    var satisfactionLevelUri = '../api/satisfactionLevels/';
    var priorityUri = '../api/prioritys/';
    var faultTypeUri = '../api/faultTypes/';
    var onwayFlowUri = '../api/onwayFlows?$filter=';
    var loginUri = '../api/Login/';
    var billNo;

    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            xhrFields: {
                withCredentials: true
            },
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    //判断操作系统32位/64位
    function getOS() {
        var os = "x86";
        var agent = navigator.userAgent.toLowerCase();
        if (agent.indexOf("win64") >= 0 || agent.indexOf("wow64") >= 0) os = "x64";
        return os;

    }

    //获取页面参数
    function getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
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

        billNo = year.toString() + month.toString() + date.toString() + hour.toString() + minute.toString() + second.toString() + millisecond.toString();
        self.newRepairApplyBill.BXDate = time;
        self.newOnwayFlow.DealDate = time;
    }

    //根据不同入口控制页面
    sessionStorage.billType = getQueryString("billType");
    sessionStorage.billId = getQueryString("id");

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

    function IsLowerThanIE9() {
        var ret = false;
        var ua = navigator.userAgent.toLowerCase();
        //if (ua.indexOf("msie 8.0") > 0 || ua.indexOf("msie 9.0") > 0 || ua.indexOf("msie 7.0") > 0 || ua.indexOf("msie 6.0") > 0) {
        //    ret = true;
        //}

        if (ua.indexOf("msie 7.0") > 0 || ua.indexOf("msie 6.0") > 0) {
            ret = true;
        }
        return ret;
    }

    //根据billType设置界面值
    function setUI() {
        if (sessionStorage.userType == 0) {
            $('#aDistribute').hide();
        }

        //billType=1：新增
        //billType=2：修改
        //billType=3：处理
        //billType=4：填写满意度
        //billType=5：查看
        if (sessionStorage.billType == 1) {
            if (IsLowerThanIE9()) {
                if (getOS() == "x86")
                    $("#lbUploadx86").show();
                else $("#lbUploadx64").show();
            }

            $('#spanDisplay').text('新增报修单');         
            $('.Satisfaction').hide();
            $('.admin').hide();
            $('#formOnwayFlow').hide();

            //var billNo = year + month + date + hour + minute + second + millisecond;

            self.newRepairApplyBill.BillNo = billNo;
            self.newRepairApplyBill.ApplyEmployee = sessionStorage.userName;
            self.newRepairApplyBill.BXEmployee = sessionStorage.userName;
            self.newRepairApplyBill.ApplyDept = sessionStorage.deptName;
            self.newRepairApplyBill.BXDept = sessionStorage.deptName;
            self.newRepairApplyBill.ApplyEMail = sessionStorage.email;
            self.newRepairApplyBill.Phone = sessionStorage.mobile;

        }
            //修改
        else if (sessionStorage.billType == 2) {
            $('#spanDisplay').text('修改报修单');
            //$('.whenDealLock').attr("disabled", true);

            $('.add').attr("disabled", true);
            $('.edit').attr("disabled", false);
            $('.Satisfaction').hide();
            $('#formOnwayFlow').hide();

            ajaxHelper('../api/repairApplyBills/' + sessionStorage.billId, 'GET').done(function (data) {
                $("#inputTitle").val(data[0].Title);
                $("#inputBillNo").val(data[0].BillNo);
                $("#selFaultType").val(data[0].FaultTypeId);
                $("#inputApplyEmployee").val(data[0].ApplyEmployee);
                $("#inputBXEmployee").val(data[0].BXEmployee);
                $("#inputApplyDept").val(data[0].ApplyDept);
                $("#inputBXDate").val(data[0].BXDate.toString().replace('T', ' '));
                $("#inputPhone").val(data[0].Phone);
                $("#inputApplyEMail").val(data[0].EMail);
                $("#inputBXDept").val(data[0].BXDept);
                $("#inputAssetCode").val(data[0].AssetCode);
                $("#inputComputerName").val(data[0].ComputerName);
                $("#inputNote").val(data[0].Note);
                $("#inputPriority").val(data[0].PriorityId);
                $("#inputHopeTime").val(data[0].HopeTime.toString().replace('T', ' '));
                $("#inputPrevOperation").val(data[0].PrevOperation);
                $("#inputImagePath").val(data[0].ImagePath);


                self.newRepairApplyBill.Title = data[0].Title;
                self.newRepairApplyBill.BillNo = data[0].BillNo;
                self.newRepairApplyBill.FaultType = data[0].FaultTypeId;
                faultTypeId = data[0].FaultTypeId;
                self.newRepairApplyBill.ApplyEmployee = data[0].ApplyEmployee;
                self.newRepairApplyBill.BXEmployee = data[0].BXEmployee;
                self.newRepairApplyBill.NextEmployee = data[0].NextEmployee;
                self.newRepairApplyBill.BXDealTime = data[0].BXDealTime.toString().replace('T', ' ');
                self.newRepairApplyBill.Priority = data[0].PriorityId;
                priorityId = data[0].PriorityId;
                self.newRepairApplyBill.PrevOperation = data[0].PrevOperation;
                self.newRepairApplyBill.ImagePath = data[0].ImagePath;
                self.newRepairApplyBill.HopeTime = data[0].HopeTime.toString().replace('T', ' ');
                self.newRepairApplyBill.ApplyDept = data[0].ApplyDept;
                self.newRepairApplyBill.BXDate = data[0].BXDate.toString().replace('T', ' ');
                self.newRepairApplyBill.Phone = data[0].Phone;
                self.newRepairApplyBill.ApplyEMail = data[0].EMail;
                self.newRepairApplyBill.BXDept = data[0].BXDept;
                self.newRepairApplyBill.AssetCode = data[0].AssetCode;
                self.newRepairApplyBill.ComputerName = data[0].ComputerName;
                self.newRepairApplyBill.Note = data[0].Note;
                self.newRepairApplyBill.SatisfactionLevel = data[0].SatisfactionLevelId;
                satisfactionLevelId = data[0].SatisfactionLevelId;
                statusId = data[0].StatusId;

                self.newRepairApplyBill.Id = data[0].Id;


                imageFile = data[0].ImagePath;

                if (imageFile.length > 0) {
                    $("#img0").attr("src", "/Images/" + imageFile);
                    $("#aImg").attr("href", "/Images/" + imageFile);
                    $("#img0").show();
                }

                //$("#inputTitle").val(data.Title);
                //$("#inputBillNo").val(data.BillNo);
                //$("#selFaultType").val(data.FaultTypeId);
                //$("#inputApplyEmployee").val(data.ApplyEmployee);
                //$("#inputBXEmployee").val(data.BXEmployee);
                //$("#inputApplyDept").val(data.ApplyDept);
                //$("#inputBXDate").val(data.BXDate.toString().replace('T', ' '));
                //$("#inputPhone").val(data.Phone);
                //$("#inputEMail").val(data.EMail);
                //$("#inputBXDept").val(data.BXDept);
                //$("#inputAssetCode").val(data.AssetCode);
                //$("#inputComputerName").val(data.ComputerName);
                //$("#inputNote").val(data.Note);
                //$("#inputPriority").val(data.PriorityId);
                //$("#inputHopeTime").val(data.HopeTime.toString().replace('T', ' '));
                //$("#inputPrevOperation").val(data.PrevOperation);
                //$("#inputImagePath").val(data.ImagePath);

                //self.newRepairApplyBill.Title = data.Title;
                //self.newRepairApplyBill.BillNo = data.BillNo;
                //self.newRepairApplyBill.FaultType = data.FaultTypeId;
                //self.newRepairApplyBill.ApplyEmployee = data.ApplyEmployee;
                //self.newRepairApplyBill.BXEmployee = data.BXEmployee;
                //self.newRepairApplyBill.NextEmployee = data.NextEmployee;
                //self.newRepairApplyBill.BXDealTime = data.BXDealTime;
                //self.newRepairApplyBill.Priority = data.PriorityId;
                //self.newRepairApplyBill.PrevOperation = data.PrevOperation;
                //self.newRepairApplyBill.ImagePath = data.ImagePath;
                //self.newRepairApplyBill.HopeTime = data.HopeTime;
                //self.newRepairApplyBill.ApplyDept = data.ApplyDept;
                //self.newRepairApplyBill.BXDate = data.BXDate.toString().replace('T', ' ');
                //self.newRepairApplyBill.Phone = data.Phone;
                //self.newRepairApplyBill.ApplyEMail = data.EMail;
                //self.newRepairApplyBill.BXDept = data.BXDept;
                //self.newRepairApplyBill.AssetCode = data.AssetCode;
                //self.newRepairApplyBill.ComputerName = data.ComputerName;
                //self.newRepairApplyBill.Note = data.Note;
                //self.newRepairApplyBill.SatisfactionLevel = data.SatisfactionLevelId;
                //self.newRepairApplyBill.Status = data.StatusId;
                //self.newRepairApplyBill.Id = data.Id;
                //self.detail(data);
            });

        }
            //处理
        else if (sessionStorage.billType == 3) {
            $('#spanDisplay').text('处理报修单');

            $('.add').attr("disabled", true);
            $('.edit').attr("disabled", true);
            $('.deal').attr("disabled", false);
            $('.Satisfaction').hide();
            $('.admin').hide();
            $('#btnRepairBill').hide();
 

            ajaxHelper(repairApplyBillsUri + sessionStorage.billId, 'GET').done(function (data) {
                $("#inputTitle").val(data[0].Title);
                $("#inputBillNo").val(data[0].BillNo);
                $("#selFaultType").val(data[0].FaultTypeId);
                $("#inputApplyEmployee").val(data[0].ApplyEmployee);
                $("#inputBXEmployee").val(data[0].BXEmployee);
                $("#inputApplyDept").val(data[0].ApplyDept);
                $("#inputBXDate").val(data[0].BXDate.toString().replace('T', ' '));
                $("#inputPhone").val(data[0].Phone);
                $("#inputApplyEMail").val(data[0].EMail);
                $("#inputBXDept").val(data[0].BXDept);
                $("#inputAssetCode").val(data[0].AssetCode);
                $("#inputComputerName").val(data[0].ComputerName);
                $("#inputNote").val(data[0].Note);
                $("#inputPriority").val(data[0].PriorityId);
                $("#inputHopeTime").val(data[0].HopeTime.toString().replace('T', ' '));
                $("#inputPrevOperation").val(data[0].PrevOperation);
                $("#inputImagePath").val(data[0].ImagePath);


                self.newRepairApplyBill.Title = data[0].Title;
                self.newRepairApplyBill.BillNo = data[0].BillNo;
                self.newRepairApplyBill.FaultType = data[0].FaultTypeId;
                faultTypeId = data[0].FaultTypeId;
                self.newRepairApplyBill.ApplyEmployee = data[0].ApplyEmployee;
                self.newRepairApplyBill.BXEmployee = data[0].BXEmployee;
                self.newRepairApplyBill.NextEmployee = data[0].NextEmployee;
                self.newRepairApplyBill.BXDealTime = data[0].BXDealTime.toString().replace('T', ' ');
                self.newRepairApplyBill.Priority = data[0].PriorityId;
                priorityId = data[0].PriorityId;
                self.newRepairApplyBill.PrevOperation = data[0].PrevOperation;
                self.newRepairApplyBill.ImagePath = data[0].ImagePath;
                self.newRepairApplyBill.HopeTime = data[0].HopeTime.toString().replace('T', ' ');
                self.newRepairApplyBill.ApplyDept = data[0].ApplyDept;
                self.newRepairApplyBill.BXDate = data[0].BXDate.toString().replace('T', ' ');
                self.newRepairApplyBill.Phone = data[0].Phone;
                self.newRepairApplyBill.ApplyEMail = data[0].EMail;
                self.newRepairApplyBill.BXDept = data[0].BXDept;
                self.newRepairApplyBill.AssetCode = data[0].AssetCode;
                self.newRepairApplyBill.ComputerName = data[0].ComputerName;
                self.newRepairApplyBill.Note = data[0].Note;
                self.newRepairApplyBill.SatisfactionLevel = data[0].SatisfactionLevelId;
                satisfactionLevelId = data[0].SatisfactionLevelId;
                statusId = data[0].StatusId;

                self.newRepairApplyBill.Id = data[0].Id;

                imageFile = data[0].ImagePath;

                if (imageFile.length > 0) {
                    $("#img0").attr("src", "/Images/" + imageFile);
                    $("#aImg").attr("href", "/Images/" + imageFile);
                    $("#img0").show();
                }

                //$("#inputTitle").val(data.Title);
                //$("#inputBillNo").val(data.BillNo);
                //$("#inputFaultType").val(data.FaultTypeId);
                //$("#inputApplyEmployee").val(data.ApplyEmployee);
                //$("#inputBXEmployee").val(data.BXEmployee);
                //$("#inputApplyDept").val(data.ApplyDept);
                //$("#inputBXDate").val(data.BXDate.toString().replace('T', ' '));
                //$("#inputPhone").val(data.Phone);
                //$("#inputApplyEMail").val(data.EMail);
                //$("#inputBXDept").val(data.BXDept);
                //$("#inputAssetCode").val(data.AssetCode);
                //$("#inputComputerName").val(data.ComputerName);
                //$("#inputNote").val(data.Note);
                //$("#inputPriority").val(data.PriorityId);
                //$("#inputHopeTime").val(data.HopeTime.toString().replace('T', ' '));
                //$("#inputPrevOperation").val(data.PrevOperation);
                //$("#inputImagePath").val(data.ImagePath);

                //self.newRepairApplyBill.Title = data.Title;
                //self.newRepairApplyBill.BillNo = data.BillNo;
                //self.newRepairApplyBill.FaultType = data.FaultTypeId;
                //self.newRepairApplyBill.ApplyEmployee = data.ApplyEmployee;
                //self.newRepairApplyBill.BXEmployee = data.BXEmployee;
                //self.newRepairApplyBill.NextEmployee = data.NextEmployee;
                //self.newRepairApplyBill.BXDealTime = data.BXDealTime;
                //self.newRepairApplyBill.Priority = data.PriorityId;
                //self.newRepairApplyBill.PrevOperation = data.PrevOperation;
                //self.newRepairApplyBill.ImagePath = data.ImagePath;
                //self.newRepairApplyBill.HopeTime = data.HopeTime;
                //self.newRepairApplyBill.ApplyDept = data.ApplyDept;
                //self.newRepairApplyBill.BXDate = data.BXDate.toString().replace('T', ' ');
                //self.newRepairApplyBill.Phone = data.Phone;
                //self.newRepairApplyBill.ApplyEMail = data.EMail;
                //self.newRepairApplyBill.BXDept = data.BXDept;
                //self.newRepairApplyBill.AssetCode = data.AssetCode;
                //self.newRepairApplyBill.ComputerName = data.ComputerName;
                //self.newRepairApplyBill.Note = data.Note;
                //self.newRepairApplyBill.SatisfactionLevel = data.SatisfactionLevelId;
                //self.newRepairApplyBill.Status = data.StatusId;
                //self.newRepairApplyBill.Id = data.Id;

                ////self.detail(data);

                $('#inputDealer').val(sessionStorage.userName);
                self.newOnwayFlow.NextDealer = data[0].ApplyEmployee;
                self.newOnwayFlow.RepairApplyBillId = data[0].Id;
                self.newOnwayFlow.CurrentDealer = sessionStorage.userName;
            });
        }

            //填写满意度
        else if (sessionStorage.billType == 4) {
            $('#spanDisplay').text('填写满意度');
            $('.add').attr("disabled", true);
            $('.edit').attr("disabled", true);
            $('.deal').attr("disabled", true);
            $('.admin').hide();   
            $('#formOnwayFlow').hide();

            ajaxHelper(repairApplyBillsUri + sessionStorage.billId, 'GET').done(function (data) {
                $("#inputTitle").val(data[0].Title);
                $("#inputBillNo").val(data[0].BillNo);
                $("#selFaultType").val(data[0].FaultTypeId);
                $("#inputApplyEmployee").val(data[0].ApplyEmployee);
                $("#inputBXEmployee").val(data[0].BXEmployee);
                $("#inputApplyDept").val(data[0].ApplyDept);
                $("#inputBXDate").val(data[0].BXDate.toString().replace('T', ' '));
                $("#inputPhone").val(data[0].Phone);
                $("#inputApplyEMail").val(data[0].EMail);
                $("#inputBXDept").val(data[0].BXDept);
                $("#inputAssetCode").val(data[0].AssetCode);
                $("#inputComputerName").val(data[0].ComputerName);
                $("#inputNote").val(data[0].Note);
                $("#inputPriority").val(data[0].PriorityId);
                $("#inputHopeTime").val(data[0].HopeTime.toString().replace('T', ' '));
                $("#inputPrevOperation").val(data[0].PrevOperation);
                $("#inputImagePath").val(data[0].ImagePath);

                self.newRepairApplyBill.Title = data[0].Title;
                self.newRepairApplyBill.BillNo = data[0].BillNo;
                self.newRepairApplyBill.FaultType = data[0].FaultTypeId;
                faultTypeId = data[0].FaultTypeId;
                self.newRepairApplyBill.ApplyEmployee = data[0].ApplyEmployee;
                self.newRepairApplyBill.BXEmployee = data[0].BXEmployee;
                self.newRepairApplyBill.NextEmployee = data[0].NextEmployee;
                self.newRepairApplyBill.BXDealTime = data[0].BXDealTime.toString().replace('T', ' ');
                self.newRepairApplyBill.Priority = data[0].PriorityId;
                priorityId = data[0].PriorityId;
                self.newRepairApplyBill.PrevOperation = data[0].PrevOperation;
                self.newRepairApplyBill.ImagePath = data[0].ImagePath;
                self.newRepairApplyBill.HopeTime = data[0].HopeTime.toString().replace('T', ' ');
                self.newRepairApplyBill.ApplyDept = data[0].ApplyDept;
                self.newRepairApplyBill.BXDate = data[0].BXDate.toString().replace('T', ' ');
                self.newRepairApplyBill.Phone = data[0].Phone;
                self.newRepairApplyBill.ApplyEMail = data[0].EMail;
                self.newRepairApplyBill.BXDept = data[0].BXDept;
                self.newRepairApplyBill.AssetCode = data[0].AssetCode;
                self.newRepairApplyBill.ComputerName = data[0].ComputerName;
                self.newRepairApplyBill.Note = data[0].Note;
                self.newRepairApplyBill.SatisfactionLevel = data[0].SatisfactionLevelId;
                satisfactionLevelId = data[0].SatisfactionLevelId;
                statusId = data[0].StatusId;

                self.newRepairApplyBill.Id = data[0].Id;

                imageFile = data[0].ImagePath;

                if (imageFile.length > 0) {
                    $("#img0").attr("src", "/Images/" + imageFile);
                    $("#aImg").attr("href", "/Images/" + imageFile);
                    $("#img0").show();
                }

                //$("#inputTitle").val(data.Title);
                //$("#inputBillNo").val(data.BillNo);
                //$("#inputFaultType").val(data.FaultTypeId);
                //$("#inputApplyEmployee").val(data.ApplyEmployee);
                //$("#inputBXEmployee").val(data.BXEmployee);
                //$("#inputApplyDept").val(data.ApplyDept);
                //$("#inputBXDate").val(data.BXDate.toString().replace('T', ' '));
                //$("#inputPhone").val(data.Phone);
                //$("#inputApplyEMail").val(data.EMail);
                //$("#inputBXDept").val(data.BXDept);
                //$("#inputAssetCode").val(data.AssetCode);
                //$("#inputComputerName").val(data.ComputerName);
                //$("#inputNote").val(data.Note);
                //$("#inputPriority").val(data.PriorityId);
                //$("#inputHopeTime").val(data.HopeTime.toString().replace('T', ' '));
                //$("#inputPrevOperation").val(data.PrevOperation);
                //$("#inputImagePath").val(data.ImagePath);

                //self.newRepairApplyBill.Title = data.Title;
                //self.newRepairApplyBill.BillNo = data.BillNo;
                //self.newRepairApplyBill.FaultType = data.FaultTypeId;
                //self.newRepairApplyBill.ApplyEmployee = data.ApplyEmployee;
                //self.newRepairApplyBill.BXEmployee = data.BXEmployee;
                //self.newRepairApplyBill.NextEmployee = data.NextEmployee;
                //self.newRepairApplyBill.BXDealTime = data.BXDealTime;
                //self.newRepairApplyBill.Priority = data.PriorityId;
                //self.newRepairApplyBill.PrevOperation = data.PrevOperation;
                //self.newRepairApplyBill.ImagePath = data.ImagePath;
                //self.newRepairApplyBill.HopeTime = data.HopeTime;
                //self.newRepairApplyBill.ApplyDept = data.ApplyDept;
                //self.newRepairApplyBill.BXDate = data.BXDate.toString().replace('T', ' ');
                //self.newRepairApplyBill.Phone = data.Phone;
                //self.newRepairApplyBill.ApplyEMail = data.EMail;
                //self.newRepairApplyBill.BXDept = data.BXDept;
                //self.newRepairApplyBill.AssetCode = data.AssetCode;
                //self.newRepairApplyBill.ComputerName = data.ComputerName;
                //self.newRepairApplyBill.Note = data.Note;
                //self.newRepairApplyBill.SatisfactionLevel = data.SatisfactionLevelId;
                //self.newRepairApplyBill.Status = data.StatusId;
                //self.newRepairApplyBill.Id = data.Id;
                //self.detail(data);

            });
        }

            //查看
        else if (sessionStorage.billType == 5) {
            $('#spanDisplay').text('查看报修单');
            $('.whenDealLock').attr("disabled", true);
            $('input').attr("disabled", true);
            $('select').attr("disabled", true);
            $('textarea').attr("disabled", true);

            $('#btnRepairBill').hide();


            ajaxHelper(repairApplyBillsUri + sessionStorage.billId, 'GET').done(function (data) {

                $("#inputTitle").val(data[0].Title);
                $("#inputBillNo").val(data[0].BillNo);
                $("#selFaultType").val(data[0].FaultTypeId);
                $("#inputApplyEmployee").val(data[0].ApplyEmployee);
                $("#inputBXEmployee").val(data[0].BXEmployee);
                $("#inputApplyDept").val(data[0].ApplyDept);
                $("#inputBXDate").val(data[0].BXDate.toString().replace('T', ' '));
                $("#inputPhone").val(data[0].Phone);
                $("#inputApplyEMail").val(data[0].EMail);
                $("#inputBXDept").val(data[0].BXDept);
                $("#inputAssetCode").val(data[0].AssetCode);
                $("#inputComputerName").val(data[0].ComputerName);
                $("#inputNote").val(data[0].Note);
                $("#inputPriority").val(data[0].PriorityId);
                $("#inputHopeTime").val(data[0].HopeTime.toString().replace('T', ' '));
                $("#inputPrevOperation").val(data[0].PrevOperation);
                $("#inputImagePath").val(data[0].ImagePath);

                $('#inputDealDate').val(data[0].BXDealTime.replace('T', ' ').replace('9999-12-30 00:00:00', ''));
                $('#inputDealer').val(data[0].BXDealEmployee);
                $('#inputDealNote').val(data[0].BXDealNote);

                statusId = data[0].StatusId;


                //显示处理方法
                if (statusId == 2) {
                    $('#divOnwayFlow').show();
                    $('#btnDealFlow').hide();

                }
                else {
                    $('#divOnwayFlow').hide();
                }

                imageFile = data[0].ImagePath;

                if (imageFile.length > 0) {
                    $("#img0").attr("src", "/Images/" + imageFile);
                    $("#aImg").attr("href", "/Images/" + imageFile);
                    $("#img0").show();
                }

            });
        }
    }


    self.addRepairApplyBill = function (formElement) {
        //新增

        if (sessionStorage.billType == 1) {
            if (bIsAdd == 0) {
                var repairApplyBill = {
                    BillNo: self.newRepairApplyBill.BillNo,
                    Title: self.newRepairApplyBill.Title(),
                    Note: self.newRepairApplyBill.Note(),
                    FaultTypeId: self.newRepairApplyBill.FaultType().Id,
                    ApplyEmployee: self.newRepairApplyBill.ApplyEmployee,
                    ApplyDept: self.newRepairApplyBill.ApplyDept,
                    BXEmployee: self.newRepairApplyBill.BXEmployee,
                    BXDept: self.newRepairApplyBill.BXDept,
                    BXDate: self.newRepairApplyBill.BXDate,
                    BXDealTime: '9999/12/30',
                    HopeTime: $('#inputHopeTime').val(),
                    PriorityId: self.newRepairApplyBill.Priority().Id,
                    PrevOperation: self.newRepairApplyBill.PrevOperation(),
                    ImagePath: sessionStorage.imagePath,
                    AssetCode: self.newRepairApplyBill.AssetCode(),
                    Phone: self.newRepairApplyBill.Phone,
                    EMail: self.newRepairApplyBill.ApplyEMail,
                    ComputerName: self.newRepairApplyBill.ComputerName(),
                    SatisfactionLevelId: 1,
                    StatusId: 1
                };

                ajaxHelper(repairApplyBillsUri, 'POST', repairApplyBill).done(function (item) {
                    self.repairApplyBills.push(item);
                }).success(function () {
                    bIsAdd = 1;
                    alert('提交成功！');
                    //location.href = 'RepairList.html?listType=5';
                });
            }
            else {
                alert('请勿重复提交！');
            }
        }
            //修改\填写满意度
        else if (sessionStorage.billType == 2 || sessionStorage.billType == 4) {
            var faultType = 1;
            if (self.newRepairApplyBill.FaultType.Id == undefined) faultType = self.newRepairApplyBill.FaultType;
            else faultType = self.newRepairApplyBill.FaultType.Id;

            var sati = 1;
            if (self.newRepairApplyBill.SatisfactionLevel.Id == undefined) sati = self.newRepairApplyBill.SatisfactionLevel;
            else sati = self.newRepairApplyBill.SatisfactionLevel.Id;

            var repairApplyBill = {
                BillNo: self.newRepairApplyBill.BillNo,
                Title: self.newRepairApplyBill.Title,
                Note: self.newRepairApplyBill.Note,
                //FaultTypeId: self.newRepairApplyBill.FaultType,
                FaultTypeId: faultType,
                ApplyEmployee: self.newRepairApplyBill.ApplyEmployee,
                BXDealTime: self.newRepairApplyBill.BXDealTime,
                HopeTime: self.newRepairApplyBill.HopeTime,
                PriorityId: self.newRepairApplyBill.Priority,
                PrevOperation: self.newRepairApplyBill.PrevOperation,
                ImagePath: self.newRepairApplyBill.ImagePath,
                NextEmployee: self.newRepairApplyBill.NextEmployee,
                ApplyDept: self.newRepairApplyBill.ApplyDept,
                BXEmployee: self.newRepairApplyBill.BXEmployee,
                BXDept: self.newRepairApplyBill.BXDept,
                BXDate: self.newRepairApplyBill.BXDate,
                AssetCode: self.newRepairApplyBill.AssetCode,
                Phone: self.newRepairApplyBill.Phone,
                EMail: self.newRepairApplyBill.ApplyEMail,
                ComputerName: self.newRepairApplyBill.ComputerName,
                //SatisfactionLevelId: self.newRepairApplyBill.SatisfactionLevel.Id,
                SatisfactionLevelId: sati,
                StatusId: statusId,
                Id: self.newRepairApplyBill.Id
            };
            var repairApplyBillsUri1 = '../api/repairApplyBills/' + sessionStorage.billId;
            ajaxHelper(repairApplyBillsUri1, 'PUT', repairApplyBill).done().success(
                function () {
                    alert('提交成功！');
                    location.href = 'RepairList.html?listType=5';
                });
        }
    }


    self.addOnwayFlow = function () {
        //更新报修单状态
        var repairApplyBill = {
            BillNo: self.newRepairApplyBill.BillNo,
            Title: self.newRepairApplyBill.Title,
            Note: self.newRepairApplyBill.Note,
            FaultTypeId: self.newRepairApplyBill.FaultType,
            ApplyEmployee: self.newRepairApplyBill.ApplyEmployee,
            ApplyDept: self.newRepairApplyBill.ApplyDept,
            BXEmployee: self.newRepairApplyBill.BXEmployee,
            BXDealTime: self.newRepairApplyBill.BXDealTime,
            HopeTime: self.newRepairApplyBill.HopeTime,
            PriorityId: self.newRepairApplyBill.Priority,
            PrevOperation: self.newRepairApplyBill.PrevOperation,
            ImagePath: self.newRepairApplyBill.ImagePath,
            NextEmployee: self.newRepairApplyBill.NextEmployee,
            BXDealEmployee: sessionStorage.userName,
            BXDealNote: self.newOnwayFlow.DealNote(),
            NextEmployee: self.newRepairApplyBill.BXEmployee,
            BXDept: self.newRepairApplyBill.BXDept,
            BXDate: self.newRepairApplyBill.BXDate,
            BXDealTime: self.newOnwayFlow.DealDate,
            AssetCode: self.newRepairApplyBill.AssetCode,
            Phone: self.newRepairApplyBill.Phone,
            EMail: self.newRepairApplyBill.ApplyEMail,
            ComputerName: self.newRepairApplyBill.ComputerName,
            SatisfactionLevelId: self.newRepairApplyBill.SatisfactionLevel,
            StatusId: 2,
            Id: self.newRepairApplyBill.Id
        };
        var repairApplyBillsUri1 = '../api/repairApplyBills/' + sessionStorage.billId;
        ajaxHelper(repairApplyBillsUri1, 'PUT', repairApplyBill).done();

        //新增一条在途流程
        var onwayFlow = {
            CurrentDealer: sessionStorage.userName,
            NextDealer: self.newOnwayFlow.NextDealer,
            DealDate: self.newOnwayFlow.DealDate,
            DealNote: self.newOnwayFlow.DealNote(),
            RepairAppyBillId: self.newOnwayFlow.RepairApplyBillId,
            DealMethodId: 2
        };
        var onwayFlowUri = '../api/onwayFlows/';
        ajaxHelper(onwayFlowUri, 'POST', onwayFlow).done(function (item) {
            self.onwayFlows.push(item);
        }).success(function () {
            alert('提交成功！');
            location.href = 'RepairList.html?listType=5';
        });
    }


    function getAllBaseData() {
        //满意度
        ajaxHelper(satisfactionLevelUri, 'GET').done(function (data) {
            self.satisfactionLevels(data);
        });
        //故障类型
        ajaxHelper(faultTypeUri, 'GET').done(function (data) {
            self.faultTypes(data);
        });
        //紧急程度
        ajaxHelper(priorityUri, 'GET').done(function (data) {
            self.prioritys(data);
        });
    }

    $(function () {

        if (window.opener) {
            //for chrome
            window.opener.returnValue = "opener returnValue";
        }
        else {
            window.returnValue = "window returnValue";
        }

        $('#aDistribute').click(function () {
            window.open('DealerSelect.html?level=1&id=' + sessionStorage.billId, null, "dialogWidth:500;dialogHeight:auto;status:no;help:no;center:yes");
        });

        $('#btnRepairBill').click(function () {
            if (!IsLowerThanIE9()) {
                var fileObj = $('#fileField').get(0).files[0];

                //var vedioOjb = $('#vedioField').get(0).files[0];

                if (fileObj) {
                    var fileSize = 0;
                    if (fileObj.size > 1024 * 1024) fileSize = (Math.round(fileObj.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
                    else fileSize = (Math.round(fileObj.size * 100 / 1024) / 100).toString() + 'KB';
                    //alert('文件名：' + fileObj.name + ',文件大小：' + fileSize + ',文件类型：' + fileObj.type);
                }

                // FormData 对象
                var form = new FormData();
                form.append("file", fileObj);
                //form.append("file", vedioOjb)

                var filePath = "";
                var start = 0;
                var end = 0;
                var xmlhttp = new XMLHttpRequest();
                xmlhttp.open("POST", "../api/Upload/", false);
                xmlhttp.send(form);
                if (xmlhttp.responseText.indexOf("Error:") != -1) {
                    //alert(xmlhttp.responseText);
                }

                filePath = xmlhttp.responseText;
                if (getExplorerType() == 'firefox') {
                    start = filePath.indexOf('|');
                    end = filePath.indexOf('</string>');
                    filePath = filePath.substring(start, end);
                }
                else {
                    filePath = filePath.substring(1, filePath.length - 1);
                }

                var array = filePath.split('|');
                for (var i = 0; i < array.length; i++) {
                    if (array[i].indexOf('.jpg') > 0 || array[i].indexOf('.png') > 0 || array[i].indexOf('.gif') > 0 || array[i].indexOf('.jpeg') > 0) {
                        sessionStorage.imagePath = array[i];
                    }
                    if (array[i].indexOf('.amr') > 0 || array[i].indexOf('.mp4') > 0 || array[i].indexOf('.3gpp') > 0 || array[i].indexOf('.mpeg') > 0) {
                        sessionStorage.vedioPath = array[i];
                    }
                }
            }

            //sessionStorage.filePath = filePath;
        });


        $("#fileField").change(function () {
            if (!IsLowerThanIE9()) {
                var objUrl = $('#fileField').get(0).files[0];
                //var fileObj = document.getElementById("fileField").files[0];
                objUrl = getObjectURL(objUrl);
                if (objUrl) {
                    $("#img0").attr("src", objUrl);
                    $("#img0").attr("style", "display:normal");
                    $("#aImg").attr("href", objUrl);
                }
                else {
                    $("#img0").attr("src", "");
                }
            }

        });

        //建立一個可存取到該file的url
        function getObjectURL(file) {
            var url = null;
            if (window.createObjectURL != undefined) { // basic
                url = window.createObjectURL(file);
            } else if (window.URL != undefined) { // mozilla(firefox)
                url = window.URL.createObjectURL(file);
            } else if (window.webkitURL != undefined || window.navigator.userAgent.indexOf("Safari") >= 1) { // webkit or chrome
                url = window.webkitURL.createObjectURL(file);
            }
            return url;
        }


        //$('#inInputPrevOperation').keydown(function () {
        //    var text1 = $("#inputPrevOperation").val();
        //    var len;//记录剩余字符串的长度
        //    if (text1.length >= 30) //限制字数
        //    {
        //        $("#inInputPrevOperation").val(text1.substr(0, 30));
        //        len = 0;
        //    }
        //    else {
        //        len = 30 - text1.length;  //限制字数
        //    }
        //    var show = "你还可以输入" + len + "个字";
        //    $("#pinglun").val(show);
        //});
    });
    //Fetch the initial data.
    getAllBaseData();
    getSystemTime();
    setUI();

};

//var vm = new ViewModel();
ko.applyBindings(ViewModel);


