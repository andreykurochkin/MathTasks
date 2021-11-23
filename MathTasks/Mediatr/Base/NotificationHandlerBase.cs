using MathTasks.Data;
using MathTasks.Extensions;
using MathTasks.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MathTasks.Mediatr.Base
{
    public abstract class NotificationHandlerBase<T> : INotificationHandler<T> where T : NotificationBase
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        public NotificationHandlerBase(ILogger<T> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        private string CreateNotificationMessage(NotificationBase notification)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(notification.Exception?.Message);
            stringBuilder.AppendLine(notification.Exception?.InnerException?.Message);
            stringBuilder.AppendLine(notification.Exception?.GetBaseException().Message);
            stringBuilder.AppendLine(notification.Exception?.StackTrace);
            return stringBuilder.ToString();
        }
        public async Task Handle(T notification, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new Notification(notification.Subject, CreateNotificationMessage(notification), notification.AddressFrom, notification.AddressTo);
                await _context.Notifications.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync();
                _logger.NotificationAdded(notification.Subject);
            }
            catch (Exception ex)
            {
                _logger.DatabaseSavingError(nameof(Notification), ex);
            }
        }
    }
}
