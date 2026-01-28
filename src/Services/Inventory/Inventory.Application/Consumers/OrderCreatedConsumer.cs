using BuildingBlocks.Messaging.IntegrationEvents;
using Inventory.Infrastructure.Contexts;
using Inventory.Application.Constants;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly InventoryDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(
        InventoryDbContext context,
        IPublishEndpoint publishEndpoint,
        ILogger<OrderCreatedConsumer> logger)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        _logger.LogInformation("OrderCreatedEvent received for OrderId: {OrderId}", context.Message.OrderId);


        var stockResult = true;
        var failureReason = "";

        foreach (var item in context.Message.OrderItems)
        {
            var inventoryItem = await _context.InventoryItems
                .FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

            if (inventoryItem == null || !inventoryItem.HasStock(item.Quantity))
            {
                stockResult = false;
                failureReason = inventoryItem == null
                    ? ValidationMessages.Product.NotFound
                    : ValidationMessages.Product.InsufficientStock
                        .Replace("{AvailableStock}", inventoryItem.Stock.ToString())
                        .Replace("{RequestedQuantity}", item.Quantity.ToString());
                break;
            }
        }

        if (stockResult)
        {
            foreach (var item in context.Message.OrderItems)
            {
                var inventoryItem = await _context.InventoryItems
                    .FirstAsync(x => x.ProductId == item.ProductId);

                inventoryItem.DecreaseStock(item.Quantity);
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Stock reserved successfully for OrderId: {OrderId}", context.Message.OrderId);


            await _publishEndpoint.Publish(new StockReservedEvent(context.Message.OrderId));
        }
        else
        {
            _logger.LogWarning($"Stok işlemi başarısız: {failureReason}");

            await _publishEndpoint.Publish(new StockNotReservedEvent(context.Message.OrderId, failureReason));
        }
    }
}
