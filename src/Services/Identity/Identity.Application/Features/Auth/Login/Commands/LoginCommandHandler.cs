using BuildingBlocks.Security.JWT;
using Identity.Application.Features.Auth.Login.Rules;
using Identity.Application.Services;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Identity.Application.Features.Auth.Login.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResponse>
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

    public async Task<LoginCommandResponse> Handle(
    LoginCommand request,
    CancellationToken cancellationToken)
    {
        _logger.Information("🔐 Login attempt for: {EmailOrUsername}", request.EmailOrUsername);

        // 1. Business Rules Validation
        User? user = await _loginBusinessRules
            .UserShouldExistByEmailOrUsername(request.EmailOrUsername);

        _loginBusinessRules.UserEmailShouldBeConfirmed(user);
        _loginBusinessRules.UserAccountShouldBeActive(user);
        _loginBusinessRules.PasswordShouldBeCorrect(
            request.Password,
            user.PasswordHash,
            user.PasswordSalt);

        // 2. Get User Roles
        var roles = user.UserOperationClaims
            .Select(uoc => uoc.OperationClaim.Name)
            .ToList();

        // 3. Create Access Token
        var accessToken = _tokenHelper.CreateToken(
            userId: user.Id,
            email: user.Email,
            userName: user.UserName,
            roles: roles
        );

        // 4. Create Refresh Token Values
        var refreshTokenValue = _tokenHelper.CreateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(
            _configuration.GetValue<int>("TokenOptions:RefreshTokenExpiration", 7)
        );

        // ✅ 5. MULTI-DEVICE SESSION MANAGEMENT
        var activeTokens = await _refreshTokenRepository
            .GetActiveTokensByUserIdAsync(user.Id, cancellationToken);

        // appsettings.json'dan oku (varsayılan: 5)
        int maxSessionCount = _configuration.GetValue<int>("SecuritySettings:MaxActiveSessions", 5);

        if (activeTokens.Count >= maxSessionCount)
        {
            // En eski oturumu iptal et
            var oldestToken = activeTokens
                .OrderBy(t => t.CreatedDate)
                .First();

            oldestToken.Revoke(
                ip: request.IpAddress,
                reason: $"Session limit exceeded (Max {maxSessionCount} devices)",
                replacedByToken: refreshTokenValue
            );

            await _refreshTokenRepository.UpdateAsync(oldestToken);

            _logger.Information(
                "🧹 Session limit ({Limit}) reached. Revoked oldest session for user: {UserId}",
                maxSessionCount,
                user.Id);
        }

        // 6. Create New Refresh Token
        var refreshToken = new RefreshToken(
            userId: user.Id,
            token: refreshTokenValue,
            expiresAt: refreshTokenExpiration,
            createdByIp: request.IpAddress
        );

        await _refreshTokenRepository.AddAsync(refreshToken);
        await _refreshTokenRepository.SaveChangesAsync();

        _logger.Information(
            "✅ User logged in successfully: {UserId} - {Email} (Active sessions: {SessionCount})",
            user.Id,
            user.Email,
            activeTokens.Count + 1);  // +1 yeni token

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