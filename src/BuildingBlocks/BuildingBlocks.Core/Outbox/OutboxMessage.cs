using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Core.Outbox;

public class OutboxMessage : Entity<Guid>
{
    /// <summary>
    /// Event'in tam tipi (AssemblyQualifiedName)
    /// Örnek: "Identity.Domain.Events.UserCreatedDomainEvent, Identity.Domain"
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Event'in JSON içeriği
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Event ne zaman oluşturuldu
    /// </summary>
    public DateTime OccurredOn { get; set; }

    /// <summary>
    /// Event ne zaman işlendi (null = henüz işlenmedi)
    /// </summary>
    public DateTime? ProcessedOn { get; set; }

    /// <summary>
    /// Hata mesajı (varsa)
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Kaç kere denendi
    /// </summary>
    public int RetryCount { get; set; }

    /// <summary>
    /// EF Core için parameteresiz constructor
    /// </summary>
    public OutboxMessage()
    {
        Type = string.Empty;
        Content = string.Empty;
    }

    /// <summary>
    /// Yeni outbox mesajı oluştur
    /// </summary>
    public OutboxMessage(string type, string content)
    {
        Id = Guid.NewGuid();
        Type = type;
        Content = content;
        OccurredOn = DateTime.UtcNow;
        CreatedDate = DateTime.UtcNow;
        RetryCount = 0;
    }

    /// <summary>
    /// Mesajı başarılı olarak işaretle
    /// </summary>
    public void MarkAsProcessed()
    {
        ProcessedOn = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Mesajı hatalı olarak işaretle
    /// </summary>
    public void MarkAsFailed(string error)
    {
        Error = error;
        RetryCount++;
        UpdatedDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Mesaj işlendi mi?
    /// </summary>
    public bool IsProcessed => ProcessedOn.HasValue;

    /// <summary>
    /// Mesaj başarısız mı? (3 kereden fazla denendi)
    /// </summary>
    public bool IsFailed => RetryCount >= 3;
}