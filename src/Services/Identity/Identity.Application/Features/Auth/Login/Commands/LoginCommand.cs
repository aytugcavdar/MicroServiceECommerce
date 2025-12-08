using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Identity.Application.Features.Auth.Login.Commands;

public class LoginCommand:IRequest<LoginCommandResponse>
{
    public string EmailOrUsername { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    [JsonIgnore]
    public string IpAddress { get; set; } = string.Empty;
}



