using BuildingBlocks.Core.Domain;

namespace Catalog.Domain.Events.Product;

public record ProductDeletedDomainEvent(
    Guid ProductId,
    string Name
) : IDomainEvent;

