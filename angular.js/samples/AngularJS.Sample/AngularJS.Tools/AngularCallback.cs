using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;

namespace AngularJS.Tools
{
    public class AngularCallback
    {
        private const string JSON_CALLBACK = "JSON_CALLBACK";

        private static HttpResponse Response
        {
            get
            {
                return HttpContext.Current.Response;
            }
        }
        private static HttpRequest Request
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }

        private static string CallbackVal
        {
            get { return Request.QueryString["callback"]; }
        }

        private static bool IsOrigion()
        {
            return CallbackVal.Contains("angular.callbacks._");
        }

        private static string BuildStr(string str, bool isJson = true)
        {
            if (isJson)
                return @"
                    (function(){
                        var _index=window.angular.callbacks.counter.toString(36)-1;
                        window.angular.callbacks['_' + _index](" + str + @");
                    })();
                ";
            return @"
                    (function(){
                        var _index=window.angular.callbacks.counter.toString(36)-1;
                        window.angular.callbacks['_' + _index]('" + str + @"');
                    })();
                ";
        }

        /// <summary>
        /// 格式化JSONP
        /// </summary>
        /// <param name="obj">需要回传的数据</param>
        /// <returns></returns>
        public static string FormatJSONP(object obj)
        {
            Response.ContentType = "application/javascript";
            var json = new JavaScriptSerializer().Serialize(obj);
            string str;
            if (IsOrigion())
            {
                str = BuildStr(json);
            }
            else
            {
                str = CallbackVal + "(" + json + ");";
            }
            return str;
        }
        public static string FormatJSONP(string file) {
            Response.ContentType = "application/javascript";
            var cnt = File.ReadAllText(file);
            return BuildStr(cnt, false);
        }
    }
}
