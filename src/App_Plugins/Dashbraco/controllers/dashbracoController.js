angular.module("umbraco").requires.push("dashbracoModule");

angular.module("dashbracoModule", [])
    .controller("dashbracoController", function ($scope, $http) {
        $scope.modules = [];

        $http.get("/umbraco/backoffice/api/DashbracoApi/GetDashbracoSettings")
            .then(function (response) {
                $scope.settings = response.data;
                $scope.modules = $scope.settings.EnabledModules;
                $scope.refreshInterval = $scope.settings.RefreshInterval;
            })
            .catch(function (error) {
                console.error("Error fetching dashboard settings:", error);
            });
    });
