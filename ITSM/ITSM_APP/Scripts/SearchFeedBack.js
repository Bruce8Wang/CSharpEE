var Uri = '../api/faultTypes/';



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


    },
    event: function () {
        $("#search_searchBtn").click(function () {
            var keysearch = $("#keysearch").val();
            localStorage.keysearch = keysearch;

            //$('#search_faultType').val(selFaultType);
            //$('#search_status').val(selStatus);
            //$('#search_key').val(key);

            //var c2 = $("#jobclass2").val();
            //var ky = $("#keysearch").val();
            //localStorage.filter = "FaultTypeId eq " + selFaultType + " and StatusId eq " + selStatus;

            location.href = "/Page/FeedBackListM.html";

            //location.href = "/Page/RepairListM.html?jc=" + jc + "&c1=" + c1 + "&c2=" + c2 + "&ky=" + ky;
        });
    },
};
