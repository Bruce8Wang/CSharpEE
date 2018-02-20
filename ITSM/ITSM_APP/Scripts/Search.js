var faultTypeUri = '../api/faultTypes/';
var statusUri = '../api/statuses/';


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


function SearchJS() {
    this.init();
    this.event();
}

SearchJS.prototype = {
    init: function () {
        var that = this;
        //that.GetRepairSearchItem();

        //故障类型
        ajaxHelper(faultTypeUri, 'GET').done(function (data) {
            var faultTypeElement = $("#filterFaultType");
            for (var i = 0; i < data.length; i++) {
                html = '<option value=' + data[i].Id + '>' + data[i].Name + '</option>';
                faultTypeElement.append(html);
            }
        });

        //处理状态
        ajaxHelper(statusUri, 'GET').done(function (data) {
            var statusElement = $("#filterStatus");
            for (var i = 0; i < data.length; i++) {
                html = '<option value=' + data[i].Id + '>' + data[i].Name + '</option>';
                statusElement.append(html);
            }
        });

    },
    event: function () {
        $("#search_searchBtn").click(function () {
            var selFaultType = $('#filterFaultType').val();
            var selStatus = $("#filterStatus").val();
            var keysearch = $("#keysearch").val();

            localStorage.faultType = selFaultType;
            localStorage.status = selStatus;
            localStorage.keysearch = keysearch;

            //$('#search_faultType').val(selFaultType);
            //$('#search_status').val(selStatus);
            //$('#search_key').val(key);

            //var c2 = $("#jobclass2").val();
            //var ky = $("#keysearch").val();
            //localStorage.filter = "FaultTypeId eq " + selFaultType + " and StatusId eq " + selStatus;
            location.href = "/Page/RepairListM.html?listType=1"

            //location.href = "/Page/RepairListM.html?jc=" + jc + "&c1=" + c1 + "&c2=" + c2 + "&ky=" + ky;
        });
    },
    //GetRepairSearchItem: function () {
    //    $.ajax({
    //        type: "Get",
    //        url: "/JobAd/_SearchItem",
    //        dataType: "json",
    //        success: function (data) {
    //            var searchItemHtml = '';
    //            for (var i = 0; i < data.length; i++) {
    //                var searchItemList = data[i];

    //                searchItemHtml += '<div data-role="controlgroup">';
    //                searchItemHtml += '<select data-iconpos="left" name="' + searchItemList.Kind + '" id="' + searchItemList.Kind + '">';
    //                searchItemHtml += '<option value="-1">请选择' + searchItemList.Name + '</option>';
    //                for (var j = 0; j < searchItemList.Items.length; j++) {
    //                    var searchItem = searchItemList.Items[j];
    //                    searchItemHtml += '<option value="' + searchItem.Value + '">' + searchItem.Name + '</option>';
    //                }
    //                searchItemHtml += '</select>';
    //                searchItemHtml += '</div>';
    //            }

    //            $("#searchItem_view").append(searchItemHtml);
    //            $("#searchItem_view").trigger("create");
    //        }
    //    });
    //}

};
