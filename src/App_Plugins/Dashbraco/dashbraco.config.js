angular.module("umbraco").requires.push("dashbracoModule");

angular.module("dashbracoModule", [])
    .controller("dashbracoController", "dashboardController");
