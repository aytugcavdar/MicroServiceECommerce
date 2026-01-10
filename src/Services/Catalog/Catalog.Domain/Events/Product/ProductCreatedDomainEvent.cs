using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Events.Product;

public record ProductCreatedDomainEvent(
    Guid ProductId,
    string Name,
    Guid CategoryId,
    decimal Price,
    int Stock
) : IDomainEvent;

