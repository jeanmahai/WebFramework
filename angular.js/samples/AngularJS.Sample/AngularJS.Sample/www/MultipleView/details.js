(function () {
    var html = '<span>{{id}}</span>';
    var _index = window.angular.callbacks.counter.toString(36) - 1;
    window.angular.callbacks['_' + _index](html);
})();