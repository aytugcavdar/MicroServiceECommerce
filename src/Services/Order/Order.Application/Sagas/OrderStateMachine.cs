using BuildingBlocks.Messaging.IntegrationEvents;
using MassTransit;
using Order.Domain.Entities;

namespace Order.Application.Sagas;

public class OrderStateMachine : MassTransitStateMachine<OrderSagaState>
{
    public OrderStateMachine()
    {
        InstanceState(x => x.CurrentState);
        Event(() => OrderCreated, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockReserved, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => StockNotReserved, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => PaymentCompleted, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => PaymentFailed, x => x.CorrelateById(m => m.Message.OrderId));

        Schedule(() => StockReservationTimeout, x => x.StockReservationTokenId, s =>
        {
            s.Delay = TimeSpan.FromMinutes(5);
            s.Received = r => r.CorrelateById(context => context.Message.OrderId);
        });

        Initially(
            When(OrderCreated)
                .Then(context =>
                {
                    context.Saga.CorrelationId = context.Message.OrderId;
                    context.Saga.UserId = context.Message.BuyerId;
                    context.Saga.TotalPrice = context.Message.TotalPrice;
                    context.Saga.CreatedDate = DateTime.UtcNow;

                    context.Saga.Items = context.Message.OrderItems
                        .Select(x => new OrderItemSnapshot
                        {
                            ProductId = x.ProductId,
                            Quantity = x.Quantity
                        })
                        .ToList();

                    Console.WriteLine($"🎯 Saga başlatıldı: OrderId={context.Message.OrderId}");
                })
                .Schedule(StockReservationTimeout,
                    context => new StockReservationTimeoutEvent { OrderId = context.Saga.CorrelationId })
                .TransitionTo(StockReservationPending)
        );

        During(StockReservationPending,
            When(StockReserved)
                .Then(context =>
                {
                    Console.WriteLine($"✅ Stok rezerve edildi: {context.Saga.CorrelationId}");
                })
                .Unschedule(StockReservationTimeout)
                .TransitionTo(PaymentPending)
                .Publish(context => new ProcessPaymentCommand
                {
                    OrderId = context.Saga.CorrelationId,
                    Amount = context.Saga.TotalPrice,
                    UserId = context.Saga.UserId
                }),

            When(StockNotReserved)
                .Then(context =>
                {
                    context.Saga.FailureReason = context.Message.Reason;
                    Console.WriteLine($"❌ Stok yetersiz: {context.Message.Reason}");
                })
                .Unschedule(StockReservationTimeout)
                .TransitionTo(Failed)
                .Finalize(),

            When(StockReservationTimeout.Received)
                .Then(context =>
                {
                    context.Saga.FailureReason = "Stock reservation timeout";
                    Console.WriteLine($"⏱️ Stok rezervasyonu zaman aşımı: {context.Saga.CorrelationId}");
                })
                .TransitionTo(Failed)
                .Publish(context => new ReleaseStockCommand
                {
                    OrderId = context.Saga.CorrelationId,
                    Items = context.Saga.Items.Select(x => new OrderItemMessage
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity
                    }).ToList()
                })
                .Finalize()
        );

        During(PaymentPending,
            When(PaymentCompleted)
                .Then(context =>
                {
                    context.Saga.CompletedDate = DateTime.UtcNow;
                    Console.WriteLine($"💰 Ödeme alındı, sipariş tamamlandı: {context.Saga.CorrelationId}");
                })
                .TransitionTo(Completed)
                .Publish(context => new OrderCompletedIntegrationEvent
                {
                    OrderId = context.Saga.CorrelationId,
                    UserId = context.Saga.UserId,
                    TotalPrice = context.Saga.TotalPrice
                })
                .Finalize(),

            When(PaymentFailed)
                .Then(context =>
                {
                    context.Saga.FailureReason = context.Message.ErrorMessage;
                    context.Saga.RetryCount++;
                    Console.WriteLine($"💳 Ödeme başarısız: {context.Message.ErrorMessage}");
                })
                .Publish(context => new ReleaseStockCommand
                {
                    OrderId = context.Saga.CorrelationId,
                    Items = context.Message.OrderItems.Select(x => new OrderItemMessage
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity
                    }).ToList()
                })
                .TransitionTo(Failed)
                .Finalize()
        );
    }

    public State StockReservationPending { get; private set; }
    public State PaymentPending { get; private set; }
    public State Failed { get; private set; }
    public State Completed { get; private set; }

    public Event<OrderCreatedEvent> OrderCreated { get; private set; }
    public Event<StockReservedEvent> StockReserved { get; private set; }
    public Event<StockNotReservedEvent> StockNotReserved { get; private set; }
    public Event<PaymentCompletedEvent> PaymentCompleted { get; private set; }
    public Event<PaymentFailedEvent> PaymentFailed { get; private set; }

    public Schedule<OrderSagaState, StockReservationTimeoutEvent> StockReservationTimeout { get; private set; }
}

public class StockReservationTimeoutEvent
{
    public Guid OrderId { get; set; }
}

public class ProcessPaymentCommand
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public Guid UserId { get; set; }
}

public class OrderCompletedIntegrationEvent
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
}