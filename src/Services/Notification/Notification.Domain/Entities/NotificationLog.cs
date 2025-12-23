using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Domain.Entities;

public class NotificationLog:Entity<Guid>,IAggregateRoot
{
    public NotificationType Type { get; set; }
    public string RecipientEmail { get; set; }
    public string? RecipientPhone { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public NotificationStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? SentAt { get; set; }
    public int RetryCount { get; set; }

    public string? TemplateName { get; set; }
    public string? EventType { get; set; } 
    public Guid? RelatedEntityId { get; set; } 

    public NotificationLog()
    {
        RecipientEmail = string.Empty;
        Subject = string.Empty;
        Content = string.Empty;
    }

    public NotificationLog(
        NotificationType type,
        string recipientEmail,
        string subject,
        string content,
        string? templateName = null,
        string? eventType = null,
        Guid? relatedEntityId = null)
    {
        Id = Guid.NewGuid();
        Type = type;
        RecipientEmail = recipientEmail;
        Subject = subject;
        Content = content;
        Status = NotificationStatus.Pending;
        RetryCount = 0;
        TemplateName = templateName;
        EventType = eventType;
        RelatedEntityId = relatedEntityId;
        CreatedDate = DateTime.UtcNow;
    }

    public void MarkAsSent()
    {
        Status = NotificationStatus.Sent;
        SentAt = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;
    }

    public void MarkAsFailed(string errorMessage)
    {
        Status = NotificationStatus.Failed;
        ErrorMessage = errorMessage;
        RetryCount++;
        UpdatedDate = DateTime.UtcNow;
    }

    public void MarkAsRetrying()
    {
        Status = NotificationStatus.Retrying;
        RetryCount++;
        UpdatedDate = DateTime.UtcNow;
    }

    public bool CanRetry() => RetryCount < 3;
}

public enum NotificationType
{
    Email = 1,
    Sms = 2,
    Push = 3
}

public enum NotificationStatus
{
    Pending = 1,
    Sent = 2,
    Failed = 3,
    Retrying = 4
}

