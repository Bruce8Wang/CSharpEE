appser = angular.module('app.services')

/***接口封裝***/

appser.factory('HttpUtil', ['$http', '$q', 'CAPI', function ($http, $q, CAPI) {
    return {
        get: function (options) {
            if (typeof options == "undefined") {
                options = {};
            }
            var defConf = {
                url: "",
                data: {},
                params: {}
            };
            var setting = $.extend(true, defConf, options);
            var defer = $q.defer();
            $http({
                method: 'get',
                url: setting.url,
                headers: { 'Content-Type': 'application/json' },
                params: setting.params, //追加到url后的参数,会自动加问号
                data: setting.data, //get放在消息体中的参数
                timeout: config.requestTimeOut
            }).success(function (response, status, headers, config) {
                defer.resolve(response);
            })
                .error(function (response, status, headers, config) {
                    defer.reject(response);
                });
            return defer.promise;
        },
        postAnswer: function (options) {
            if (typeof options == "undefined") {
                options = {};
            }
            var defConf = {
                url: "",
                data: {},
                params: {}
            };
            var setting = $.extend(true, defConf, options);
            var defer = $q.defer();
            $http({
                method: 'post',
                url: setting.url,
                headers: { 'Content-Type': 'application/json' },
                params: setting.params, //追加到url后的参数
                data: setting.data, //get放在消息体中的参数
                timeout: config.requestTimeOut
            }).success(function (response, status, headers, config) {
                defer.resolve(response);
            })
                .error(function (response, status, headers, config) {
                    defer.reject(response);
                });
            return defer.promise;
        }
    }
}]);
//获取选股结果
appser.factory('SomeService', ['HttpUtil', 'CAPI', function (HttpUtil, CAPI) {
    return {
        //获取股票列表
        getStockListFir: function (choData, urlParam, successFn, errorFn) {
            HttpUtil.postAnswer({
                url: CAPI.api + "/Index/StockDetail",
                data: choData,
                params: urlParam
            })
                .then(function (data) {
                    /*if(data.error) {
                     if(errorFn){errorFn(data)}
                     } else {*/
                    successFn(data);
                    /*}*/
                }).then(function (error) {

                });
        },
        getStockListMore: function (choData, urlParam, successFn, errorFn) {
            HttpUtil.postAnswer({
                url: CAPI.api + "/Index/StockDetail",
                data: choData,
                params: urlParam
            })
                .then(function (data) {
                    /*if(data.error) {
                     if(errorFn){errorFn(data)}
                     } else {*/
                    successFn(data);
                    /*}*/
                }).then(function (error) {

                });
        },
        getBiaoZhun: function (params, successFn, errorFn) {
            HttpUtil.get({
                //url:"http://10.1.135.80:9999/Index/Detail?$filter=Code eq '"+params+"' &$orderby=SelectTerm desc &$top=1",
                url: CAPI.api + "/Index/Detail?$filter=Code eq '" + params + "' and IsDefault eq true &$top=1"
                //http://10.1.135.80:9999/Index/Detail?$filter=Code eq 'UDPPS' &$orderby=SelectTerm desc &$top=1
                // params:params
            })
                .then(function (data) {
                    if (data.error) {
                        if (errorFn) { errorFn(data) }
                    } else {
                        successFn(data);
                    }
                }).then(function (error) {

                });
        },
        saveSolution: function (params, successFn, errorFn) {

            HttpUtil.postAnswer({
                url: CAPI.api + "/Index/SaveSolution",
                data: params
            })
                .then(function (data) {
                    /*if(data.error) {
                     if(errorFn){errorFn(data)}
                     } else {*/
                    successFn(data);
                    /*}*/
                }).then(function (error) {

                });
        },
        getSoluName: function (successFn, errorFn) {
            var randow = Math.round(Math.random() * 1000000);
            HttpUtil.get({
                url: CAPI.api + "/Index/MySolutions?time=" + randow,
                /*data:params*/
            })
                .then(function (data) {
                    /*if(data.error) {
                     if(errorFn){errorFn(data)}
                     } else {*/
                    successFn(data);
                    /*}*/
                }).then(function (error) {

                });
        },
        getSoluCont: function (params, successFn, errorFn) {

            HttpUtil.get({
                url: CAPI.api + "/Index/MyConditions/" + params,
                /*params:params*/
            })
                .then(function (data) {
                    /*if(data.error) {
                     if(errorFn){errorFn(data)}
                     } else {*/
                    successFn(data);
                    /*}*/
                }).then(function (error) {

                });
        },

    }
}])
;

//获取指标服务
appser.service('dataServices', ['$http', 'CAPI', function ($http, CAPI) {
    var listData = [];
    var listData2 = [];
    var listData3 = [];
    var FinAnalysisData = [];
    var BanlaceSheetData = [];
    var ProfitSheetData = [];
    var CashFlowData = [];
    var ConceptPData = [];
    var MarketPData = [];
    var RegionPData = [];
    var tradePData = [];
    function subStringSrc(str, len, replaceStr) {
        // var chineseRegex = /[^\x00-\xff]/g;
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
    function filterData(arr) {
        return arr.map(function (item, index) {
            var newItem = {};
            newItem.name = item.Name;
            newItem.nameCon = subStringSrc(newItem.name, 14, '...');
            newItem.code = item.Code;
            newItem.titlea = item.Desc;
            return newItem;
        })
    }

    this.getLataData = function (successCallback, errorCallback) {

        if (listData.length == 0) {
            $http.get(
                CAPI.api + "/Index/Tree?$filter=TypeCode eq '0'",
                { headers: { 'Content-Type': 'application/json' } }
            )
                .success(function (data, statusCode, header, config) {
                    listData = data
                    // console.log(567);
                    successCallback(filterData(listData));
                    // console.log(filterData(listData));

                })
                .error(function (error) {
                    errorCallback(error);
                })
        }
        else {
            successCallback(filterData(listData));
        }
    };
    this.getMarketData = function (successCallback, errorCallback) {

        if (listData3.length == 0) {
            $http.get(
                CAPI.api + "/Index/Tree?$filter=TypeCode eq '3'",

                { headers: { 'Content-Type': 'application/json' } }
            )
                .success(function (data, statusCode, header, config) {
                    listData3 = data
                    successCallback(filterData(listData3));
                })
                .error(function (error) {
                    errorCallback(error);
                })
        }
        else {
            successCallback(filterData(listData3));
        }
    }
    this.getTechData = function (successCallback, errorCallback) {

        if (listData2.length == 0) {
            $http.get(
                CAPI.api + "/Index/Tree?$filter=TypeCode eq '4'",

                { headers: { 'Content-Type': 'application/json' } }
            )
                .success(function (data, statusCode, header, config) {
                    listData2 = data
                    // console.log(567);
                    successCallback(filterData(listData2));
                    //console.log(filterData(listData));

                })
                .error(function (error) {
                    errorCallback(error);
                })
        }
        else {
            successCallback(filterData(listData2));
        }
    }

    this.getFinAnalysisData = function (successCallback, errorCallback) {
        if (FinAnalysisData.length == 0) {
            $http.get(
                CAPI.api + "/Index/Tree?$filter=TypeCode eq '1' and SubTypeCode eq '1_1'",

                { headers: { 'Content-Type': 'application/json' } }
            )
                .success(function (data, statusCode, header, config) {
                    FinAnalysisData = data
                    successCallback(filterData(FinAnalysisData));
                })
                .error(function (error) {
                    errorCallback(error);
                })
        }
        else {
            successCallback(filterData(FinAnalysisData));
        }
    }
    this.getBanlaceSheetData = function (successCallback, errorCallback) {
        if (BanlaceSheetData.length == 0) {
            $http.get(
                CAPI.api + "/Index/Tree?$filter=TypeCode eq '1' and SubTypeCode eq '1_2'",

                { headers: { 'Content-Type': 'application/json' } }
            )
                .success(function (data, statusCode, header, config) {
                    BanlaceSheetData = data
                    successCallback(filterData(BanlaceSheetData));
                })
                .error(function (error) {
                    errorCallback(error);
                })
        }
        else {
            successCallback(filterData(BanlaceSheetData));
        }
    }
    this.getProfitSheetData = function (successCallback, errorCallback) {
        if (ProfitSheetData.length == 0) {

            $http.get(
                CAPI.api + "/Index/Tree?$filter=TypeCode eq '1' and SubTypeCode eq '1_3'",
                { headers: { 'Content-Type': 'application/json' } }
            )
                .success(function (data, statusCode, header, config) {
                    ProfitSheetData = data;
                    successCallback(filterData(ProfitSheetData));
                })
                .error(function (error) {
                    errorCallback(error);
                })
        } else {
            successCallback(filterData(ProfitSheetData));
        }
    }
    this.getCashFlowData = function (successCallback, errorCallback) {
        if (CashFlowData.length == 0) {

            $http.get(
                CAPI.api + "/Index/Tree?$filter=TypeCode eq '1' and SubTypeCode eq '1_4'",
                { headers: { 'Content-Type': 'application/json' } }
            )
                .success(function (data, statusCode, header, config) {
                    CashFlowData = data
                    successCallback(filterData(CashFlowData));
                })
                .error(function (error) {
                    errorCallback(error);
                })
        } else {
            successCallback(filterData(CashFlowData));
        }
    }
    this.getConceptPData = function (successCallback, errorCallback) {
        if (ConceptPData.length == 0) {

            $http.get(
                CAPI.api + "/Index/Tree?$filter=TypeCode eq '2' and SubTypeCode eq '2_1'",
                { headers: { 'Content-Type': 'application/json' } }
            )
                .success(function (data, statusCode, header, config) {
                    ConceptPData = data
                    successCallback(filterData(ConceptPData));
                })
                .error(function (error) {
                    errorCallback(error);
                })
        } else {
            successCallback(filterData(ConceptPData));
        }
    }
    this.getMarketPData = function (successCallback, errorCallback) {
        if (MarketPData.length == 0) {

            $http.get(
                CAPI.api + "/Index/Tree?$filter=TypeCode eq '2' and SubTypeCode eq '2_2'",
                { headers: { 'Content-Type': 'application/json' } }
            )
                .success(function (data, statusCode, header, config) {
                    MarketPData = data
                    successCallback(filterData(MarketPData));
                })
                .error(function (error) {
                    errorCallback(error);
                })
        } else {
            successCallback(filterData(MarketPData));
        }
    }
    this.getRegionPData = function (successCallback, errorCallback) {
        if (RegionPData.length == 0) {

            $http.get(
                CAPI.api + "/Index/Tree?$filter=TypeCode eq '2' and SubTypeCode eq '2_3'",
                { headers: { 'Content-Type': 'application/json' } }
            )
                .success(function (data, statusCode, header, config) {
                    RegionPData = data
                    successCallback(filterData(RegionPData));
                })
                .error(function (error) {
                    errorCallback(error);
                })
        } else {
            successCallback(filterData(RegionPData));
        }
    }
    this.getTradePData = function (successCallback, errorCallback) {
        if (tradePData.length == 0) {

            $http.get(
                CAPI.api + "/Index/Tree?$filter=TypeCode eq '2' and SubTypeCode eq '2_4'",
                { headers: { 'Content-Type': 'application/json' } }
            )
                .success(function (data, statusCode, header, config) {
                    tradePData = data
                    successCallback(filterData(tradePData));
                })
                .error(function (error) {
                    errorCallback(error);
                })
        } else {
            successCallback(filterData(tradePData));
        }
    }

}])
