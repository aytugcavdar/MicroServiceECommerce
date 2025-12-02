using BuildingBlocks.CrossCutting.Exceptions.types;
using Identity.Application.Services;
using MediatR;

namespace Identity.Application.Features.Auth.ConfirmEmail.Commands;

// ============================================
// 6. ResendConfirmationEmailCommandHandler.cs
// ============================================


public class ResendConfirmationEmailCommandHandler
    : IRequestHandler<ResendConfirmationEmailCommand, ResendConfirmationEmailCommandResponse>
{
    private readonly IUserRepository _userRepository;

    public ResendConfirmationEmailCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ResendConfirmationEmailCommandResponse> Handle(
        ResendConfirmationEmailCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(
            u => u.Email == request.Email,
            cancellationToken: cancellationToken);

        if (user == null)
            throw new BusinessException("User not found");

        if (user.IsEmailConfirmed)
            throw new BusinessException("Email is already confirmed");

        // Yeni token oluştur
        string newToken = Guid.NewGuid().ToString();
        user.EmailConfirmationToken = newToken;
        // user.EmailConfirmationTokenExpiration = DateTime.UtcNow.AddHours(24);

        await _userRepository.UpdateAsync(user);

        // TODO: Email gönder
        // await _emailService.SendEmailConfirmationAsync(user.Email, newToken);

        return new ResendConfirmationEmailCommandResponse
        {
            Success = true,
            Message = "Confirmation email has been resent. Please check your inbox."
        };
    }
}
