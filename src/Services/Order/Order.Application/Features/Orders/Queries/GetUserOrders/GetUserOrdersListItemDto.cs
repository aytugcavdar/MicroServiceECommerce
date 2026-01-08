using Order.Domain.Enums;

namespace Order.Application.Features.Orders.Queries.GetUserOrders;

public class GetUserOrdersListItemDto
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public DateTime CreatedDate { get; set; }

    public int ItemCount { get; set; }

    public string City { get; set; } = string.Empty;
}
