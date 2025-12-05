using BuildingBlocks.CrossCutting.Exceptions.types;
using Identity.Application.Features.Auth.Register.Rules;
using Identity.Application.Services;
using MediatR;
using Serilog;

namespace Identity.Application.Features.Auth.ConfirmEmail.Commands;

public class ConfirmEmailCommandHandler
    : IRequestHandler<ConfirmEmailCommand, ConfirmEmailCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly AuthBusinessRules _authBusinessRules;
    private readonly Serilog.ILogger _logger;

    public ConfirmEmailCommandHandler(
        IUserRepository userRepository,
        AuthBusinessRules authBusinessRules)
    {
        _userRepository = userRepository;
        _authBusinessRules = authBusinessRules;
        _logger = Log.ForContext<ConfirmEmailCommandHandler>();
    }

    public async Task<ConfirmEmailCommandResponse> Handle(
        ConfirmEmailCommand request,
        CancellationToken cancellationToken)
    {
        _logger.Information(
            "📧 Email confirmation attempt for: {Email}",
            request.Email);

        // ============================================
        // 1. BUSINESS RULES VALIDATION
        // ============================================
        var user = await _authBusinessRules.UserShouldExistWhenSelected(
            request.Email,
            cancellationToken);

        _authBusinessRules.EmailShouldNotBeAlreadyConfirmed(user);

        _authBusinessRules.EmailConfirmationTokenShouldBeValid(
            user,
            request.Token);

        // ============================================
        // 2. CONFIRM EMAIL (Domain Logic)
        // ============================================
        user.ConfirmEmail();  // Parametresiz çağır (token kontrolü zaten yapıldı)

        // ============================================
        // 3. SAVE TO DATABASE
        // ============================================
        await _userRepository.UpdateAsync(user);

        _logger.Information(
            "✅ Email confirmed successfully: {Email}",
            user.Email);

        // ============================================
        // 4. RETURN RESPONSE
        // ============================================
        return new ConfirmEmailCommandResponse
        {
            Success = true,
            Message = "Email confirmed successfully. You can now log in."
        };
    }
}