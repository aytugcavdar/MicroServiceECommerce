using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Core.Domain;

public abstract class Entity<TId>:IEntity<TId>
{
    public TId Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

}
