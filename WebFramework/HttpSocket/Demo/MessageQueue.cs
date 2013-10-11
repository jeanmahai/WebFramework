using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo
{
    public static class MessageQueue
    {
        private static List<Message> Messages = new List<Message>();
        private static object LockObject = new object();

        public static List<Message> GetMessage(string to)
        {
            var query = (from a in Messages
                         where a.To == to && a.IsPush == false
                         select a).ToList();
            query.ForEach(p =>
            {
                p.IsPush = true;
                p.PullDate = DateTime.Now;
            });
            return query;
        }
        public static void AddMessage(Message message)
        {
            lock (LockObject)
            {
                Messages.Add(message);
            }
        }
    }
}