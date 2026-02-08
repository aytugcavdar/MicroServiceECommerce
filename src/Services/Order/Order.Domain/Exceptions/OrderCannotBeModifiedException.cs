using Order.Domain.Enums;

namespace Order.Domain.Exceptions;

/// <summary>
/// Exception thrown when attempting to modify an order that is no longer in a modifiable state.
/// </summary>
public class OrderCannotBeModifiedException : Exception
{
    public Guid OrderId { get; }
    public OrderStatus CurrentStatus { get; }

    public OrderCannotBeModifiedException(Guid orderId, OrderStatus status)
        : base($"Order {orderId} cannot be modified. Current status: {status}")
    {
        OrderId = orderId;
        CurrentStatus = status;
    }

    public OrderCannotBeModifiedException(Guid orderId, OrderStatus status, string message)
        : base(message)
    {
        OrderId = orderId;
        CurrentStatus = status;
    }
}
