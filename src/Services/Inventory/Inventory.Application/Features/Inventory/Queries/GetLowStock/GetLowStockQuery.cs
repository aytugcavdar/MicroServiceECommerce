using MediatR;

namespace Inventory.Application.Features.Inventory.Queries.GetLowStock;

public class GetLowStockQuery : IRequest<List<GetLowStockResponse>>
{
    public int Threshold { get; set; } = 10;
}
