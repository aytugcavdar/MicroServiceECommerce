using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Entities;

public class UserOperationClaim : Entity<Guid>
{
    public Guid UserId { get; set; }
    public Guid OperationClaimId { get; set; }

    // Navigation properties
    public User User { get; set; }
    public OperationClaim OperationClaim { get; set; }

    public UserOperationClaim()
    {
    }

    public UserOperationClaim(Guid userId, Guid operationClaimId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        OperationClaimId = operationClaimId;
        CreatedDate = DateTime.UtcNow;
    }
}
