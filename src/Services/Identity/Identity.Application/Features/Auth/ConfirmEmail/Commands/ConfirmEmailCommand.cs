using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.ConfirmEmail.Commands;

public class ConfirmEmailCommand : IRequest<ConfirmEmailCommandResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
