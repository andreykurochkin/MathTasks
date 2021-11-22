using MathTasks.Data;
using MathTasks.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
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
        public async Task Handle(T notification, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new Notification(notification.Subject, notification.Content, notification.AddressFrom, notification.AddressTo);
                await _context.Notifications.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                _logger.DatabaseSavingError();
            }
        }
    }
}
