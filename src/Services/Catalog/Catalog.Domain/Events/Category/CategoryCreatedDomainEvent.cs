using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Events.Catalog;

public record CategoryCreatedDomainEvent(
    Guid CategoryId,
    string Name,
    Guid? ParentCategoryId
) : IDomainEvent;

