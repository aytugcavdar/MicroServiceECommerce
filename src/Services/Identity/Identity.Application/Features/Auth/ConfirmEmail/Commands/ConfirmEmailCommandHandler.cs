using BuildingBlocks.CrossCutting.Exceptions.types;
using Identity.Application.Features.Auth.ConfirmEmail.Rules;
using Identity.Application.Features.Users.Rules;
using Identity.Application.Services;
using MediatR;
using Serilog;

namespace Identity.Application.Features.Auth.ConfirmEmail.Commands;

public class ConfirmEmailCommandHandler
    : IRequestHandler<ConfirmEmailCommand, ConfirmEmailCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ConfirmEmailBusinessRules _confirmEmailBusinessRules;
    private readonly UserBusinessRules _userBusinessRules; 
    private readonly Serilog.ILogger _logger;

    public ConfirmEmailCommandHandler(
        IUserRepository userRepository,
        ConfirmEmailBusinessRules confirmEmailBusinessRules,
        UserBusinessRules userBusinessRules) 
    {
        _userRepository = userRepository;
        _confirmEmailBusinessRules = confirmEmailBusinessRules;
        _userBusinessRules = userBusinessRules;
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
        // 1. BUSINESS RULES VALIDATION & RETRIEVAL
        // ============================================

        var user = await _userBusinessRules.UserShouldExist(request.Email);

        _confirmEmailBusinessRules.EmailShouldNotBeAlreadyConfirmed(user);

        _confirmEmailBusinessRules.EmailConfirmationTokenShouldBeValid(
            user,
            request.Token);

        // ============================================
        // 2. CONFIRM EMAIL (Domain Logic)
        // ============================================
        user.ConfirmEmail();

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