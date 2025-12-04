using BuildingBlocks.Core.Domain;

namespace Identity.Domain.Events;

/// <summary>
/// Kullanıcı email'i doğruladığında fırlatılan domain event
/// </summary>
public class UserEmailConfirmedDomainEvent : IDomainEvent
{
    public Guid UserId { get; }
    public string Email { get; }
    public DateTime OccurredOn { get; }

    public UserEmailConfirmedDomainEvent(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
        OccurredOn = DateTime.UtcNow;
    }
}
