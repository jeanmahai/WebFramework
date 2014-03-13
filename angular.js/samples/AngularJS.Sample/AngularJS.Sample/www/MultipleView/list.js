(function () {
    var _index = window.angular.callbacks.counter.toString(36) - 1;
    window.angular.callbacks['_' + _index]('<ul><li ng-repeat="user in users"><a href="#/users/{{user.id}}">{{user.name}}</a></li></ul>');
})();


