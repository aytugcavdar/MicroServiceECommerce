using BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Events;

public class UserCreatedDomainEvent : IDomainEvent
{
    public Guid UserId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string UserName { get; }
    public string? EmailConfirmationToken { get; }
    public DateTime OccurredOn { get; }

    public UserCreatedDomainEvent(
        Guid userId,
        string email,
        string firstName,
        string lastName,
        string userName,
        string? emailConfirmationToken = null)
    {
        UserId = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        EmailConfirmationToken = emailConfirmationToken;
        OccurredOn = DateTime.UtcNow;
    }
}
