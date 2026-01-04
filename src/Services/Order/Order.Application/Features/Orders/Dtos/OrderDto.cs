namespace Order.Application.Features.Orders.Dtos;

public class OrderDto
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Status { get; set; } // Veya Enum olarak tutmak isterseniz OrderStatus
    public AddressDto Address { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
}