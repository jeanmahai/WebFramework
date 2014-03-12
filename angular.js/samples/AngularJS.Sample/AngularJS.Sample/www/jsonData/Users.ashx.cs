using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using AngularJS.Tools;

namespace AngularJS.Sample.www.jsonData
{
    /// <summary>
    /// Summary description for Users
    /// </summary>
    public class Users : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/javascript";
            var ls = new List<object>();
            ls.Add(new
            {
                name = "jean",
                sex = "male",
                age = 1
            });
            ls.Add(new
            {
                name = "eva",
                sex = "female",
                age = 2
            });
            //context.Response.Write(AngularCallback.FormatJSONP(ls));
            context.Response.Write(AngularCallback.FormatJSONP(ls));
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