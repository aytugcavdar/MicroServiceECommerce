using BuildingBlocks.Core.Domain;
using Order.Domain.Enums;
using Order.Domain.Events;
using Order.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Entities;

public class Order : Entity<Guid>, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public Address Address { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalPrice { get; private set; }
    public DateTime CreatedDate { get; private set; }

    // Dışarıdan doğrudan koleksiyona müdahale edilemesin (Encapsulation)
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public Order()
    {
        // EF Core
    }

    public Order(Guid userId, Address address)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Address = address;
        Status = OrderStatus.Submitted;
        CreatedDate = DateTime.UtcNow;

        AddDomainEvent(new OrderCreatedDomainEvent(
            Id,
            userId,
            0, 
            Array.Empty<OrderItem>()
        ));
    }

    public void AddOrderItem(Guid productId, string productName, decimal price, int quantity)
    {
        var orderItem = new OrderItem(productId, productName, price, quantity);
        _orderItems.Add(orderItem);

        
        CalculateTotalPrice();
    }

    private void CalculateTotalPrice()
    {
        TotalPrice = _orderItems.Sum(x => x.Price * x.Quantity);
    }
    public void UpdateAddress(Address newAddress)
    {
        
        if (Status != OrderStatus.Submitted)
        {
            throw new OrderCannotBeModifiedException(Id, Status, "Address can only be updated for submitted orders");
        }

        Address = newAddress;
    }

    public void UpdateStatus(OrderStatus newStatus, string? reason = null)
    {
        var oldStatus = Status;
        Status = newStatus;

        AddDomainEvent(new OrderStatusChangedDomainEvent(
            Id,
            oldStatus,
            newStatus,
            reason
        ));

        if (newStatus == OrderStatus.Completed)
        {
            AddDomainEvent(new OrderCompletedDomainEvent(Id, UserId, DateTime.UtcNow));
        }
        else if (newStatus == OrderStatus.Canceled || newStatus == OrderStatus.Failed)
        {
            AddDomainEvent(new OrderCancelledDomainEvent(Id, reason ?? "Unknown"));
        }
    }
}