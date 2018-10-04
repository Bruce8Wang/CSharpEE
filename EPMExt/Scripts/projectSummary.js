var url = '/api/ProjectSummaries';
function ajaxHelper(uri, method, data) {
    return $.ajax({
        type: method,
        url: uri,
        async: false,
        dataType: 'json',
        contentType: 'application/json',
        data: data ? JSON.stringify(data) : null
    }).fail(function (jqXHR, textStatus, errorThrown) {

    });
}

function getData() {
    ajaxHelper(storeHouseUrl, 'GET').done(function (data) {
        var objList = document.getElementById("storehouse");
        objList.innerHTML = "";
        if (data.length > 0) {
            $("#storehouse").append(generateHtmlOption(data, type));
        }
    });
}