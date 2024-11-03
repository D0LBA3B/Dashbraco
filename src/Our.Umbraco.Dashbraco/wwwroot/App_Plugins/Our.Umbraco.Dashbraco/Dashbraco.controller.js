(function () {
    "use strict";
    function controller($http, userService, dateHelper, $sce) {
        var vm = this;
        vm.analyticsData = {};
        vm.pictureOfTheDay = {};
        vm.activeTab = 'analytics';
        vm.setActiveTab = function (tab) {
            vm.activeTab = tab;
        };

        //Config
        $http.get('backoffice/api/Dashbraco/GetConfig').then(function (configRes) {
            var defaultWidgets = configRes.data.defaultWidgets;
            vm.showPictureOfTheDay = defaultWidgets.includes("Picture of the Day");

            if (vm.showPictureOfTheDay) {
                userService.getCurrentUser().then(function (user) {
                    vm.pictureOfTheDayTitle = "Welcome " + user.name + ", here is an umbazing picture for you !!!";
                });

                $http.get("https://api.nasa.gov/planetary/apod?api_key=9LeHWFqoCqowcoqRsAuDWTV0Qvg06p12hMw4Qvyg&hd=true")
                    .then(function (response) {
                        vm.pictureOfTheDay = response.data;
                    });
            }
        });

        $http.get('backoffice/api/Dashbraco/GetAnalyticsData').then(function (res) {
            vm.analyticsData = res.data;
        });

        vm.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };

        return vm;
    }

    angular.module("umbraco").controller("Our.Umbraco.Dashbraco.Controller", ['$http', 'userService', 'dateHelper', '$sce', controller]);
})();
