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
        // Event(() => PaymentCompleted, x => x.CorrelateById(m => m.Message.OrderId)); // İleride eklenecek

        Initially(
            When(OrderCreated)
                .Then(context =>
                {
                    // Saga State verilerini doldur
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
                })
                .TransitionTo(PaymentPending)
                .Finalize() 
        );

        During(StockReservationPending,
            When(StockNotReserved)
                .Then(context =>
                {
   
                    context.Saga.UpdatedDate = DateTime.UtcNow;
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
}
