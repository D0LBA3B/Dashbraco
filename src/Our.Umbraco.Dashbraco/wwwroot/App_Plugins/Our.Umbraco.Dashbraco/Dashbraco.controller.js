(function () {
    "use strict";
    function controller($http, userService, dateHelper) {
        var vm = this;
        vm.analyticsData = {};
        $http.get('backoffice/api/Dashbraco/GetAnalyticsData').then(function (res) {
            vm.analyticsData = res.data;
        });

        return vm;
    }
    angular.module("umbraco").controller("Our.Umbraco.Dashbraco.Controller", ['$http', 'userService', 'dateHelper', controller]);
})();
