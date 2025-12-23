using BuildingBlocks.Infrastructure.EntityFramework;
using Notification.Application.Services;
using Notification.Domain.Entities;
using Notification.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Repositories;

public class NotificationLogRepository
    : EfRepositoryBase<NotificationLog, Guid, NotificationDbContext>,
      INotificationLogRepository
{
    public NotificationLogRepository(NotificationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<NotificationLog>> GetFailedNotificationsAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.NotificationLogs
            .Where(n => n.Status == NotificationStatus.Failed && n.RetryCount < 3)
            .OrderBy(n => n.CreatedDate)
            .Take(50)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<NotificationLog>> GetPendingNotificationsAsync(
        int count = 100,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.NotificationLogs
            .Where(n => n.Status == NotificationStatus.Pending)
            .OrderBy(n => n.CreatedDate)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
}
