namespace Inventory.Application.Features.Inventory.Queries.GetLowStock;

public class GetLowStockResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Stock { get; set; }
}
