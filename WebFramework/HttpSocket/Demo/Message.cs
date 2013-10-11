using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo
{
    public class Message
    {
        public bool IsPush { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime PullDate { get; set; }
        public string Content { get; set; }
    }
}