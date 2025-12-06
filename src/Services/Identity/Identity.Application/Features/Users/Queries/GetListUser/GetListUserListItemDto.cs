using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Users.Queries.GetListUser;

public class GetListUserListItemDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public bool Status { get; set; }
    public bool IsEmailConfirmed { get; set; }

    public string RegistrationIp { get; set; } = string.Empty;


}
