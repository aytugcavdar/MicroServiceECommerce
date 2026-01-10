using BuildingBlocks.Core.Domain;

namespace Catalog.Domain.Events.Product;

public record ProductStockChangedDomainEvent(
    Guid ProductId,
    int OldStock,
    int NewStock
) : IDomainEvent;

