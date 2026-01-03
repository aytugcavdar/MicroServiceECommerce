using BuildingBlocks.Core.Domain;
using Order.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Entities;

public class Order : Entity<Guid>, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public Address Address { get; private set; } = default!;
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
    }

    public void AddOrderItem(Guid productId, string productName, decimal price, int quantity)
    {
        var orderItem = new OrderItem(productId, productName, price, quantity);
        _orderItems.Add(orderItem);

        // Her ürün eklendiğinde toplam fiyat güncellensin
        CalculateTotalPrice();
    }

    private void CalculateTotalPrice()
    {
        TotalPrice = _orderItems.Sum(x => x.Price * x.Quantity);
    }

    // Sipariş statüsünü güncelleme metodu
    public void UpdateStatus(OrderStatus newStatus)
    {
        Status = newStatus;
    }
}