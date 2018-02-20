var statusId = 0;
var trans2oaUrl = '/api/Trans2OA';

localStorage.imagePath = "";
localStorage.vedioPath = "";
var imageFile = "";
var bIsAdd = 0;
var bIsDeal = 0;
var status = $('input[name="rStatus"]:checked').val();
var bxDealEmployee, bxDealNote;
var nowTime;

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


    self.faultTypes = ko.observable().extend({
        minLength: { params: 1, message: "故障类别必须选择" },

    });



    self.newRepairApplyBill = {
        BillNo: ko.observable(),
        Title: ko.observable(),
        Note: ko.observable(),
        //FaultType: ko.observable(),
        Status: ko.observable(),
        ApplyEmployee: ko.observable(),
        ApplyDept: ko.observable(),
        BXDate: ko.observable(),
        BXEmployee: ko.observable(),
        NextEmployee: ko.observable(),
        BXDealTime: ko.observable(),
        //BXDept: ko.observable(),
        AssetCode: ko.observable(),
        //ComputerName: ko.observable(),
        Phone: ko.observable(),
        ApplyEMail: ko.observable(),
        //SatisfactionLevel: ko.observable(),
        //Priority: ko.observable(),
        PrevOperation: ko.observable(),
        //HopeTime: ko.observable(),
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
                objList = document.getElementById("selBXDept");
                objList.innerHTML = "";
                if (data.length > 0) {
                    $("#selBXDept").append(generateHtmlOption(data, type));
                }

                //ajaxHelper('/App_Data/center.json', 'GET').done(function (data) {
                //    var objList = document.getElementById("selBXDept");
                //    objList.innerHTML = "";
                //    if (data.length > 0) {
                //        $("#selBXDept").append(generateHtmlOption(data, type));
                //    }
                //});
                break;
            case 'faultType':
                ajaxHelper(faultTypeUri, 'GET').done(function (data) {
                    objList = document.getElementById("selFaultType");
                    objList.innerHTML = "";
                    if (data.length > 0) {
                        $("#selFaultType").append(generateHtmlOption(data, type));
                    }
                })
                break;
            case 'priority':
                ajaxHelper(priorityUri, 'GET').done(function (data) {
                    objList = document.getElementById("selPriority");
                    objList.innerHTML = "";
                    if (data.length > 0) {
                        $("#selPriority").append(generateHtmlOption(data, type));
                    }
                })
                break;
            case 'satisfactionLevels':
                ajaxHelper(satisfactionLevelUri, 'GET').done(function (data) {
                    objList = document.getElementById("selSatisfactionLevels");
                    objList.innerHTML = "";
                    if (data.length > 0) {
                        $("#selSatisfactionLevels").append(generateHtmlOption(data, type));
                    }
                })
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
            case 'faultType':

                option = option + '<option value="">' + "" + '</option>';
                $.each(dataArr, function (key, val) {
                    option = option + '<option value="' + val.Id + '">' + val.Name + '</option>';
                });
                break;
            case 'priority':
            case 'satisfactionLevels':
                $.each(dataArr, function (key, val) {
                    option = option + '<option value="' + val.Id + '">' + val.Name + '</option>';
                });
                break;
        }
        return option;
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
        nowTime = time;
    }

    //根据不同入口控制页面
    localStorage.billType = getQueryString("billType");
    localStorage.billId = getQueryString("id");

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
        if (ua.indexOf("msie 8.0") > 0 || ua.indexOf("msie 9.0") > 0 || ua.indexOf("msie 7.0") > 0 || ua.indexOf("msie 6.0") > 0) {
            ret = true;
        }

        //if (ua.indexOf("msie 7.0") > 0 || ua.indexOf("msie 6.0") > 0) {
        //    ret = true;
        //}
        return ret;
    }

    //根据billType设置界面值
    function setUI() {

        if (localStorage.userType == 0) {
            $('#aDistribute').hide();
        }

        //billType=1：新增
        //billType=2：修改
        //billType=3：处理
        //billType=4：填写满意度
        //billType=5：查看
        if (localStorage.billType == 1) {

            if (IsLowerThanIE9()) {
                //if (getOS() == "x86")
                //    $("#lbUploadx86").show();
                //else $("#lbUploadx64").show();
            }

            $('#spanDisplay').text('新增报修单');
            $('.Satisfaction').hide();
            $('.admin').hide();
            $('#formOnwayFlow').hide();

            //var billNo = year + month + date + hour + minute + second + millisecond;

            self.newRepairApplyBill.BillNo = billNo;
            self.newRepairApplyBill.ApplyEmployee = localStorage.userName;
            self.newRepairApplyBill.BXEmployee = localStorage.userName;
            self.newRepairApplyBill.ApplyDept = localStorage.deptName;
            //self.newRepairApplyBill.BXDept = localStorage.deptName;
            $('#selBXDept').val(localStorage.deptName);
            self.newRepairApplyBill.ApplyEMail = localStorage.email;
            self.newRepairApplyBill.Phone = localStorage.mobile;
        }
            //修改
        else if (localStorage.billType == 2) {
            $('#spanDisplay').text('修改报修单');
            //$('.whenDealLock').attr("readonly", true);

            $('.add').attr("readonly", true);
            $('.edit').attr("readonly", false);
            $('.Satisfaction').hide();
            $('#formOnwayFlow').hide();

            ajaxHelper('../api/repairApplyBills/' + localStorage.billId, 'GET').done(function (data) {
                $("#inputTitle").val(data[0].Title);
                $("#inputBillNo").val(data[0].BillNo);
                $("#selFaultType").val(data[0].FaultTypeId);
                $("#inputApplyEmployee").val(data[0].ApplyEmployee);
                $("#inputBXEmployee").val(data[0].BXEmployee);
                $("#inputApplyDept").val(data[0].ApplyDept);
                $("#inputBXDate").val(data[0].BXDate.toString().replace('T', ' '));
                $("#inputPhone").val(data[0].Phone);
                $("#inputApplyEMail").val(data[0].EMail);
                $("#selBXDept").val(data[0].BXDept);
                $("#inputAssetCode").val(data[0].AssetCode);
                //$("#inputComputerName").val(data[0].ComputerName);
                $("#inputNote").val(data[0].Note);
                $("#selPriority").val(data[0].PriorityId);
                //$("#inputHopeTime").val(data[0].HopeTime.toString().replace('T', ' '));
                $("#inputPrevOperation").val(data[0].PrevOperation);
                $("#inputImagePath").val(data[0].ImagePath);


                self.newRepairApplyBill.Title = data[0].Title;
                self.newRepairApplyBill.BillNo = data[0].BillNo;
                //self.newRepairApplyBill.FaultType = data[0].FaultTypeId;
                //faultTypeId = data[0].FaultTypeId;
                self.newRepairApplyBill.ApplyEmployee = data[0].ApplyEmployee;
                self.newRepairApplyBill.BXEmployee = data[0].BXEmployee;
                self.newRepairApplyBill.NextEmployee = data[0].NextEmployee;
                self.newRepairApplyBill.BXDealTime = data[0].BXDealTime.toString().replace('T', ' ');
                //self.newRepairApplyBill.Priority = data[0].PriorityId;
                //priorityId = data[0].PriorityId;
                self.newRepairApplyBill.PrevOperation = data[0].PrevOperation;
                self.newRepairApplyBill.ImagePath = data[0].ImagePath;
                //self.newRepairApplyBill.HopeTime = data[0].HopeTime.toString().replace('T', ' ');
                self.newRepairApplyBill.ApplyDept = data[0].ApplyDept;
                self.newRepairApplyBill.BXDate = data[0].BXDate.toString().replace('T', ' ');
                self.newRepairApplyBill.Phone = data[0].Phone;
                self.newRepairApplyBill.ApplyEMail = data[0].EMail;
                //self.newRepairApplyBill.BXDept = data[0].BXDept;
                self.newRepairApplyBill.AssetCode = data[0].AssetCode;
                //self.newRepairApplyBill.ComputerName = data[0].ComputerName;
                self.newRepairApplyBill.Note = data[0].Note;
                //self.newRepairApplyBill.SatisfactionLevel = data[0].SatisfactionLevelId;
                //satisfactionLevelId = data[0].SatisfactionLevelId;
                statusId = data[0].StatusId;
                self.newRepairApplyBill.Id = data[0].Id;
                imageFile = data[0].ImagePath;

                if (imageFile.length > 0) {
                    $("#img0").attr("src", "/Images/" + imageFile);
                    $("#aImg").attr("href", "/Images/" + imageFile);
                    $("#img0").show();
                }
            });

        }
            //处理
        else if (localStorage.billType == 3) {
            $('#spanDisplay').text('处理报修单');

            $('.add').attr("readonly", true);

            $('.edit').attr("readonly", true);
            $('.deal').attr("readonly", false);
            $('.Satisfaction').hide();
            $('.admin').hide();
            $('#btnRepairBill').hide();

            //状态：处理完成
            if (status == 2) {
                $('#inputDealProcess').attr("readonly", true);
                $('#inputDealNote').attr("readonly", false);
            }
                //状态：接单
            else if (status == 1) {
                $('#inputDealProcess').attr("readonly", false);
                $('#inputDealNote').attr("readonly", true);
            }

            ajaxHelper(repairApplyBillsUri + localStorage.billId, 'GET').done(function (data) {
                $("#inputTitle").val(data[0].Title);
                $("#inputBillNo").val(data[0].BillNo);
                $("#selFaultType").val(data[0].FaultTypeId);
                $("#inputApplyEmployee").val(data[0].ApplyEmployee);
                $("#inputBXEmployee").val(data[0].BXEmployee);
                $("#inputApplyDept").val(data[0].ApplyDept);
                $("#inputBXDate").val(data[0].BXDate.toString().replace('T', ' '));
                $("#inputPhone").val(data[0].Phone);
                $("#inputApplyEMail").val(data[0].EMail);
                $("#selBXDept").val(data[0].BXDept);
                $("#inputAssetCode").val(data[0].AssetCode);
                //$("#inputComputerName").val(data[0].ComputerName);
                $("#inputNote").val(data[0].Note);
                $("#selPriority").val(data[0].PriorityId);
                //$("#inputHopeTime").val(data[0].HopeTime.toString().replace('T', ' '));
                $("#inputPrevOperation").val(data[0].PrevOperation);
                $("#inputImagePath").val(data[0].ImagePath);
                $('#inputDealNote').val(data[0].BXDealNote);
                $('#inputDealProcess').val(data[0].BXDealProcess);

                self.newRepairApplyBill.Title = data[0].Title;
                self.newRepairApplyBill.BillNo = data[0].BillNo;
                //self.newRepairApplyBill.FaultType = data[0].FaultTypeId;
                //faultTypeId = data[0].FaultTypeId;
                self.newRepairApplyBill.ApplyEmployee = data[0].ApplyEmployee;
                self.newRepairApplyBill.BXEmployee = data[0].BXEmployee;
                self.newRepairApplyBill.NextEmployee = data[0].NextEmployee;
                self.newRepairApplyBill.BXDealTime = data[0].BXDealTime.toString().replace('T', ' ');
                //self.newRepairApplyBill.Priority = data[0].PriorityId;
                //priorityId = data[0].PriorityId;
                self.newRepairApplyBill.PrevOperation = data[0].PrevOperation;
                self.newRepairApplyBill.ImagePath = data[0].ImagePath;
                //self.newRepairApplyBill.HopeTime = data[0].HopeTime.toString().replace('T', ' ');
                self.newRepairApplyBill.ApplyDept = data[0].ApplyDept;
                self.newRepairApplyBill.BXDate = data[0].BXDate.toString().replace('T', ' ');
                self.newRepairApplyBill.Phone = data[0].Phone;
                self.newRepairApplyBill.ApplyEMail = data[0].EMail;
                //self.newRepairApplyBill.BXDept = data[0].BXDept;
                self.newRepairApplyBill.AssetCode = data[0].AssetCode;
                //self.newRepairApplyBill.ComputerName = data[0].ComputerName;
                self.newRepairApplyBill.Note = data[0].Note;
                //self.newRepairApplyBill.SatisfactionLevel = data[0].SatisfactionLevelId;
                //satisfactionLevelId = data[0].SatisfactionLevelId;
                statusId = data[0].StatusId;

                self.newRepairApplyBill.Id = data[0].Id;

                imageFile = data[0].ImagePath;
                if (imageFile != null) {
                    if (imageFile.length > 0) {
                        $("#img0").attr("src", "/Images/" + imageFile);
                        $("#aImg").attr("href", "/Images/" + imageFile);
                        $("#img0").show();
                    }
                }

                $('#inputDealer').val(localStorage.userName);
                self.newOnwayFlow.NextDealer = data[0].ApplyEmployee;
                self.newOnwayFlow.RepairApplyBillId = data[0].Id;
                self.newOnwayFlow.CurrentDealer = localStorage.userName;
            });
        }

            //填写满意度
        else if (localStorage.billType == 4) {
            $('#spanDisplay').text('填写满意度');
            $('.add').attr("readonly", true);
            $('.edit').attr("readonly", true);
            $('.deal').attr("readonly", true);
            $('.admin').hide();
            $('#formOnwayFlow').hide();

            ajaxHelper(repairApplyBillsUri + localStorage.billId, 'GET').done(function (data) {
                $("#inputTitle").val(data[0].Title);
                $("#inputBillNo").val(data[0].BillNo);
                $("#selFaultType").val(data[0].FaultTypeId);
                $("#inputApplyEmployee").val(data[0].ApplyEmployee);
                $("#inputBXEmployee").val(data[0].BXEmployee);
                $("#inputApplyDept").val(data[0].ApplyDept);
                $("#inputBXDate").val(data[0].BXDate.toString().replace('T', ' '));
                $("#inputPhone").val(data[0].Phone);
                $("#inputApplyEMail").val(data[0].EMail);
                $("#selBXDept").val(data[0].BXDept);
                $("#inputAssetCode").val(data[0].AssetCode);
                //$("#inputComputerName").val(data[0].ComputerName);
                $("#inputNote").val(data[0].Note);
                $("#selPriority").val(data[0].PriorityId);
                //$("#inputHopeTime").val(data[0].HopeTime.toString().replace('T', ' '));
                $("#inputPrevOperation").val(data[0].PrevOperation);
                $("#inputImagePath").val(data[0].ImagePath);

                self.newRepairApplyBill.Title = data[0].Title;
                self.newRepairApplyBill.BillNo = data[0].BillNo;
                //self.newRepairApplyBill.FaultType = data[0].FaultTypeId;
                //faultTypeId = data[0].FaultTypeId;
                self.newRepairApplyBill.ApplyEmployee = data[0].ApplyEmployee;
                self.newRepairApplyBill.BXEmployee = data[0].BXEmployee;
                self.newRepairApplyBill.NextEmployee = data[0].NextEmployee;

                self.newRepairApplyBill.BXDealTime = data[0].BXDealTime.toString().replace('T', ' ');
                //self.newRepairApplyBill.Priority = data[0].PriorityId;
                //priorityId = data[0].PriorityId;
                self.newRepairApplyBill.PrevOperation = data[0].PrevOperation;
                self.newRepairApplyBill.ImagePath = data[0].ImagePath;
                //self.newRepairApplyBill.HopeTime = data[0].HopeTime.toString().replace('T', ' ');
                self.newRepairApplyBill.ApplyDept = data[0].ApplyDept;
                self.newRepairApplyBill.BXDate = data[0].BXDate.toString().replace('T', ' ');
                self.newRepairApplyBill.Phone = data[0].Phone;
                self.newRepairApplyBill.ApplyEMail = data[0].EMail;
                //self.newRepairApplyBill.BXDept = data[0].BXDept;
                self.newRepairApplyBill.AssetCode = data[0].AssetCode;
                //self.newRepairApplyBill.ComputerName = data[0].ComputerName;
                self.newRepairApplyBill.Note = data[0].Note;
                //self.newRepairApplyBill.SatisfactionLevel = data[0].SatisfactionLevelId;
                //satisfactionLevelId = data[0].SatisfactionLevelId;
                statusId = data[0].StatusId;

                self.newRepairApplyBill.Id = data[0].Id;

                bxDealEmployee = data[0].BXDealEmployee;
                bxDealNote = data[0].BXDealNote;

                imageFile = data[0].ImagePath;
                if (imageFile != null) {
                    if (imageFile.length > 0) {
                        $("#img0").attr("src", "/Images/" + imageFile);
                        $("#aImg").attr("href", "/Images/" + imageFile);
                        $("#img0").show();
                    }
                }
            });
        }

            //查看
        else if (localStorage.billType == 5) {
            $('#spanDisplay').text('查看报修单');
            $('.whenDealLock').attr("readonly", true);
            $('input').attr("readonly", true);
            $('select').attr("readonly", true);
            $('textarea').attr("readonly", true);

            $('#btnRepairBill').hide();
            $('#aDistribute').hide();

            ajaxHelper(repairApplyBillsUri + localStorage.billId, 'GET').done(function (data) {
                $("#inputTitle").val(data[0].Title);
                $("#inputBillNo").val(data[0].BillNo);
                $("#selFaultType").val(data[0].FaultTypeId);
                $("#inputApplyEmployee").val(data[0].ApplyEmployee);
                $("#inputBXEmployee").val(data[0].BXEmployee);
                $("#inputApplyDept").val(data[0].ApplyDept);
                $("#inputBXDate").val(data[0].BXDate.toString().replace('T', ' '));
                $("#inputPhone").val(data[0].Phone);
                $("#inputApplyEMail").val(data[0].EMail);
                $("#selBXDept").val(data[0].BXDept);
                $("#inputAssetCode").val(data[0].AssetCode);
                //$("#inputComputerName").val(data[0].ComputerName);
                $("#inputNote").val(data[0].Note);
                $("#selPriority").val(data[0].PriorityId);
                //$("#inputHopeTime").val(data[0].HopeTime.toString().replace('T', ' '));
                $("#inputPrevOperation").val(data[0].PrevOperation);
                $("#inputImagePath").val(data[0].ImagePath);
                $('#selSatisfactionLevels').val(data[0].SatisfactionLevelId);

                $('#inputDealDate').val(data[0].BXDealTime.replace('T', ' ').replace('9999-12-30 00:00:00', ''));
                $('#inputDealer').val(data[0].BXDealEmployee);
                $('#inputDealNote').val(data[0].BXDealNote);
                $('#inputDealProcess').val(data[0].BXDealProcess);

                statusId = data[0].StatusId;

                $("input[name='rStatus'][value=" + statusId + "]").attr("checked", true);

                //显示处理方法
                //if (statusId == 2) {
                //    $('#divOnwayFlow').show();


                //}
                //else {
                //    //$('#divOnwayFlow').hide();
                //}

                if (statusId == 3) {
                    $('.deal1').hide();
                }

                $('#btnDealFlow').hide();
                $('#btnTransferOA').hide();

                imageFile = data[0].ImagePath;
                if (imageFile != null) {
                    if (imageFile.length > 0) {
                        $("#img0").attr("src", "/Images/" + imageFile);
                        $("#aImg").attr("href", "/Images/" + imageFile);
                        $("#img0").show();
                    }
                }
            });
        }
    }


    self.addRepairApplyBill = function (formElement) {
        //新增

        if (localStorage.billType == 1) {
            if (bIsAdd == 0) {
                var repairApplyBill = {
                    BillNo: self.newRepairApplyBill.BillNo,
                    Title: self.newRepairApplyBill.Title(),
                    Note: self.newRepairApplyBill.Note(),
                    //FaultTypeId: self.newRepairApplyBill.FaultType().Id,
                    FaultTypeId: $('#selFaultType').val(),
                    ApplyEmployee: self.newRepairApplyBill.ApplyEmployee,
                    ApplyDept: self.newRepairApplyBill.ApplyDept,
                    BXEmployee: self.newRepairApplyBill.BXEmployee,
                    BXDept: $('#selBXDept').val(),
                    BXDate: self.newRepairApplyBill.BXDate,
                    BXDealTime: '9999/12/30',
                    //HopeTime: $('#inputHopeTime').val(),
                    HopeTime: nowTime,
                    PriorityId: $('#selPriority').val(),
                    PrevOperation: self.newRepairApplyBill.PrevOperation(),
                    ImagePath: localStorage.imagePath,
                    AssetCode: self.newRepairApplyBill.AssetCode(),
                    Phone: self.newRepairApplyBill.Phone,
                    EMail: self.newRepairApplyBill.ApplyEMail,
                    ComputerName: self.newRepairApplyBill.AssetCode(),
                    SatisfactionLevelId: 1,
                    StatusId: 3,
                    DeviceType: localStorage.deviceType
                };

                if ($("#inputTitle").val() == "") {
                    alert("主题必须输入！");
                    return false;
                }

                if ($("#inputAssetCode").val() == "") {
                    alert("资产编号必须输入！");
                    return false;
                }

                //if ($('#selFaultType').val() == "") {
                //    alert("故障类型必须输入！");
                //    return false;
                //}



                ajaxHelper(repairApplyBillsUri, 'POST', repairApplyBill).done(function (item) {
                    self.repairApplyBills.push(item);
                }).success(function () {
                    bIsAdd = 1;
                });

                alert('提交成功！');
                location.href = 'RepairList.html?listType=1';

            }
            else {
                alert('请勿重复提交！');
            }
        }
            //修改\填写满意度
        else if (localStorage.billType == 2 || localStorage.billType == 4) {
            //var faultType = 1;
            //if (self.newRepairApplyBill.FaultType.Id == undefined) faultType = self.newRepairApplyBill.FaultType;
            //else faultType = self.newRepairApplyBill.FaultType.Id;

            //var sati = 1;
            //if (self.newRepairApplyBill.SatisfactionLevel.Id == undefined) sati = self.newRepairApplyBill.SatisfactionLevel;
            //else sati = self.newRepairApplyBill.SatisfactionLevel.Id;

            var repairApplyBill = {
                BillNo: self.newRepairApplyBill.BillNo,
                Title: self.newRepairApplyBill.Title,
                Note: self.newRepairApplyBill.Note,
                //FaultTypeId: self.newRepairApplyBill.FaultType,
                FaultTypeId: $('#selFaultType').val(),
                ApplyEmployee: self.newRepairApplyBill.ApplyEmployee,
                BXDealTime: self.newRepairApplyBill.BXDealTime,
                //HopeTime: self.newRepairApplyBill.HopeTime,
                HopeTime: nowTime,
                //PriorityId: self.newRepairApplyBill.Priority,
                PriorityId: $('#selPriority').val(),
                PrevOperation: self.newRepairApplyBill.PrevOperation,
                ImagePath: self.newRepairApplyBill.ImagePath,
                NextEmployee: self.newRepairApplyBill.NextEmployee,
                ApplyDept: self.newRepairApplyBill.ApplyDept,
                BXEmployee: self.newRepairApplyBill.BXEmployee,
                BXDept: $('#selBXDept').val(),
                BXDate: self.newRepairApplyBill.BXDate,
                BXDealEmployee: bxDealEmployee,
                BXDealNote: bxDealNote,
                AssetCode: self.newRepairApplyBill.AssetCode,
                Phone: self.newRepairApplyBill.Phone,
                EMail: self.newRepairApplyBill.ApplyEMail,
                ComputerName: self.newRepairApplyBill.AssetCode,
                SatisfactionLevelId: $('#selSatisfactionLevels').val(),
                //SatisfactionLevelId: sati,
                StatusId: statusId,
                Id: self.newRepairApplyBill.Id
            };
            var repairApplyBillsUri1 = '../api/repairApplyBills/' + localStorage.billId;
            ajaxHelper(repairApplyBillsUri1, 'PUT', repairApplyBill).done().success(
                function () {
                    alert('提交成功！');
                    location.href = 'RepairList.html?listType=5';
                });
        }
    }

    self.addOnwayFlow = function () {

        var dealmethod = 2;
        if (status == 2) {
            if ($("#inputDealNote").val() == "") {
                alert("请录入【解决方法】！");
                return false;
            }
            dealmethod = 2;
        }
        else if (status == 1) {
            if ($("#inputDealProcess").val() == "") {
                alert("请录入【处理过程】！");
                return false;
            }
            dealmethod = 4;
        }


        var onwayFlowEntity = {
            onwayFlow: {
                CurrentDealer: localStorage.userName,
                NextDealer: self.newOnwayFlow.NextDealer,
                DealDate: self.newOnwayFlow.DealDate,
                DealNote: self.newOnwayFlow.DealNote(),
                RepairAppyBillId: self.newOnwayFlow.RepairApplyBillId,
                DealMethodId: dealmethod,
                DealProcess: $('#inputDealProcess').val()
            },
            repairApplyBill: {
                BillNo: self.newRepairApplyBill.BillNo,
                Title: self.newRepairApplyBill.Title,
                Note: self.newRepairApplyBill.Note,
                //FaultTypeId: faultType,
                FaultTypeId: $('#selFaultType').val(),
                ApplyEmployee: self.newRepairApplyBill.ApplyEmployee,
                BXDealTime: self.newRepairApplyBill.BXDealTime,
                //HopeTime: self.newRepairApplyBill.HopeTime,
                HopeTime: nowTime,
                //PriorityId: self.newRepairApplyBill.Priority,
                PriorityId: $('#selPriority').val(),
                PrevOperation: self.newRepairApplyBill.PrevOperation,
                ImagePath: self.newRepairApplyBill.ImagePath,
                NextEmployee: self.newRepairApplyBill.NextEmployee,
                BXDealEmployee: localStorage.userName,
                BXDealNote: self.newOnwayFlow.DealNote(),
                ApplyDept: self.newRepairApplyBill.ApplyDept,
                BXEmployee: self.newRepairApplyBill.BXEmployee,
                BXDept: $('#selBXDept').val(),
                BXDate: self.newRepairApplyBill.BXDate,
                BXDealTime: self.newOnwayFlow.DealDate,
                AssetCode: self.newRepairApplyBill.AssetCode,
                Phone: self.newRepairApplyBill.Phone,
                EMail: self.newRepairApplyBill.ApplyEMail,
                ComputerName: self.newRepairApplyBill.AssetCode,
                SatisfactionLevelId: 1,
                //StatusId: 2,
                StatusId: status,
                Id: self.newRepairApplyBill.Id,
                BXDealProcess: $('#inputDealProcess').val()
            }
        }

        var onwayFlowEntityUri = '../api/onwayFlowsEntity/';
        ajaxHelper(onwayFlowEntityUri, 'POST', onwayFlowEntity).done(function (data, status) {
            if (status == 'success') {
                alert('提交成功！');
                location.href = 'RepairList.html?listType=1';
            }
        });
    }

    function getAllBaseData() {

        //报修人中心
        getData('center');
        //故障类型
        getData('faultType');
        //满意度   
        getData('satisfactionLevels');
        //紧急程度
        getData('priority');
    }

    $(function () {

        $(".status").change(function () {
            var val = $("input[name='rStatus']:checked").val();//获得选中的radio的值 
            status = val;
            if (val == '1') {//接单,处理中
                $('#inputDealProcess').attr("readonly", false);
                $('#inputDealNote').attr("readonly", true);
            }
            else if (val == '2') {//处理完成
                $('#inputDealProcess').attr("readonly", true);
                $('#inputDealNote').attr("readonly", false);
            }
        });

        $('#btnTransferOA').click(function () {
            var obj = {
                BillNo: $('#inputBillNo').val(),
                Title: $('#inputTitle').val(),
                ApplyEmployee: $('#inputApplyEmployee').val(),
                ApplyDept: $('#inputApplyDept').val(),
                BXEmployee: $('#inputBXEmployee').val(),
                BXDept: $('#selBXDept').val(),
                Phone: $('#inputPhone').val(),
                EMail: $('#inputApplyEMail').val(),
                Note: $('#inputNote').val(),
                FaultTypeName: $("#selFaultType").find("option:selected").text(),
                BXDate: $('#inputBXDate').val(),
                PriorityName: $("#selPriority").find("option:selected").text(),
                PrevOperation: $('#inputPrevOperation').val(),
                AssetCode: $('#inputAssetCode').val(),
                BXDealEmployee: $('#inputDealer').val(),
                BXDealTime: $('#inputDealDate').val(),
                BXDealNote: $('#inputDealNote').val(),
                BXDealProcess: $('#inputDealProcess').val()
            };
            ajaxHelper(trans2oaUrl, 'POST', obj).done(function (item) {
            });
            alert('提交成功！');
            location.href = 'RepairList.html?listType=1';
        });


        //if (window.opener) {
        //    //for chrome
        //    window.opener.returnValue = "opener returnValue";
        //}
        //else {
        //    window.returnValue = "window returnValue";
        //}

        $('#aDistribute').click(function () {
            window.open('DealerSelect.html?level=1&id=' + localStorage.billId, null, "dialogWidth:500;dialogHeight:auto;status:no;help:no;center:yes");
        });

        //$('#btnRepairBill').click(function () {
        //    if (!IsLowerThanIE9()) {
        //        var fileObj = $('#fileField').get(0).files[0];

        //        //var vedioOjb = $('#vedioField').get(0).files[0];

        //        if (fileObj) {
        //            var fileSize = 0;
        //            if (fileObj.size > 1024 * 1024) fileSize = (Math.round(fileObj.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
        //            else fileSize = (Math.round(fileObj.size * 100 / 1024) / 100).toString() + 'KB';
        //            //alert('文件名：' + fileObj.name + ',文件大小：' + fileSize + ',文件类型：' + fileObj.type);
        //        }

        //        // FormData 对象
        //        var form = new FormData();
        //        form.append("file", fileObj);
        //        //form.append("file", vedioOjb)

        //        var filePath = "";
        //        var start = 0;
        //        var end = 0;
        //        var xmlhttp = new XMLHttpRequest();
        //        xmlhttp.open("POST", "../api/Upload/", false);
        //        xmlhttp.send(form);
        //        if (xmlhttp.responseText.indexOf("Error:") != -1) {
        //            //alert(xmlhttp.responseText);
        //        }

        //        filePath = xmlhttp.responseText;
        //        if (getExplorerType() == 'firefox') {
        //            start = filePath.indexOf('|');
        //            end = filePath.indexOf('</string>');
        //            filePath = filePath.substring(start, end);
        //        }
        //        else {
        //            filePath = filePath.substring(1, filePath.length - 1);
        //        }

        //        var array = filePath.split('|');
        //        for (var i = 0; i < array.length; i++) {
        //            if (array[i].indexOf('.jpg') > 0 || array[i].indexOf('.png') > 0 || array[i].indexOf('.gif') > 0 || array[i].indexOf('.jpeg') > 0) {
        //                localStorage.imagePath = array[i];
        //            }
        //            if (array[i].indexOf('.amr') > 0 || array[i].indexOf('.mp4') > 0 || array[i].indexOf('.3gpp') > 0 || array[i].indexOf('.mpeg') > 0) {
        //                localStorage.vedioPath = array[i];
        //            }
        //        }
        //    }

        //    //localStorage.filePath = filePath;
        //});


        //$("#fileField").change(function () {
        //    if (!IsLowerThanIE9()) {
        //        var objUrl = $('#fileField').get(0).files[0];
        //        //var fileObj = document.getElementById("fileField").files[0];
        //        objUrl = getObjectURL(objUrl);
        //        if (objUrl) {
        //            $("#img0").attr("src", objUrl);
        //            $("#img0").attr("style", "display:normal");
        //            $("#aImg").attr("href", objUrl);
        //        }
        //        else {
        //            $("#img0").attr("src", "");
        //        }
        //    }

        //});

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


