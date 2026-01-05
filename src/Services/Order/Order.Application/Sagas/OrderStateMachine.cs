using BuildingBlocks.Messaging.IntegrationEvents;
using MassTransit;
using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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

        Initially(
            When(OrderCreated)
                .Then(context =>
                {
                    context.Saga.CorrelationId = context.Message.OrderId;
                    context.Saga.UserId = context.Message.BuyerId;
                    context.Saga.CreatedDate = DateTime.UtcNow;
                })
                .TransitionTo(StockReservationPending) 
        );

        During(StockReservationPending,
            When(StockReserved)
                .Then(context =>
                {
                    context.Saga.UpdatedDate = DateTime.UtcNow;
                    Console.WriteLine($"Stok ayrıldı: {context.Saga.CorrelationId}");
                })
                .TransitionTo(PaymentPending),

            When(StockNotReserved)
                .Then(context =>
                {
                    context.Saga.UpdatedDate = DateTime.UtcNow;
                    Console.WriteLine($"Stok ayrılamadı, sipariş iptal: {context.Saga.CorrelationId}");
                })
                .TransitionTo(Failed)
                .Finalize() 
        );

        During(PaymentPending,
            When(PaymentCompleted)
                .Then(context =>
                {
                    context.Saga.UpdatedDate = DateTime.UtcNow;
                    Console.WriteLine($"Ödeme alındı, sipariş tamamlandı: {context.Saga.CorrelationId}");
                })
                .TransitionTo(Completed)
                .Finalize(), 

            When(PaymentFailed)
                .Then(context =>
                {
                    context.Saga.UpdatedDate = DateTime.UtcNow;
                    Console.WriteLine($"Ödeme alınamadı: {context.Saga.CorrelationId}");
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
}
