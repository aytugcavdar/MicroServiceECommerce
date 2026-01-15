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

        var user = await _userBusinessRules.UserShouldExist(request.Email);

        _confirmEmailBusinessRules.EmailShouldNotBeAlreadyConfirmed(user);
        _confirmEmailBusinessRules.EmailConfirmationTokenShouldBeValid(user, request.Token);
        _confirmEmailBusinessRules.EmailConfirmationTokenShouldNotBeExpired(user); 

        user.ConfirmEmail();

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        _logger.Information(
            "✅ Email confirmed successfully: {Email}",
            user.Email);

        
        return new ConfirmEmailCommandResponse
        {
            Success = true,
            Message = "Email confirmed successfully. You can now log in."
        };
    }
}