using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Domain.Events;

public record OrderCreatedDomainEvent(Guid OrderId) : IDomainEvent;
