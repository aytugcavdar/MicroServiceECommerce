namespace Identity.Application.Features.Auth.ConfirmEmail.Commands;

public class ConfirmEmailCommandResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
