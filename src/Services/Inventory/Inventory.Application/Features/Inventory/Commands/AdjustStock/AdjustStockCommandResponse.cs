namespace Inventory.Application.Features.Inventory.Commands.AdjustStock;

public class AdjustStockCommandResponse
{
    public Guid ProductId { get; set; }
    public int PreviousStock { get; set; }
    public int NewStock { get; set; }
    public int AdjustedQuantity { get; set; }
    public DateTime AdjustedAt { get; set; }
}
