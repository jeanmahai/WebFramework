﻿var userControllers = angular.module("userControllers", []);

userControllers.controller("UserListCtrl", ["$scope", "$http", function ($scope, $http) {
    $http.jsonp("../jsonData/UsersListHandler.js?callback=UsersListHandler");
    window.UsersListHandler = function (data) {
        $scope.users = data;
        delete window.UsersListHandler;
    };
} ]);

userControllers.controller("UserDetailsCtrl", ["$scope", "$routeParams", function ($scope, $routeParams) {
    $scope.id = $routeParams.id;

    console.info($scope);

    $scope.$on("$viewContentLoaded", function (name, listener) {
        console.info(name);
        console.info(listener);
        console.info("view content loaded.");
    });
} ]);