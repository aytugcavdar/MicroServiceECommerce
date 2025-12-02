using MediatR;

namespace Identity.Application.Features.Auth.ConfirmEmail.Commands;

public class ResendConfirmationEmailCommand : IRequest<ResendConfirmationEmailCommandResponse>
{
    public string Email { get; set; } = string.Empty;
}
