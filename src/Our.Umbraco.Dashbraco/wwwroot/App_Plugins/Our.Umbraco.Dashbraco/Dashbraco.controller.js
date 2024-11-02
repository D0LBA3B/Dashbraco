(function () {
    "use strict";
    function controller($http, userService, dateHelper) {
        /**
         * Processes dates into a nice format based on user culture.
         * @param {any} arrItemsToProcess
         * @param {any} callback
         */
        var processDates = function (arrItemsToProcess, callback) {
            userService.getCurrentUser().then(function (currentUser) {
                angular.forEach(arrItemsToProcess, function (item) {
                    item.datestampFormatted = dateHelper.getLocalDate(item.datestamp, currentUser.locale, 'LLL');

                    if (item.scheduledPublishDate != null) {
                        item.scheduledPublishDateFormatted = dateHelper.getLocalDate(item.datestamp, currentUser.locale, 'LLL');
                    }
                });
                callback();
            });
        }

        var vm = this
        $http.get('backoffice/api/Dashbraco/GetAllRecentActivities').then(function (res) {
        });

        vm.clickElement = function (elementSelector) {
            // Just clicks on the element provides as parameter.
            $(elementSelector).click();
        }
        return vm;
    }
    angular.module("umbraco").controller("Our.Umbraco.Dashbraco.Controller", ['$http', 'userService', 'dateHelper', controller]);
})();
