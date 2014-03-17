var ngApp = angular.module("ngApp", [
    "ngRoute",
    "userControllers"
]);

ngApp.config(["$routeProvider", function ($routeProvider) {
    //配置导航
    $routeProvider.when("/users", {
        templateUrl: "list.agv.js",
        controller: "UserListCtrl",
        method: "jsonp"
    }).when("/users/:id", {
        templateUrl: "details.agv.js",
        //templateUrl: "http://localhost:58723/www/MultipleView/GetView.ashx",
        controller: "UserDetailsCtrl",
        method: "jsonp"
    }).otherwise({
        redirectTo: "/users"
    });

} ]);