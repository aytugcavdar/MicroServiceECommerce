using BuildingBlocks.Security.JWT;

namespace Identity.Application.Features.Auth.Login.Commands;

public class LoginCommandResponse
{
    public AccessToken AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiration { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}



