网址:http://angularjs.org/

关于跨域的问题:

在phonegap中确保如下配置:
res\xml\config.xml
确认里面有行配置  <access origin="*"/>

在chrome中调试时,chrome加启动参数:
--disable-web-security
