var ngApp = angular.module("ngApp", [
    "ngRoute",
    "userControllers"
]);

ngApp.config(["$routeProvider", function ($routeProvider) {
    //配置导航
    $routeProvider.when("/users", {
        templateUrl: "list.htm",
        controller: "UserListCtrl"
    }).when("/users/:id", {
        templateUrl: "details.htm",
        //templateUrl: "http://localhost:58723/www/MultipleView/GetView.ashx",
        controller: "UserDetailsCtrl",
        method:"jsonp"
    }).otherwise({
        redirectTo: "/users"
    });
} ]);