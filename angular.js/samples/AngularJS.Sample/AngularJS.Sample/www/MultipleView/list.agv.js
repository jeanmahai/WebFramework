
(function(){
	var _index = (window.angular.callbacks.counter-1).toString(36);
	var _html='<ul><li ng-repeat="user in users"><a href="#/users/{{user.id}}">{{user.name}}</a></li></ul>';
	window.angular.callbacks['_' + _index](_html);
})();