namespace Inventory.Application.Features.Inventory.Queries.GetInventoryByProductId;

public class GetInventoryByProductIdResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Stock { get; set; }
    public bool IsInStock => Stock > 0;
    public bool IsLowStock => Stock > 0 && Stock < 10;
}
