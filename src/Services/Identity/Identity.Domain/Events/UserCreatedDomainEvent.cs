using BuildingBlocks.Core.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Events;

public class UserCreatedDomainEvent : IDomainEvent
{
    public Guid UserId { get; private set; }
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string UserName { get; private set; }
    public string? EmailConfirmationToken { get; private set; }
    public DateTime OccurredOn { get; private set; }

    public UserCreatedDomainEvent()
    {

    }

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
