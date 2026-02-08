using Microsoft.Extensions.Logging;
using Payment.Application.Services;
using Payment.Domain.Constants;

namespace Payment.Infrastructure.Gateways;

/// <summary>
/// Mock payment gateway for demonstration purposes.
/// Simulates payment processing with configurable success rate.
/// In production, this would integrate with Stripe, PayPal, etc.
/// </summary>
public class MockPaymentGateway : IPaymentGateway
{
    private readonly ILogger<MockPaymentGateway> _logger;
    private readonly Random _random;
    
    // Configurable success rate (80% success by default)
    private const double SuccessRate = 0.8;

    public MockPaymentGateway(ILogger<MockPaymentGateway> logger)
    {
        _logger = logger;
        _random = new Random();
    }

    public async Task<PaymentResult> ProcessPaymentAsync(
        Guid orderId, 
        decimal amount, 
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("🏦 Mock Gateway: Processing payment for Order {OrderId}, Amount: {Amount:C}", 
            orderId, amount);

        // Simulate processing delay (1-3 seconds)
        var delay = _random.Next(1000, 3000);
        await Task.Delay(delay, cancellationToken);

        // Validate amount
        if (amount < PaymentConstants.MinimumPaymentAmount)
        {
            _logger.LogWarning("❌ Mock Gateway: Amount too low: {Amount}", amount);
            return new PaymentResult(false, null, "Amount is below minimum threshold");
        }

        if (amount > PaymentConstants.MaximumPaymentAmount)
        {
            _logger.LogWarning("❌ Mock Gateway: Amount too high: {Amount}", amount);
            return new PaymentResult(false, null, "Amount exceeds maximum limit");
        }

        // Random success/failure based on success rate
        var isSuccess = _random.NextDouble() < SuccessRate;

        if (isSuccess)
        {
            var transactionId = $"TXN-{Guid.NewGuid():N}".ToUpperInvariant()[..20];
            _logger.LogInformation("✅ Mock Gateway: Payment successful, TransactionId: {TransactionId}", transactionId);
            return new PaymentResult(true, transactionId, null);
        }
        else
        {
            // Random failure reason
            var errorMessages = new[]
            {
                PaymentConstants.ErrorMessages.InsufficientFunds,
                PaymentConstants.ErrorMessages.CardDeclined,
                PaymentConstants.ErrorMessages.ProcessingError,
                PaymentConstants.ErrorMessages.NetworkError
            };
            
            var errorMessage = errorMessages[_random.Next(errorMessages.Length)];
            _logger.LogWarning("❌ Mock Gateway: Payment failed, Error: {Error}", errorMessage);
            return new PaymentResult(false, null, errorMessage);
        }
    }
}
