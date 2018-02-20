var filter, name, company, beginDate, endDate;

var userPermissionUrl = '/api/BD_UserPermission';

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

$(function () {
    //getData('giftCategory');
    //var bCanExport = 0;
    ////获取用户权限
    //ajaxHelper(userPermissionUrl + "?$filter=FD_USER eq'" + localStorage.userId + "' and FD_OBJECTID eq 'rptGiftFetch'", "GET").done(function (data, status) {
    //    if (status == 'success') {
    //        if (data.length > 0) {
    //            for (var i = 0; i < data.length; i++) {
    //                if (data[i].FD_CANWRITE == 1) {
    //                    bCanExport = 1;
    //                    break;
    //                }
    //            }
    //        }

    //        if (bCanExport == 1) $('#btnExport').show();
    //    }
    //});
});

//function getData(type) {
//    switch (type) {
//        case 'giftCategory':
//            ajaxHelper(giftCategoryUrl, 'GET').done(function (data) {
//                var objList = document.getElementById("gift-category");
//                objList.innerHTML = "";
//                if (data.length > 0) {
//                    $("#gift-category").append(generateHtmlOption(data, type));
//                }
//            });
//            break;
//    }
//}

query();
function query() {
    $('#lbNoData').hide();
    GenerateTable();
}

function exportExcel() {
    var objFilter = {
        Name: $('#inputName').val(),
        Company: $('#inputCompany').val(),
        BeginDate: $('#inputBeginDate').val(),
        EndDate: $('#inputEndDate').val()
    };
    ajaxHelper('/api/ExportData', 'POST', objFilter).done(function (data) {
        if (data == null) $('#lbNoData').show();
        else window.open('../Export/' + data.FileName);
    });

}


function GenerateTable() {

    name = $('#inputName').val();
    company = $('#inputCompany').val();
    beginDate = $('#inputBeginDate').val();
    endDate = $('#inputEndDate').val();
    filter = "1 eq 1";

    if (name.length > 0) {
        filter = filter + ' and substringof(\'' + name + '\',Name)'
    }
    if (company.length > 0) {
        filter = filter + ' and substringof(\'' + company + '\',Company)'
    }
    if (beginDate.length > 0) {
        //filter = filter + ' and FD_APPLYDATE ge \'' + beginDate + '\'';
        filter = filter + ' and CreateTime ge DateTime\'' + beginDate + 'T00:00:00\'';

    }
    if (endDate.length > 0) {
        //filter = filter + ' and FD_APPLYDATE le \'' + endDate + '\'';
        filter = filter + ' and CreateTime le DateTime\'' + endDate + 'T23:59:59\'';
    }

    var table = null;
    table = new TableView('table_div');
    table.header = {
        Name: '反馈人',
        Company: '公司名',
        EMail: '邮箱',
        Mobile: '手机号',
        Content: '反馈内容',
        CreateTime: '反馈时间'
    };

    table.dataKey = 'Id';
    table.pager.size = 15;
    ajaxHelper('/api/FeedBacks?$filter=' + filter, 'GET').done(function (data) {
        table.addRange(data);
    });
    table.display["count"] = true;
    table.display["filter"] = true;
    table.display["marker"] = true;
    table.display["pager"] = true;
    table.display["sort"] = true;
    table.display["multiple"] = true;
    table.render();
}




