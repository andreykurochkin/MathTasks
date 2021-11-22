using MathTasks.Mediatr.Base;
using System;

namespace MathTasks.Mediatr
{
    public class FeedBackNotification : NotificationBase
    {
        public FeedBackNotification(string subject, string content, string addressFrom, string addressTo, Exception? exception = null) : base(subject, content, addressFrom, addressTo, exception)
        {
        }
    }
}
