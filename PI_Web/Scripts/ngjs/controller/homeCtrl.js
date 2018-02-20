app = angular.module('app.controllers')
app.controller('myCtrl', ['$scope', "$http", "$timeout", 'SomeService', 'dataServices', 'CAPI', function ($scope, $http, $timeout, SomeService, dataServices, CAPI) {
    // 定义
    // 操作符定义
    $scope.operation1 = [
        { id: '1', op: '+' },
        { id: '2', op: '-' },
        { id: '3', op: '×' },
        { id: '4', op: '÷' }
    ];
    $scope.operation2 = [
        { id: '1', op: '≥' },
        { id: '2', op: '＞' },
        { id: '3', op: '≤' },
        { id: '4', op: '＜' },
        { id: '5', op: '=' }
    ];
    //指标绑定
    $scope.latas = [];
    $scope.techs = [];
    $scope.markets = [];
    //财务
    $scope.finAnalys = [];
    $scope.BanlaceSheets = [];
    $scope.ProfitSheets = [];
    $scope.CashFlows = [];
    //板块
    $scope.ConceptPs = [];
    $scope.MarketPs = [];
    $scope.RegionPs = [];
    $scope.TradePs = [];
    $scope.solutions = [];
    //获取指标
    dataServices.getLataData(
        function success(data) {
            $scope.latas = data;
        },
        function fail(message) {
        }
    );
    dataServices.getTechData(
        function success(data) {
            $scope.techs = data;
        },
        function fail(message) {
        }
    );
    dataServices.getMarketData(
        function success(data) {
            $scope.markets = data;
        },
        function fail(message) {
        }
    );
    dataServices.getFinAnalysisData(
        function success(data) {
            $scope.finAnalys = data;
        },
        function fail(message) {
        }
    );
    dataServices.getBanlaceSheetData(
        function success(data) {
            $scope.BanlaceSheets = data;
        },
        function fail(message) {
        }
    );
    dataServices.getProfitSheetData(
        function success(data) {
            $scope.ProfitSheets = data;
        },
        function fail(message) {
        }
    );
    dataServices.getCashFlowData(
        function success(data) {
            $scope.CashFlows = data;
        },
        function fail(message) {
        }
    );
    dataServices.getConceptPData(
        function success(data) {
            $scope.ConceptPs = data;
        },
        function fail(message) {
        }
    );
    dataServices.getMarketPData(
        function success(data) {
            $scope.MarketPs = data;
        },
        function fail(message) {
        }
    );
    dataServices.getRegionPData(
        function success(data) {
            $scope.RegionPs = data;
        },
        function fail(message) {
        }
    );
    dataServices.getTradePData(
        function success(data) {
            $scope.TradePs = data;
        },
        function fail(message) {
        }
    );
    //获取方案并且显示方案名
    SomeService.getSoluName(function (data) {
        if (data) {
            for (var i = 0, j = data.length; i < j; i++) {
                data[i].Name = data.length - i + "、" + data[i].Name;
            };
            $scope.solutions = data;
        }
    }, function (error) {
    });




    //搜索框获取数据以及键盘上下键的选择
    $scope.dataScr = [];
    $scope.name = "";
    $scope.now = 0;
    var timer = null;
    var inputName = $scope.name
    $scope.change = function (ev) {
        var seaDataArr = [];
        if (ev.keyCode == 38 || ev.keyCode == 40) return;
        $scope.name = $("#indexName").val();
        if (!($scope.name == "" || $scope.name.length == 0)) {
            if (null !== timer) { $timeout.cancel(timer); }
            timer = $timeout(function () {
                $timeout.cancel(timer); timer = null;
                $http({
                    method: "get",
                    url: CAPI.api + "/Index/Lite/" + $scope.name + "",
                    /*url:"http://10.1.135.80:9999/Index/Lite?$filter=substringof('"+$scope.name+"',Name)",*/
                }).success(function (data) {
                    //console.log(data);
                    if (data) {
                        angular.forEach(data, function (value, key) {
                            var searchDataObj = {};
                            searchDataObj.name = value.Name;
                            searchDataObj.code = value.Code;
                            this.push(searchDataObj);
                        }, seaDataArr);
                        //console.log(seaDataArr);
                        $scope.dataScr = seaDataArr;
                    }

                    // $scope.dataScr=data;
                    // console.log($scope.dataScr);
                    if ($scope.dataScr == [] || $scope.dataScr.length == 0) {
                        $("#nameBody").hide();
                    } else {
                        $("#nameBody").show();
                        $scope.now = 0;
                    };
                })
                    .error(function () {
                    });

            }, 100);

        } else {
            $("#nameBody").hide();
            $scope.dataScr.length = 0;
        }
    };
    var isMOuse = true;
    $scope.changeUpDown = function (ev) {
        // console.log(isMOuse);
        if ($scope.dataScr == [] || $scope.dataScr.length == 0) { return false; }
        isMOuse = false;

        if (!isMOuse) {
            var self = this;
            if (ev.keyCode == 38) {
                ev.preventDefault();
                //console.log(self.now);
                self.now--;
                if (self.now == -1) {
                    // console.log(self.dataScr.length);
                    self.now = self.dataScr.length - 1;
                }
                self.name = self.dataScr[self.now].name;
            };
            if (ev.keyCode == 40) {
                ev.preventDefault();
                //console.log(self.now);
                self.now++;
                if (self.now == self.dataScr.length) {
                    self.now = 0;
                }
                self.name = self.dataScr[self.now].name;
            };
        }
        isMOuse = true;

    }
    $scope.changeMouse = function (index) {
        isMOuse = true;
        //console.log(isMOuse);
        if (isMOuse) {
            $scope.now = index;
            $scope.name = $scope.dataScr[index].name;
        }
        isMOuse = false;
        //console.log(isMOuse);
    };
    $("#clearBtn").click(function () {
        $("#indexName").val("");
        $(this).css("display", "none");
        $("#nameBody").css("display", "none");
        $scope.name = "";
        $scope.dataScr == [];
        $scope.dataScr.length = 0;
    });
    //已选条件模块
    /*$scope.choiceItem={};*/
    $scope.choiceItems = [];//用户已选择的标准
    $scope.resultList = []; //搜索结果
    $scope.resultLenght = 0;//每次选股结果股票数量
    $scope.resultLenghtAll = 0;//选股结果总条数
    $scope.selfItems = [];//用户自定义的标准
    $scope.choToStock = {
        IndexRangeConditions: $scope.choiceItems,
        CustomIndexConditions: $scope.selfItems
    };

    $scope.defaultItem = {};//自定义的默认状态
    $scope.defaultItem = {
        first: {},
        firstInputVal: '',
        operator1: '1',
        operator1Name: '+',
        second: {},
        secondInputVal: '',
        operator2: '1',
        operator2Name: '≥',
        third: {},
        thirdInputVal: '',
        isFirstRo: false,
        isSecondRo: false,
        isThirdRo: false
    };//用户自定义
    // console.log($scope.defaultItem.first.name);
    //console.log($scope.defaultItem.secondInputVal);
    /*自定义选股框无值时显示加号背景图*/

    $scope.choseSign = function (item, type) {
        item = angular.copy(item);
        if (type === 1) {
            $scope.defaultItem.operator1 = item.id;
            $scope.defaultItem.operator1Name = item.op;
        } else if (type === 2) {
            $scope.defaultItem.operator2 = item.id;
            $scope.defaultItem.operator2Name = item.op;
        }
    };
    /*模拟下拉列表--选择时间*/
    // 交互控制
    var objTime;
    var classReg = new RegExp("divselect");
    $("#rightBody").on("click", ".divselect", function (e) {
        if ($(this).parent().className == "speSec") {
            objTime = $(this).parent().parent().parent().parent();
        } else {
            objTime = $(this).parent().parent().parent();
        };
        if (e.target.tagName.toLowerCase() == "span") {
            $(".divselect").not($(this)).find("ul").hide();
            $(this).find("ul").toggle(1);
        } else if (e.target.tagName.toLowerCase() == "li") {
            $(this).find("span").eq(0).text($(e.target).text());
            $(this).find("ul").hide();
        };
    });
    /*已选条件下拉列表 加入Enter事件*/
    // 按键事件
    $scope.enterChoItem = {};
    $scope.enterChose = function (parentItem) {
        $scope.enterChoItem = parentItem;

    };
    var liNum = 0;
    $("#rightBody").on("mouseenter", ".enterCho", function () {
        liNum = $(this).index();
        $(this).addClass("actLi");
        if ($(this).closest("ul").attr("class")) {
            //console.log(333);
            var selEnter = $(this).closest("ul").attr("class");
        };
        var parent = $(this).parent().parent();
        var len = parent.find("li").length;
        // 新增点击事件
        // $(this).click(function(){
        // 	if (liNum>1) {
        // 		sliderFlag=false;
        // 	};
        // });
        // 上下左右键,分别是38,40,37,39;   13 enter 键   sliderFlag 控制组合插件显示、隐藏
        $(document).keydown(function (event) {
            var e = event || window.event;
            var k = e.keyCode || e.which || e.charCode;
            if (k == 38) {
                if (liNum > 0) {
                    liNum--;
                } else {
                    liNum = 0;
                };
                parent.find("li").removeClass("actLi");
                parent.find("li").eq(liNum).addClass("actLi");
                return false;
            } else if (k == 40) {
                if (liNum < len - 1) {
                    liNum++;
                } else {
                    liNum = len - 1;
                };
                parent.find("li").removeClass("actLi");
                parent.find("li").eq(liNum).addClass("actLi");
                return false;
            } else if (k == 13) {
                $(".divselect ul").hide();
                parent.find("span").eq(0).text(parent.find("li").eq(liNum).text());
                var enterCont = (parent.find("li").eq(liNum).text())
                if (selEnter == "op1Enter") {
                    $scope.defaultItem.operator1 = liNum + 1;
                    $scope.defaultItem.operator1Name = enterCont;
                    $scope.$apply($scope.defaultItem);
                    return false;
                } else if (selEnter == "op2Enter") {
                    $scope.defaultItem.operator2 = liNum + 1;
                    $scope.defaultItem.operator2Name = enterCont;
                    $scope.$apply($scope.defaultItem);
                    return false;
                } else {
                    $scope.enterChoItem.selectTermDisplay = enterCont;
                    $scope.$apply($scope.choiceItems);
                    if (selEnter == "defEnter") {
                        return false;
                    };
                    var params = $scope.enterChoItem.code;
                    var strTime = $scope.enterChoItem.selectTermList[liNum].SelectItem;
                    $.ajax({
                        url: CAPI.api + "/Index/Detail?$filter=Code eq '" + params + "' and SelectTerm eq '" + strTime + "'",
                        type: "GET",
                        dataType: 'json',
                        success: function (data) {
                            if (data.length > 0) {
                                // console.log(data,"enter");
                                var arrValue = data[0].Value;
                                $scope.enterChoItem.value = arrValue;
                                $scope.$apply($scope.choiceItems);
                                //console.log($scope.choiceItems);
                                addSlider({
                                    wrap: objTime,
                                    range_min: getNum(arrValue, false) * 100,
                                    range_max: getNum(arrValue, true) * 100,
                                    cur_min: getNum(arrValue, false) * 100,
                                    cur_max: getNum(arrValue, true) * 100
                                });
                                drawMap({
                                    can: objTime.find(".canvas")[0],
                                    data: arrValue,
                                    len: 100,
                                    xSpa: 5,
                                    ySpa: 5,
                                    color: "#FDB9B9"
                                });
                            };
                        }
                    });

                }

            };
        });
    });
    $("#rightBody").on("mouseleave", "li", function () {
        $(this).removeClass("actLi");
        $(document).off("keydown");
    });
    //已选条件下拉列表-点击事件
    $scope.choseTime = function (item, parentItem) {
        item = angular.copy(item);
        //console.log(item);
        parentItem.selectTermDisplay = item.SelectDisplay;
        parentItem.selectTerm = item.SelectItem;
        //console.log(parentItem);
        var params = parentItem.code;
        //console.log(params);
        var strTime = parentItem.selectTerm;
        $.ajax({
            url: CAPI.api + "/Index/Detail?$filter=Code eq '" + params + "' and SelectTerm eq '" + strTime + "'",
            type: "GET",
            dataType: 'json',
            success: function (data) {
                if (data.length > 0) {
                    // console.log(data,"click");
                    var arrValue = data[0].Value;
                    //console.log(arrValue);
                    parentItem.value = arrValue;
                    $scope.$apply($scope.choiceItems);
                    //console.log($scope.choiceItems);
                    addSlider({
                        wrap: objTime,
                        range_min: getNum(arrValue, false) * 100,
                        range_max: getNum(arrValue, true) * 100,
                        cur_min: getNum(arrValue, false) * 100,
                        cur_max: getNum(arrValue, true) * 100
                    });
                    drawMap({
                        can: objTime.find(".canvas")[0],
                        data: arrValue,
                        len: 100,
                        xSpa: 5,
                        ySpa: 5,
                        color: "#FDB9B9"
                    });
                };
            }
        });
    };
    //自定义选股选择参数，为避免自定义选股时出现分布图
    $scope.selfChoseTime = function (item, parentItem) {
        item = angular.copy(item);
        parentItem.selectTermDisplay = item.SelectDisplay;
        parentItem.selectTerm = item.SelectItem;
        //console.log(parentItem);
    };
    $scope.choParam = function (item, parentItem) {
        item = angular.copy(item);
        parentItem.selectLeft = item.SelectDisplay;
    };
    $scope.selfChoParam = function (item, parentItem) {
        item = angular.copy(item);
        parentItem.selectLeft = item.SelectDisplay;

    };
    /*分布图数据*/
    $scope.fenBuPicItem = {};
    $scope.fenbutu = function (picItem) {
        // console.log("fenbutu");
        $scope.fenBuPicItem = picItem;

    };
    //保存方案方法，供点击时调用,并且把最新的方案名显示在我的选股方案处
    $scope.saveSolu = function (params) {
        SomeService.saveSolution(params, function (data) {
            SomeService.getSoluName(function (data) {
                if (data) {
                    for (var i = 0, j = data.length; i < j; i++) {
                        data[i].Name = data.length - i + "、" + data[i].Name;
                    };
                    $scope.solutions = data;
                }
            }, function (error) {
            });
            if (data) {
            }
        }, function (error) {

        });
    }

    // 字符串截取函数
    function subString(str, len, replaceStr) {
        // var chineseRegex = /[^\x00-\xff]/g;
        /*ljq加上判断 当字符为空返回，否则会报错*/
        if (str == null || str == "") { return };
        var newLength = 0,
            newStr = "",
            chineseRegex = /[\u4e00-\u9fa5]/g,
            singleChar = "",
            strLength = str.replace(chineseRegex, "**").length;
        for (var i = 0; i <= strLength; i++) {
            singleChar = str.charAt(i).toString();
            if (singleChar.match(chineseRegex) != null) {
                newLength += 2;
            } else {
                newLength++;
            };
            if (newLength > len) {
                break;
            };
            newStr += singleChar;
        };
        if (strLength > len) {
            if (newStr.charAt(newStr.length - 1).match(chineseRegex) == null) {
                newStr = newStr.substring(0, newStr.length - 1);
            };
            newStr += replaceStr;
        };
        return newStr;
    }
    // 分布图相关函数
    function addSlider(obj) {
        obj.wrap.find(".sliderWrap").remove();
        $(obj.wrap).find("input.valueLeft").off("keyup");
        $(obj.wrap).find("input.valueRight").off("keyup");
        $('<div></div>', {
            class: 'sliderWrap'
        }).appendTo(obj.wrap);
        var sliderWrap = obj.wrap.find(".sliderWrap");
        $('<canvas></canvas>', {
            class: 'canvas'
        }).appendTo(sliderWrap);
        $('<div></div>', {
            class: 'nstSlider',
            'data-range_min': obj.range_min,
            'data-range_max': obj.range_max,
            'data-cur_min': obj.cur_min,
            'data-cur_max': obj.cur_max
        }).appendTo(sliderWrap);
        var classArr = ['bar', 'leftGrip', 'rightGrip', 'leftLabel', 'rightLabel'];
        var nstSliderObj = sliderWrap.find(".nstSlider");
        for (var i = 0, j = classArr.length; i < j; i++) {
            $('<div></div>', {
                class: classArr[i]
            }).appendTo(nstSliderObj);
        }
        nstSliderObj.nstSlider({
            "crossable_handles": true,
            "left_grip_selector": ".leftGrip",
            "right_grip_selector": ".rightGrip",
            "value_bar_selector": ".bar",
            "value_changed_callback": function (cause, leftValue, rightValue) {
                $(this).parent().find('.leftLabel').text(leftValue);
                $(this).parent().find('.rightLabel').text(rightValue);
                // not finished
                // left
                // var leftRealPos=posNum(leftValue/100);
                // var Symbol=1;
                // leftValue>=0?Symbol=1:Symbol=-1;
                // if (leftRealPos>=get10N(8)) {
                //     $(obj.wrap).find("input.valueLeft").val((Symbol*leftRealPos/get10N(8)).toFixed(2)+"亿");
                // }else if(leftRealPos>=get10N(4) && leftRealPos<get10N(8)){
                //     $(obj.wrap).find("input.valueLeft").val((Symbol*leftRealPos/get10N(4)).toFixed(2)+"万");
                // }else{
                //     $(obj.wrap).find("input.valueLeft").val((Symbol*leftRealPos).toFixed(2));
                // } 
                // changeVal(leftValue,$(obj.wrap).find("input.valueLeft"));
                // $(obj.wrap).find("input.valueLeft").val(leftValue/100);

                // $scope.fenBuPicItem.minValue=(getVal(leftValue));
                $scope.fenBuPicItem.minValue = (leftValue / 100);
                // setTimeout(function(){
                // changeVal(leftValue,$(obj.wrap).find("input.valueLeft"));
                // },1000);     
                // $(obj.wrap).find("input.valueRight").val(rightValue/100);                
                $scope.fenBuPicItem.maxValue = (rightValue / 100);
                $scope.$apply($scope.choiceItems);
            }
        });
        // 左侧输入框 keyup 事件
        $(obj.wrap).find("input.valueLeft").keyup(function () {
            if ($(obj.wrap).find("input.valueLeft").val() * 100 >= obj.range_min - 0
                && $(obj.wrap).find("input.valueLeft").val() * 100 <= obj.range_max - 0) {
                if ($(obj.wrap).find("input.valueLeft").val() - 0 < $(obj.wrap).find("input.valueRight").val() - 0) {

                    $(obj.wrap).find(".leftGrip").animate({
                        "left": Math.floor(($(obj.wrap).find("input.valueLeft").val() * 100 - obj.range_min - 0) / (obj.range_max - 0 - obj.range_min - 0) * 300) + 'px'
                    });
                };
            } else if ($(obj.wrap).find("input.valueLeft").val() * 100 <= obj.range_min - 0) {
                $(obj.wrap).find(".leftGrip").animate({
                    "left": '0px'
                });
            };
        });
        // 右侧输入框 keyup 事件
        $(obj.wrap).find("input.valueRight").keyup(function () {
            if ($(obj.wrap).find("input.valueRight").val() * 100 <= obj.range_max - 0 &&
                $(obj.wrap).find("input.valueRight").val() * 100 >= obj.range_min - 0) {
                if ($(obj.wrap).find("input.valueLeft").val() - 0 < $(obj.wrap).find("input.valueRight").val() - 0) {
                    $(obj.wrap).find(".rightGrip").animate({
                        "left": Math.floor(($(obj.wrap).find("input.valueRight").val() * 100 - obj.range_min - 0) / (obj.range_max - 0 - obj.range_min - 0) * 300) + 'px'
                    });
                };
            } else if ($(obj.wrap).find("input.valueRight").val() * 100 >= obj.range_max - 0) {
                $(obj.wrap).find(".rightGrip").animate({
                    "left": '300px'
                });
            }
        });
    }
    function drawMap(obj) {
        var c = obj.can,
            data = obj.data,
            len = obj.len,
            xSpa = obj.xSpa,
            ySpa = obj.ySpa,
            color = obj.color,
            can = c.getContext("2d"),
            cWidth = c.width,
            cHeight = c.height,
            barWidth = (cWidth - 2 * xSpa) / len,
            maxLen = getNum(data, true) - getNum(data, false),
            arrNum = getArr(data, maxLen / (len - 1), len),
            maxNum = getNum(arrNum, true);
        can.clearRect(0, 0, cWidth, cHeight);
        drawChart();
        // draw
        function drawChart() {
            var bX, bY, bHt;
            for (var i = 0; i < arrNum.length; i++) {
                bHt = (cHeight - 2 * ySpa) * arrNum[i] / maxNum;
                bX = i * barWidth + xSpa;
                bY = cHeight - ySpa - bHt;
                drawRectangle(can, bX, bY, barWidth, bHt, color);
            };
        }
        // draw rect
        function drawRectangle(context, x, y, w, h, color) {
            context.save();
            context.beginPath();
            context.fillStyle = color;
            context.fill();
            context.rect(x, y, w, h);
            context.closePath();
            context.fill();
            context.restore();
        }
        // get mapArr
        function getArr(arr, lenUnit, len) {
            var newArr = [],
                min = getNum(arr, false);
            for (var i = 0; i < len; i++) {
                newArr[i] = 0;
                for (var m = 0, n = arr.length; m < n; m++) {
                    if (arr[m] >= min + i * lenUnit && arr[m] < min + (i + 1) * lenUnit) {
                        newArr[i] = newArr[i] + 1;
                    };
                }
            }
            return newArr;
        }
    }
    function getNum(arr, bool) {
        if (arr) {
            if (arr.length > 0) {
                var max = arr[0] - 0,
                    min = arr[0] - 0;
                for (var m = 0, n = arr.length; m < n; m++) {
                    if (arr[m] - 0 >= max) {
                        max = arr[m] - 0;
                    } else if (arr[m] - 0 <= min) {
                        min = arr[m] - 0;
                    }
                };
                return bool ? max : min;
            };
        }
    };
    function posNum(num) {
        return num >= 0 ? num : num * (-1);
    }
    function get10N(n) {
        var num = 1;
        for (var i = 0; i < n; i++) {
            num = num * 10;
        }
        return num;
    }
    function getVal(valSrc) {
        var RealPos = posNum(valSrc / 100);
        var Symbol = 1;
        var needVal = '';
        valSrc >= 0 ? Symbol = 1 : Symbol = -1;
        if (RealPos >= get10N(8)) {
            needVal = (Symbol * RealPos / get10N(8)).toFixed(2) + "亿";
        } else if (RealPos >= get10N(4) && RealPos < get10N(8)) {
            needVal = (Symbol * RealPos / get10N(4)).toFixed(2) + "万";
        } else {
            needVal = (Symbol * RealPos).toFixed(2);
        }
        return needVal;
    };
    function changeVal(valSrc, obj) {
        var RealPos = posNum(valSrc / 100);
        var Symbol = 1;
        valSrc >= 0 ? Symbol = 1 : Symbol = -1;
        if (RealPos >= get10N(8)) {
            obj.val((Symbol * RealPos / get10N(8)).toFixed(2) + "亿");
        } else if (RealPos >= get10N(4) && RealPos < get10N(8)) {
            obj.val((Symbol * RealPos / get10N(4)).toFixed(2) + "万");
        } else {
            obj.val((Symbol * RealPos).toFixed(2));
        }
    };
    function numAddUnit(num) {
        var RealPos = posNum(num);
        var Symbol = 1;
        var changedNum = 0;
        num >= 0 ? Symbol = 1 : Symbol = -1;
        if (RealPos >= get10N(8)) {
            changedNum = (Symbol * RealPos / get10N(8)).toFixed(2) + "亿";
        } else if (RealPos >= get10N(4) && RealPos < get10N(8)) {
            changedNum = (Symbol * RealPos / get10N(4)).toFixed(2) + "万";
        } else {
            changedNum = (Symbol * RealPos).toFixed(2);
        }
        return changedNum;
    }
    // 保存、删除共用部分
    var winHeight = $(window).height(),
        winWidth = $(window).width();
    var makeSureMes = true;
    // 自定义弹出框  间距、内容、类 
    var outBoxFlag = false;
    var outBoxTimer;
    function outBox(str, num1, num2, cls) {
        $(document).off("keydown");
        clearTimeout(outBoxTimer);
        $("#bombBox").html("");
        $('<a></a>', {
            class: 'closeBtn'
        }).appendTo($("#bombBox"));
        $('<div></div>', {
            class: 'tipCon'
        }).appendTo($("#bombBox"));
        $('<div></div>', {
            class: 'btnWrap'
        }).appendTo($("#bombBox"));
        $(".closeBtn").html('×');
        $(".tipCon").html('<span>!</span><span>' + str + '</span>');
        $(".btnWrap").html('<a class="' + cls + '">确定</a>');
        $(".tipCon span").eq(1).css("marginLeft", num1 + "px");
        $("." + cls).css("marginLeft", num2 + "px");
        $(".btnWrap a").eq(0).addClass('btnAct');
        $("#mask").css("height", winHeight);
        $("#mask").css("width", winWidth);
        $("#mask").show();
        $("#bombBox").css({
            "display": "block",
            "top": -200 + "px",
            "left": (winWidth - 320) / 2 + "px"
        }).animate({
            top: (winHeight - 200) / 2 + "px",
            left: (winWidth - 320) / 2 + "px"
        });
        outBoxFlag = true;
        $(document).keydown(function (event) {
            var e = event || window.event;
            var k = e.keyCode || e.which || e.charCode;
            if (k == 13) {
                e.preventDefault();
                closeAll();
                if ($(".btnWrap a").eq(0).hasClass("btnAct")) {
                    if (outBoxFlag) {
                        closeAll();
                        outBoxFlag = false;
                    };
                };
            };
            if (k == 37 || k == 39) {
                $(".btnWrap a").each(function () {
                    $(this).toggleClass('btnAct');
                });
                if (getOs().toLowerCase() == "firefox") {
                    return false;
                };
            };
        });
        outBoxTimer = setTimeout(function () {
            closeAll();
        }, 3000);
    };
    function closeAll() {
        $("#mask").hide();
        $("#bombBox").css({
            "display": "none"
        });
        $("#bombBox").html('');
        makeSureMes = true;
    };
    // 调用
    $(function () {
        // 保存、删除功能

        $("#bombBox").on("click", "a", function () {
            if ($(this).text() == "确定") {
                // return;
            } else {
                closeAll();
                return;
            };
            if (makeSureMes) {
                closeAll();
                return;
            };
        });
        function getOs() {
            var OsObject = "";
            if (navigator.userAgent.indexOf("MSIE") > 0) {
                return "MSIE";
            }
            if (isFirefox = navigator.userAgent.indexOf("Firefox") > 0) {
                return "Firefox";
            }
            if (isSafari = navigator.userAgent.indexOf("Safari") > 0) {
                return "Safari";
            }
            if (isCamino = navigator.userAgent.indexOf("Camino") > 0) {
                return "Camino";
            }
            if (isMozilla = navigator.userAgent.indexOf("Gecko/") > 0) {
                return "Gecko";
            }
        };
        $("#bombBox").on('mouseenter', 'a', function () {
            if (!$(this).hasClass("closeBtn")) {
                $("#bombBox a").not(".closeBtn").removeClass("btnAct");
                $(this).addClass('btnAct');
            };
        });
        // 删除 
        var delFlag = false;
        $("#del").click(function () {
            $(document).off("keydown");
            $("#bombBox").html("");
            $('<a></a>', {
                class: 'closeBtn'
            }).appendTo($("#bombBox"));
            $('<div></div>', {
                class: 'tipCon'
            }).appendTo($("#bombBox"));
            $('<div></div>', {
                class: 'btnWrap'
            }).appendTo($("#bombBox"));
            $(".closeBtn").html('×');
            $(".tipCon").html('<span>!</span><span>确定清空所有条件？</span>');
            $(".btnWrap").html('<a>确定</a><a>取消</a>');
            $(".btnWrap a").eq(0).addClass('btnAct');
            $("#mask").css("height", winHeight);
            $("#mask").css("width", winWidth);
            $("#mask").show();
            $("#bombBox").css({
                "display": "block",
                "top": -200 + "px",
                "left": (winWidth - 320) / 2 + "px"
            }).animate({
                top: (winHeight - 200) / 2 + "px",
                left: (winWidth - 320) / 2 + "px"
            });
            function delMes() {
                $scope.choToStock.IndexRangeConditions = $scope.choiceItems = [];
                $scope.choToStock.CustomIndexConditions = $scope.selfItems = [];
                $scope.defaultItem = {
                    first: {},
                    firstInputVal: '',
                    operator1: '1',
                    operator1Name: '+',
                    second: {},
                    secondInputVal: '',
                    operator2: '1',
                    operator2Name: '≥',
                    third: {},
                    thirdInputVal: '',
                    isFirstRo: false,
                    isSecondRo: false,
                    isThirdRo: false
                };//用户自定义
                isHastarget();
                $scope.$apply($scope.choToStock);
                $(".tips").css({ "display": "block" });
                /*$(".result").css("display","none");*/
            };
            $(".btnWrap a").eq(0).click(function () {
                delMes();
            });
            delFlag = true;
            $(document).keydown(function (event) {
                var e = event || window.event;
                var k = e.keyCode || e.which || e.charCode;
                if (k == 13) {
                    e.preventDefault();
                    if ($(".btnWrap a").eq(0).hasClass("btnAct")) {
                        if (delFlag) {
                            delMes();
                            delFlag = false;
                        };
                    };
                    closeAll();
                };
                if (k == 37 || k == 39) {
                    $(".btnWrap a").each(function () {
                        $(this).toggleClass('btnAct');
                    });
                    if (getOs().toLowerCase() == "firefox") {
                        return false;
                    };
                };
            });
        });
        // 保存
        var saveFlag = false;
        $("#save").click(function () {
            $(document).off("keydown");
            $("#bombBox").html("");
            $('<a></a>', {
                class: 'closeBtn'
            }).appendTo($("#bombBox"));
            $('<div></div>', {
                class: 'tipConSave'
            }).appendTo($("#bombBox"));
            $('<div></div>', {
                class: 'inputWrap'
            }).appendTo($("#bombBox"));
            $('<div></div>', {
                class: 'btnWrap'
            }).appendTo($("#bombBox"));
            $('<p></p>', {
                class: 'nameMes'
            }).appendTo($("#bombBox"));
            $(".closeBtn").html('×');
            $(".tipConSave").html('请输入要保存的方案名称');
            $(".inputWrap").html('<input type="text" value="我的方案" >');
            $(".btnWrap").html('<a>确定</a><a>取消</a>');
            $(".btnWrap a").eq(0).addClass('btnAct');
            $("#mask").css("height", winHeight);
            $("#mask").css("width", winWidth);
            $("#mask").show();
            $("#bombBox").css({
                "display": "block",
                "top": -200 + "px",
                "left": (winWidth - 320) / 2 + "px"
            }).animate({
                top: (winHeight - 200) / 2 + "px",
                left: (winWidth - 320) / 2 + "px"
            });
            $(".inputWrap input").select();
            var re = /^[\u4E00-\u9FA5A-Za-z0-9]+$/;
            var Myname = $(".inputWrap input").eq(0).val();
            $(".inputWrap input").eq(0).focus(function () {
                $(".nameMes").html('');
                makeSureMes = true;
            });
            $(".inputWrap input").eq(0).blur(function () {
                Myname = $(".inputWrap input").eq(0).val();
                if (!re.test(Myname)) {
                    makeSureMes = false;
                    $(".nameMes").html('请输入中英文或者数字');
                } else {
                    var chineseReg = /[\u4e00-\u9fa5]/g;
                    if (Myname.replace(chineseReg, "**").length > 20) {
                        makeSureMes = false;
                        $(".nameMes").html('输入字符长度超过20');
                    } else {
                        makeSureMes = true;
                    };
                };
            });
            function saveMes() {
                var dataChoice = {};
                var dataChoice = {
                    IndexRangeConditions: $scope.choiceItems,
                    CustomIndexConditions: $scope.selfItems
                };
                // dataChoice.CustomIndexConditions=getObjUnit($scope.selfItems);         
                var choSaveSolu = {
                    "Id": 0,
                    "Name": Myname,
                    "InputCondition": dataChoice,
                    /* "UserId": 0,
                     "CreateDate": "2017-01-23T06:17:58.428Z",
                     "pi_users": {
                       "Id": 0,
                       "Email": "string",
                       "PasswordHash": "string",
                       "UserName": "string",
                       "pi_choice_solution": [
                         dataChoice
                       ]
                     }*/
                };
                if (Myname != null && Myname != "") {
                    $scope.saveSolu(choSaveSolu);
                    $scope.$apply($scope.solutions);
                    /*var url="http://10.1.135.80:9999/Index/SaveSolution";
                    $.ajax({
                         url: url,
                         type: "POST",
                         data :data,
                         dataType:'json',
                         ContentType: "application/json",
                         success: function(data){
                            $("#myOptions ul").prepend($('<li>'+Myname+'</li>'));
                         }
                    });  */
                };
            };
            $(".btnWrap a").eq(0).click(function () {
                if (makeSureMes) {
                    saveMes();
                };
            });
            saveFlag = true;
            $(document).keydown(function (event) {
                Myname = $(".inputWrap input").eq(0).val();
                var e = event || window.event;
                var k = e.keyCode || e.which || e.charCode;
                if (k == 13) {
                    e.preventDefault();
                    if ($(".btnWrap a").eq(0).hasClass("btnAct")) {
                        if (saveFlag) {
                            saveMes();
                            saveFlag = false;
                        };
                    };
                    closeAll();
                };
                if (k == 37 || k == 39) {
                    $(".btnWrap a").each(function () {
                        $(this).toggleClass('btnAct');
                    });
                    if (getOs().toLowerCase() == "firefox") {
                        return false;
                    };
                };
            });
        });
        /*加号显示隐藏*/
        if ($scope.choiceItems == [] || $scope.choiceItems.length == 0) {
            $(".tips").css({ "display": "block" });
        } else {
            $(".tips").css({ "display": "none" });
        };
        /*拖拽*/
        //判断对象是否为空
        function isEmptyObject(e) {
            var t;
            for (t in e)
                return !1;
            return !0
        };
        //为空显示加号,判断对象为空的方法
        function isHastarget() {
            if (isEmptyObject($scope.defaultItem.first) && ($scope.defaultItem.firstInputVal).trim()=="") {
                $(".selFirTip").addClass("addTipUrl");
            } else {
                $(".selFirTip").removeClass("addTipUrl");

            };
            if (isEmptyObject($scope.defaultItem.second) && ($scope.defaultItem.secondInputVal).trim()=="") {
                $(".selSencdTip").addClass("addTipUrl");
                // console.log(421);
            } else {
                $(".selSencdTip").removeClass("addTipUrl");
            };
            if (isEmptyObject($scope.defaultItem.third) && ($scope.defaultItem.thirdInputVal).trim()=="") {
                $(".selTridTip").addClass("addTipUrl");
                // console.log(821);
            } else {
                $(".selTridTip").removeClass("addTipUrl");
                return false;
            };
        };
        isHastarget();

        // 获取位置
        function getPoint(obj) { //获取某元素以浏览器左上角为原点的坐标
            var offsetDence = {};
            offsetDence.t = obj.offsetTop; //获取该元素对应父容器的上边距
            offsetDence.l = obj.offsetLeft; //对应父容器的上边距
            //判断是否有父容器，如果存在则累加其边距
            while (obj = obj.offsetParent) {//等效 obj = obj.offsetParent;while (obj != undefined)
                offsetDence.t += obj.offsetTop; //叠加父容器的上边距
                offsetDence.l += obj.offsetLeft; //叠加父容器的左边距
            };
            return offsetDence;
        };
        // 拖拽区域
        function canDragArea() {
            /*  console.log(dragEles[0].offset().left);*/
            var dragXYs = [];
            for (var i = 0, len = dragEles.length; i < len; i++) {
                var dragXY = {};
                dragXY.divx1 = getPoint(dragEles[i]).l;
                dragXY.divy1 = getPoint(dragEles[i]).t;
                dragXY.divx2 = getPoint(dragEles[i]).l + dragEles[i].offsetWidth;
                dragXY.divy2 = getPoint(dragEles[i]).t + dragEles[i].offsetHeight;
                dragXY.type = dragEles[i].getAttribute('data-drag');
                dragXYs.push(dragXY);
            };
            //console.log(dragXYs);
            return dragXYs
        };
        // 可拖拽对象判断
        function isBelongDrag(x, y) {
            //console.log(x, y);
            var isDragArea = canDragArea();
            var dragAtt = {}
            for (var i = 0, len = isDragArea.length; i < len; i++) {
                var dragEleXY = isDragArea[i];
                if (x < dragEleXY.divx2 && x > dragEleXY.divx1 && y < dragEleXY.divy2 && y > dragEleXY.divy1) {
                    dragAtt.isBelong = true;
                    dragAtt.type = dragEleXY.type;
                    return dragAtt;
                }
            }
            return false;
        }
        /*将可拖拽区域坐标存储 start*/
        var dragEles = $("div[data-drag]");
        var dragXYs = [];
        for (var i = 0, len = dragEles.length; i < len; i++) {
            var dragXY = {};
            // console.log(dragXY);
            dragXY.divx1 = getPoint(dragEles[i]).l;
            dragXY.divy1 = getPoint(dragEles[i]).t;
            dragXY.divx2 = getPoint(dragEles[i]).l + dragEles[i].offsetWidth;
            dragXY.divy2 = getPoint(dragEles[i]).t + dragEles[i].offsetHeight;
            dragXY.type = dragEles[i].getAttribute('data-drag');
            dragXYs.push(dragXY);
        }
        // 拖拽交互
        $("#leftBody").on('mousedown', '.targetBox', function (e) {
            e.preventDefault();
            e.stopPropagation();
            var disX = e.offsetX;
            var disY = e.offsetY;
            //鼠标离所选元素的距离
            if ($(e.target).closest("ul").attr("id")) {
                var searchDrag = $(e.target).closest("ul").attr("id");
            };
            var cloneBox = $(e.target).clone();
            $("body").append(cloneBox);
            $(document).on("mousemove.targetBox", function (e) {
                var x = e.pageX - disX;
                var y = e.pageY - disY;
                if (searchDrag == "nameBody") {
                    cloneBox.addClass("searchCho").css({ left: x, top: y });
                } else {
                    cloneBox.addClass("choiceItem").css({ left: x, top: y });
                };
            });
            $(document).on("mouseup.targetBox", function (e) {
                $(document).off("mousemove.targetBox");
                $(document).off("mouseup.targetBox");
                cloneBox.remove();
                var x1 = e.pageX;
                var y1 = e.pageY;
                //拖拽到有效区域                    
                if (isBelongDrag(x1, y1) && ($scope.choiceItem)) {
                    var type = isBelongDrag(x1, y1).type;
                    if (type === 'default' && (!isEmptyObject($scope.choiceItem))) {
                        if ($scope.choiceItems.length < 10) {
                            $scope.choiceItems.push($scope.choiceItem);
                            $scope.choiceItem = null;
                            $scope.$apply($scope.choiceItems);

                        } else {
                            outBox("最多只能添加10个条件！", 24, 65, "sureBtn");
                        };
                    } else {
                        type = parseInt(type);
                        var choiceItem = angular.copy($scope.choiceItem);
                        switch (type) {
                            case 1: $scope.defaultItem.first = choiceItem; $scope.defaultItem.isFirstRo = true; break;
                            case 2: $scope.defaultItem.second = choiceItem; $scope.defaultItem.isSecondRo = true; break;
                            case 3: $scope.defaultItem.third = choiceItem; $scope.defaultItem.isThirdRo = true; break;
                            default: break;
                        };
                        $scope.choiceItem = null;
                        $scope.$apply($scope.defaultItem);
                        isHastarget();
                    };
                };
                if ($scope.choiceItems == [] || $scope.choiceItems.length == 0) {
                    $(".tips").css({ "display": "block" });
                } else {
                    $(".tips").css({ "display": "none" });
                };

            });
            /*return false;*/
        });
        /*范围选股拖拽删除*/
        $("#checkedWrap").on('mousedown', '.dragDelDiv', function (e) {
            e.preventDefault();
            e.stopPropagation();
            var disX = e.offsetX;
            var disY = e.offsetY;
            //console.log(disY);
            //鼠标离所选元素的距离
            var cloneBox = $(e.target).clone();
            $("body").append(cloneBox);
            $(document).on("mousemove.dragDelDiv", function (e) {
                var x = e.pageX - disX;
                var y = e.pageY - disY;
                //console.log(e.pageY) 为整个浏览器的距离
                cloneBox.addClass("addDragDel").css({ left: x, top: y });
            });
            //mouseup
            $(document).on("mouseup.dragDelDiv", function (e) {
                /*因为和分布图冲突，事件绑定注释*/
                $(document).off("mousemove.dragDelDiv");
                $(document).off("mouseup.dragDelDiv");
                /*重新为document加上时间*/
                cloneBox.remove();
                var x1 = e.pageX;
                var y1 = e.pageY;
                //拖拽到有效区域
                //console.log(isBelongDrag(x1,y1));
                if (!isBelongDrag(x1, y1) || isBelongDrag(x1, y1).type != "default") {
                    $scope.choiceItems.splice(($scope.choseDelItem), 1);
                    $scope.$apply($scope.choiceItems);
                };
                /*把所有指标删除时背景出现*/
                if ($scope.choiceItems == [] || $scope.choiceItems.length == 0) {
                    $(".tips").css({ "display": "block" });
                } else {
                    $(".tips").css({ "display": "none" });
                };
            });
            /*return false;*/
        });
        /*自定义选框删除*/
        $("#customWrap").on('mousedown', '.selDelFir', function (e) {
            e.preventDefault();
            e.stopPropagation();
            var disX = e.offsetX;
            var disY = e.offsetY;
            //console.log(disY);
            //鼠标离所选元素的距离
            var selDelArea = $(e.target).parents("div[data-drag]");
            var selDeltype = selDelArea.attr('data-drag');
            //console.log(selDeltype);
            var cloneBox = $(e.target).clone();
            $("body").append(cloneBox);
            $(document).on("mousemove.selDelFir", function (e) {
                var x = e.pageX - disX;
                var y = e.pageY - disY;
                //console.log(e.pageY) 为整个浏览器的距离
                cloneBox.addClass("addDragDel").css({ left: x, top: y });
            });
            //mouseup
            $(document).on("mouseup.selDelFir", function (e) {
                /*因为和分布图冲突，事件绑定注释*/
                $(document).off("mousemove.selDelFir");
                $(document).off("mouseup.selDelFir");
                /*重新为document加上时间*/
                cloneBox.remove();

                var x1 = e.pageX;
                var y1 = e.pageY;
                //拖拽到有效区域
                if (!isBelongDrag(x1, y1) || isBelongDrag(x1, y1).type == "default") {
                    var type = parseInt(selDeltype);
                    switch (type) {
                        case 1: $scope.defaultItem.first = {};
                            $scope.$apply($scope.defaultItem.first);
                            $scope.defaultItem.isFirstRo = false;
                            $scope.defaultItem.firstInputVal="";
                            $(".selFirTip").addClass("addTipUrl");
                            //console.log($scope.defaultItem);
                            break;
                        case 2: $scope.defaultItem.second = {};
                            $scope.$apply($scope.defaultItem.second);
                            $scope.defaultItem.secondInputVal="";
                            $scope.defaultItem.isSecondRo = false;
                            $(".selSencdTip").addClass("addTipUrl");
                            break;
                        case 3: $scope.defaultItem.third = {};
                            $scope.$apply($scope.defaultItem.third);
                            $scope.defaultItem.thirdInputVal="";
                            $scope.defaultItem.isThirdRo = false;
                            $(".selTridTip").addClass("addTipUrl");
                            break;
                        default: break;
                    };
                };
            });
            /*return false;*/
        });

        // 已选条件-下拉列表-时间-数据交互
        $("#checkedWrap").on('mouseenter', '.checkedConUnit', function () {
            var data = [];
            var curMin = 0;
            var curMax = 0;
            if ($scope.fenBuPicItem.value != undefined || $scope.fenBuPicItem.value != null) {
                data = $scope.fenBuPicItem.value;
                curMin = $scope.fenBuPicItem.minValue;
                curMax = $scope.fenBuPicItem.maxValue;
            };
            /*ljq加上判断，当后台的分布图数据没有时，分布图不出现*/
            // console.log($scope.fenBuPicItem);
            // console.log(getNum(data,false)*100,getNum(data,true)*100,curMin*100,curMax*100);
            if (data.length > 0) {
                // console.log(data,"need");
                if ($(this).find(".sliderWrap").length <= 0) {
                    addSlider({
                        wrap: $(this),
                        range_min: getNum(data, false) * 100,
                        range_max: getNum(data, true) * 100,
                        cur_min: curMin * 100,
                        cur_max: curMax * 100
                    });
                    drawMap({
                        can: $(this).find(".canvas")[0],
                        data: data,
                        len: 100,
                        xSpa: 5,
                        ySpa: 5,
                        color: "#FDB9B9"
                    });
                };
                $(".sliderWrap").css({
                    "display": "none"
                });
                $(this).find(".sliderWrap").css({
                    "display": "block"
                });
            };
        });
        $("#checkedWrap").on('mouseleave', '.checkedConUnit', function () {
            $(this).find(".sliderWrap").css({
                "display": "none"
            });
        });
    });
    // 其他
    /*拖拽移除得到第几个*/
    $scope.choseDelItem = 0;
    $scope.delDivItem = function (dataIndex) {
        $scope.choseDelItem = dataIndex;
    };
    //点击方案名 显示方案内容
    $scope.getSolutionCont = function (item) {
        $scope.choToStock.IndexRangeConditions = $scope.choiceItems = [];
        $scope.choToStock.CustomIndexConditions = $scope.selfItems = [];
        var selDefContArr = [];
        var rangeContArr = [];
        var param = item.Id;
        /*处理自定义选股条件后台字段供前台调用*/
        function filSelfDet(item) {
            var newItem = {};
            if (item.Name == null) { return item = {}; }
            newItem.name = item.Name;
            newItem.nameCon = subString(newItem.name, 14, '...');
            newItem.code = item.Code;
            newItem.minValue = item.MinValue;
            newItem.maxValue = item.MaxValue;
            newItem.selectTerm = item.SelectTerm;
            newItem.selectTermDisplay = item.SelectTermDisplay;
            // newItem.selectTermList=item.SelectTermList;
            newItem.selectLeft = item.SelectLeft;
            //  newItem.selectLeftList=item.SelectLeftList;
            newItem.paramsValues = item.ParamsValues;
            //  newItem.picValue=item.Value;
            return newItem;
        };

        SomeService.getSoluCont(param, function (data) {
            // console.log("getSoluCont");
            if (data) {
                angular.forEach(data.CustomIndexConditions, function (value, key) {
                    var selDefCont = {};
                    selDefCont.first = filSelfDet(value.first);
                    selDefCont.second = filSelfDet(value.second);
                    selDefCont.third = filSelfDet(value.third);
                    selDefCont.operator1 = value.operator1;
                    selDefCont.operator2 = value.operator2;
                    selDefCont.operator1Name = value.operator1Name;
                    selDefCont.operator2Name = value.operator2Name;
                    if (value.firstInputVal != null) {
                        selDefCont.firstInputVal = value.firstInputVal;
                    } else {
                        selDefCont.firstInputVal = "";
                    };
                    if (value.secondInputVal != null) {
                        selDefCont.secondInputVal = value.secondInputVal;
                    } else {
                        selDefCont.secondInputVal = "";
                    };
                    if (value.thirdInputVal != null) {
                        selDefCont.thirdInputVal = value.thirdInputVal;
                    } else {
                        selDefCont.thirdInputVal = "";
                    };
                    this.push(selDefCont);
                }, selDefContArr);
                //console.log(selDefContArr);
                $scope.choToStock.CustomIndexConditions = $scope.selfItems = selDefContArr;
                /*有则自动打开*/
                if ($scope.selfItems == [] || $scope.selfItems.length == 0) {
                    $("#customWrap").css({ "display": "none" });
                    $(".customTit").find("div").removeClass("tabAct");

                } else {
                    $("#customWrap").css({ "display": "block" });
                    $(".customTit").find("div").addClass("tabAct");
                };
                angular.forEach(data.IndexRangeConditions, function (value, key) {
                    var rangeCont = {};
                    rangeCont.value = value.Value;
                    rangeCont.minValue = value.MinValue;
                    rangeCont.maxValue = value.MaxValue;
                    rangeCont.selectTerm = value.SelectTerm;
                    rangeCont.selectTermDisplay = value.SelectTermDisplay;
                    rangeCont.selectTermList = value.SelectTermList;
                    rangeCont.selectLeft = value.SelectLeft;
                    rangeCont.selectLeftList = value.SelectLeftList;
                    rangeCont.paramsValues = value.ParamsValues;
                    rangeCont.name = value.Name;
                    rangeCont.nameCon = subString(rangeCont.name, 14, '...');
                    rangeCont.code = value.Code;
                    this.push(rangeCont);
                }, rangeContArr);
                $scope.choToStock.IndexRangeConditions = $scope.choiceItems = rangeContArr;
                if ($scope.choiceItems == [] || $scope.choiceItems.length == 0) {
                    $(".tips").css({ "display": "block" });
                } else {
                    $(".tips").css({ "display": "none" });
                };
            };
        }, function () {
            //接口失败处理方式
        });

    };

    //选择标准
    $scope.choiceItemParam={};
    $scope.selectItem = function (data) {
        //console.log(789);
        $scope.choiceItem=null;
        //需要深拷贝
        $scope.choiceItemParam = angular.copy(data);
        //已选条件连接口，获取最大值等属性
        var params = $scope.choiceItemParam.code;
        var indexDetArr = [];
        SomeService.getBiaoZhun(params, function (data) {
            if (data) {
                angular.forEach(data, function (value, key) {
                    var indexDet = {};
                    indexDet.value = value.Value;
                    indexDet.minValue = value.MinValue;
                    indexDet.maxValue = value.MaxValue;
                    indexDet.selectTerm = value.SelectTerm;
                    indexDet.selectTermDisplay = value.SelectTermDisplay;
                    indexDet.selectTermList = value.SelectTermList;
                    indexDet.selectLeft = value.SelectLeft;
                    indexDet.selectLeftList = value.SelectLeftList;
                    indexDet.paramsValues = value.ParamsValues;
                    indexDet.name = value.Name;
                    indexDet.nameCon = subString(indexDet.name, 14, '...');
                    indexDet.code = value.Code;
                    this.push(indexDet);
                }, indexDetArr);
            };
            $scope.choiceItem = indexDetArr[0];
           // console.log($scope.choiceItem);
        }, function () {
            //接口失败处理方式
        });
    };

    //查询结果
    //选股结果把已选条件上半部分和自定义选股综合在一起,传给后台调结果
    /*控制升降状态，默认降序*/
    var isNewGet = true;
    // var isClick=false;
    var isDesc = true;
    $scope.getResults = function (choData, urlParamData) {
        if (urlParamData == null) {
            urlParamData = "ChangeRate";
            isNewGet = true;
            isDesc = true;
            // isClick=false;
        };
        if (choData == null) {
            choData = $scope.choToStock;
            isNewGet = false;
        };
        document.getElementById('tableBody').scrollTop = 0;
        if (isNewGet || (isDesc)) {
            urlParamData = urlParamData + " desc"
            isDesc = false;
        } else {
            urlParamData = urlParamData
            isDesc = true;
        };
        if (choData.IndexRangeConditions.length == 0 && choData.CustomIndexConditions.length == 0) {
            return $scope.resultList = [], $scope.resultLenghtAll = $scope.resultList.length;
        };
        var stockArrFir = [];
        var isMoreLoading = false; //是否加载更多
        var loadTip = document.getElementById("loadTip");
        var isAllData = false; //是否加载更多
        //获取请求
        var urlParamFir = {
            $orderby: urlParamData,
            $inlinecount: 'allpages',
        };
        SomeService.getStockListFir(choData, urlParamFir, function (data) {
            isMoreLoading = false;
            if (data) {
                // console.log(data);
                angular.forEach(data.Items, function (value, key) {
                    var stock = {};
                    stock.name = value.Name;
                    stock.code = value.Code;
                    stock.recentRice = value.RecentRice;
                    stock.changeRate = ((value.ChangeRate) * 100).toFixed(2);
                    stock.changeAmount = value.ChangeAmount;
                    stock.lastAmount = value.LastAmount;
                    stock.volume = numAddUnit(value.volume);
                    stock.todayAmount = numAddUnit(value.TodayAmount);
                    this.push(stock);
                }, stockArrFir);
                $scope.resultList = stockArrFir;
                //console.log(stockArrFir);
                $scope.resultLenghtAll = data.Count
                $scope.resultLenght = $scope.resultList.length;
                // console.log($scope.resultLenght);
                if ($scope.resultLenghtAll <= $scope.resultLenght) {
                    isAllData = true;
                    loadTip.innerHTML = "到底了"
                } else {
                    isAllData = false;
                    loadTip.innerHTML = "滚动加载更多"
                };

            };
        }, function (error) {
        });
        /*inifiScorllStart*/
        function moreAndMoreOnscroll(callback, ele) {
            // console.log("moreLOAD");
            var parentEle = document.getElementById('tableBody');
            if (!callback) {
                parentEle.onscroll = false;
                return false;
            };
            var MAM = document.getElementById(ele);
            /* MAM.scrollTop= 0;//绑定元素*/
            var gap = parseInt(MAM.getAttribute("data-gap")) || 0; //获取差值
            var winHeight = 480; //此处为#tableBody的最大高度max-height值
            var mTop, sTop, result;
            parentEle.onscroll = function () {
                //console.log("onscroll");
                mTop = MAM.offsetTop;
                sTop = parentEle.scrollTop;  //滚动条距离顶部
                result = mTop - sTop;
                if (result <= (winHeight + gap) && (!isAllData)) {
                    if (!isMoreLoading) {
                        //console.log("1286"+isMoreLoading);
                        callback();  //回调
                        isMoreLoading = true;
                    };
                } else {
                    isMoreLoading = false;
                };
            };
        };
        var pageCount = 0; //页数
        var pageAmount = 50; //每页显示多少
        var lastCount = 50; //上一页显示的页数
        //滚动加载数据
        var loadMore = function () {
            pageCount += 1;
            //改变请求条件
            var urlParams = {
                $orderby: urlParamData,
                $inlinecount: 'allpages',
                $top: pageAmount,
                /*$skip : pageAmount * (pageCount-1) + lastCount;*/
                $skip: pageAmount * pageCount
            };
            var stockArr = [];
            SomeService.getStockListMore(choData, urlParams, function (data) {
                if (data.Items && data.Items.length > 0) {
                    angular.forEach(data.Items, function (value, key) {
                        var stock = {};
                        stock.name = value.Name;
                        stock.code = value.Code;
                        stock.recentRice = value.RecentRice;
                        stock.changeRate = ((value.ChangeRate) * 100).toFixed(2);
                        stock.changeAmount = value.ChangeAmount;
                        stock.lastAmount = value.LastAmount;
                        stock.volume = numAddUnit(value.volume);
                        stock.todayAmount = numAddUnit(value.TodayAmount);
                        this.push(stock);
                    }, stockArr);
                    // console.log(stockArr);
                    $scope.resLenTemp = stockArr.length;
                    //console.log($scope.resLenTemp);
                    Array.prototype.push.apply($scope.resultList, stockArr); //合并数组
                    // console.log($scope.resultList);
                    $scope.resultLenght = $scope.resultList.length;
                    //console.log($scope.resultLenght);
                    if ($scope.resultLenghtAll <= $scope.resultLenght) {
                        isMoreLoading = true;
                        loadTip.innerHTML = "到底了"
                        isAllData = true;
                        //console.log("不加了");
                        return isMoreLoading;
                    } else {
                        loadTip.innerHTML = "滚动加载更多"
                    };
                } else {
                    isMoreLoading = true;

                };

            }, function () {

            });

        };
        //股票列表元素注册滚动事件
        moreAndMoreOnscroll(loadMore, "MoreAndMore");
    };
    //监控选项改变则调取结果
    var timerResult = null;
    $scope.$watch(function () {
        return $scope.choToStock;
    }, function (newValue, oldValue) {
        if (newValue != oldValue) {
            /* console.log(newValue);
             console.log(oldValue);*/
            if (null !== timerResult) { $timeout.cancel(timerResult);};
            timerResult = $timeout(function () {
                $timeout.cancel(timerResult); timerResult = null;
                $scope.getResults(newValue, null);

            }, 500);


        };
    }, true);
    //添加自定义标准
    $scope.addItem = function (item) {
       // $(".unitRightAdd").eq(0).removeClass("sureAct");
        /*点击前对选择框的值进行判定，产品判定规则：前两个有任一输入值，最后一个必须输入值*/
        var isPut = false;
        function isPutFun(obj, str) {
            isPut = false;
            /*判定是否为空对象*/
            if (angular.equals({}, obj) && str == "") {
                isPut = false;
                // console.log("null123");
            }else {
                // console.log("some");
                isPut = true;
            };
            return isPut;
        };
        var fir = $scope.defaultItem.first;
        var sec = $scope.defaultItem.second;
        var thir = $scope.defaultItem.third;
        var firIn = $scope.defaultItem.firstInputVal;
        var secIn = $scope.defaultItem.secondInputVal;
        var thirIn = $scope.defaultItem.thirdInputVal;
        if (((isPutFun(fir, firIn)) || (isPutFun(sec, secIn))) && (isPutFun(thir, thirIn))) {
            // console.log("canput");
            if ($scope.selfItems.length < 10) {
                $scope.selfItems.push(angular.copy($scope.defaultItem));
                /*defaultItem重新赋值*/
                $scope.defaultItem.first = {};
                $scope.defaultItem.second = {};
                $scope.defaultItem.third = {};
                $scope.defaultItem.firstInputVal = '';
                $scope.defaultItem.secondInputVal = '';
                $scope.defaultItem.thirdInputVal = '';
                $scope.defaultItem.isFirstRo = false;
                $scope.defaultItem.isSecondRo = false;
                $scope.defaultItem.isThirdRo = false;
                $(".selFirTip").addClass("addTipUrl");
                $(".selSencdTip").addClass("addTipUrl");
                $(".selTridTip").addClass("addTipUrl");
                //console.log($scope.selfItems);
            } else {
                // alert("指标数量已达上限");
                outBox("最多只能添加10个条件！", 24, 65, "sureBtn");
            };
        };
    };
    //删除自定义标准
    $scope.delSelfItem = function (index) {
        /*因为排序为倒序，index需反向*/
        var selfItemsLen = $scope.selfItems.length - 1;
        var delSelNo = selfItemsLen - index;
        $scope.selfItems.splice(delSelNo, 1);
    };
}]);