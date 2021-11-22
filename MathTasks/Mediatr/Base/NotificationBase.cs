using MediatR;
using System;

namespace MathTasks.Mediatr.Base
{
    public abstract class NotificationBase : INotification
    {
        protected NotificationBase(string subject, string content, string addressFrom, string addressTo, Exception? exception = null)
        {
            Subject = subject;
            Content = content;
            AddressFrom = addressFrom;
            AddressTo = addressTo;
        }
        public string Subject { get; }
        public string Content { get; }
        public Exception? Exception { get; }
        public string AddressFrom { get; }
        public string AddressTo { get; }
    }
}
