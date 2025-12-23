using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Domain.Events;

public class NotificationSentDomainEvent:IDomainEvent
{
    public Guid NotificationId { get; }
    public string RecipientEmail { get; }
    public string EventType { get; }
    public DateTime OccurredOn { get; }

    public NotificationSentDomainEvent(
        Guid notificationId,
        string recipientEmail,
        string eventType)
    {
        NotificationId = notificationId;
        RecipientEmail = recipientEmail;
        EventType = eventType;
        OccurredOn = DateTime.UtcNow;
    }
}
