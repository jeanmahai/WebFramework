using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Demo
{
    public partial class MessageHandler : System.Web.UI.Page
    {
        public string From
        {
            get { return Request.QueryString["From"]; }
        }
        public string To
        {
            get { return Request.QueryString["To"]; }
        }
        public string Action
        {
            get { return Request.QueryString["Action"]; }
        }
        public string Msg
        {
            get { return Request.QueryString["Msg"]; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Action == "get")
            {
                var messages = MessageQueue.GetMessage(To);
                while (messages.Count <= 0)
                {
                    Thread.Sleep(1000);
                    messages = MessageQueue.GetMessage(To);
                }
                Response.Clear();
                Response.Write(new JavaScriptSerializer().Serialize(messages));
                Response.End();
            }
            if(Action=="send")
            {
                MessageQueue.AddMessage(new Message
                                            {
                                                ReceiveDate=DateTime.Now,
                                                From=From,
                                                Content = Msg,
                                                To=To
                                            });
                Response.Clear();
                Response.Write("send success");
                Response.End();
            }
        }
    }
}