using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Entities;

public class OperationClaim : Entity<Guid>, IAggregateRoot
{
    public string Name { get; set; }

    // Navigation property
    public ICollection<UserOperationClaim> UserOperationClaims { get; set; }

    public OperationClaim()
    {
        Name = string.Empty;
        UserOperationClaims = new HashSet<UserOperationClaim>();
    }

    public OperationClaim(string name) : this()
    {
        Id = Guid.NewGuid();
        Name = name;
        CreatedDate = DateTime.UtcNow;
    }
}
