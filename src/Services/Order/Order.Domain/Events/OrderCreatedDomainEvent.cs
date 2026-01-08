using BuildingBlocks.Core.Domain;
using Order.Domain.Entities;
using Order.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Events;

public record OrderCreatedDomainEvent(
    Guid OrderId,
    Guid UserId,
    decimal TotalPrice,
    IReadOnlyList<OrderItem> Items
) : IDomainEvent;

public record OrderStatusChangedDomainEvent(
    Guid OrderId,
    OrderStatus OldStatus,
    OrderStatus NewStatus,
    string? Reason = null
) : IDomainEvent;

public record OrderCompletedDomainEvent(
    Guid OrderId,
    Guid UserId,
    DateTime CompletedAt
) : IDomainEvent;

public record OrderCancelledDomainEvent(
    Guid OrderId,
    string Reason
) : IDomainEvent;
