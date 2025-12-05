using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Features.Auth.ConfirmEmail.Commands;

public class ResendConfirmationEmailCommandResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
