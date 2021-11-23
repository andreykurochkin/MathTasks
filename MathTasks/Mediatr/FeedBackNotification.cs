using MathTasks.Data;
using MathTasks.Mediatr.Base;
using Microsoft.Extensions.Logging;
using System;
using MathTasks.Providers;

namespace MathTasks.Mediatr
{
    public class FeedBackNotification : NotificationBase
    {
        public FeedBackNotification(string content, Exception? exception = null)
            : base(
                  subject: "Feedback",
                  content,
                  addressFrom: new DefaultRobotEmailProvider().ToString(),
                  addressTo: new DefaultEmailProvider().ToString(),
                  exception) { }
    }

    public class FeedBackNotificationHandler : NotificationHandlerBase<FeedBackNotification>
    {
        public FeedBackNotificationHandler(ILogger<FeedBackNotification> logger, ApplicationDbContext context) : base(logger, context)
        {

        }
    }
}
