using BuildingBlocks.Core.Domain;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Entities;

public class OrderSagaState : Entity<Guid>, SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public int CurrentState { get; set; } 
    public byte[] RowVersion { get; set; } = default!;

    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItemSnapshot> Items { get; set; } = new();

    public Guid? StockReservationTokenId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? FailureReason { get; set; }
    public int RetryCount { get; set; }
}

public class OrderItemSnapshot
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}