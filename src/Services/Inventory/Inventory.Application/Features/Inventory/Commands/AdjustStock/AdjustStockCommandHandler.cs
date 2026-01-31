using Inventory.Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Inventory.Commands.AdjustStock;

public class AdjustStockCommandHandler : IRequestHandler<AdjustStockCommand, AdjustStockCommandResponse>
{
    private readonly InventoryDbContext _context;

    public AdjustStockCommandHandler(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<AdjustStockCommandResponse> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.InventoryItems
            .FirstOrDefaultAsync(x => x.ProductId == request.ProductId, cancellationToken);

        if (item == null)
        {
            throw new KeyNotFoundException($"ProductId '{request.ProductId}' için stok kaydı bulunamadı.");
        }

        var previousStock = item.Stock;
        var newStock = previousStock + request.Quantity;

        if (newStock < 0)
        {
            throw new InvalidOperationException($"Yetersiz stok. Mevcut: {previousStock}, İstenen azaltma: {Math.Abs(request.Quantity)}");
        }

        item.Stock = newStock;
        item.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new AdjustStockCommandResponse
        {
            ProductId = request.ProductId,
            PreviousStock = previousStock,
            NewStock = newStock,
            AdjustedQuantity = request.Quantity,
            AdjustedAt = DateTime.UtcNow
        };
    }
}
