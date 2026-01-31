using Inventory.Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Inventory.Queries.GetLowStock;

public class GetLowStockHandler : IRequestHandler<GetLowStockQuery, List<GetLowStockResponse>>
{
    private readonly InventoryDbContext _context;

    public GetLowStockHandler(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetLowStockResponse>> Handle(GetLowStockQuery request, CancellationToken cancellationToken)
    {
        var items = await _context.InventoryItems
            .Where(x => x.Stock <= request.Threshold)
            .OrderBy(x => x.Stock)
            .Select(x => new GetLowStockResponse
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Stock = x.Stock
            })
            .ToListAsync(cancellationToken);

        return items;
    }
}
