using MathTasks.Data;
using MathTasks.Mediatr.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace MathTasks.Mediatr
{
    public class ErrorNotification : NotificationBase
    {
        public ErrorNotification(string subject, string content, string addressFrom, string addressTo, Exception? exception = null) : base(subject, content, addressFrom, addressTo, exception)
        {
        }
    }

    public class ErrorNotificationHandler : NotificationHandlerBase<ErrorNotification>
    {
        public ErrorNotificationHandler(ILogger<ErrorNotification> logger, ApplicationDbContext context) : base(logger, context)
        {

        }
    }

}
