var ViewModel = function () {
    var self = this;
    self.repairApplyBills = ko.observableArray();
    self.curTime = ko.observable();
    self.error = ko.observable();
    self.satisfactionLevels = ko.observableArray();
    self.faultTypes = ko.observableArray();
    self.statuses = ko.observableArray();
    self.onwayFlows = ko.observableArray();
    self.detail = ko.observable();
    self.newRepairApplyBill = {
        BillNo: ko.observable(),
        Title: ko.observable(),
        Note: ko.observable(),
        FaultType: ko.observable(),
        Status: ko.observable(),
        ApplyEmployee: ko.observable(),
        BXDate: ko.observable(),
        BXEmployee: ko.observable(),
        AssetCode: ko.observable(),
        ComputerName: ko.observable(),
        Phone: ko.observable(),
        SatisfactionLevel: ko.observable()
    }

    var repairApplyBillsUri = '../api/repairApplyBills';
    var satisfactionLevelUri = '../api/satisfactionLevels/';
    var faultTypeUri = '../api/faultTypes/';
    var statusUri = '../api/statuses/';
    var onwayFlowUri = '../api/onwayFlows?$filter=';
    var loginUri = '../api/Login/';
    var exportUri = '../api/ExportData/'

    var recordCount = 0;//总记录数
    var pagesize = 15; //页面大小
    var pageCount = 0; //页数
    var pageIndex = 1; //页索引
    var filter = '';

    //生成下拉框的HTML
    function generateHtmlOption(dataArr, type) {
        var option = '<option value="' + "" + '">' + " " + '</option>';
        var arr = []; //定义一个数组
        var length = 0; //dataArr.length;
        switch (type) {
            case 'status':
            case 'faultType':
                $.each(dataArr, function (key, val) {
                    option = option + '<option value="' + val.Id + '">' + val.Name + '</option>';
                });
                break;
        }
        return option;
    }

    function getData(type) {
        var objList;
        ajaxHelper(faultTypeUri, 'GET').done(function (data) {
            objList = document.getElementById("selFaultType");
            objList.innerHTML = "";
            if (data.length > 0) {
                $("#selFaultType").append(generateHtmlOption(data, "faultType"));
            }
        });
        ajaxHelper(statusUri, 'GET').done(function (data) {
            objList = document.getElementById("selStatus");
            objList.innerHTML = "";
            if (data.length > 0) {
                $("#selStatus").append(generateHtmlOption(data, "status"));
            }
        });

    }

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

    function getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

    var listType = getQueryString("listType");

    //普通用户
    if (localStorage.userType == 0) {
        $('.deal').hide();
        //页面过滤条件
        localStorage.pageFilter = '';

        //查询报修单，查询所有的报修单
        if (listType == 1) {

            $('#aAdd').show();
            //if (localStorage.where == undefined)
            //    localStorage.filter = '';
            //else
            //    localStorage.filter = localStorage.where;

            localStorage.originalFilter = '';

        }
            //我的待处理，过滤出已处理完成，且满意度未填写的 
        else if (listType == 3) {
            $('#spanDisplay').text('我的待处理');
            //由入口携带的过滤条件
            localStorage.originalFilter = 'NextEmployee eq \'' + localStorage.userName + '\' and StatusId eq 2 and SatisfactionLevelId eq 1';

            //localStorage.filter = 'NextEmployee eq \'' + localStorage.userName + '\' and StatusId eq 2 and SatisfactionLevelId eq 1';
        }
            //我的报修单
        else if (listType == 5) {
            $('#spanDisplay').text('我的报修单');
            //$('#aDeal').hide();
            //$('#aEdit').hide();
            //$('#aDistribute').hide();
            //localStorage.filter = 'ApplyEmployee eq \'' + localStorage.userName + '\'';

            //由入口携带的过滤条件
            localStorage.originalFilter = 'ApplyEmployee eq \'' + localStorage.userName + '\'';
        }
    }

        //管理账号
    else {
        //页面过滤条件
        localStorage.pageFilter = '';

        //查询报修单，查询所有的报修单
        if (listType == 1) {
            //if (localStorage.where == undefined)
            //    localStorage.filter = '';
            //else
            //    localStorage.filter = localStorage.where;
            localStorage.originalFilter = '';
        }
            //我的待处理，过滤出状态为处理中的
        else if (listType == 3) {
            $('#spanDisplay').text('我的待处理');
            //localStorage.filter = 'NextEmployee eq \'' + localStorage.userName + '\' and StatusId eq 1 ';

            //由入口携带的过滤条件
            localStorage.originalFilter = 'NextEmployee eq \'' + localStorage.userName + '\' and StatusId eq 1 ';

        }
            //我的已处理，过滤出状态为已完成的
        else if (listType == 4) {
            $('#spanDisplay').text('我的已处理');
            $('#aDeal').hide();
            $('#aDistribute').hide();
            $('#aSatisfaction').hide();
            //localStorage.filter = 'BXDealEmployee eq \'' + localStorage.userName + '\' and StatusId eq 2 ';

            //由入口携带的过滤条件
            localStorage.originalFilter = 'BXDealEmployee eq \'' + localStorage.userName + '\' and StatusId eq 2 ';

        }
            //我的报修单
        else if (listType == 5) {
            $('#spanDisplay').text('我的报修单');
            $('#aDeal').hide();
            $('#aEdit').hide();
            $('#aDistribute').hide();
            //localStorage.filter = 'ApplyEmployee eq \'' + localStorage.userName + '\'';

            //由入口携带的过滤条件
            localStorage.originalFilter = 'ApplyEmployee eq \'' + localStorage.userName + '\'';
        }
    }

    //组装过滤条件
    function getFilter() {
        if (localStorage.pageFilter == undefined) {
            if (localStorage.originalFilter.length == 0)
                filter = '';
            else
                filter = localStorage.originalFilter;
        }
        else {
            if (localStorage.pageFilter.length == 0) {
                if (localStorage.originalFilter.length == 0)
                    filter = '';
                else
                    filter = localStorage.originalFilter;
            }
            else {
                if (localStorage.originalFilter.length == 0)
                    filter = localStorage.pageFilter;
                else
                    filter = localStorage.originalFilter + ' and ' + localStorage.pageFilter;
            }
        }
    }

    //查询我的报修单/我的待处理/我的已处理
    function getRepairApplyBills() {
        var arr = new Array;
        getFilter();

        arr.push(repairApplyBillsUri);
        if (filter.length > 0) {
            arr.push('?$filter=');
            arr.push(filter);
        }
        var uri = arr.join("");
        uri = encodeURI(uri);
        ajaxHelper(uri, 'GET').done(function (data) {
            localStorage.data = data;
            localStorage.recordCount = data.length;
            GetTotalCount();
            GetPageCount();
            bindPager();
        });
    }


    //按页码查询
    function getRepairApplyBillsByPage() {
        self.onwayFlows('');
        var arr = new Array;
        getFilter();
        arr.push(repairApplyBillsUri);
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
        ajaxHelper(uri, 'GET').done(function (data) {
            self.repairApplyBills(data);

            //单击记录显示当前在途流程
            $("#mytable tr").bind("click", function () {
                if ($(this).find('span[id=Status]').text() == '处理中') $('#aSatisfaction').hide();

                else $('#aSatisfaction').show();
                getOnwayFlowsByFilter($(this).find('span[id=BillNo]').text());
            });

            $("#mytable tr").bind("dblclick", function () {
                var billId = $(this).find('span[id=Id]').text();
                window.open('RepairBill.html?billType=5&id=' + billId);
            });

            //$("#mytable tr #linkDeal").bind("click", function () {
            //    var billId = 0;
            //    billId = $(this).find('span[id=Id]').text();
            //    window.open('RepairBill.html?billType=3&id=' + billId);
            //});
        });
    }

    //根据过滤条件查询
    self.getRepairApplyBillsByFilter = function () {
        //var faultTypeId = self.newRepairApplyBill.FaultType().Id;
        //var statusId = self.newRepairApplyBill.Status().Id;

        var faultTypeId = $('#selFaultType').val();
        var statusId = $('#selStatus').val();

        var bxEmployee = $('#inputBXEmployee').val();
        var billNo = $('#inputBillNo').val();
        var beginDate = $('#inputBeginTime').val();
        var endDate = $('#inputEndTime').val();

        localStorage.pageFilter = 'Id eq Id';

        if (faultTypeId.length > 0) localStorage.pageFilter = localStorage.pageFilter + ' and FaultTypeId eq ' + faultTypeId;
        if (statusId.length > 0) localStorage.pageFilter = localStorage.pageFilter + ' and StatusId eq ' + statusId;

        //if (faultTypeId == 10000) {
        //    localStorage.pageFilter = ' StatusId eq ' + statusId;
        //}
        //else {
        //    localStorage.pageFilter = ' FaultTypeId eq ' + faultTypeId + ' and StatusId eq ' + statusId;
        //}
        if (bxEmployee.length > 0) localStorage.pageFilter = localStorage.pageFilter + ' and substringof(\'' + bxEmployee + '\',BXEmployee)';
        if (billNo.length > 0) localStorage.pageFilter = localStorage.pageFilter + ' and endswith(BillNo, \'' + billNo + '\')';
        if (beginDate.length > 0) localStorage.pageFilter = localStorage.pageFilter + ' and BXDate ge DateTime\'' + beginDate + 'T00:00:00\'';
        if (endDate.length > 0) localStorage.pageFilter = localStorage.pageFilter + ' and BXDate le DateTime\'' + endDate + 'T23:59:59\'';

        getRepairApplyBills();
        getRepairApplyBillsByPage();

    }

    self.getRepairApplyBillDetail = function (item) {
        ajaxHelper(repairApplyBillsUri + item.Id, 'GET').done(function (data) {
            self.detail(data);
        });
    }

    function getAllSatisfactionLevels() {
        ajaxHelper(satisfactionLevelUri, 'GET').done(function (data) {
            self.satisfactionLevels(data);
        });
    }

    function getAllFaultTypes() {
        ajaxHelper(faultTypeUri, 'GET').done(function (data) {
            self.faultTypes(data);
        });
    }

    function getAllStatuses() {
        ajaxHelper(statusUri, 'GET').done(function (data) {
            self.statuses(data);
        });
    }

    function getOnwayFlowsByFilter(billNo) {
        onwayFlowUri = '../api/onwayFlows?$filter=RepairAppyBillNo eq \'' + billNo + '\'';
        ajaxHelper(onwayFlowUri, 'GET').done(function (data) {
            self.onwayFlows(data);
        });
    }

    //getLoginUserInfo();
    //getAllFaultTypes();
    getData();

    getAllSatisfactionLevels();
    getAllStatuses();
    getRepairApplyBills();
    getRepairApplyBillsByPage();

    $(function () {
        //第一页按钮click事件
        $("#first").click(function () {
            pageIndex = 1;
            $("#lblCurrent").text(1);
            getRepairApplyBillsByPage();
        });
        //上一页按钮click事件
        $("#previous").click(function () {
            if (pageIndex != 1) {
                pageIndex--;
                $("#lblCurrent").text(pageIndex);
            }
            getRepairApplyBillsByPage();
        });
        //下一页按钮click事件
        $("#next").click(function () {
            var pageCount = $("#lblPageCount").text();
            if (pageIndex != pageCount) {
                pageIndex++;
                $("#lblCurrent").text(pageIndex);
            }
            getRepairApplyBillsByPage();
        });
        //最后一页按钮click事件
        $("#last").click(function () {
            var pageCount = $("#lblPageCount").text();
            pageIndex = pageCount;
            getRepairApplyBillsByPage();
        });

        //查询
        $("#btnSearch").click(function () {
            where = " where 1=1";
            var csbh = $("#txtCSBH").val();
            if (csbh != null && csbh != NaN) {
                pageIndex = 1;
                where += " and csbh like '%" + csbh + "%'";
            }
            getRepairApplyBillsByPage();
        });


        //编辑
        $('#aEdit').click(function () {
            var $table = $("#mytable");//
            var $trs = $table.find("tr");
            var selCount = 0
            var billId = 0;
            for (var i = 0; i < $trs.length - 1; i++) {
                //循环获取每一行
                var check = $("input[type='checkbox']");
                var id = $("span[id='Id']");
                if (check.eq(i)[0].checked == true) {
                    selCount = selCount + 1;
                    billId = id.eq(i).text();
                }
            }

            if (selCount == 0) alert('请先勾选一张报修单再进行修改！');
            else if (selCount > 1) {
                alert('一次只能选择一张报修单进行修改！');
            }

            else {
                window.open('RepairBill.html?billType=2&id=' + billId);
            }
        });

        //查看
        $('#aView').click(function () {
            var $table = $("#mytable");//
            var $trs = $table.find("tr");
            var selCount = 0
            var billId = 0;
            for (var i = 0; i < $trs.length - 1; i++) {
                //循环获取每一行
                var check = $("input[type='checkbox']");
                var id = $("span[id='Id']");
                if (check.eq(i)[0].checked == true) {
                    selCount = selCount + 1;
                    billId = id.eq(i).text();
                }
            }

            if (selCount == 0) alert('请先勾选一张报修单再查看！');
            else if (selCount > 1) {
                alert('一次只能选择一张报修单查看！');
            }

            else {
                //window.open('RepairBill.html?billType=5&id=' + billId, '查看报修单', "dialogWidth:1024px;dialogHeight:768px;status:no;help:no;location:no;");
                window.open('RepairBill.html?billType=5&id=' + billId);
            }
        });

        //满意度填写
        $('#aSatisfaction').click(function () {
            var $table = $("#mytable");//
            var $trs = $table.find("tr");
            var selCount = 0
            var billId = 0;
            for (var i = 0; i < $trs.length - 1; i++) {
                //循环获取每一行
                var check = $("input[type='checkbox']");
                var id = $("span[id='Id']");
                if (check.eq(i)[0].checked == true) {
                    selCount = selCount + 1;
                    billId = id.eq(i).text();
                }
            }

            if (selCount == 0) alert('请先勾选一张报修单再填写满意度！');
            else if (selCount > 1) {
                alert('一次只能选择一张报修单填写满意度！');
            }
            else {
                //window.open('RepairBill.html?billType=4&id=' + billId, null, "dialogWidth:1024px;dialogHeight:768px;status:no;help:no;location:no;");
                window.open('RepairBill.html?billType=4&id=' + billId);
            }


        });

        //处理
        $('#aDeal').click(function () {
            var $table = $("#mytable");//
            var $trs = $table.find("tr");
            var selCount = 0
            var billId = 0;
            for (var i = 0; i < $trs.length - 1; i++) {
                //循环获取每一行
                var check = $("input[type='checkbox']");
                var id = $("span[id='Id']");
                if (check.eq(i)[0].checked == true) {
                    selCount = selCount + 1;
                    billId = id.eq(i).text();
                }
            }

            if (selCount == 0) alert('请先勾选一张报修单再进行处理！');
            else if (selCount > 1) {
                alert('一次只能选择一张报修单进行处理！');
            }
            else {
                //window.open('RepairBill.html?billType=3&id=' + billId, null, "dialogWidth:500;dialogHeight:inherit;status:no;help:no;location:no;");
                window.open('RepairBill.html?billType=3&id=' + billId);
            }

        });


        //转发
        $('#aDistribute').click(function () {
            var $table = $("#mytable");//
            var $trs = $table.find("tr");
            var selCount = 0
            var billId = 0;
            for (var i = 0; i < $trs.length - 1; i++) {
                //循环获取每一行
                var check = $("input[type='checkbox']");
                var id = $("span[id='Id']");
                if (check.eq(i)[0].checked == true) {
                    selCount = selCount + 1;
                    billId = id.eq(i).text();
                }
            }
            if (selCount == 0) alert('请先勾选一张报修单再进行转发！');
            else {
                window.open('DealerSelect.html?level=1&id=' + billId, null, "dialogWidth:300;dialogHeight:auto;status:no;help:no;center:yes;location:no;");
            }
        });

        //导出
        $('#aExportData').click(function () {
            var filter = {
                BeginTime: $('#inputBeginTime').val(),
                EndTime: $('#inputEndTime').val(),
                BillNo: $('#inputBillNo').val()
            };
            ajaxHelper(exportUri, 'POST', filter).done(function (data) {
                window.open('../Export/' + data.FileName);
            }).success(function () {
                //alert('提交成功！');
            });
        });

    })

    // 页脚属性设置
    function bindPager() {
        //填充分布控件信息
        var pageCount = $("#lblPageCount").text(); //总页数
        if (pageCount == 0) {
            $("#lblCurrent").text(0);
        }
        else {
            if (pageIndex > pageCount) {
                $("#lblCurrent").text(1);
            }
            else {
                $("#lblCurrent").text(pageIndex); //当前页
            }
        }
        //$("#first").attr("disabled", (pageIndex == 1 || $("#lblCurrent").text() == "0") ? true : false);
        //$("#previous").attr("disabled", (pageIndex <= 1 || $("#lblCurrent").text() == "0") ? true : false);
        //$("#next").attr("disabled", (pageIndex >= pageCount) ? true : false);
        //$("#last").attr("disabled", (pageIndex == pageCount || $("#lblCurrent").text() == "0") ? true : false);
    }
    //总页数
    function GetPageCount() {
        $('#lblPageCount').text(Math.ceil(localStorage.recordCount / pagesize));
    }
    //总记录数
    function GetTotalCount() {
        $('#lblToatl').text(localStorage.recordCount);
    }
};

ko.applyBindings(new ViewModel());












