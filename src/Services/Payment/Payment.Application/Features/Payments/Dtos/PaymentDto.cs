using Payment.Domain.Enums;

namespace Payment.Application.Features.Payments.Dtos;

public class PaymentDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public PaymentMethod Method { get; set; }
    public string? TransactionId { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public DateTime CreatedDate { get; set; }
}
