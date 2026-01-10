using BuildingBlocks.Core.Domain;

namespace Catalog.Domain.Events.Catalog;

public record CategoryActivatedDomainEvent(
    Guid CategoryId
) : IDomainEvent;
