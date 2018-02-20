var isTransfer = 0;
var ViewModel = function () {
    var self = this;
    self.flowConfigs = ko.observableArray();
    self.satisfactionLevels = ko.observableArray();
    self.superUsers = ko.observableArray();
    self.faultTypes = ko.observableArray();
    self.onwayFlows = ko.observableArray();
    self.error = ko.observable();
    self.detail = ko.observable();
    self.newFlowConfig = {
        Level: ko.observable(),
        Dealer: ko.observable(),
        Note: ko.observable()
        //EMail: ko.observable(),
        //Mobile: ko.observable()

    }

    self.newSatisfactionLevel = {
        Level: ko.observable(),
        Note: ko.observable()
    }

    self.newFaultType = {
        Name: ko.observable(),
        Note: ko.observable()
    }

    self.newSuperUser = {
        UserName: ko.observable()
    }
    localStorage.status = 'add';
    localStorage.itemId = 0;

    var flowConfigsUri = '../api/flowConfigs/';
    var satisfactionLevelUri = '../api/satisfactionLevels/';
    var onwayFlowUri = '../api/onwayFlows';
    var faultTypesUri = '../api/faultTypes/';
    var superUsersUri = '../api/superUsers/'

    //获取系统当前时间，赋予报修时间字段
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
    if (millisecond < 10)
        millisecond = '00' + millisecond;
    else if (millisecond < 100)
        millisecond = '0' + millisecond;
    var time = year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second + "." + millisecond;


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
        }).fail(
        function (jqXHR, textStatus, errorThrown) {
            //self.error(errorThrown);
            alert('Error: ' + errorThrown);
        });
    }


    $(function () {
        $("#aFlowConfig").click(function () {
            $("#divSatisfactionLevel").hide();
            $("#divFlowConfig").fadeIn("slow");
            $('#divFaultType').hide();
            $('#divSuperUser').hide();


        });
        $("#aSatisfactionLevel").click(function () {
            $('#divFaultType').hide();
            $("#divFlowConfig").hide();
            $("#divSatisfactionLevel").fadeIn("slow");
            $('#divSuperUser').hide();

        });
        $("#aFaultType").click(function () {
            $('#divFaultType').fadeIn("slow");
            $("#divFlowConfig").hide();
            $("#divSatisfactionLevel").hide();
            $('#divSuperUser').hide();
        });
        $("#aSuperUser").click(function () {
            $('#divFaultType').hide();
            $("#divFlowConfig").hide();
            $("#divSatisfactionLevel").hide();
            $('#divSuperUser').fadeIn("slow");

        });

    })


    function getAllFlowConfigs() {
        ajaxHelper(flowConfigsUri, 'GET').done(function (data) {
            self.flowConfigs(data);
        });
    }

    function getAllSatisfactionLevels() {
        ajaxHelper(satisfactionLevelUri, 'GET').done(function (data) {
            self.satisfactionLevels(data);
        });
    }

    function getAllFaultTypes() {
        ajaxHelper(faultTypesUri, 'GET').done(function (data) {
            self.faultTypes(data);
        });
    }

    function getAllSuperUsers() {
        ajaxHelper(superUsersUri, 'GET').done(function (data) {
            self.superUsers(data);
        });
    }
    localStorage.billId = getQueryString("id");

    function getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;

    }

    self.getFlowConfigDetail = function (item) {
        ajaxHelper(flowConfigsUri + item.Id, 'GET').done(function (data) {
            var Level = data.Level;
            var Dealer = data.Dealer;
            var Note = data.Note;
            //var EMail = data.EMail;
            //var Mobile = data.Mobile;

            $("#inputFLevel").val(Level);
            $("#inputFDealer").val(Dealer);
            $("#inputFNote").val(Note);
            //$("#inputFEMail").val(EMail);
            //$("#inputFMobile").val(Mobile);

            self.newFlowConfig.Level = Level;
            self.newFlowConfig.Dealer = Dealer;
            self.newFlowConfig.Note = Note;
            //self.newFlowConfig.EMail = EMail;
            //self.newFlowConfig.Mobile = Mobile;

            $('#btnFlowconfig').text('保存');
            localStorage.status = 'edit';
            localStorage.itemId = item.Id;

        });
    }

    self.getFaultTypeDetail = function (item) {
        ajaxHelper(faultTypesUri + item.Id, 'GET').done(function (data) {
            var Name = data.Name;
            var Note = data.Note;
            $("#inputTName").val(Name);
            $("#inputTNote").val(Note);
            self.newFaultType.Name = Name;
            self.newFaultType.Note = Note;

            $('#btnFaultType').text('保存');
            localStorage.status = 'edit';
            localStorage.itemId = item.Id;
        });

    }

    self.getSatisfactionLevelDetail = function (item) {
        ajaxHelper(satisfactionLevelUri + item.Id, 'GET').done(function (data) {
            var Name = data.Name;
            var Level = data.Level;
            $("#inputSName").val(Name);
            $("#inputSLevel").val(Level);
            self.newSatisfactionLevel.Name = Name;
            self.newSatisfactionLevel.Level = Level;

            $('#btnSatisfactionLevel').text('保存');
            localStorage.status = 'edit';
            localStorage.itemId = item.Id;
        });

    }

    //删除故障类型
    self.deleteFaultType = function (item) {
        ajaxHelper(faultTypesUri + item.Id, 'DELETE').done(function () {
            self.faultTypes.pop(item);
        });
    }

    //删除流程配置
    self.deleteFlowConfig = function (item) {
        ajaxHelper(flowConfigsUri + item.Id, 'DELETE').done(function () {
            self.flowConfigs.pop(item);
        });
    }

    //删除满意度定义
    self.deleteSatisfactionLevel = function (item) {
        ajaxHelper(satisfactionLevelUri + item.Id, 'DELETE').done(function () {
            self.satisfactionLevels.pop(item);
        });
    }

    //删除管理账号
    self.deleteSuperUser = function (item) {
        ajaxHelper(superUsersUri + item.Id, 'DELETE').done(function () {
            self.superUsers.pop(item);
        });
    }


    self.addFlowConfig = function (formElement) {
        if (localStorage.status == 'add') {
            var flowConfig = {
                Level: self.newFlowConfig.Level(),
                Dealer: self.newFlowConfig.Dealer(),
                Note: self.newFlowConfig.Note()
                //EMail: self.newFlowConfig.EMail(),
                //Mobile: self.newFlowConfig.Mobile()
            };

            ajaxHelper(flowConfigsUri, 'POST', flowConfig).done(function (item) {
                self.flowConfigs.push(item);
            });
        }
        else if (localStorage.status == 'edit') {
            var flowConfig = {
                Id: localStorage.itemId,
                Level: self.newFlowConfig.Level,
                Dealer: self.newFlowConfig.Dealer,
                Note: self.newFlowConfig.Note
                //EMail: self.newFlowConfig.EMail,
                //Mobile: self.newFlowConfig.Mobile
            };

            ajaxHelper(flowConfigsUri + localStorage.itemId, 'PUT', flowConfig).done(function (item) {
                self.flowConfigs.push(item);
            });
        }
    }

    self.addFaultType = function (formElement) {

        if (localStorage.status == 'add') {
            var faultType = {
                Name: self.newFaultType.Name(),
                Note: self.newFaultType.Note()
            };
            ajaxHelper(faultTypesUri, 'POST', faultType).done(function (item) {
                self.faultTypes.push(item);
            });
        }
        else if (localStorage.status == 'edit') {
            var faultType = {
                Id: localStorage.itemId,
                Name: self.newFaultType.Name,
                Note: self.newFaultType.Note
            };

            ajaxHelper(faultTypesUri + localStorage.itemId, 'PUT', faultType).done(function (item) {
                self.faultTypes.pop(item);
                self.faultTypes.push(item);
            });
        }
    }

    self.addSatisfactionLevel = function (formElement) {
        if (localStorage.status == 'add') {
            var satisfactionLevel = {
                Name: self.newSatisfactionLevel.Name,
                Level: self.newSatisfactionLevel.Level
            };
            ajaxHelper(satisfactionLevelUri, 'POST', satisfactionLevel).done(function (item) {
                self.satisfactionLevels.push(item);
            });
        }
        else if (localStorage.status == 'edit') {
            var satisfactionLevel = {
                id: localStorage.itemId,
                Name: self.newSatisfactionLevel.Name,
                Level: self.newSatisfactionLevel.Level
            };
            ajaxHelper(satisfactionLevelUri + localStorage.itemId, 'PUT', satisfactionLevel).done(function (item) {
                self.satisfactionLevels.push(item);
            });
        }
    }

    self.addSuperUser = function (formElement) {
        var superUser = {
            PermLevl: 1,
            UserName: self.newSuperUser.UserName()
        }
        ajaxHelper(superUsersUri, 'POST', superUser).done(function (item) {
            self.superUsers.push(item);
        });

    }

    $(function () {
        $('#aAddFaultType').click(function () {
            $('input').val('');
            $('#btnFaultType').text('提交');
            //状态为新增
            localStorage.status = 'add';
        });

        $('#aAddFlowconfig').click(function () {
            $('input').val('');
            $('#btnFlowconfig').text('提交');
            //状态为新增
            localStorage.status = 'add';
        });

        $('#aAddSatisfactionLevel').click(function () {
            $('input').val('');
            $('#btnSatisfactionLevel').text('提交');
            //状态为新增
            localStorage.status = 'add';
        });

        $('#inputFDealer').change(function () {
            var userName = $('#inputFDealer').val();

        });

        $('#btnFlow').click(function () {
            var $table = $("#mytable");//
            var $trs = $table.find("tr");
            var selCount = 0
            var dealer = '';
            for (var i = 0; i < $trs.length - 3; i++) {
                //循环获取每一行
                var check = $("input[type='checkbox']");
                var spanDealer = $("span[id='spanDealer']");
                if (check.eq(i)[0].checked == true) {
                    selCount = selCount + 1;
                    dealer = spanDealer.eq(i).text();
                }
            }

            if (selCount == 0) alert('请先勾选一个处理人再进行转发！');
            else if (selCount > 1) {
                alert('一次只能选择一个处理人进行转发！');
            }
            else {
                //var onwayFlow = {
                //    CurrentDealer: dealer,
                //    NextDealer: '',
                //    DealDate: '9999/12/30',
                //    DealNote: '',
                //    RepairAppyBillId: localStorage.billId,
                //    DealMethodId: 4
                //};
                if (isTransfer == 0) {
                    var onwayFlow = {
                        CurrentDealer: localStorage.userName,
                        NextDealer: dealer,
                        DealDate: time,
                        DealNote: $('#inputDealNote').val(),
                        RepairAppyBillId: localStorage.billId,
                        DealMethodId: 3
                    };
                    //ajaxHelper(onwayFlowUri, 'POST', onwayFlow).done(function (item) {
                    //    self.onwayFlows.push(item);
                    //}).done().success(function () {
                    //    isTransfer = 1;

                    //});
                    //window.close();

                    ajaxHelper(onwayFlowUri, 'POST', onwayFlow).done(function (data, status) {
                        if (status == 'success') {                    
                            window.close();
                        }
                    });


                }
            }

        });

    });

    //Fetch the initial data.
    getAllFlowConfigs();
    getAllSatisfactionLevels();
    getAllFaultTypes();
    getAllSuperUsers();

};


ko.applyBindings(new ViewModel());