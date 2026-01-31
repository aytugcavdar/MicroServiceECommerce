using MediatR;

namespace Inventory.Application.Features.Inventory.Commands.AdjustStock;

public class AdjustStockCommand : IRequest<AdjustStockCommandResponse>
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string? Reason { get; set; }
}
