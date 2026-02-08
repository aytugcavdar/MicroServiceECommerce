using BuildingBlocks.Messaging.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Payment.Application.Services;
using Payment.Domain.Enums;

namespace Payment.Application.Consumers;

/// <summary>
/// MassTransit consumer that processes payment requests from the Order Saga.
/// Listens for ProcessPaymentCommand and publishes PaymentCompletedEvent or PaymentFailedEvent.
/// </summary>
public class ProcessPaymentCommandConsumer : IConsumer<ProcessPaymentCommand>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentGateway _paymentGateway;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProcessPaymentCommandConsumer> _logger;

    public ProcessPaymentCommandConsumer(
        IPaymentRepository paymentRepository,
        IPaymentGateway paymentGateway,
        IPublishEndpoint publishEndpoint,
        ILogger<ProcessPaymentCommandConsumer> logger)
    {
        _paymentRepository = paymentRepository;
        _paymentGateway = paymentGateway;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProcessPaymentCommand> context)
    {
        var message = context.Message;
        _logger.LogInformation("💳 Payment processing started for OrderId: {OrderId}, Amount: {Amount}",
            message.OrderId, message.Amount);

        // Check if payment already exists for this order
        var existingPayment = await _paymentRepository.GetByOrderIdAsync(message.OrderId, context.CancellationToken);
        if (existingPayment != null)
        {
            _logger.LogWarning("Payment already exists for OrderId: {OrderId}, Status: {Status}",
                message.OrderId, existingPayment.Status);

            // If already completed, republish the event
            if (existingPayment.Status == PaymentStatus.Completed)
            {
                await _publishEndpoint.Publish(new PaymentCompletedEvent
                {
                    OrderId = message.OrderId
                }, context.CancellationToken);
            }
            return;
        }

        // Create new payment record
        var payment = new Domain.Entities.Payment(
            message.OrderId,
            message.UserId,
            message.Amount
        );

        payment.MarkAsProcessing();
        await _paymentRepository.AddAsync(payment);
        await _paymentRepository.SaveChangesAsync(context.CancellationToken);

        _logger.LogInformation("💳 Payment record created: {PaymentId}", payment.Id);

        try
        {
            // Process payment through gateway (mock)
            var result = await _paymentGateway.ProcessPaymentAsync(
                message.OrderId,
                message.Amount,
                message.UserId,
                context.CancellationToken);

            if (result.IsSuccess)
            {
                payment.Complete(result.TransactionId!);
                await _paymentRepository.UpdateAsync(payment);
                await _paymentRepository.SaveChangesAsync(context.CancellationToken);

                _logger.LogInformation("✅ Payment completed for OrderId: {OrderId}, TransactionId: {TransactionId}",
                    message.OrderId, result.TransactionId);

                // Publish success event to Saga
                await _publishEndpoint.Publish(new PaymentCompletedEvent
                {
                    OrderId = message.OrderId
                }, context.CancellationToken);
            }
            else
            {
                payment.Fail(result.ErrorMessage!);
                await _paymentRepository.UpdateAsync(payment);
                await _paymentRepository.SaveChangesAsync(context.CancellationToken);

                _logger.LogWarning("❌ Payment failed for OrderId: {OrderId}, Error: {Error}",
                    message.OrderId, result.ErrorMessage);

                // Publish failure event to Saga
                await _publishEndpoint.Publish(new PaymentFailedEvent
                {
                    OrderId = message.OrderId,
                    ErrorMessage = result.ErrorMessage!,
                    OrderItems = new List<OrderItemMessage>() // Will be filled by Saga state
                }, context.CancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Unexpected error processing payment for OrderId: {OrderId}", message.OrderId);

            payment.Fail($"Unexpected error: {ex.Message}");
            await _paymentRepository.UpdateAsync(payment);
            await _paymentRepository.SaveChangesAsync(context.CancellationToken);

            await _publishEndpoint.Publish(new PaymentFailedEvent
            {
                OrderId = message.OrderId,
                ErrorMessage = $"System error: {ex.Message}",
                OrderItems = new List<OrderItemMessage>()
            }, context.CancellationToken);
        }
    }
}

/// <summary>
/// ProcessPaymentCommand - defined here to match what Order Saga publishes.
/// This should ideally be in BuildingBlocks.Messaging but we define it here for now.
/// </summary>
public class ProcessPaymentCommand
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public Guid UserId { get; set; }
}
