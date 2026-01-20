using BuildingBlocks.Messaging.IntegrationEvents;
using Inventory.Infrastructure.Contexts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.Consumers;

public class ReleaseStockConsumer : IConsumer<ReleaseStockCommand>
{
    private readonly InventoryDbContext _context;
    private readonly ILogger<ReleaseStockConsumer> _logger;

    public ReleaseStockConsumer(
        InventoryDbContext context,
        ILogger<ReleaseStockConsumer> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ReleaseStockCommand> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "🔄 ReleaseStockCommand received for OrderId: {OrderId}",
            message.OrderId);

        try
        {
            if (message.Items == null || !message.Items.Any())
            {
                _logger.LogWarning(
                    "⚠️ ReleaseStock: Items list is empty for OrderId: {OrderId}. Cannot release stock.",
                    message.OrderId);
                return;
            }

            foreach (var item in message.Items)
            {
                var inventoryItem = await _context.InventoryItems
                    .FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                if (inventoryItem == null)
                {
                    _logger.LogWarning(
                        "⚠️ Product not found in inventory: ProductId={ProductId}",
                        item.ProductId);
                    continue;
                }

                inventoryItem.Stock += item.Quantity;

                _logger.LogInformation(
                    "✅ Stock released: ProductId={ProductId}, Quantity={Quantity}, NewStock={NewStock}",
                    item.ProductId,
                    item.Quantity,
                    inventoryItem.Stock);
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "✅ Stock successfully released for OrderId: {OrderId}",
                message.OrderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "❌ Error releasing stock for OrderId: {OrderId}",
                message.OrderId);

            throw;
        }
    }
}