using Notification.Domain.Entities;

namespace Notification.Application.Features.Notifications.Queries.GetNotificationHistory;

public class GetNotificationHistoryResponse
{
    public Guid Id { get; set; }
    public NotificationType Type { get; set; }
    public string RecipientEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public NotificationStatus Status { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime CreatedDate { get; set; }
}
