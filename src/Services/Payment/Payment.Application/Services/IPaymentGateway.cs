namespace Payment.Application.Services;

public record PaymentResult(bool IsSuccess, string? TransactionId, string? ErrorMessage);

public interface IPaymentGateway
{
    Task<PaymentResult> ProcessPaymentAsync(Guid orderId, decimal amount, Guid userId, CancellationToken cancellationToken = default);
}
