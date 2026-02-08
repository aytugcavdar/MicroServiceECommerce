using BuildingBlocks.Core.Domain;
using Payment.Domain.Enums;

namespace Payment.Domain.Entities;

public class Payment : Entity<Guid>, IAggregateRoot
{
    public Guid OrderId { get; private set; }
    public Guid UserId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public PaymentMethod Method { get; private set; }
    public string? TransactionId { get; private set; }
    public string? ErrorMessage { get; private set; }
    public DateTime? ProcessedAt { get; private set; }

    private Payment() { }

    public Payment(Guid orderId, Guid userId, decimal amount, PaymentMethod method = PaymentMethod.CreditCard)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        UserId = userId;
        Amount = amount;
        Method = method;
        Status = PaymentStatus.Pending;
        CreatedDate = DateTime.UtcNow;
    }

    public void MarkAsProcessing()
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException($"Cannot process payment in {Status} status");

        Status = PaymentStatus.Processing;
        UpdatedDate = DateTime.UtcNow;
    }

    public void Complete(string transactionId)
    {
        if (Status != PaymentStatus.Processing)
            throw new InvalidOperationException($"Cannot complete payment in {Status} status");

        if (string.IsNullOrWhiteSpace(transactionId))
            throw new ArgumentException("Transaction ID is required", nameof(transactionId));

        Status = PaymentStatus.Completed;
        TransactionId = transactionId;
        ProcessedAt = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;
    }

    public void Fail(string errorMessage)
    {
        if (Status != PaymentStatus.Processing)
            throw new InvalidOperationException($"Cannot fail payment in {Status} status");

        Status = PaymentStatus.Failed;
        ErrorMessage = errorMessage;
        ProcessedAt = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;
    }

    public void Refund()
    {
        if (Status != PaymentStatus.Completed)
            throw new InvalidOperationException("Only completed payments can be refunded");

        Status = PaymentStatus.Refunded;
        UpdatedDate = DateTime.UtcNow;
    }
}
