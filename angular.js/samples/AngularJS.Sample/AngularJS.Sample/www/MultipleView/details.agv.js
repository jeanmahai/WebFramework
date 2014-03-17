
(function(){
	var _index = (window.angular.callbacks.counter-1).toString(36);
	var _html='<span>{{id}}</span><script type="text/javascript">    console.info("sdf");</script>';
	window.angular.callbacks['_' + _index](_html);
})();