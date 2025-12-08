using BuildingBlocks.Security.JWT;
using Identity.Application.Features.Auth.Login.Rules;
using Identity.Application.Services;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Identity.Application.Features.Auth.Login.Commands;

public class LoginCommandHandler :IRequestHandler<LoginCommand,LoginCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenHelper _tokenHelper;
    private readonly LoginBusinessRules _loginBusinessRules;
    private readonly IConfiguration _configuration;
    private readonly Serilog.ILogger _logger;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenHelper tokenHelper,
        LoginBusinessRules loginBusinessRules,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenHelper = tokenHelper;
        _loginBusinessRules = loginBusinessRules;
        _configuration = configuration;
        _logger = Log.ForContext<LoginCommandHandler>();
    }

    public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.Information(
            "🔐 Login attempt for: {EmailOrUsername}", request.EmailOrUsername
            );

        User? user = await _loginBusinessRules
            .UserShouldExistByEmailOrUsername(request.EmailOrUsername);

        _loginBusinessRules.UserEmailShouldBeConfirmed(user);

        _loginBusinessRules.UserAccountShouldBeActive(user);

        _loginBusinessRules.PasswordShouldBeCorrect(
            request.Password,
            user.PasswordHash,
            user.PasswordSalt);

        var roles = user.UserOperationClaims
            .Select(uoc => uoc.OperationClaim.Name)
            .ToList();

        var accessToken = _tokenHelper.CreateToken(
            userId: user.Id,
            email: user.Email,
            userName: user.UserName,
            roles: roles
        );
        var refreshTokenValue = _tokenHelper.CreateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(
            _configuration.GetValue<int>("TokenOptions:RefreshTokenExpiration", 7)
        );

        var refreshToken = new RefreshToken(
            userId: user.Id,
            token: refreshTokenValue,
            expiresAt: refreshTokenExpiration,
            createdByIp: request.IpAddress
        );

        await _refreshTokenRepository.AddAsync(refreshToken);

        _logger.Information(
            "✅ User logged in successfully: {UserId} - {Email}",
            user.Id,
            user.Email);
        return new LoginCommandResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            RefreshTokenExpiration = refreshTokenExpiration,
            UserId = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Roles = roles
        };
    }
}



