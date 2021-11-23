using Microsoft.Extensions.Logging;
using System;

namespace MathTasks.Extensions
{
    public static class EventIdentifiers
    {
        public static readonly EventId DatabaseSavingErrorId = new (7001, "DatabaseSavingError");
        public static readonly EventId NotificationAddedId = new(7004, "NotificationAdded");
    }
    public static class LoggerExtensions
    {
        public static void NotificationAdded(this ILogger source, string subject)
        {
            NotificationAddedExecute(source, subject, null);
        }

        private static readonly Action<ILogger, string, Exception?> NotificationAddedExecute =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                EventIdentifiers.NotificationAddedId,
                "New notification created: {subject}");

        public static void DatabaseSavingError(this ILogger source, string entityName, Exception? exception = null)
        {
            DatabaseSavingErrorExecute(source, entityName, exception);
        }

        private static readonly Action<ILogger, string, Exception?> DatabaseSavingErrorExecute = 
            LoggerMessage.Define<string>(
                LogLevel.Information,
                EventIdentifiers.DatabaseSavingErrorId, 
                "{entityName}");
    }
}
