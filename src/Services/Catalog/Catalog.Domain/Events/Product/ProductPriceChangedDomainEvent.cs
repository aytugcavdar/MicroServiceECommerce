using BuildingBlocks.Core.Domain;

namespace Catalog.Domain.Events.Product;

public record ProductPriceChangedDomainEvent(
    Guid ProductId,
    decimal OldPrice,
    decimal NewPrice
) : IDomainEvent;

