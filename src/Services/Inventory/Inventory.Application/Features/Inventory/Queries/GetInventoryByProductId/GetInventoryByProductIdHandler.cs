using Inventory.Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Inventory.Queries.GetInventoryByProductId;

public class GetInventoryByProductIdHandler : IRequestHandler<GetInventoryByProductIdQuery, GetInventoryByProductIdResponse>
{
    private readonly InventoryDbContext _context;

    public GetInventoryByProductIdHandler(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<GetInventoryByProductIdResponse> Handle(GetInventoryByProductIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _context.InventoryItems
            .FirstOrDefaultAsync(x => x.ProductId == request.ProductId, cancellationToken);

        if (item == null)
        {
            throw new KeyNotFoundException($"ProductId '{request.ProductId}' için stok kaydı bulunamadı.");
        }

        return new GetInventoryByProductIdResponse
        {
            Id = item.Id,
            ProductId = item.ProductId,
            Stock = item.Stock
        };
    }
}
