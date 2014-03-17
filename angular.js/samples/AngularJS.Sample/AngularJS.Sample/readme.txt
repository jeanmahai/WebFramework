1. 原生的$routeProvider不支持跨域切换view,现在进行了如下修改,调用时如果加上method=jsonp将进行跨域调用,
修改了angular-route.js 533行代码.

2.AngularAddin 添加了离线angular template 的自动生成.