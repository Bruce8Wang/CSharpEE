
var ViewModel = function () {
    var self = this;
    self.dealMethods = ko.observableArray();
    self.error = ko.observable();
    self.detail = ko.observable();
    self.newDealMethod = {
       Name: ko.observable(),
       Note: ko.observable()
    }

    var dealMethodsUri = '../api/dealMethods/';

    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    function getAllDealMethods() {
        ajaxHelper(dealMethodsUri, 'GET').done(function (data) {
            self.dealMethods(data);
        });
    }

    self.getDealMethodDetail = function (item) {
        ajaxHelper(dealMethodsUri + item.Id, 'GET').done(function (data) {
            self.detail(data);
        });
    }

    self.addDealMethod = function (formElement) {
        var dealMethod = {
           Name: self.newDealMethod.Name(),
           Note: self.newDealMethod.Note()
        };

        ajaxHelper(dealMethodsUri, 'POST', dealMethod).done(function (item) {
            self.dealMethods.push(item);
        });
    }

    //etch the initial data.
    getAllDealMethods();
};

ko.applyBindings(new ViewModel());