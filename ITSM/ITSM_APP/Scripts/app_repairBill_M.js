var billType = 0;
var billId = 0;

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
        //ImagePath: ko.observable(),
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
            cache: false,
            contentType: 'application/json',
            xhrFields: {
                withCredentials: true
            },
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
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

        billNo = year.toString() + month.toString() + date.toString() + hour.toString() + minute.toString() + second.toString() + millisecond.toString();
        self.newRepairApplyBill.BXDate = time;
        self.newOnwayFlow.DealDate = time;
    }

    //获取页面参数
    function getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;

    }

    //根据billType设置界面值
    function setUI() {
        billId = getQueryString('id');
        billType = getQueryString('billType');

        if (sessionStorage.userName == undefined) {
            //获取当前登陆账号的姓名、部门、邮箱
            ajaxHelper(loginUri, 'GET').done(function (data) {
                sessionStorage.userName = data.UserName;
                sessionStorage.deptName = data.DeptName;
                sessionStorage.email = data.EMail;
            });
        }

        //self.newRepairApplyBill.BillNo = billNo;
        //self.newRepairApplyBill.ApplyEmployee = sessionStorage.userName;
        //self.newRepairApplyBill.BXEmployee = sessionStorage.userName;
        //self.newRepairApplyBill.ApplyDept = sessionStorage.deptName;
        //self.newRepairApplyBill.BXDept = sessionStorage.deptName;
        //self.newRepairApplyBill.ApplyEMail = sessionStorage.email;

        if (sessionStorage.userType == 0) {
            $('.admin').hide();
        }

        //billType=1：新增
        //billType=2：修改
        //billType=3：处理
        //billType=4：填写满意度
        //billType=5：查看
        if (billType == 1) {
            $('#spanDisplay').text('新增报修单');
            $('.whenAddHide').hide();
            $('#dealModule').hide();
            //$('.Satisfaction').hide();
            $('.admin').hide();
            $('#divDistribute').hide();

            $("#inputBillNo").val(billNo)
            $("#inputApplyEmployee").val(sessionStorage.userName);
            $("#inputBXEmployee").val(sessionStorage.userName);
            $("#inputApplyDept").val(sessionStorage.deptName);
            $("#inputBXDept").val(sessionStorage.deptName);
            $("#inputApplyEMail").val(sessionStorage.email);

            self.newRepairApplyBill.BillNo = billNo;
            self.newRepairApplyBill.ApplyEmployee = sessionStorage.userName;
            self.newRepairApplyBill.BXEmployee = sessionStorage.userName;
            self.newRepairApplyBill.ApplyDept = sessionStorage.deptName;
            self.newRepairApplyBill.BXDept = sessionStorage.deptName;
            self.newRepairApplyBill.ApplyEMail = sessionStorage.email;

        }
            //修改
        else if (billType == 2) {
            $('#spanDisplay').text('修改报修单');
            $('.whenDealLock').attr("disabled", true);
            $('.Satisfaction').hide();
            $('.whenAddHide').hide();
            $('#dealModule').hide();
            $('#divDistribute').hide();
            ajaxHelper('../api/repairApplyBills/' + billId, 'GET').done(function (data) {
                $("#inputTitle").val(data.Title);
                $("#inputBillNo").val(data.BillNo);
                //$("#selFaultType").val(data.FaultTypeId);
                $("#inputApplyEmployee").val(data.ApplyEmployee);
                $("#inputBXEmployee").val(data.BXEmployee);
                $("#inputApplyDept").val(data.ApplyDept);
                $("#inputBXDate").val(data.BXDate.toString().replace('T', ' '));
                $("#inputPhone").val(data.Phone);
                $("#inputApplyEMail").val(data.EMail);
                $("#inputBXDept").val(data.BXDept);
                $("#inputAssetCode").val(data.AssetCode);
                $("#inputComputerName").val(data.ComputerName);
                $("#inputNote").val(data.Note);
                $("#inputPriority").val(data.PriorityId);
                $("#inputHopeTime").val(data.HopeTime.toString().replace('T', ' '));
                $("#inputPrevOperation").val(data.PrevOperation);
                //$("#inputImagePath").val(data.ImagePath);

                self.newRepairApplyBill.Title = data.Title;
                self.newRepairApplyBill.BillNo = data.BillNo;
                self.newRepairApplyBill.FaultType = data.FaultTypeId;
                self.newRepairApplyBill.ApplyEmployee = data.ApplyEmployee;
                self.newRepairApplyBill.BXEmployee = data.BXEmployee;
                self.newRepairApplyBill.NextEmployee = data.NextEmployee;
                self.newRepairApplyBill.BXDealTime = data.BXDealTime;
                self.newRepairApplyBill.Priority = data.PriorityId;
                self.newRepairApplyBill.PrevOperation = data.PrevOperation;
                //self.newRepairApplyBill.ImagePath = data.ImagePath;
                self.newRepairApplyBill.HopeTime = data.HopeTime;
                self.newRepairApplyBill.ApplyDept = data.ApplyDept;
                self.newRepairApplyBill.BXDate = data.BXDate.toString().replace('T', ' ');
                self.newRepairApplyBill.Phone = data.Phone;
                self.newRepairApplyBill.ApplyEMail = data.EMail;
                self.newRepairApplyBill.BXDept = data.BXDept;
                self.newRepairApplyBill.AssetCode = data.AssetCode;
                self.newRepairApplyBill.ComputerName = data.ComputerName;
                self.newRepairApplyBill.Note = data.Note;
                self.newRepairApplyBill.SatisfactionLevel = data.SatisfactionLevelId;
                self.newRepairApplyBill.Status = data.StatusId;
                self.newRepairApplyBill.Id = data.Id;
                //self.detail(data);
            });

        }
            //处理
        else if (billType == 3) {
            $('#spanDisplay').text('处理报修单');
            $('.whenDealLock').attr("disabled", true);
            $('.Satisfaction').hide();
            //$('#divRepairBill').hide();
            $('#btnRepairBill').hide();
            //$('.whenAddHide').hide();

            ajaxHelper(repairApplyBillsUri + billId, 'GET').done(function (data) {
                $("#inputTitle").val(data.Title);
                $("#inputBillNo").val(data.BillNo);
                //$('#selFaultType').attr('optionsText', data.FaultTypeName);

                $("#inputApplyEmployee").val(data.ApplyEmployee);
                $("#inputBXEmployee").val(data.BXEmployee);
                $("#inputApplyDept").val(data.ApplyDept);
                $("#inputBXDate").val(data.BXDate.toString().replace('T', ' '));
                $("#inputPhone").val(data.Phone);
                $("#inputApplyEMail").val(data.EMail);
                $("#inputBXDept").val(data.BXDept);
                $("#inputAssetCode").val(data.AssetCode);
                $("#inputComputerName").val(data.ComputerName);
                $("#inputNote").val(data.Note);
                $("#inputPriority").val(data.PriorityId);
                $("#inputHopeTime").val(data.HopeTime.toString().replace('T', ' '));
                $("#inputPrevOperation").val(data.PrevOperation);
                //$("#inputImagePath").val(data.ImagePath);

                self.newRepairApplyBill.Title = data.Title;
                self.newRepairApplyBill.BillNo = data.BillNo;
                self.newRepairApplyBill.FaultType = data.FaultTypeId;
                self.newRepairApplyBill.ApplyEmployee = data.ApplyEmployee;
                self.newRepairApplyBill.BXEmployee = data.BXEmployee;
                self.newRepairApplyBill.NextEmployee = data.NextEmployee;
                self.newRepairApplyBill.BXDealTime = data.BXDealTime;
                self.newRepairApplyBill.Priority = data.PriorityId;
                self.newRepairApplyBill.PrevOperation = data.PrevOperation;
                //self.newRepairApplyBill.ImagePath = data.ImagePath;
                self.newRepairApplyBill.HopeTime = data.HopeTime;
                self.newRepairApplyBill.ApplyDept = data.ApplyDept;
                self.newRepairApplyBill.BXDate = data.BXDate.toString().replace('T', ' ');
                self.newRepairApplyBill.Phone = data.Phone;
                self.newRepairApplyBill.ApplyEMail = data.EMail;
                self.newRepairApplyBill.BXDept = data.BXDept;
                self.newRepairApplyBill.AssetCode = data.AssetCode;
                self.newRepairApplyBill.ComputerName = data.ComputerName;
                self.newRepairApplyBill.Note = data.Note;
                self.newRepairApplyBill.SatisfactionLevel = data.SatisfactionLevelId;
                self.newRepairApplyBill.Status = data.StatusId;
                self.newRepairApplyBill.Id = data.Id;

                ////self.detail(data);

                $('#inputDealer').val(sessionStorage.userName);
                self.newOnwayFlow.NextDealer = data.ApplyEmployee;
                self.newOnwayFlow.RepairApplyBillId = data.Id;
                self.newOnwayFlow.CurrentDealer = sessionStorage.userName;
            });
        }

            //填写满意度
        else if (billType == 4) {
            $('#spanDisplay').text('填写满意度');
            $('.whenDealLock').attr("disabled", true);
            $('.whenAddHide').hide();
            $('#dealModule').hide();
            $('#divDistribute').hide();
            ajaxHelper(repairApplyBillsUri + billId, 'GET').done(function (data) {

                $("#inputTitle").val(data.Title);
                $("#inputBillNo").val(data.BillNo);
                $("#inputFaultType").val(data.FaultTypeId);
                $("#inputApplyEmployee").val(data.ApplyEmployee);
                $("#inputBXEmployee").val(data.BXEmployee);
                $("#inputApplyDept").val(data.ApplyDept);
                $("#inputBXDate").val(data.BXDate.toString().replace('T', ' '));
                $("#inputPhone").val(data.Phone);
                $("#inputApplyEMail").val(data.EMail);
                $("#inputBXDept").val(data.BXDept);
                $("#inputAssetCode").val(data.AssetCode);
                $("#inputComputerName").val(data.ComputerName);
                $("#inputNote").val(data.Note);
                $("#inputPriority").val(data.PriorityId);
                $("#inputHopeTime").val(data.HopeTime.toString().replace('T', ' '));
                $("#inputPrevOperation").val(data.PrevOperation);
                //$("#inputImagePath").val(data.ImagePath);

                self.newRepairApplyBill.Title = data.Title;
                self.newRepairApplyBill.BillNo = data.BillNo;
                self.newRepairApplyBill.FaultType = data.FaultTypeId;
                self.newRepairApplyBill.ApplyEmployee = data.ApplyEmployee;
                self.newRepairApplyBill.BXEmployee = data.BXEmployee;
                self.newRepairApplyBill.NextEmployee = data.NextEmployee;
                self.newRepairApplyBill.BXDealTime = data.BXDealTime;
                self.newRepairApplyBill.Priority = data.PriorityId;
                self.newRepairApplyBill.PrevOperation = data.PrevOperation;
                //self.newRepairApplyBill.ImagePath = data.ImagePath;
                self.newRepairApplyBill.HopeTime = data.HopeTime;
                self.newRepairApplyBill.ApplyDept = data.ApplyDept;
                self.newRepairApplyBill.BXDate = data.BXDate.toString().replace('T', ' ');
                self.newRepairApplyBill.Phone = data.Phone;
                self.newRepairApplyBill.ApplyEMail = data.EMail;
                self.newRepairApplyBill.BXDept = data.BXDept;
                self.newRepairApplyBill.AssetCode = data.AssetCode;
                self.newRepairApplyBill.ComputerName = data.ComputerName;
                self.newRepairApplyBill.Note = data.Note;
                self.newRepairApplyBill.SatisfactionLevel = data.SatisfactionLevelId;
                self.newRepairApplyBill.Status = data.StatusId;
                self.newRepairApplyBill.Id = data.Id;
                //self.detail(data);

            });
        }

            //查看
        else if (billType == 5) {
            $('#spanDisplay').text('查看报修单');
            $('.whenDealLock').attr("disabled", true);
            $('input').attr("disabled", true);
            $('select').attr("disabled", true);
            $('#btnRepairBill').hide();
            $('#divOnwayFlow').hide();
            $('#divDistribute').hide();
            $('#dealModule').hide();
            ajaxHelper(repairApplyBillsUri + billId, 'GET').done(function (data) {
                $("#inputTitle").val(data.Title);
                $("#inputBillNo").val(data.BillNo);

                //$("#selFaultType").val(data.FaultTypeId);
                //$("#selFaultType").attr("value", data.FaultTypeId);
                $("#inputApplyEmployee").val(data.ApplyEmployee);
                $("#inputBXEmployee").val(data.BXEmployee);
                $("#inputApplyDept").val(data.ApplyDept);
                $("#inputBXDate").val(data.BXDate.toString().replace('T', ' '));
                $("#inputPhone").val(data.Phone);
                $("#inputApplyEMail").val(data.EMail);
                $("#inputBXDept").val(data.BXDept);
                $("#inputAssetCode").val(data.AssetCode);
                $("#inputComputerName").val(data.ComputerName);
                $("#inputNote").val(data.Note);
                $("#inputPriority").val(data.PriorityId);
                $("#inputHopeTime").val(data.HopeTime.toString().replace('T', ' '));
                $("#inputPrevOperation").val(data.PrevOperation);
                //$("#inputImagePath").val(data.ImagePath);
                $("#selSatisfactionLevels").val(data.SatisfactionLevelId);

                self.newRepairApplyBill.Title = data.Title;
                self.newRepairApplyBill.BillNo = data.BillNo;
                self.newRepairApplyBill.FaultType = data.FaultTypeId;
                self.newRepairApplyBill.ApplyEmployee = data.ApplyEmployee;
                self.newRepairApplyBill.BXEmployee = data.BXEmployee;
                self.newRepairApplyBill.NextEmployee = data.NextEmployee;
                self.newRepairApplyBill.BXDealTime = data.BXDealTime;
                self.newRepairApplyBill.Priority = data.PriorityId;
                self.newRepairApplyBill.PrevOperation = data.PrevOperation;
                //self.newRepairApplyBill.ImagePath = data.ImagePath;
                self.newRepairApplyBill.HopeTime = data.HopeTime;
                self.newRepairApplyBill.ApplyDept = data.ApplyDept;
                self.newRepairApplyBill.BXDate = data.BXDate.toString().replace('T', ' ');
                self.newRepairApplyBill.Phone = data.Phone;
                self.newRepairApplyBill.ApplyEMail = data.EMail;
                self.newRepairApplyBill.BXDept = data.BXDept;
                self.newRepairApplyBill.AssetCode = data.AssetCode;
                self.newRepairApplyBill.ComputerName = data.ComputerName;
                self.newRepairApplyBill.Note = data.Note;
                self.newRepairApplyBill.SatisfactionLevel = data.SatisfactionLevelId;
                self.newRepairApplyBill.Status = data.StatusId;
                self.newRepairApplyBill.Id = data.Id;

                $('#inputDealer').val(sessionStorage.userName);
                self.newOnwayFlow.NextDealer = data.ApplyEmployee;
                self.newOnwayFlow.RepairApplyBillId = data.Id;
                self.newOnwayFlow.CurrentDealer = sessionStorage.userName;
            });
        }
    }


    self.addRepairApplyBill = function (formElement) {
        //新增
        if (billType == 1) {
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
                ImagePath: sessionStorage.filePath,
                AssetCode: self.newRepairApplyBill.AssetCode(),
                Phone: self.newRepairApplyBill.Phone(),
                EMail: self.newRepairApplyBill.ApplyEMail,
                ComputerName: self.newRepairApplyBill.ComputerName(),
                SatisfactionLevelId: 1,
                StatusId: 1
            };
            ajaxHelper(repairApplyBillsUri, 'POST', repairApplyBill).done(function (item) {
                $('input').val('');
                $('textarea').val('');
                self.repairApplyBills.push(item);
            }).success(alert('提交成功！').location('RepairBillQuery.html?listType=5')
            );
        }
            //修改\填写满意度
        else if (billType == 2 || billType == 4) {
            var repairApplyBill = {
                BillNo: self.newRepairApplyBill.BillNo,
                Title: self.newRepairApplyBill.Title,
                Note: self.newRepairApplyBill.Note,
                FaultTypeId: self.newRepairApplyBill.FaultType,
                ApplyEmployee: self.newRepairApplyBill.ApplyEmployee,
                BXDealTime: self.newRepairApplyBill.BXDealTime,
                HopeTime: self.newRepairApplyBill.HopeTime,
                PriorityId: self.newRepairApplyBill.Priority,
                PrevOperation: self.newRepairApplyBill.PrevOperation,
                //ImagePath: self.newRepairApplyBill.ImagePath,
                NextEmployee: self.newRepairApplyBill.NextEmployee,
                ApplyDept: self.newRepairApplyBill.ApplyDept,
                BXEmployee: self.newRepairApplyBill.BXEmployee,
                BXDept: self.newRepairApplyBill.BXDept,
                BXDate: self.newRepairApplyBill.BXDate,
                AssetCode: self.newRepairApplyBill.AssetCode,
                Phone: self.newRepairApplyBill.Phone,
                EMail: self.newRepairApplyBill.ApplyEMail,
                ComputerName: self.newRepairApplyBill.ComputerName,
                SatisfactionLevelId: self.newRepairApplyBill.SatisfactionLevel.Id,
                StatusId: self.newRepairApplyBill.Status,
                Id: self.newRepairApplyBill.Id
            };
            var repairApplyBillsUri1 = '../api/repairApplyBills/' + billId;
            ajaxHelper(repairApplyBillsUri1, 'PUT', repairApplyBill).done().success(window.close());
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
            //ImagePath: self.newRepairApplyBill.ImagePath,
            NextEmployee: self.newRepairApplyBill.NextEmployee,
            BXDealEmployee: sessionStorage.userName,
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
        var repairApplyBillsUri1 = '../api/repairApplyBills/' + billId;
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
        }).success(alert('提交成功！')
        );


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
        $('#btnRepairBill').click(function () {
            var fileObj = $('#fileField').get(0).files[0];

            if (fileObj) {
                var fileSize = 0;
                if (fileObj.size > 1024 * 1024) fileSize = (Math.round(fileObj.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
                else fileSize = (Math.round(fileObj.size * 100 / 1024) / 100).toString() + 'KB';
                //alert('文件名：' + fileObj.name + ',文件大小：' + fileSize + ',文件类型：' + fileObj.type);
            }

            // FormData 对象
            var form = new FormData();
            form.append("file", fileObj);

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
            start = filePath.indexOf('|');
            end = filePath.indexOf('</string>');
            filePath = filePath.substring(start, end);
            sessionStorage.filePath = filePath;
        });

        $('#btnDistribute').click(function () {
            window.open('DealerSelect.html?level=1&id=' + billId, null, "dialogWidth:500;dialogHeight:auto;status:no;help:no;center:yes");
        });

        $('#btnDeal').click(function () {
            $('#dealModule').show();
            $('.whenAddHide').show();
            $('#inputDealNote').focus();

        });

        $('#linkHome').click(function () {
            location.href = "/IndexMobile.html";
        });

        $('#linkQry').click(function () {
            location.href = "../Page/RepairBillQueryMobile.html?listType=1";
        });

        $("#fileField").change(function () {
            var objUrl = getObjectURL(this.files[0]);
            console.log("objUrl = " + objUrl);
            if (objUrl) {
                $("#img0").attr("src", objUrl);
                $("#aImg").attr("href", objUrl);
            }
        });


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


