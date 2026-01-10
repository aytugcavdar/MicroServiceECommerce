using BuildingBlocks.Core.Domain;

namespace Catalog.Domain.Events.Catalog;

public record CategoryDeletedDomainEvent(
    Guid CategoryId,
    string Name
) : IDomainEvent;
