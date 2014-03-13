using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AngularJS.Tools;

namespace AngularJS.Sample.www.MultipleView
{
    /// <summary>
    /// Summary description for GetView
    /// </summary>
    public class GetView : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/javascript";
            context.Response.Write(AngularCallback.FormatJSONP(@"D:\jean.h.ma\Downloads\github\WebFramework\angular.js\samples\AngularJS.Sample\AngularJS.Sample\www\MultipleView\details.htm"));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}