using BuildingBlocks.Core.Repositories;
using Notification.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.Services;

public interface INotificationLogRepository : IAsyncRepository<NotificationLog, Guid>
{
    Task<List<NotificationLog>> GetFailedNotificationsAsync(
        CancellationToken cancellationToken = default);

    Task<List<NotificationLog>> GetPendingNotificationsAsync(
        int count = 100,
        CancellationToken cancellationToken = default);
}
