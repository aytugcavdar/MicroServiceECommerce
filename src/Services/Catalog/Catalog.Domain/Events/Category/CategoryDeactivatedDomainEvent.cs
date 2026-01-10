using BuildingBlocks.Core.Domain;

namespace Catalog.Domain.Events.Catalog;

public record CategoryDeactivatedDomainEvent(
    Guid CategoryId
) : IDomainEvent;
