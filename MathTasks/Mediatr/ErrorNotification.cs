using MathTasks.Data;
using MathTasks.Mediatr.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using MathTasks.Providers;

namespace MathTasks.Mediatr
{
    public class ErrorNotification : NotificationBase
    {
        public ErrorNotification(string subject, string content, string addressFrom, string addressTo, Exception? exception = null) 
            : base(
                  subject: "Error", 
                  content, 
                  addressFrom: new DefaultRobotEmailProvider().ToString(), 
                  addressTo: new DefaultEmailProvider().ToString(), exception)
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
