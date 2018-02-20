
//
//    dataSrcALL.indexDetArr=[];

/**
 * 判断id是否在某一数组中
 */
function isCantain(arr, key, val) {
    var len = arr.length;
    if (len) {
        for (var i = 0; i < len; i++) {
            if (val == arr[i][key]) {
                return true;
            }
        }
    }
    return false;
}
function delEle(arr, key, val) {
    var len = arr.length;
    if (len) {
        for (var i = 0; i < len; i++) {
            if (val == arr[i][key]) {
                arr.splice(i, 1);
                break;
            }
        }
    }
    return arr;
}
/**
 * 避免keyup事件被多次调用
 * @type {boolean}
 */
var dbclick = true;
var dbClick = function () {
    if (dbclick) {
        dbclick = false;
        setTimeout(function () {
            dbclick = true;
        }, 500)
    }
    return dbclick
};
/**配置文件**/
var config = {
    baseUri: '',
    requestTimeOut: 10000
}

var app = angular.module('app', ['app.services', 'app.controllers']);


angular.module('app.services', []);
angular.module('app.controllers', []);
app.constant("CAPI", {
    "api": "http://10.1.135.80:9999",
});

function needArr(str, arr) {
    for (var i = 0, j = arr.length; i < j; i++) {
        if (arr[i].TermDisplay == str) {
            return arr[i].Value;
        };
    }
}
