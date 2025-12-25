using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Application.IntegrationEvents;

public class UserCreatedIntegrationEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? EmailConfirmationToken { get; set; }
    public DateTime OccurredOn { get; set; }
}
