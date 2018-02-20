var imageFile = "";

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
    var Id = getQueryString('id');
    if (Id != null) {
        var Url = '/api/FeedBacks?$filter=Id eq ' + Id;
        ajaxHelper(Url, 'GET').done(function (data) {
            if (data.length > 0) {
                $('#inputName').val(data[0].Name);
                $('#inputCompany').val(data[0].Company);
                $('#inputEMail').val(data[0].EMail);
                $('#inputMobile').val(data[0].Mobile);
                $('#inputContent').text(data[0].Content);
                $('#inputCreateTime').val(data[0].CreateTime.replace('T', ' '));
                imageFile = data[0].FilePath;

                if (imageFile != "") {
                    $('#aImg').show();
                    $("#img0").attr("src", "http://oa.sztechand.com:9998/Images/" + imageFile);
                    $("#aImg").attr("href", "/Images/" + imageFile);
                }


            }
        });
    }
});

//获取页面参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
