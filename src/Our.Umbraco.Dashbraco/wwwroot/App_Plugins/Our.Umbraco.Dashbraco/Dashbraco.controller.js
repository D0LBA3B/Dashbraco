(function () {
    "use strict";
    function controller($http, userService, dateHelper, $sce) {
        var vm = this;
        vm.analyticsData = {};
        vm.pictureOfTheDay = {};
        vm.unusedMedia = [];
        vm.haveAnalyticsError = false;
        vm.isProcessingMedia = false;
        vm.totalAmountOfMedia = 0;
        vm.totalUnusedMedia = 0;
        vm.activeTab = null;

        vm.setActiveTab = function (tab) {
            vm.activeTab = tab;
            if (tab === 'unusedMedia') {
                vm.getUnusedMediaReport();
            }
            if (tab === 'analytics') {
                vm.loadAnalyticsData();
            }
        };

        vm.startUnusedMediaReport = function () {
            $http.get('backoffice/api/Dashbraco/StartUnusedMediaReport')
                .then(function (response) {
                    vm.isProcessingMedia = true;
                    vm.unusedMedia = [];
                })
                .catch(function (error) {
                    console.error("Error starting media report: ", error);
                });
        };

        vm.getUnusedMediaReport = function () {
            $http.get('backoffice/api/Dashbraco/GetUnusedMediaReport')
                .then(function (response) {
                    vm.isProcessingMedia = false;
                    vm.unusedMedia = response.data;
                    vm.totalUnusedMedia = vm.unusedMedia.length;
                    vm.totalAmountOfMedia = response.data.totalAmountOfMedia || vm.unusedMedia.length;
                })
                .catch(function (error) {
                    console.error("Error fetching unused media report: ", error);
                });
        };

        vm.moveItemToRecycling = function (mediaId) {
            $http.post(`backoffice/api/Dashbraco/MoveItemToRecycling?mediaId=${mediaId}`)
                .then(function (response) {
                    vm.unusedMedia = vm.unusedMedia.filter(item => item.id !== mediaId);
                    vm.totalUnusedMedia = vm.unusedMedia.length;
                })
                .catch(function (error) {
                    console.error("Error moving media to recycling: ", error);
                });
        };

        $http.get('backoffice/api/Dashbraco/GetConfig').then(function (configRes) {
            var defaultWidgets = configRes.data.defaultWidgets;
            vm.showPictureOfTheDay = defaultWidgets.includes("PictureOfTheDay");
            vm.showUnusedMedia = defaultWidgets.includes("UnusedMedia");
            vm.showEntriesActivites = defaultWidgets.includes("EntriesActivites");

            var colors = configRes.data.styles;
            if (colors) {
                document.documentElement.style.setProperty('--primary-color', colors.primaryColor);
                document.documentElement.style.setProperty('--secondary-color', colors.secondaryColor);
                document.documentElement.style.setProperty('--text-color', colors.textColor);
                document.documentElement.style.setProperty('--active-tab-color', colors.activeTabColor);
                document.documentElement.style.setProperty('--inactive-tab-color', colors.inactiveTabColor);
                document.documentElement.style.setProperty('--background-color', colors.backgroundColor);
                document.documentElement.style.setProperty('--border-color', colors.borderColor);
                document.documentElement.style.setProperty('--hover-color', colors.hoverColor);
                document.documentElement.style.setProperty('--button-color', colors.buttonColor);
                document.documentElement.style.setProperty('--button-text-color', colors.buttonTextColor);
                document.documentElement.style.setProperty('--success-color', colors.successColor);
                document.documentElement.style.setProperty('--warning-color', colors.warningColor);
                document.documentElement.style.setProperty('--error-color', colors.errorColor);
                document.documentElement.style.setProperty('--link-color', colors.linkColor);
                document.documentElement.style.setProperty('--link-hover-color', colors.linkHoverColor);
            }

            if (vm.showPictureOfTheDay) {
                vm.activeTab = 'pictureOfTheDay';
                userService.getCurrentUser().then(function (user) {
                    vm.pictureOfTheDayTitle = "Welcome " + user.name + ", here is an umbazing picture for you!";
                });

                $http.get("https://api.nasa.gov/planetary/apod?api_key=9LeHWFqoCqowcoqRsAuDWTV0Qvg06p12hMw4Qvyg&hd=true")
                    .then(function (response) {
                        vm.pictureOfTheDay = response.data;
                    });
            }
        });

        vm.loadAnalyticsData = function () {
            $http.get('backoffice/api/Dashbraco/CheckGoogleAnalyticsConfig').then(function (res) {
                if (res?.data?.item1 == "true") {
                    $http.get('backoffice/api/Dashbraco/GetAnalyticsData').then(function (res) {
                        vm.analyticsData = res.data;

                        if (vm.analyticsData.bounceRate) {
                            vm.analyticsData.bounceRate = (parseFloat(vm.analyticsData.bounceRate) * 100).toFixed(2);
                        }
                    });

                    $http.get('backoffice/api/Dashbraco/GetDailyActiveUsers').then(function (res) {
                        vm.dailyActiveUsers = res.data;
                        vm.initChart();
                    });
                }
                else {
                    vm.haveAnalyticsError = true;
                    vm.analyticsErrorContent = res?.data?.item2 || "An error occurred with the analytics configuration.";
                }
            });
        };

        vm.initChart = function () {
            var dates = vm.dailyActiveUsers.map(data => data.date);
            var activeUsers = vm.dailyActiveUsers.map(data => data.activeUsers);

            var options = {
                chart: {
                    type: 'bar',
                    height: 350
                },
                series: [{
                    name: 'Active users',
                    data: dates.map((date, index) => [new Date(date).getTime(), activeUsers[index]])
                }],
                xaxis: {
                    type: 'datetime',
                    title: { text: 'Date' }
                },
                yaxis: {
                    title: { text: 'Active users' }
                },
                title: {
                    text: "Number of active users per day(last 30 days)"
                }
            };

            var chart = new ApexCharts(document.querySelector("#analyticsChart"), options);
            chart.render();
        };

        vm.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };
        return vm;
    }

    angular.module("umbraco").controller("Our.Umbraco.Dashbraco.Controller", ['$http', 'userService', 'dateHelper', '$sce', controller]);
})();
