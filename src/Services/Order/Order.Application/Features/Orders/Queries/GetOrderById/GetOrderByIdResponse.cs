using Order.Application.Features.Orders.Dtos;
using Order.Domain.Enums;

namespace Order.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdResponse
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public OrderStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public AddressDto Address { get; set; } = new();

    public List<OrderItemDto> Items { get; set; } = new();
    public int TotalItems { get; set; }

    public bool CanBeCancelled { get; set; }
    public bool CanBeEdited { get; set; }
}